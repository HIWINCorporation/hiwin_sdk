Imports System.Threading

Module Module1
    Private Sub EventFun(command As UInt16, result As UInt16, ByRef msg As UInt16, length As Integer)
    End Sub

    Public callback As HRobot.CallBackFun
    Public device_id As Integer
    Sub Main()
        callback = New HRobot.CallBackFun(AddressOf EventFun)
        device_id = HRobot.open_connection("127.0.0.1", 1, callback)
        LINmotion(device_id, callback)
        Console.ReadLine()
        HRobot.disconnect(device_id)
    End Sub
    Sub LINmotion(device_id As Integer, callback As HRobot.CallBackFun)
        If (device_id >= 0) Then
            Console.WriteLine("connect successful.")
            HRobot.set_override_ratio(device_id, 100)
        Else
            Console.WriteLine("connect failure.")
        End If

        If HRobot.get_motor_state(device_id) = 0 Then
            HRobot.set_motor_state(device_id, 1)
            HRobot.set_override_ratio(device_id, 50)
            Thread.Sleep(3000)
        End If

        Dim pos() As Double = {0, 450, 200, 180, -10, 90}
        Const pointNum As Integer = 8
        Dim pointIdx As Integer = 0
        Dim x(pointNum)
        Dim y(pointNum)
        Dim z(pointNum)
        Dim xoffset As Integer = 20
        Dim zoffset As Integer = 10


        x(pointIdx) = pos(0)
        y(pointIdx) = pos(1)
        z(pointIdx) = pos(2)
        pointIdx += 1

        x(pointIdx) = pos(0) + xoffset
        y(pointIdx) = pos(1)
        z(pointIdx) = pos(2)
        pointIdx += 1

        x(pointIdx) = pos(0) + xoffset
        y(pointIdx) = pos(1)
        z(pointIdx) = pos(2) - zoffset
        pointIdx += 1

        x(pointIdx) = pos(0) + xoffset
        y(pointIdx) = pos(1)
        z(pointIdx) = pos(2)
        pointIdx += 1

        x(pointIdx) = pos(0) - xoffset
        y(pointIdx) = pos(1)
        z(pointIdx) = pos(2)
        pointIdx += 1

        x(pointIdx) = pos(0) - xoffset
        y(pointIdx) = pos(1)
        z(pointIdx) = pos(2) - zoffset
        pointIdx += 1

        x(pointIdx) = pos(0) - xoffset
        y(pointIdx) = pos(1)
        z(pointIdx) = pos(2)
        pointIdx += 1

        x(pointIdx) = pos(0)
        y(pointIdx) = pos(1)
        z(pointIdx) = pos(2)

        'lin motion
        Dim a As Integer = 0
        For a = 0 To pointNum - 1
            pos(0) = x(a)
            pos(1) = y(a)
            pos(2) = z(a)
            While (HRobot.get_command_count(device_id) > 100)
                Thread.Sleep(500)
            End While
            HRobot.lin_pos(device_id, 1, 50, pos)
        Next
        Thread.Sleep(1000)

        Dim pos_home() As Double = {0, 0, 0, 0, -90, 0}
        HRobot.lin_axis(device_id, 1, 50, pos_home)

        'rel_pos
        Thread.Sleep(500)
        Dim pos1() As Double = {10, 20, 10, 20, 10, 20}
        HRobot.lin_rel_pos(device_id, 2, 50, pos1)
        'rel_axis
        Dim pos2() As Double = {-10, -20, -10, -20, -10, -20}
        Thread.Sleep(500)
        HRobot.lin_rel_axis(device_id, 2, 50, pos)

        HRobot.lin_pr(device_id, 2, 50, 1)

    End Sub
End Module
