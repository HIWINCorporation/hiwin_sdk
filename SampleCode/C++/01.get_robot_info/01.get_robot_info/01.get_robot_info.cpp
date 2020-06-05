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

void PrintRPM();
void PrintCartPos();
void PrintJointPos();
void PrintEnc();
void PrintTorq();
void PrintMil();
void PrintFunction();
void PrintHomePos();
void PrintPrePos();
void PrintDIOSetting();

HROBOT s;

int _tmain(int argc, _TCHAR* argv[]) {
	s = open_connection("127.0.0.1", 1, callBack);
	PrintFunction();
	disconnect(s);
	return 0;
}

int GetRobotInfo(HROBOT device_id, callback_function callback) {
	s = device_id;
	PrintFunction();
	return 0;
}


void PrintFunction() {
	DWORD bytes = 0;
	wchar_t data[10000];

	HANDLE oldBuffer = GetStdHandle(STD_OUTPUT_HANDLE);
	HANDLE newBuffer = CreateConsoleScreenBuffer(0x80000000 | 0x40000000, 0x00000001, NULL, 1, NULL);
	SetConsoleActiveScreenBuffer(newBuffer);
	COORD coordBufSize;
	COORD coordBufCoord;
	coordBufSize.X = 100;
	coordBufSize.Y = 72;
	coordBufCoord.X = 0;
	coordBufCoord.Y = 0;
	SetConsoleScreenBufferSize(oldBuffer, coordBufSize);
	SetConsoleScreenBufferSize(newBuffer, coordBufSize);
	DWORD written;


	while (s >= 0) {
		system("cls");
		char* v = new char[256];
		get_hrsdk_version(v);
		std::cout << "HRSDK VERSION:\t" << v << std::endl;

		char* HrssV = new char[256];
		get_hrss_version(s, HrssV);
		std::cout << "HRSS version:\t" << HrssV << std::endl;
		delete[]HrssV;

		char* robot_id = new char[256];
		get_robot_id(s, robot_id);
		std::cout << "Robot_ID:\t" << robot_id << std::endl;
		delete[]robot_id;

		char* rsr_prog_name = new char[256];
		get_rsr_prog_name(s, 1, rsr_prog_name);
		std::cout << "RSR 1 name:\t" << rsr_prog_name << std::endl;
		delete[]rsr_prog_name;

		char* execute_file_name = new char[50];
		get_execute_file_name(s, execute_file_name);
		std::cout << "Execute file name:\t" << execute_file_name << std::endl;
		delete[]execute_file_name;


		uint64_t alarm_code[20] = { 0 };
		int count = 0;
		get_alarm_code(s, count, alarm_code);

		int motor_state = get_motor_state(s);
		std::cout << "motor_state:\t" << motor_state << std::endl;

		int operation_mode = get_operation_mode(s);
		std::cout << "operation_mode:\t" << operation_mode << std::endl;

		get_robot_type(s, v);
		std::cout << "ROBOT TYPE:\t" << v << std::endl;
		delete[]v;
		switch (get_motion_state(s)) {
		case 1:
			std::cout << "運動狀態:\t" << "閒置" << std::endl;
			break;
		case 2:
			std::cout << "運動狀態:\t" << "運動" << std::endl;
			break;
		case 3:
			std::cout << "運動狀態:\t" << "暫停" << std::endl;
			break;
		case 4:
			std::cout << "運動狀態:\t" << "延遲" << std::endl;
			break;
		case 5:
			std::cout << "運動狀態:\t" << "等待命令" << std::endl;
			break;
		default:
			std::cout << "運動狀態:\t" << "發生錯誤" << std::endl;
		}
		printf("-------------------------------------------------------------\n");
		std::cout << "ACC RATIO:\t" << get_acc_dec_ratio(s) << "(%)" << std::endl;
		std::cout << "ACC TIME:\t" << get_acc_time(s) << "(ms)" << std::endl;
		std::cout << "PTP RATIO:\t" << get_ptp_speed(s) << "(%)" << std::endl;
		std::cout << "LIN SPEED:\t" << get_lin_speed(s) << "mm/s" << std::endl;
		std::cout << "OVERRIDE RATIO:\t" << get_override_ratio(s) << "(%)" << std::endl;
		printf("-------------------------------------------------------------\n");
		int YMD[20] = { 0 };
		get_device_born_date(s, YMD);
		std::cout << "BIRTHDAY:\t" << YMD[0] << "年 " << YMD[1] << "月 " << YMD[2] << "日 " << std::endl;
		get_operation_time(s, YMD);
		std::cout << "OPERATION TIME:\t" << YMD[0] << "年 " << YMD[1] << "月 " << YMD[2] << "日 " << YMD[3] << "時 " << YMD[4] << "分" << std::endl;

		double utilization_ratio;
		get_utilization_ratio(s, utilization_ratio);
		std::cout << "UTILIZATION RATIO:\t" << utilization_ratio << "(%)" << std::endl;
		get_utilization(s, YMD);
		std::cout << "UTILIZATION:\t" << YMD[0] << "年 " << YMD[1] << "月 " << YMD[2] << "日 " << YMD[3] << "時 " << YMD[4] << "分" << YMD[5] << "秒" << std::endl;
		printf("-------------------------------------------------------------\n");

		std::cout << "\tA1\tA2\tA3\tA4\tA5\tA6" << std::endl;
		PrintRPM();
		printf("\n\t-----------------------------------------------\n");
		PrintCartPos();
		printf("\n\t-----------------------------------------------\n");
		PrintJointPos();
		printf("\n\t-----------------------------------------------\n");
		PrintEnc();
		printf("\n\t-----------------------------------------------\n");
		PrintTorq();
		printf("\n\t-----------------------------------------------\n");
		PrintMil();
		printf("\n\t-----------------------------------------------\n");
		PrintHomePos();
		printf("\n\t-----------------------------------------------\n");
		PrintPrePos();
		printf("\n\t-----------------------------------------------\n");

		int cmd_count = get_command_count(s);
		std::cout << "cmd_count:\t" << cmd_count;

		int cmd_id = get_command_id(s);
		std::cout << "\tcmd_id:\t" << cmd_id << std::endl;

		int mode = get_hrss_mode(s);
		std::cout << "mode:\t" << mode << std::endl;

		int DI = get_digital_input(s, 5);
		std::cout << "DI_5:\t" << DI;

		int DO = get_digital_output(s, 5);
		std::cout << "\tDO_5:\t" << DO;

		int DI_sim = get_DI_simulation_Enable(s, 5);
		std::cout << "\tDI_sim_5:\t" << DI << std::endl;

		char* comment = new char[200];
		int DI_comment = get_digital_input_comment(s, 5, comment);
		std::cout << "DI_comment_5:\t" << comment << std::endl;

		int DO_comment = get_digital_output_comment(s, 5, comment);
		std::cout << "DO_comment_5:\t" << comment << std::endl;

		int FI = get_function_input(s, 5);
		std::cout << "FI_5:\t" << FI;

		int FO = get_function_output(s, 5);
		std::cout << "\tFO_5:\t" << FO << std::endl;

		int RI = get_robot_input(s, 1);
		std::cout << "RI_1:\t" << RI;

		int RO = get_robot_output(s, 1);
		std::cout << "\tRO_5:\t" << RO;

		int VO = get_valve_output(s, 1);
		std::cout << "\tVO_1:\t" << VO << std::endl;

		int timer = get_timer(s, 5);
		std::cout << "timer:\t" << timer;

		int timer_status = get_timer_status(s, 5);
		std::cout << "\ttimer_status:\t" << timer_status;

		get_timer_name(s, 5, comment);
		std::cout << "\t timer_name:\t" << comment << std::endl;

		int counter = get_counter(s, 5);
		std::cout << "counter:\t" << counter;

		get_counter_name(s, 5, comment);
		std::cout << "\t counter_name:\t" << counter << std::endl;

		double pr_coor[6] = { 0, 0, 0, 0, -90, 0 };
		set_pr(s, 5, 1, pr_coor, 5, 5);

		int pr_type = get_pr_type(s, 5);
		std::cout << "pr_type:\t" << pr_type << std::endl;

		char* pr_comment = new char[256];
		get_pr_comment(s, 5, pr_comment);
		std::cout << "PR Comment:\t" << pr_comment << std::endl;
		delete[]pr_comment;

		double get_coor[6] = { 0 };
		get_pr_coordinate(s, 5, get_coor);
		std::cout << "pr_coordinate:\t";
		for (int i = 0; i < 6; i++) {
			std::cout << get_coor[i] << " ";
		}
		std::cout << std::endl;

		int get_tool_base[6] = { 0 };
		get_pr_tool_base(s, 5, get_tool_base);
		std::cout << "pr_tool_base:\t";
		for (int i = 0; i < 6; i++) {
			std::cout << get_tool_base[i] << " ";
		}
		std::cout << std::endl;

		int coor_type = 0;
		double coor[6] = { 0 };
		int tool = 0;
		int base = 0;
		get_pr(s, 5, &coor_type, coor, &tool, &base);
		std::cout << "coor_type:\t" << coor_type << std::endl;
		std::cout << "get_pr:\t";
		for (int i = 0; i < 6; i++) {
			std::cout << coor[i] << " ";
		}
		std::cout << std::endl;
		std::cout << "tool:\t" << tool << std::endl;
		std::cout << "base:\t" << base << std::endl;

		int base_num = get_base_number(s);
		std::cout << "base_number:\t" << base_num << std::endl;

		double base_data[6] = { 0 };
		get_base_data(s, 5, base_data);
		std::cout << "get_base_data:\t";
		for (int i = 0; i < 6; i++) {
			std::cout << base_data[i] << " ";
		}
		std::cout << std::endl;

		int tool_num = get_tool_number(s);
		std::cout << "tool_number:\t" << tool_num << std::endl;

		double tool_data[6] = { 0 };
		get_tool_data(s, 5, tool_data);
		std::cout << "get_tool_data:\t";
		for (int i = 0; i < 6; i++) {
			std::cout << tool_data[i] << " ";
		}
		std::cout << std::endl;

		/*Module I/O*/
		int mi_input = 5;
		bool mi_sim = false;
		bool mi_value = false;
		int mi_type = 0;
		int mi_start = 0;
		int mi_end = 0;
		char* mi_comment = new char[256];
		get_module_input_config(s, mi_input, mi_sim, mi_value, mi_type, mi_start, mi_end, mi_comment);
		printf("MI: index:%d  sim:%d  value:%d  type:%d  ", mi_input, mi_sim, mi_value, mi_type);
		printf("start:%d  end:%d  comment:%s \n", mi_start, mi_end, mi_comment);

		int mo_output = 5;
		bool mo_value = false;
		int mo_type = 0;
		int mo_start = 0;
		int mo_end = 0;
		char* mo_comment = new char[256];
		get_module_output_config(s, mo_output, mo_value, mo_type, mo_start, mo_end, mo_comment);
		printf("MO: index:%d  value:%d  type:%d  ", mo_output, mo_value, mo_type);
		printf("start:%d  end:%d  comment: %s \n", mo_start, mo_end, mo_comment);

		get_user_alarm_setting_message(s, 5, comment);
		printf("user_alarm_msg_5:\t%s \n", comment);

		int value = -1;
		get_payload_value(s, value);
		printf("payload:\t %d \n", value);

		PrintDIOSetting();

		int year = -1;
		int month = -1;
		int day = -1;
		int hour = -1;
		int min = -1;
		int second = -1;
		get_controller_time(s, year, month, day, hour, min, second);
		printf("controller_time: %d/%d/%d %d:%d:%d  \n", year, month, day, hour, min, second);

		ReadConsoleOutputCharacter(oldBuffer, data, 10000, coordBufCoord, &bytes);
		WriteConsoleOutputCharacter(newBuffer, data, 10000, coordBufCoord, &bytes);

		Sleep(1000);
	}
	system("pause");
}


