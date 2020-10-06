Public Class Enemy : Inherits Sprite
    Public x, y As Integer

    Sub New(s As Sprite, ByVal type As Integer)
        MyBase.New(s.spr, s.width, s.height)

        Me.x = 0
        Me.y = 0
    End Sub
End Class