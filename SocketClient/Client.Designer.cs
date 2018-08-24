namespace SocketClient
{
    partial class Client
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
            this.btnsend = new System.Windows.Forms.Button();
            this.msgtext = new System.Windows.Forms.TextBox();
            this.sendtext = new System.Windows.Forms.TextBox();
            this.pclist = new System.Windows.Forms.CheckedListBox();
            this.btnconnect = new System.Windows.Forms.Button();
            this.btnstop = new System.Windows.Forms.Button();
            this.pcname = new System.Windows.Forms.TextBox();
            this.tbserverip = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnsend
            // 
            this.btnsend.Location = new System.Drawing.Point(501, 290);
            this.btnsend.Margin = new System.Windows.Forms.Padding(4);
            this.btnsend.Name = "btnsend";
            this.btnsend.Size = new System.Drawing.Size(100, 29);
            this.btnsend.TabIndex = 0;
            this.btnsend.Text = "发送消息";
            this.btnsend.UseVisualStyleBackColor = true;
            this.btnsend.Click += new System.EventHandler(this.Btnsend_Click);
            // 
            // msgtext
            // 
            this.msgtext.BackColor = System.Drawing.SystemColors.Window;
            this.msgtext.ForeColor = System.Drawing.SystemColors.WindowText;
            this.msgtext.Location = new System.Drawing.Point(184, 14);
            this.msgtext.Margin = new System.Windows.Forms.Padding(4);
            this.msgtext.Multiline = true;
            this.msgtext.Name = "msgtext";
            this.msgtext.ReadOnly = true;
            this.msgtext.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.msgtext.Size = new System.Drawing.Size(533, 224);
            this.msgtext.TabIndex = 1;
            // 
            // sendtext
            // 
            this.sendtext.Location = new System.Drawing.Point(184, 246);
            this.sendtext.Margin = new System.Windows.Forms.Padding(4);
            this.sendtext.Multiline = true;
            this.sendtext.Name = "sendtext";
            this.sendtext.Size = new System.Drawing.Size(533, 34);
            this.sendtext.TabIndex = 2;
            // 
            // pclist
            // 
            this.pclist.FormattingEnabled = true;
            this.pclist.Location = new System.Drawing.Point(16, 14);
            this.pclist.Margin = new System.Windows.Forms.Padding(4);
            this.pclist.Name = "pclist";
            this.pclist.Size = new System.Drawing.Size(159, 224);
            this.pclist.TabIndex = 4;
            // 
            // btnconnect
            // 
            this.btnconnect.Location = new System.Drawing.Point(292, 290);
            this.btnconnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnconnect.Name = "btnconnect";
            this.btnconnect.Size = new System.Drawing.Size(100, 29);
            this.btnconnect.TabIndex = 5;
            this.btnconnect.Text = "连接服务";
            this.btnconnect.UseVisualStyleBackColor = true;
            this.btnconnect.Click += new System.EventHandler(this.Btnconnect_Click);
            // 
            // btnstop
            // 
            this.btnstop.Location = new System.Drawing.Point(184, 290);
            this.btnstop.Margin = new System.Windows.Forms.Padding(4);
            this.btnstop.Name = "btnstop";
            this.btnstop.Size = new System.Drawing.Size(100, 29);
            this.btnstop.TabIndex = 6;
            this.btnstop.Text = "断开连接";
            this.btnstop.UseVisualStyleBackColor = true;
            this.btnstop.Click += new System.EventHandler(this.Btnstop_Click);
            // 
            // pcname
            // 
            this.pcname.Location = new System.Drawing.Point(17, 251);
            this.pcname.Margin = new System.Windows.Forms.Padding(4);
            this.pcname.Name = "pcname";
            this.pcname.Size = new System.Drawing.Size(159, 25);
            this.pcname.TabIndex = 7;
            this.pcname.Text = "AAAA";
            // 
            // tbserverip
            // 
            this.tbserverip.Location = new System.Drawing.Point(17, 291);
            this.tbserverip.Name = "tbserverip";
            this.tbserverip.Size = new System.Drawing.Size(158, 25);
            this.tbserverip.TabIndex = 8;
            this.tbserverip.Text = "192.168.13.64";
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 332);
            this.Controls.Add(this.tbserverip);
            this.Controls.Add(this.pcname);
            this.Controls.Add(this.btnstop);
            this.Controls.Add(this.btnconnect);
            this.Controls.Add(this.pclist);
            this.Controls.Add(this.sendtext);
            this.Controls.Add(this.msgtext);
            this.Controls.Add(this.btnsend);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Client";
            this.Text = "Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnsend;
        private System.Windows.Forms.TextBox msgtext;
        private System.Windows.Forms.TextBox sendtext;
        private System.Windows.Forms.CheckedListBox pclist;
        private System.Windows.Forms.Button btnconnect;
        private System.Windows.Forms.Button btnstop;
        private System.Windows.Forms.TextBox pcname;
        private System.Windows.Forms.TextBox tbserverip;
    }
}