void PrintRPM() {
	double p[6] = { 0 };
	get_current_rpm(s, p);
	printf("PRM:");
	for (int a = 0; a < 6; a++) {
		printf("\t%.2f", p[a]);
	}
}

void PrintCartPos() {
	double p[6] = { 0 };
	get_current_position(s, p);
	printf("Cart:");
	for (int a = 0; a < 6; a++) {
		printf("\t%.2f", p[a]);
	}
	printf("\n");
}
void PrintJointPos() {
	double p[6] = { 0 };
	get_current_joint(s, p);
	printf("Joint:");
	for (int a = 0; a < 6; a++) {
		printf("\t%.2f", p[a]);
	}
	printf("\n");
}

void PrintEnc() {
	int32_t p[6] = { 0 };
	get_encoder_count(s, p);
	printf("ENC:");
	for (int a = 0; a < 6; a++) {
		printf("\t%d", p[a]);
	}
}

void PrintTorq() {
	double p[6] = { 0 };
	get_motor_torque(s, p);
	printf("Torq:");
	for (int a = 0; a < 6; a++) {
		printf("\t%.2f", p[a]);
	}
}

void PrintMil() {
	double p[6] = { 0 };
	get_mileage(s, p);
	printf("Mil:");
	for (int a = 0; a < 6; a++) {
		printf("\t%.2f", p[a]);
	}
	printf("\n");

	get_total_mileage(s, p);
	printf("TMil:");
	for (int a = 0; a < 6; a++) {
		printf("\t%.2f", p[a]);
	}
}

