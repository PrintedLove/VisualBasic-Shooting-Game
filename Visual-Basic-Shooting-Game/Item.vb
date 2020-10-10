Public Class Item : Inherits ObjectBase

    Public type As Int16        'item type[ 0: exp(small), 1: exp(medium), 2: exp(big), 3(hp) ]
    Public value As UInteger

    Sub New()
        type = random.Next(0, 4)
        spr_index = type
    End Sub
End Class