'[Visual Basic] Visual Basic Shooting Game

'made by - Printed Love
'Blog: https://printed.tistory.com
'YouTube: https://youtube.com/channel/UCtKTjiof6Mwa_4ffHDYyCbQ

Imports System.Math
Imports System.Threading

Public Class Form_play
    'picturebox bitmap
    Private screen_bmp As New Bitmap(S_WIDTH, S_HEIGHT, Imaging.PixelFormat.Format32bppArgb)

    'game control
    Private timer_main As Thread

    Private Sub Form_play_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "Visual Basic Shooting Game"
        FormBorderStyle = FormBorderStyle.FixedSingle
        Width = S_WIDTH
        Height = S_HEIGHT
        BackColor = GRAY
        AutoSizeMode = 0
        FormBorderStyle = 1
        player_rec = New Rectangle(S_WIDTH \ 2, S_HEIGHT \ 2, 25, 25)

        'picture box initialize
        PictureBox_play.Size = New Size(S_WIDTH, S_HEIGHT)

        Dim bmp As New Bitmap(S_WIDTH, S_HEIGHT, Imaging.PixelFormat.Format32bppArgb)

        screen_bmp = bmp

        Using g As Graphics = Graphics.FromImage(screen_bmp)
            Me.DoubleBuffered = True
            g.Clear(GRAY)
        End Using

        PictureBox_play.Image = bmp

        'main timer thread
        timer_main = New Thread(AddressOf TimerMain) With {
            .IsBackground = True
        }
    End Sub

    Private Sub Form_play_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        NewGame()
        timer_main.Start()
        Enabled = True
    End Sub

    Private Sub TimerMain()
        Do
            tick = DateTime.Now.Ticks       'current time tick

            'game event every gametick
            If tick > tick_recent + 333333 Then     'initialize every 0.03 Sec
                tick_recent = tick
                playtime_s = (tick_recent - tick_start) \ 10000000      'playtime count
                playtime_m = playtime_s \ 60
                playtime_s = playtime_s Mod 60

                If gameTick < 32 Then
                    gameTick += 1
                Else
                    gameTick = 0
                End If

                Application.DoEvents()
                BackgroundControl()
                ObjectControl()
                PlayerControl()
                DrawGraphics()
            End If

            'player attack
            If EnemyDistance <= atk_range And tick > tick_attack + atk_reload * 10000000 Then
                tick_attack = tick
                EnemyDistance = 9999

                Invoke(Sub() GS_PlaySound("snd_shoot"))

                For i As Short = 1 To atk_num
                    CreateObject(3, i - 1)
                Next
            End If
        Loop
    End Sub

    Private Sub DrawGraphics()
        Dim bmp As Bitmap = PictureBox_play_GetImage()

        If PictureBox_play.InvokeRequired = True Then
            bmp = PictureBox_play.Invoke(Function() PictureBox_play_GetImage())
        Else
            bmp = PictureBox_play_GetImage()
        End If

        If Not screen_bmp.Equals(bmp) Then
            screen_bmp = bmp
        End If

        Using g As Graphics = Graphics.FromImage(bmp)
            Me.DoubleBuffered = True
            g.Clear(GRAY)

            'Draw Background Lines
            DrawLine(g, New Point(S_WIDTH \ 2 + backgound_x, 0), New Point(S_WIDTH \ 2 + backgound_x, S_HEIGHT), GRAY_DEEP, MAX_ALPHA)
            DrawLine(g, New Point(0, S_HEIGHT \ 2 + backgound_y), New Point(S_WIDTH, S_HEIGHT \ 2 + backgound_y), GRAY_DEEP, MAX_ALPHA)
            DrawLine(g, New Point(S_WIDTH \ 2 + backgound_x - Sign(backgound_x) * S_WIDTH \ 2, 0),
                     New Point(S_WIDTH \ 2 + backgound_x - Sign(backgound_x) * S_WIDTH \ 2, S_HEIGHT), GRAY_DEEP, MAX_ALPHA)
            DrawLine(g, New Point(0, S_HEIGHT \ 2 + backgound_y - Sign(backgound_y) * S_HEIGHT \ 2),
                     New Point(S_WIDTH, S_HEIGHT \ 2 + backgound_y - Sign(backgound_y) * S_HEIGHT \ 2), GRAY_DEEP, MAX_ALPHA)

            'Draw objects
            For Each obj As Object In obj_list
                obj.Draw(g)
            Next

            'Draw Player
            DrawSprite(g, spr_player_core, S_WIDTH \ 2 - player_hspeed \ 2, S_HEIGHT \ 2 - player_vspeed \ 2)
            DrawSprite(g, spr_player_body, S_WIDTH \ 2, S_HEIGHT \ 2)

            'Draw LV, EXP, HP Bar
            DrawLine(g, New Point(5, 14), New Point(S_WIDTH - 21, 14), GRAY_LIGHT, MAX_ALPHA, 16)
            DrawLine(g, New Point(5, 14), New Point(5 + CInt((S_WIDTH - 26) * exp_present / exp_required), 14), SKYBLUE, MAX_ALPHA, 16)
            DrawText(g, "LV. " & CStr(lv), 40, 2, font_16, WHITE, MAX_ALPHA)
            DrawSprite(g, spr_hpBar, S_WIDTH \ 2, S_HEIGHT \ 2 + 30)
            DrawLine(g, New Point(S_WIDTH \ 2 - 13, S_HEIGHT \ 2 + 30), New Point(S_WIDTH \ 2 - 13 + CInt(27 * hp / hp_max),
                                                                                  S_HEIGHT \ 2 + 30), WHITE, MAX_ALPHA, 3)

            'Draw Time
            DrawText(g, Format(playtime_m, "00") & " : " & Format(playtime_s, "00"), S_WIDTH \ 2, 75, font_16, WHITE, MAX_ALPHA)

            'Draw Stat LV
            DrawText(g, statText, 64, 48, font_8, WHITE, MAX_ALPHA)

            'Draw Stat Window
            If showStatWindow > 0 Then
                For i As Short = 0 To 2
                    DrawLine(g, New Point(S_WIDTH \ 2 - 256 + i * 175, S_HEIGHT - 144),
                             New Point(S_WIDTH \ 2 - 105 + i * 175, S_HEIGHT - 144), statBnt_color(i), 160, 96)
                    DrawSprite(g, spr_skillicon, statBnt_statTpye(i), S_WIDTH \ 2 - 208 + i * 175, S_HEIGHT - 144)
                    DrawText(g, statBnt_Text(i), S_WIDTH \ 2 - 140 + i * 175, S_HEIGHT - 160,
                             font_8, WHITE, MAX_ALPHA)
                    DrawText(g, "LV. " & CStr(statBnt_statLV(i)), S_WIDTH \ 2 - 140 + i * 175, S_HEIGHT - 128,
                             font_12, SKYBLUE, MAX_ALPHA)
                Next
            End If
        End Using

        'Renewal PictureBox_play image
        If PictureBox_play.InvokeRequired = True Then
            PictureBox_play.Invoke(Sub() PictureBox_play_SetImage(bmp))
        Else
            PictureBox_play_SetImage(bmp)
        End If

    End Sub

    Private Sub ObjectControl()
        'Object Event
        Dim list_index As Short = 0

        While list_index < obj_list.Count()
            Dim obj As Object = obj_list.Item(list_index)
            obj.Index = list_index
            obj.IndividualEvent()

            'object destroy
            If obj.kill = True Then
                obj.Die()

                Select Case obj.type
                    Case 0 To 9
                        item_num -= 1
                        If obj.getItem = True Then Invoke(Sub() GS_PlaySoundOnce("snd_getExp"))

                    Case 10 To 99
                        enemy_num -= 1

                    Case 100
                        If atk_explosion > 0 Then Invoke(Sub() GS_PlaySoundOnce("snd_explosion"))
                End Select

                obj_list.RemoveAt(list_index)
            Else
                list_index += 1
            End If
        End While

        'Enemy object create, number control
        If enemy_num < enemy_numMax And gameTick Mod 25 = 0 Then
            CreateObject(1)
        End If

        'Item object create, number control
        If item_num < 16 Then
            CreateObject(2)
        End If
    End Sub

    Private Sub BackgroundControl()
        'Background Line Move
        backgound_x += player_hspeed
        backgound_y += player_vspeed

        If (backgound_x > S_WIDTH \ 2 Or backgound_x < -S_WIDTH \ 2) Then
            backgound_x -= Sign(backgound_x) * S_WIDTH \ 2
        End If

        If (backgound_y > S_HEIGHT \ 2 Or backgound_y < -S_HEIGHT \ 2) Then
            backgound_y -= Sign(backgound_y) * S_HEIGHT \ 2
        End If

        Dim time_m As Double = (tick_recent - tick_start) / 60000000

        'Stage up by Time
        If stage < 9 And timeToDif(difficulty - 1, stage) < time_m \ 10 Then
            stage += 1
        End If

        If timeToEA * 10 < time_m Then
            timeToEA += 0.01
        End If

        'Check Stat Window Button
        If showStatWindow > 0 Then
            For i As Short = 0 To 2
                If S_WIDTH \ 2 - 256 + i * 175 < mouse_coord.X And mouse_coord.X < S_WIDTH \ 2 - 105 + i * 175 And
                   S_HEIGHT - 192 < mouse_coord.Y And mouse_coord.Y < S_HEIGHT - 96 Then
                    statBnt_touch(i) = True
                    statBnt_color(i) = GRAY_LIGHT
                Else
                    statBnt_touch(i) = False
                    statBnt_color(i) = GRAY_DEEP
                End If
            Next
        End If
    End Sub

    Private Sub PlayerControl()
        'Check EXP
        If exp_present >= exp_required Then
            Dim exp_excess As UInteger = exp_present - exp_required

            lv += 1
            exp_present = exp_excess
            exp_required = Round(exp_required * 1.01 + 25)

            Invoke(Sub() GS_PlaySound("snd_lvUp"))

            'stat window open
            showStatWindow += 1

            SetStatWindow()
        End If

        'Check HP
        If gameTick = 0 Then
            If hp < hp_max Then
                If hp_regen > 0 Then
                    hp += hp_regen
                End If

                If hp > hp_max Then
                    hp = hp_max
                End If
            End If
        End If

        If hp < 1 Then
            If Me.InvokeRequired = True Then
                Me.Invoke(Sub() Me.Close())
            Else
                Close()
            End If
        End If

        'Player Move Control
        If playerMove = True Then
            Dim mouse_angle As Double = GetAngleTwoPoint(S_WIDTH \ 2, S_HEIGHT \ 2, mouse_coord.X, mouse_coord.Y)
            Dim player_moveCoord As Point = GetCoordCircle(0, 0, mouse_angle, speed)

            player_hspeed = -player_moveCoord.X
            player_vspeed = -player_moveCoord.Y
        Else
            If player_hspeed <> 0 Then
                player_hspeed -= Sign(player_hspeed) * Max(Abs(player_hspeed \ 3), 1)
            End If

            If player_vspeed <> 0 Then
                player_vspeed -= Sign(player_vspeed) * Max(Abs(player_vspeed \ 3), 1)
            End If
        End If
    End Sub

    Sub GS_PlaySound(ByVal SndName As String)
        GS.Play(SndName)
    End Sub

    Sub GS_PlaySoundOnce(ByVal SndName As String)
        If GS.IsPlaying(SndName) = False Then GS.Play(SndName)
    End Sub

    Function PictureBox_play_GetImage()
        Return PictureBox_play.Image
    End Function

    Sub PictureBox_play_SetImage(ByVal img As Bitmap)
        PictureBox_play.Image = img
    End Sub

    Private Sub PictureBox_play_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox_play.MouseMove
        mouse_coord = e.Location
    End Sub

    Private Sub PictureBox_play_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox_play.MouseDown
        playerMove = True
    End Sub

    Private Sub PictureBox_play_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox_play.MouseUp
        playerMove = False

        'Click stat choose button
        If showStatWindow > 0 Then
            Dim editStatText As Boolean = False

            For i As Short = 0 To 2
                If statBnt_touch(i) = True Then
                    SetStatLV(statBnt_statTpye(i))

                    showStatWindow -= 1
                    editStatText = True

                    If showStatWindow > 0 Then
                        SetStatWindow()
                    End If
                End If
            Next

            'rewrite stat info text
            If editStatText = True Then
                statText = ""

                For i As Short = 0 To 14
                    Dim statValue = GetStatLV(i)

                    If statValue(0) > 1 Then
                        statText += statValue(1) & " LV." & CStr(statValue(0) - 1) & vbCrLf
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub Form_play_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        screen_bmp.Dispose()
        timer_main.Abort()

        'Object Destroy
        Dim list_index As Short = 0

        While list_index < obj_list.Count()
            Dim obj As Object = obj_list.Item(list_index)

            obj.Die()
            obj_list.RemoveAt(list_index)
        End While

        'Toggle to Form_start
        With Form_start
            .Show()
            .Button_help.Focus()
            .Button_help.Text = "- BACK -"
            .Button_exit.Hide()
            .Button_start.Hide()
            .Label_name.Text = "SCORE: " & Format(playtime_m, "00") & " : " & Format(playtime_s, "00")
            .GS_PlaySound("snd_background")
        End With

        Dispose()
    End Sub

End Class