namespace HttpClient
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.startbtn = new System.Windows.Forms.Button();
            this.bodytb = new System.Windows.Forms.TextBox();
            this.methodTypecb = new System.Windows.Forms.ComboBox();
            this.urlcb = new System.Windows.Forms.ComboBox();
            this.logstb = new System.Windows.Forms.TextBox();
            this.runautobt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startbtn
            // 
            this.startbtn.Location = new System.Drawing.Point(17, 48);
            this.startbtn.Margin = new System.Windows.Forms.Padding(4);
            this.startbtn.Name = "startbtn";
            this.startbtn.Size = new System.Drawing.Size(100, 28);
            this.startbtn.TabIndex = 0;
            this.startbtn.Text = "Start";
            this.startbtn.UseVisualStyleBackColor = true;
            this.startbtn.Click += new System.EventHandler(this.startbtn_Click);
            // 
            // bodytb
            // 
            this.bodytb.Location = new System.Drawing.Point(17, 85);
            this.bodytb.Margin = new System.Windows.Forms.Padding(4);
            this.bodytb.Multiline = true;
            this.bodytb.Name = "bodytb";
            this.bodytb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.bodytb.Size = new System.Drawing.Size(1083, 238);
            this.bodytb.TabIndex = 2;
            this.bodytb.Text = "\r\n\r\n";
            // 
            // methodTypecb
            // 
            this.methodTypecb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.methodTypecb.FormattingEnabled = true;
            this.methodTypecb.Location = new System.Drawing.Point(979, 16);
            this.methodTypecb.Name = "methodTypecb";
            this.methodTypecb.Size = new System.Drawing.Size(121, 24);
            this.methodTypecb.TabIndex = 4;
            // 
            // urlcb
            // 
            this.urlcb.FormattingEnabled = true;
            this.urlcb.Location = new System.Drawing.Point(17, 16);
            this.urlcb.Name = "urlcb";
            this.urlcb.Size = new System.Drawing.Size(956, 24);
            this.urlcb.TabIndex = 5;
            // 
            // logstb
            // 
            this.logstb.Location = new System.Drawing.Point(17, 330);
            this.logstb.Multiline = true;
            this.logstb.Name = "logstb";
            this.logstb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logstb.Size = new System.Drawing.Size(1083, 264);
            this.logstb.TabIndex = 6;
            // 
            // runautobt
            // 
            this.runautobt.Location = new System.Drawing.Point(124, 48);
            this.runautobt.Name = "runautobt";
            this.runautobt.Size = new System.Drawing.Size(138, 28);
            this.runautobt.TabIndex = 7;
            this.runautobt.Text = "Run Automatically";
            this.runautobt.UseVisualStyleBackColor = true;
            this.runautobt.Click += new System.EventHandler(this.runautobt_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1117, 606);
            this.Controls.Add(this.runautobt);
            this.Controls.Add(this.logstb);
            this.Controls.Add(this.urlcb);
            this.Controls.Add(this.methodTypecb);
            this.Controls.Add(this.bodytb);
            this.Controls.Add(this.startbtn);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(1135, 653);
            this.MinimumSize = new System.Drawing.Size(1135, 653);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HttpClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startbtn;
        private System.Windows.Forms.TextBox bodytb;
        private System.Windows.Forms.ComboBox methodTypecb;
        private System.Windows.Forms.ComboBox urlcb;
        private System.Windows.Forms.TextBox logstb;
        private System.Windows.Forms.Button runautobt;
    }
}

