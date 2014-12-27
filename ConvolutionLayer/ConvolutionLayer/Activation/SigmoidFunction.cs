using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvolutionLayer.Activation
{
    public class SigmoidFunction : IFunction
    {

        private readonly float _alpha = 1.0f;

        public string ShortName
        {
            get
            {
                return "Sigm";
            }
        }


        public SigmoidFunction(float alpha)
        {
            _alpha = alpha;
        }

        public float Compute(float x)
        {
            var r = (float)(1.0 / (1.0 + Math.Exp(-1.0 * _alpha * x)));
            return r;
        }

        public float ComputeFirstDerivative(float x)
        {
            var computed = this.Compute(x);

            return
                (float)(_alpha * computed * (1.0 - computed));
        }
    }
}
