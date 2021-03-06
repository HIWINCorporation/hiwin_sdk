#include "stdafx.h"
#include "iostream"
#include "windows.h"
#include "../../../../include/HRSDK.h"

#ifdef x64
#pragma comment(lib, "../../../../lib/x64/HRSDK.lib")
#else
#pragma comment(lib, "../../../../lib/x86/HRSDK.lib")
#endif

void __stdcall callBack(uint16_t, uint16_t, uint16_t*, int) {

}

int _tmain(int argc, _TCHAR* argv[]) {
	// set robot parameter
	int rlt; // return 0 if succeed
	HROBOT device_id = open_connection("127.0.0.1", 1, callBack);
	rlt = set_operation_mode(device_id, 1); // switch mode to running mode ,cuz acc can only be changed in runing mode.
	Sleep(50);      // delay is needed when you want to change mode
	std::string ro_id = "timer_name";
	char* c_robot_id = new char[ro_id.length() + 1];
	strcpy_s(c_robot_id, ro_id.length() + 1, ro_id.c_str());
	rlt = set_robot_id(device_id, c_robot_id);
	rlt = set_acc_dec_ratio(device_id, 50);	// set acc ratio(%)
	rlt = set_acc_time(device_id, 250);
	rlt = set_operation_mode(device_id, 0); // switch mode to safety mode ,cuz ptp speed can only be changed in safety mode.
	rlt = set_override_ratio(device_id, 50);	// override ratio(%)
	rlt = set_ptp_speed(device_id, 50);	// PTP speed ratio(%)
	rlt = set_lin_speed(device_id, 800);	// LIN speed (mm/s)
	rlt = set_command_id(device_id, 10);
	rlt = remove_command(device_id, 10);
	rlt = set_digital_output(device_id, 5, true);
	rlt = set_robot_output(device_id, 5, true);
	rlt = set_valve_output(device_id, 1, true);

	/*Module I/O*/
	rlt = set_module_input_value(device_id, 5, true);
	rlt = set_module_input_start(device_id, 5, 1);
	rlt = set_module_input_end(device_id, 5, 5);
	std::string mi_comment = "mi_comment";
	char* c_mi_comment = new char[mi_comment.length() + 1];
	strcpy_s(c_mi_comment, mi_comment.length() + 1, mi_comment.c_str());
	rlt = set_module_input_comment(device_id, 5, c_mi_comment);
	rlt = set_module_output_value(device_id, 5, true);
	rlt = set_module_output_start(device_id, 5, 1);
	rlt = set_module_output_end(device_id, 5, 5);
	std::string mo_comment = "mo_comment";
	char* c_mo_comment = new char[mo_comment.length() + 1];
	strcpy_s(c_mo_comment, mo_comment.length() + 1, mo_comment.c_str());
	set_module_output_comment(device_id, 5, c_mo_comment);

	rlt = set_base_number(device_id, 5);
	rlt = set_tool_number(device_id, 5);
	rlt = set_timer(device_id, 5, 1000);
	rlt = set_timer_start(device_id, 5);
	rlt = set_timer_stop(device_id, 5);
	std::string timer_name = "timer_name";
	char* c_timer_name = new char[timer_name.length() + 1];
	strcpy_s(c_timer_name, timer_name.length() + 1, timer_name.c_str());
	rlt = set_timer_name(device_id, 5, c_timer_name);
	rlt = set_counter(device_id, 5, 1000);
	rlt = set_pr_type(device_id, 5, 1000);
	double coor[6] = { 0, 0, 0, 0, -90, 0 };
	rlt = set_pr_coordinate(device_id, 5, coor);
	double tool_base[6] = { 5, 5 };
	rlt = set_pr_coordinate(device_id, 5, tool_base);
	rlt = set_pr_tool_base(device_id, 5, 2, 2);
	std::string pr_comment = "pr_comment";
	char* c_pr_comment = new char[pr_comment.length() + 1];
	strcpy_s(c_pr_comment, pr_comment.length() + 1, pr_comment.c_str());
	rlt = set_pr_comment(device_id, 5, c_pr_comment);
	rlt = define_base(device_id, 5, coor);
	rlt = define_tool(device_id, 5, coor);
	rlt = set_pr(device_id, 5, 1, coor, 5, 5);
	rlt = remove_pr(device_id, 5);
	rlt = set_smooth_length(device_id, 200);
	std::string file_name = "set_rsr.hrb";
	char* c_file_name = new char[file_name.length() + 1];
	strcpy_s(c_file_name, file_name.length() + 1, file_name.c_str());
	rlt = set_rsr(device_id, c_file_name, 1);
	rlt = remove_rsr(device_id, 1);
	rlt = set_motor_state(device_id, 1);

	// new 2020.03.19
	bool ON = true;
	char* str = "test text";
	rlt = set_DI_simulation_Enable(device_id, 5, ON);
	rlt = set_DI_simulation(device_id, 5, ON);
	rlt = set_digital_input_comment(device_id, 5, str);
	rlt = set_digital_output_comment(device_id, 5, str);
	double joint[6] = { 0, 0, 0, 0, 0, 0 };
	rlt = set_home_point(device_id, joint);
	rlt = set_module_input_type(device_id, 5, 0);
	rlt = set_module_output_type(device_id, 5, 1);
	rlt = set_counter_name(device_id, 5, str);
	int DIO = 0;
	int SIO = 1;
	int data[12] = { DIO, 34, DIO, 35, SIO, 36, SIO, 37, DIO, 38, SIO, 39 };
	rlt = set_digital_setting(device_id, data, str);
	rlt = set_user_alarm_setting_message(device_id, 5, str);
	rlt = set_language(device_id, 0);
	rlt = save_module_io_setting(device_id);

	return 0;
}

