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

void wait_for_stop(int device_id) {
	while (get_motion_state(device_id) != 1 && get_connection_level(device_id) != -1) {
		Sleep(30);
	}
}

int SoftLimitExample(HROBOT device_id, callback_function callback) {
	device_id = open_connection("127.0.0.1", 1, callback);
	if (device_id >= 0) {
		char* v = new char[256];
		get_hrsdk_version(v);
		std::cout << "get_HRSDK_version:" << v << std::endl;
		set_motor_state(device_id, 1);

		double joint_low_limit[6] = { -20, -20, -35, -20, 0, 0 };
		double joint_high_limit[6] = { 20, 20, 0, 0, 0, 0 };
		double cart_low_limit[6] = { -100, 300, -100, 0, 0, 0 };
		double cart_high_limit[6] = { 100, 450, -25, 0, 0, 0 };
		double cart_home[6] = { 0, 400, 0, 0, -90, 0 };
		double joint_home[6] = { 0, 0, 0, 0, -90, 0 };
		double now_pos[6] = { 0 };
		bool re_bool = false;
		get_current_position(device_id, now_pos);

		// run joint softlimit
		set_override_ratio(device_id, 100);
		set_joint_soft_limit(device_id, joint_low_limit, joint_high_limit);
		enable_joint_soft_limit(device_id, true);
		enable_cart_soft_limit(device_id, false);
		get_joint_soft_limit_config(device_id, re_bool, joint_low_limit, joint_high_limit);
		std::cout << "Enable Joint SoftLimit: " << re_bool << std::endl;
		jog_home(device_id);
		wait_for_stop(device_id);
		Sleep(1000);
		for (int i = 0; i < 4; i++) {
			jog(device_id, 1, i, -1);
			wait_for_stop(device_id);
			std::cout <<  "On the limits of SoftLimit" << std::endl;
		}
		for (int i = 0; i < 4; i++) {
			jog(device_id, 1, i, 1);
			wait_for_stop(device_id);
			std::cout << "On the limits of SoftLimit" << std::endl;
		}
		enable_joint_soft_limit(device_id, false);

		// run cartesian softlimit
		ptp_axis(device_id, 0, joint_home);
		wait_for_stop(device_id);
		set_joint_soft_limit(device_id, cart_low_limit, cart_high_limit);
		enable_cart_soft_limit(device_id, true);
		get_joint_soft_limit_config(device_id, re_bool, cart_low_limit, cart_high_limit);
		std::cout << "Enable Cart SoftLimit: " << re_bool << std::endl;
		lin_pos(device_id, 0, 0, cart_home);
		wait_for_stop(device_id);
		for (int i = 0; i < 3; i++) {
			jog(device_id, 0, i, -1);
			wait_for_stop(device_id);
			std::cout << "On the limits of SoftLimit" << std::endl << std::endl;
			clear_alarm(device_id);
			Sleep(2000);
		}
		for (int i = 0; i < 3; i++) {
			jog(device_id, 0, i, 1);
			wait_for_stop(device_id);
			std::cout << "On the limits of SoftLimit" << std::endl << std::endl;
			clear_alarm(device_id);
			Sleep(2000);
		}
		enable_joint_soft_limit(device_id, false);
		enable_cart_soft_limit(device_id, false);

		std::cout << "End of motion " << std::endl;
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
	if (cmd == 0 && rlt == 4030 && len > 1) {
		std::cout << "[ALARM NOTIFY]: " << recv << std::endl;
	}
	delete recv;
}

int main() {
	HROBOT device_id = 1;
	SoftLimitExample(device_id, callBack);
	Disconnect(device_id);
}
