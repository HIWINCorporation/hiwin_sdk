Imports System.Text
Imports System.Threading

Module Module1
    Public callback As HRobot.CallBackFun
    Public device_id As Integer
    Dim x As Integer = 0
    Dim is_Run As Boolean = False
    Dim aTimer As Boolean

    Private Sub EventFun(command As UInt16, result As UInt16, ByRef msg As UInt16, length As Integer)
    End Sub

    Sub wait_for_stop(device_id As Integer)
        While (HRobot.get_motion_state(device_id) <> 1 And HRobot.get_connection_level(device_id) <> -1)
            Thread.Sleep(30)
        End While
    End Sub


    Sub Main()
        callback = New HRobot.CallBackFun(AddressOf EventFun)
        device_id = HRobot.open_connection("127.0.0.1", 1, callback)
        SoftLimitExample(device_id, callback)
        Console.ReadLine()
        HRobot.disconnect(device_id)
    End Sub


    Sub SoftLimitExample(device_id As Integer, callback As HRobot.CallBackFun)
        If (device_id >= 0) Then
            Console.WriteLine("connect successful.")
        Else
            Console.WriteLine("connect failure.")
        End If
        Dim ver As StringBuilder = New StringBuilder(256)
        HRobot.get_hrsdk_version(ver)
        Console.WriteLine("get_HRSDK_version: " & ver.ToString())
        HRobot.set_motor_state(device_id, 1)

        Dim joint_low_limit() As Double = {-20, -20, -35, -20, 0, 0}
        Dim joint_high_limit() As Double = {20, 20, 0, 0, 0, 0}
        Dim cart_low_limit() As Double = {-100, 300, -100, 0, 0, 0}
        Dim cart_high_limit() As Double = {100, 450, -25, 0, 0, 0}
        Dim cart_home() As Double = {0, 400, 0, 0, -90, 0}
        Dim joint_home() As Double = {0, 0, 0, 0, -90, 0}
        Dim re_bool As Boolean = False

        ' Joint softlimit 
        HRobot.set_override_ratio(device_id, 100)
        HRobot.set_joint_soft_limit(device_id, joint_low_limit, joint_high_limit)
        HRobot.enable_joint_soft_limit(device_id, True)
        HRobot.enable_cart_soft_limit(device_id, False)
        HRobot.get_joint_soft_limit_config(device_id, re_bool, joint_low_limit, joint_high_limit)
        Console.WriteLine("Enable Joint SoftLimit: " & re_bool)
        HRobot.jog_home(device_id)
        wait_for_stop(device_id)
        Thread.Sleep(1000)
        For i As Integer = 0 To 3
            HRobot.jog(device_id, 1, i, -1)
            wait_for_stop(device_id)
            Console.WriteLine("On the limits of SoftLimit")
        Next
        For i As Integer = 0 To 3
            HRobot.jog(device_id, 1, i, 1)
            wait_for_stop(device_id)
            Console.WriteLine("On the limits of SoftLimit")
        Next
        HRobot.enable_joint_soft_limit(device_id, False)

        ' Cart Softlimit
        HRobot.ptp_axis(device_id, 0, joint_home)
        wait_for_stop(device_id)
        HRobot.set_joint_soft_limit(device_id, cart_low_limit, cart_high_limit)
        HRobot.enable_cart_soft_limit(device_id, True)
        HRobot.get_cart_soft_limit_config(device_id, re_bool, cart_low_limit, cart_high_limit)
        Console.WriteLine("Enable Cart SoftLimit: " & re_bool)
        HRobot.lin_pos(device_id, 0, 0, cart_home)
        wait_for_stop(device_id)
        For i As Integer = 0 To 2
            HRobot.jog(device_id, 0, i, -1)
            wait_for_stop(device_id)
            Console.WriteLine("On the limits of SoftLimit")
            HRobot.clear_alarm(device_id)
            Thread.Sleep(2000)
        Next
        For i As Integer = 0 To 2
            HRobot.jog(device_id, 0, i, 1)
            wait_for_stop(device_id)
            Console.WriteLine("On the limits of SoftLimit")
            HRobot.clear_alarm(device_id)
            Thread.Sleep(2000)
        Next
        HRobot.enable_joint_soft_limit(device_id, False)
        HRobot.enable_cart_soft_limit(device_id, False)

        Console.WriteLine("End of Motion")
    End Sub
End Module
