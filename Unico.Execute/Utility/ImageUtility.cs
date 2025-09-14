using System.Drawing;
using System.Drawing.Imaging;

namespace Executable.Utility
{
    public class ImageUtility
    {
        protected static ImageUtility INSTANCE = new ImageUtility();
        public ImageUtility()
        {
        }
        public static ImageUtility GetInstance()
        {
            return INSTANCE;
        }
        public Bitmap ChangeOpacity(Image img, float opacityvalue)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height); // Determining Width and Height of Source Image
            ImageAttributes imgAttribute = new ImageAttributes();
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                imgAttribute.SetColorMatrix(new ColorMatrix() { Matrix33 = opacityvalue }, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                graphics.DrawImage(img, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttribute);
                graphics.Dispose();   // Releasing all resource used by graphics 
            }
            return bmp;
        }
    }
}
