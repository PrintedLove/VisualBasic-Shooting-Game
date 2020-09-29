'made by - Im Won Ju (219124131)
'Visual Basic Class Final Exam

Imports System.IO
Imports System.Drawing
Imports System.Drawing.Text
Imports System.Reflection
Imports System.ComponentModel
Imports System.Runtime.InteropServices

Public Class Form_play

    'screen graphic
    Dim graphic, graphic_beforeScreen As Graphics
    Dim BeforeScreen As Bitmap

    'color
    Const MAX_ALPHA As Int16 = 255
    Dim WHITE As Color = Color.FromArgb(MAX_ALPHA, 214, 231, 255)
    Dim GRAY_DEEP As Color = Color.FromArgb(MAX_ALPHA, 66, 71, 75)
    Dim GRAY_LIGHT As Color = Color.FromArgb(MAX_ALPHA, 79, 85, 91)
    Dim SKYBLUE As Color = Color.FromArgb(MAX_ALPHA, 107, 165, 247)

    'image
    Dim spr_character As Sprite

    'game control value
    Dim isRunning As Boolean = True
    Dim isGameOver As Boolean = False

    'game FPS, timer
    Dim tick, tick_start As ULong
    Dim tick_recent As ULong = 0
    Dim playtime_m, playtime_s As UInteger

    'font, stringformat
    Dim font_32 As Font
    Dim strFormat As New StringFormat

    Private Sub Form_play_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Visual Basic Shooting Game"
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.Font = Me.GetFont(Me.GetType.Assembly, "Visual_Basic_Shooting_Game_Project.munro.ttf", 16, FontStyle.Regular)       'set default font

        'Me.Font = font2
        Me.Width = 640
        Me.Height = 640
        Me.Show()
        Me.Focus()

        graphic = Me.CreateGraphics
        graphic_beforeScreen = Me.CreateGraphics
        BeforeScreen = New Bitmap(Me.Width, Me.Height)

        font_32 = New Font(Me.Font.FontFamily, 64, Me.Font.Style)
        strFormat.LineAlignment = StringAlignment.Center
        strFormat.Alignment = StringAlignment.Center

        spr_character = GetSprite("\Resources\image\character.png")
        spr_character.x = Me.Width \ 2
        spr_character.y = Me.Height \ 2

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

                'If isGameOver = False Then

                'End If
            End If
        Loop

        Me.Close()
    End Sub

    Private Sub DrawGraphics()
        'draw background

        'draw runtime
        DrawText(Format(playtime_m, "00") & " : " & Format(playtime_s, "00"), Me.Width \ 2, 50, Me.Font, WHITE, MAX_ALPHA)

        'draw character
        DrawSprite(spr_character)

        'draw before screen
        graphic = Graphics.FromImage(BeforeScreen)
        graphic_beforeScreen.DrawImage(BeforeScreen, 0, 0, Me.Width, Me.Height)

        'clear
        graphic.Clear(GRAY_LIGHT)
    End Sub

    Private Sub DrawSprite(ByVal sprite As Sprite)
        graphic.DrawImage(sprite.spr, sprite.x - sprite.width \ 2, sprite.y - sprite.height \ 2)
    End Sub

    Private Sub DrawText(ByVal str As String, ByVal x As Integer, ByVal y As Integer, ByVal fnt As Font, ByVal color As Color, ByVal alpha As Int16)
        Dim text_color As Color = Color.FromArgb(alpha, color.R, color.G, color.B)
        Dim brush = New Drawing.SolidBrush(text_color)
        graphic.DrawString(str, fnt, brush, x, y, strFormat)
    End Sub

    Public Function GetSprite(ByVal file_name As String) As Sprite
        Dim strImageName As String = Application.ExecutablePath

        strImageName = strImageName.Substring(0, strImageName.LastIndexOf("\bin")) & file_name

        If IO.File.Exists(strImageName) Then
            Dim img As Image = Image.FromFile(strImageName)
            Dim bm As New Bitmap(width:=img.Width, height:=img.Height, format:=img.PixelFormat)

            Using g As Graphics = Graphics.FromImage(bm)
                g.DrawImage(img, Point.Empty)
            End Using

            img.Dispose()

            Dim spr As New Sprite(bm, 0, 0, bm.Size.Width, bm.Size.Height)

            Return spr
        Else
            Throw New Exception(String.Format("Cannot load _image '{0}'", strImageName))
            Return Nothing
        End If
    End Function

    Public Function GetFont(aAssembly As Assembly, strFontName As String, intFontSize As Integer, fsFontStyle As FontStyle) As Font
        Using pcolFonts As New PrivateFontCollection
            Dim bFont() As Byte = Me.bRawFontData(aAssembly, strFontName)
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
        graphic.Dispose()
        graphic_beforeScreen.Dispose()
        BeforeScreen.Dispose()
    End Sub

    Private Sub Form_play1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If isRunning = True Then
            isRunning = False
            e.Cancel = True
        End If
    End Sub
End Class