Public Class Sprite     'image file class
    Public spr As Image
    Public width, height As Integer

    Sub New(ByVal i As Image, ByVal size_x As Integer, ByVal size_y As Integer)
        Me.spr = i
        Me.width = size_x
        Me.height = size_y
    End Sub
End Class