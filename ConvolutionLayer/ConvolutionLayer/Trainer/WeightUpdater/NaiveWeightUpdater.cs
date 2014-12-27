using System;

namespace ConvolutionLayer.Trainer.WeightUpdater
{
    public class NaiveWeightUpdater : IWeightUpdater
    {
        public void UpdateWeights(
            MemFloat currentLayerKernel,
            MemFloat deltaWeights,
            float learningRate
            )
        {
            if (currentLayerKernel == null)
            {
                throw new ArgumentNullException("currentLayerKernel");
            }
            if (deltaWeights == null)
            {
                throw new ArgumentNullException("deltaWeights");
            }

            for (var a = 0; a < currentLayerKernel.Width; a++)
            {
                for (var b = 0; b < currentLayerKernel.Height; b++)
                {
                    var origv = currentLayerKernel.GetValueFromCoordSafely(a, b);
                    var deltav = deltaWeights.GetValueFromCoordSafely(a, b);

                    var newv = origv - learningRate * deltav;

                    currentLayerKernel.SetValueFromCoord(
                        a,
                        b,
                        newv
                        );
                }
            }
        }
    }
}