using CalcHypto.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CalcHypto.ViewModel
{
    internal class InfoAvanceVM : INotifyPropertyChanged
    {
        public InfoAvanceVM(Simulation simulationDetaille)
        {
            _lesVersements = new List<Versement>();
            if(simulationDetaille != null)
            {
                _lesVersements = simulationDetaille.lesVersements();
                ValeurChangee("LesVersements");
                InteretTotal = simulationDetaille.getTotalInteret();
                CoutTotal = simulationDetaille.getTotal();
                CapitalTotal = simulationDetaille.GetTotalCapital();
            }
        }

        #region Declarations
        string propertyName;
        private List<Versement> _lesVersements;

        public List<Versement> LesVersements
        {
            get { return new List<Versement>(_lesVersements); }

            set
            {
                _lesVersements = value;
                ValeurChangee("LesVersements");
            }
        }


        private float _capitalTotal;

        public float CapitalTotal
        {
            get { return _capitalTotal; }
            set
            {
                _capitalTotal = value;
                ValeurChangee("CapitalTotal");
            }
        }

        private float _interetTotal;

        public float InteretTotal
        {
            get { return _interetTotal; }
            set
            {
                _interetTotal = value;
                ValeurChangee("CapitalTotal");
            }
        }

        private float _coutTotal;

        public float CoutTotal
        {
            get { return _coutTotal; }
            set
            {
                _coutTotal = value;
                ValeurChangee("CapitalTotal");
            }
        }

        #endregion


        #region Notification
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void ValeurChangee(string nomPropriete)
        {
            if(this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(nomPropriete));
            }
        }
        #endregion

        #region fonctions

        #endregion
    }
}
