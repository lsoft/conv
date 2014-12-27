namespace ConvolutionLayer.Metrics
{
    public interface IMetrics
    {
        float Calculate(
            float[] desiredValues,
            float[] predictedValues
            );

        float CalculatePartialDerivativeByV2Index(
            float[] desiredValues,
            float[] predictedValues,
            int v2Index
            );
    }
}