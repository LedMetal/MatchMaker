using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker.bus
{
    public class SubTrial
    {
        List<Pairing> subTrialPairings;
        double subTrialAverageSimilarity;

        public SubTrial()
        {
            this.subTrialPairings = new List<Pairing>();
            this.subTrialAverageSimilarity = 0.0d;
        }

        public SubTrial(List<Pairing> subTrialPairings, double subTrialaverageSimilarity)
        {
            this.subTrialPairings = subTrialPairings;
            this.subTrialAverageSimilarity = subTrialaverageSimilarity;
        }

        public void CalculateAverageSimilarity()
        {
            this.subTrialAverageSimilarity = Convert.ToDouble(this.subTrialPairings.Average(x => x.SimilarityPercent));
        }


        /*---------------------------------------------------------------------------------------------------------*/

        public List<Pairing> SubTrialPairings { get => subTrialPairings; set => subTrialPairings = value; }
        public double SubTrialAverageSimilarity { get => subTrialAverageSimilarity; set => subTrialAverageSimilarity = value; }
    }
}
