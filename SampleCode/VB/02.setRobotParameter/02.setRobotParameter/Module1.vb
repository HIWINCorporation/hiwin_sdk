Imports System.Text

Module Module1
    Private Sub EventFun(command As UInt16, result As UInt16, ByRef msg As UInt16, length As Integer)
    End Sub
    Public callback As HRobot.CallBackFun = New CallBackFun(AddressOf EventFun)
    Public device_id As Integer
    Sub Main()
        callback = New HRobot.CallBackFun(AddressOf EventFun)
        device_id = HRobot.open_connection("127.0.0.1", 1, callback)
        If (device_id >= 0) Then
            Console.WriteLine("connect successful.")
            SetRobotParameter(device_id, callback)
        Else
            Console.WriteLine("connect failure.")
        End If
        Console.ReadLine()
        HRobot.disconnect(device_id)
    End Sub
    Sub SetRobotParameter(device_id As Integer, callback As HRobot.CallBackFun)
        If (device_id >= 0) Then
            Console.WriteLine("connect successful.")
            Dim rlt As Integer
            rlt = HRobot.set_operation_mode(device_id, 1)
            Dim robot_id As StringBuilder = New StringBuilder("HrSs_Robot_ID")
            rlt = HRobot.set_robot_id(device_id, robot_id)
            rlt = HRobot.set_acc_dec_ratio(device_id, 50)
            rlt = HRobot.set_acc_time(device_id, 250)
            rlt = HRobot.set_operation_mode(device_id, 0)
            rlt = HRobot.set_operation_mode(device_id, 1)
            rlt = HRobot.set_override_ratio(device_id, 50)
            rlt = HRobot.set_ptp_speed(device_id, 50)
            rlt = HRobot.set_lin_speed(device_id, 800)
            rlt = HRobot.set_command_id(device_id, 10)
            rlt = HRobot.set_digital_output(device_id, 5, True)
            rlt = HRobot.set_robot_output(device_id, 5, True)
            rlt = HRobot.set_valve_output(device_id, 1, True)
            rlt = HRobot.set_base_number(device_id, 5)
            rlt = HRobot.set_tool_number(device_id, 5)
            rlt = HRobot.set_timer(device_id, 5, 1000)
            Dim timer_name As StringBuilder = New StringBuilder("timer_name")
            rlt = HRobot.set_timer_name(device_id, 5, timer_name)
            rlt = HRobot.set_timer_start(device_id, 5)
            rlt = HRobot.set_timer_stop(device_id, 5)
            rlt = HRobot.set_counter(device_id, 5, 1000)
            rlt = HRobot.set_pr_type(device_id, 5, 1000)
            Dim coor() As Double = {0, 0, 0, 0, -90, 0}
            rlt = HRobot.set_pr_coordinate(device_id, 5, coor)
            Dim tool_base() As Double = {5, 5}
            rlt = HRobot.set_pr_coordinate(device_id, 5, tool_base)
            rlt = HRobot.define_base(device_id, 5, coor)
            rlt = HRobot.define_tool(device_id, 5, coor)
            rlt = HRobot.set_pr(device_id, 5, 1, coor, 5, 5)
            rlt = HRobot.set_pr_tool_base(device_id, 5, 2, 2)
            Dim pr_comment As StringBuilder = New StringBuilder("pr_comment")
            rlt = HRobot.set_pr_comment(device_id, 5, pr_comment)
            rlt = HRobot.remove_pr(device_id, 5)
            rlt = HRobot.set_smooth_length(device_id, 200)
            Dim rsr As String = "set_rsr.hrb"
            rlt = HRobot.set_rsr(device_id, rsr, 1)
            rlt = HRobot.remove_rsr(device_id, 1)
            rlt = HRobot.set_motor_state(device_id, 1)
            rlt = HRobot.set_operation_mode(device_id, 1)
            rlt = HRobot.remove_command(device_id, 1)
            rlt = HRobot.remove_command_tail(device_id, 1)

            'Module I/O
            rlt = HRobot.set_module_input_simulation(device_id, 5, True)
            rlt = HRobot.set_module_input_value(device_id, 5, True)
            rlt = HRobot.set_module_input_start(device_id, 5, 1)
            rlt = HRobot.set_module_input_end(device_id, 5, 5)
            Dim mi_comment As StringBuilder = New StringBuilder("mi_comment")
            rlt = HRobot.set_module_input_comment(device_id, 5, mi_comment)
            rlt = HRobot.set_module_output_value(device_id, 5, True)
            rlt = HRobot.set_module_output_start(device_id, 5, 1)
            rlt = HRobot.set_module_output_end(device_id, 5, 5)
            Dim mo_comment As StringBuilder = New StringBuilder("mo_commemt")
            rlt = HRobot.set_module_output_comment(device_id, 5, mo_comment)


            ''' New 2020.03.19
            Dim Str As StringBuilder = New StringBuilder("test text")
            Dim DIO As Integer = 0
            Dim SIO As Integer = 1
            Dim data() As Integer = {DIO, 34, DIO, 35, SIO, 36, SIO, 37, DIO, 38, SIO, 39}
            Dim ON_ As Boolean = True

            rlt = HRobot.set_DI_simulation_Enable(device_id, 5, ON_)
            rlt = HRobot.set_DI_simulation(device_id, 5, ON_)
            rlt = HRobot.set_digital_input_comment(device_id, 5, Str)
            rlt = HRobot.set_digital_output_comment(device_id, 5, Str)
            Dim joint() As Double = {0, 0, 0, 0, 0, 0}
            rlt = HRobot.set_home_point(device_id, joint)
            rlt = HRobot.set_module_input_type(device_id, 5, 0)
            rlt = HRobot.set_module_output_type(device_id, 5, 1)
            rlt = HRobot.set_counter_name(device_id, 5, Str)

            rlt = HRobot.set_digital_setting(device_id, Data, Str)
            rlt = HRobot.set_user_alarm_setting_message(device_id, 5, Str)
            rlt = HRobot.set_language(device_id, 0)
            rlt = HRobot.save_module_io_setting(device_id)

        Else
            Console.WriteLine("connect failure.")
        End If

        Console.ReadLine()
    End Sub



End Module
