
# HIWIN ROBOT SDK
The SDK version is 6477, make sure your HRSS version is 6472.
This package allows you to monitor and control HIWIN industrial robots.

## Prerequisites:

Microsoft Visual Studio

## Run:

You can open HIWIN Visual Studio project file then run the sample code, directly.
If an error occurs in the file path or project property.
Please modify the pre-build event and follow the steps below to set up the properties.

1. Copy the include files into your project.
2. Copy the .dll file to your output directory.
3. Set the linker input if your project is C++.
4. Run the SampleCode with the vistual studio, and make sure files are in the right path.
5. If you have setup problems, please refer to the HIWIN Robot Software Deveelopment Kit.

## Related documents:

https://www.hiwin.tw/support/mar/rs405/rs405_400_200_LU.aspx

## Project description:

* Each of projects does support for both of x86 and x64.
* The detailed description of API is in Software_Development_Kit.pdf

#### 00.Connect
Simple code to connect and disconnect with the HRSS robot.

#### 01.Get_robot_info
After the SDK and HRSS are connected, use commands to obtain the robot's system information and IO status.

#### 02.Set_robot_parameter
After the SDK and HRSS are connected, use commands to set the robot's system information and IO status.

#### 03.Jog
Press the customize button keys to JOG the robot.
This command is useful for fine-tuning the position of TCP (Tool Center Point).

#### 04.PTPmotion
This program runs PTP (point to point) of customized position. 
The PTP motion move to destination based on the shortest path.
The motion commands include absolute position and relative position, and each has the type of Cartesian coordinates and joint coordinates.
In addition, the robot can also move by obtaining the data stored in the position register.

#### 05.LINmotion
This program runs LIN (line) of customized position. 
The LIN motion move to destination based on the shortest path.
The motion commands include absolute position and relative position, and each has the type of Cartesian coordinates and joint coordinates.
In addition, the robot can also move by obtaining the data stored in the position register.

#### 06.CIRCmotion
This program runs CIRC of customized position. 
The CIRC motion move to destination based on the circular path.
The motion commands include absolute position and relative position.
In addition, the robot can also move by obtaining the data stored in the position register.

#### 07.Motion
When the motion is in progress, you can give command to control the robot. 
Those commands include pause, continue and abort.

#### 08.File
Users can upload HRB robot program files from the local computer, and start or abort the execution of HRB task files.

#### 09.SyncOutput 
The robot changes the value of the digital output during the movement.

#### 10.TaskStart
The robot cyclically executes four task files. 
Which print the messages of task by callback notification.

#### 11.SoftLimit
The user sets the coordinate limit line.
When the robot moves beyond the limit, it will stop motion immediately. 
And, print the alarm messages by callback notification.

#### 12.NetWork
This project is an offline version of the robot network communication example. 
If you want to use the online version, please make sure to connect to the correct IP address.
The robot sends a message to the local socket, which resends the received message to the robot.
Callback notification mechanism print message from the local socket.
And, robot sets the message data to the value of counters.

## Licence

All the files included in this directory are under the BSD 3 Clause Licence. A copy of the licence is included in this folder.
