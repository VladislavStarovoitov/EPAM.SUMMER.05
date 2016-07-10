using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polynomial
{
    /// <summary>
    /// Provides methods for creating and manipulating polynomials.
    /// </summary>
    public class Polynomial
    {
        private double[] _coefficients;

        public Polynomial(params double[] coefficients)
        {
            _coefficients = (double[])coefficients.Clone();
        }

        public static Polynomial operator +(Polynomial firstPolynomial, Polynomial secondPolynomial)
        {
            return AddOrSub(firstPolynomial, secondPolynomial, (first, second) => first + second);
        } 

        public static Polynomial operator -(Polynomial firstPolynomial, Polynomial secondPolynomial)
        {
            return AddOrSub(firstPolynomial, secondPolynomial, (first, second) => first - second);
        }

        private static Polynomial AddOrSub(Polynomial firstPolynomial, Polynomial secondPolynomial, Func<double, double, double> operation)
        {
            double[] firstCoefficients = firstPolynomial._coefficients;
            double[] secondCoefficients = secondPolynomial._coefficients;

            EqualizeCoefficientsLength(firstCoefficients, secondCoefficients);
            int degree = firstCoefficients.Length;

            double[] resultCoefficients = new double[degree];
            for (int i = 0; i < degree; i++)
            {
                resultCoefficients[i] = operation(firstCoefficients[i], secondCoefficients[i]);
            }
            return new Polynomial(resultCoefficients);
        }

        private static void EqualizeCoefficientsLength(double[] firstCoefficients, double[] secondCoefficients)
        {
            if (firstCoefficients.Length != secondCoefficients.Length)
            {
                if (firstCoefficients.Length > secondCoefficients.Length)
                {
                    Array.Resize(ref secondCoefficients, firstCoefficients.Length);
                }
                else
                {
                    Array.Resize(ref firstCoefficients, secondCoefficients.Length);
                }
            }
        }
    }
}
