Public Class Item : Inherits ObjectBase

    Public getItem As Boolean = False

    Sub New()
        spr = spr_item
        Dim item_type = random.Next(1, 100)

        'spon probability
        If item_type < 3 Then           '2 %
            type = 2
        ElseIf item_type < 13 Then      '10 %
            type = 3
        ElseIf item_type < 23 Then      '10 %
            type = 1
        Else                            '77 %
            type = 0
        End If

        spr_index = type
        rec.Location = GetCoordCircle(S_WIDTH \ 2, S_HEIGHT \ 2,
                                      random.Next(0, 360) * 180 / Math.PI, S_WIDTH + random.Next(0, S_WIDTH \ 2))

        'size setting
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

        'collision with player 
        If Not getItem And rec.IntersectsWith(player_rec) Then
            kill = True

            Select Case type
                Case 0
                    exp_present += 10 + exp_required \ 100      'exp 10 + 1 %
                Case 1
                    exp_present += 20 + exp_required \ 10       'exp 20 + 10 %
                Case 2
                    exp_present += 50 + exp_required \ 3        'exp 50 + 33 %
                Case 3
                    hp += hp_max \ 5                           'hp 20%

                    If hp > hp_max Then
                        hp = hp_max
                    End If
            End Select
        End If
    End Sub
End Class