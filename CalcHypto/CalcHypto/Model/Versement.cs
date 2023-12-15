using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalcHypto.Model
{
    public class Versement
    {
        public int mois { get;private set; }
        public float paiement { get;private set; }
        public float capital { get;private set; }
        public float interet { get;private set; }
        public float balance { get;private set; }

        public Versement(int mois, float paiement, float capital, float interet, float balance)
        {
            this.mois = mois;
            this.paiement = paiement;
            this.capital = capital;
            this.interet = interet;
            this.balance = balance;
        }
    }
}
