namespace ConvolutionLayer.Trainer.WeightUpdater
{
    public interface IWeightUpdater
    {
        void UpdateWeights(
            MemFloat currentLayerKernel,
            MemFloat deltaWeights,
            float learningRate
            );
    }
}