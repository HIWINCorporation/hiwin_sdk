using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SDKHrobot;

namespace _09.SyncOutput {
    class Program {
        private static HRobot.CallBackFun callback = new HRobot.CallBackFun( EventFun );
        static void Main( string[] args ) {
            int device_id = HRobot.open_connection( "127.0.0.1", 1, callback );
            HRobot.set_motor_state( device_id, 1 );
            if ( device_id >= 0 ) {
                SyncOutput( device_id, callback );
                SDKHrobot.HRobot.disconnect( device_id );
            }
        }

        public static void SyncOutput( int device_id, HRobot.CallBackFun callback ) {
            double[] p1 = { -10, -200, 0, 0, 0, 0 };
            double[] p2 = { 10, 150, 0, 0, 0, 0 };
            int type = 0;  //DO
            int id = 1;
            int ON = 1;
            int OFF = 0;
            int delay = 1000;
            int distance = 50;
            int Start = 0;
            int End = 1;
            int Path = 2;
            HRobot.jog_home( device_id );
            Thread.Sleep( 100 );
            HRobot.lin_rel_pos( device_id, 0, 0, p1 );
            HRobot.SyncOutput( device_id, type, id, ON, Start, delay, distance );
            HRobot.SyncOutput( device_id, type, 2, ON, Path, -1000, distance );
            HRobot.SyncOutput( device_id, type, 3, ON, Path, 0, distance );
            HRobot.SyncOutput( device_id, type, 4, ON, Path, delay, distance );
            HRobot.SyncOutput( device_id, type, 5, ON, Path, -1000, -50 );
            HRobot.SyncOutput( device_id, type, 6, ON, Path, 0, -50 );
            HRobot.SyncOutput( device_id, type, 7, ON, Path, 1000, -50 );
            HRobot.SyncOutput( device_id, type, 8, ON, End, -1000, distance );
            HRobot.lin_rel_pos( device_id, 0, 0, p2 );
            Console.ReadKey();
        }

        public static void EventFun( UInt16 cmd, UInt16 rlt, ref UInt16 Msg, int len ) {
            Console.WriteLine( Msg );
        }
    }
}
