using System;

namespace ConvolutionLayer
{
    public class Kernel
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

        public int PartHorizontal
        {
            get
            {
                return
                    (Width - 1)/2;
            }
        }

        public int PartVertical
        {
            get
            {
                return
                    (Height - 1) / 2;
            }
        }

        public Kernel(int width, int height, float[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }
            if ((width % 2) != 1)
            {
                throw new ArgumentException("(width%2) != 1");
            }
            if ((height % 2) != 1)
            {
                throw new ArgumentException("(height%2) != 1");
            }
            if (values.Length != width * height)
            {
                throw new ArgumentException("values.Length != width*height");
            }

            Width = width;
            Height = height;
            Values = values;
        }

        public Kernel(int width, int height)
        {
            if ((width % 2) != 1)
            {
                throw new ArgumentException("(width%2) != 1");
            }
            if ((height % 2) != 1)
            {
                throw new ArgumentException("(height%2) != 1");
            }

            Width = width;
            Height = height;
            Values = new float[width * height];
        }

        public void SetValueFromCoord(int readw, int readh, float value)
        {
            Values[readh * Width + readw] = value;
        }

        public float GetValueFromCenter(int shiftw, int shifth)
        {
            var readw = shiftw + PartHorizontal;
            var readh = shifth + PartVertical;

            return
                Values[readh * Width + readw];
        }

        public float GetValueFromCoord(int readw, int readh)
        {
            return
                Values[readh * Width + readw];
        }
    }
}