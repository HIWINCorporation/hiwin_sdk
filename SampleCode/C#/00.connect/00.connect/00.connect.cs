using SDKHrobot;
using System;
using System.Collections.Generic;
using System.Text;

namespace _00_connect {
    class Program {
        public static void Connect( int device_id, SDKHrobot.HRobot.CallBackFun callback ) {
            device_id = HRobot.open_connection( "127.0.0.1", 1, callback );
            if ( device_id >= 0 ) {
                Console.WriteLine( "connect successful." );
                StringBuilder v = new StringBuilder( 100 );
                SDKHrobot.HRobot.get_hrsdk_version( v );
                Console.WriteLine( "hrsdk_version:" + v );

                int level = SDKHrobot.HRobot.get_connection_level( device_id );
                Console.WriteLine( "level:" + level );

                SDKHrobot.HRobot.set_connection_level( device_id, 1 );
                level = SDKHrobot.HRobot.get_connection_level( device_id );
                Console.WriteLine( "level:" + level );

                Console.ReadLine();
            } else {
                Console.WriteLine( "connect failure." );
            }
        }
        public static void Disconnect( int device_id, SDKHrobot.HRobot.CallBackFun callback ) {
            if ( device_id >= 0 ) {
                SDKHrobot.HRobot.disconnect( device_id );
            }
        }
    }
}
