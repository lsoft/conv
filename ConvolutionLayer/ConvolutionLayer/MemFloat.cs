using System;

namespace ConvolutionLayer
{
    public class MemFloat
    {
        public float[] Array
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

        public MemFloat(int width, int height, float[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            if (array.Length != width*height)
            {
                throw new ArgumentException("Array.Length != width*height");
            }

            Width = width;
            Height = height;
            this.Array = array;
        }

        public MemFloat(int width, int height)
        {
            Width = width;
            Height = height;
            this.Array = new float[width * height];
        }

        public void SetValueFromCoord(int readw, int readh, float value)
        {
            Array[readh * Width + readw] = value;
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
                Array[fromw * Width + fromh];
        }

    }
}