

#include "pch.h"
#include <iostream>
#include "../../../../include/HRSDK.h"
#include <windows.h>

#ifdef x64
#pragma comment(lib, "../../../../lib/x64/HRSDK.lib")
#else
#pragma comment(lib, "../../../../lib/x86/HRSDK.lib")
#endif

void SyncOutputFunc(int);
void __stdcall callBack(uint16_t, uint16_t, uint16_t*, int) {

}
int main()
{
	HROBOT device_id = open_connection("127.0.0.1", 1, callBack);
	set_motor_state(device_id, 1);
	if (device_id >= 0) {
		SyncOutputFunc(device_id);
	}
	std::cin.get();
	disconnect(device_id);
	return 0;
}

void SyncOutputFunc(int device_id) {
	int type = 0;  //DO
	int id = 1;
	int ON = 1;
	int OFF = 0;
	int delay = 1000;
	int distance = 50;
	int Start = 0;
	int End = 1;
	int Path = 2;

	jog_home(device_id);
	Sleep(100);
	double p1[6] = { -10, -200, 0, 0, 0, 0 };
	double p2[6] = { 10, 150, 0, 0, 0, 0 };
	lin_rel_pos(device_id, 0, 0, p1);
	SyncOutput(device_id, type, id, ON, Start, delay, distance);
	SyncOutput(device_id, type, 2, ON, Path, -1000, distance);
	SyncOutput(device_id, type, 3, ON, Path, 0, distance);
	SyncOutput(device_id, type, 4, ON, Path, delay, distance);
	SyncOutput(device_id, type, 5, ON, Path, -1000, -50);
	SyncOutput(device_id, type, 6, ON, Path, 0, -50);
	SyncOutput(device_id, type, 7, ON, Path, 1000, -50);
	SyncOutput(device_id, type, 8, ON, End, -1000, distance);
	lin_rel_pos(device_id, 0, 0, p2);
}
