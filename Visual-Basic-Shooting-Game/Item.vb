Public Class Item : Inherits ObjectBase     'item type[ 0: exp(small), 1: exp(medium), 2: exp(big), 3(hp) ]

    Public value As UInteger

    Sub New()
        spr = spr_item
        type = random.Next(0, 4)
        spr_index = type
        coord = GetCoordCircle(S_WIDTH \ 2, S_HEIGHT \ 2, random.Next(0, 360) * 180 / Math.PI, S_WIDTH + random.Next(0, S_WIDTH \ 2))
    End Sub
End Class