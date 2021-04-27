using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetInfoGit
{
    public class QRcode
    {
        //Champs
        private int version;
        private string txt;
        private string correction;
        private string mode;
        private string indiceNbChar;
        private List<byte> code;

        public QRcode (string txt,int version)
        {
            //On utilise un itérateur pour bien placer les bytes du code
            int iterateur = 0;
            // on initialise le mode alphanumérique
            this.mode = "0010";
            //on l'implémente au code
            List<byte> code = new List<byte>();
            for(int i=0;i<4;i++)
            {
                code[i] = Convert.ToByte(this.mode[i]);
                iterateur++;
            }
            // on initialise l'indice du nombre de caractère de notre texte et on l'implémente au code en prenant en compte le fait qu'il fasse 9 caractères.
            this.indiceNbChar = Convert.ToString(txt.Length,2);
            if (this.indiceNbChar.Length<9)
            {
                int différence = 9 - this.indiceNbChar.Length;
                for(int i=0;i<différence;i++)
                {
                    code[iterateur] = 0;
                    iterateur++;
                }
            }
            for(int i=0;i<this.indiceNbChar.Length;i++)
            {
                code[iterateur] = Convert.ToByte(this.indiceNbChar[i]);
            }
            string txtMaj = txt.ToUpper();
            for (int i = 0; i < txtMaj.Length - 1; i=i+2)
            {
                char a = txtMaj[i];
                char b = txtMaj[i + 1];
                int valeur = ConvertisseurASCII(a, b);
                
            }
           
        }

        private int ConvertisseurASCII(char a, char b)
        {
            int somme = 0;
            int valA = 0;
            int valB = 0;
            if (a == 'A')
            {
                valA = 45 * 10;

            }
            if (b == 'A')
            {
                valB = 10;

            }
            if (a == 'B')
            {
                valA = 45 * 11;

            }
            if (b == 'B')
            {

                valB = 11;
            }
            if (a == 'C')
            {
                valA = 45 * 12;

            }
            if (b == 'C')
            {

                valB = 12;
            }
            if (a == 'D')
            {
                valA = 45 * 13;

            }
            if (b == 'D')
            {

                valB = 13;
            }
            if (a == 'E')
            {
                valA = 45 * 14;

            }
            if (b == 'E')
            {

                valB = 14;
            }
            if (a == 'F')
            {
                valA = 45 * 15;

            }
            if (b == 'F')
            {

                valB = 15;
            }
            if (a == 'G')
            {
                valA = 45 * 16;

            }
            if (b == 'G')
            {

                valB = 16;
            }
            if (a == 'H')
            {
                valA = 45 * 17;

            }
            if (b == 'H')
            {

                valB = 17;
            }
            if (a == 'I')
            {
                valA = 45 * 18;
            }
            if (b == 'I')
            {

                valB = 18;
            }
            if (a == 'J')
            {
                valA = 45 * 19;

            }
            if (b == 'J')
            {

                valB = 19;
            }
            if (a == 'K')
            {
                valA = 45 * 20;

            }
            if (b == 'K')
            {

                valB = 20;
            }
            if (a == 'L')
            {
                valA = 45 * 21;

            }
            if (b == 'L')
            {

                valB = 21;
            }
            if (a == 'M')
            {
                valA = 45 * 22;

            }
            if (b == 'M')
            {

                valB = 22;
            }
            if (a == 'N')
            {
                valA = 45 * 23;

            }
            if (b == 'N')
            {

                valB = 23;
            }
            if (a == 'O')
            {
                valA = 45 * 24;

            }
            if (b == 'O')
            {

                valB = 24;
            }
            if (a == 'P')
            {
                valA = 45 * 25;

            }
            if (b == 'P')
            {

                valB = 25;
            }
            if (a == 'Q')
            {
                valA = 45 * 26;

            }
            if (b == 'Q')
            {

                valB = 26;
            }
            if (a == 'R')
            {
                valA = 45 * 27;
            }
            if (b == 'R')
            {

                valB = 27;
            }
            if (a == 'S')
            {
                valA = 45 * 28;

            }
            if (b == 'S')
            {

                valB = 28;
            }
            if (a == 'T')
            {
                valA = 45 * 29;

            }
            if (b == 'T')
            {

                valB = 29;
            }
            if (a == 'U')
            {
                valA = 45 * 30;

            }
            if (b == 'U')
            {

                valB = 30;
            }
            if (a == 'V')
            {
                valA = 45 * 31;

            }
            if (b == 'V')
            {

                valB = 31;
            }
            if (a == 'W')
            {
                valA = 45 * 32;

            }
            if (b == 'W')
            {

                valB = 32;
            }
            if (a == 'X')
            {
                valA = 45 * 33;

            }
            if (b == 'X')
            {

                valB = 33;
            }
            if (a == 'Y')
            {
                valA = 45 * 34;

            }
            if (b == 'Y')
            {

                valB = 34;
            }
            if (a == 'Z')
            {
                valA = 45 * 35;

            }
            if (b == 'Z')
            {

                valB = 35;
            }
            if (a == ' ')
            {
                valA = 45 * 36;

            }
            if (b == ' ')
            {

                valB = 36;
            }

            somme = valB + valA;
            return somme;
        }
    }
}
