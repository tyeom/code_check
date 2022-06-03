using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SMEnglish.StudyTools.Study.StudyControl.Writing.ScriptSort.Controls
{
    public partial class ImageLabel : UserControl
    {
        private Color _bgColor = Color.Transparent;
        private Image _imgTop = null;
        private Image _imgBottom = null;
        private Image _imgLeft = null;
        private Image _imgRight = null;
        private Image _imgTopLeft = null;
        private Image _imgTopRight = null;
        private Image _imgBottomLeft = null;
        private Image _imgBottomRight = null;

        public ImageLabel()
        {
            InitializeComponent();
        }

        #region Label Properties

        /// <summary>
        /// Label의 Font를 가져오거나 설정합니다.
        /// </summary>
        public Font lblFont
        {
            get { return this.lbl.Font; }
            set { this.lbl.Font = value; }
        }

        /// <summary>
        /// Label의 ForeColor를 가져오거나 설정합니다.
        /// </summary>
        public Color lblForeColor
        {
            get { return this.lbl.ForeColor; }
            set { this.lbl.ForeColor = value; }
        }

        /// <summary>
        /// Label의 Text를 가져오거나 설정합니다.
        /// </summary>
        public string lblText
        {
            get { return this.lbl.Text; }
            set 
            { 
                this.lbl.Text = value;
                this.ReSizeText();
            }
        }

        public new Image Top
        {
            get => _imgTop;
            set
            {
                _imgTop = value;
            }
        }

        public new Image Bottom
        {
            get => _imgBottom;
            set
            {
                _imgBottom = value;
            }
        }

        public new Image Left
        {
            get => _imgLeft;
            set
            {
                _imgLeft = value;
            }
        }

        public new Image Right
        {
            get => _imgRight;
            set
            {
                _imgRight = value;
            }
        }

        public new Image TopLeft
        {
            get => _imgTopLeft;
            set
            {
                _imgTopLeft = value;
            }
        }

        public new Image TopRight
        {
            get => _imgTopRight;
            set
            {
                _imgTopRight = value;
            }
        }

        public new Image BottomLeft
        {
            get => _imgBottomLeft;
            set
            {
                _imgBottomLeft = value;
            }
        }

        public new Image BottomRight
        {
            get => _imgBottomRight;
            set
            {
                _imgBottomRight = value;
            }
        }
        #endregion

        /// <summary>
        /// 입력된 텍스트에 맞게 세로 사이즈를 조절합니다.
        /// </summary>
        public void ReSizeText()
        {
            string text = this.lbl.Text;

            if (string.IsNullOrEmpty(text))
                return;                

            Graphics g = this.lbl.CreateGraphics();

            SizeF sizeF = g.MeasureString(text, this.lbl.Font, this.lbl.Width);

            this.Height = (int)sizeF.Height + 20;

            this.Invalidate();
        }

        #region Background Color & Images

        /// <summary>
        /// 배경이미지를 초기화합니다.
        /// </summary>
        /// <param name="imgTop"></param>
        /// <param name="imgBottom"></param>
        /// <param name="imgLeft"></param>
        /// <param name="imgRight"></param>
        /// <param name="imgTopLeft"></param>
        /// <param name="imgTopRight"></param>
        /// <param name="imgBottomLeft"></param>
        /// <param name="imgBottomRight"></param>
        public void InitializeImage(Image imgTop, Image imgBottom, Image imgLeft, Image imgRight,
                                    Image imgTopLeft, Image imgTopRight, Image imgBottomLeft, Image imgBottomRight)
        {
            this._imgTop = imgTop;
            this._imgBottom = imgBottom;
            this._imgLeft = imgLeft;
            this._imgRight = imgRight;
            this._imgTopLeft = imgTopLeft;
            this._imgTopRight = imgTopRight;
            this._imgBottomLeft = imgBottomLeft;
            this._imgBottomRight = imgBottomRight;
        }

        /// <summary>
        /// 배경색을 초기화합니다.
        /// </summary>
        /// <param name="bgColor"></param>
        public void InitializeBackgroundColor(Color bgColor)
        {
            this._bgColor = bgColor;
            this.BackColor = this._bgColor;
        }

        // 배경 이미지를 그려줍니다.
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Draw Background Images
            if (this.IsInitializeImage())
            {
                int xPos = 0;
                int yPos = 0;

                // Draw Top
                xPos = this._imgTopLeft.Width;

                do
                {
                    e.Graphics.DrawImage(this._imgTop, xPos, 0, this._imgTop.Width, this._imgTop.Height);
                    xPos += this._imgTop.Width;
                }
                while (xPos < this.Width - this._imgTopRight.Width);

                e.Graphics.DrawImage(this._imgTop, this.Width - this._imgTopRight.Width - this._imgTop.Width, 0, this._imgTop.Width, this._imgTop.Height);

                // Draw Left
                yPos = this._imgTopLeft.Height;

                do
                {
                    e.Graphics.DrawImage(this._imgLeft, 0, yPos, this._imgLeft.Width, this._imgLeft.Height);
                    yPos += this._imgLeft.Height;
                }
                while (yPos < this.Height - this._imgLeft.Height);

                e.Graphics.DrawImage(this._imgLeft, 0, this.Height - this._imgLeft.Height - this._imgBottomLeft.Height, this._imgLeft.Width, this._imgLeft.Height);

                // Draw Right
                xPos = this.Width - this._imgRight.Width;
                yPos = this._imgBottomRight.Height;

                do
                {
                    e.Graphics.DrawImage(this._imgRight, xPos, yPos, this._imgRight.Width, this._imgRight.Height);
                    yPos += this._imgRight.Height;
                }
                while (yPos < this.Height - this._imgRight.Height);

                e.Graphics.DrawImage(this._imgRight, xPos, this.Height - this._imgRight.Height - this._imgBottomRight.Height, this._imgRight.Width, this._imgRight.Height);

                // Draw Bottom
                xPos = this._imgBottomLeft.Width;

                do
                {
                    e.Graphics.DrawImage(this._imgBottom, xPos, this.Height - this._imgBottom.Height, this._imgBottom.Width, this._imgBottom.Height);
                    xPos += this._imgBottom.Width;
                }
                while (xPos  < this.Width - this._imgBottomRight.Width);

                e.Graphics.DrawImage(this._imgBottom, this.Width - this._imgBottomRight.Width - this._imgBottom.Width, this.Height - this._imgBottom.Height, this._imgBottom.Width, this._imgBottom.Height);

                // Draw TopLeft
                e.Graphics.DrawImage(this._imgTopLeft, 0, 0, this._imgTopLeft.Width, this._imgTopLeft.Height);

                // Draw TopRight
                e.Graphics.DrawImage(this._imgTopRight, this.Width - this._imgTopRight.Width, 0, this._imgTopRight.Width, this._imgTopRight.Height);

                // Draw Bottom Left
                e.Graphics.DrawImage(this._imgBottomLeft, 0, this.Height - this._imgBottomLeft.Height, this._imgBottomLeft.Width, this._imgBottomLeft.Height);

                // Draw Bottom Right
                e.Graphics.DrawImage(this._imgBottomRight, this.Width - this._imgBottomRight.Width, this.Height - this._imgBottomRight.Height, this._imgBottomRight.Width, this._imgBottomRight.Height);
            }
        }

        /// <summary>
        /// 이미지가 초기화 되었는지를 확인합니다.
        /// </summary>
        /// <returns></returns>
        private bool IsInitializeImage()
        {
            bool result = true;

            if (this._imgTop == null ||
                this._imgBottom == null ||
                this._imgLeft == null ||
                this._imgRight == null ||
                this._imgTopLeft == null ||
                this._imgTopRight == null ||
                this._imgBottomLeft == null ||
                this._imgBottomRight == null)
                result = false;

            return result;
        }

        #endregion

        private void lbl_MouseDown(object sender, MouseEventArgs e)
        {
            this.OnMouseDown(e);
        }

        private void lbl_MouseMove(object sender, MouseEventArgs e)
        {
            this.OnMouseMove(e);
        }

        private void lbl_MouseUp(object sender, MouseEventArgs e)
        {
            this.OnMouseUp(e);
        }
    }
}
