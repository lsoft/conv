using System;

namespace ConvolutionLayer.Trainer.Convolution.Calculator
{
    public class NaiveConvolutionCalculator : IConvolutionCalculator
    {
        public MemFloat CalculateConvolution(
            MemFloat currentLayerKernel,
            MemFloat previousLayer,
            int forwardSize
            )
        {
            if (currentLayerKernel == null)
            {
                throw new ArgumentNullException("currentLayerKernel");
            }
            if (previousLayer == null)
            {
                throw new ArgumentNullException("previousLayer");
            }

            var zConvolution = new MemFloat(forwardSize, forwardSize);
            for (var i = 0; i < zConvolution.Width; i++)
            {
                for (var j = 0; j < zConvolution.Height; j++)
                {
                    var zSum = 0f;
                    for (var a = 0; a < currentLayerKernel.Width; a++)
                    {
                        for (var b = 0; b < currentLayerKernel.Height; b++)
                        {
                            var w = currentLayerKernel.GetValueFromCoordSafely(a, b);
                            var y = previousLayer.GetValueFromCoordSafely(i + a, j + b);

                            var z = w * y;
                            zSum += z;
                        }
                    }

                    zConvolution.SetValueFromCoord(
                        i,
                        j,
                        zSum
                        );
                }
            }

            return zConvolution;
        }
    }
}