
#include <stdio.h>
#include "pch.h"
#include "iostream"
#include "windows.h"
#include "string"
#include "vector"
#include "../../../../include/HRSDK.h"
#include <thread>
#pragma comment(lib, "ws2_32.lib")

#ifdef x64
#pragma comment(lib, "../../../../lib/x64/HRSDK.lib")
#else
#pragma comment(lib, "../../../../lib/x86/HRSDK.lib")
#endif

bool is_received = false;
std::vector<std::string> result_val;
std::vector<std::string> string_split(std::string str, std::string token);
void socket_server();
void socket_client();

int Disconnect(HROBOT device_id) {
	if (device_id >= 0) {
		disconnect(device_id);
	}
	return 0;
}

void __stdcall callBack(uint16_t cmd, uint16_t rlt, uint16_t* Msg, int len) {
	if (len > 1) {
		if (cmd == 0 && rlt == 4034) {
			char* recv = new char[len];
			int p = (int)recv;
			char* ptr_bracket;
			char* ptr_separator;
			char* ptr;
			for (int i = 0; i < len; i++) {
				recv[i] = (char)Msg[i];
			}

			std::vector<std::string> result;
			result_val.clear();
			result = string_split(std::string(recv), "&&&");  // splitting received messages
			for (int i = 0; i < result.size(); i++) {
				if (result[i].find("Read") == -1) {
					continue;
				}
				int data_begin = result[i].find('{') + 1;
				int data_len = result[i].find('}') - result[i].find('{') - 1;

				std::string str2 = result[i].substr(data_begin, data_len);
				result_val = string_split(std::string(str2), ",");  // splitting parameter values
				is_received = true;
				printf("[NETWORK NOTIFY] %s  \n", result[i].c_str());
			}
			delete recv;
		}
	}
}

// If you want to get the network information, you does have this function.
// The received parameter is set to the value of the counter.
void run_notify(HROBOT device_id)
{
	printf("Strat Notify \n");
	double pos[6] = { 0 };
	get_current_position(device_id, pos);
	while (true) {
		network_get_state(device_id);
		if (is_received) {
			for (int i = 1; i <= result_val.size(); i++) {
				set_counter(device_id, i, atoi(result_val[i - 1].c_str()));
			}
			is_received = false;
		}
		Sleep(200);
	}
}

// Split string based on token.
std::vector<std::string> string_split(std::string str, std::string token) {
	std::vector<std::string>result;
	while (str.size()) {
		int index = str.find(token);
		if (index != std::string::npos) {
			result.push_back(str.substr(0, index));
			str = str.substr(index + token.size());
			if (str.size() == 0) {
				result.push_back(str);
			}
		} else {
			result.push_back(str);
			str = "";
		}
	}
	return result;
}

// HRSS send the message to another local socket.
void send_msg(HROBOT device_id) {
	int x = 0;
	printf("The received data is set as counter value.\n");
	while (true) {
		std::string str;
		std::string str_num1 = std::to_string(x);
		std::string str_num2 = std::to_string(x + 1);
		std::string str_num3 = std::to_string(x + 2);
		str = str_num1 + "," + str_num2 + "," + str_num3 + "%%%";
		char * send_msg = new char[str.size() + 1];
		strcpy_s(send_msg, str.size() + 1, str.c_str());
		network_send_msg(device_id, send_msg);
		delete[] send_msg;
		Sleep(1000);
		if (x == 10) {
			network_disconnect(device_id);
			break;
		}
		x++;
	}
}

// HRSS is socket client
int  NetWorkExample_Client(HROBOT device_id) {
	int client_type = 1; // socket client
	char* ip = "127.0.0.1";
	int port = 5000;
	int bracket = 0;
	int separator = 0;
	bool is_format = false;
	int show_msg = -1;

	// create local socket server
	std::thread create_server(socket_server);
	create_server.detach();
	Sleep(2000);

	// network setting
	set_network_config(device_id, client_type, ip, port, bracket, separator, is_format);
	network_connect(device_id);
	get_network_show_msg(device_id, show_msg);
	if (!show_msg) {
		set_network_show_msg(device_id, true);
	}
	printf("HRSS is socket client. \n");

	send_msg(device_id);

	return 0;
}

