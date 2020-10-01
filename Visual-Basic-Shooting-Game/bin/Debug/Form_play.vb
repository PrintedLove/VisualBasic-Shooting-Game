'made by - Im Won Ju (219124131)
'Visual Basic Class Final Exam

Imports System.IO
Imports System.Drawing
Imports System.Drawing.Text
Imports System.Reflection
Imports System.ComponentModel
Imports System.Runtime.InteropServices

Public Class Form_play

    'screen
    Dim S_WIDTH As Int16 = 640
    Dim S_HEIGHT As Int16 = 640

    'color
    Dim MAX_ALPHA As Int16 = 255
    Dim WHITE As Color = Color.FromArgb(MAX_ALPHA, 214, 231, 255)
    Dim GRAY_DEEP As Color = Color.FromArgb(MAX_ALPHA, 66, 71, 75)
    Dim GRAY_LIGHT As Color = Color.FromArgb(MAX_ALPHA, 79, 85, 91)
    Dim SKYBLUE As Color = Color.FromArgb(MAX_ALPHA, 107, 165, 247)

    'font
    Dim font_16 As Font = GetFont(Me.GetType.Assembly, "Visual_Basic_Shooting_Game.munro.ttf", 16, FontStyle.Regular)
    Dim font_32 As Font = New Font(font_16.FontFamily, 64, font_16.Style)
    Dim strFormat As New StringFormat

    'Dim screen_pic As PictureBox
    Dim screen_bmp As New Bitmap(S_WIDTH, S_HEIGHT, Imaging.PixelFormat.Format32bppArgb)

    'game control value
    Dim isRunning As Boolean = True
    Dim isGameOver As Boolean = False

    'game FPS, timer
    Dim tick, tick_start As ULong
    Dim tick_recent As ULong = 0
    Dim playtime_m, playtime_s As UInteger

    'image
    Public spr_character As Sprite = GetSprite("character.png")

    'player
    Public player_hspeed, player_vspeed As Integer

    'background
    Public bg_x, bg_y As Integer

    Private Sub Form_play_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Text = "Visual Basic Shooting Game"
        FormBorderStyle = FormBorderStyle.FixedSingle
        Width = S_WIDTH
        Height = S_HEIGHT
        'Focus()
        'Show()

        Dim bmp As New Bitmap(S_WIDTH, S_HEIGHT, Imaging.PixelFormat.Format32bppArgb)

        screen_bmp = bmp

        Using g As Graphics = Graphics.FromImage(screen_bmp)
            Me.DoubleBuffered = True
            g.Clear(GRAY_LIGHT)
        End Using

        PictureBox_play.Image = bmp

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

    Public Sub DrawSprite(ByVal g As Graphics, ByVal sprite As Sprite, ByVal x As Integer, ByVal y As Integer)
        g.DrawImage(sprite.spr, x - sprite.width \ 2, y - sprite.height \ 2)
    End Sub

    Public Sub DrawText(ByVal g As Graphics, ByVal str As String, ByVal x As Integer, ByVal y As Integer, ByVal fnt As Font, ByVal color As Color, ByVal alpha As Int16)
        Dim text_color As Color = Color.FromArgb(alpha, color.R, color.G, color.B)

        Using brush As Brush = New Drawing.SolidBrush(text_color)
            g.DrawString(str, fnt, brush, x, y, strFormat)
        End Using

        fnt.Dispose()
    End Sub

    Public Sub DrawLine(ByVal g As Graphics, ByVal pnt_x As Point, ByVal pnt_y As Point, ByVal color As Color, ByVal alpha As Int16)
        Dim line_color As Color = Color.FromArgb(alpha, color.R, color.G, color.B)

        Using brush As Brush = New Drawing.SolidBrush(line_color), pen As Pen = New Drawing.Pen(brush)
            g.DrawLine(pen, pnt_x, pnt_y)
        End Using
    End Sub

    Public Function GetSprite(ByVal file_name As String) As Sprite
        Dim strImageName As String = Application.ExecutablePath

        strImageName = strImageName.Substring(0, strImageName.LastIndexOf("\bin")) & "\image\" & file_name

        If IO.File.Exists(strImageName) Then
            Dim img As Image = Image.FromFile(strImageName)
            Dim bm As New Bitmap(width:=img.Width, height:=img.Height, format:=img.PixelFormat)

            Using g As Graphics = Graphics.FromImage(bm)
                g.DrawImage(img, Point.Empty)
            End Using

            img.Dispose()

            Return New Sprite(bm, bm.Size.Width, bm.Size.Height)
        Else
            Throw New Exception(String.Format("Cannot load _image '{0}'", strImageName))
            Return Nothing
        End If
    End Function

    Public Function GetFont(aAssembly As Assembly, strFontName As String, intFontSize As Integer, fsFontStyle As FontStyle) As Font
        Using pcolFonts As New PrivateFontCollection
            Dim bFont() As Byte = bRawFontData(aAssembly, strFontName)
            Dim ptrMemFont As IntPtr = Marshal.AllocCoTaskMem(bFont.Length)

            Marshal.Copy(bFont, 0, ptrMemFont, bFont.Length)
            pcolFonts.AddMemoryFont(ptrMemFont, bFont.Length)

            Marshal.FreeCoTaskMem(ptrMemFont)

            Return New Font(pcolFonts.Families(0), intFontSize, fsFontStyle)
        End Using
    End Function

    Private Function bRawFontData(aAssembly As Assembly, strFontName As String) As Byte()
        Using stFont As Stream = aAssembly.GetManifestResourceStream(strFontName)
            If (stFont Is Nothing) Then Throw New Exception(String.Format("Cannot load _font '{0}'", strFontName))

            Dim bFontBuffer() As Byte = New Byte(CInt(stFont.Length - 1)) {}
            stFont.Read(bFontBuffer, 0, CInt(stFont.Length))

            Return bFontBuffer
        End Using
    End Function

    Private Sub Form_play_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        PictureBox_play.Dispose()
    End Sub

    Private Sub Form_play_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If isRunning = True Then
            isRunning = False
            e.Cancel = True
        End If
    End Sub
End Class
