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
        SyncOutput(device_id, callback)
        Console.ReadLine()
        HRobot.disconnect(device_id)
    End Sub



    Sub SyncOutput(device_id As Integer, callback As HRobot.CallBackFun)
        If (device_id >= 0) Then
            Console.WriteLine("connect successful.")
        Else
            Console.WriteLine("connect failure.")
        End If

        HRobot.set_motor_state(device_id, 1)
        Dim p1() As Double = {0, -200, 0, 0, 0, 0}
        Dim p2() As Double = {0, 150, 0, 0, 0, 0}
        Dim home() As Double = {0, 0, 0, 0, 0, 0}
        Dim p3() As Double = {20, 0, 0, 0, 0, 0}
        Dim p4() As Double = {-20, 0, 0, 0, 0, 0}

        Dim Type As Integer = 0
        Dim id As Integer = 1
        Dim ON_ As Integer = 1
        Dim OFF As Integer = 0
        Dim delay As Integer = 1000
        Dim distance As Integer = 50
        Dim Start As Integer = 0
        Dim End_ As Integer = 1
        Dim Path As Integer = 2

        HRobot.jog_home(device_id)
        Thread.Sleep(100)
        HRobot.lin_rel_pos(device_id, 0, 0, p1)
        HRobot.SyncOutput(device_id, Type, id, ON_, Start, delay, distance)
        HRobot.SyncOutput(device_id, Type, 2, ON_, Path, -1000, distance)
        HRobot.SyncOutput(device_id, Type, 3, ON_, Path, 0, distance)
        HRobot.SyncOutput(device_id, Type, 4, ON_, Path, delay, distance)
        HRobot.SyncOutput(device_id, Type, 5, ON_, Path, -1000, -50)
        HRobot.SyncOutput(device_id, Type, 6, ON_, Path, 0, -50)
        HRobot.SyncOutput(device_id, Type, 7, ON_, Path, 1000, -50)
        HRobot.SyncOutput(device_id, Type, 8, ON_, End_, -1000, distance)
        HRobot.lin_rel_pos(device_id, 0, 0, p2)
    End Sub
End Module
