using System;
using System.Drawing;
using System.Windows.Forms;

namespace Myphoto
{
    internal class PixelDialog
    {
        public bool IsDisposed { get; internal set; }
        public MainForm Owner { get; internal set; }
        public bool Visible { get; internal set; }
        public string Text { get; internal set; }

#pragma warning disable CS0109 // Member does not hide an inherited member; new keyword is not required

        internal void Show() => throw new NotImplementedException();

#pragma warning restore CS0109 // Member does not hide an inherited member; new keyword is not required
        internal void ClearPixelData() => throw new NotImplementedException();

        internal void UpdatePixelData(int x, int y, Bitmap bmp, Rectangle displayRectangle, Rectangle rectangle, PictureBoxSizeMode sizeMode)
        {
            throw new NotImplementedException();
        }
    }
}