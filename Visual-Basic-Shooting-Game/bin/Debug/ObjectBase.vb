Public Class ObjectBase
    Implements IDisposable

    Protected disposed As Boolean = False
    Public kill As Boolean = False

    Public coord As Point
    Public spr As SpriteSheet
    Public spr_index As Int16
    Public type As Int16
    Public random As Random

    Sub New()
        random = New Random()
    End Sub

    Sub Draw(ByVal g As Graphics)
        If -spr.width \ 2 < coord.X < S_WIDTH + spr.width \ 2 And -spr.height \ 2 < coord.Y < S_HEIGHT + spr.height \ 2 Then
            DrawSprite(g, spr, spr_index, coord.X, coord.Y)
        End If
    End Sub

    Sub DefaultEvent()
        coord.X += player_hspeed
        coord.Y += player_vspeed

        If GetDistanceTwoPoint(coord.X, coord.Y, S_WIDTH \ 2, S_HEIGHT \ 2) > S_WIDTH * 2 Then
            kill = True
        End If
    End Sub

    Sub Die()
        Finalize()
    End Sub

    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposed Then
            If disposing Then
                coord = Nothing
                spr = Nothing
                random = Nothing
            End If

        End If
        Me.disposed = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
        MyBase.Finalize()
    End Sub
End Class
