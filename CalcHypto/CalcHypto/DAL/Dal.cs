using CalcHypto.DAL.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcHypto.DAL
{
    internal class Dal
    {
        private SimulationFactory? _simulationFact = null;
        

        public SimulationFactory SimulationFact
        {
            get
            {
                if (_simulationFact == null)
                {
                    _simulationFact = new SimulationFactory();
                }

                return _simulationFact;
            }
        }
    }
}
