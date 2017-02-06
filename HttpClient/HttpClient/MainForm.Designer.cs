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
            this.addresstb = new System.Windows.Forms.TextBox();
            this.bodytb = new System.Windows.Forms.TextBox();
            this.responsetb = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // startbtn
            // 
            this.startbtn.Location = new System.Drawing.Point(13, 39);
            this.startbtn.Name = "startbtn";
            this.startbtn.Size = new System.Drawing.Size(75, 23);
            this.startbtn.TabIndex = 0;
            this.startbtn.Text = "Start";
            this.startbtn.UseVisualStyleBackColor = true;
            this.startbtn.Click += new System.EventHandler(this.startbtn_Click);
            // 
            // addresstb
            // 
            this.addresstb.Location = new System.Drawing.Point(13, 13);
            this.addresstb.Name = "addresstb";
            this.addresstb.Size = new System.Drawing.Size(813, 20);
            this.addresstb.TabIndex = 1;
            this.addresstb.Text = "http://localhost:8080/servers/localhost-8091/info";
            // 
            // bodytb
            // 
            this.bodytb.Location = new System.Drawing.Point(13, 69);
            this.bodytb.Multiline = true;
            this.bodytb.Name = "bodytb";
            this.bodytb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.bodytb.Size = new System.Drawing.Size(405, 260);
            this.bodytb.TabIndex = 2;
            this.bodytb.Text = "{\r\n\t\"name\": \"] My P3rfect Server [\",\r\n\t\"gameModes\": [ \"DM\", \"TDM\" ]\r\n}\r\n";
            // 
            // responsetb
            // 
            this.responsetb.Location = new System.Drawing.Point(424, 69);
            this.responsetb.Multiline = true;
            this.responsetb.Name = "responsetb";
            this.responsetb.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.responsetb.Size = new System.Drawing.Size(402, 260);
            this.responsetb.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 341);
            this.Controls.Add(this.responsetb);
            this.Controls.Add(this.bodytb);
            this.Controls.Add(this.addresstb);
            this.Controls.Add(this.startbtn);
            this.Name = "MainForm";
            this.Text = "HttpClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startbtn;
        private System.Windows.Forms.TextBox addresstb;
        private System.Windows.Forms.TextBox bodytb;
        private System.Windows.Forms.TextBox responsetb;
    }
}

