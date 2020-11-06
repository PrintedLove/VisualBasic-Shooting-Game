Public Class Effect : Inherits ObjectBase

    Public hspeed, vspeed As Integer
    Public collisionList As New ArrayList()
    Public penetrateNum As Int16
    Private liveTime, liveTimeMax As Int16

    Sub New(ByVal mode As Int16, ByVal x As Integer, ByVal y As Integer,
            Optional value1 As Integer = 0, Optional value2 As Integer = 0, Optional value3 As Integer = 0)
        MyBase.New()

        If mode = 0 Then
            type = 100          'palyer attack
            spr = spr_attack
            spr_index = atk_size - 1
            rec.Size = New Point(3 + atk_size * 2, 3 + atk_size * 2)
            liveTimeMax = 10
            penetrateNum = atk_penetrate

            Dim moveAngle As Double = GetAngleTwoPoint(player_rec.X, player_rec.Y, value1, value2)

            If atk_num > 1 Then
                Dim randoms As New Random(gameTick + value3 * 11)
                Dim MVDegree As Double = moveAngle * (180 / Math.PI)

                If atk_num = 2 Then
                    MVDegree += randoms.Next(-8, 8)
                ElseIf atk_num = 3 Then
                    MVDegree += randoms.Next(-12, 12)
                ElseIf atk_num = 4 Then
                    MVDegree += randoms.Next(-16, 16)
                End If

                If MVDegree > 359 Then
                    MVDegree -= 360
                End If

                If MVDegree < 0 Then
                    MVDegree += 360
                End If

                moveAngle = MVDegree * (Math.PI / 180)
            End If

            Dim moveCoord As Point = GetCoordCircle(0, 0, moveAngle, atk_spd)

            hspeed = moveCoord.X
            vspeed = moveCoord.Y
        ElseIf mode = 1 Then
            type = 101          'enemy attack
            spr = spr_attack_enemy
            spr_index = 0
            rec.Size = New Point(11, 11)
            liveTimeMax = 15

            Dim moveAngle As Double = GetAngleTwoPoint(x, y, player_rec.X, player_rec.Y)
            Dim moveCoord As Point = GetCoordCircle(0, 0, moveAngle, 12)

            hspeed = moveCoord.X
            vspeed = moveCoord.Y
        Else
            type = mode          'effect

            'size setting
            Select Case type
                Case 200                              'attack partical
                    Dim randoms As New Random(gameTick + value1 + x)

                    spr = spr_partical_attack
                    spr_index = 0
                    liveTimeMax = 5

                    Dim moveAngle As Double = randoms.Next(0, 359) * (Math.PI / 180)
                    Dim moveCoord As Point = GetCoordCircle(0, 0, moveAngle, randoms.Next(5, 15))

                    hspeed = moveCoord.X
                    vspeed = moveCoord.Y

                Case 201                              'enemy partical
                    Dim randoms As New Random(gameTick + value1 + x)

                    spr = spr_partical_enemy
                    spr_index = randoms.Next(0, 2)
                    liveTimeMax = 7 - spr_index

                    Dim moveAngle As Double = randoms.Next(0, 359) * (Math.PI / 180)
                    Dim moveCoord As Point = GetCoordCircle(0, 0, moveAngle, randoms.Next(5, 10))

                    hspeed = moveCoord.X
                    vspeed = moveCoord.Y

                Case 202                             'attack boom
                    spr = spr_attack_boom
                    spr_index = 0
                    rec.Size = New Point(60, 60)
                    liveTimeMax = 9
                    hspeed = 0
                    vspeed = 0

                Case 203                             'critical partical
                    spr = spr_partical_critical
                    spr_index = 0
                    liveTimeMax = 5
                    hspeed = 0
                    vspeed = 0
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

                If type >= 200 Then
                    spr_index += 1
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