Imports System.Text
Imports System.Threading

Module Module1
    Private Sub EventFun(command As UInt16, result As UInt16, ByRef msg As UInt16, length As Integer)
    End Sub
    Public callback As HRobot.CallBackFun
    Public device_id As Integer

    Sub Main()
        callback = New HRobot.CallBackFun(AddressOf EventFun)
        device_id = HRobot.open_connection("127.0.0.1", 1, callback)
        File(device_id, callback)
        Console.ReadLine()
        HRobot.disconnect(device_id)
    End Sub
    Sub File(device_id As Integer, callback As HRobot.CallBackFun)
        If (device_id >= 0) Then
            Console.WriteLine("connect successful.")
            HRobot.set_override_ratio(device_id, 100)
        Else
            Console.WriteLine("connect failure.")
        End If

        If HRobot.get_motor_state(device_id) Then
            HRobot.set_motor_state(device_id, 1)
        End If

        HRobot.ext_task_start(device_id, 0, 1)
        HRobot.task_start(device_id, "test.hrb")
        Thread.Sleep(2000)
        HRobot.task_hold(device_id)
        HRobot.task_continue(device_id)
        Thread.Sleep(2000)
        HRobot.task_abort(device_id)

        Thread.Sleep(2000)
        Dim from_file_path As String = "test.hrb"
        Dim to_file_path As String = "Program\\test.hrb"
        HRobot.download_file(device_id, from_file_path, to_file_path)
        Thread.Sleep(2000) ' wait for downloading file to finish, download_file and send_file can not execute at the same time
        Dim file_name As String = "test.hrb"
        Dim file_path As String = "Program\\"
        HRobot.send_file(device_id, file_name, file_name)
        Thread.Sleep(2000)
        Dim cnt As Integer = -1
        Dim alarm(20) As ULong
        HRobot.get_alarm_code(device_id, cnt, alarm)
        If (cnt > 0) Then
            HRobot.clear_alarm(device_id)
        End If
        Thread.Sleep(2000)
        Dim update As String = "update"
        HRobot.update_hrss(device_id, update)


        HRobot.new_folder(device_id, "new_folder")
        HRobot.file_rename(device_id, "test2.hrb", "test3.hrb")
        HRobot.file_drag(device_id, "test3.hrb", "new_folder/test3.hrb")
        HRobot.delete_file(device_id, "new_folder/test3.hrb")
        HRobot.delete_folder(device_id, "new_folder")
        For i As Integer = 0 To HRobot.get_prog_number(device_id)
            Dim str2 As StringBuilder = New StringBuilder(50)
            HRobot.get_prog_name(device_id, i, str2)
            Console.WriteLine(str2)
        Next
    End Sub
End Module
