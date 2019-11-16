namespace 上位机
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lbFrame = new System.Windows.Forms.Label();
            this.btStart = new System.Windows.Forms.Button();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.pbPath = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label01 = new System.Windows.Forms.Label();
            this.myone = new System.Windows.Forms.Label();
            this.label02 = new System.Windows.Forms.Label();
            this.mytwo = new System.Windows.Forms.Label();
            this.label03 = new System.Windows.Forms.Label();
            this.mythree = new System.Windows.Forms.Label();
            this.label04 = new System.Windows.Forms.Label();
            this.myfour = new System.Windows.Forms.Label();

            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
           
            // ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPath)).BeginInit();
           // ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lbFrame);
            this.splitContainer1.Panel1.Controls.Add(this.btStart);
            this.splitContainer1.Panel1.Controls.Add(this.tbPath);
            this.splitContainer1.Panel1.Controls.Add(this.pbPath);
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            // 
            // splitContainer1.Panel2
            // 

            this.splitContainer1.Panel2.Controls.Add(this.label01);
            this.splitContainer1.Panel2.Controls.Add(this.myone);

            this.splitContainer1.Panel2.Controls.Add(this.label02);
            this.splitContainer1.Panel2.Controls.Add(this.mytwo);

            this.splitContainer1.Panel2.Controls.Add(this.label03);
            this.splitContainer1.Panel2.Controls.Add(this.mythree);

            this.splitContainer1.Panel2.Controls.Add(this.label04);
            this.splitContainer1.Panel2.Controls.Add(this.myfour);


            this.splitContainer1.Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Size = new System.Drawing.Size(44, 701);
            this.splitContainer1.SplitterDistance = 167;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // lbFrame
            // 
            this.lbFrame.AutoSize = true;
            this.lbFrame.Location = new System.Drawing.Point(132, 65);
            this.lbFrame.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbFrame.Name = "lbFrame";
            this.lbFrame.Size = new System.Drawing.Size(45, 15);
            this.lbFrame.TabIndex = 12;
            this.lbFrame.Text = "第0帧";
            // 
            // btStart
            // 
            this.btStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btStart.Location = new System.Drawing.Point(24, 59);
            this.btStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(100, 29);
            this.btStart.TabIndex = 11;
            this.btStart.Text = "开始";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // tbPath
            // 
            this.tbPath.Location = new System.Drawing.Point(100, 29);
            this.tbPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(257, 25);
            this.tbPath.TabIndex = 9;
            this.tbPath.Text = "E:\\jjq\\img\\1.txt";
            // 
            // pbPath
            // 
            this.pbPath.BackColor = System.Drawing.Color.Transparent;
            this.pbPath.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbPath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbPath.Image = global::上位机.Properties.Resources.path;
            this.pbPath.Location = new System.Drawing.Point(361, 29);
            this.pbPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pbPath.Name = "pbPath";
            this.pbPath.Size = new System.Drawing.Size(28, 26);
            this.pbPath.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbPath.TabIndex = 10;
            this.pbPath.TabStop = false;
            this.pbPath.Click += new System.EventHandler(this.pbPath_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 32);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 15);
            this.label9.TabIndex = 8;
            this.label9.Text = "读取路径：";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(42, 527);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "说明：\r\n空格键  暂停/播放\r\n←      后退一帧\r\n→      前进一帧\r\n\r\n";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.pictureBox1);
            // 
            // splitContainer2.Panel2
            // 
              this.splitContainer2.Panel2.Controls.Add(this.splitContainer1);
            
           
            this.splitContainer2.Size = new System.Drawing.Size(1445, 701);
            this.splitContainer2.SplitterDistance = 1396;
            this.splitContainer2.SplitterWidth = 5;
            this.splitContainer2.TabIndex = 1;
            


            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::上位机.Properties.Resources.TIM图片201708211952251;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1396, 701);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1445, 701);
            this.Controls.Add(this.splitContainer2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "华工智能车队上位机";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
          //  ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPath)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
          //  ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

            
            //laber01
            this.label01.AutoSize = true;
            this.label01.Location = new System.Drawing.Point(12, 88);
            this.label01.Name = "label01";
            this.label01.Size = new System.Drawing.Size(71, 12);
            this.label01.TabIndex = 38;
            this.label01.Text = "one:";

            this.myone.AutoSize = true;
            this.myone.Location = new System.Drawing.Point(62, 88);
            this.myone.Name = "myone";
            this.myone.Size = new System.Drawing.Size(11, 12);
            this.myone.TabIndex = 35;
            this.myone.Text = "0";

            //laber02
            this.label02.AutoSize = true;
            this.label02.Location = new System.Drawing.Point(12, 102);
            this.label02.Name = "label02";
            this.label02.Size = new System.Drawing.Size(71, 12);
            this.label02.TabIndex = 38;
            this.label02.Text = "two:";

            this.mytwo.AutoSize = true;
            this.mytwo.Location = new System.Drawing.Point(62, 102);
            this.mytwo.Name = "mytwo";
            this.mytwo.Size = new System.Drawing.Size(11, 12);
            this.mytwo.TabIndex = 35;
            this.mytwo.Text = "0";

            //laber03
            this.label03.AutoSize = true;
            this.label03.Location = new System.Drawing.Point(12, 116);
            this.label03.Name = "label03";
            this.label03.Size = new System.Drawing.Size(71, 12);
            this.label03.TabIndex = 38;
            this.label03.Text = "three:";

            this.mythree.AutoSize = true;
            this.mythree.Location = new System.Drawing.Point(62, 116);
            this.mythree.Name = "mythree";
            this.mythree.Size = new System.Drawing.Size(11, 12);
            this.mythree.TabIndex = 35;
            this.mythree.Text = "0";

            //laber04
            this.label04.AutoSize = true;
            this.label04.Location = new System.Drawing.Point(12, 130);
            this.label04.Name = "label04";
            this.label04.Size = new System.Drawing.Size(71, 12);
            this.label04.TabIndex = 38;
            this.label04.Text = "four:";

            this.myfour.AutoSize = true;
            this.myfour.Location = new System.Drawing.Point(62, 130);
            this.myfour.Name = "myfour";
            this.myfour.Size = new System.Drawing.Size(11, 12);
            this.myfour.TabIndex = 35;
            this.myfour.Text = "0";


        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.PictureBox pbPath;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lbFrame;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label01;
        private System.Windows.Forms.Label myone;
        private System.Windows.Forms.Label label02;
        private System.Windows.Forms.Label mytwo;
        private System.Windows.Forms.Label label03;
        private System.Windows.Forms.Label mythree;
        private System.Windows.Forms.Label label04;
        private System.Windows.Forms.Label myfour;

    }
}

