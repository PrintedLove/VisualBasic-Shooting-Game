Imports System.Drawing.Text
Imports System.Math


Module Module_main
    'timer
    Public gameTick As Int16 = 0
    Public tick, tick_recent, tick_start As ULong
    Public playtime_m, playtime_s As UInteger

    'screen
    Public S_WIDTH As Int16 = 960
    Public S_HEIGHT As Int16 = 960
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
    Public spr_player_core As Sprite = GetSprite("player_core.png")
    Public spr_player_body As Sprite = GetSprite("player_body.png")
    Public spr_hpBar As Sprite = GetSprite("hpBar.png")
    Public spr_skillicon As SpriteSheet = GetSprite("skillicon.png", 15)
    Public spr_item As SpriteSheet = GetSprite("item.png", 4)

    'object list
    Public obj_list As List(Of Object) = New List(Of Object) From {}
    Public enemy_list As List(Of Object) = New List(Of Object) From {}

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

    Public Sub CreateEnemy()
        Dim obj As Object = New Enemy

        obj_list.Add(obj)
        enemy_list.Add(obj)
    End Sub

    Public Sub DrawSprite(ByVal g As Graphics, ByVal sprite As Sprite, ByVal x As Integer, ByVal y As Integer)
        g.DrawImage(sprite.spr, x - sprite.width \ 2, y - sprite.height \ 2)
    End Sub

    Public Sub DrawSprite(ByVal g As Graphics, ByVal sprite_sheet As SpriteSheet, ByVal index As Int16, ByVal x As Integer, ByVal y As Integer)
        g.DrawImage(sprite_sheet.spr(index), x - sprite_sheet.width \ 2, y - sprite_sheet.height \ 2)
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

    Public Function GetDistanceTwoPoint(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer)
        Return Sqrt(Pow(x2 - x1, 2) + Pow(y2 - y1, 2))
    End Function

    Public Function GetAngleTwoPoint(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer)
        Return Atan2(y2 - y1, x2 - x1)
    End Function

    Public Function GetCoordCircle(ByVal x As Integer, ByVal y As Integer, ByVal dgree As Double, ByVal radius As Integer)
        Return New Point(x + CInt(Cos(dgree) * radius), y + CInt(Sin(dgree) * radius))
    End Function
End Module
