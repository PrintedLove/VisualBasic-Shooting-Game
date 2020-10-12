Public Class Effect : Inherits ObjectBase

    Public value As UInteger

    Sub New(ByVal x As Integer, ByVal y As Integer)
        MyBase.New()

        type = random.Next(0, 4)
        spr_index = type
        rec.X = x
        rec.Y = y
    End Sub

    Sub IndividualEvent()
        DefaultEvent()
    End Sub
End Class