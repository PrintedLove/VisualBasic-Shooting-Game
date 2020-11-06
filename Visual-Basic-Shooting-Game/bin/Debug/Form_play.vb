'[Visual Basic] Visual Basic Shooting Game

'made by - Printed Love
'Blog: https://printed.tistory.com
'YouTube: https://youtube.com/channel/UCtKTjiof6Mwa_4ffHDYyCbQ

Imports System.ComponentModel
Imports System.Math
Imports System.Threading

Public Class Form_play

    'picturebox bitmap
    Private screen_bmp As New Bitmap(S_WIDTH, S_HEIGHT, Imaging.PixelFormat.Format32bppArgb)

    'game control
    Private isRunning As Boolean = True
    Private isGameOver As Boolean = False

    Private timer_main As Thread

    Private Sub Form_play_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "Visual Basic Shooting Game"
        FormBorderStyle = FormBorderStyle.FixedSingle
        Width = S_WIDTH
        Height = S_HEIGHT
        PictureBox_play.Size = New Size(S_WIDTH, S_HEIGHT)

        'picture box initialize
        Dim bmp As New Bitmap(S_WIDTH, S_HEIGHT, Imaging.PixelFormat.Format32bppArgb)

        screen_bmp.Dispose()
        screen_bmp = bmp

        Using g As Graphics = Graphics.FromImage(screen_bmp)
            Me.DoubleBuffered = True
            g.Clear(GRAY)
        End Using

        PictureBox_play.Image = bmp
        PictureBox_play.Refresh()

        'main timer thread
        timer_main = New Thread(AddressOf TimerMain) With {
            .IsBackground = True
        }

        'start game
        SetValue()
        NewGame()
    End Sub

    Private Sub NewGame()
        isRunning = True
        isGameOver = False

        stage = 0

        lv = 1
        exp_present = 0
        exp_required = 100

        hp_max = 100
        hp = 100
        hp_regen = 0
        defense = 0

        atk_dam = 15
        atk_reload = 0.5
        atk_spd = 12
        atk_num = 4
        atk_size = 3
        atk_range = 200
        atk_penetrate = 1
        atk_explosion = 20

        critical = 25
        critical_dam = 200

        speed = 10

        player_hspeed = 0
        player_vspeed = 0
        playerMove = False

        bg_x = 0
        bg_y = 0

        enemy_num = 0
        enemy_numMax = 16
        item_num = 0
        obj_list.Clear()
        EnemyDistance = 9999

        tick_start = DateTime.Now.Ticks
        tick_recent = 0
        tick_attack = 0
        gameTick = 0

        timer_main.Start()
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

                GameEvent()

                If isGameOver Then
                    timer_main.Abort()
                End If

                If Not isRunning Then
                    Form_play_ToClose()
                End If
            End If

            'player attack
            If EnemyDistance <= atk_range And tick > tick_attack + atk_reload * 10000000 Then
                tick_attack = tick
                EnemyDistance = 9999

                For i As Int16 = 1 To atk_num
                    CreateObject(3, i - 1)
                Next
            End If
        Loop
    End Sub

    Private Sub GameEvent()
        Application.DoEvents()

        BackgroundControl()
        ObjectControl()
        PlayerControl()
        DrawGraphics()
    End Sub

    Private Sub DrawGraphics()
        Dim bmp As New Bitmap(PictureBox_play.Image)

        If Not screen_bmp.Equals(bmp) Then
            screen_bmp.Dispose()
            screen_bmp = bmp
        End If

        Using g As Graphics = Graphics.FromImage(bmp)
            Me.DoubleBuffered = True
            g.Clear(GRAY)

            'Draw Background Lines
            DrawLine(g, New Point(S_WIDTH \ 2 + bg_x, 0), New Point(S_WIDTH \ 2 + bg_x, S_HEIGHT), GRAY_DEEP, MAX_ALPHA)
            DrawLine(g, New Point(0, S_HEIGHT \ 2 + bg_y), New Point(S_WIDTH, S_HEIGHT \ 2 + bg_y), GRAY_DEEP, MAX_ALPHA)
            DrawLine(g, New Point(S_WIDTH \ 2 + bg_x - Sign(bg_x) * S_WIDTH \ 2, 0),
                     New Point(S_WIDTH \ 2 + bg_x - Sign(bg_x) * S_WIDTH \ 2, S_HEIGHT), GRAY_DEEP, MAX_ALPHA)
            DrawLine(g, New Point(0, S_HEIGHT \ 2 + bg_y - Sign(bg_y) * S_HEIGHT \ 2),
                     New Point(S_WIDTH, S_HEIGHT \ 2 + bg_y - Sign(bg_y) * S_HEIGHT \ 2), GRAY_DEEP, MAX_ALPHA)

            'Draw objects
            For Each obj As Object In obj_list
                obj.Draw(g)
            Next

            'Draw Player
            DrawSprite(g, spr_player_core, S_WIDTH \ 2 - player_hspeed \ 2, S_HEIGHT \ 2 - player_vspeed \ 2)
            DrawSprite(g, spr_player_body, S_WIDTH \ 2, S_HEIGHT \ 2)

            'Draw LV, EXP Bar
            DrawLine(g, New Point(5, 14), New Point(S_WIDTH - 21, 14), GRAY_LIGHT, MAX_ALPHA, 16)
            DrawLine(g, New Point(5, 14), New Point(5 + CInt((S_WIDTH - 26) * exp_present / exp_required), 14), SKYBLUE, MAX_ALPHA, 16)
            DrawText(g, "LV. " & CStr(lv), 40, 15, font_16, WHITE, MAX_ALPHA)

            'Draw HP Bar
            DrawSprite(g, spr_hpBar, S_WIDTH \ 2, S_HEIGHT \ 2 + 30)
            DrawLine(g, New Point(S_WIDTH \ 2 - 13, S_HEIGHT \ 2 + 30), New Point(S_WIDTH \ 2 - 13 + CInt(27 * hp / hp_max),
                                                                                  S_HEIGHT \ 2 + 30), WHITE, MAX_ALPHA, 3)

            'Draw Time
            DrawText(g, Format(playtime_m, "00") & " : " & Format(playtime_s, "00"), S_WIDTH \ 2, 75, font_16, WHITE, MAX_ALPHA)
        End Using

        If Not PictureBox_play.Image.Equals(bmp) Then
            PictuerBox_play_SetImage(bmp)
        End If
    End Sub

    Private Sub ObjectControl()
        'object event
        Dim list_index As Int16 = 0

        While list_index < obj_list.Count()
            Dim obj As Object = obj_list.Item(list_index)
            obj.Index = list_index
            obj.IndividualEvent()

            If obj.kill Then
                obj.Die()

                Select Case obj.type
                    Case 0 To 9
                        item_num -= 1

                    Case 10 To 99
                        enemy_num -= 1
                End Select

                obj_list.RemoveAt(list_index)
            Else
                list_index += 1
            End If
        End While

        'enemy object create,  number control
        If enemy_num < enemy_numMax And gameTick Mod 25 = 0 Then
            CreateObject(1)
        End If

        'item object create, number control
        If item_num < 16 Then
            CreateObject(2)
        End If
    End Sub

    Private Sub BackgroundControl()
        'Background Line Move
        bg_x += player_hspeed
        bg_y += player_vspeed

        If (bg_x > S_WIDTH \ 2 Or bg_x < -S_WIDTH \ 2) Then
            bg_x -= Sign(bg_x) * S_WIDTH \ 2
        End If

        If (bg_y > S_HEIGHT \ 2 Or bg_y < -S_HEIGHT \ 2) Then
            bg_y -= Sign(bg_y) * S_HEIGHT \ 2
        End If

        'stage up by time
        If stage < 9 And timeToDif(difficulty - 1, stage) < (tick_recent - tick_start) / 600000000 Then
            stage += 1
        End If
    End Sub

    Private Sub PlayerControl()
        'Check EXP
        If exp_present >= exp_required Then
            Dim exp_excess As UInteger = exp_present - exp_required

            lv += 1
            exp_present = exp_excess
            exp_required = Round(exp_required * 1.01 + 25)
        End If

        If gameTick = 0 Then

            'Check HP
            If hp < hp_max Then
                If hp_regen > 0 Then
                    hp += hp_regen
                End If

                If hp > hp_max Then
                    hp = hp_max
                End If
            End If
        End If

        'Move Control
        If playerMove Then
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

    Delegate Sub ToClose()

    Delegate Sub SetImage(ByVal img As Bitmap)

    Private Sub Form_play_ToClose()
        If InvokeRequired Then
            Dim c As ToClose = New ToClose(AddressOf Form_play_ToClose)
            Me.Invoke(c)
        Else
            Close()
        End If
    End Sub

    Private Sub PictuerBox_play_SetImage(ByVal img As Bitmap)
        If Me.PictureBox_play.InvokeRequired Then
            Dim si As SetImage = New SetImage(AddressOf PictuerBox_play_SetImage)
            Me.Invoke(si, img)
        Else
            PictureBox_play.Image = img
            PictureBox_play.Refresh()
        End If
    End Sub

    Private Sub PictureBox_play_MouseMove(sender As Object, e As MouseEventArgs) Handles PictureBox_play.MouseMove
        mouse_coord = e.Location
    End Sub

    Private Sub PictureBox_play_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox_play.MouseDown
        playerMove = True
    End Sub

    Private Sub PictureBox_play_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox_play.MouseUp
        playerMove = False
    End Sub

    Private Sub Form_play_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        screen_bmp.Dispose()
        PictureBox_play.Dispose()
        timer_main.Abort()
    End Sub

    Private Sub Form_play_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If isRunning = True Then
            isRunning = False
            e.Cancel = True
        End If
    End Sub
End Class