// HRSS is socket server
int  NetWorkExample_Server(HROBOT device_id) {
	int server_type = 0; // socket server
	char* ip = "127.0.0.1";
	int port = 5123;
	int bracket = 0;  // {}
	int separator = 0;  // ,
	bool is_format = false;
	int show_msg = -1;

	// network setting
	set_network_config(device_id, server_type, ip, port, bracket, separator, is_format);
	network_connect(device_id);
	get_network_show_msg(device_id, show_msg);
	if (!show_msg) {
		set_network_show_msg(device_id, true);
	}
	printf("HRSS is socket server. \n");
	Sleep(2000);

	// create local socket client
	std::thread create_client(socket_client);
	create_client.detach();

	send_msg(device_id);

	return 0;
}

int main()
{
	HROBOT device_id = 1;
	char* ver = new char[256];

	device_id = open_connection("127.0.0.1", 1, callBack);
	if (device_id >= 0) {
		printf("Success connection \n");
		std::thread notify(run_notify, device_id);
		notify.detach();

		get_hrsdk_version(ver);
		std::cout << "HRSDK version: " << ver << std::endl;
		set_motor_state(device_id, 1);

		NetWorkExample_Client(device_id); // HRSS is socket client
		Sleep(2000);
		printf("\n");
		NetWorkExample_Server(device_id); // HRSS is socket server

		Sleep(3000);
		std::cout << " \n Please press enter key to end." << std::endl;

		std::cin.get();
		Disconnect(device_id);
	}
}

// This is a local socket server.
// If you want to connect the other HIWIN robot, you can delete this socket function.
void socket_server()
{
	SOCKET sListen;
	SOCKET sAccept;
	sockaddr_in sockaddrServer;
	SOCKADDR_IN client;
	char szMessage[100];
	int ret;
	int iaddrSize = sizeof(SOCKADDR_IN);
	// Create a TCP/IP socket.
	sListen = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (sListen == INVALID_SOCKET) {
		printf("Error creating socket, ec: %d\n", WSAGetLastError());
	}

	sockaddrServer.sin_family = AF_INET;
	sockaddrServer.sin_port = htons(5000);
	sockaddrServer.sin_addr.s_addr = inet_addr("127.0.0.1");
	// Bind the socket to the local endpoint and listen for incoming connections.
	bind(sListen, (struct sockaddr *) &sockaddrServer, sizeof(SOCKADDR_IN));
	listen(sListen, 1);

	fd_set oRead, oWrite;
	// nonblocking connect
	TIMEVAL Timeout;
	Timeout.tv_sec = 1;
	Timeout.tv_usec = 0;

	FD_ZERO(&oRead);
	FD_ZERO(&oWrite);
	FD_SET(sListen, &oRead);
	FD_SET(sListen, &oWrite);
	int nEnd = 0;
	printf("Create a local socket server. \n");
	select(sListen, &oRead, &oWrite, NULL, &Timeout);
	sAccept = accept(sListen, (struct sockaddr*)&client, &iaddrSize);
	// When socket received message, it will send that data to sender.
	while (true) {
		char recv_data[50];
		int nLen = recv(sAccept, recv_data, sizeof(recv_data), 0);
		if (nLen == 0) {
			break;
		}
		recv_data[nLen] = '\0';
		send(sAccept, recv_data, strlen(recv_data), 0);
		Sleep(50);
	}
	closesocket(sAccept);
	closesocket(sListen);
	printf("Local socket server is closed. \n");
}

// This is a local socket client.
// If you want to connect the other HIWIN robot, you can delete this socket function.
void socket_client()
{
	SOCKET sListen;
	SOCKET sClient;
	sockaddr_in sockaddrServer;
	char szMessage[100];
	int ret;
	int iaddrSize = sizeof(SOCKADDR_IN);
	// Create a TCP/IP socket.
	sListen = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (sListen == INVALID_SOCKET) {
		printf("Error creating socket, ec: %d\n", WSAGetLastError());
	}

	sockaddrServer.sin_family = AF_INET;
	sockaddrServer.sin_port = htons(5123);
	sockaddrServer.sin_addr.s_addr = inet_addr("127.0.0.1");
	int err = connect(sListen, (struct sockaddr *)&sockaddrServer, sizeof(sockaddrServer));
	if (err == -1) {
		printf("Connection error");
	}
	printf("Create a local socket client. \n");
	while (true) {
		char recv_data[50];
		int nLen = recv(sListen, recv_data, sizeof(recv_data), 0);
		recv_data[nLen] = '\0';
		send(sListen, recv_data, strlen(recv_data), 0);
		Sleep(50);
		if (nLen == 0) {
			break;
		}
	}
	closesocket(sListen);
	printf("Local socket client is closed. \n");
}
