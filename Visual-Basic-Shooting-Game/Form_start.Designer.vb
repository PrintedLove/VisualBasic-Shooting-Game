<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form_start
    Inherits System.Windows.Forms.Form

    'Form은 Dispose를 재정의하여 구성 요소 목록을 정리합니다.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Button_start = New System.Windows.Forms.Button()
        Me.Button_help = New System.Windows.Forms.Button()
        Me.Button_exit = New System.Windows.Forms.Button()
        Me.Label_name = New System.Windows.Forms.Label()
        Me.Timer_formStart = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'Button_start
        '
        Me.Button_start.AutoSize = True
        Me.Button_start.BackColor = System.Drawing.Color.Transparent
        Me.Button_start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.Button_start.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_start.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.Button_start.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_start.Location = New System.Drawing.Point(174, 276)
        Me.Button_start.Name = "Button_start"
        Me.Button_start.Size = New System.Drawing.Size(132, 43)
        Me.Button_start.TabIndex = 0
        Me.Button_start.Text = "START"
        Me.Button_start.UseVisualStyleBackColor = False
        '
        'Button_help
        '
        Me.Button_help.AutoSize = True
        Me.Button_help.BackColor = System.Drawing.Color.Transparent
        Me.Button_help.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.Button_help.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_help.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.Button_help.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_help.Location = New System.Drawing.Point(174, 325)
        Me.Button_help.Name = "Button_help"
        Me.Button_help.Size = New System.Drawing.Size(132, 43)
        Me.Button_help.TabIndex = 1
        Me.Button_help.Text = "HELP"
        Me.Button_help.UseVisualStyleBackColor = False
        '
        'Button_exit
        '
        Me.Button_exit.AutoSize = True
        Me.Button_exit.BackColor = System.Drawing.Color.Transparent
        Me.Button_exit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.Button_exit.Cursor = System.Windows.Forms.Cursors.Default
        Me.Button_exit.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.Button_exit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Button_exit.Location = New System.Drawing.Point(174, 374)
        Me.Button_exit.Name = "Button_exit"
        Me.Button_exit.Size = New System.Drawing.Size(132, 43)
        Me.Button_exit.TabIndex = 2
        Me.Button_exit.Text = "EXIT"
        Me.Button_exit.UseVisualStyleBackColor = False
        '
        'Label_name
        '
        Me.Label_name.Location = New System.Drawing.Point(20, 49)
        Me.Label_name.Name = "Label_name"
        Me.Label_name.Size = New System.Drawing.Size(440, 118)
        Me.Label_name.TabIndex = 1
        Me.Label_name.Text = "Visual Basic Shooting Game"
        Me.Label_name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Timer_formStart
        '
        Me.Timer_formStart.Enabled = True
        Me.Timer_formStart.Interval = 20
        '
        'Form_start
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackgroundImage = Global.Visual_Basic_Shooting_Game.My.Resources.Resources.logo
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ClientSize = New System.Drawing.Size(480, 479)
        Me.Controls.Add(Me.Label_name)
        Me.Controls.Add(Me.Button_exit)
        Me.Controls.Add(Me.Button_help)
        Me.Controls.Add(Me.Button_start)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_start"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Form_start"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Button_start As Button
    Friend WithEvents Button_help As Button
    Friend WithEvents Button_exit As Button
    Friend WithEvents Label_name As Label
    Friend WithEvents Timer_formStart As Timer
End Class
