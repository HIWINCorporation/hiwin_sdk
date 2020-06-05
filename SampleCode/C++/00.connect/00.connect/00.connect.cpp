#include "stdafx.h"
#include "iostream"
#include "windows.h"
#include "../../../../include/HRSDK.h"

#ifdef x64
#pragma comment(lib, "../../../../lib/x64/HRSDK.lib")
#else
#pragma comment(lib, "../../../../lib/x86/HRSDK.lib")
#endif

int Connect(HROBOT device_id, callback_function callback) {
	device_id = open_connection("127.0.0.1", 1, callback);
	if (device_id >= 0) {
		char* v = new char[256];
		get_hrsdk_version(v);
		std::cout << "get_HRSDK_version:" << v << std::endl;

		int level = get_connection_level(device_id);
		std::cout << "level:" << level << std::endl;

		set_connection_level(device_id, 1);
		level = get_connection_level(device_id);
		std::cout << "level:" << level << std::endl;
	}

	return 0;
}

int Disconnect(HROBOT device_id) {
	if (device_id >= 0) {
		disconnect(device_id);
	}
	return 0;
}

void __stdcall callBack(uint16_t, uint16_t, uint16_t*, int) {

}

int main() {
	HROBOT device_id = 1;
	Connect(device_id, callBack);
	Disconnect(device_id);
}

