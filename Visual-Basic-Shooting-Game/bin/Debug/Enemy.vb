Public Class Enemy : Inherits ObjectBase

    Public type As Int16

    Sub New()
        spr = spr_item
        spr_index = random.Next(0, 3)
        rec.Location = GetCoordCircle(S_WIDTH \ 2, S_HEIGHT \ 2, random.Next(0, 360) * 180 / Math.PI, S_WIDTH + random.Next(0, S_WIDTH \ 2))
    End Sub

    Sub IndividualEvent()
        DefaultEvent()
    End Sub
End Class