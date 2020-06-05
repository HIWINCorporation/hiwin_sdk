using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SDKHrobot;
using System.Timers;

namespace _10.TaskStart {
    class Program {
        private static System.Timers.Timer aTimer;
        static int device_id = -1;
        static int x = 0;
        static bool is_Run = false;
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
                SetTimer();
                TaskStartExample();
                Console.ReadLine();
                HRobot.disconnect( device_id );
            } else {
                Console.WriteLine( "connect failure." );
            }
        }
        public static void EventFun( UInt16 cmd, UInt16 rlt, ref UInt16 Msg, int len ) {
            if ( cmd == 4001 ) {
                Console.WriteLine( "Command: " + cmd + "  Result: " + rlt + "  Msg:  " + Msg );
                switch ( rlt ) {
                    case 4012:
                        Console.WriteLine( "task_start HRSS_TASK_NAME_ERR" );
                        break;
                    case 4013:
                        Console.WriteLine( "task_start Alaeady exist." );
                        break;
                    case 4014:
                        Console.WriteLine( "task_start success. Program starts to Run." );
                        aTimer.Enabled = true; // 啟動 timer, 等待 程式結束
                        x++;
                        break;
                }
            }
        }

        public static void TaskStartExample() {
            Console.WriteLine( "send_file0: " + HRobot.send_file( device_id, "../../../code0.hrb", "code0.hrb" ) );
            Thread.Sleep( 500 );
            Console.WriteLine( "send_file1: " + HRobot.send_file( device_id, "../../../code1.hrb", "code1.hrb" ) );
            Thread.Sleep( 500 );
            Console.WriteLine( "send_file2: " + HRobot.send_file( device_id, "../../../code2.hrb", "code2.hrb" ) );
            Thread.Sleep( 500 );
            Console.WriteLine( "send_file3: " + HRobot.send_file( device_id, "../../../code3.hrb", "code3.hrb" ) );
            Thread.Sleep( 500 );
            double[] pos = new double[6];
            HRobot.get_current_position( device_id, pos ); // run callback.
            while ( true ) {
                if ( !is_Run ) {
                    switch ( x % 4 ) {
                        case 0:
                            HRobot.task_start( device_id, "code0.hrb" );
                            break;
                        case 1:
                            HRobot.task_start( device_id, "code1.hrb" );
                            break;
                        case 2:
                            HRobot.task_start( device_id, "code2.hrb" );
                            break;
                        case 3:
                            HRobot.task_start( device_id, "code3.hrb" );
                            break;
                    }
                    Console.WriteLine( "run code{0}.hrb", x % 4 );
                    is_Run = true;
                }
                Thread.Sleep( 4000 );
            }
        }

        public static void SetTimer() {
            aTimer = new System.Timers.Timer( 1000 ); // 1000 ms timer
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
        }

        private static void OnTimedEvent( Object source, ElapsedEventArgs e ) {
            // 判斷 程式結束
            if ( HRobot.get_function_output( device_id, 1 ) == 0 ) {
                Console.WriteLine( "task_start finish. \n" );
                is_Run = false;
                aTimer.Enabled = false;
            }
        }
    }
}

