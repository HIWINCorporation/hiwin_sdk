Imports System.Text
Imports System.Threading


Module Module1
    Dim x As Integer = 0
    Dim is_Run As Boolean = False
    Dim callback As HRobot.CallBackFun
    Dim device_id As Integer
    Dim aTimer As Boolean

    Private Sub EventFun(command As UInt16, result As UInt16, ByRef msg As UInt16, length As Integer)
        If (command = 4001) Then
            Console.WriteLine("Command: " & command & "  Result: " & result)
            Select Case result
                Case 4012
                    Console.WriteLine("task_start HRSS_TASK_NAME_ERR")
                Case 4013
                    Console.WriteLine("task_start Alaeady exist.")
                Case 4014
                    Console.WriteLine("task_start success. Program starts to Run.")
                    aTimer = True
                    x = x + 1

            End Select

        End If

    End Sub



    Sub Main()
        callback = New HRobot.CallBackFun(AddressOf EventFun)
        device_id = HRobot.open_connection("127.0.0.1", 1, callback)
        TaskStartExample(device_id, callback)
        Console.ReadLine()
        HRobot.disconnect(device_id)
    End Sub


    Sub TaskStartExample(device_id As Integer, callback As HRobot.CallBackFun)
        If (device_id >= 0) Then
            Console.WriteLine("connect successful.")
        Else
            Console.WriteLine("connect failure.")
        End If
        Dim pos(6) As Double
        Dim ver As StringBuilder = New StringBuilder(256)
        HRobot.get_hrsdk_version(ver)
        Console.WriteLine("get_HRSDK_version: " & ver.ToString())
        HRobot.set_motor_state(device_id, 1)

        Console.WriteLine("send_file0: " & HRobot.send_file(device_id, "../../../code0.hrb", "code0.hrb"))
        Thread.Sleep(500)
        Console.WriteLine("send_file1: " & HRobot.send_file(device_id, "../../../code1.hrb", "code1.hrb"))
        Thread.Sleep(500)
        Console.WriteLine("send_file2: " & HRobot.send_file(device_id, "../../../code2.hrb", "code2.hrb"))
        Thread.Sleep(500)
        Console.WriteLine("send_file3: " & HRobot.send_file(device_id, "../../../code3.hrb", "code3.hrb"))
        Thread.Sleep(500)

        HRobot.get_current_position(device_id, pos) 'run callback.
        While (True)
            If (Not is_Run) Then
                Select Case (x Mod 4)
                    Case 0
                        HRobot.task_start(device_id, "code0.hrb")
                    Case 1
                        HRobot.task_start(device_id, "code1.hrb")
                    Case 2
                        HRobot.task_start(device_id, "code2.hrb")
                    Case 3
                        HRobot.task_start(device_id, "code3.hrb")
                End Select
                Console.WriteLine("run code{0}.hrb", x Mod 4)
                Console.WriteLine(is_Run & aTimer)
                is_Run = True
            End If
            If (aTimer) Then
                If (get_function_output(device_id, 1) = 0) Then
                    Console.WriteLine("task_start finish. ")
                    Console.WriteLine("")
                    is_Run = False
                    aTimer = False
                End If
            End If
            Thread.Sleep(1000)

        End While
    End Sub
End Module
