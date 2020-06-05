using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using SDKHrobot;

namespace _01_get_robot_info {
    class Program {
        static int device_id = -1;
        private static HRobot.CallBackFun callback = new HRobot.CallBackFun( EventFun );

        static void Main( string[] args ) {
            device_id = HRobot.open_connection( "127.0.0.1", 1, callback );
            if ( device_id >= 0 ) {
                Console.WriteLine( "connect successful." );
                PrintFunction();
                Console.ReadLine();
                HRobot.disconnect( device_id );
            } else {
                Console.WriteLine( "connect failure." );
            }
        }
        public static void EventFun( UInt16 cmd, UInt16 rlt, ref UInt16 Msg, int len ) {
            //Console.WriteLine( "Command: " + cmd + " Resault: " + rlt );
        }

        private static void PrintFunction() {
            const int STD_OUTPUT_HANDLE = -11;
            IntPtr oldBuffer = GetStdHandle( STD_OUTPUT_HANDLE ); //Gets the handle for the default console buffer
            IntPtr newBuffer = CreateConsoleScreenBuffer( 0x80000000 | 0x40000000, 0x00000001, IntPtr.Zero, 1, IntPtr.Zero ); //Creates a new console buffer
            SetConsoleActiveScreenBuffer( newBuffer );

            COORD coord = new COORD( 0, 0 );
            COORD size = new COORD( 100, 80 );
            SetConsoleScreenBufferSize( oldBuffer, size ); //重新設置緩衝區大小
            SetConsoleScreenBufferSize( newBuffer, size ); //重新設置緩衝區大小
            uint bytes = 0;
            StringBuilder data = new StringBuilder( 10000 );
            double utilization_ratio = -1;

            while ( true ) {
                Console.Clear();
                Console.WriteLine( "--------------------------" );
                StringBuilder v = new StringBuilder( 256 );
                SDKHrobot.HRobot.get_hrsdk_version( v );
                Console.WriteLine( "HRSDK VERSION:\t" + v );

                StringBuilder HrssV = new StringBuilder( 256 );
                SDKHrobot.HRobot.get_hrss_version( device_id, HrssV );
                Console.WriteLine( "HRSS version:\t" + HrssV );

                StringBuilder robot_id = new StringBuilder( 256 );
                SDKHrobot.HRobot.get_robot_id( device_id, robot_id );
                Console.WriteLine( "Robot_ID:\t" + robot_id );

                StringBuilder rsr_prog_name = new StringBuilder( 256 );
                SDKHrobot.HRobot.get_rsr_prog_name( device_id, 1, rsr_prog_name );
                Console.WriteLine( "RSR 1 name:\t" + rsr_prog_name );

                StringBuilder execute_file_name = new StringBuilder( 256 );
                SDKHrobot.HRobot.get_execute_file_name( device_id, execute_file_name );
                Console.WriteLine( "Execute file name:\t" + execute_file_name );

                ulong[] alarm_code = new ulong[256];
                int count = 10;
                SDKHrobot.HRobot.get_alarm_code( device_id, ref count, alarm_code );

                SDKHrobot.HRobot.get_robot_type( device_id, v );
                Console.WriteLine( "ROBOT TYPE:\t" + v );

                int motor_state = SDKHrobot.HRobot.get_motor_state( device_id );
                Console.WriteLine( "motor_state:\t" + motor_state );

                int operation_mode = SDKHrobot.HRobot.get_operation_mode( device_id );
                Console.WriteLine( "operation_mode:\t" + operation_mode );

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
                Console.WriteLine( "ACC TIME:\t" + SDKHrobot.HRobot.get_acc_time( device_id ) + "(ms)" );
                Console.WriteLine( "PTP RATIO:\t" + SDKHrobot.HRobot.get_ptp_speed( device_id ) + "(%)" );
                Console.WriteLine( "LIN SPEED:\t" + SDKHrobot.HRobot.get_lin_speed( device_id ) + "mm/s" );
                Console.WriteLine( "OVERRIDE RATIO:\t" + SDKHrobot.HRobot.get_override_ratio( device_id ) + "(%)" );
                Console.WriteLine( "-------------------------------------------------------------\n" );
                int[] YMD = new int[6];
                SDKHrobot.HRobot.get_device_born_date( device_id, YMD );
                Console.WriteLine( "BIRTHDAY:\t" + YMD[0] + "年 " + YMD[1] + "月 " + YMD[2] + "日 " );
                SDKHrobot.HRobot.get_operation_time( device_id, YMD );
                Console.WriteLine( "OPERATION TIME:\t" + YMD[0] + "年 " + YMD[1] + "月 " + YMD[2] + "日 " + YMD[3] + "時 " + YMD[4] + "分" );
                SDKHrobot.HRobot.get_utilization_ratio( device_id, ref utilization_ratio );
                Console.WriteLine( "UTILIZATION RATIO:\t" + utilization_ratio + "(%)" );
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
                PrintHomePos();
                Console.WriteLine( "\n\t-----------------------------------------------\n" );
                PrintPrePos();
                Console.WriteLine( "\n\t-----------------------------------------------\n" );

                int cmd_count = SDKHrobot.HRobot.get_command_count( device_id );
                Console.Write( "cmd_count:\t" + cmd_count );

                int cmd_id = SDKHrobot.HRobot.get_command_id( device_id );
                Console.WriteLine( "\tcmd_id:\t" + cmd_id );

                int mode = SDKHrobot.HRobot.get_hrss_mode( device_id );
                Console.WriteLine( "mode:\t" + mode );

                int DI = SDKHrobot.HRobot.get_digital_input( device_id, 5 );
                Console.Write( "DI_5:\t" + DI );

                int DO = SDKHrobot.HRobot.get_digital_output( device_id, 5 );
                Console.Write( "\tDO_5:\t" + DO );

                int DI_sim = SDKHrobot.HRobot.get_DI_simulation_Enable( device_id, 5 );
                Console.WriteLine( "\tDI_sim_5:\t" + DI_sim );

                StringBuilder DIO_comment = new StringBuilder( 200 );
                int DI_comment = SDKHrobot.HRobot.get_digital_input_comment( device_id, 5, DIO_comment );
                Console.WriteLine( "DI_comment_5:\t" + DIO_comment );

                int DO_comment = SDKHrobot.HRobot.get_digital_output_comment( device_id, 5, DIO_comment );
                Console.WriteLine( "DO_comment_5:\t" + DIO_comment );

                int FI = SDKHrobot.HRobot.get_function_input( device_id, 5 );
                Console.Write( "FI_5:\t" + FI );

                int FO = SDKHrobot.HRobot.get_function_output( device_id, 5 );
                Console.WriteLine( "\tFO_5:\t" + FO );

                int RI = SDKHrobot.HRobot.get_robot_input( device_id, 5 );
                Console.Write( "RI_5:\t" + RI );

                int RO = SDKHrobot.HRobot.get_robot_output( device_id, 5 );
                Console.Write( "\tRO_5:\t" + RO );

                int VO = SDKHrobot.HRobot.get_valve_output( device_id, 1 );
                Console.WriteLine( "\tVO_1:\t" + VO );

                int timer = SDKHrobot.HRobot.get_timer( device_id, 5 );
                Console.Write( "timer_5:\t" + timer );

                int timer_status = HRobot.get_timer_status( device_id, 5 );
                Console.Write( "\ttimer_status:\t" + timer_status );

                StringBuilder timer_name = new StringBuilder( 200 );
                HRobot.get_timer_name( device_id, 5, timer_name );
                Console.WriteLine( "\ttimer_name:\t" + timer_name );

                int counter = SDKHrobot.HRobot.get_counter( device_id, 5 );
                Console.Write( "counter:\t" + counter );

                StringBuilder str = new StringBuilder( 200 );
                int counter_name = SDKHrobot.HRobot.get_counter_name( device_id, 5, str );
                Console.WriteLine( "\tcounter_name:\t" + str );


                int pr_type = SDKHrobot.HRobot.get_pr_type( device_id, 5 );
                Console.WriteLine( "pr_type:\t" + pr_type );

                double[] set_coor = { 0, 0, 0, 0, -90, 0 };
                SDKHrobot.HRobot.set_pr_coordinate( device_id, 5, set_coor );

                int type = -1;
                double[] coor = { 0, 0, 0, 0, 0, 0 }; ;
                int tool = -1;
                int _base = -1;
                Console.Write( "get_pr:\t" );
                SDKHrobot.HRobot.get_pr( device_id, 5, ref type, coor, ref tool, ref _base );
                for ( int a = 0; a < 6; a++ ) {
                    Console.Write( coor[a] + "\t" );
                }
                Console.WriteLine();

                double[] get_coor = new double[6];
                SDKHrobot.HRobot.get_pr_coordinate( device_id, 5, get_coor );
                Console.Write( "pr_coordinate:\t" );
                for ( int i = 0; i < 6; i++ ) {
                    Console.Write( get_coor[i] + " " );
                }
                Console.WriteLine();

                int[] get_tool_base = new int[2];
                SDKHrobot.HRobot.get_pr_tool_base( device_id, 5, get_tool_base );
                Console.Write( "pr_tool_base:\t" );
                for ( int i = 0; i < 2; i++ ) {
                    Console.Write( get_tool_base[i] + " " );
                }
                Console.WriteLine();

                StringBuilder pr_comment = new StringBuilder( 200 );
                HRobot.get_pr_comment( device_id, 5, pr_comment );
                Console.WriteLine( "pr_comment:\t" + pr_comment );

                int base_num = SDKHrobot.HRobot.get_base_number( device_id );
                Console.WriteLine( "base_number:\t" + base_num );

                double[] base_data = new double[6];
                SDKHrobot.HRobot.get_base_data( device_id, 5, base_data );
                Console.Write( "get_base_data:\t" );
                for ( int i = 0; i < 6; i++ ) {
                    Console.Write( base_data[i] + " " );
                }
                Console.WriteLine();

                int tool_num = SDKHrobot.HRobot.get_tool_number( device_id );
                Console.WriteLine( "tool_number:\t" + tool_num );

                double[] tool_data = new double[6];
                SDKHrobot.HRobot.get_tool_data( device_id, 5, tool_data );
                Console.Write( "get_base_data:\t" );
                for ( int i = 0; i < 6; i++ ) {
                    Console.Write( tool_data[i] + " " );
                }
                Console.WriteLine();

                int mi_index = 1;
                bool mi_sim = false;
                bool mi_value = false;
                int mi_type = -1;
                int mi_start = -1;
                int mi_end = -1;
                StringBuilder mi_comment = new StringBuilder( 200 );
                HRobot.get_module_input_config( device_id, mi_index, ref mi_sim, ref mi_value, ref mi_type, ref mi_start, ref mi_end, mi_comment );
                Console.Write( "MI: index:{0}  sim:{1}  value:{2}  type:{3}  ", mi_index, mi_sim, mi_value, mi_type );
                Console.WriteLine( "start:{0}  end:{1}  comment:{2} ", mi_start, mi_end, mi_comment );

                int mo_index = 1;
                bool mo_value = false;
                int mo_type = -1;
                int mo_start = -1;
                int mo_end = -1;
                StringBuilder mo_comment = new StringBuilder( 200 );
                HRobot.get_module_output_config( device_id, mo_index, ref mo_value, ref mo_type, ref mo_start, ref mo_end, mo_comment );
                Console.Write( "MO: index:{0}  value:{1}  type:{2}  ", mo_index, mo_value, mo_type );
                Console.WriteLine( "start:{0}  end:{1}  comment:{2} ", mo_start, mo_end, mo_comment );

                StringBuilder user_msg = new StringBuilder( 200 );
                SDKHrobot.HRobot.get_user_alarm_setting_message( device_id, 5, user_msg );
                Console.WriteLine( "user_alarm_msg_5:\t" + user_msg );

                int value = -1;
                SDKHrobot.HRobot.get_payload_value( device_id, ref value );
                Console.WriteLine( "payload:\t" + value );

                PrintDIOSetting();

                int year = -1;
                int month = -1;
                int day = -1;
                int hour = -1;
                int min = -1;
                int second = -1;
                SDKHrobot.HRobot.get_controller_time( device_id, ref year, ref month, ref day, ref hour, ref min, ref second );
                Console.WriteLine( "controller_time: {0}/{1}/{2} {3}:{4}:{5}  ", year, month, day, hour, min, second );

                ReadConsoleOutputCharacter( oldBuffer, data, 10000, coord, out bytes );
                WriteConsoleOutputCharacter( newBuffer, data, 10000, coord, out bytes );
                System.Threading.Thread.Sleep( 1000 );
            }

        }

