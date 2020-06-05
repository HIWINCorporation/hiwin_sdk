Imports System.Runtime.InteropServices
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
        HRobot.set_motor_state(device_id, 1)
        Jog(device_id, callback)
        Console.ReadLine()
        HRobot.disconnect(device_id)
    End Sub

    Sub Jog(id As Integer, callback As HRobot.CallBackFun)
        device_id = id
        If (device_id >= 0) Then
            Console.WriteLine("connect successful.")
        Else
            Console.WriteLine("connect failure.")
        End If

        If HRobot.get_motor_state(device_id) Then
            HRobot.set_motor_state(device_id, 1)
        End If

        HRobot.set_override_ratio(device_id, 100)
        HRobot.set_operation_mode(device_id, 0)
        Dim printThread As Thread = New Thread(AddressOf PrintFunction)
        printThread.Start()
        While (True)
            Dim keyNum As ConsoleKey = Console.ReadKey().Key
            If (keyNum = ConsoleKey.Escape) Then
                Exit While
            End If

            JogByKeyDown(keyNum, 0)
            WaitKeUp()
        End While


    End Sub


    Private Sub JogByKeyDown(keyNum As ConsoleKey, type As Integer)
        Select Case (keyNum)
            Case ConsoleKey.Q
                HRobot.jog(device_id, type, 0, 1)
            Case ConsoleKey.W
                HRobot.jog(device_id, type, 0, -1)
            Case ConsoleKey.A
                HRobot.jog(device_id, type, 1, 1)
            Case ConsoleKey.S
                HRobot.jog(device_id, type, 1, -1)
            Case ConsoleKey.Z
                HRobot.jog(device_id, type, 2, 1)
            Case ConsoleKey.X
                HRobot.jog(device_id, type, 2, -1)
            Case ConsoleKey.E
                HRobot.jog(device_id, type, 3, 1)
            Case ConsoleKey.R
                HRobot.jog(device_id, type, 3, -1)
            Case ConsoleKey.D
                HRobot.jog(device_id, type, 4, 1)
            Case ConsoleKey.F
                HRobot.jog(device_id, type, 4, -1)
            Case ConsoleKey.C
                HRobot.jog(device_id, type, 5, 1)
            Case ConsoleKey.V
                HRobot.jog(device_id, type, 5, -1)
        End Select
    End Sub
    Private Sub WaitKeUp()
        While (GetAsyncKeyState(81) < 0 Or
                GetAsyncKeyState(87) < 0 Or
                GetAsyncKeyState(65) < 0 Or
                GetAsyncKeyState(83) < 0 Or
                GetAsyncKeyState(90) < 0 Or
                GetAsyncKeyState(88) < 0 Or
                GetAsyncKeyState(69) < 0 Or
                GetAsyncKeyState(82) < 0 Or
                GetAsyncKeyState(68) < 0 Or
                GetAsyncKeyState(70) < 0 Or
                GetAsyncKeyState(67) < 0 Or
                GetAsyncKeyState(86) < 0)
            Thread.Sleep(5)
        End While
        HRobot.jog_stop(device_id)
    End Sub

    Private Sub PrintFunction()
        While True
            Console.Clear()
            Console.WriteLine("--------------------------")
            Dim v As StringBuilder = New StringBuilder(256)
            HRobot.get_hrsdk_version(v)
            Console.WriteLine("HRSDK VERSION:" & vbTab & v.ToString())

            Dim HrssV As StringBuilder = New StringBuilder(256)
            HRobot.get_hrss_version(device_id, HrssV)
            Console.WriteLine("HRSS version:" & vbTab & HrssV.ToString())

            HRobot.get_robot_type(device_id, v)
            Console.WriteLine("ROBOT TYPE:" + v.ToString())
            Select Case (HRobot.get_motion_state(device_id))
                Case 1
                    Console.WriteLine("運動狀態:" & vbTab & "閒置")

                Case 2
                    Console.WriteLine("運動狀態:" & vbTab & "運動")

                Case 3
                    Console.WriteLine("運動狀態:" & vbTab & "暫停")

                Case 4
                    Console.WriteLine("運動狀態:" & vbTab & "延遲")

                Case 5
                    Console.WriteLine("運動狀態:" & vbTab & "等待命令")
            End Select
            Console.WriteLine("-------------------------------------------------------------" & vbCrLf)
            Console.WriteLine("ACC RATIO:" & vbTab & HRobot.get_acc_dec_ratio(device_id).ToString() + "(%)")
            Console.WriteLine("PTP RATIO:" & vbTab & HRobot.get_ptp_speed(device_id).ToString() + "(%)")
            Console.WriteLine("LIN SPEED:" & vbTab & HRobot.get_lin_speed(device_id).ToString() + "mm/s")
            Console.WriteLine("OVERRIDE RATIO:" & vbTab & HRobot.get_override_ratio(device_id).ToString() + "(%)")
            Console.WriteLine("-------------------------------------------------------------" & vbCrLf)
            Dim YMD(0 To 7) As Integer
            HRobot.get_device_born_date(device_id, YMD)
            Console.WriteLine("BIRTHDAY:" & vbTab + YMD(0).ToString() + "年 " + YMD(1).ToString() + "月 " + YMD(2).ToString() + "日 ")
            HRobot.get_operation_time(device_id, YMD)
            Console.WriteLine("OPERATION TIME:" & vbTab + YMD(0).ToString() + "年 " + YMD(1).ToString() + "月 " + YMD(2).ToString() + "日 " + YMD(3).ToString() + "時 " + YMD(4).ToString() + "分")
            Dim ratio_value As Double
            Console.WriteLine("UTILIZATION RATIO:" & vbTab + HRobot.get_utilization_ratio(device_id, ratio_value).ToString() + "(%)")
            HRobot.get_utilization(device_id, YMD)
            Console.WriteLine("UTILIZATION:" & vbTab + YMD(0).ToString() + "年 " + YMD(1).ToString() + "月 " + YMD(2).ToString() + "日 " + YMD(3).ToString() + "時 " + YMD(4).ToString() + "分" + YMD(5).ToString() + "秒")
            Console.WriteLine("-------------------------------------------------------------" & vbCrLf)
            Console.WriteLine(vbTab & "A1" & vbTab & "A2" & vbTab & "A3" & vbTab & "A4" & vbTab & "A5" & vbTab & "A6")
            PrintRPM()
            Console.WriteLine(vbCrLf & vbTab & "-----------------------------------------------" & vbCrLf)
            PrintCartPos()
            Console.WriteLine(vbCrLf & vbTab & "-----------------------------------------------" & vbCrLf)
            PrintJointPos()
            Console.WriteLine(vbCrLf & vbTab & "-----------------------------------------------" & vbCrLf)
            PrintEnc()
            Console.WriteLine(vbCrLf & vbTab & "-----------------------------------------------" & vbCrLf)
            PrintTorq()
            Console.WriteLine(vbCrLf & vbTab & "-----------------------------------------------" & vbCrLf)
            PrintMil()
            Console.WriteLine(vbCrLf & vbTab & "-----------------------------------------------" & vbCrLf)
            Thread.Sleep(2000)
        End While


    End Sub

    Sub PrintRPM()
        Dim p(6) As Double
        p = New Double() {0, 0, 0, 0, 0, 0}
        HRobot.get_current_rpm(device_id, p)
        Console.Write("PRM:" & vbTab)
        For a As Integer = 0 To 5
            Console.Write(p(a) & vbTab)
        Next
        Console.WriteLine()
    End Sub

    Sub PrintCartPos()
        Dim p(6) As Double
        p = New Double() {0, 0, 0, 0, 0, 0}
        HRobot.get_current_position(device_id, p)
        Console.Write("Cart:" & vbTab)
        For a As Integer = 0 To 5
            Console.Write(p(a) & vbTab)
        Next
        Console.WriteLine()
    End Sub

    Sub PrintJointPos()
        Dim p(6) As Double
        p = New Double() {0, 0, 0, 0, 0, 0}
        HRobot.get_current_joint(device_id, p)
        Console.Write("Joint:" & vbTab)
        For a As Integer = 0 To 5
            Console.Write(p(a) & vbTab)
        Next
        Console.WriteLine()
    End Sub

    Sub PrintEnc()
        Dim p(6) As Integer
        p = New Integer() {0, 0, 0, 0, 0, 0}
        HRobot.get_encoder_count(device_id, p)
        Console.Write("ENC:" & vbTab)
        For a As Integer = 0 To 5
            Console.Write(p(a) & vbTab)
        Next
        Console.WriteLine()
    End Sub

    Sub PrintTorq()
        Dim p(6) As Double
        p = New Double() {0, 0, 0, 0, 0, 0}
        HRobot.get_motor_torque(device_id, p)
        Console.Write("Torq:" & vbTab)
        For a As Integer = 0 To 5
            Console.Write(p(a) & vbTab)
        Next
        Console.WriteLine()
    End Sub

    Sub PrintMil()
        Dim p(6) As Double
        HRobot.get_mileage(device_id, p)
        Console.Write("Mil:" & vbTab)
        For a As Integer = 0 To 5
            Console.Write(p(a) & vbTab)
        Next
        Console.WriteLine()

        HRobot.get_total_mileage(device_id, p)
        Console.Write("TMil:" & vbTab)
        For a As Integer = 0 To 5
            Console.Write(p(a) & vbTab)
        Next

    End Sub
    Public Declare Function CreateConsoleScreenBuffer Lib "kernel32.dll" (dwDesiredAccess As UInteger, dwShareMode As UInteger, lpSecurityAttributes As IntPtr, dwFlags As UInteger, lpScreenBufferData As IntPtr) As IntPtr
    Public Declare Function GetStdHandle Lib "kernel32.dll" (nStdHandle As Integer) As IntPtr
    Public Declare Function SetConsoleActiveScreenBuffer Lib "kernel32.dll" (hConsoleOutput As IntPtr) As Integer

    <DllImport("user32.dll", SetLastError:=True)>
    Function GetAsyncKeyState(vKey As Integer) As Short
    End Function
    <StructLayout(LayoutKind.Sequential)>
    Public Structure COORD
        Public X As Short
        Public Y As Short

        Public Sub New(ByVal x As Short, ByVal y As Short)
            Me.X = x
            Me.Y = y
        End Sub
    End Structure

    Public Declare Function SetConsoleScreenBufferSize Lib "kernel32.dll" (hConsoleOutput As IntPtr, size As COORD) As Integer

    <DllImport("kernel32.dll", SetLastError:=True)>
    Function ReadConsoleOutputCharacter(hConsoleOutput As IntPtr, lpCharacter As StringBuilder, nLength As UInteger, size As COORD, ByRef lpNumberOfCharsRead As UInteger) As Integer
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Function WriteConsoleOutputCharacter(hConsoleOutput As IntPtr, lpCharacter As StringBuilder, nLength As UInteger, size As COORD, ByRef lpNumberOfCharsRead As UInteger) As Integer
    End Function
End Module
