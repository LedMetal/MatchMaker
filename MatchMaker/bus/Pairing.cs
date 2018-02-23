using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.bus
{
    public class Pairing
    {
        double rootNumber, randomNumber;
        double similarityPercent;

        public Pairing()
        {
            this.rootNumber = 0.0d;
            this.randomNumber = 0.0d;
            similarityPercent = 0.0d;
        }

        public Pairing(double number1, double number2)
        {
            this.rootNumber = number1;
            this.randomNumber = number2;
            this.similarityPercent = CalculateSimilarityPercent(number1, number2);
        }

        // Calculate and return the similarity percent value of the pair
        double CalculateSimilarityPercent(double num1, double num2)
        {
            double similarityPercent = 0.0d;

            if (num1 > num2)
            {
                similarityPercent = num2 / Math.Sqrt(num1 * num2);
            }
            else
            {
                similarityPercent = num1 / Math.Sqrt(num1 * num2);
            }

            return similarityPercent;
        }

        public override string ToString()
        {
            return "Root Number: " + this.rootNumber + "\nRandomNumber: " + this.randomNumber + "\nSimilarity Percent: " + this.similarityPercent;
        }


        /*------------------------------------------------------------------------------------------------*/

        public double RootNumber { get => rootNumber; set => rootNumber = value; }
        public double RandomNumber { get => randomNumber; set => randomNumber = value; }
        public double SimilarityPercent { get => similarityPercent; set => similarityPercent = value; }
    }
}
