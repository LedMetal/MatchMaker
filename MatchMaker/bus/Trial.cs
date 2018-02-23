using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.bus
{
    public class Trial
    {
        List<SubTrial> subTrialsList;
        double trialAverageSimilarity, trialStandardDeviation;

        public Trial()
        {
            this.subTrialsList = new List<SubTrial>();
            this.trialAverageSimilarity = 0.0d;
            this.trialStandardDeviation = 0.0d;
        }

        public Trial(List<SubTrial> subTrialsList, double trialAverageSimilarity, double trialStandardDeviation)
        {
            this.subTrialsList = subTrialsList;
            this.trialStandardDeviation = trialAverageSimilarity;
            this.trialStandardDeviation = trialStandardDeviation;
        }

        public void CalculateTrialAverageSimilarity()
        {
            this.trialAverageSimilarity = this.subTrialsList.Average(x => x.SubTrialAverageSimilarity);
        }

        public void CalculateTrialStandardDeviation()
        {
            int count = this.subTrialsList.Count();

            // Sum of (x - average)^2
            double sum = this.subTrialsList.Sum(x => (Math.Abs(x.SubTrialAverageSimilarity - this.trialAverageSimilarity)) * (Math.Abs(x.SubTrialAverageSimilarity - this.trialAverageSimilarity)));

            // Standard Deviation equals square root of (Sum divided by count)
            this.trialStandardDeviation = Math.Sqrt(sum / count);
        }


        /*-------------------------------------------------------------------------------------------------------------*/

        public List<SubTrial> SubTrialsList { get => subTrialsList; set => subTrialsList = value; }
        public double TrialAverageSimilarity { get => trialAverageSimilarity; set => trialAverageSimilarity = value; }
        public double TrialStandardDeviation { get => trialStandardDeviation; set => trialStandardDeviation = value; }
    }
}