void PrintHomePos() {
	double p[6] = { 0 };
	get_home_point(s, p);
	printf("Home:\t");
	for (int a = 0; a < 6; a++) {
		printf("%.2f\t", p[a]);
	}
}

void PrintPrePos() {
	double p[6] = { 0 };
	get_previous_pos(s, p);
	printf("Pre:\t");
	for (int a = 0; a < 6; a++) {
		printf("%.2f\t", p[a]);
	}
}

void PrintDIOSetting() {
	int D_setting[13] = { 0 };
	char text[100] = "";
	std::string DI_SI[2] = { "DI", "SI" };
	std::string DO_SO[2] = { "DO", "SO" };
	std::string msg[6] = { "Clear Error", "External Alarm", "System Shutdown", "Moter Warning", "System StartUp", "Mode Output" };
	get_digital_setting(s, D_setting, text);
	printf("DIO setting:\n");
	for (int i = 0; i < 12; i++) {
		if (i == 6) {
			printf("\n");
		}
		if (i % 2 == 0 && i < 6) {
			printf("%s  ", msg[(int)(i / 2)].c_str());
			printf("%s:", DI_SI[D_setting[i]].c_str());
		}
		else if (i % 2 == 0 && i >= 6) {
			printf("%s  ", msg[(int)(i / 2)].c_str());
			printf("%s:", DO_SO[D_setting[i]].c_str());
		}
		else {
			printf("%d,   ", D_setting[i]);
		}
	}
	printf("\n");
	printf("Text Length:%d,  Show Text: %s \n", D_setting[12], text);
}