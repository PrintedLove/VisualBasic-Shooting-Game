'[Visual Basic] Visual Basic Shooting Game

'made by - Printed Love
'Blog: https://printed.tistory.com
'YouTube: https://youtube.com/channel/UCtKTjiof6Mwa_4ffHDYyCbQ

Public Class Form_start
    Private chooseMode As Short = 0
    Private nameColor = {{75, 140, 235}, {True, True, True}}

    Private Sub Form_start_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        SetValue()

        Text = "Visual Basic Shooting Game"
        BackColor = GRAY
        FormBorderStyle = FormBorderStyle.FixedSingle

        'sound
        GS.Play("snd_background")

        'button setting
        Dim fixedGRAY As Color = Color.FromArgb(80, GRAY_DEEP.R, GRAY_DEEP.G, GRAY_DEEP.B)
        Dim fixedDGRAY As Color = Color.FromArgb(160, GRAY_DEEP.R, GRAY_DEEP.G, GRAY_DEEP.B)

        With Button_start
            .Font = font_16
            .FlatStyle = FlatStyle.Flat
            .FlatAppearance.BorderSize = 0
            .FlatAppearance.BorderColor = GRAY
            .FlatAppearance.MouseDownBackColor = fixedDGRAY
            .FlatAppearance.MouseOverBackColor = fixedGRAY
            .BackColor = Color.Transparent
            .Focus()
        End With

        With Button_help
            .Font = font_16
            .FlatStyle = FlatStyle.Flat
            .FlatAppearance.BorderSize = 0
            .FlatAppearance.BorderColor = GRAY
            .FlatAppearance.MouseDownBackColor = fixedDGRAY
            .FlatAppearance.MouseOverBackColor = fixedGRAY
            .BackColor = Color.Transparent
        End With

        With Button_exit
            .Font = font_16
            .FlatStyle = FlatStyle.Flat
            .FlatAppearance.BorderSize = 0
            .FlatAppearance.BorderColor = GRAY
            .FlatAppearance.MouseDownBackColor = fixedDGRAY
            .FlatAppearance.MouseOverBackColor = fixedGRAY
            .BackColor = Color.Transparent
        End With

        'label setting
        Label_name.Font = font_32
        Label_name.ForeColor = SKYBLUE
        Label_name.BackColor = Color.Transparent
    End Sub

    Private Sub Timer_formStart_Tick(sender As Object, e As EventArgs) Handles Timer_formStart.Tick
        'Label_name 's color set
        For i As Short = 0 To 2
            If nameColor(1, i) = True Then
                nameColor(0, i) += 5
            Else
                nameColor(0, i) -= 5
            End If

            If nameColor(0, i) = 255 Or nameColor(0, i) = 0 Then
                If nameColor(1, i) = True Then
                    nameColor(1, i) = False
                Else
                    nameColor(1, i) = True
                End If
            End If
        Next

        Dim nameColors As Color = Color.FromArgb(255, nameColor(0, 0), nameColor(0, 1), nameColor(0, 2))
        Label_name.ForeColor = nameColors

        'button text change by mouse position
        If chooseMode = 0 Then
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
        ElseIf chooseMode = 1 Then
            If Button_start.Focused = True Then
                Button_start.Text = "- EASY -"
                Button_help.Text = "NORMAL"
                Button_exit.Text = "HARD"
            ElseIf Button_help.Focused = True Then
                Button_start.Text = "EASY"
                Button_help.Text = "- NORMAL -"
                Button_exit.Text = "HARD"
            ElseIf Button_exit.Focused = True Then
                Button_start.Text = "EASY"
                Button_help.Text = "NORMAL"
                Button_exit.Text = "- HARD -"
            End If
        End If

        If chooseMode < 2 Then
            If GS.IsPlaying("snd_background") = False Then GS.Play("snd_background")
        End If
    End Sub

    Private Sub Button_Click(sender As Object, e As EventArgs) Handles Button_start.Click, Button_help.Click, Button_exit.Click
        GS_PlaySound("snd_button")

        Dim btn As Button = DirectCast(sender, Button)

        Select Case btn.Name
            Case "Button_start"
                If chooseMode = 0 Then
                    chooseMode = 1
                ElseIf chooseMode = 1 Then
                    difficulty = 1
                    S_WIDTH = 960
                    S_HEIGHT = 960
                    chooseMode = 2
                    GS.Stop("snd_background")
                    Dim formPlay As New Form_play
                    formPlay.Show()
                    Hide()
                End If

            Case "Button_help"
                If chooseMode = 0 Then
                    MsgBox("Press the left mouse button to move player. When enemy reaches the attack range, player automatically attacks them.
If player level up, the stat selection window appears. And you can select one to increase player's stats.", vbOKOnly, "How to play")
                ElseIf chooseMode = 1 Then
                    difficulty = 2
                    S_WIDTH = 720
                    S_HEIGHT = 720
                    chooseMode = 2
                    GS.Stop("snd_background")
                    Dim formPlay As New Form_play
                    formPlay.Show()
                    Hide()
                Else
                    chooseMode = 0
                    Button_start.Show()
                    Button_exit.Show()
                    Button_start.Text = "START"
                    Button_help.Text = "- HELP -"
                    Button_exit.Text = "EXIT"
                    Label_name.Text = "Visual Basic Shooting Game"
                End If

            Case "Button_exit"
                If chooseMode = 0 Then
                    Dim result As DialogResult = MessageBox.Show("Are you sure to quit the game?", "Game Exit", MessageBoxButtons.YesNo)

                    If result = DialogResult.Yes Then
                        Close()
                    End If
                ElseIf chooseMode = 1 Then
                    difficulty = 3
                    S_WIDTH = 640
                    S_HEIGHT = 640
                    chooseMode = 2
                    GS.Stop("snd_background")
                    Dim formPlay As New Form_play
                    formPlay.Show()
                    Hide()
                End If
        End Select
    End Sub

    Private Sub Mouse_move(sender As Object, e As EventArgs) Handles Button_start.MouseEnter,
        Button_help.MouseEnter, Button_exit.MouseEnter
        Dim btn As Button = DirectCast(sender, Button)

        If chooseMode <> 2 Then
            Select Case btn.Name
                Case "Button_start"
                    Button_start.Focus()

                Case "Button_help"
                    Button_help.Focus()

                Case "Button_exit"
                    Button_exit.Focus()
            End Select
        End If
    End Sub

    Public Sub GS_PlaySound(ByVal SndName As String)
        If GS.IsPlaying(SndName) = False Then GS.Play(SndName)
    End Sub

    Private Sub Form_start_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        'object destroy
        Dim list_index As Short = 0

        While list_index < obj_list.Count()
            Dim obj As Object = obj_list.Item(list_index)

            obj.Die()
            obj_list.RemoveAt(list_index)
        End While

        GS.Dispose()
        Dispose()
    End Sub

End Class