using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace _04_PTPmotion {
    class Program {
        private static SDKHrobot.HRobot.CallBackFun callback = new SDKHrobot.HRobot.CallBackFun( EventFun );
        static void Main( string[] args ) {
            int device_id = SDKHrobot.HRobot.open_connection( "127.0.0.1", 1, callback );
            if ( device_id >= 0 ) {
                PTPMotion( device_id, callback );
                SDKHrobot.HRobot.disconnect( device_id );
            }
        }
        public static void PTPMotion( int device_id, SDKHrobot.HRobot.CallBackFun callback ) {
            if ( device_id >= 0 ) {
                Console.WriteLine( "connect successful." );
                callback = new SDKHrobot.HRobot.CallBackFun( EventFun );
                SDKHrobot.HRobot.set_override_ratio( device_id, 60 );
            } else {
                Console.WriteLine( "connect failure." );
                return;
            }

            if ( SDKHrobot.HRobot.get_motor_state( device_id ) == 0 ) {
                SDKHrobot.HRobot.set_motor_state( device_id, 1 ); // Servo on
                SDKHrobot.HRobot.set_override_ratio( device_id, 50 );
                Thread.Sleep( 3000 );
            }


            List<double> x = new List<double>();
            List<double> y = new List<double>();
            List<double> z = new List<double>();

            double[] pos = { 0, 300, -100, 0, 0, 0 };
            int xoffset = 20;
            int zoffset = 10;

            x.Add( pos[0] );
            y.Add( pos[1] );
            z.Add( pos[2] );

            x.Add( pos[0] + xoffset );
            y.Add( pos[1] );
            z.Add( pos[2] );

            x.Add( pos[0] + xoffset );
            y.Add( pos[1] );
            z.Add( pos[2] - zoffset );

            x.Add( pos[0] + xoffset );
            y.Add( pos[1] );
            z.Add( pos[2] );

            x.Add( pos[0] - xoffset );
            y.Add( pos[1] );
            z.Add( pos[2] );

            x.Add( pos[0] - xoffset );
            y.Add( pos[1] );
            z.Add( pos[2] - zoffset );

            x.Add( pos[0] - xoffset );
            y.Add( pos[1] );
            z.Add( pos[2] );

            x.Add( pos[0] );
            y.Add( pos[1] );
            z.Add( pos[2] );

            //ptp motion
            for ( int a = 0; a < x.Count; a++ ) {
                pos[0] = x[a];
                pos[1] = y[a];
                pos[2] = z[a];
                while ( SDKHrobot.HRobot.get_command_count( device_id ) > 100 ) {
                    Thread.Sleep( 500 );
                }
                SDKHrobot.HRobot.ptp_pos( device_id, 1, pos );
            }
            Thread.Sleep( 5000 );
            //axis
            for ( int a = 0; a < x.Count; a++ ) {
                pos[0] = x[a];
                pos[1] = y[a] - 300;
                pos[2] = z[a];
                while ( SDKHrobot.HRobot.get_command_count( device_id ) > 100 ) {
                    Thread.Sleep( 500 );
                }
                SDKHrobot.HRobot.ptp_axis( device_id, 1, pos );
            }
            Thread.Sleep( 5000 );
            pos[1] = 0;
            //rel_pos
            SDKHrobot.HRobot.ptp_rel_pos( device_id, 1, pos );
            Thread.Sleep( 500 );
            //rel_axis
            SDKHrobot.HRobot.ptp_rel_axis( device_id, 1, pos );
            Thread.Sleep( 500 );
            SDKHrobot.HRobot.ptp_pr( device_id, 1, 1 );
        }

        public static void EventFun( UInt16 cmd, UInt16 rlt, ref UInt16 Msg, int len ) {
            Console.WriteLine( "Command: " + cmd + " Resault: " + rlt );
        }
    }
}
