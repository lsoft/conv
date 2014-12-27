using ConvolutionLayer.Activation;

namespace ConvolutionLayer.Trainer.Convolution.Calculator
{
    public interface IConvolutionCalculator
    {
        void CalculateConvolution(
            IFunction activationFunction,
            MemFloat currentLayerKernel,
            MemFloat previousLayer,
            int forwardSize,
            MemFloat currentLayerNET,
            MemFloat currentLayerState
            );
    }
}