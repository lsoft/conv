using System;

namespace ConvolutionLayer.Trainer.Convolution.Delta
{
    public class NaiveDeltaCalculator : IDeltaCalculator
    {
        public MemFloat CalculateDelta(
            MemFloat currentLayer,
            MemFloat nextLayerDeDz,
            MemFloat previousLayer,
            int kernelSize
            )
        {
            if (currentLayer == null)
            {
                throw new ArgumentNullException("currentLayer");
            }
            if (nextLayerDeDz == null)
            {
                throw new ArgumentNullException("nextLayerDeDz");
            }
            if (previousLayer == null)
            {
                throw new ArgumentNullException("previousLayer");
            }

            var delta = new MemFloat(kernelSize, kernelSize);
            for (var a = 0; a < kernelSize; a++)
            {
                for (var b = 0; b < kernelSize; b++)
                {
                    var dEdw_ab = 0f;

                    for (var i = 0; i < currentLayer.Width; i++)
                    {
                        for (var j = 0; j < currentLayer.Height; j++)
                        {
                            var sigma_sh_ij = 1f;

                            var dEdy_ij = nextLayerDeDz.GetValueFromCoordSafely(i, j);
                            var dEdz_ij = dEdy_ij * sigma_sh_ij;

                            var y = previousLayer.GetValueFromCoordSafely(
                                i + a,
                                j + b
                                );

                            var mul = dEdz_ij * y;

                            dEdw_ab += mul;
                        }
                    }

                    //вычислено dEdw_ab
                    delta.SetValueFromCoord(a, b, dEdw_ab);
                }
            }
            return delta;
        }
    }
}