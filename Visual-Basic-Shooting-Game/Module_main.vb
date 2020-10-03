Imports System.Drawing.Text
Imports System.Math
Imports System.Net.NetworkInformation
'Imports System.ComponentModel
'Imports System.Drawing

Module Module_main
    'screen
    Public S_WIDTH As Int16 = 640
    Public S_HEIGHT As Int16 = 640
    Public mouse_coord As Point

    'color
    Public MAX_ALPHA As Int16 = 255
    Public WHITE As Color = Color.FromArgb(MAX_ALPHA, 214, 231, 255)
    Public GRAY_DEEP As Color = Color.FromArgb(MAX_ALPHA, 66, 71, 75)
    Public GRAY_LIGHT As Color = Color.FromArgb(MAX_ALPHA, 79, 85, 91)
    Public SKYBLUE As Color = Color.FromArgb(MAX_ALPHA, 107, 165, 247)

    'font
    Public font_munro As PrivateFontCollection = New PrivateFontCollection()
    Public font_16 As Font
    Public font_32 As Font
    Public strFormat As New StringFormat

    'image
    Public spr_character As Sprite = GetSprite("character.png")

    Public Sub DrawSprite(ByVal g As Graphics, ByVal sprite As Sprite, ByVal x As Integer, ByVal y As Integer)
        g.DrawImage(sprite.spr, x - sprite.width \ 2, y - sprite.height \ 2)
    End Sub

    Public Sub DrawText(ByVal g As Graphics, ByVal str As String, ByVal x As Integer, ByVal y As Integer, ByVal fnt As Font, ByVal color As Color, ByVal alpha As Int16)
        Dim text_color As Color = Color.FromArgb(alpha, color.R, color.G, color.B)

        Using brush As Brush = New Drawing.SolidBrush(text_color), f As Font = New Font(fnt.FontFamily, fnt.Size, fnt.Style)
            g.DrawString(str, f, brush, x, y, strFormat)
        End Using
    End Sub

    Public Sub DrawLine(ByVal g As Graphics, ByVal pnt_x As Point, ByVal pnt_y As Point, ByVal color As Color, ByVal alpha As Int16)
        Dim line_color As Color = Color.FromArgb(alpha, color.R, color.G, color.B)

        Using brush As Brush = New SolidBrush(line_color), pen As Pen = New Drawing.Pen(brush, 2)
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

    Public Function GetAngleTwoPoint(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer)
        Dim dx As Integer = x2 - x1
        Dim dy As Integer = y2 - y1

        Dim angle As Double = Atan2(dy, dx)

        Return angle
    End Function

    Public Function GetCoordCircle(ByVal x As Integer, ByVal y As Integer, ByVal dgree As Double, ByVal radius As Integer)
        Dim circle_x As Integer = x + CInt(Cos(dgree) * radius)
        Dim circle_y As Integer = y + CInt(Sin(dgree) * radius)
        Dim circle_coord As New Point(circle_x, circle_y)

        Return circle_coord
    End Function
End Module
