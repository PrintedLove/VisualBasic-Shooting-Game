Public Class Sprite
    Public spr As Image
    Public x, y, width, height As Integer

    Sub New(ByVal i As Image, ByVal coord_x As Integer, ByVal coord_y As Integer, ByVal size_x As Integer, ByVal size_y As Integer)
        Me.spr = i
        Me.x = coord_x
        Me.y = coord_y
        Me.width = size_x
        Me.height = size_y
    End Sub
End Class