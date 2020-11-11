'[Visual Basic] Visual Basic Shooting Game

'made by - Printed Love
'Blog: https://printed.tistory.com
'YouTube: https://youtube.com/channel/UCtKTjiof6Mwa_4ffHDYyCbQ

Public Class Form_start

    Private Sub Form_start_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormBorderStyle = FormBorderStyle.FixedSingle
        Button_start.Font = font_16

        SetValue()
    End Sub

    Private Sub Button_start_Click(sender As Object, e As EventArgs) Handles Button_start.Click
        'toggle to Form_play

        Form_play.Show()
        Hide()
    End Sub
End Class