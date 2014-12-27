using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using Microsoft.VisualBasic;

namespace ConvolutionLayer
{
    class Program
    {
        static void Main(string[] args)
        {
            const int ImageSize = 5;
            const int KernelSize = 3;
            const int ForwardSize = ImageSize - KernelSize + 1;

            var random = new Random(564);

            var image = new Img(ImageSize, ImageSize);
            for (var cc = 0; cc < ImageSize; cc++)
            {
                image.SetValueFromCoord(ImageSize - cc - 1, cc, 1f);
            }
            //LayerVisualizer.Show("image", image.Values, image.Width, image.Height);

            var desired = new Img(ForwardSize, ForwardSize);
            for (var cc = 0; cc < ForwardSize; cc++)
            {
                desired.SetValueFromCoord(ForwardSize - cc - 1, cc, 1f);
            }

            var kernel = new Kernel(KernelSize, KernelSize);
            kernel.Values.Fill(j => (float)random.NextDouble());
            //for (var cc = 0; cc < KernelSize; cc++)
            //{
            //    kernel.SetValueFromCoord(kernel.PartHorizontal, cc, 1f);
            //}
            LayerVisualizer.Show("default kernel", kernel.Values, kernel.Width, kernel.Height);

            //функцию нейрона применять не будем для простоты
            //т.е. y = x, y' = 1

            //функция ошибки
            var e = new HalfSquaredEuclidianDistance();

            for(var epoch = 0; ; epoch++)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();

                LayerVisualizer.Show("image", image.Values, image.Width, image.Height);

                LayerVisualizer.Show("desired", desired.Values, desired.Width, desired.Height);

                //вычисляем проход вперед
                var forward = new Img(ForwardSize, ForwardSize);
                for (var i = kernel.PartHorizontal; i < ImageSize - kernel.PartHorizontal; i++)
                {
                    for (var j = kernel.PartHorizontal; j < ImageSize - kernel.PartHorizontal; j++)
                    {
                        var sum = 0f;
                        for (var a = -kernel.PartHorizontal; a <= kernel.PartHorizontal; a++)
                        {
                            for (var b = -kernel.PartVertical; b <= kernel.PartVertical; b++)
                            {
                                var w = kernel.GetValueFromCenter(a, b);
                                var y = image.GetValueFromCoordSafely(i + a, j + b);

                                var z = w * y;
                                sum += z;
                            }
                        }

                        forward.SetValueFromCoord(
                            i - kernel.PartHorizontal,
                            j - kernel.PartHorizontal,
                            sum
                            );
                    }
                }
                LayerVisualizer.Show("forward w\\o function", forward.Values, forward.Width, forward.Height);

                //вычисляем значение ошибки (dE/dy)
                var err = new Img(desired.Width, desired.Height);

                for (var w = 0; w < err.Width; w++)
                {
                    for (var h = 0; h < err.Height; h++)
                    {
                        var index = h*err.Width + w;

                        var errv = e.CalculatePartialDerivativeByV2Index(
                            desired.Values,
                            forward.Values,
                            index
                            );

                        err.Values[index] = errv;
                    }
                }

                LayerVisualizer.Show(
                    "err",
                    err.Values,
                    err.Width,
                    err.Height
                    );


                //вычисляем производную функции активации нейрона
                var delta = new Kernel(KernelSize, KernelSize);

                for (var a = 0; a < KernelSize; a++)
                {
                    for (var b = 0; b < KernelSize; b++)
                    {
                        var dEdw_ab = 0f;

                        for (var i = 0; i < forward.Width; i++)
                        {
                            for (var j = 0; j < forward.Height; j++)
                            {
                                var sigma_sh_ij = 1f;

                                var dEdy_ij = err.GetValueFromCoordSafely(i, j);
                                var dEdz_ij = dEdy_ij*sigma_sh_ij;

                                var y = image.GetValueFromCoordSafely(
                                    i + a,
                                    j + b
                                    );

                                var mul = dEdz_ij * y;

                                dEdw_ab += mul;
                            }
                        }

                        //вычислено dEdw_ab
                        delta.SetValueFromCoord(a, b, dEdw_ab);
                    }
                }

                LayerVisualizer.Show(
                    "delta (" + epoch + ")",
                    delta.Values,
                    delta.Width,
                    delta.Height
                    );

                //вычитаем
                const float LearningRate = 0.1f;

                for (var a = 0; a < kernel.Width; a++)
                {
                    for (var b = 0; b < kernel.Height; b++)
                    {
                        kernel.SetValueFromCoord(
                            a,
                            b,
                            kernel.GetValueFromCoord(a, b) - LearningRate * delta.GetValueFromCoord(a, b));
                    }
                }

                LayerVisualizer.Show(
                    "kernel (" + epoch + ")",
                    kernel.Values,
                    kernel.Width,
                    kernel.Height
                    );

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.ReadLine();
            }

        }
    }
}
