Imports System.Runtime.InteropServices
Imports System.Text

Module Module1
    Private Sub EventFun(command As UInt16, result As UInt16, ByRef msg As UInt16, length As Integer)
    End Sub

    Public callback As HRobot.CallBackFun
    Public device_id As Integer


    Sub Main()
        callback = New HRobot.CallBackFun(AddressOf EventFun)
        device_id = HRobot.open_connection("127.0.0.1", 1, callback)
        If (device_id >= 0) Then
            Console.WriteLine("connect successful.")
            PrintFunction()
        Else
            Console.WriteLine("connect failure.")
        End If
        Console.ReadLine()
        HRobot.disconnect(device_id)
    End Sub

    Sub PrintFunction()
        While True
            Console.Clear()
            Console.WriteLine("--------------------------")
            Dim v As StringBuilder = New StringBuilder(256)
            HRobot.get_hrsdk_version(v)
            Console.WriteLine("HRSDK VERSION:" & vbTab & v.ToString())

            Dim HrssV As StringBuilder = New StringBuilder(256)
            HRobot.get_hrss_version(device_id, HrssV)
            Console.WriteLine("HRSS version:" & vbTab & HrssV.ToString())

            Dim robot_id As StringBuilder = New StringBuilder(256)
            HRobot.get_robot_id(device_id, robot_id)
            Console.WriteLine("Robot_ID:" & vbTab & robot_id.ToString())

            Dim rsr_prog_name As StringBuilder = New StringBuilder(256)
            HRobot.get_rsr_prog_name(device_id, 1, rsr_prog_name)
            Console.WriteLine("RSR 1 name:" & vbTab & rsr_prog_name.ToString())

            Dim execute_file_name As StringBuilder = New StringBuilder(256)
            HRobot.get_execute_file_name(device_id, execute_file_name)
            Console.WriteLine("Execute file name:" & vbTab & execute_file_name.ToString())

            'Dim alarm_code() As ULong
            'Dim count As Integer = 10
            'HRobot.get_alarm_code(device_id, count, alarm_code)

            'Dim from_file_path As String = "log.txt"
            'Dim to_file_path As String = "log.txt"
            'HRobot.download_log_file(device_id, from_file_path, to_file_path)

            HRobot.get_robot_type(device_id, v)
            Console.WriteLine("ROBOT TYPE:" + v.ToString())

            Dim motor_state As Integer = HRobot.get_motor_state(device_id)
            Console.WriteLine("motor_state:" & motor_state)

            Dim operation_mode As Integer = HRobot.get_operation_mode(device_id)
            Console.WriteLine("operation_mode:" & operation_mode)


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
            Console.WriteLine("ACC RATIO:" & vbTab & HRobot.get_acc_time(device_id).ToString() + "(%)")
            Console.WriteLine("PTP RATIO:" & vbTab & HRobot.get_ptp_speed(device_id).ToString() + "(%)")
            Console.WriteLine("LIN SPEED:" & vbTab & HRobot.get_lin_speed(device_id).ToString() + "mm/s")
            Console.WriteLine("OVERRIDE RATIO:" & vbTab & HRobot.get_override_ratio(device_id).ToString() + "(%)")
            Console.WriteLine("-------------------------------------------------------------" & vbCrLf)
            Dim YMD(0 To 7) As Integer
            HRobot.get_device_born_date(device_id, YMD)
            Console.WriteLine("BIRTHDAY:" & vbTab + YMD(0).ToString() + "年 " + YMD(1).ToString() + "月 " + YMD(2).ToString() + "日 ")
            HRobot.get_operation_time(device_id, YMD)
            Console.WriteLine("OPERATION TIME:" & vbTab + YMD(0).ToString() + "年 " + YMD(1).ToString() + "月 " + YMD(2).ToString() + "日 " + YMD(3).ToString() + "時 " + YMD(4).ToString() + "分")
            Dim utilization_ratio As Double
            HRobot.get_utilization_ratio(device_id, utilization_ratio)
            Console.WriteLine("UTILIZATION RATIO:" & vbTab + utilization_ratio.ToString() + "(%)")
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
            PrintHomePos()
            Console.WriteLine(vbCrLf & vbTab & "-----------------------------------------------" & vbCrLf)
            PrintPrePos()
            Console.WriteLine(vbCrLf & vbTab & "-----------------------------------------------" & vbCrLf)

            Dim cmd_count As Integer = HRobot.get_command_count(device_id)
            Console.Write("cmd_count:" & cmd_count)

            Dim cmd_id As Integer = HRobot.get_command_id(device_id)
            Console.WriteLine(vbTab & "cmd_id:" & cmd_id)

            Dim mode As Integer = HRobot.get_hrss_mode(device_id)
            Console.WriteLine("mode:" & mode)

            Dim DI As Integer = HRobot.get_digital_input(device_id, 5)
            Console.Write("DI_5:" & DI)

            Dim DOo As Integer = HRobot.get_digital_output(device_id, 5)
            Console.Write(vbTab & "DO_5:" & DOo)

            Dim DI_sim As Integer = HRobot.get_DI_simulation_Enable(device_id, 5)
            Console.WriteLine(vbTab & "DI_sim_5:" & DOo)

            Dim FI As Integer = HRobot.get_function_input(device_id, 5)
            Console.Write("FI_5:" & FI)

            Dim FO As Integer = HRobot.get_function_output(device_id, 5)
            Console.WriteLine(vbTab & "FO_5:" & FO)

            Dim RI As Integer = HRobot.get_robot_input(device_id, 5)
            Console.Write("RI_5:" & RI)

            Dim RO As Integer = HRobot.get_robot_output(device_id, 5)
            Console.Write(vbTab & "RO_5:" & RO)

            Dim VO As Integer = HRobot.get_valve_output(device_id, 1)
            Console.WriteLine(vbTab & "VO_1:" & VO)

            Dim Timer As Integer = HRobot.get_timer(device_id, 5)
            Console.Write("timer:" & Timer)

            Dim timer_status As Integer = HRobot.get_timer_status(device_id, 5)
            Console.Write(vbTab & "timer_status:" & timer_status)

            Dim timer_name As StringBuilder = New StringBuilder(50)
            Dim rlt As Integer = HRobot.get_timer_name(device_id, 5, timer_name)
            Console.WriteLine(vbTab & "timer_name:" & timer_name.ToString)


            Dim counter As Integer = HRobot.get_counter(device_id, 5)
            Console.WriteLine("counter:" & counter)

            Dim pr_type As Integer = HRobot.get_pr_type(device_id, 5)
            Console.WriteLine("pr_type:" & pr_type)

            Dim get_coor() As Double = {0}
            HRobot.get_pr_coordinate(device_id, 5, get_coor)
            Console.WriteLine("pr_coordinate:" & get_coor(0))

            Dim get_tool_base() As Integer = {0}
            HRobot.get_pr_tool_base(device_id, 5, get_tool_base)
            Console.WriteLine("pr_tool_base:" & get_coor(0))

            Dim tool As Integer = 0
            Dim _base As Integer = 0
            HRobot.get_pr(device_id, 5, pr_type, get_coor, tool, _base)

            Dim base_num As Integer = HRobot.get_base_number(device_id)
            Console.WriteLine("base_number:" & base_num)

            Dim base_data() As Double = {0}
            HRobot.get_base_data(device_id, 5, base_data)
            'Console.WriteLine("get_base_data:\t" & base_data(0))

            Dim tool_num As Integer = HRobot.get_tool_number(device_id)
            Console.WriteLine("tool_number:" & tool_num)

            Dim tool_data() As Double = {0}
            HRobot.get_tool_data(device_id, 5, tool_data)
            'Console.WriteLine("get_base_data:\t" & tool_data(0))

            Dim mi_index As Integer = 1
            Dim mi_sim As Boolean = True
            Dim mi_value As Integer = True
            Dim mi_type As Integer = -1
            Dim mi_start As Integer = -1
            Dim mi_end As Integer = -1
            Dim mi_comment As StringBuilder = New StringBuilder(50)
            HRobot.get_module_input_config(device_id, mi_index, mi_sim, mi_value, mi_type, mi_start, mi_end, mi_comment)
            Console.Write("MI: index:{0}  sim:{1}  value:{2}  type:{3}  ", mi_index, mi_sim, mi_value, mi_type)
            Console.WriteLine("start:{0}  end:{1}  comment:{2} ", mi_start, mi_end, mi_comment)

            Dim mo_index As Integer = 1
            Dim mo_sim As Boolean = True
            Dim mo_value As Integer = True
            Dim mo_type As Integer = -1
            Dim mo_start As Integer = -1
            Dim mo_end As Integer = -1
            Dim mo_comment As StringBuilder = New StringBuilder(50)
            HRobot.get_module_output_config(device_id, mo_index, mo_value, mo_type, mo_start, mo_end, mo_comment)
            Console.Write("MO: index:{0}  value:{1}  type:{2}  ", mo_index, mo_value, mo_type)
            Console.WriteLine("start:{0}  end:{1}  comment:{2} ", mo_start, mo_end, mo_comment)

            Dim user_msg As StringBuilder = New StringBuilder(200)
            HRobot.get_user_alarm_setting_message(device_id, 5, user_msg)
            Console.WriteLine("user_alarm_msg_5:" & vbTab & user_msg.ToString)

            Dim value As Integer = -1
            HRobot.get_payload_value(device_id, value)
            Console.WriteLine("payload:" & vbTab & value)

            PrintDIOSetting()

            Dim Year As Integer = -1
            Dim Month As Integer = -1
            Dim Day As Integer = -1
            Dim Hour As Integer = -1
            Dim min As Integer = -1
            Dim Second As Integer = -1
            HRobot.get_controller_time(device_id, Year, Month, Day, Hour, min, Second)
            Console.WriteLine("controller_time: {0}/{1}/{2} {3}:{4}:{5}  ", Year, Month, Day, Hour, min, Second)
            Threading.Thread.Sleep(5000)
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

    Sub PrintHomePos()
        Dim p(6) As Double
        HRobot.get_home_point(device_id, p)
        Console.Write("Home:" & vbTab)
        For a As Integer = 0 To 5
            Console.Write(p(a) & vbTab)
        Next
    End Sub

    Sub PrintPrePos()
        Dim p(6) As Double
        HRobot.get_previous_pos(device_id, p)
        Console.Write("Pre:" & vbTab)
        For a As Integer = 0 To 5
            Console.Write(p(a) & vbTab)
        Next
    End Sub


    Sub PrintDIOSetting()
        Dim D_setting(13) As Integer
        Dim text As StringBuilder = New StringBuilder(100)
        Dim DI_SI() As String = {"DI", "SI"}
        Dim DO_SO() As String = {"DO", "SO"}
        Dim msg() As String = {"Clear Error", "External Alarm", "System Shutdown", "Moter Warning", "System StartUp", "Mode Output"}
        HRobot.get_digital_setting(device_id, D_setting, text)
        Console.WriteLine("DIO setting:")
        For i As Integer = 0 To 11
            If (i = 6) Then
                Console.WriteLine()
            End If
            If (i Mod 2 = 0 And i < 6) Then
                Console.Write(msg(Convert.ToInt32(i / 2)).ToString() & " ")
                Console.Write(DI_SI(D_setting(i)) & " ")
            ElseIf (i Mod 2 = 0 And i >= 6) Then
                Console.Write(msg(Convert.ToInt32(i / 2)) & " ")
                Console.Write("{0}:", DO_SO(D_setting(i)))
            Else
                Console.Write(D_setting(i) & "   ")
            End If
        Next
        Console.WriteLine()
        Console.WriteLine("Text Length:{0},  Show Text: {1} ", D_setting(12), text)
    End Sub



    Public Declare Function CreateConsoleScreenBuffer Lib "kernel32.dll" (dwDesiredAccess As UInteger, dwShareMode As UInteger, lpSecurityAttributes As IntPtr, dwFlags As UInteger, lpScreenBufferData As IntPtr) As IntPtr
    Public Declare Function GetStdHandle Lib "kernel32.dll" (nStdHandle As Integer) As IntPtr
    Public Declare Function SetConsoleActiveScreenBuffer Lib "kernel32.dll" (hConsoleOutput As IntPtr) As Integer

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure COORD
        Public X As Short
        Public Y As Short

        Public Sub New(ByVal x As Short, ByVal y As Short)
            Me.X = x
            Me.Y = y
        End Sub
    End Structure

    Public Declare Function SetConsoleScreenBufferSize Lib "kernel32.dll" (hConsoleOutput As IntPtr, size As COORD) As Integer

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Function ReadConsoleOutputCharacter(hConsoleOutput As IntPtr, lpCharacter As StringBuilder, nLength As UInteger, size As COORD, ByRef lpNumberOfCharsRead As UInteger) As Integer
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Function WriteConsoleOutputCharacter(hConsoleOutput As IntPtr, lpCharacter As StringBuilder, nLength As UInteger, size As COORD, ByRef lpNumberOfCharsRead As UInteger) As Integer
    End Function   
End Module
