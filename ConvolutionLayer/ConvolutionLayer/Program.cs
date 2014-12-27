﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using ConvolutionLayer.Helper;
using ConvolutionLayer.Metrics;
using ConvolutionLayer.Trainer;
using ConvolutionLayer.Trainer.Convolution;
using ConvolutionLayer.Trainer.Convolution.Calculator;
using ConvolutionLayer.Trainer.Convolution.Delta;
using ConvolutionLayer.Trainer.ErrorCalculator;
using ConvolutionLayer.Trainer.WeightUpdater;
using Microsoft.VisualBasic;

namespace ConvolutionLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            const int ImageSize = 5;
            const int KernelSize = 3;
            const int ConvolutionSize = ImageSize - KernelSize + 1;
            const float LearningRate = 0.1f;
            const int MaxEpoches = 100;

            var random = new Random(5641);

            var image = new MemFloat(ImageSize, ImageSize);
            for (var cc = 0; cc < ImageSize; cc++)
            {
                image.SetValueFromCoord(ImageSize - cc - 1, cc, 1f);
            }
            //LayerVisualizer.Show("image", image.Values, image.Width, image.Height);

            var desiredValues = new MemFloat(ConvolutionSize, ConvolutionSize);
            for (var cc = 0; cc < ConvolutionSize; cc++)
            {
                desiredValues.SetValueFromCoord(ConvolutionSize - cc - 1, cc, 1f);
            }

            var kernel = new MemFloat(KernelSize, KernelSize);
            kernel.Values.Fill(j => (float)random.NextDouble());
            //for (var cc = 0; cc < KernelSize; cc++)
            //{
            //    kernel.SetValueFromCoord(KernelSize / 2, cc, 1f);
            //}
            LayerVisualizer.Show("default kernel", kernel.Values, kernel.Width, kernel.Height);

            //функцию нейрона применять не будем для простоты
            //т.е. y = x, y' = 1

            //функция ошибки
            var e = new HalfSquaredEuclidianDistance();

            var convolutionCalculator = new NaiveConvolutionCalculator();

            var weightUpdater = new NaiveWeightUpdater();

            var errorCalculator = new NaiveErrorCalculator();

            var deltaCalculator = new NaiveDeltaCalculator();

            var oneLayerTrainer = new OneLayerTrainer(
                convolutionCalculator,
                weightUpdater,
                errorCalculator,
                deltaCalculator,
                e
                );

            oneLayerTrainer.DoTrain(
                image,
                kernel,
                desiredValues,
                MaxEpoches,
                ConvolutionSize,
                KernelSize,
                LearningRate
                );

        }
    }
}
