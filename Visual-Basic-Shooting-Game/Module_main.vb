Imports System.Drawing.Text
Imports System.Math

Module Module_main

    'screen
    Public S_WIDTH As Int16 = 960
    Public S_HEIGHT As Int16 = 960
    Public mouse_coord As Point

    'color
    Public MAX_ALPHA As Int16 = 255
    Public WHITE As Color = Color.FromArgb(MAX_ALPHA, 214, 231, 255)
    Public GRAY_DEEP As Color = Color.FromArgb(MAX_ALPHA, 66, 71, 75)
    Public GRAY As Color = Color.FromArgb(MAX_ALPHA, 79, 81, 91)
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
    Public spr_enemy As SpriteSheet = GetSprite("enemy.png", 14)
    Public spr_attack As SpriteSheet = GetSprite("attack.png", 4)
    Public spr_attack_boom As SpriteSheet = GetSprite("attack_boom.png", 9)
    Public spr_attack_enemy As SpriteSheet = GetSprite("attack_enemy.png", 1)
    Public spr_partical_attack As SpriteSheet = GetSprite("partical_attack.png", 5)
    Public spr_partical_critical As SpriteSheet = GetSprite("partical_critical.png", 5)
    Public spr_partical_enemy As SpriteSheet = GetSprite("partical_enemy.png", 7)

    'game
    Public difficulty As Int16 = 1
    Public stage As Int16
    Public gameTick As Int16 = 0
    Public tick, tick_recent, tick_start, tick_attack As ULong
    Public playtime_m, playtime_s As UInteger
    Public EnemyDistance As Integer
    Public nearestEnemyIndex As Int16

    Public obj_list As List(Of Object) = New List(Of Object) From {}
    Public enemy_num, enemy_numMax, item_num As UInt16
    Public enemy_spon =                 'enemy spon Probability by stage
        {{100, 0, 0, 0, 0, 0, 0, 16},
        {90, 10, 0, 0, 0, 0, 0, 18},
        {80, 10, 0, 10, 0, 0, 0, 20},
        {70, 15, 0, 15, 0, 0, 0, 22},
        {60, 15, 5, 20, 0, 0, 0, 24},
        {45, 20, 5, 20, 5, 5, 0, 28},
        {30, 25, 10, 15, 10, 10, 0, 32},
        {25, 20, 15, 15, 10, 10, 5, 36},
        {20, 15, 20, 10, 15, 10, 10, 40},
        {10, 10, 30, 5, 20, 10, 15, 48}}

    Public timeToDif =                  'Time of stage up (depending on difficulty level)
        {{1, 1.5, 2.25, 3, 4, 5.5, 7, 9, 12, 15},
        {0.5, 1, 1.5, 2, 3, 4, 5.5, 7, 9, 11},
        {0.25, 0.5, 0.75, 1.5, 2, 2.5, 3.5, 5, 6, 8}}

    Public hpToDif =                    'enemy HP (depending on difficulty level)
        {{20, 100, 500, 15, 75, 50, 150},
        {30, 150, 750, 20, 125, 75, 225},
        {50, 300, 1500, 35, 200, 125, 400}}

    Public backgound_x, backgound_y As Integer
    Public showStatWindow As Boolean

    'player
    Public lv, exp_present, exp_required As UInteger
    Public exp_bonus As Double

    Public hp_max As UInteger
    Public hp As Integer
    Public hp_regen, defense As Int16

    Public atk_dam As UInteger
    Public atk_reload As Double
    Public atk_spd, atk_num, atk_size, atk_range, atk_penetrate, atk_explosion, critical, critical_dam As Int16

    Public speed As UInt16

    Public player_rec As Rectangle
    Public player_hspeed, player_vspeed As Integer
    Public playerMove As Boolean = False

    Public Sub SetValue()
        Dim strFontName As String = Application.ExecutablePath
        strFontName = strFontName.Substring(0, strFontName.LastIndexOf("\bin")) & "\font\munro.ttf"

        font_munro.AddFontFile(strFontName)
        font_12 = New Font(font_munro.Families(0), 12)
        font_16 = New Font(font_munro.Families(0), 16)
        font_32 = New Font(font_munro.Families(0), 32)

        strFormat.LineAlignment = StringAlignment.Center
        strFormat.Alignment = StringAlignment.Center

        player_rec = New Rectangle(S_WIDTH \ 2, S_HEIGHT \ 2, 25, 25)

        difficulty = 1
    End Sub

    Public Sub NewGame()
        stage = 0

        lv = 1
        exp_present = 0
        exp_required = 100
        exp_bonus = 1
        showStatWindow = False

        hp_max = 100
        hp = 100
        hp_regen = 0
        defense = 0

        atk_dam = 10
        atk_reload = 0.75
        atk_spd = 10
        atk_num = 1
        atk_size = 1
        atk_range = 200
        atk_penetrate = 0
        atk_explosion = 0

        critical = 10
        critical_dam = 150

        speed = 10

        player_hspeed = 0
        player_vspeed = 0
        playerMove = False

        backgound_x = 0
        backgound_y = 0

        enemy_num = 0
        enemy_numMax = 16
        item_num = 0
        EnemyDistance = 9999

        tick_start = DateTime.Now.Ticks
        tick_recent = 0
        tick_attack = 0
        gameTick = 0
    End Sub

    Public Sub CreateObject(ByVal obj_type As Int16, Optional value1 As Integer = 0,
                            Optional value2 As Integer = 0, Optional value3 As Integer = 0, Optional value4 As Integer = 0)
        Dim obj As Object

        Select Case obj_type
            Case 1                  'enemy
                obj = New Enemy(value1, value2, value3)
                obj_list.Add(obj)
                enemy_num += 1
            Case 2                  'item
                obj = New Item
                obj_list.Add(obj)
                item_num += 1
            Case 3                  'player attack
                If obj_list.Count > nearestEnemyIndex Then
                    obj = New Effect(0, S_WIDTH \ 2, S_HEIGHT \ 2,
                                 obj_list.Item(nearestEnemyIndex).rec.X, obj_list.Item(nearestEnemyIndex).rec.Y, value1)
                    obj_list.Add(obj)
                End If
            Case 4                  'enemy attack
                obj = New Effect(1, value1, value2)
                obj_list.Add(obj)
            Case 5                  'effect
                obj = New Effect(value1, value2, value3, value4)
                obj_list.Add(obj)
        End Select
    End Sub

    Public Sub CreateItem()
        Dim obj As Object = New Item
        obj_list.Add(obj)
    End Sub

    Public Sub DrawSprite(ByVal g As Graphics, ByVal sprite As Sprite, ByVal x As Integer, ByVal y As Integer)
        g.DrawImage(sprite.spr, x - sprite.width \ 2, y - sprite.height \ 2)
    End Sub

    Public Sub DrawSprite(ByVal g As Graphics, ByVal sprite_sheet As SpriteSheet,
                          ByVal index As Int16, ByVal x As Integer, ByVal y As Integer)
        g.DrawImage(sprite_sheet.spr(index), x - sprite_sheet.width \ 2, y - sprite_sheet.height \ 2)
    End Sub

    Public Sub DrawText(ByVal g As Graphics, ByVal str As String, ByVal x As Integer,
                        ByVal y As Integer, ByVal fnt As Font, ByVal color As Color, ByVal alpha As Int16)
        Dim text_color As Color = Color.FromArgb(alpha, color.R, color.G, color.B)

        Using brush As Brush = New Drawing.SolidBrush(text_color), f As Font = New Font(fnt.FontFamily, fnt.Size, fnt.Style)
            g.DrawString(str, f, brush, x, y, strFormat)
        End Using
    End Sub

    Public Sub DrawLine(ByVal g As Graphics, ByVal pnt_x As Point, ByVal pnt_y As Point,
                        ByVal color As Color, ByVal alpha As Int16, Optional size As Int16 = 2)
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
