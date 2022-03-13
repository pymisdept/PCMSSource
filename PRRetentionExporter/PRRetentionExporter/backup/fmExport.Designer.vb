<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class fmExport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(fmExport))
        Me.lbl_hdr = New System.Windows.Forms.Label
        Me.btn_Export = New System.Windows.Forms.Button
        Me.txt_toPeriod = New System.Windows.Forms.TextBox
        Me.lbl_to = New System.Windows.Forms.Label
        Me.txt_fromPeriod = New System.Windows.Forms.TextBox
        Me.lbl_from = New System.Windows.Forms.Label
        Me.progressbar = New System.Windows.Forms.ProgressBar
        Me.bgWorker = New System.ComponentModel.BackgroundWorker
        Me.lbl_processing = New System.Windows.Forms.Label
        Me.folderDlg = New System.Windows.Forms.FolderBrowserDialog
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.cb_PR2 = New System.Windows.Forms.CheckBox
        Me.cbx_PR2 = New System.Windows.Forms.CheckedListBox
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.cb_TD2 = New System.Windows.Forms.CheckBox
        Me.cbx_TD2 = New System.Windows.Forms.CheckedListBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.cb_PR4 = New System.Windows.Forms.CheckBox
        Me.cbx_PR4 = New System.Windows.Forms.CheckedListBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.txt_PrjCode = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.btn_deletePrj = New System.Windows.Forms.Button
        Me.btn_addPrj = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.GroupBox4 = New System.Windows.Forms.GroupBox
        Me.cbx_Result = New System.Windows.Forms.CheckedListBox
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.cbx_Export = New System.Windows.Forms.CheckedListBox
        Me.CheckBox1 = New System.Windows.Forms.CheckBox
        Me.CheckBox2 = New System.Windows.Forms.CheckBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.SuspendLayout()
        '
        'lbl_hdr
        '
        Me.lbl_hdr.AutoSize = True
        Me.lbl_hdr.Font = New System.Drawing.Font("Arial", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_hdr.Location = New System.Drawing.Point(11, 8)
        Me.lbl_hdr.Name = "lbl_hdr"
        Me.lbl_hdr.Size = New System.Drawing.Size(197, 19)
        Me.lbl_hdr.TabIndex = 8
        Me.lbl_hdr.Text = "Export Projects && Period"
        '
        'btn_Export
        '
        Me.btn_Export.Location = New System.Drawing.Point(246, 624)
        Me.btn_Export.Name = "btn_Export"
        Me.btn_Export.Size = New System.Drawing.Size(227, 36)
        Me.btn_Export.TabIndex = 3
        Me.btn_Export.Text = "Export to Excel"
        Me.btn_Export.UseVisualStyleBackColor = True
        '
        'txt_toPeriod
        '
        Me.txt_toPeriod.Location = New System.Drawing.Point(196, 37)
        Me.txt_toPeriod.Name = "txt_toPeriod"
        Me.txt_toPeriod.Size = New System.Drawing.Size(65, 22)
        Me.txt_toPeriod.TabIndex = 2
        Me.txt_toPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lbl_to
        '
        Me.lbl_to.AutoSize = True
        Me.lbl_to.Location = New System.Drawing.Point(171, 43)
        Me.lbl_to.Name = "lbl_to"
        Me.lbl_to.Size = New System.Drawing.Size(18, 12)
        Me.lbl_to.TabIndex = 12
        Me.lbl_to.Text = "To"
        '
        'txt_fromPeriod
        '
        Me.txt_fromPeriod.Location = New System.Drawing.Point(101, 38)
        Me.txt_fromPeriod.Name = "txt_fromPeriod"
        Me.txt_fromPeriod.Size = New System.Drawing.Size(65, 22)
        Me.txt_fromPeriod.TabIndex = 1
        Me.txt_fromPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lbl_from
        '
        Me.lbl_from.AutoSize = True
        Me.lbl_from.Location = New System.Drawing.Point(14, 44)
        Me.lbl_from.Name = "lbl_from"
        Me.lbl_from.Size = New System.Drawing.Size(73, 12)
        Me.lbl_from.TabIndex = 14
        Me.lbl_from.Text = "Report Period:"
        '
        'progressbar
        '
        Me.progressbar.Location = New System.Drawing.Point(246, 595)
        Me.progressbar.Name = "progressbar"
        Me.progressbar.Size = New System.Drawing.Size(227, 23)
        Me.progressbar.TabIndex = 15
        '
        'bgWorker
        '
        Me.bgWorker.WorkerReportsProgress = True
        Me.bgWorker.WorkerSupportsCancellation = True
        '
        'lbl_processing
        '
        Me.lbl_processing.AutoSize = True
        Me.lbl_processing.Location = New System.Drawing.Point(331, 580)
        Me.lbl_processing.Name = "lbl_processing"
        Me.lbl_processing.Size = New System.Drawing.Size(63, 12)
        Me.lbl_processing.TabIndex = 16
        Me.lbl_processing.Text = "Processing..."
        Me.lbl_processing.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cb_PR2)
        Me.GroupBox1.Controls.Add(Me.cbx_PR2)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 307)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(222, 255)
        Me.GroupBox1.TabIndex = 24
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "PR2"
        '
        'cb_PR2
        '
        Me.cb_PR2.AutoSize = True
        Me.cb_PR2.Location = New System.Drawing.Point(6, 232)
        Me.cb_PR2.Name = "cb_PR2"
        Me.cb_PR2.Size = New System.Drawing.Size(121, 16)
        Me.cb_PR2.TabIndex = 30
        Me.cb_PR2.Text = "Uncheck / Check All"
        Me.cb_PR2.UseVisualStyleBackColor = True
        '
        'cbx_PR2
        '
        Me.cbx_PR2.CheckOnClick = True
        Me.cbx_PR2.FormattingEnabled = True
        Me.cbx_PR2.Items.AddRange(New Object() {"Original Contract Sum", "Forecast Final Income", "Contract Award Date", "Overall Original Contract Period", "Contract Commencement Date", "Overall Original Completion Date", "Overall Extended Completion Date", "Overall Anticipated Completion Date", "Overall Practical Completion Date", "Overall Delay", "Client / Customer", "Employer", "Main Contractor", "Reporting Company", "P. Code (Main Contractor)", "P. Code (Rpt Company)"})
        Me.cbx_PR2.Location = New System.Drawing.Point(6, 21)
        Me.cbx_PR2.Name = "cbx_PR2"
        Me.cbx_PR2.Size = New System.Drawing.Size(211, 191)
        Me.cbx_PR2.TabIndex = 21
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.cb_TD2)
        Me.GroupBox2.Controls.Add(Me.cbx_TD2)
        Me.GroupBox2.Location = New System.Drawing.Point(240, 307)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(235, 255)
        Me.GroupBox2.TabIndex = 25
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "TD2"
        '
        'cb_TD2
        '
        Me.cb_TD2.AutoSize = True
        Me.cb_TD2.Location = New System.Drawing.Point(6, 232)
        Me.cb_TD2.Name = "cb_TD2"
        Me.cb_TD2.Size = New System.Drawing.Size(121, 16)
        Me.cb_TD2.TabIndex = 31
        Me.cb_TD2.Text = "Uncheck / Check All"
        Me.cb_TD2.UseVisualStyleBackColor = True
        '
        'cbx_TD2
        '
        Me.cbx_TD2.CheckOnClick = True
        Me.cbx_TD2.FormattingEnabled = True
        Me.cbx_TD2.Items.AddRange(New Object() {"Tender Sum", "Income Adjustments", "Variation + Claim + Other Income", "Remeasurement", "Provisional Sums", "Income Provisions - B", "Future Fluctuation", "Forecast Final Income (TD2)", "Original Budget", "Committed Final Cost (Project Report)", "Cost Provision - E", "Forecast Final Cost", "Tender Margin", "Current Margin Adjustments", "Future Margin Adjustments", "Forecast Final Margin", "Tender Allowance", "Anticipated Future Certification", "Certified to Date", "Anticipated Total Recovery"})
        Me.cbx_TD2.Location = New System.Drawing.Point(6, 18)
        Me.cbx_TD2.Name = "cbx_TD2"
        Me.cbx_TD2.Size = New System.Drawing.Size(220, 191)
        Me.cbx_TD2.TabIndex = 22
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.cb_PR4)
        Me.GroupBox3.Controls.Add(Me.cbx_PR4)
        Me.GroupBox3.Location = New System.Drawing.Point(481, 307)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(283, 255)
        Me.GroupBox3.TabIndex = 26
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "PR4"
        '
        'cb_PR4
        '
        Me.cb_PR4.AutoSize = True
        Me.cb_PR4.Location = New System.Drawing.Point(10, 232)
        Me.cb_PR4.Name = "cb_PR4"
        Me.cb_PR4.Size = New System.Drawing.Size(121, 16)
        Me.cb_PR4.TabIndex = 32
        Me.cb_PR4.Text = "Uncheck / Check All"
        Me.cb_PR4.UseVisualStyleBackColor = True
        '
        'cbx_PR4
        '
        Me.cbx_PR4.CheckOnClick = True
        Me.cbx_PR4.FormattingEnabled = True
        Me.cbx_PR4.Items.AddRange(New Object() {"Total Max Retention", "Total Retention Amount related to Completion 1", "Total Retention Amount related to Completion 2", "Total Retention Amount related to DLP", "Retention On-Hold", "Section", "Section Description", "Release No.", "Max Retention", "Retention related to Completion 1", "Retention related to Completion 2", "Retention related to DLP", "Status (A/B)", "Actual (Y/N)", "Release Date (D1)", "Release Amount (D2)"})
        Me.cbx_PR4.Location = New System.Drawing.Point(10, 18)
        Me.cbx_PR4.Name = "cbx_PR4"
        Me.cbx_PR4.Size = New System.Drawing.Size(268, 191)
        Me.cbx_PR4.TabIndex = 23
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.DimGray
        Me.Label1.Location = New System.Drawing.Point(11, 245)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(760, 22)
        Me.Label1.TabIndex = 27
        Me.Label1.Text = "                                                                                 " & _
            "                                                                     "
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 12.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(14, 277)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(110, 19)
        Me.Label3.TabIndex = 28
        Me.Label3.Text = "Export Fields"
        '
        'txt_PrjCode
        '
        Me.txt_PrjCode.Location = New System.Drawing.Point(101, 65)
        Me.txt_PrjCode.Name = "txt_PrjCode"
        Me.txt_PrjCode.Size = New System.Drawing.Size(161, 22)
        Me.txt_PrjCode.TabIndex = 30
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(14, 70)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(68, 12)
        Me.Label4.TabIndex = 29
        Me.Label4.Text = "Project Code:"
        '
        'btn_deletePrj
        '
        Me.btn_deletePrj.Location = New System.Drawing.Point(209, 183)
        Me.btn_deletePrj.Name = "btn_deletePrj"
        Me.btn_deletePrj.Size = New System.Drawing.Size(32, 23)
        Me.btn_deletePrj.TabIndex = 34
        Me.btn_deletePrj.Text = "<"
        Me.btn_deletePrj.UseVisualStyleBackColor = True
        '
        'btn_addPrj
        '
        Me.btn_addPrj.Location = New System.Drawing.Point(209, 120)
        Me.btn_addPrj.Name = "btn_addPrj"
        Me.btn_addPrj.Size = New System.Drawing.Size(32, 23)
        Me.btn_addPrj.TabIndex = 33
        Me.btn_addPrj.Text = ">"
        Me.btn_addPrj.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(292, 36)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 39
        Me.Button1.Text = "Retrieve"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(373, 35)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(125, 23)
        Me.Button2.TabIndex = 40
        Me.Button2.Text = "Import from Text file"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.CheckBox1)
        Me.GroupBox4.Controls.Add(Me.cbx_Result)
        Me.GroupBox4.Location = New System.Drawing.Point(15, 94)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(185, 163)
        Me.GroupBox4.TabIndex = 41
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Search Result"
        '
        'cbx_Result
        '
        Me.cbx_Result.CheckOnClick = True
        Me.cbx_Result.FormattingEnabled = True
        Me.cbx_Result.Location = New System.Drawing.Point(9, 18)
        Me.cbx_Result.Name = "cbx_Result"
        Me.cbx_Result.Size = New System.Drawing.Size(165, 123)
        Me.cbx_Result.Sorted = True
        Me.cbx_Result.TabIndex = 36
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.CheckBox2)
        Me.GroupBox5.Controls.Add(Me.cbx_Export)
        Me.GroupBox5.Location = New System.Drawing.Point(251, 94)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(185, 163)
        Me.GroupBox5.TabIndex = 42
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Projects to be exported"
        '
        'cbx_Export
        '
        Me.cbx_Export.CheckOnClick = True
        Me.cbx_Export.FormattingEnabled = True
        Me.cbx_Export.Location = New System.Drawing.Point(9, 18)
        Me.cbx_Export.Name = "cbx_Export"
        Me.cbx_Export.Size = New System.Drawing.Size(165, 123)
        Me.cbx_Export.Sorted = True
        Me.cbx_Export.TabIndex = 37
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(10, 144)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(121, 16)
        Me.CheckBox1.TabIndex = 38
        Me.CheckBox1.Text = "Uncheck / Check All"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(10, 144)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(121, 16)
        Me.CheckBox2.TabIndex = 39
        Me.CheckBox2.Text = "Uncheck / Check All"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.DimGray
        Me.Label2.Location = New System.Drawing.Point(11, 550)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(760, 22)
        Me.Label2.TabIndex = 43
        Me.Label2.Text = "                                                                                 " & _
            "                                                                     "
        '
        'fmExport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ClientSize = New System.Drawing.Size(774, 666)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.btn_deletePrj)
        Me.Controls.Add(Me.btn_addPrj)
        Me.Controls.Add(Me.txt_PrjCode)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lbl_processing)
        Me.Controls.Add(Me.progressbar)
        Me.Controls.Add(Me.lbl_from)
        Me.Controls.Add(Me.txt_toPeriod)
        Me.Controls.Add(Me.lbl_to)
        Me.Controls.Add(Me.txt_fromPeriod)
        Me.Controls.Add(Me.btn_Export)
        Me.Controls.Add(Me.lbl_hdr)
        Me.Controls.Add(Me.Label2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimumSize = New System.Drawing.Size(784, 584)
        Me.Name = "fmExport"
        Me.Text = "PCMS | Retention Exporter "
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lbl_hdr As System.Windows.Forms.Label
    Friend WithEvents btn_Export As System.Windows.Forms.Button
    Friend WithEvents txt_toPeriod As System.Windows.Forms.TextBox
    Friend WithEvents lbl_to As System.Windows.Forms.Label
    Friend WithEvents txt_fromPeriod As System.Windows.Forms.TextBox
    Friend WithEvents lbl_from As System.Windows.Forms.Label
    Friend WithEvents progressbar As System.Windows.Forms.ProgressBar
    Friend WithEvents bgWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents lbl_processing As System.Windows.Forms.Label
    Friend WithEvents folderDlg As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents cbx_PR2 As System.Windows.Forms.CheckedListBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents cbx_TD2 As System.Windows.Forms.CheckedListBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents cbx_PR4 As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cb_PR2 As System.Windows.Forms.CheckBox
    Friend WithEvents cb_TD2 As System.Windows.Forms.CheckBox
    Friend WithEvents cb_PR4 As System.Windows.Forms.CheckBox
    Friend WithEvents txt_PrjCode As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents btn_deletePrj As System.Windows.Forms.Button
    Friend WithEvents btn_addPrj As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents cbx_Result As System.Windows.Forms.CheckedListBox
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents cbx_Export As System.Windows.Forms.CheckedListBox
    Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBox2 As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
