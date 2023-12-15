using CalcHypto.DAL;
using CalcHypto.Model;
using PartageDepense.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace CalcHypto.ViewModel
{
    internal class SimulationVM : INotifyPropertyChanged
    {

        #region declaration
        //erreur

        private string _messageErreur;

        public string MessageErreur
        {
            get { return _messageErreur; }
            set
            {
                _messageErreur = value;
                ValeurChangee("MessageErreur");
            }
        }

        private Visibility _visibiliteErreur;

        public Visibility VisibiliteErreur
        {
            get { return _visibiliteErreur; }
            set
            {
                _visibiliteErreur = value;
                ValeurChangee("VisibiliteErreur");
            }
        }



        private List<Simulation> _simulations;

        public List<Simulation> Simulations
        {
            get
            {
                return new List<Simulation>(_simulations);
            }
            set
            {
                ValeurChangee("Simulations");
                _simulations = value;
            }
        }

        private Visibility _visAjouterModifier;
        public Visibility VisAjouterModifier
        {
            get
            {
                return _visAjouterModifier;
            }
            set
            {
                _visAjouterModifier = value;
                ValeurChangee("VisAjouterModifier");
            }
        }


        private Visibility _visSupprimerAfficher;
        public Visibility VisSupprimerAfficher
        {
            get
            {
                return _visSupprimerAfficher;
            }
            set
            {
                _visSupprimerAfficher = value;
                ValeurChangee("VisSupprimerAfficher");
            }
        }


        private bool _modifierState;

        public bool ModifierState
        {
            get
            {
                return _modifierState;
            }

            set
            {
                _modifierState = value;
                ValeurChangee("ModifierState");
            }

        }


        private bool _supprimerState;

        public bool SupprimerState
        {
            get
            {
                return _supprimerState;
            }

            set
            {
                _supprimerState = value;
                ValeurChangee("SupprimerState");
            }

        }

        private string _editMode;

        public string EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                ValeurChangee("EditMode");
            }
        }
        private Simulation _simulationSelectioner;

        public Simulation SimulationSelectioner
        {
            get
            {
                return _simulationSelectioner;
            }
            set
            {

                _simulationSelectioner = value;
                if (_simulationSelectioner != null)
                {
                    SelectionerSimulation(_simulationSelectioner);
                    AfficherSetup();
                    ValeurChangee("SimulationSelectioner");
                }

            }
        }

        private string _nom, _prenom, _description, _frequence;
        private float _montantFinance, _TauxInteretAnuelle;
        private int _periodeAmortissement;

        public string Nom
        {
            get
            {
                return _nom;
            }
            set
            {
                _nom = value;
                ValeurChangee("Nom");
            }
        }

        public string Prenom
        {
            get
            {
                return _prenom;
            }
            set
            {
                _prenom = value;
                ValeurChangee("Prenom");
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                ValeurChangee("Description");
            }
        }

        public string Frequence
        {
            get
            {
                if (_frequence != null)
                {
                    if (_frequence.Contains(':'))
                    {
                        string trueFrequence = _frequence.Split(':')[1].Trim();
                        return trueFrequence;
                    }

                }

                return _frequence;
            }
            set
            {
                _frequence = value;
                ValeurChangee("Frequence");
            }
        }

        public float MontantFinance
        {
            get
            {
                return _montantFinance;
            }
            set
            {
                _montantFinance = value;
                ValeurChangee("MontantFinance");
            }
        }
        public float TauxInteretAnuelle
        {
            get
            {
                return _TauxInteretAnuelle;
            }
            set
            {
                _TauxInteretAnuelle = value;
                ValeurChangee("TauxInteretAnuelle");
            }
        }

        public int PeriodeAmortissement
        {
            get
            {
                return _periodeAmortissement;
            }
            set
            {
                _periodeAmortissement = value;
                ValeurChangee("PeriodeAmortissement");
            }
        }

        #endregion

        #region Commandes

        //Setup AjouterSimulation
        private ICommand _AjouterSimulationSetup;

        public ICommand AjouterSimulationSetup
        {
            get { return _AjouterSimulationSetup; }
            private set { _AjouterSimulationSetup = value; }
        }
        private void AjouterSimulationSetup_Execute(object parSelect)
        {
            EditMode = "Ajouter";
            AjouterSetup();
        }
        private bool AjouterSimulationSetup_CanExecute(object param)
        {
            return true;
        }

        //AjouterSimulation

        private ICommand _AjouterSimulation;

        public ICommand AjouterSimulation
        {
            get { return _AjouterSimulation; }
            private set { _AjouterSimulation = value; }
        }
        private void AjouterSimulation_Execute(object parSelect)
        {
            Dal dal = new Dal();


            if (IsSimulationFull() && IsChampsConvenable())
            {
                Simulation SimulationAsauvgarder = new Simulation(Nom, Prenom, Description, MontantFinance, TauxInteretAnuelle, PeriodeAmortissement, Frequence);
                if (EditMode == "Modifier")
                {
                    SimulationAsauvgarder.uniqueId = SimulationSelectioner.uniqueId;
                    _simulations.Remove(SimulationSelectioner);
                }
                try
                {
                    Simulation vraiSimulation = dal.SimulationFact.SaveSimulation(SimulationAsauvgarder);
                    _simulations.Add(vraiSimulation);
                    ValeurChangee("Simulations");
                    SimulationSelectioner = vraiSimulation;
                }
                catch (Exception e)
                {

                    SendErrorAsync(e.Message);
                }
            }
        }
        private bool AjouterSimulation_CanExecute(object param)
        {
            return true;
        }


        //Setup ModifierSimulation

        private ICommand _ModifierSimulationSetup;

        public ICommand ModifierSimulationSetup
        {
            get { return _ModifierSimulationSetup; }
            private set { _ModifierSimulationSetup = value; }
        }
        private void ModifierSimulationSetup_Execute(object parSelect)
        {
            EditMode = "Modifier";
            ModificationSetup();
        }
        private bool ModifierSimulationSetup_CanExecute(object param)
        {
            return true;
        }



        //supprimer Simulation

        private ICommand _SupprimerSimulation;

        public ICommand SupprimerSimulation
        {
            get { return _SupprimerSimulation; }
            private set { _SupprimerSimulation = value; }
        }
        private void SupprimerSimulation_Execute(object parSelect)
        {
            Dal dal = new Dal();

            try
            {
                dal.SimulationFact.DeleteSimulation(SimulationSelectioner);

                _simulations.Remove(SimulationSelectioner);
                ValeurChangee("Simulations");
                ClearAffichage();
            }
            catch (Exception e)
            {
                SendErrorAsync(e.Message);
            }




        }
        private bool SupprimerSimulation_CanExecute(object param)
        {
            return true;
        }



        #endregion

        #region Fonction

        private void AjouterSetup()
        {
            VisAjouterModifier = Visibility.Visible;
            VisSupprimerAfficher = Visibility.Collapsed;
            ModifierState = false;
            SupprimerState = false;
            ClearAffichage();
        }
        private void ModificationSetup()
        {
            VisAjouterModifier = Visibility.Visible;
            VisSupprimerAfficher = Visibility.Collapsed;
            ModifierState = false;
            SupprimerState = false;
        }
        private void AfficherSetup()
        {
            EditMode = string.Empty;
            VisSupprimerAfficher = Visibility.Visible;
            VisAjouterModifier = Visibility.Collapsed;
        }

        private void ClearAffichage()
        {
            Nom = string.Empty;
            Prenom = string.Empty;
            Description = string.Empty;
            MontantFinance = 0;
            TauxInteretAnuelle = 0;
            PeriodeAmortissement = 0;
            Frequence = "Mensuelle";
        }

        private void SendErrorAsync(string Error)
        {
            MessageErreur = "⚠ " + Error + " ⚠";
            Thread thread = new Thread(new ThreadStart(SendError));
            thread.Start();

        }

        private void SendError()
        {
            VisibiliteErreur = Visibility.Visible;
            Thread.Sleep(5000);
            VisibiliteErreur = Visibility.Collapsed;
        }

        private bool IsSimulationFull()
        {
            if (Nom != string.Empty && Prenom != string.Empty && Description != string.Empty && Frequence != string.Empty && MontantFinance != 0 && PeriodeAmortissement != 0 && TauxInteretAnuelle != 0)
            {
                return true;
            }
            else
            {
                SendErrorAsync("Attention Champs incomplet");
                return false;
            }
        }

        private bool IsChampsConvenable()
        {
            bool isConvenable = true;
            string Message = "Erreur dans les Champs: ";
            if (PeriodeAmortissement < 0 || PeriodeAmortissement > 30)
            {
                Message = Message + "la periode d'amortissement dois ètre entre 0 et 30, ";
                isConvenable = false;
            }

            if (TauxInteretAnuelle > 1 || TauxInteretAnuelle < 0)
            {
                Message = Message + "Le taux dois être formater avec des Points ... Exemple: 0.04 = 4%, ";
                isConvenable = false;
            }

            if (MontantFinance < 0)
            {
                Message = Message + "Le Montant dois être positif !, ";
                isConvenable = false;
            }

            if (!isConvenable)
                SendErrorAsync(Message);

            return isConvenable;
        }

        private void SelectionerSimulation(Simulation simulationSelectioner)
        {
            Nom = simulationSelectioner.nom;
            Prenom = simulationSelectioner.prenom;
            Description = SimulationSelectioner.description;
            MontantFinance = simulationSelectioner.montantFinance;
            TauxInteretAnuelle = simulationSelectioner.tauxInteret;
            PeriodeAmortissement = simulationSelectioner.periodeAmortissement;
            Frequence = simulationSelectioner.frequenceVersement;
            ModifierState = true;
            SupprimerState = true;
        }


        #endregion
        

        public SimulationVM()
        {
            Dal dal = new Dal();
            try
            {
                _simulations = dal.SimulationFact.GetAllSimulation();
            }
            catch (Exception e)
            {
                _simulations = new List<Simulation>();
                SendErrorAsync(e.Message);
            }

            ValeurChangee("Simulations");


            _AjouterSimulation = new CommandeRelais(AjouterSimulation_Execute, AjouterSimulation_CanExecute);
            _AjouterSimulationSetup = new CommandeRelais(AjouterSimulationSetup_Execute, AjouterSimulationSetup_CanExecute);
            _ModifierSimulationSetup = new CommandeRelais(ModifierSimulationSetup_Execute, ModifierSimulationSetup_CanExecute);
            _SupprimerSimulation = new CommandeRelais(SupprimerSimulation_Execute, SupprimerSimulation_CanExecute);
            AfficherSetup();
            _visibiliteErreur = Visibility.Collapsed;
            ValeurChangee("VisibiliteErreur");

        }



        #region Notification

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void ValeurChangee(string nomPropriete)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(nomPropriete));
            }
        }

        #endregion



    }
}
