using ConvolutionLayer.Activation;

namespace ConvolutionLayer.Trainer.Convolution.Delta
{
    public interface IDeltaCalculator
    {
        MemFloat CalculateDelta(
            IFunction activationFunction,
            MemFloat currentLayerNET,
            MemFloat nextLayerDeDy,
            MemFloat previousLayer,
            int kernelSize
            );
    }
}