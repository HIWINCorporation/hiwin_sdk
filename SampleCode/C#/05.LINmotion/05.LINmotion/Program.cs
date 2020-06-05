using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace _05_LINmotion {
    class Program {
        private static SDKHrobot.HRobot.CallBackFun callback = new SDKHrobot.HRobot.CallBackFun( EventFun );
        static void Main( string[] args ) {
            int device_id = SDKHrobot.HRobot.open_connection( "127.0.0.1", 1, callback );
            if ( device_id >= 0 ) {
                LINMotion( device_id, callback );
                SDKHrobot.HRobot.disconnect( device_id );
            }
        }
        public static void LINMotion( int device_id, SDKHrobot.HRobot.CallBackFun callback ) {
            if ( device_id >= 0 ) {
                Console.WriteLine( "connect successful." );
                callback = new SDKHrobot.HRobot.CallBackFun( EventFun );
                SDKHrobot.HRobot.set_override_ratio( device_id, 100 );
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

            double[] pos = { 0, 450, 200, 180, 0, 90 };
            int xoffset = 20;
            int zoffset = 10;

            //make path
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

            //line motion
            for ( int a = 0; a < x.Count; a++ ) {
                pos[0] = x[a];
                pos[1] = y[a];
                pos[2] = z[a];
                while ( SDKHrobot.HRobot.get_command_count( device_id ) > 100 ) {
                    Thread.Sleep( 500 );
                }
                SDKHrobot.HRobot.lin_pos( device_id, 2, 50, pos );
            }
            Thread.Sleep( 1000 );
            //axis
            double[] pos_home = { 0, 0, 0, 0, -90, 0 };
            SDKHrobot.HRobot.lin_axis( device_id, 2, 50, pos_home );

            //rel_pos
            Thread.Sleep( 500 );
            double[] pos1 = { 10, 0, 0, 0, 0, 0 };
            SDKHrobot.HRobot.lin_rel_pos( device_id, 2, 50, pos1 );
            //rel_axis
            Thread.Sleep( 500 );
            double[] pos2 = { 10, 10, -10, 10, -10, 10 };
            SDKHrobot.HRobot.lin_rel_axis( device_id, 2, 50, pos2 );

            Thread.Sleep( 500 );
            SDKHrobot.HRobot.lin_pr( device_id, 2, 50, 1 );
        }

        public static void EventFun( UInt16 cmd, UInt16 rlt, ref UInt16 Msg, int len ) {
            Console.WriteLine( "Command: " + cmd + " Resault: " + rlt );
        }
    }
}