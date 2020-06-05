// 08.File.cpp : 此檔案包含 'main' 函式。程式會於該處開始執行及結束執行。
//

#include "pch.h"
#include <iostream>
#include "stdafx.h"
#include "../../../../include/HRSDK.h"
#include <windows.h>

#ifdef x64
#pragma comment(lib, "../../../../lib/x64/HRSDK.lib")
#else
#pragma comment(lib, "../../../../lib/x86/HRSDK.lib")
#endif

void __stdcall callBack(uint16_t, uint16_t, uint16_t*, int) {

}
int main(int argc, _TCHAR* argv[])
{
	HROBOT device_id = open_connection("127.0.0.1", 1, callBack);
	if (device_id >= 0) {
		double Home[6] = { 0, 0, 0, 0, -90, 0 };
		set_override_ratio(device_id, 60);	// override

		if (get_motor_state(device_id) == 0) {
			set_motor_state(device_id, 1);   // Servo on
		}
		Sleep(200);
		ext_task_start(device_id, 0, 1);
		std::string str = "test.hrb";
		char *file_name = new char[str.length() + 1];
		strcpy_s(file_name, str.length() + 1, str.c_str());
		task_start(device_id, file_name);
		Sleep(2000);
		char* name = new char[50];
		get_execute_file_name(device_id, name);
		printf("%c", name);

		Sleep(2000);
		task_hold(device_id);
		Sleep(2000);
		task_continue(device_id);
		Sleep(2000);
		task_abort(device_id);

		str = "test2.hrb";
		strcpy_s(file_name, str.length() + 1, str.c_str());
		send_file(device_id,  file_name, file_name);
		Sleep(2000); // wait for sending file to finish, download_file and send_file can not execute at the same time
		char *from_file_path = new char[str.length() + 1];
		strcpy_s(from_file_path, str.length() + 1, str.c_str());
		char *to_file_path = new char[str.length() + 1];
		strcpy_s(to_file_path, str.length() + 1, str.c_str());
		download_file(device_id, from_file_path, to_file_path);


		int cnt = -1;
		uint64_t* alarm = new uint64_t[20];
		get_alarm_code(device_id, cnt, alarm);
		if (cnt > 0) {
			clear_alarm(device_id);
		}

		char* update_file = "NULL";
		update_hrss(device_id, update_file);

		new_folder(device_id, "new_folder");
		file_rename(device_id, "test2.hrb", "test3.hrb");
		file_drag(device_id, "test3.hrb", "new_folder/test3.hrb");
		delete_file(device_id, "new_folder/test3.hrb");
		delete_folder(device_id, "new_folder");
		for (int i = 0; i < get_prog_number(device_id); i++) {
			char* str2 = new char[100];
			get_prog_name(device_id, i, str2);
			printf("%s \n", str2);
		}
		system("pause");
	}
	disconnect(device_id);
	return 0;
}

