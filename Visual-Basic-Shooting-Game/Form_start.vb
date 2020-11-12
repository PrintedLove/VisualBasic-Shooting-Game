'[Visual Basic] Visual Basic Shooting Game

'made by - Printed Love
'Blog: https://printed.tistory.com
'YouTube: https://youtube.com/channel/UCtKTjiof6Mwa_4ffHDYyCbQ

Public Class Form_start

    Private nameColorR, nameColorG, nameColorB As UInt16
    Private nameColorR_t, nameColorG_t, nameColorB_t As Boolean

    Private Sub Form_start_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetValue()

        Text = "Visual Basic Shooting Game"
        BackColor = GRAY
        FormBorderStyle = FormBorderStyle.FixedSingle

        'button setting
        Dim fixedGRAY As Color = Color.FromArgb(100, GRAY_DEEP.R, GRAY_DEEP.G, GRAY_DEEP.B)
        Button_start.Font = font_16
        Button_start.FlatStyle = Windows.Forms.FlatStyle.Flat
        Button_start.FlatAppearance.BorderSize = 0
        Button_start.FlatAppearance.MouseDownBackColor = fixedGRAY
        Button_start.FlatAppearance.MouseOverBackColor = Color.Transparent
        Button_start.BackColor = Color.Transparent
        Button_help.Font = font_16
        Button_help.FlatStyle = Windows.Forms.FlatStyle.Flat
        Button_help.FlatAppearance.BorderSize = 0
        Button_help.FlatAppearance.MouseDownBackColor = fixedGRAY
        Button_help.FlatAppearance.MouseOverBackColor = Color.Transparent
        Button_help.BackColor = Color.Transparent
        Button_exit.Font = font_16
        Button_exit.FlatStyle = Windows.Forms.FlatStyle.Flat
        Button_exit.FlatAppearance.BorderSize = 0
        Button_exit.FlatAppearance.MouseDownBackColor = fixedGRAY
        Button_exit.FlatAppearance.MouseOverBackColor = Color.Transparent
        Button_exit.BackColor = Color.Transparent
        Button_start.Focus()

        'label setting
        Label_name.Font = font_32
        Label_name.ForeColor = SKYBLUE
        Label_name.BackColor = Color.Transparent
        nameColorR = 75
        nameColorG = 140
        nameColorB = 235
        nameColorR_t = True
        nameColorG_t = True
        nameColorB_t = True
    End Sub

    Private Sub Timer_formStart_Tick(sender As Object, e As EventArgs) Handles Timer_formStart.Tick
        If nameColorR_t = True Then
            nameColorR += 5
        Else
            nameColorR -= 5
        End If

        If nameColorG_t = True Then
            nameColorG += 5
        Else
            nameColorG -= 5
        End If

        If nameColorB_t = True Then
            nameColorB += 5
        Else
            nameColorB -= 5
        End If

        If nameColorR = 255 Or nameColorR = 0 Then
            If nameColorR_t = True Then
                nameColorR_t = False
            Else
                nameColorR_t = True
            End If
        End If

        If nameColorG = 255 Or nameColorG = 0 Then
            If nameColorG_t = True Then
                nameColorG_t = False
            Else
                nameColorG_t = True
            End If
        End If

        If nameColorB = 255 Or nameColorB = 0 Then
            If nameColorB_t = True Then
                nameColorB_t = False
            Else
                nameColorB_t = True
            End If
        End If

        Dim nameColor As Color = Color.FromArgb(255, nameColorR, nameColorG, nameColorB)
        Label_name.ForeColor = nameColor

        If Button_start.Focused = True Then
            Button_start.Text = "- START -"
            Button_help.Text = "HELP"
            Button_exit.Text = "EXIT"
        ElseIf Button_help.Focused = True Then
            Button_start.Text = "START"
            Button_help.Text = "- HELP -"
            Button_exit.Text = "EXIT"
        ElseIf Button_exit.Focused = True Then
            Button_start.Text = "START"
            Button_help.Text = "HELP"
            Button_exit.Text = "- EXIT -"
        End If
    End Sub

    Private Sub Button_Click(sender As Object, e As EventArgs) Handles Button_start.Click, Button_help.Click, Button_exit.Click
        Dim btn As Button = DirectCast(sender, Button)

        Select Case btn.Name
            Case "Button_start"
                Dim formPlay As New Form_play
                formPlay.Show()
                Hide()

            Case "Button_help"

            Case "Button_exit"
                Close()
        End Select
    End Sub

    Private Sub Mouse_move(sender As Object, e As EventArgs) Handles Button_start.MouseEnter,
        Button_help.MouseEnter, Button_exit.MouseEnter
        Dim btn As Button = DirectCast(sender, Button)

        Select Case btn.Name
            Case "Button_start"
                Button_start.Focus()

            Case "Button_help"
                Button_help.Focus()

            Case "Button_exit"
                Button_exit.Focus()
        End Select
    End Sub

    Private Sub Form_start_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        'object destroy
        Dim list_index As Int16 = 0

        While list_index < obj_list.Count()
            Dim obj As Object = obj_list.Item(list_index)

            obj.Die()
            obj_list.RemoveAt(list_index)
        End While

        Dispose()
    End Sub

End Class