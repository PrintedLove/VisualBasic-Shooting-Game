Public Class Effect : Inherits ObjectBase

    Public hspeed, vspeed As Integer
    Private liveTime, liveTimeMax As Int16

    Sub New(ByVal mode As Int16, ByVal x As Integer, ByVal y As Integer,
            Optional EnemyPointX As Integer = 0, Optional EnemyPointY As Integer = 0)
        MyBase.New()

        If mode = 0 Then
            type = 100          'palyer attack
            spr = spr_attack
            spr_index = atk_size - 1
            rec.Size = New Point(3 + atk_size * 2, 3 + atk_size * 2)
            liveTimeMax = 10

            Dim moveAngle As Double = GetAngleTwoPoint(player_rec.X, player_rec.Y, EnemyPointX, EnemyPointY)
            Dim moveCoord As Point = GetCoordCircle(0, 0, moveAngle, atk_spd)

            hspeed = moveCoord.X
            vspeed = moveCoord.Y
        ElseIf mode = 1 Then
            type = 101          'enemy attack
            spr = spr_attack_enemy
            spr_index = 0
            rec.Size = New Point(9, 9)
            liveTimeMax = 8

            Dim moveAngle As Double = GetAngleTwoPoint(x, y, player_rec.X, player_rec.Y)
            Dim moveCoord As Point = GetCoordCircle(0, 0, moveAngle, 12)

            hspeed = moveCoord.X
            vspeed = moveCoord.Y
        Else
            type = mode          'effect

            'size setting
            Select Case type
                Case 200                              'attack partical
                    Dim randoms As New Random(gameTick + EnemyPointX)

                    spr = spr_partical_attack
                    spr_index = 0
                    liveTimeMax = 3

                    Dim moveAngle As Double = randoms.Next(0, 359) * (Math.PI / 180)
                    Dim moveCoord As Point = GetCoordCircle(0, 0, moveAngle, randoms.Next(5, 15))

                    hspeed = moveCoord.X
                    vspeed = moveCoord.Y

                Case 201                              'enemy partical
                    Dim randoms As New Random(gameTick + EnemyPointX)

                    spr = spr_partical_enemy
                    spr_index = randoms.Next(0, 2)
                    liveTimeMax = 5

                    Dim moveAngle As Double = randoms.Next(0, 359) * (Math.PI / 180)
                    Dim moveCoord As Point = GetCoordCircle(0, 0, moveAngle, randoms.Next(5, 10))

                    hspeed = moveCoord.X
                    vspeed = moveCoord.Y
            End Select
        End If

        rec.X = x
        rec.Y = y
        liveTime = 0
    End Sub

    Sub IndividualEvent()
        DefaultEvent()

        rec.X += hspeed
        rec.Y += vspeed

        If kill = False Then
            'check live time
            If gameTick Mod 3 = 0 Then
                liveTime += 1

                If liveTime > liveTimeMax Then
                    kill = True
                End If
            End If

            If type = 101 Then            'enemy attack
                'touch with palyer
                If rec.IntersectsWith(player_rec) Then
                    kill = True
                    hp -= 10
                End If
            End If
        End If
    End Sub
End Class