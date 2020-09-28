'made by - Im Won Ju (219124131)
'Visual Basic Class Final Exam

Imports System.IO
Imports System.Drawing
Imports System.Drawing.Text
Imports System.Reflection
Imports System.ComponentModel
Imports System.Runtime.InteropServices

Public Class Form_play

    'graphic
    Dim graphic, graphic_BeforeScreen As Graphics
    Dim BeforeScreen As Bitmap

    'game control value
    Dim isRunning As Boolean = True
    Dim isGameOver As Boolean = False

    'game FPS, timer
    Dim tick, tick_start As ULong
    Dim tick_recent As ULong = 0
    Dim runtime_m, runtime_s As UInteger

    Dim strFormat As New StringFormat

    Private Sub Form_play_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Visual Basic Shooting Game"
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.Font = Me.GetFont(Me.GetType.Assembly, "Visual_Basic_Shooting_Game_Project.munro.ttf", 16, FontStyle.Bold)
        Me.Width = 640
        Me.Height = 640
        Me.Show()
        Me.Focus()

        graphic = Me.CreateGraphics
        graphic_BeforeScreen = Me.CreateGraphics
        BeforeScreen = New Bitmap(Me.Width, Me.Height)
        strFormat.LineAlignment = StringAlignment.Center
        strFormat.Alignment = StringAlignment.Center

        StartGameLoop()
    End Sub

    Private Sub StartGameLoop()
        tick_start = DateTime.Now.Ticks

        Do While isRunning = True
            tick = DateTime.Now.Ticks

            If tick > tick_recent + 200000 Then     '0.02 Sec game Initialize
                tick_recent = tick
                runtime_s = (tick - tick_start) \ 10000000
                runtime_m = runtime_s \ 60
                runtime_s = runtime_s Mod 60
                Application.DoEvents()

                If isGameOver = False Then
                    DrawGraphics()
                Else
                    DrawGraphics()
                End If
            End If
        Loop

        Me.Close()
    End Sub

    Private Sub DrawGraphics()
        'draw background

        'draw runtime
        graphic.DrawString(Format(runtime_m, "00") & " : " & Format(runtime_s, "00"), Me.Font, Brushes.LightGray, Me.Width \ 2, 50, strFormat)

        'draw character
        graphic.FillRectangle(Brushes.WhiteSmoke, Me.Width \ 2 - 8, Me.Height \ 2 - 8, 16, 16)

        If isGameOver = True Then
            graphic.DrawString("GAME OVER", Me.Font, Brushes.Red, 550, 40)
            graphic.DrawString("Press F1 to restart game or Esc to quit", Me.Font, Brushes.Black, 550, 70)
        End If

        'draw before screen
        graphic = Graphics.FromImage(BeforeScreen)
        graphic_BeforeScreen.DrawImage(BeforeScreen, 0, 0, Me.Width, Me.Height)

        'clear
        graphic.Clear(Color.DimGray)
    End Sub

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
        graphic_BeforeScreen.Dispose()
        BeforeScreen.Dispose()
    End Sub

    Private Sub Form_play1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If isRunning = True Then
            isRunning = False
            e.Cancel = True
        End If
    End Sub
End Class