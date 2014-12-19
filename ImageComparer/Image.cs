using System;
using System.IO;
using System.Drawing;

namespace ImageComparer
{
    class Image
    {
        public float Percent { get; set; }
        private int FailCount { get; set; }
        private int TotalPixels { get; set; }
        public float Difference
        {
            get { return (this.TotalPixels - this.FailCount) / this.TotalPixels; }
        }

        public Image()
        {
            this.Percent = 1;
            this.FailCount = 0;
            this.TotalPixels = 0;
        }

        public bool SameAs(string fileNameOne, string fileNameTwo, float percent)
        {
            if (this.FilesExist(fileNameOne, fileNameTwo))
            {
                this.Percent = percent;
                if (this.CompareImages(fileNameOne, fileNameTwo))
                    return true;
            }
            return false;
        }

        public bool SameAs(string fileNameOne, string fileNameTwo)
        {
            return this.SameAs(fileNameOne, fileNameTwo, 1);
        }

        private bool CompareImages(string fileNameOne, string fileNameTwo)
        {
            var imageOne = new Bitmap(fileNameOne);
            var imageTwo = new Bitmap(fileNameTwo);

            if (this.IsSizeSame(imageOne, imageTwo))
            {
                this.PixelDifference(imageOne, imageTwo);
                if (this.Difference >= this.Percent)
                    return true;
            }
            return false;
        }

        private void PixelDifference(Bitmap imageOne, Bitmap imageTwo)
        {
            for (int x = 0; x < imageOne.Width; x++)
            {
                for (int y = 0; y < imageOne.Height; y++)
                {
                    this.CompareColor(imageOne.GetPixel(x, y).ToString(),
                        imageTwo.GetPixel(x, y).ToString());
                    this.TotalPixels++;
                }
            }
        }

        private void CompareColor(string pixelOne, string pixelTwo)
        {
            if (pixelOne != pixelTwo)
                this.FailCount++;
        }

        private bool IsSizeSame(Bitmap imageOne, Bitmap imageTwo)
        {
            if (imageOne.Width == imageTwo.Width &&
                imageOne.Height == imageTwo.Height)
            {
                if (imageOne.Width > 0 &&
                    imageOne.Height > 0)
                    return true;
            }
            return false;
        }

        private bool FilesExist(string fileNameOne, string fileNameTwo)
        {
            if (File.Exists(fileNameOne))
                if (File.Exists(fileNameTwo))
                    return true;
                else
                    throw new FileNotFoundException("Could not find " + fileNameTwo.ToString());
            else
                throw new FileNotFoundException("Could not find " + fileNameOne.ToString());
        }
    }
}
