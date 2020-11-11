Public Class Sprite
    Public spr As Image
    Public width, height As Integer

    Sub New(ByVal i As Image, ByVal size_x As Integer, ByVal size_y As Integer)
        spr = i
        width = size_x
        height = size_y
    End Sub
End Class