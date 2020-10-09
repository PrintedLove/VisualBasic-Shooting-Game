<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_play
    Inherits System.Windows.Forms.Form

    'Form은 Dispose를 재정의하여 구성 요소 목록을 정리합니다.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows Form 디자이너에 필요합니다.
    Private components As System.ComponentModel.IContainer

    '참고: 다음 프로시저는 Windows Form 디자이너에 필요합니다.
    '수정하려면 Windows Form 디자이너를 사용하십시오.  
    '코드 편집기에서는 수정하지 마세요.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PictureBox_play = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox_play, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox_play
        '
        Me.PictureBox_play.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox_play.Margin = New System.Windows.Forms.Padding(0)
        Me.PictureBox_play.Name = "PictureBox_play"
        Me.PictureBox_play.Size = New System.Drawing.Size(640, 640)
        Me.PictureBox_play.TabIndex = 0
        Me.PictureBox_play.TabStop = False
        '
        'Form_play
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(624, 601)
        Me.Controls.Add(Me.PictureBox_play)
        Me.Name = "Form_play"
        Me.Text = "Form_play"
        CType(Me.PictureBox_play, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents PictureBox_play As PictureBox
End Class
