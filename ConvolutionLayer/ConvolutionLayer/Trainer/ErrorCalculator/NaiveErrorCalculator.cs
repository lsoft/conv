using System;
using ConvolutionLayer.Metrics;

namespace ConvolutionLayer.Trainer.ErrorCalculator
{
    public class NaiveErrorCalculator : IErrorCalculator
    {
        public MemFloat CalculateError(
            MemFloat calculatedValues,
            MemFloat desiredValues,
            IMetrics e
            )
        {
            if (calculatedValues == null)
            {
                throw new ArgumentNullException("calculatedValues");
            }
            if (desiredValues == null)
            {
                throw new ArgumentNullException("desiredValues");
            }
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            var err = new MemFloat(desiredValues.Width, desiredValues.Height);
            for (var w = 0; w < err.Width; w++)
            {
                for (var h = 0; h < err.Height; h++)
                {
                    var index = h * err.Width + w;

                    var errv = e.CalculatePartialDerivativeByV2Index(
                        desiredValues.Values,
                        calculatedValues.Values,
                        index
                        );

                    err.Values[index] = errv;
                }
            }

            return err;
        }
    }
}