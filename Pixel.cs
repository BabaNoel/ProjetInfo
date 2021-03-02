using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetInfoGit
{
    public class Pixel
    {
        #region Attribut
        //Attribut
        public byte rouge;
        public byte bleu;
        public byte vert;
        #endregion

        #region Constructeur
        //Constructeur
        public Pixel(byte rouge, byte vert, byte bleu)
        {
            this.rouge = rouge;
            this.vert = vert;
            this.bleu = bleu;
        }
        #endregion

        #region Propriété
        //Propriété
        public byte Rouge
        {
            get { return rouge; }
            set
            {

                if (0 <= value && value <= 255)
                {
                    rouge = value;
                }
                else
                {
                    Console.WriteLine("Valeur de votre pixel rouge incorrecte");
                }
            }
        }
        public byte Vert
        {
            get { return vert; }
            set
            {

                if (0 <= value && value <= 255)
                {
                    vert = value;
                }
                else
                {
                    Console.WriteLine("Valeur de votre pixel vert incorrecte");
                }
            }
        }
        public byte Bleu
        {
            get { return bleu; }
            set
            {

                if (0 <= value && value <= 255)
                {
                    bleu = value;
                }
                else
                {
                    Console.WriteLine("Valeur de votre pixel bleu incorrecte");
                }

            }
        }
        #endregion;

    }


}
