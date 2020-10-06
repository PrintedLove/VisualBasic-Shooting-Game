Public Class SpriteSheet     'image file class
    Public spr_sheet As Bitmap()
    Public width, height As Integer

    Sub New(ByVal i As Bitmap(), ByVal size_x As Integer, ByVal size_y As Integer)
        Me.spr_sheet = i
        Me.width = size_x
        Me.height = size_y
    End Sub
End Class
