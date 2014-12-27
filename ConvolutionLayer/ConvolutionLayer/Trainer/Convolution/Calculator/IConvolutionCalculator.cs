namespace ConvolutionLayer.Trainer.Convolution.Calculator
{
    public interface IConvolutionCalculator
    {
        MemFloat CalculateConvolution(
            MemFloat currentLayerKernel,
            MemFloat previousLayer,
            int forwardSize
            );
    }
}