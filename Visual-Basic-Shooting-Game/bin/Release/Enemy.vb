Public Class Enemy : Inherits ObjectBase

    Public type As Int16

    Sub New()
        spr = spr_item
        spr_index = random.Next(0, 3)
    End Sub
End Class