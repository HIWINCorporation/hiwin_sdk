using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _08_File {
    class Program {
        private static SDKHrobot.HRobot.CallBackFun callback = new SDKHrobot.HRobot.CallBackFun( EventFun );
        static void Main( string[] args ) {
            int device_id = SDKHrobot.HRobot.open_connection( "127.0.0.1", 1, callback );
            if ( device_id >= 0 ) {
                File( device_id, callback );
                SDKHrobot.HRobot.disconnect( device_id );
            }
        }
        public static void File( int device_id, SDKHrobot.HRobot.CallBackFun callback ) {
            if ( device_id >= 0 ) {
                Console.WriteLine( "connect successful." );
                callback = new SDKHrobot.HRobot.CallBackFun( EventFun );
            } else {
                Console.WriteLine( "connect failure." );
                return;
            }

            if ( SDKHrobot.HRobot.get_motor_state( device_id ) == 0 ) {
                SDKHrobot.HRobot.set_motor_state( device_id, 1 ); // Servo on
            }

            SDKHrobot.HRobot.ext_task_start( device_id, 0, 1 );
            Thread.Sleep( 2000 );
            SDKHrobot.HRobot.task_start( device_id, "test.hrb" );
            Thread.Sleep( 2000 );
            SDKHrobot.HRobot.task_hold( device_id );
            Thread.Sleep( 2000 );
            SDKHrobot.HRobot.task_continue( device_id );
            Thread.Sleep( 2000 );
            SDKHrobot.HRobot.task_abort( device_id );

            Thread.Sleep( 2000 );
            string from_file_path = "test.hrb";
            string to_file_path = "Program\\test.hrb";
            SDKHrobot.HRobot.download_file( device_id, from_file_path, to_file_path ); // 下載控制器檔名為test.hrb檔案至專案底下的Program資料夾中，且以test.hrb為名。
            string file_name = "Program\\test.hrb";
            Thread.Sleep( 2000 ); // 等待檔案下載完成，download_file與send_file不可同時進行
            SDKHrobot.HRobot.send_file( device_id, file_name, "test.hrb" ); // 從專案底下的Program資料夾中上傳檔名為test.hrb檔案，且以test.hrb為名。
            Thread.Sleep( 100 );
            int cnt = -1;
            ulong[] alarm = new ulong[20];
            SDKHrobot.HRobot.get_alarm_code( device_id, ref cnt, alarm );
            if ( cnt > 0 ) {
                SDKHrobot.HRobot.clear_alarm( device_id );
            }
            Thread.Sleep( 200 );
            StringBuilder update = new StringBuilder( "update" );
            SDKHrobot.HRobot.update_hrss( device_id, update );

            SDKHrobot.HRobot.new_folder( device_id, "new_folder" );
            SDKHrobot.HRobot.file_rename( device_id, "test2.hrb", "test3.hrb" );
            SDKHrobot.HRobot.file_drag( device_id, "test3.hrb", "new_folder/test3.hrb" );
            SDKHrobot.HRobot.delete_file( device_id, "new_folder/test3.hrb" );
            SDKHrobot.HRobot.delete_folder( device_id, "new_folder" );
            for ( int i = 0; i < SDKHrobot.HRobot.get_prog_number( device_id ); i++ ) {
                StringBuilder str2 = new StringBuilder( 20 );
                SDKHrobot.HRobot.get_prog_name( device_id, i, str2 );
                Console.WriteLine( str2 );
            }
            Console.ReadKey();
        }

        public static void EventFun( UInt16 cmd, UInt16 rlt, ref UInt16 Msg, int len ) {
            Console.WriteLine( "Command: " + cmd + " Resault: " + rlt );
        }
    }
}
