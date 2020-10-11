'[Visual Basic] Visual Basic Shooting Game
'made by - Printed Love
'Blog: https://printed.tistory.com
'YouTube: https://www.youtube.com/channel/UCtKTjiof6Mwa_4ffHDYyCbQ?view_as=subscriber

Imports System.ComponentModel
Imports System.Math
Imports System.Threading
Imports Microsoft.VisualBasic.CompilerServices

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

        lv = 1
        exp_present = 0
        exp_required = 100

        hp_max = 100
        hp = 100
        hp_regen = 0
        defense = 0

        atk_dam = 10
        atk_reload = 1
        atk_spd = 1
        atk_num = 1
        atk_size = 1
        atk_range = 10
        atk_penetrate = 0
        atk_explosion = 0

        critical = 0
        critical_dam = 150

        speed = 3

        player_hspeed = 0
        player_vspeed = 0
        playerMove = False

        bg_x = 0
        bg_y = 0

        tick_start = DateTime.Now.Ticks
        tick_recent = 0
        gameTick = 0

        timer_main.Start()
    End Sub

    Private Sub TimerMain()
        Do
            tick = DateTime.Now.Ticks       'current time tick

            If tick > tick_recent + 100000 Then     'initialize every 0.01 Sec
                tick_recent = tick
                playtime_s = (tick_recent - tick_start) \ 10000000      'playtime count
                playtime_m = playtime_s \ 60
                playtime_s = playtime_s Mod 60

                If gameTick < 99 Then
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
            DrawLine(g, New Point(S_WIDTH \ 2 + bg_x - Sign(bg_x) * S_WIDTH \ 2, 0), New Point(S_WIDTH \ 2 + bg_x - Sign(bg_x) * S_WIDTH \ 2, S_HEIGHT), GRAY_DEEP, MAX_ALPHA)
            DrawLine(g, New Point(0, S_HEIGHT \ 2 + bg_y - Sign(bg_y) * S_HEIGHT \ 2), New Point(S_WIDTH, S_HEIGHT \ 2 + bg_y - Sign(bg_y) * S_HEIGHT \ 2), GRAY_DEEP, MAX_ALPHA)

            'Draw objects
            For Each obj As Object In obj_list
                obj.Draw(g)
            Next

            'Draw Player
            DrawSprite(g, spr_player_core, S_WIDTH \ 2 - player_hspeed, S_HEIGHT \ 2 - player_vspeed)
            DrawSprite(g, spr_player_body, S_WIDTH \ 2, S_HEIGHT \ 2)

            'Draw LV, EXP Bar
            DrawText(g, "HP", 15, 15, font_16, WHITE, MAX_ALPHA)
            DrawLine(g, New Point(5, 14), New Point(S_WIDTH - 21, 14), GRAY_LIGHT, MAX_ALPHA, 16)
            DrawLine(g, New Point(5, 14), New Point(5 + CInt((S_WIDTH - 26) * exp_present / exp_required), 14), SKYBLUE, MAX_ALPHA, 16)

            'Draw HP Bar
            DrawSprite(g, spr_hpBar, S_WIDTH \ 2, S_HEIGHT \ 2 + 30)
            DrawLine(g, New Point(S_WIDTH \ 2 - 13, S_HEIGHT \ 2 + 30), New Point(S_WIDTH \ 2 - 13 + CInt(27 * hp / hp_max), S_HEIGHT \ 2 + 30), WHITE, MAX_ALPHA, 3)

            'Draw Time
            DrawText(g, Format(playtime_m, "00") & " : " & Format(playtime_s, "00"), S_WIDTH \ 2, 75, font_16, WHITE, MAX_ALPHA)
        End Using

        If Not PictureBox_play.Image.Equals(bmp) Then
            PictuerBox_play_SetImage(bmp)
        End If
    End Sub

    Private Sub ObjectControl()
        For Each obj As Object In obj_list
            obj.DefaultEvent()
        Next
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
    End Sub

    Private Sub PlayerControl()
        'Check EXP
        If exp_present >= exp_required Then
            Dim exp_excess As UInteger = exp_present - exp_required

            lv += 1
            exp_present = exp_excess
            exp_required = Round(exp_required * 1.01 + 25)
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
        CreateEnemy()
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