namespace ConvolutionLayer.Trainer.Convolution.Delta
{
    public interface IDeltaCalculator
    {
        MemFloat CalculateDelta(
            MemFloat currentLayer,
            MemFloat nextLayerDeDz,
            MemFloat previousLayer,
            int kernelSize
            );
    }
}