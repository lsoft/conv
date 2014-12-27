using System;
using ConvolutionLayer.Activation;
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
            IFunction activationFunction,
            MemFloat image,
            MemFloat kernel,
            MemFloat desiredValues,
            int maxEpoches,
            int convolutionSize,
            int kernelSize,
            float learningRate
            )
        {
            if (activationFunction == null)
            {
                throw new ArgumentNullException("activationFunction");
            }
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

                LayerVisualizer.Show("image", image.Array, image.Width, image.Height);

                LayerVisualizer.Show("desired", desiredValues.Array, desiredValues.Width, desiredValues.Height);

                //вычисляем проход вперед
                var currentLayerNET = new MemFloat(convolutionSize, convolutionSize);
                var currentLayerState = new MemFloat(convolutionSize, convolutionSize);
                _convolutionCalculator.CalculateConvolution(
                    activationFunction,
                    kernel,
                    image,
                    convolutionSize,
                    currentLayerNET,
                    currentLayerState
                    );
                LayerVisualizer.Show("currentLayerNET (convolution w\\o activation function)", currentLayerNET.Array, currentLayerNET.Width, currentLayerNET.Height);
                LayerVisualizer.Show("currentLayerState (convolution with activation function)", currentLayerState.Array, currentLayerState.Width, currentLayerState.Height);

                //вычисляем значение ошибки (dE/dy)
                var dEdY = _errorCalculator.CalculateError(
                    currentLayerState,
                    desiredValues,
                    _e
                    );
                LayerVisualizer.Show("err (dE/dY)", dEdY.Array, dEdY.Width, dEdY.Height);

                //вычисляем производную функции активации нейрона
                var delta = _deltaCalculator.CalculateDelta(
                    activationFunction,
                    currentLayerNET,
                    dEdY,
                    image,
                    kernelSize
                    );

                LayerVisualizer.Show("delta (" + epoch + ")", delta.Array, delta.Width, delta.Height);

                //вычитаем
                _weightUpdater.UpdateWeights(
                    kernel,
                    delta,
                    learningRate
                    );

                LayerVisualizer.Show("kernel (" + epoch + ")", kernel.Array, kernel.Width, kernel.Height);

                //Console.ReadLine();

            }

            Console.ReadLine();
        }
    }

    public class ConvolutionLayerContainer
    {
        public MemFloat Net
        {
            get;
            private set;
        }

        public MemFloat State
        {
            get;
            private set;
        }

        public MemFloat Kernel
        {
            get;
            private set;
        }

        public ConvolutionLayerContainer(
            int convolutionWidth,
            int convolutionHeight,
            int kernelWidth,
            int kernelHeight
            )
        {
            this.Net = new MemFloat(convolutionWidth, convolutionHeight);
            this.State = new MemFloat(convolutionWidth, convolutionHeight);

            this.Kernel = new MemFloat(kernelWidth, kernelHeight);
        }
    }

    public interface ILayerPropagator
    {
        void ComputeLayer(
            );
    }

    public class ConvolutionLayerPropagator : ILayerPropagator
    {
        private readonly ConvolutionLayerContainer _previousContainer;
        private readonly ConvolutionLayerContainer _currentContainer;
        private readonly IConvolutionCalculator _convolutionCalculator;
        private readonly int _convolutionSize;
        private readonly IFunction _activationFunction;

        public ConvolutionLayerPropagator(
            ConvolutionLayerContainer previousContainer,
            ConvolutionLayerContainer currentContainer,
            IConvolutionCalculator convolutionCalculator,
            int convolutionSize,
            IFunction activationFunction

            )
        {
            if (previousContainer == null)
            {
                throw new ArgumentNullException("previousContainer");
            }
            if (currentContainer == null)
            {
                throw new ArgumentNullException("currentContainer");
            }
            if (convolutionCalculator == null)
            {
                throw new ArgumentNullException("convolutionCalculator");
            }
            if (activationFunction == null)
            {
                throw new ArgumentNullException("activationFunction");
            }

            _previousContainer = previousContainer;
            _currentContainer = currentContainer;
            _convolutionCalculator = convolutionCalculator;
            _convolutionSize = convolutionSize;
            _activationFunction = activationFunction;
        }

        public void ComputeLayer(
            )
        {
            _convolutionCalculator.CalculateConvolution(
                _activationFunction,
                _currentContainer.Kernel,
                _previousContainer.State,
                _convolutionSize,
                _currentContainer.Net,
                _currentContainer.State
                );
            LayerVisualizer.Show("currentLayerNET (convolution w\\o activation function)", _currentContainer.Net.Array, _currentContainer.Net.Width, _currentContainer.Net.Height);
            LayerVisualizer.Show("currentLayerState (convolution with activation function)", _currentContainer.State.Array, _currentContainer.State.Width, _currentContainer.State.Height);
        }
    }

}