// AForge Machine Learning Library
// AForge.NET framework
//
// Copyright � Andrew Kirillov, 2007
// andrew.kirillov@gmail.com
//

namespace AForge.MachineLearning
{
    using System;

    /// <summary>
    /// Boltzmann distribution exploration policy
    /// </summary>
    /// 
    /// <remarks></remarks>
    /// 
    public class BoltzmannDistributionExploration : IExplorationPolicy
    {
        // termperature parameter of Boltzmann distribution
        private double temperature;

        // random number generator
        private Random rand = new Random( (int) DateTime.Now.Ticks );

        /// <summary>
        /// Termperature parameter of Boltzmann distribution
        /// </summary>
        /// 
        public double Temperature
        {
            get { return temperature; }
            set { temperature = Math.Max( 0, value ); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoltzmannDistributionExploration"/> class
        /// </summary>
        /// 
        /// <param name="temperature">Termperature parameter of Boltzmann distribution</param>
        /// 
        public BoltzmannDistributionExploration( double temperature )
        {
            Temperature = temperature;
        }

        /// <summary>
        /// Choose an action
        /// </summary>
        /// 
        /// <param name="actionEstimates">Action estimates</param>
        /// 
        /// <returns>Returns the next action</returns>
        /// 
        /// <remarks>The method chooses an action depending on the provided estimates. The
        /// estimates can be any sort of estimate, which values usefulness of the action
        /// (expected summary reward, discounted reward, etc).</remarks>
        /// 
        public int ChooseAction( double[] actionEstimates )
        {
            // actions count
            int actionsCount = actionEstimates.Length;
            // action probabilities
            double[] actionProbabilities = new double[actionsCount];
            // actions sum
            double sum = 0, probailitiesSum = 0;

            for ( int i = 0; i < actionsCount; i++ )
            {
                actionProbabilities[i] = Math.Exp( actionEstimates[i] / temperature );
                sum += Math.Exp( actionEstimates[i] / ( temperature * temperature ) );
            }
            for ( int i = 0; i < actionsCount; i++ )
            {
                actionProbabilities[i] /= sum;
                probailitiesSum += actionProbabilities[i];
            }
            sum = 0;

            // get random number, which determines which action to choose
            double actionRandomNumber = rand.Next( );

            for ( int i = 0; i < actionsCount; i++ )
            {
                sum += actionProbabilities[i] / probailitiesSum;
                if ( actionRandomNumber <= sum )
                    return i;
            }

            return actionsCount - 1;
        }
    }
}
