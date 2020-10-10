Public Class Enemy : Inherits ObjectBase

    Public type As Int16

    Sub New()
        'Dim rnd_angle As Integer = random.Next(0, 360)

        spr = spr_expBall
        spr_index = random.Next(0, 3)
    End Sub
End Class