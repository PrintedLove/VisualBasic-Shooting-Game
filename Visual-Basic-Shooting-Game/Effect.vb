Public Class Effect : Inherits ObjectBase

    Public hspeed, vspeed As Integer

    Sub New(ByVal mode As Int16, ByVal x As Integer, ByVal y As Integer,
            Optional EnemyPointX As Integer = 0, Optional EnemyPointY As Integer = 0)
        MyBase.New()

        If mode = 0 Then
            type = 100          'palyer attack
            spr = spr_attack
            spr_index = atk_size - 1
            rec.Size = New Point(3 + atk_size * 2, 3 + atk_size * 2)

            Dim moveAngle As Double = GetAngleTwoPoint(player_rec.X, player_rec.Y, EnemyPointX, EnemyPointY)
            Dim moveCoord As Point = GetCoordCircle(0, 0, moveAngle, atk_spd)

            hspeed = moveCoord.X
            vspeed = moveCoord.Y
        ElseIf mode = 1 Then
            type = 101          'enemy attack
        Else
            type = 102          'effect
        End If

        rec.X = x
        rec.Y = y
    End Sub

    Sub IndividualEvent()
        DefaultEvent()

        rec.X += hspeed
        rec.Y += vspeed
    End Sub
End Class