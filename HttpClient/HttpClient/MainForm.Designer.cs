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
            this.responsetb = new System.Windows.Forms.TextBox();
            this.methodTypecb = new System.Windows.Forms.ComboBox();
            this.urlcb = new System.Windows.Forms.ComboBox();
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
            this.bodytb.Size = new System.Drawing.Size(539, 319);
            this.bodytb.TabIndex = 2;
            this.bodytb.Text = "\r\n\r\n";
            // 
            // responsetb
            // 
            this.responsetb.Location = new System.Drawing.Point(565, 85);
            this.responsetb.Margin = new System.Windows.Forms.Padding(4);
            this.responsetb.Multiline = true;
            this.responsetb.Name = "responsetb";
            this.responsetb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.responsetb.Size = new System.Drawing.Size(535, 319);
            this.responsetb.TabIndex = 3;
            // 
            // methodTypecb
            // 
            this.methodTypecb.AutoCompleteCustomSource.AddRange(new string[] {
            "PUT",
            "GET",
            "POST",
            "DELETE"});
            this.methodTypecb.FormattingEnabled = true;
            this.methodTypecb.Items.AddRange(new object[] {
            "PUT",
            "GET",
            "POST",
            "DELETE"});
            this.methodTypecb.Location = new System.Drawing.Point(979, 16);
            this.methodTypecb.Name = "methodTypecb";
            this.methodTypecb.Size = new System.Drawing.Size(121, 24);
            this.methodTypecb.TabIndex = 4;
            // 
            // urlcb
            // 
            this.urlcb.AutoCompleteCustomSource.AddRange(new string[] {
            "http://localhost:8080/servers/localhost-9999/info ",
            "http://localhost:8080/servers/localhost-9999/matches/2017-01-22T15:17:00Z",
            "http://localhost:8080/servers/info",
            "http://localhost:8080/servers/localhost-9999/stats",
            "http://localhost:8080/players/Player1/stats",
            "http://localhost:8080/reports/recent-matches[/2]",
            "http://localhost:8080/reports/best-players[/2]",
            "http://localhost:8080/reports/popular-servers[/2]"});
            this.urlcb.FormattingEnabled = true;
            this.urlcb.Items.AddRange(new object[] {
            "http://localhost:8080/servers/localhost-9999/info ",
            "http://localhost:8080/servers/localhost-9999/matches/2017-01-22T15:17:00Z",
            "http://localhost:8080/servers/info",
            "http://localhost:8080/servers/localhost-9999/stats",
            "http://localhost:8080/players/Player1/stats",
            "http://localhost:8080/reports/recent-matches[/2]",
            "http://localhost:8080/reports/best-players[/2]",
            "http://localhost:8080/reports/popular-servers[/2]"});
            this.urlcb.Location = new System.Drawing.Point(17, 16);
            this.urlcb.Name = "urlcb";
            this.urlcb.Size = new System.Drawing.Size(956, 24);
            this.urlcb.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1117, 420);
            this.Controls.Add(this.urlcb);
            this.Controls.Add(this.methodTypecb);
            this.Controls.Add(this.responsetb);
            this.Controls.Add(this.bodytb);
            this.Controls.Add(this.startbtn);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "HttpClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startbtn;
        private System.Windows.Forms.TextBox bodytb;
        private System.Windows.Forms.TextBox responsetb;
        private System.Windows.Forms.ComboBox methodTypecb;
        private System.Windows.Forms.ComboBox urlcb;
    }
}

