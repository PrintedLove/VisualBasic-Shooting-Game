Imports System.Drawing.Text
Imports System.Math

Module Module_main
    'screen
    Public S_WIDTH As Int16 = 640
    Public S_HEIGHT As Int16 = 640
    Public mouse_coord As Point

    'color
    Public MAX_ALPHA As Int16 = 255
    Public WHITE As Color = Color.FromArgb(MAX_ALPHA, 214, 231, 255)
    Public GRAY_DEEP As Color = Color.FromArgb(MAX_ALPHA, 66, 71, 75)
    Public GRAY As Color = Color.FromArgb(MAX_ALPHA, 79, 85, 91)
    Public GRAY_LIGHT As Color = Color.FromArgb(MAX_ALPHA, 107, 121, 141)
    Public SKYBLUE As Color = Color.FromArgb(MAX_ALPHA, 74, 140, 234)

    'font
    Public font_munro As PrivateFontCollection = New PrivateFontCollection()
    Public font_12 As Font
    Public font_16 As Font
    Public font_32 As Font
    Public strFormat As New StringFormat

    'image
    Public spr_player As Sprite = GetSprite("player.png")
    Public spr_hpBar As Sprite = GetSprite("hpBar.png")
    Public spr_skillicon As SpriteSheet = GetSprite("skillicon.png", 15)

    'player
    Public lv, exp_present, exp_required As UInteger

    Public hp_max, hp As UInteger
    Public hp_regen, defense, exp_bonus As Int16

    Public atk_dam As UInteger
    Public atk_reload As Double
    Public atk_spd, atk_num, atk_size, atk_range, atk_penetrate, atk_explosion, critical, critical_dam As Int16

    Public speed As UInt16

    Public player_hspeed, player_vspeed As Integer
    Public playerMove As Boolean = False

    'background
    Public bg_x, bg_y As Integer

    Public Sub SetValue()
        Dim strFontName As String = Application.ExecutablePath
        strFontName = strFontName.Substring(0, strFontName.LastIndexOf("\bin")) & "\font\munro.ttf"

        font_munro.AddFontFile(strFontName)
        font_12 = New Font(font_munro.Families(0), 12)
        font_16 = New Font(font_munro.Families(0), 16)
        font_32 = New Font(font_munro.Families(0), 32)

        strFormat.LineAlignment = StringAlignment.Center
        strFormat.Alignment = StringAlignment.Center
    End Sub

    Public Sub DrawSprite(ByVal g As Graphics, ByVal sprite As Sprite, ByVal x As Integer, ByVal y As Integer)
        g.DrawImage(sprite.spr, x - sprite.width \ 2, y - sprite.height \ 2)
    End Sub

    Public Sub DrawSprite(ByVal g As Graphics, ByVal sprite_sheet As SpriteSheet, ByVal index As Int16, ByVal x As Integer, ByVal y As Integer)
        g.DrawImage(sprite_sheet.spr_sheet(index), x - sprite_sheet.width \ 2, y - sprite_sheet.height \ 2)
    End Sub

    Public Sub DrawText(ByVal g As Graphics, ByVal str As String, ByVal x As Integer, ByVal y As Integer, ByVal fnt As Font, ByVal color As Color, ByVal alpha As Int16)
        Dim text_color As Color = Color.FromArgb(alpha, color.R, color.G, color.B)

        Using brush As Brush = New Drawing.SolidBrush(text_color), f As Font = New Font(fnt.FontFamily, fnt.Size, fnt.Style)
            g.DrawString(str, f, brush, x, y, strFormat)
        End Using
    End Sub

    Public Sub DrawLine(ByVal g As Graphics, ByVal pnt_x As Point, ByVal pnt_y As Point, ByVal color As Color, ByVal alpha As Int16, Optional size As Int16 = 2)
        Dim line_color As Color = Color.FromArgb(alpha, color.R, color.G, color.B)

        Using brush As Brush = New SolidBrush(line_color), pen As Pen = New Drawing.Pen(brush, size)
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

    Public Function GetSprite(ByVal file_name As String, ByVal number As Int16) As SpriteSheet
        Dim strImageName As String = Application.ExecutablePath

        strImageName = strImageName.Substring(0, strImageName.LastIndexOf("\bin")) & "\image\" & file_name

        If IO.File.Exists(strImageName) Then
            Dim img As Image = Image.FromFile(strImageName)
            Dim bm(number) As Bitmap
            Dim xsize = img.Width \ number
            Dim ysize = img.Height

            For index As Integer = 0 To number
                bm(index) = New Bitmap(xsize, ysize)

                Using g As Graphics = Graphics.FromImage(bm(index))
                    g.DrawImage(img, 0, 0, New RectangleF(index * xsize, 0, xsize, ysize), GraphicsUnit.Pixel)
                End Using
            Next

            img.Dispose()

            Return New SpriteSheet(bm, xsize, ysize)
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
