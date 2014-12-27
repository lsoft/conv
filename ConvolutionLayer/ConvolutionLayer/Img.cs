using System;

namespace ConvolutionLayer
{
    public class Img
    {
        public float[] Values
        {
            get;
            private set;
        }

        public int Width
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        public Img(int width, int height, float[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            if (values.Length != width*height)
            {
                throw new ArgumentException("values.Length != width*height");
            }

            Width = width;
            Height = height;
            this.Values = values;
        }

        public Img(int width, int height)
        {
            Width = width;
            Height = height;
            this.Values = new float[width * height];
        }

        public void SetValueFromCoord(int readw, int readh, float value)
        {
            Values[readh * Width + readw] = value;
        }

        public float GetValueFromCoordSafely(int fromw, int fromh)
        {
            if (fromw < 0 || fromw >= Width)
            {
                throw new InvalidOperationException("fromw < 0 || fromw >= Width");
            }
            if (fromh < 0 || fromh >= Height)
            {
                throw new InvalidOperationException("fromh < 0 || fromh >= Height");
            }

            return
                Values[fromw * Width + fromh];
        }

    }
}