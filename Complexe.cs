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

        //Constructeur
        public Complexe(double P_réelle, double P_imaginaire)
        {
            this.P_réelle = P_réelle;
            this.P_imaginaire = P_imaginaire;
        }

        //Propriétés
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

        //opérations
        /// <summary>
        /// définit la multiplication pour les complexes
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns>Complexe</returns>
        public static Complexe operator *(Complexe c1, Complexe c2)
        {
            return (new Complexe(((c1.Réelle * c2.Réelle) - (c1.Imaginaire * c2.Imaginaire)), ((c1.Réelle * c2.Imaginaire) + (c2.Réelle * c1.Imaginaire))));
        }

        /// <summary>
        /// définit la somme pour les complexes
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <returns>Complexe</returns>
        public static Complexe operator +(Complexe c1, Complexe c2)
        {
            return (new Complexe(c1.Réelle + c2.Réelle, c1.Imaginaire + c2.Imaginaire));
        }
        

        //Méthodes
        /// <summary>
        /// Donne le module d'un complexe
        /// </summary>
        /// <returns>double</returns>
        public double Module()
        {
            return (Math.Sqrt(Math.Pow(this.P_réelle, 2) + Math.Pow(this.Imaginaire, 2)));
        }

        /// <summary>
        /// Ecrit notre complexe sous sa forme complexe
        /// </summary>
        /// <returns>string</returns>
        public string Tostring()
        {
            return ("Complexe = " + this.P_réelle + " + i*" + this.P_imaginaire);
        }
    }
}