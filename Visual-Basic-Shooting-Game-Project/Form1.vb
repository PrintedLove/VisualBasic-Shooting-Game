'made by "Im Won Ju (219124131)"
'Visual Basic Class Final Exam

Imports System.ComponentModel
Imports System.Drawing

Public Class Form_play

    'graphic
    Dim graphic As Graphics
    Dim graphic_BeforeScreen As Graphics
    Dim BeforeScreen As Bitmap

    'game control value
    Dim isRunning As Boolean = True
    Dim isGameOver As Boolean = False

    'game FPS
    Dim tick As ULong = DateTime.Now.Ticks
    Dim last_tick As ULong = 0

    Private Sub Form_play_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "Visual Basic Shooting Game"
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.Show()
        Me.Focus()

        graphic = Me.CreateGraphics
        graphic_BeforeScreen = Me.CreateGraphics
        BeforeScreen = New Bitmap(Me.Width, Me.Height)

        StartGameLoop()
    End Sub

    Private Sub StartGameLoop()
        Do While isRunning = True
            tick = DateTime.Now.Ticks

            If tick > last_tick + 200000 Then
                last_tick = tick

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


        'draw time
        graphic.DrawString(DateTime.Now.Millisecond.ToString, Me.Font, Brushes.LightGray, 320, 10)

        'draw character
        graphic.FillRectangle(Brushes.WhiteSmoke, 312, 312, 16, 16)

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