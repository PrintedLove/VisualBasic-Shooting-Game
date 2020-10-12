Public Class Item : Inherits ObjectBase     'item type[ 0: exp(small), 1: exp(medium), 2: exp(big), 3: hp ball ]

    Public item_type As Int16
    Public getItem As Boolean = False

    Sub New()
        spr = spr_item
        item_type = random.Next(1, 100)

        If item_type < 4 Then           '3 %
            type = 2
        ElseIf item_type < 16 Then      '12 %
            type = 3
        ElseIf item_type < 31 Then      '15 %
            type = 1
        Else                            '70 %
            type = 0
        End If

        spr_index = type
        rec.Location = GetCoordCircle(S_WIDTH \ 2, S_HEIGHT \ 2, random.Next(0, 360) * 180 / Math.PI, S_WIDTH + random.Next(0, S_WIDTH \ 2))

        Select Case type
            Case 0                              'exp(small)
                rec.Size = New Point(11, 11)
            Case 1                              'exp(medium)
                rec.Size = New Point(17, 17)
            Case 2                              'exp(big)
                rec.Size = New Point(23, 23)
            Case 3                              'hp  ball
                rec.Size = New Point(15, 15)
        End Select
    End Sub

    Sub IndividualEvent()
        DefaultEvent()

        'item collision player 
        If Not getItem And rec.IntersectsWith(player_rec) Then
            kill = True

            Select Case type
                Case 0
                    exp_present += 10 + exp_required \ 100
                Case 1
                    exp_present += 20 + exp_required \ 10
                Case 2
                    exp_present += 50 + exp_required \ 3
                Case 3
                    hp += hp_max \ 10

                    If hp > hp_max Then
                        hp = hp_max
                    End If
            End Select
        End If
    End Sub
End Class