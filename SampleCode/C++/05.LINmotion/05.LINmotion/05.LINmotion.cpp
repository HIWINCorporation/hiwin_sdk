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
	HROBOT device_id = open_connection("127.0.0.1", 1, callBack);
	set_override_ratio(device_id, 100);

	if (get_motor_state(device_id) == 0) {
		set_motor_state(device_id, 1);   // Servo on
		set_override_ratio(device_id, 50);
		Sleep(3000);
	}

	const int pointNum = 8;
	int pointIdx = 0;
	double x[pointNum] = { 0 };
	double y[pointNum] = { 0 };
	double z[pointNum] = { 0 };
	int xoffset = 10;
	int zoffset = 5;
	double pos[6] = { 0, 450, 200, 180, 0, 90 };

	// make path
	x[pointIdx] = pos[0];
	y[pointIdx] = pos[1];
	z[pointIdx] = pos[2];
	pointIdx++;

	x[pointIdx] = pos[0] + xoffset;
	y[pointIdx] = pos[1];
	z[pointIdx] = pos[2];
	pointIdx++;

	x[pointIdx] = pos[0] + xoffset;
	y[pointIdx] = pos[1];
	z[pointIdx] = pos[2] - zoffset;
	pointIdx++;

	x[pointIdx] = pos[0] + xoffset;
	y[pointIdx] = pos[1];
	z[pointIdx] = pos[2];
	pointIdx++;

	x[pointIdx] = pos[0] - xoffset;
	y[pointIdx] = pos[1];
	z[pointIdx] = pos[2];
	pointIdx++;

	x[pointIdx] = pos[0] - xoffset;
	y[pointIdx] = pos[1];
	z[pointIdx] = pos[2] - zoffset;
	pointIdx++;

	x[pointIdx] = pos[0] - xoffset;
	y[pointIdx] = pos[1];
	z[pointIdx] = pos[2];
	pointIdx++;

	x[pointIdx] = pos[0];
	y[pointIdx] = pos[1];
	z[pointIdx] = pos[2];

	// ptp motion
	for (int a = 0; a < pointNum; a++) {
		pos[0] = x[a];
		pos[1] = y[a];
		pos[2] = z[a];
		while (get_command_count(device_id) > 100) {
			Sleep(500);
		}
		lin_pos(device_id, 2, 20, pos);
	}
	Sleep(1000);
	// axis
	double pos_home[6] = { 0, 0, 0, 0, -90, 0 };
	lin_axis(device_id, 2, 20, pos_home);

	// rel_pos
	Sleep(1000);
	double pos1[6] = { 10, 0, 0, 10, 0, 0 };
	lin_rel_pos(device_id, 2, 50, pos1);
	// rel_axis
	Sleep(1000);
	double pos2[6] = { -10, 0, 0, 0, 10, 0 };
	lin_rel_axis(device_id, 2, 10, pos2);
	// rel_pr
	double AxishomePoint[6] = { 0, 0, 0, 0, -90, 0 };
	set_pr(device_id, 100, 1, AxishomePoint, 0, 0);
	lin_pr(device_id, 2, 50, 1);
	disconnect(device_id);
	return 0;
}

