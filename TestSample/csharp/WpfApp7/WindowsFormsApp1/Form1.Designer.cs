namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.myBtn2 = new WindowsFormsApp1.MyBtn();
            this.imageLabel2 = new SMEnglish.StudyTools.Study.StudyControl.Writing.ScriptSort.Controls.ImageLabel();
            this.imageLabel1 = new SMEnglish.StudyTools.Study.StudyControl.Writing.ScriptSort.Controls.ImageLabel();
            this.myBtn1 = new WindowsFormsApp1.MyBtn();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.AcceptsTab = true;
            this.richTextBox1.Location = new System.Drawing.Point(121, 124);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(205, 162);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            this.richTextBox1.VScroll += new System.EventHandler(this.richTextBox1_VScroll);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(489, 202);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(831, 50);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(493, 326);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(933, 504);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 7;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(153, 473);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(285, 96);
            this.richTextBox2.TabIndex = 8;
            this.richTextBox2.Text = "";
            // 
            // richTextBox3
            // 
            this.richTextBox3.Location = new System.Drawing.Point(489, 473);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(300, 96);
            this.richTextBox3.TabIndex = 9;
            this.richTextBox3.Text = "";
            // 
            // myBtn2
            // 
            this.myBtn2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(100)))), ((int)(((byte)(50)))));
            this.myBtn2.Location = new System.Drawing.Point(275, 52);
            this.myBtn2.Name = "myBtn2";
            this.myBtn2.Size = new System.Drawing.Size(212, 23);
            this.myBtn2.TabIndex = 5;
            this.myBtn2.Text = "텍스트 삽입";
            this.myBtn2.UseVisualStyleBackColor = false;
            this.myBtn2.Click += new System.EventHandler(this.myBtn2_Click);
            // 
            // imageLabel2
            // 
            this.imageLabel2.Bottom = ((System.Drawing.Image)(resources.GetObject("imageLabel2.Bottom")));
            this.imageLabel2.BottomLeft = ((System.Drawing.Image)(resources.GetObject("imageLabel2.BottomLeft")));
            this.imageLabel2.BottomRight = ((System.Drawing.Image)(resources.GetObject("imageLabel2.BottomRight")));
            this.imageLabel2.lblFont = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.imageLabel2.lblForeColor = System.Drawing.SystemColors.ControlText;
            this.imageLabel2.lblText = "둥근 라벨? 레이블?";
            this.imageLabel2.Left = ((System.Drawing.Image)(resources.GetObject("imageLabel2.Left")));
            this.imageLabel2.Location = new System.Drawing.Point(121, 360);
            this.imageLabel2.Name = "imageLabel2";
            this.imageLabel2.Padding = new System.Windows.Forms.Padding(3);
            this.imageLabel2.Right = ((System.Drawing.Image)(resources.GetObject("imageLabel2.Right")));
            this.imageLabel2.Size = new System.Drawing.Size(575, 44);
            this.imageLabel2.TabIndex = 4;
            this.imageLabel2.Top = ((System.Drawing.Image)(resources.GetObject("imageLabel2.Top")));
            this.imageLabel2.TopLeft = ((System.Drawing.Image)(resources.GetObject("imageLabel2.TopLeft")));
            this.imageLabel2.TopRight = ((System.Drawing.Image)(resources.GetObject("imageLabel2.TopRight")));
            // 
            // imageLabel1
            // 
            this.imageLabel1.Bottom = ((System.Drawing.Image)(resources.GetObject("imageLabel1.Bottom")));
            this.imageLabel1.BottomLeft = ((System.Drawing.Image)(resources.GetObject("imageLabel1.BottomLeft")));
            this.imageLabel1.BottomRight = ((System.Drawing.Image)(resources.GetObject("imageLabel1.BottomRight")));
            this.imageLabel1.lblFont = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.imageLabel1.lblForeColor = System.Drawing.SystemColors.ControlText;
            this.imageLabel1.lblText = "asdasdasd";
            this.imageLabel1.Left = ((System.Drawing.Image)(resources.GetObject("imageLabel1.Left")));
            this.imageLabel1.Location = new System.Drawing.Point(121, 292);
            this.imageLabel1.Name = "imageLabel1";
            this.imageLabel1.Padding = new System.Windows.Forms.Padding(3);
            this.imageLabel1.Right = ((System.Drawing.Image)(resources.GetObject("imageLabel1.Right")));
            this.imageLabel1.Size = new System.Drawing.Size(575, 43);
            this.imageLabel1.TabIndex = 3;
            this.imageLabel1.Top = ((System.Drawing.Image)(resources.GetObject("imageLabel1.Top")));
            this.imageLabel1.TopLeft = ((System.Drawing.Image)(resources.GetObject("imageLabel1.TopLeft")));
            this.imageLabel1.TopRight = ((System.Drawing.Image)(resources.GetObject("imageLabel1.TopRight")));
            // 
            // myBtn1
            // 
            this.myBtn1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(100)))), ((int)(((byte)(50)))));
            this.myBtn1.Location = new System.Drawing.Point(470, 124);
            this.myBtn1.Name = "myBtn1";
            this.myBtn1.Size = new System.Drawing.Size(75, 23);
            this.myBtn1.TabIndex = 2;
            this.myBtn1.Text = "myBtn1";
            this.myBtn1.UseVisualStyleBackColor = false;
            this.myBtn1.Click += new System.EventHandler(this.myBtn1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1368, 642);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.myBtn2);
            this.Controls.Add(this.imageLabel2);
            this.Controls.Add(this.imageLabel1);
            this.Controls.Add(this.myBtn1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private MyBtn myBtn1;
        private SMEnglish.StudyTools.Study.StudyControl.Writing.ScriptSort.Controls.ImageLabel imageLabel1;
        private SMEnglish.StudyTools.Study.StudyControl.Writing.ScriptSort.Controls.ImageLabel imageLabel2;
        private MyBtn myBtn2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox3;
    }
}

