﻿Imports System.Math

Public Class Enemy : Inherits ObjectBase

    Private enemyHp As Integer
    Private enemyspd, damage_touch, damage_shot As Int16
    Private touchKill As Boolean = False

    Sub New()
        spr = spr_enemy
        Dim enemy_type As Int16 = random.Next(1, 100)
        Dim enemy_per As Int16 = 0

        'spon probability
        For index As Int16 = 0 To 6
            enemy_per += enemy_spon(stage, index)

            If enemy_type <= enemy_per Then
                type = 10 + index
                Exit For
            End If
        Next

        spr_index = type - 10
        rec.Location = GetCoordCircle(S_WIDTH \ 2, S_HEIGHT \ 2,
                                      random.Next(0, 360) * 180 / Math.PI, S_WIDTH + random.Next(0, S_WIDTH \ 2))

        damage_touch = 10
        damage_shot = 0

        'stat setting
        Select Case type
            Case 10                              'normal_small
                rec.Size = New Point(28, 28)
                enemyspd = 7
            Case 11                              'normal_medium
                rec.Size = New Point(65, 65)
                enemyspd = 6
            Case 12                              'normal_big
                rec.Size = New Point(120, 120)
                enemyspd = 5
            Case 13                              'rush_small
                rec.Size = New Point(13, 13)
                damage_touch = 20
                enemyspd = 15
                touchKill = True
            Case 14                              'rush_big
                rec.Size = New Point(25, 25)
                damage_touch = 30
                enemyspd = 12
                touchKill = True
            Case 15                              'shoter_small
                rec.Size = New Point(31, 31)
                damage_shot = 10
                enemyspd = 10
            Case 16                              'shoter_big
                rec.Size = New Point(31, 31)
                damage_shot = 10
                enemyspd = 8
        End Select

        enemyHp = hpToDif(difficulty - 1, type - 10)
    End Sub

    Sub IndividualEvent()
        DefaultEvent()

        'move to player
        Dim move_angle As Double = GetAngleTwoPoint(rec.X, rec.Y, S_WIDTH \ 2, S_HEIGHT \ 2)
        Dim moveCoord As Point = GetCoordCircle(0, 0, move_angle, enemyspd)

        rec.X += moveCoord.X
        rec.Y += moveCoord.Y

        'hp manage
        If enemyHp < 1 Then
            kill = True
        End If

        'collision with player
        If gameTick Mod 10 = 0 And rec.IntersectsWith(player_rec) Then
            If touchKill = True Then
                kill = True
            End If

            hp -= damage_touch      'touch damage
        End If

        If Not kill Then
            'collision with other(enemy, player attack)
            Dim list_index As Int16 = 0

            While list_index < obj_list.Count()
                Dim obj As Object = obj_list.Item(list_index)

                If 9 < obj.type And obj.type < 100 Then                   'collision with enemy
                    If Not obj.Equals(Me) And rec.IntersectsWith(obj.rec) Then
                        rec.X -= Sign(obj.rec.X - rec.X)
                        rec.Y -= Sign(obj.rec.Y - rec.Y)
                    End If
                ElseIf obj.type = 100 Then                   'collision with player attack
                    If rec.IntersectsWith(obj.rec) Then
                        enemyHp -= atk_dam
                        obj.kill = True
                    End If
                End If

                list_index += 1
            End While

            'is enemy in player attack distance
            Dim distanceToPlayer As UInteger = GetDistanceTwoPoint(player_rec.X, player_rec.Y, rec.X, rec.Y)

            If EnemyDistance > distanceToPlayer Then
                EnemyDistance = distanceToPlayer
                nearestEnemyIndex = index
            End If
        End If
    End Sub
End Class