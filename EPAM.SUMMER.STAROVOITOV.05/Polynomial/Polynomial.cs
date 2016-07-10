using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Polynomial
{
    /// <summary>
    /// Provides methods for creating and manipulating polynomials.
    /// </summary>
    public class Polynomial: IEquatable<Polynomial>
    {
        private double[] _coefficients;

        public Polynomial(params double[] coefficients)
        {
            if (coefficients == null)
            {
                _coefficients = new double[0];
            }
            else
            {
                _coefficients = (double[])coefficients.Clone();
            }
        }

        public static Polynomial operator +(Polynomial firstPolynomial, Polynomial secondPolynomial)
        {
            return AddOrSub(firstPolynomial, secondPolynomial, (first, second) => first + second);
        } 

        public static Polynomial operator -(Polynomial firstPolynomial, Polynomial secondPolynomial)
        {
            return AddOrSub(firstPolynomial, secondPolynomial, (first, second) => first - second);
        }

        public static Polynomial operator *(Polynomial firstPolynomial, Polynomial secondPolynomial)
        {
            double[] firstCoefficients = firstPolynomial._coefficients;
            double[] secondCoefficients = secondPolynomial._coefficients;

            double[] resultCoefficients = new double[firstCoefficients.Length + secondCoefficients.Length - 1];
            for (int i = 0; i < firstCoefficients.Length; i++)
            {
                for (int j = 0; j < secondCoefficients.Length; j++)
                {
                    resultCoefficients[i + j] += firstCoefficients[i] * secondCoefficients[j];
                }
            }
            return new Polynomial(resultCoefficients);
        }

        public static bool operator ==(Polynomial firstPolynomial, Polynomial secondPolynomial)
        {
            if (System.Object.ReferenceEquals(firstPolynomial, secondPolynomial))
            {
                return true;
            }

            if ((firstPolynomial == null) || (secondPolynomial == null))
            {
                return false;
            }
            return firstPolynomial.Equals(secondPolynomial);
        }

        public static bool operator !=(Polynomial firstPolynomial, Polynomial secondPolynomial)
        {
            return !(firstPolynomial == secondPolynomial);
        }

        public bool Equals(Polynomial other)
        {
            if (other == null)
                return false;

            if (object.ReferenceEquals(this, other))
                return true;

            if (this.GetType() != other.GetType())
                return false;

            if (this._coefficients.Length != other._coefficients.Length)
                return false;

            var comparer = StructuralComparisons.StructuralEqualityComparer;
            return comparer.Equals(this._coefficients, other._coefficients);
        }

        public double GetPolynomialValue(double x)
        {
            double result = 0;
            for (int i = 0; i < _coefficients.Length; i++)
            {
                result += Math.Pow(x, i) * _coefficients[i];
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Polynomial;
            if (other == null)
                return false;
            return Equals(other);
        }

        public override int GetHashCode()
        {
            StringBuilder result = new StringBuilder(string.Empty);
            foreach (double coef in _coefficients)
            {
                result.Append(coef.ToString());
            }
            return result.ToString().GetHashCode();
        }

        private static Polynomial AddOrSub(Polynomial firstPolynomial, Polynomial secondPolynomial, Func<double, double, double> operation)
        {
            double[] firstCoefficients = firstPolynomial._coefficients;
            double[] secondCoefficients = secondPolynomial._coefficients;

            EqualizeCoefficientsLength(firstCoefficients, secondCoefficients);
            int degree = firstCoefficients.Length;

            double[] resultCoefficients = new double[degree];
            bool isZero = false;
            for (int i = 0; i < degree; i++)
            {
                isZero = false;
                resultCoefficients[i] = operation(firstCoefficients[i], secondCoefficients[i]);
                if (resultCoefficients[i] == 0)
                    isZero = true;
            }

            if (isZero)
            {
                int zeroCount = 0;
                for (int i = resultCoefficients.Length - 1; i < -1; i--)
                {
                    if (resultCoefficients[i] == 0)
                        zeroCount++;
                    else
                        break;
                }
                Array.Resize(ref resultCoefficients, resultCoefficients.Length - zeroCount);
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
