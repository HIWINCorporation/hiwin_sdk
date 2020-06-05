Imports System.Text

Module Module1
    Sub Main()
        Dim device_id As Integer
        device_id = HRobot.open_connection("127.0.0.1", 1, callback)
        Connect(device_id, callback)
        Disconnect(device_id, callback)
    End Sub
    Enum Command_
        NOTIFY = 0
        CLEAR_ALARM = 1450
        TASK_START = 4001
        UPDATE_HRSS = 4011
    End Enum
    Public Enum Result_
        ERROR_OPEN_FILE_FAIL = 4011
        HRSS_TASK_START_FINISH = 4014
        HRSS_UPDATE_FILE_ERROR = 4020
        HRSS_UPDATE_FILE_TRANSFER_ERROR = 4021
        HRSS_UPDATE_FILE_UNARCHIVER_ERROR = 4022
        HRSS_HARD_DISK_CAPACITY_IS_NOT_ENOUTH = 4023
        HRSS_START_UPDATE = 4026
        HRSS_UPDATE_FILE_TRANSFER_SUCCESS = 4027
        HRSS_START_CLEAR_ALARM = 4028
        HRSS_FINISH_CLEAR_ALARM = 4029
        HRSS_ALARM_NOTIFY = 4030
        HRSS_BATTERY_WARRING = 4031
        HRSS_BATTERY_ALARM = 4032
        HRSS_BATTERY_NORMAL = 4033
        HRSS_NETWORK_MSG_NOTIFY = 4034
        HRSS_RS232_MSG_NOTIFY = 4035
        HRSS_PROGRAM_LINE_NUMBER_NOTIFY = 4700
        HRSS_ROBOT_INFO_NOTIFY = 4702
    End Enum
    Private Sub EventFun(command As UInt16, result As UInt16, ByRef msg As UInt16, length As Integer)
        Select Case command
            Case Command_.NOTIFY
                Select Case result
                    Case Result_.HRSS_ALARM_NOTIFY
                        Console.Write("[Notify] Alarm:")
                        For i As Integer = 0 To length - 1
                            Console.Write(Asc(msg.ToString()))
                        Next i
                        Console.WriteLine()
                    Case Result_.HRSS_BATTERY_WARRING
                        Console.WriteLine("[Notify] Battery Status: Warning")
                    Case Result_.HRSS_BATTERY_ALARM
                        Console.WriteLine("[Notify] Battery Status: Alarm")
                    Case Result_.HRSS_BATTERY_NORMAL
                        Console.WriteLine("[Notify] Battery Status: Normal")
                    Case Else
                End Select
            Case Command_.CLEAR_ALARM
                Select Case result
                    Case Result_.HRSS_START_CLEAR_ALARM
                        Console.WriteLine("[Notify] Start to clean alarm.")
                    Case Result_.HRSS_FINISH_CLEAR_ALARM
                        Console.WriteLine("[Notify] Finish cleaning alarm.")
                    Case Else
                End Select
            Case Command_.UPDATE_HRSS
                Select Case result
                    Case Result_.HRSS_UPDATE_FILE_ERROR
                        Console.WriteLine("[Notify] Updated file error.")
                    Case Result_.HRSS_UPDATE_FILE_TRANSFER_ERROR
                        Console.WriteLine("[Notify] File transfer failure.")
                    Case Result_.HRSS_UPDATE_FILE_UNARCHIVER_ERROR
                        Console.WriteLine("[Notify] Updated file compression failure.")
                    Case Result_.HRSS_HARD_DISK_CAPACITY_IS_NOT_ENOUTH
                        Console.WriteLine("[Notify] Insufficient of controller space.")
                    Case Result_.HRSS_START_UPDATE
                        Console.WriteLine("[Notify] Start to update HRSS.")
                    Case Result_.HRSS_UPDATE_FILE_TRANSFER_SUCCESS
                        Console.WriteLine("[Notify] Upload update file success.")
                    Case Else
                        Console.WriteLine("[Notify] Offline can not be updated.")
                End Select
            Case Command_.TASK_START
                Select Case result
                    Case Result_.HRSS_TASK_START_FINISH
                        Console.WriteLine("[Notify] Task start finish.")
                    Case Result_.ERROR_OPEN_FILE_FAIL
                        Console.WriteLine("[Notify] Open file failed.")
                    Case Else
                End Select
            Case Else
        End Select
    End Sub
    Private callback As HRobot.CallBackFun = New HRobot.CallBackFun(AddressOf EventFun)
    Sub Connect(device_id As Integer, callback As HRobot.CallBackFun)
        open_connection(device_id, 1, callback)
        If (device_id >= 0) Then
            Console.WriteLine("connect successful.")

            Dim v As StringBuilder = New StringBuilder(100)
            HRobot.get_hrsdk_version(v)
            Console.WriteLine(v)

            Dim level As Integer = HRobot.get_connection_level(device_id)
            Console.WriteLine("level:" & level)

            HRobot.set_connection_level(device_id, 1)
            level = HRobot.get_connection_level(device_id)
            Console.WriteLine("level:" & level)

        Else
            Console.WriteLine("connect failure.")
        End If
        Console.ReadLine()
    End Sub

    Sub Disconnect(device_id As Integer, callback As HRobot.CallBackFun)
        If (device_id >= 0) Then
            HRobot.disconnect(device_id)
        End If
    End Sub

End Module
