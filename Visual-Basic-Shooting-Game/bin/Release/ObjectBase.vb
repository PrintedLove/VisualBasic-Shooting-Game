Public Class ObjectBase
    Public coord As Point
    Public spr As SpriteSheet
    Public spr_index As Int16
    Public random As New Random(gameTick)

    Sub New()
        Dim rnd_angle As Integer = random.Next(0, 360)

        coord = GetCoordCircle(S_WIDTH \ 2, S_HEIGHT \ 2, rnd_angle * 180 / Math.PI, S_WIDTH)
        spr = spr_expBall
        spr_index = random.Next(0, 3)
    End Sub

    Sub Draw(ByVal g As Graphics)
        If 0 - spr.width \ 2 < coord.X < S_WIDTH + spr.width \ 2 And 0 - spr.height \ 2 < coord.Y < S_HEIGHT + spr.height \ 2 Then
            DrawSprite(g, spr, spr_index, coord.X, coord.Y)
        End If
    End Sub

    Sub Move(ByVal x_spd As Integer, ByVal y_spd As Integer)
        coord.X += x_spd
        coord.Y += y_spd
    End Sub

    Sub Die()
        obj_list.Remove(MyBase.ToString)
        enemy_list.Remove(MyBase.ToString)
        Finalize()
    End Sub
End Class
