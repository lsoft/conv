using ConvolutionLayer.Metrics;

namespace ConvolutionLayer.Trainer.ErrorCalculator
{
    public interface IErrorCalculator
    {
        MemFloat CalculateError(
            MemFloat calculatedValues,
            MemFloat desiredValues,
            IMetrics e
            );
    }
}