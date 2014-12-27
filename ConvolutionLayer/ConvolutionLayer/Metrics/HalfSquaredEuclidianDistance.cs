using System;

namespace ConvolutionLayer.Metrics
{
    public class HalfSquaredEuclidianDistance : IMetrics
    {
        public float Calculate(
            float[] desiredValues,
            float[] predictedValues
            )
        {
            if (desiredValues == null)
            {
                throw new ArgumentNullException("desiredValues");
            }
            if (predictedValues == null)
            {
                throw new ArgumentNullException("predictedValues");
            }
            if (desiredValues.Length != predictedValues.Length)
            {
                throw new InvalidOperationException("v1.Length != v2.Length");
            }

            var d = 0.0f;
            for (var i = 0; i < desiredValues.Length; i++)
            {
                var diff = desiredValues[i] - predictedValues[i];
                d += diff * diff;
            }

            return 0.5f * d;
        }

        public float CalculatePartialDerivativeByV2Index(
            float[] desiredValues,
            float[] predictedValues,
            int v2Index
            )
        {
            if (desiredValues == null)
            {
                throw new ArgumentNullException("desiredValues");
            }
            if (predictedValues == null)
            {
                throw new ArgumentNullException("predictedValues");
            }
            if (desiredValues.Length != predictedValues.Length)
            {
                throw new InvalidOperationException("v1.Length != v2.Length");
            }
            if (v2Index >= predictedValues.Length)
            {
                throw new ArgumentException("v2Index >= v2.Length");
            }

            return
                predictedValues[v2Index] - desiredValues[v2Index];
        }
    }

}
