#include "stdafx.h"
#include "iostream"
#include "windows.h"
#include "thread"
#include <conio.h>
#include "../../../../include/HRSDK.h"

#ifdef x64
#pragma comment(lib, "../../../../lib/x64/HRSDK.lib")
#else
#pragma comment(lib, "../../../../lib/x86/HRSDK.lib")
#endif

void PrintRPM();
void PrintCartPos();
void PrintJointPos();
void PrintEnc();
void PrintTorq();
void PrintMil();
void JogByKeyDown(HROBOT, char, int);
void WaitKeUp(HROBOT);

void __stdcall callBack(uint16_t, uint16_t, uint16_t*, int) {

}

void PrintFunction() {

}

int _tmain(int argc, _TCHAR* argv[]) {
	HROBOT id = open_connection("127.0.0.1", 1, callBack);

	if (get_motor_state(id) == 0) {
		set_motor_state(id, 1);   // Servo on
	}

	set_override_ratio(id, 100);
	std::thread printThread(PrintFunction);
	printThread.detach();
	while (true) {
		char keyNum = _getch();
		if (keyNum == 27) {
			break;
		}
		JogByKeyDown(id, keyNum, 0);
		WaitKeUp(id);
	}
	disconnect(id);
	return 0;
}


void JogByKeyDown(HROBOT id, char keyNum, int type) {

	switch (keyNum) {
	case 'Q':
	case 'q':
		jog(id, type, 0, 1);
		break;
	case 'W':
	case 'w':
		jog(id, type, 0, -1);
		break;
	case 'A':
	case 'a':
		jog(id, type, 1, 1);
		break;
	case 'S':
	case 's':
		jog(id, type, 1, -1);
		break;
	case 'Z':
	case 'z':
		jog(id, type, 2, 1);
		break;
	case 'X':
	case 'x':
		jog(id, type, 2, -1);
		break;
	case 'E':
	case 'e':
		jog(id, type, 3, 1);
		break;
	case 'R':
	case 'r':
		jog(id, type, 3, -1);
		break;
	case 'D':
	case 'd':
		jog(id, type, 4, 1);
		break;
	case 'F':
	case 'f':
		jog(id, type, 4, -1);
		break;
	case 'C':
	case 'c':
		jog(id, type, 5, 1);
		break;
	case 'V':
	case 'v':
		jog(id, type, 5, -1);
		break;
	}
}

void WaitKeUp(HROBOT id) {

	while (GetAsyncKeyState(81)  < 0 || \
	        GetAsyncKeyState(87)  < 0 || \
	        GetAsyncKeyState(65)  < 0 || \
	        GetAsyncKeyState(83)  < 0 || \
	        GetAsyncKeyState(90)  < 0 || \
	        GetAsyncKeyState(88)  < 0 || \
	        GetAsyncKeyState(69)  < 0 || \
	        GetAsyncKeyState(82)  < 0 || \
	        GetAsyncKeyState(68)  < 0 || \
	        GetAsyncKeyState(70)  < 0 || \
	        GetAsyncKeyState(67)  < 0 || \
	        GetAsyncKeyState(86)  < 0) {
		Sleep(5);
	}
	jog_stop(id);
}
