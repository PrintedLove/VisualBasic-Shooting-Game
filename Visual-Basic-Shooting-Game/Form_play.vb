'made by - Im Won Ju (219124131)
'Visual Basic Class Final Exam

Imports System.Drawing.Text
Imports System.ComponentModel
'Imports System.Drawing

Public Class Form_play

    'picturebox bitmap
    Dim screen_bmp As New Bitmap(S_WIDTH, S_HEIGHT, Imaging.PixelFormat.Format32bppArgb)

    'game control value
    Dim isRunning As Boolean = True
    Dim isGameOver As Boolean = False

    'game FPS, timer
    Dim tick, tick_start As ULong
    Dim tick_recent As ULong = 0
    Dim playtime_m, playtime_s As UInteger

    'player
    Public player_hspeed, player_vspeed As Integer

    'background
    Public bg_x, bg_y As Integer

    Private Sub Form_play_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "Visual Basic Shooting Game"
        FormBorderStyle = FormBorderStyle.FixedSingle
        Width = S_WIDTH
        Height = S_HEIGHT

        Dim bmp As New Bitmap(S_WIDTH, S_HEIGHT, Imaging.PixelFormat.Format32bppArgb)

        screen_bmp.Dispose()
        screen_bmp = bmp

        Using g As Graphics = Graphics.FromImage(screen_bmp)
            Me.DoubleBuffered = True
            g.Clear(GRAY_LIGHT)
        End Using

        PictureBox_play.Image = bmp
        PictureBox_play.Refresh()

        Dim strFontName As String = Application.ExecutablePath
        strFontName = strFontName.Substring(0, strFontName.LastIndexOf("\bin")) & "\font\munro.ttf"

        font_munro.AddFontFile(strFontName)
        font_16 = New Font(font_munro.Families(0), 16)
        font_32 = New Font(font_munro.Families(0), 32)
        strFormat.LineAlignment = StringAlignment.Center
        strFormat.Alignment = StringAlignment.Center

        bg_x = 0
        bg_y = 0

        StartGameLoop()
    End Sub

    Private Sub StartGameLoop()
        tick_start = DateTime.Now.Ticks     'start tick initialise

        Do While isRunning = True
            tick = DateTime.Now.Ticks       'current time tick

            If tick > tick_recent + 200000 Then     'game initialize every 0.02 Sec
                tick_recent = tick
                playtime_s = (tick - tick_start) \ 10000000      'playtime count
                playtime_m = playtime_s \ 60
                playtime_s = playtime_s Mod 60

                Application.DoEvents()

                DrawGraphics()
            End If
        Loop

        Close()
    End Sub

    Private Sub DrawGraphics()
        Dim bmp As New Bitmap(PictureBox_play.Image)

        If Not screen_bmp.Equals(bmp) Then
            screen_bmp.Dispose()
            screen_bmp = bmp
        End If

        Using g As Graphics = Graphics.FromImage(bmp)
            Me.DoubleBuffered = True
            g.Clear(GRAY_LIGHT)
            DrawLine(g, New Point(S_WIDTH \ 2 + bg_x, 0), New Point(S_WIDTH \ 2 + bg_x, S_HEIGHT), GRAY_DEEP, MAX_ALPHA)
            DrawLine(g, New Point(0, S_HEIGHT \ 2 + bg_y), New Point(S_WIDTH, S_HEIGHT \ 2 + bg_y), GRAY_DEEP, MAX_ALPHA)
            DrawText(g, Format(playtime_m, "00") & " : " & Format(playtime_s, "00"), S_WIDTH \ 2, 50, font_16, WHITE, MAX_ALPHA)
            DrawSprite(g, spr_character, S_WIDTH \ 2, S_HEIGHT \ 2)
        End Using

        If Not PictureBox_play.Image.Equals(bmp) Then
            PictureBox_play.Image = bmp
            PictureBox_play.Refresh()
        End If
    End Sub

    Private Sub Form_play_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        screen_bmp.Dispose()
        PictureBox_play.Dispose()
    End Sub

    Private Sub Form_play_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If isRunning = True Then
            isRunning = False
            e.Cancel = True
        End If
    End Sub
End Class
