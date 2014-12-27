using System;
using ConvolutionLayer.Activation;

namespace ConvolutionLayer.Trainer.Convolution.Calculator
{
    public class NaiveConvolutionCalculator : IConvolutionCalculator
    {
        public void CalculateConvolution(
            IFunction activationFunction,
            MemFloat currentLayerKernel,
            MemFloat previousLayer,
            int forwardSize,
            MemFloat currentLayerNET, //z
            MemFloat currentLayerState //y
            )
        {
            if (activationFunction == null)
            {
                throw new ArgumentNullException("activationFunction");
            }
            if (currentLayerKernel == null)
            {
                throw new ArgumentNullException("currentLayerKernel");
            }
            if (previousLayer == null)
            {
                throw new ArgumentNullException("previousLayer");
            }

            //делаем свертку
            for (var i = 0; i < currentLayerNET.Width; i++)
            {
                for (var j = 0; j < currentLayerNET.Height; j++)
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

                    currentLayerNET.SetValueFromCoord(
                        i,
                        j,
                        zSum
                        );
                }
            }

            //применяем функцию активации
            for (var i = 0; i < currentLayerState.Width; i++)
            {
                for (var j = 0; j < currentLayerState.Height; j++)
                {
                    var prevv = currentLayerNET.GetValueFromCoordSafely(i, j);

                    var newv = activationFunction.Compute(prevv);

                    currentLayerState.SetValueFromCoord(i, j, newv);
                }
            }
        }
    }
}