        static private void PrintRPM() {
            double[] p = new double[6];
            SDKHrobot.HRobot.get_current_rpm( device_id, p );
            Console.Write( "RPM:\t" );
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

        static private void PrintHomePos() {
            double[] p = new double[6];
            SDKHrobot.HRobot.get_home_point( device_id, p );
            Console.Write( "Home:\t" );
            for ( int a = 0; a < 6; a++ ) {
                Console.Write( p[a] + "\t" );
            }
        }

        static private void PrintPrePos() {
            double[] p = new double[6];
            SDKHrobot.HRobot.get_previous_pos( device_id, p );
            Console.Write( "Pre:\t" );
            for ( int a = 0; a < 6; a++ ) {
                Console.Write( p[a] + "\t" );
            }
        }

        static private void PrintDIOSetting() {
            int[] D_setting = new int[13];
            StringBuilder text = new StringBuilder( 100 );
            string[] DI_SI = { "DI", "SI" };
            string[] DO_SO = { "DO", "SO" };
            string[] msg = { "Clear Error", "External Alarm", "System Shutdown", "Moter Warning", "System StartUp", "Mode Output" };
            SDKHrobot.HRobot.get_digital_setting( device_id, D_setting, text );
            Console.WriteLine( "DIO setting:" );
            for ( int i = 0; i < 12; i++ ) {
                if ( i == 6 ) {
                    Console.WriteLine();
                }
                if ( i % 2 == 0 && i < 6 ) {
                    Console.Write( "{0}  ", msg[( int )( i / 2 )] );
                    Console.Write( "{0}:", DI_SI[D_setting[i]] );
                } else if ( i % 2 == 0 && i >= 6 ) {
                    Console.Write( "{0}  ", msg[( int )( i / 2 )] );
                    Console.Write( "{0}:", DO_SO[D_setting[i]] );
                } else {
                    Console.Write( "{0},   ", D_setting[i] );
                }
            }
            Console.WriteLine();
            Console.WriteLine( "Text Length:{0},  Show Text: {1} ", D_setting[12], text );

        }

        //-----Import dll----------------------//
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
