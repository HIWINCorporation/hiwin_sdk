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
	double cp1[6] = { 100, 368, 200, 180, 0, 90 };
	double cp2[6] = { 0, 368, 100, 180, 0, 90 };
	double cp3[6] = { -100, 368, 0, 180, 0, 90 };
	double cp4[6] = { 0, 368, -100, 180, 0, 90 };
	double cp5[6] = { 100, 368, 0, 180, 0, 90 };
	double cp6[6] = { 0, 368, 100, 180, 0, 90 };
	double cp7[6] = { -100, 368, 200, 180, 0, 90 };
	double cp8[6] = { 0, 368, 293.5, 180, 0, 90 };
	double Home[6] = { 0, 0, 0, 0, -90, 0 };
	set_override_ratio(device_id, 60);	// override

	if (get_motor_state(device_id) == 0) {
		set_motor_state(device_id, 1);   // Servo on
		set_override_ratio(device_id, 50);
		Sleep(3000);
	}

	ptp_axis(device_id, 0, Home);	// ptp to axis home
	circ_pos(device_id, 1, cp1, cp2);  // circ motion
	Sleep(2000);
	circ_pos(device_id, 1, cp3, cp4);
	Sleep(2000);
	circ_axis(device_id, 1, cp5, cp6);
	Sleep(2000);
	circ_axis(device_id, 1, cp7, cp8);
	Sleep(2000);
	// circ_pr(device_id, 1, 1, 2);
	disconnect(device_id);
	return 0;
}

