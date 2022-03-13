<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SrvSetting
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btn_Submit = New System.Windows.Forms.Button
        Me.btn_Cancel = New System.Windows.Forms.Button
        Me.tab_Company = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.txt_DBPW = New System.Windows.Forms.MaskedTextBox
        Me.txt_DBID = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txt_COMPANY = New System.Windows.Forms.TextBox
        Me.txt_SAPID = New System.Windows.Forms.TextBox
        Me.txt_LICENCE = New System.Windows.Forms.TextBox
        Me.txt_SAPPW = New System.Windows.Forms.MaskedTextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.txt_ServerIP = New System.Windows.Forms.TextBox
        Me.lbl_SrvName = New System.Windows.Forms.Label
        Me.tab_Add = New System.Windows.Forms.TabPage
        Me.tab_Company.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'btn_Submit
        '
        Me.btn_Submit.Location = New System.Drawing.Point(12, 331)
        Me.btn_Submit.Name = "btn_Submit"
        Me.btn_Submit.Size = New System.Drawing.Size(75, 23)
        Me.btn_Submit.TabIndex = 2
        Me.btn_Submit.Text = "&SAVE"
        Me.btn_Submit.UseVisualStyleBackColor = True
        '
        'btn_Cancel
        '
        Me.btn_Cancel.Location = New System.Drawing.Point(93, 331)
        Me.btn_Cancel.Name = "btn_Cancel"
        Me.btn_Cancel.Size = New System.Drawing.Size(75, 23)
        Me.btn_Cancel.TabIndex = 3
        Me.btn_Cancel.Text = "&Cancel"
        Me.btn_Cancel.UseVisualStyleBackColor = True
        '
        'tab_Company
        '
        Me.tab_Company.Controls.Add(Me.TabPage1)
        Me.tab_Company.Controls.Add(Me.tab_Add)
        Me.tab_Company.Location = New System.Drawing.Point(12, 12)
        Me.tab_Company.Name = "tab_Company"
        Me.tab_Company.SelectedIndex = 0
        Me.tab_Company.Size = New System.Drawing.Size(352, 313)
        Me.tab_Company.TabIndex = 10
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.Controls.Add(Me.GroupBox2)
        Me.TabPage1.Controls.Add(Me.GroupBox1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(344, 287)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "[ 1 ]"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txt_DBPW)
        Me.GroupBox3.Controls.Add(Me.txt_DBID)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.Label2)
        Me.GroupBox3.Location = New System.Drawing.Point(6, 62)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(330, 80)
        Me.GroupBox3.TabIndex = 7
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Database Information:"
        '
        'txt_DBPW
        '
        Me.txt_DBPW.Location = New System.Drawing.Point(115, 47)
        Me.txt_DBPW.Name = "txt_DBPW"
        Me.txt_DBPW.Size = New System.Drawing.Size(200, 20)
        Me.txt_DBPW.TabIndex = 5
        '
        'txt_DBID
        '
        Me.txt_DBID.Location = New System.Drawing.Point(115, 21)
        Me.txt_DBID.Name = "txt_DBID"
        Me.txt_DBID.Size = New System.Drawing.Size(200, 20)
        Me.txt_DBID.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 13)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "DB Login ID:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(6, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(103, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "DB Login Password:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.txt_COMPANY)
        Me.GroupBox2.Controls.Add(Me.txt_SAPID)
        Me.GroupBox2.Controls.Add(Me.txt_LICENCE)
        Me.GroupBox2.Controls.Add(Me.txt_SAPPW)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Location = New System.Drawing.Point(6, 147)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(330, 130)
        Me.GroupBox2.TabIndex = 6
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "SAP Information"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 48)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(101, 13)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Company DB name:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 22)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 13)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "Licence Server:"
        '
        'txt_COMPANY
        '
        Me.txt_COMPANY.Location = New System.Drawing.Point(115, 45)
        Me.txt_COMPANY.Name = "txt_COMPANY"
        Me.txt_COMPANY.Size = New System.Drawing.Size(200, 20)
        Me.txt_COMPANY.TabIndex = 11
        '
        'txt_SAPID
        '
        Me.txt_SAPID.Location = New System.Drawing.Point(116, 71)
        Me.txt_SAPID.Name = "txt_SAPID"
        Me.txt_SAPID.Size = New System.Drawing.Size(200, 20)
        Me.txt_SAPID.TabIndex = 7
        '
        'txt_LICENCE
        '
        Me.txt_LICENCE.Location = New System.Drawing.Point(115, 19)
        Me.txt_LICENCE.Name = "txt_LICENCE"
        Me.txt_LICENCE.Size = New System.Drawing.Size(200, 20)
        Me.txt_LICENCE.TabIndex = 6
        '
        'txt_SAPPW
        '
        Me.txt_SAPPW.Location = New System.Drawing.Point(115, 97)
        Me.txt_SAPPW.Name = "txt_SAPPW"
        Me.txt_SAPPW.Size = New System.Drawing.Size(200, 20)
        Me.txt_SAPPW.TabIndex = 9
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(6, 100)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(109, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "SAP Login Password:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(6, 74)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(74, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "SAP Login ID:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txt_ServerIP)
        Me.GroupBox1.Controls.Add(Me.lbl_SrvName)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(330, 50)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Window Information:"
        '
        'txt_ServerIP
        '
        Me.txt_ServerIP.Location = New System.Drawing.Point(116, 19)
        Me.txt_ServerIP.Name = "txt_ServerIP"
        Me.txt_ServerIP.Size = New System.Drawing.Size(200, 20)
        Me.txt_ServerIP.TabIndex = 3
        '
        'lbl_SrvName
        '
        Me.lbl_SrvName.AutoSize = True
        Me.lbl_SrvName.Location = New System.Drawing.Point(7, 22)
        Me.lbl_SrvName.Name = "lbl_SrvName"
        Me.lbl_SrvName.Size = New System.Drawing.Size(91, 13)
        Me.lbl_SrvName.TabIndex = 2
        Me.lbl_SrvName.Text = "Server Name (IP):"
        '
        'tab_Add
        '
        Me.tab_Add.Location = New System.Drawing.Point(4, 22)
        Me.tab_Add.Name = "tab_Add"
        Me.tab_Add.Size = New System.Drawing.Size(344, 287)
        Me.tab_Add.TabIndex = 1
        Me.tab_Add.Text = "Add"
        Me.tab_Add.UseVisualStyleBackColor = True
        '
        'SrvSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(374, 366)
        Me.Controls.Add(Me.tab_Company)
        Me.Controls.Add(Me.btn_Cancel)
        Me.Controls.Add(Me.btn_Submit)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SrvSetting"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Setting INI - Setup "
        Me.TopMost = True
        Me.tab_Company.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btn_Submit As System.Windows.Forms.Button
    Friend WithEvents btn_Cancel As System.Windows.Forms.Button
    Friend WithEvents tab_Company As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents tab_Add As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents txt_DBPW As System.Windows.Forms.MaskedTextBox
    Friend WithEvents txt_DBID As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txt_COMPANY As System.Windows.Forms.TextBox
    Friend WithEvents txt_SAPID As System.Windows.Forms.TextBox
    Friend WithEvents txt_LICENCE As System.Windows.Forms.TextBox
    Friend WithEvents txt_SAPPW As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txt_ServerIP As System.Windows.Forms.TextBox
    Friend WithEvents lbl_SrvName As System.Windows.Forms.Label
End Class
