using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace _03_jog {
    class Program {
        private static SDKHrobot.HRobot.CallBackFun callback = new SDKHrobot.HRobot.CallBackFun( EventFun );
        static int device_id;
        static void Main( string[] args ) {
            device_id = SDKHrobot.HRobot.open_connection( "127.0.0.1", 1, callback );
            if ( device_id >= 0 ) {
                Jog( device_id, callback );
                SDKHrobot.HRobot.disconnect( device_id );
            }
        }
        public static void Jog( int id, SDKHrobot.HRobot.CallBackFun callback ) {
            device_id = id;
            if ( device_id >= 0 ) {
                Console.WriteLine( "connect successful." );
                callback = new SDKHrobot.HRobot.CallBackFun( EventFun );
            } else {
                Console.WriteLine( "connect failure." );
            }

            if ( SDKHrobot.HRobot.get_motor_state( device_id ) == 0 ) {
                SDKHrobot.HRobot.set_motor_state( device_id, 1 ); // Servo on
            }

            SDKHrobot.HRobot.set_override_ratio( device_id, 100 );
            Thread printThread = new Thread( PrintFunction );
            printThread.Start();
            while ( true ) {
                ConsoleKey keyNum = Console.ReadKey().Key;
                if ( keyNum == ConsoleKey.Escape ) {
                    break;
                }
                JogByKeyDown( keyNum, 0 );
                WaitKeUp();
            }

        }
        public static void EventFun( UInt16 cmd, UInt16 rlt, ref UInt16 Msg, int len ) {
            Console.WriteLine( "Command: " + cmd + " Resault: " + rlt );
        }

        static void JogByKeyDown( ConsoleKey keyNum, int type ) {

            switch ( keyNum ) {
                case ConsoleKey.Q:
                    SDKHrobot.HRobot.jog( device_id, type, 0, 1 );
                    break;
                case ConsoleKey.W:
                    SDKHrobot.HRobot.jog( device_id, type, 0, -1 );
                    break;
                case ConsoleKey.A:
                    SDKHrobot.HRobot.jog( device_id, type, 1, 1 );
                    break;
                case ConsoleKey.S:
                    SDKHrobot.HRobot.jog( device_id, type, 1, -1 );
                    break;
                case ConsoleKey.Z:
                    SDKHrobot.HRobot.jog( device_id, type, 2, 1 );
                    break;
                case ConsoleKey.X:
                    SDKHrobot.HRobot.jog( device_id, type, 2, -1 );
                    break;
                case ConsoleKey.E:
                    SDKHrobot.HRobot.jog( device_id, type, 3, 1 );
                    break;
                case ConsoleKey.R:
                    SDKHrobot.HRobot.jog( device_id, type, 3, -1 );
                    break;
                case ConsoleKey.D:
                    SDKHrobot.HRobot.jog( device_id, type, 4, 1 );
                    break;
                case ConsoleKey.F:
                    SDKHrobot.HRobot.jog( device_id, type, 4, -1 );
                    break;
                case ConsoleKey.C:
                    SDKHrobot.HRobot.jog( device_id, type, 5, 1 );
                    break;
                case ConsoleKey.V:
                    SDKHrobot.HRobot.jog( device_id, type, 5, -1 );
                    break;
            }
        }

        static void WaitKeUp() {

            while ( GetAsyncKeyState( 81 ) < 0 ||
                    GetAsyncKeyState( 87 ) < 0 ||
                    GetAsyncKeyState( 65 ) < 0 ||
                    GetAsyncKeyState( 83 ) < 0 ||
                    GetAsyncKeyState( 90 ) < 0 ||
                    GetAsyncKeyState( 88 ) < 0 ||
                    GetAsyncKeyState( 69 ) < 0 ||
                    GetAsyncKeyState( 82 ) < 0 ||
                    GetAsyncKeyState( 68 ) < 0 ||
                    GetAsyncKeyState( 70 ) < 0 ||
                    GetAsyncKeyState( 67 ) < 0 ||
                    GetAsyncKeyState( 86 ) < 0 )

            {
                Thread.Sleep( 5 );
            }
            SDKHrobot.HRobot.jog_stop( device_id );
        }

        private static void PrintFunction() {

            while ( true ) {
                Console.Clear();
                Console.WriteLine( "--------------------------------------------" );
                Console.WriteLine( "press Q/W to move joint 1" );
                Console.WriteLine( "press A/S to move joint 2" );
                Console.WriteLine( "press Z/X to move joint 3" );
                Console.WriteLine( "press E/R to move joint 4" );
                Console.WriteLine( "press D/F to move joint 5" );
                Console.WriteLine( "press C/V to move joint 6" );
                Console.WriteLine( "--------------------------------------------" );
                StringBuilder v = new StringBuilder( 256 );
                SDKHrobot.HRobot.get_hrsdk_version( v );
                Console.WriteLine( "HRSDK VERSION:\t" + v );

                StringBuilder HrssV = new StringBuilder( 256 );
                SDKHrobot.HRobot.get_hrss_version( device_id, HrssV );
                Console.WriteLine( "HRSS version:\t" + HrssV );

                SDKHrobot.HRobot.get_robot_type( device_id, v );
                Console.WriteLine( "ROBOT TYPE:\t" + v );
                switch ( SDKHrobot.HRobot.get_motion_state( device_id ) ) {
                    case 1:
                        Console.WriteLine( "運動狀態:\t" + "閒置" );
                        break;
                    case 2:
                        Console.WriteLine( "運動狀態:\t" + "運動" );
                        break;
                    case 3:
                        Console.WriteLine( "運動狀態:\t" + "暫停" );
                        break;
                    case 4:
                        Console.WriteLine( "運動狀態:\t" + "延遲" );
                        break;
                    case 5:
                        Console.WriteLine( "運動狀態:\t" + "等待命令" );
                        break;
                    default:
                        Console.WriteLine( "運動狀態:\t" + "發生錯誤" );
                        break;
                }
                Console.WriteLine( "-------------------------------------------------------------\n" );
                Console.WriteLine( "ACC RATIO:\t" + SDKHrobot.HRobot.get_acc_dec_ratio( device_id ) + "(%)" );
                Console.WriteLine( "PTP RATIO:\t" + SDKHrobot.HRobot.get_ptp_speed( device_id ) + "(%)" );
                Console.WriteLine( "LIN SPEED:\t" + SDKHrobot.HRobot.get_lin_speed( device_id ) + "mm/s" );
                Console.WriteLine( "OVERRIDE RATIO:\t" + SDKHrobot.HRobot.get_override_ratio( device_id ) + "(%)" );
                Console.WriteLine( "-------------------------------------------------------------\n" );
                int[] YMD = new int[6];
                SDKHrobot.HRobot.get_device_born_date( device_id, YMD );
                Console.WriteLine( "BIRTHDAY:\t" + YMD[0] + "年 " + YMD[1] + "月 " + YMD[2] + "日 " );
                SDKHrobot.HRobot.get_operation_time( device_id, YMD );
                Console.WriteLine( "OPERATION TIME:\t" + YMD[0] + "年 " + YMD[1] + "月 " + YMD[2] + "日 " + YMD[3] + "時 " + YMD[4] + "分" );
                double ratio = -1;
                Console.WriteLine( "UTILIZATION RATIO:\t" + SDKHrobot.HRobot.get_utilization_ratio( device_id, ref ratio ) + "(%)" );
                SDKHrobot.HRobot.get_utilization( device_id, YMD );
                Console.WriteLine( "UTILIZATION:\t" + YMD[0] + "年 " + YMD[1] + "月 " + YMD[2] + "日 " + YMD[3] + "時 " + YMD[4] + "分" + YMD[5] + "秒" );
                Console.WriteLine( "-------------------------------------------------------------\n" );

                Console.WriteLine( "\tA1\tA2\tA3\tA4\tA5\tA6" );
                PrintRPM();
                Console.WriteLine( "\n\t-----------------------------------------------\n" );
                PrintCartPos();
                Console.WriteLine( "\n\t-----------------------------------------------\n" );
                PrintJointPos();
                Console.WriteLine( "\n\t-----------------------------------------------\n" );
                PrintEnc();
                Console.WriteLine( "\n\t-----------------------------------------------\n" );
                PrintTorq();
                Console.WriteLine( "\n\t-----------------------------------------------\n" );
                PrintMil();
                Console.WriteLine( "\n\t-----------------------------------------------\n" );
                Thread.Sleep( 2000 );
            }

        }

        static private void PrintRPM() {
            double[] p = new double[6];
            SDKHrobot.HRobot.get_current_rpm( device_id, p );
            Console.Write( "PRM:\t" );
            for ( int a = 0; a < 6; a++ ) {
                Console.Write( p[a] + "\t" );
            }

        }

        static private void PrintCartPos() {
            double[] p = new double[6];
            SDKHrobot.HRobot.get_current_position( device_id, p );
            Console.Write( "Cart:\t" );
            for ( int a = 0; a < 6; a++ ) {
                Console.Write( p[a] + "\t" );
            }
        }
        static private void PrintJointPos() {
            double[] p = new double[6];
            SDKHrobot.HRobot.get_current_joint( device_id, p );
            Console.Write( "Joint:\t" );
            for ( int a = 0; a < 6; a++ ) {
                Console.Write( p[a] + "\t" );
            }
        }

        static private void PrintEnc() {

            Int32[] p = new Int32[6];
            SDKHrobot.HRobot.get_encoder_count( device_id, p );
            Console.Write( "ENC:\t" );
            for ( int a = 0; a < 6; a++ ) {
                Console.Write( p[a] + "\t" );
            }
        }

        static private void PrintTorq() {
            double[] p = new double[6];
            SDKHrobot.HRobot.get_motor_torque( device_id, p );
            Console.Write( "Torq:\t" );
            for ( int a = 0; a < 6; a++ ) {
                Console.Write( p[a] + "\t" );
            }
        }

        static private void PrintMil() {
            double[] p = new double[6];
            SDKHrobot.HRobot.get_mileage( device_id, p );
            Console.Write( "Mil:\t" );
            for ( int a = 0; a < 6; a++ ) {
                Console.Write( p[a] + "\t" );
            }
            Console.WriteLine( "\n" );

            SDKHrobot.HRobot.get_total_mileage( device_id, p );
            Console.Write( "TMil:\t" );
            for ( int a = 0; a < 6; a++ ) {
                Console.Write( p[a] + "\t" );
            }

        }

        //-----Import dll----------------------//

        [DllImport( "user32.dll" )]
        static extern short GetAsyncKeyState( int vKey );

        [DllImport( "kernel32.dll", SetLastError = true )]
        static extern IntPtr CreateConsoleScreenBuffer( uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwFlags, IntPtr lpScreenBufferData );

        [DllImport( "Kernel32", SetLastError = true )]
        private static extern IntPtr GetStdHandle( int nStdHandle );

        [DllImport( "kernel32.dll", SetLastError = true )]
        static extern bool SetConsoleActiveScreenBuffer( IntPtr hConsoleOutput );

        [DllImport( "kernel32.dll", SetLastError = true )]
        static extern bool SetConsoleScreenBufferSize( IntPtr hConsoleOutput, COORD size );

        [DllImport( "Kernel32", SetLastError = true )]
        static extern bool ReadConsoleOutputCharacter( IntPtr hConsoleOutput,
                [Out] StringBuilder lpCharacter, uint nLength, COORD dwReadCoord,
                out uint lpNumberOfCharsRead );

        [DllImport( "kernel32.dll" )]
        static extern bool WriteConsoleOutputCharacter( IntPtr hConsoleOutput,
                [Out] StringBuilder lpCharacter, uint nLength, COORD dwWriteCoord,
                out uint lpNumberOfCharsWritten );

        [StructLayout( LayoutKind.Sequential )]
        public struct COORD {
            public short X;
            public short Y;

            public COORD( short X, short Y ) {
                this.X = X;
                this.Y = Y;
            }
        };
    }
}
