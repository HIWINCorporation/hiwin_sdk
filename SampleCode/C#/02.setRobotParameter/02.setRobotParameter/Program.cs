using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SDKHrobot;

namespace _02_setRobotParameter {
    class Program {
        private static HRobot.CallBackFun callback = new HRobot.CallBackFun( EventFun );
        static void Main( string[] args ) {
            int device_id = HRobot.open_connection( "127.0.0.1", 1, callback );
            if ( device_id >= 0 ) {
                SetRobotParameter( device_id, callback );
                SDKHrobot.HRobot.disconnect( device_id );
            }

        }
        public static void SetRobotParameter( int device_id, HRobot.CallBackFun callback ) {
            if ( device_id >= 0 ) {
                Console.WriteLine( "connect successful." );
                callback = new HRobot.CallBackFun( EventFun );
            } else {
                Console.WriteLine( "connect failure." );
                Console.ReadLine();
            }
            bool ON = true;
            int rlt = HRobot.set_operation_mode( device_id, 1 ); //switch mode to running mode ,cuz acc can only be changed in runing mode.
            StringBuilder robot_id = new StringBuilder( "HrSs_Robot_ID" );
            StringBuilder str = new StringBuilder( "test text" );

            rlt = HRobot.set_robot_id( device_id, robot_id );
            rlt = HRobot.set_acc_time( device_id, 250 );
            rlt = HRobot.set_acc_dec_ratio( device_id, 50 );	// set acc ratio(%)
            rlt = HRobot.set_operation_mode( device_id, 0 ); //switch mode to safety mode ,cuz ptp speed can only be changed in safety mode.
            rlt = HRobot.set_override_ratio( device_id, 50 );	//override ratio(%)
            rlt = HRobot.set_ptp_speed( device_id, 50 );	//PTP speed ratio(%)
            rlt = HRobot.set_lin_speed( device_id, 800 );	//LIN speed (mm/s)
            rlt = HRobot.set_command_id( device_id, 10 );
            rlt = HRobot.set_digital_output( device_id, 5, true );
            rlt = HRobot.set_robot_output( device_id, 5, true );
            rlt = HRobot.set_valve_output( device_id, 1, true );
            rlt = HRobot.set_base_number( device_id, 5 );
            rlt = HRobot.set_tool_number( device_id, 5 );
            rlt = HRobot.set_timer( device_id, 5, 1000 );
            rlt = HRobot.set_timer_start( device_id, 5 );
            rlt = HRobot.set_timer_stop( device_id, 5 );
            StringBuilder timer_name = new StringBuilder( "timer_name" );
            rlt = HRobot.set_timer_name( device_id, 5, timer_name );
            rlt = HRobot.set_counter( device_id, 5, 1000 );
            rlt = HRobot.set_pr_type( device_id, 5, 1000 );
            double[] coor = { 0, 0, 0, 0, -90, 0 };
            rlt = HRobot.set_pr_coordinate( device_id, 5, coor );
            double[] tool_base = { 5, 5 };
            rlt = HRobot.set_pr_coordinate( device_id, 5, tool_base );
            rlt = HRobot.set_pr_tool_base( device_id, 5, 2, 2 );
            rlt = HRobot.define_base( device_id, 5, coor );
            rlt = HRobot.define_tool( device_id, 5, coor );
            rlt = HRobot.set_pr( device_id, 5, 1, coor, 5, 5 );
            rlt = HRobot.remove_pr( device_id, 5 );
            rlt = HRobot.set_smooth_length( device_id, 200 );
            StringBuilder rsr = new StringBuilder( "set_rsr.hrb" );
            rlt = HRobot.set_rsr( device_id, rsr, 1 );
            rlt = HRobot.set_motor_state( device_id, 1 );

            rlt = HRobot.set_module_input_simulation( device_id, 5, true );
            rlt = HRobot.set_module_input_value( device_id, 5, true );
            rlt = HRobot.set_module_input_start( device_id, 5, 1 );
            rlt = HRobot.set_module_input_end( device_id, 5, 5 );
            StringBuilder mi_comment = new StringBuilder( "mi_comment" );
            rlt = HRobot.set_module_input_comment( device_id, 5, mi_comment );
            rlt = HRobot.set_module_output_value( device_id, 5, true );
            rlt = HRobot.set_module_output_start( device_id, 5, 1 );
            rlt = HRobot.set_module_output_end( device_id, 5, 5 );
            StringBuilder mo_comment = new StringBuilder( "mi_comment" );
            rlt = HRobot.set_module_output_comment( device_id, 5, mo_comment );

            // new 2020.03.19
            rlt = HRobot.set_DI_simulation_Enable( device_id, 5, ON );
            rlt = HRobot.set_DI_simulation( device_id, 5, ON );
            rlt = HRobot.set_digital_input_comment( device_id, 5, str );
            rlt = HRobot.set_digital_output_comment( device_id, 5, str );
            double[] joint = { 0, 0, 0, 0, 0, 0 };
            rlt = HRobot.set_home_point( device_id, joint );
            rlt = HRobot.set_module_input_type( device_id, 5, 0 );
            rlt = HRobot.set_module_output_type( device_id, 5, 1 );
            rlt = HRobot.set_counter_name( device_id, 5, str );
            int DIO = 0;
            int SIO = 1;
            int[] data = { DIO, 34, DIO, 35, SIO, 36, SIO, 37, DIO, 38, SIO, 39};
            rlt = HRobot.set_digital_setting( device_id, data, str );
            rlt = HRobot.set_user_alarm_setting_message( device_id, 5, str );
            rlt = HRobot.set_language( device_id, 0 );
            rlt = HRobot.save_module_io_setting( device_id );

            Console.ReadLine();
        }

        public static void EventFun( UInt16 cmd, UInt16 rlt, ref UInt16 Msg, int len ) {
            Console.WriteLine( "Command: " + cmd + " Resault: " + rlt );
        }
    }
}
