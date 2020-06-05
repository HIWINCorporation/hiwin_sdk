using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDKHrobot;
using System.Threading;

namespace _11.SoftLimit {
    class Program {
        static int device_id = -1;
        private static HRobot.CallBackFun callback = new HRobot.CallBackFun( EventFun );
        static void Main( string[] args ) {
            device_id = HRobot.open_connection( "127.0.0.1", 1, callback );
            Thread.Sleep( 1000 );
            StringBuilder sdk_version = new StringBuilder();
            HRobot.get_hrsdk_version( sdk_version );
            Console.WriteLine( "sdk version: " + sdk_version );
            if ( device_id >= 0 ) {
                Console.WriteLine( "connect successful." );
                HRobot.set_motor_state( device_id, 1 );
                SoftLimitExample( device_id );
                Console.ReadLine();
                HRobot.disconnect( device_id );
            } else {
                Console.WriteLine( "connect failure." );
            }
        }

        public static void EventFun( UInt16 cmd, UInt16 rlt, ref UInt16 Msg, int len ) {
            String info_p = "";
            unsafe {
                fixed ( UInt16* p = &Msg ) {
                    for ( int i = 0; i < len; i++ ) {
                        info_p += ( char )p[i];
                    }
                }
            }
            if ( cmd == 0 && rlt == 4030 && len > 1 ) {
                Console.WriteLine( "[ALARM NOTIFY]: " + info_p );
            }
        }

        public static void wait_for_stop( int device_id ) {
            while ( HRobot.get_motion_state( device_id ) != 1 && HRobot.get_connection_level( device_id ) != -1 ) {
                Thread.Sleep( 30 );
            }
        }

        public static void SoftLimitExample( int device_id ) {
            double[] joint_low_limit = { -20, -20, -35, -20, 0, 0 };
            double[] joint_high_limit = { 20, 20, 0, 0, 0, 0 };
            double[] cart_low_limit = { -100, 300, -100, 0, 0, 0 };
            double[] cart_high_limit = { 100, 450, -25, 0, 0, 0 };
            double[] cart_home = { 0, 400, 0, 0, -90, 0 };
            double[] joint_home = { 0, 0, 0, 0, -90, 0 };
            double[] now_pos = { 0, 0, 0, 0, 0, 0 };
            bool re_bool = false;
            HRobot.get_current_position( device_id, now_pos );

            // run joint softlimit
            HRobot.set_override_ratio( device_id, 100 );
            HRobot.set_joint_soft_limit( device_id, joint_low_limit, joint_high_limit );
            HRobot.enable_joint_soft_limit( device_id, true );
            HRobot.enable_cart_soft_limit( device_id, false );
            HRobot.get_joint_soft_limit_config( device_id, ref re_bool, joint_low_limit, joint_high_limit );
            Console.WriteLine( "Enable Joint SoftLimit: " + re_bool );
            HRobot.jog_home( device_id );
            wait_for_stop( device_id );
            Thread.Sleep( 1000 );
            for ( int i = 0; i < 4; i++ ) {
                HRobot.jog( device_id, 1, i, -1 );
                wait_for_stop( device_id );
                Console.WriteLine( "On the limits of SoftLimit" );
            }
            for ( int i = 0; i < 4; i++ ) {
                HRobot.jog( device_id, 1, i, 1 );
                wait_for_stop( device_id );
                Console.WriteLine( "On the limits of SoftLimit" );
            }
            HRobot.enable_joint_soft_limit( device_id, false );

            // run cartesian softlimit
            HRobot.ptp_axis( device_id, 0, joint_home );
            wait_for_stop( device_id );
            HRobot.set_cart_soft_limit( device_id, cart_low_limit, cart_high_limit );
            HRobot.enable_cart_soft_limit( device_id, true );
            HRobot.get_cart_soft_limit_config( device_id, ref re_bool, cart_low_limit, cart_high_limit );
            Console.WriteLine( "Enable Cart SoftLimit: " + re_bool );
            HRobot.lin_pos( device_id, 0, 0, cart_home );
            wait_for_stop( device_id );
            for ( int i = 0; i < 3; i++ ) {
                HRobot.jog( device_id, 0, i, -1 );
                wait_for_stop( device_id );
                Console.WriteLine( "On the limits of SoftLimit" );
                Console.WriteLine( "" );
                HRobot.clear_alarm( device_id );
                Thread.Sleep( 2000 );
            }
            for ( int i = 0; i < 3; i++ ) {
                HRobot.jog( device_id, 0, i, 1 );
                wait_for_stop( device_id );
                Console.WriteLine( "On the limits of SoftLimit" );
                Console.WriteLine( "" );
                HRobot.clear_alarm( device_id );
                Thread.Sleep( 2000 );
            }
            Console.WriteLine( "End of motion" );

            HRobot.enable_joint_soft_limit( device_id, false );
            HRobot.enable_cart_soft_limit( device_id, false );
        }
    }
}
