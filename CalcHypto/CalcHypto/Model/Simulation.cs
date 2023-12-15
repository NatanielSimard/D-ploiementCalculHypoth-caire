using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CalcHypto.Model
{
    [XmlRoot(ElementName = "Simulation")]
    public class Simulation
    {
        [XmlElement(ElementName = "nom")]
        public string nom { get; set; }

        [XmlElement(ElementName = "prenom")]
        public string prenom { get; set; }

        [XmlElement(ElementName = "description")]
        public string description { get; set; }

        [XmlElement(ElementName = "montantFinance")]

        public float montantFinance { get; set; }

        [XmlElement(ElementName = "tauxInteret")]

        public float tauxInteret { get; set; }

        [XmlElement(ElementName = "periodeAmoortissement")]

        public int periodeAmortissement { get; set; }

        [XmlElement(ElementName = "frequenceVersement")]

        public string frequenceVersement { get; set; }

        [XmlElement(ElementName = "uniqueId")]

        public int uniqueId { get; set; }

        public Simulation(string nom, string prenom, string description, float montantFinance, float tauxInteret, int periodeAmortissement, string frequenceVersement)
        {
            this.nom = nom;
            this.prenom = prenom;
            this.description = description;
            this.montantFinance = montantFinance;
            this.tauxInteret = tauxInteret;
            this.periodeAmortissement = periodeAmortissement;
            this.frequenceVersement = frequenceVersement;
            uniqueId = (int)DateTime.Now.Ticks;
        }

        public Simulation()
        { }

        public bool Equal(Simulation simulation)
        {
            if (simulation.uniqueId == this.uniqueId)
            {
                return true;
            }
            return false;
        }

        public float GetTotalCapital()
        {
            return this.montantFinance;
        }

        public float getTotalInteret()
        {
            float InteretTotal = 0;
            for (int i = 0; i < periodeAmortissement; i++)
            {
                InteretTotal = InteretTotal + GetTotalCapital() * tauxInteret;
            }
            return InteretTotal;
        }

        public float getTotal()
        {
            return getTotalInteret() + GetTotalCapital();
        }

        private int GetNombreDePaiementParAnné()
        {
            switch (frequenceVersement.ToUpper())
            {

                case "MOIS":
                    return 12;

                case "SEMAINE":
                    return 52;

                case "DEUX_SEMAINE":
                    return 26;

                default:
                    frequenceVersement = "MOIS";
                    return 12;
            }
        }

        //utiliser pour permettre de convertir le nombre de versement en versement par mois
        private int GetDiviseurDeMois()
        {
            switch (frequenceVersement.ToUpper())
            {

                case "MOIS":
                    return 1;

                case "SEMAINE":
                    return 4;

                case "DEUX_SEMAINE":
                    return 2;

                default:
                    frequenceVersement = "MOIS";
                    return 12;
            }
        }

        private int GetNombreDeVersementTotal()
        {
            return GetNombreDePaiementParAnné() * periodeAmortissement;
        }

        private float GetTauxPeriode()
        {
            return tauxInteret / GetNombreDePaiementParAnné();
        }
        private float GetPaiement()
        {
            float part1 = montantFinance * GetTauxPeriode();
            double part2 = 1 - Math.Pow((1 + GetTauxPeriode()), (-1 * GetNombreDeVersementTotal()));
            double result = part1 / part2;
            return (float)result;
        }

        public List<Versement> lesVersements()
        {
            float balance = montantFinance;
            List<Versement> lesVersements = new List<Versement>();
            for (int i = 1; i <= GetNombreDeVersementTotal() / GetDiviseurDeMois(); i++)
            {
                float paiement = 0;
                float interet = 0;
                float capital = 0;

                for (int j = 0; j < GetDiviseurDeMois(); j++)
                {
                    float paiementTemp = 0;
                    float interetTemp = 0;
                    float capitalTemp = 0;

                    paiementTemp += GetPaiement();
                    interetTemp += balance * GetTauxPeriode();
                    capitalTemp += paiementTemp - interetTemp;

                    paiement += paiementTemp;
                    interet += interetTemp;
                    capital += capitalTemp;

                    balance = balance - capitalTemp;
                }

                if (i == GetNombreDeVersementTotal() / GetDiviseurDeMois())
                    lesVersements.Add(new Versement(i, paiement, capital, interet, 0));
                else
                    lesVersements.Add(new Versement(i, paiement, capital, interet, balance));



            }
            return lesVersements;
        }



    }
}
