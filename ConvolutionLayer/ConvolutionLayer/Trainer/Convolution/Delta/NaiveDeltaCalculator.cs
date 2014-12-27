using System;
using System.Runtime.CompilerServices;
using ConvolutionLayer.Activation;

namespace ConvolutionLayer.Trainer.Convolution.Delta
{
    public class NaiveDeltaCalculator : IDeltaCalculator
    {
        public MemFloat CalculateDelta(
            IFunction activationFunction,
            MemFloat currentLayerNET,
            MemFloat nextLayerDeDy,
            MemFloat previousLayer,
            int kernelSize
            )
        {
            if (activationFunction == null)
            {
                throw new ArgumentNullException("activationFunction");
            }
            if (currentLayerNET == null)
            {
                throw new ArgumentNullException("currentLayerNET");
            }
            if (nextLayerDeDy == null)
            {
                throw new ArgumentNullException("nextLayerDeDy");
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

                    for (var i = 0; i < currentLayerNET.Width; i++)
                    {
                        for (var j = 0; j < currentLayerNET.Height; j++)
                        {
                            var z_ij = currentLayerNET.GetValueFromCoordSafely(i, j);
                            var sigma_sh_ij = activationFunction.ComputeFirstDerivative(z_ij);

                            var dEdy_ij = nextLayerDeDy.GetValueFromCoordSafely(i, j);
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