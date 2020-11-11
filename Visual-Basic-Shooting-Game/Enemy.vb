Imports System.Math

Public Class Enemy : Inherits ObjectBase

    Private enemyHp As Integer
    Private enemyspd, damage_touch As Int16
    Private touchKill As Boolean = False
    Private isPlayerKill As Boolean = False
    Private attackCharged As Int16 = 0
    Private hitTime As Int16 = 0

    Sub New(ByVal mode As Int16, ByVal spon_x As Integer, ByVal spon_y As Integer)
        spr = spr_enemy

        If mode = 0 Then
            'random spon
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

            rec.Location = GetCoordCircle(S_WIDTH \ 2, S_HEIGHT \ 2,
                              random.Next(0, 360) * 180 / Math.PI, S_WIDTH + random.Next(0, S_WIDTH \ 2))
        Else
            'coord spon
            type = mode
            rec.Location = New Point(spon_x, spon_y)
        End If

        spr_index = type - 10
        damage_touch = 10

        'stat setting
        Select Case type
            Case 10                              'normal_small
                rec.Size = New Point(28, 28)
                enemyspd = 7 + random.Next(0, 2)
            Case 11                              'normal_medium
                rec.Size = New Point(55, 55)
                damage_touch = 16
                enemyspd = 6 + random.Next(0, 2)
            Case 12                              'normal_big
                rec.Size = New Point(100, 100)
                damage_touch = 25
                enemyspd = 5 + random.Next(0, 2)
            Case 13                              'rush_small
                rec.Size = New Point(20, 20)
                damage_touch = 33
                enemyspd = 12
                touchKill = True
            Case 14                              'rush_big
                rec.Size = New Point(35, 35)
                damage_touch = 75
                enemyspd = 11
                touchKill = True
            Case 15                              'shoter_small
                rec.Size = New Point(30, 30)
                enemyspd = 8
            Case 16                              'shoter_big
                rec.Size = New Point(30, 30)
                enemyspd = 8
        End Select

        enemyHp = hpToDif(difficulty - 1, type - 10)
    End Sub

    Sub IndividualEvent()
        DefaultEvent()

        'hp manage
        If enemyHp < 1 Then
            kill = True
            isPlayerKill = True
        End If

        'collision with player
        If kill = False And gameTick Mod 10 = 0 And rec.IntersectsWith(player_rec) Then
            If touchKill = True Then
                kill = True
            End If

            hp -= damage_touch - defense      'touch damage
        End If

        If kill = False Then
            'move to player
            Dim move_angle As Double = GetAngleTwoPoint(rec.X, rec.Y, S_WIDTH \ 2, S_HEIGHT \ 2)
            Dim moveCoord As Point = GetCoordCircle(0, 0, move_angle, enemyspd)

            rec.X += moveCoord.X
            rec.Y += moveCoord.Y

            'enemy attack
            If type > 14 Then
                If attackCharged > 9 Then
                    If type = 15 Then
                        CreateObject(4, rec.X, rec.Y)
                    Else
                        CreateObject(4, rec.X + 19, rec.Y - 17)
                        CreateObject(4, rec.X, rec.Y + 25)
                        CreateObject(4, rec.X - 19, rec.Y - 17)
                    End If

                    attackCharged = 0
                Else
                    If gameTick Mod 3 Then
                        attackCharged += 1
                    End If
                End If
            End If

            'collision with other(enemy, player attack)
            Dim list_index As Int16 = 0

            While list_index < obj_list.Count()
                Dim obj As Object = obj_list.Item(list_index)

                If 9 < obj.type And obj.type < 100 Then                     'collision with enemy
                    If Not obj.Equals(Me) And rec.IntersectsWith(obj.rec) Then
                        rec.X -= Sign(obj.rec.X - rec.X)
                        rec.Y -= Sign(obj.rec.Y - rec.Y)
                    End If
                ElseIf obj.type = 100 Then                                  'collision with player attack
                    If Not obj.collisionList.Contains(Me.GetHashCode) And rec.IntersectsWith(obj.rec) Then
                        Dim damage As Int16 = atk_dam
                        Dim ctkChance As Int16 = random.Next(1, 100)

                        If ctkChance <= critical Then
                            damage = Round(atk_dam * critical_dam / 100)
                            CreateObject(5, 203, obj.rec.X, obj.rec.Y)
                        End If

                        enemyHp -= damage

                        If hitTime = 0 Then
                            hitTime = 2
                            spr_index += 7
                        End If

                        If obj.penetrateNum > 0 Then
                            obj.penetrateNum -= 1
                            obj.collisionList.Add(Me.GetHashCode)
                        Else
                            obj.kill = True
                        End If

                        If atk_explosion > 0 Then
                            CreateObject(5, 202, obj.rec.X, obj.rec.Y)
                        End If

                        For i As Int16 = 1 To 3
                            CreateObject(5, 200, obj.rec.X, obj.rec.Y, i)
                        Next
                    End If
                ElseIf obj.type = 202 Then                                  'collision with player attack boom
                    If Not obj.collisionList.Contains(Me.GetHashCode) And rec.IntersectsWith(obj.rec) Then
                        obj.collisionList.Add(Me.GetHashCode)
                        enemyHp -= Round(atk_dam * atk_explosion / 100)

                        If hitTime = 0 Then
                            hitTime = 1
                            spr_index += 7
                        End If
                    End If
                End If

                list_index += 1
            End While

            'is enemy in player attack distance
            Dim distanceToPlayer As Integer = GetDistanceTwoPoint(player_rec.X, player_rec.Y, rec.X, rec.Y)

            If EnemyDistance > distanceToPlayer Then
                EnemyDistance = distanceToPlayer
                nearestEnemyIndex = index
            End If

            If hitTime > 0 And gameTick Mod 3 = 0 Then
                hitTime -= 1

                If hitTime = 0 Then
                    spr_index -= 7
                End If
            End If
        Else
            'kill by player
            If isPlayerKill = True Then
                For i As Int16 = 1 To 4
                    CreateObject(5, 201, rec.X, rec.Y, i)
                Next

                Select Case type
                    Case 10                              'normal_small
                        exp_present += Round(1 * exp_bonus)

                    Case 11                              'normal_medium
                        exp_present += Round(3 * exp_bonus)
                        CreateObject(1, 10, rec.X, rec.Y)
                        CreateObject(1, 10, rec.X, rec.Y)
                    Case 12                              'normal_big
                        exp_present += Round(10 * exp_bonus)
                        CreateObject(1, 11, rec.X, rec.Y)
                        CreateObject(1, 11, rec.X, rec.Y)
                    Case 13                              'rush_small
                        exp_present += Round(2 * exp_bonus)

                    Case 14                              'rush_big
                        exp_present += Round(7 * exp_bonus)
                        CreateObject(1, 13, rec.X, rec.Y)
                        CreateObject(1, 13, rec.X, rec.Y)
                    Case 15                              'shoter_small
                        exp_present += Round(5 * exp_bonus)

                    Case 16                              'shoter_big
                        exp_present += Round(10 * exp_bonus)
                        CreateObject(1, 15, rec.X, rec.Y)
                        CreateObject(1, 15, rec.X, rec.Y)
                End Select
            End If
        End If
    End Sub
End Class