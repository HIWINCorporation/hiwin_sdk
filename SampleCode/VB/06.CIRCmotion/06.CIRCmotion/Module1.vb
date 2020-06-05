Imports System.Threading

Module Module1
    Private Sub EventFun(command As UInt16, result As UInt16, ByRef msg As UInt16, length As Integer)
    End Sub
    Public callback As HRobot.CallBackFun
    Public device_id As Integer
    Sub Main()
        callback = New HRobot.CallBackFun(AddressOf EventFun)
        device_id = HRobot.open_connection("127.0.0.1", 1, callback)
        CIRCmotion(device_id, callback)
        Console.ReadLine()
        HRobot.disconnect(device_id)
    End Sub
    Sub CIRCmotion(device_id As Integer, callback As HRobot.CallBackFun)
        If (device_id >= 0) Then
            Console.WriteLine("connect successful.")
            HRobot.set_override_ratio(device_id, 100)
        Else
            Console.WriteLine("connect failure.")
        End If

        If HRobot.get_motor_state(device_id) Then
            HRobot.set_motor_state(device_id, 1)
            HRobot.set_override_ratio(device_id, 50)
            Thread.Sleep(3000)
        End If

        Dim cp1() As Double = {100, 368, 200, 180, 0, 90}
        Dim cp2() As Double = {0, 368, 100, 180, 0, 90}
        Dim cp3() As Double = {-100, 368, 0, 180, 0, 90}
        Dim cp4() As Double = {0, 368, -100, 180, 0, 90}
        Dim cp5() As Double = {100, 368, 0, 180, 0, 90}
        Dim cp6() As Double = {0, 368, 100, 180, 0, 90}
        Dim cp7() As Double = {-100, 368, 200, 180, 0, 90}
        Dim cp8() As Double = {0, 368, 293.5, 180, 0, 90}
        Dim Home() As Double = {0, 0, 0, 0, -90, 0}

        HRobot.set_override_ratio(device_id, 60)
        HRobot.ptp_axis(device_id, 0, Home)

        HRobot.circ_pos(device_id, 1, cp1, cp2)
        Thread.Sleep(2000)
        HRobot.circ_pos(device_id, 1, cp3, cp4)
        Thread.Sleep(2000)
        HRobot.circ_axis(device_id, 1, cp5, cp6)
        Thread.Sleep(2000)
        HRobot.circ_axis(device_id, 1, cp7, cp8)
        Thread.Sleep(2000)
        'HRobot.circ_pr(device_id, 1, 1, 2)

    End Sub


End Module
