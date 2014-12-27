namespace ConvolutionLayer.Activation
{
    public interface IFunction
    {
        string ShortName
        {
            get;
        }

        float Compute(float x);
        float ComputeFirstDerivative(float x);

    }
}