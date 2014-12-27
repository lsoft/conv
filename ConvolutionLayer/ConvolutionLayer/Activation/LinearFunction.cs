using System;

namespace ConvolutionLayer.Activation
{
    [Serializable]
    public class LinearFunction : IFunction
    {

        private readonly float _alpha = 1.0f;

        public string ShortName
        {
            get
            {
                return "Lin";
            }
        }

        public LinearFunction(float alpha)
        {
            _alpha = alpha;
        }

        public float Compute(float x)
        {
            var r = _alpha * x;
            return r;
        }

        public float ComputeFirstDerivative(float computed)
        {
            return
                _alpha;
        }
    }
}
