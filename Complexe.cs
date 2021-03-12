using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetInfoGit
{
    public class Complexe
    {
        //Champs
        private double P_réelle;
        private double P_imaginaire;

        public Complexe(double P_réelle, double P_imaginaire)
        {
            this.P_réelle = P_réelle;
            this.P_imaginaire = P_imaginaire;
        }

        public double Réelle
        {
            get { return this.P_réelle; }
            set
            {
                this.P_réelle = value;
            }
        }

        public double Imaginaire
        {
            get { return this.P_imaginaire; }
            set
            {
                this.P_imaginaire = value;

            }
        }

        public static Complexe operator *(Complexe c1, Complexe c2)
        {
            return (new Complexe(((c1.Réelle * c2.Réelle) - (c1.Imaginaire * c2.Imaginaire)), ((c1.Réelle * c2.Imaginaire) + (c2.Réelle * c1.Imaginaire))));
        }

        public static Complexe operator +(Complexe c1, Complexe c2)
        {
            return (new Complexe(c1.Réelle + c2.Réelle, c1.Imaginaire + c2.Imaginaire));
        }

        public double Module()
        {
            return (Math.Sqrt(Math.Pow(this.P_réelle, 2) + Math.Pow(this.Imaginaire, 2)));
        }

        public string Tostring()
        {
            return ("Complexe = " + this.P_réelle + " + i*" + this.P_imaginaire);
        }
    }
}

