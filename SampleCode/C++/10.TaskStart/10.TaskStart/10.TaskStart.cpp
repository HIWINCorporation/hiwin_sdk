// 10.TaskStart.cpp : 此檔案包含 'main' 函式。程式會於該處開始執行及結束執行。
//

#include "pch.h"
#include "iostream"
#include "windows.h"
#include "../../../../include/HRSDK.h"
#include <thread>

#ifdef x64
#pragma comment(lib, "../../../../lib/x64/HRSDK.lib")
#else
#pragma comment(lib, "../../../../lib/x86/HRSDK.lib")
#endif

int x = 0;
bool is_Run = false;
bool aTimer = false;

int TaskStart(HROBOT device_id, callback_function callback) {
	device_id = open_connection("127.0.0.1", 1, callback);
	if (device_id >= 0) {
		char* v = new char[256];
		get_hrsdk_version(v);
		std::cout << "get_HRSDK_version:" << v << std::endl;
		set_motor_state(device_id, 1);

		std::cout << "send_file0: " << send_file(device_id, "code0.hrb", "code0.hrb") << std::endl;
		Sleep(500);
		std::cout << "send_file1: " << send_file(device_id, "code1.hrb", "code1.hrb") << std::endl;
		Sleep(500);
		std::cout << "send_file2: " << send_file(device_id, "code2.hrb", "code2.hrb") << std::endl;
		Sleep(500);
		std::cout << "send_file3: " << send_file(device_id, "code3.hrb", "code3.hrb") << std::endl;
		Sleep(500);
		double pos[6] = {0};
		get_current_position(device_id, pos); // run callback.
		while (true) {
			if (!is_Run) {
				switch (x % 4) {
				case 0:
					task_start(device_id, "code0.hrb");
					break;
				case 1:
					task_start(device_id, "code1.hrb");
					break;
				case 2:
					task_start(device_id, "code2.hrb");
					break;
				case 3:
					task_start(device_id, "code3.hrb");
					break;
				}
				printf("run code%d.hrb \n", x % 4);
				is_Run = true;
			}
			if (aTimer) {
				if (get_function_output(device_id, 1) == 0) {
					printf("task_start finish. \n\n");
					is_Run = false;
					aTimer = false;
				}
			}
			Sleep(1000);
		}
		//std::cin.get();
	}
	return 0;
}

int Disconnect(HROBOT device_id) {
	if (device_id >= 0) {
		disconnect(device_id);
	}
	return 0;
}

void __stdcall callBack(uint16_t cmd, uint16_t rlt, uint16_t* Msg, int len) {
	char* recv = new char[len];
	for (int i = 0; i < len; i++) {
		recv[i] = (char)Msg[i];
	}
	std::string info_p(recv);
	if (cmd == 4001) {
		std::cout << "Command: " << cmd << "  Result: " << rlt << "  Msg:  " << recv << std::endl;
		switch (rlt) {
		case 4012:
			printf("task_start HRSS_TASK_NAME_ERR\n");
			break;
		case 4013:
			printf("task_start Alaeady exist.\n");
			break;
		case 4014:
			printf("task_start success. Program starts to Run.\n");
			aTimer = true;
			x++;
			break;
		}
	}
}

int main() {
	HROBOT device_id = 1;
	TaskStart(device_id, callBack);
	Disconnect(device_id);
}
