using System;
using ConvolutionLayer.Helper;
using ConvolutionLayer.Metrics;
using ConvolutionLayer.Trainer.Convolution.Calculator;
using ConvolutionLayer.Trainer.Convolution.Delta;
using ConvolutionLayer.Trainer.ErrorCalculator;
using ConvolutionLayer.Trainer.WeightUpdater;

namespace ConvolutionLayer.Trainer
{
    public class OneLayerTrainer
    {
        private readonly IConvolutionCalculator _convolutionCalculator;
        private readonly IWeightUpdater _weightUpdater;
        private readonly IErrorCalculator _errorCalculator;
        private readonly IDeltaCalculator _deltaCalculator;
        private readonly IMetrics _e;

        public OneLayerTrainer(
            IConvolutionCalculator convolutionCalculator,
            IWeightUpdater weightUpdater,
            IErrorCalculator errorCalculator,
            IDeltaCalculator deltaCalculator,
            IMetrics e
            )
        {
            if (convolutionCalculator == null)
            {
                throw new ArgumentNullException("convolutionCalculator");
            }
            if (weightUpdater == null)
            {
                throw new ArgumentNullException("weightUpdater");
            }
            if (errorCalculator == null)
            {
                throw new ArgumentNullException("errorCalculator");
            }
            if (deltaCalculator == null)
            {
                throw new ArgumentNullException("deltaCalculator");
            }
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            _convolutionCalculator = convolutionCalculator;
            _weightUpdater = weightUpdater;
            _errorCalculator = errorCalculator;
            _deltaCalculator = deltaCalculator;
            _e = e;
        }

        public void DoTrain(
            MemFloat image,
            MemFloat kernel,
            MemFloat desiredValues,
            int maxEpoches,
            int convolutionSize,
            int kernelSize,
            float learningRate
            )
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }
            if (desiredValues == null)
            {
                throw new ArgumentNullException("desiredValues");
            }


            for (var epoch = 0; epoch < maxEpoches; epoch++)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                LayerVisualizer.Show("image", image.Values, image.Width, image.Height);

                LayerVisualizer.Show("desired", desiredValues.Values, desiredValues.Width, desiredValues.Height);

                //вычисляем проход вперед
                var convolution = _convolutionCalculator.CalculateConvolution(
                    kernel,
                    image,
                    convolutionSize
                    );
                LayerVisualizer.Show("convolution", convolution.Values, convolution.Width, convolution.Height);

                //вычисляем значение ошибки (dE/dy)
                var err = _errorCalculator.CalculateError(
                    convolution,
                    desiredValues,
                    _e
                    );
                LayerVisualizer.Show("err", err.Values, err.Width, err.Height);

                //вычисляем производную функции активации нейрона
                var delta = _deltaCalculator.CalculateDelta(
                    convolution,
                    err,
                    image,
                    kernelSize
                    );

                LayerVisualizer.Show("delta (" + epoch + ")", delta.Values, delta.Width, delta.Height);

                //вычитаем
                _weightUpdater.UpdateWeights(
                    kernel,
                    delta,
                    learningRate
                    );

                LayerVisualizer.Show("kernel (" + epoch + ")", kernel.Values, kernel.Width, kernel.Height);

            }

            Console.ReadLine();
        }
    }
}