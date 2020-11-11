<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_start
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
        Me.Button_start = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Button_start
        '
        Me.Button_start.AutoSize = True
        Me.Button_start.BackColor = System.Drawing.Color.Transparent
        Me.Button_start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.Button_start.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_start.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.Button_start.Location = New System.Drawing.Point(173, 292)
        Me.Button_start.Name = "Button_start"
        Me.Button_start.Size = New System.Drawing.Size(132, 43)
        Me.Button_start.TabIndex = 0
        Me.Button_start.Text = "START"
        Me.Button_start.UseVisualStyleBackColor = False
        '
        'Form_start
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(464, 441)
        Me.Controls.Add(Me.Button_start)
        Me.Name = "Form_start"
        Me.Text = "Form_start"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button_start As Button
End Class
