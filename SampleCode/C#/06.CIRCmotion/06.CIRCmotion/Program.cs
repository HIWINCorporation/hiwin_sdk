using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace _06_CIRCmotion {
    class Program {
        private static SDKHrobot.HRobot.CallBackFun callback = new SDKHrobot.HRobot.CallBackFun( EventFun );
        static void Main( string[] args ) {
            int device_id = SDKHrobot.HRobot.open_connection( "127.0.0.1", 1, callback );
            if ( device_id >= 0 ) {
                CIRCMotion( device_id, callback );
                SDKHrobot.HRobot.disconnect( device_id );
            }
        }
        public static void CIRCMotion( int device_id, SDKHrobot.HRobot.CallBackFun callback ) {
            if ( device_id >= 0 ) {
                Console.WriteLine( "connect successful." );
                callback = new SDKHrobot.HRobot.CallBackFun( EventFun );
            } else {
                Console.WriteLine( "connect failure." );
                return;
            }

            if ( SDKHrobot.HRobot.get_motor_state( device_id ) == 0 ) {
                SDKHrobot.HRobot.set_motor_state( device_id, 1 ); // Servo on
                SDKHrobot.HRobot.set_override_ratio( device_id, 50 );
                Thread.Sleep( 3000 );
            }

            double[] cp1 = { 100, 368, 200, 180, 0, 90 };
            double[] cp2 = { 0, 368, 100, 180, 0, 90 };
            double[] cp3 = { -100, 368, 0, 180, 0, 90 };
            double[] cp4 = { 0, 368, -100, 180, 0, 90 };
            double[] cp5 = { 100, 368, 0, 180, 0, 90 };
            double[] cp6 = { 0, 368, 100, 180, 0, 90 };
            double[] cp7 = { -100, 368, 200, 180, 0, 90 };
            double[] cp8 = { 0, 368, 293.5, 180, 0, 90 };
            double[] Home = { 0, 0, 0, 0, -90, 0 };

            SDKHrobot.HRobot.set_override_ratio( device_id, 60 ); //override speed ratio
            SDKHrobot.HRobot.ptp_axis( device_id, 0, Home ); //ptp to axis home

            SDKHrobot.HRobot.circ_pos( device_id, 1, cp1, cp2 ); //circ motion
            SDKHrobot.HRobot.circ_pos( device_id, 1, cp3, cp4 );
            Thread.Sleep( 2000 );
            SDKHrobot.HRobot.circ_axis( device_id, 1, cp5, cp6 );
            SDKHrobot.HRobot.circ_axis( device_id, 1, cp7, cp8 );
            Thread.Sleep( 2000 );
            //SDKHrobot.HRobot.circ_pr( device_id, 1, 1, 2 );

        }

        public static void EventFun( UInt16 cmd, UInt16 rlt, ref UInt16 Msg, int len ) {
            Console.WriteLine( "Command: " + cmd + " Resault: " + rlt );
        }
    }
}