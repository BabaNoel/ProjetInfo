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
        private byte[] correction;
        private string mode;
        private string indiceNbChar;
        private string code;
        private string masque;

        public QRcode(string txt, int version)
        {
            this.masque = "111011111000100";
            this.mode = "0010";
            string code;
            //on implémente au code le mode alphanumérique au code
            code = mode;

            // on initialise le texte et l'indice du nombre de caractère de notre texte et on l'implémente au code en prenant en compte le fait qu'il fasse 9 caractères.
            this.txt = txt;
            this.indiceNbChar = Convert.ToString(txt.Length, 2);
            if (this.indiceNbChar.Length < 9)
            {
                int différence = 9 - this.indiceNbChar.Length;
                for (int i = 0; i < différence; i++)
                {
                    code = code + '0'; //on rajoute le nombre de zéro manquant devant notre élèment
                }
            }
            code = code + indiceNbChar;
            //On convertit le texte en alphanumérique et on l'implémente 
            string txtMaj = txt.ToUpper();
            for (int i = 0; i < txtMaj.Length; i++)
            {
                string valeurbinaire;
                if (txtMaj.Length % 2 == 1 && i == txtMaj.Length - 1)
                {
                    char a1 = txtMaj[i];
                    int valeur = ConvertisseurASCII('|', a1);
                    valeurbinaire = Convert.ToString(valeur, 2);

                    //on comble les 6 bits avec des zeros
                    if (valeurbinaire.Length < 6)
                    {
                        int différence = 6 - valeurbinaire.Length;
                        for (int j = 0; j < différence; j++)
                        {
                            code = code + '0';
                        }
                    }
                    code = code + valeurbinaire;
                }
                else if (i % 2 == 0)
                {
                    char a = txtMaj[i];
                    char b = txtMaj[i + 1];
                    int valeur = ConvertisseurASCII(a, b);

                    valeurbinaire = Convert.ToString(valeur, 2);

                    //on comble les onze bits avec des zeros
                    if (valeurbinaire.Length < 11)
                    {
                        int différence = 11 - valeurbinaire.Length;
                        for (int j = 0; j < différence; j++)
                        {
                            code = code + '0';
                        }
                    }
                    code = code + valeurbinaire;
                }


            }
            //on ajoute la terminaison
            if (code.Length < 152)
            {
                int difference = 152 - code.Length;
                // pour une différence de 1 à 4, on a ajoute le nombre de zéros manquants
                if (difference <= 4)
                {
                    for (int i = 0; i < difference; i++)
                    {
                        code = code + '0';
                    }
                }
                // pour une différence supérieure, on ajoute les 4 zeros, puis on termine l'octet avec des zeros et si on a toujours pas 152, on rajoute des octets spécifiques
                else if (difference > 4)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        code = code + '0';
                    }
                    int reste = code.Length % 8;
                    for (int i = 0; i < 8 - reste; i++)
                    {
                        code = code + '0';
                    }
                    if (code.Length != 152)
                    {
                        int NbOctetRestant = (152 - code.Length) / 8;
                        int alternance = 0;
                        string fill;
                        for (int i = 0; i < NbOctetRestant; i++)
                        {
                            if (alternance == 0)
                            {
                                fill = "11101100";
                                alternance = 1;
                            }
                            else
                            {
                                fill = "00010001";
                                alternance = 0;
                            }
                            code = code + fill;

                        }

                    }

                }
            }
            this.code = code;
            //on met le code sous la forme d'un tableau d'octet
            byte[] tabOctet = new byte[19];
            int itérateur = 0;
            for (int i = 0; i < code.Length; i += 8)
            {
                string str = "";
                for (int j = i; j < i + 8; j++)
                {
                    str += code[j];
                }
                tabOctet[itérateur] = Convert.ToByte(str, 2);
                itérateur++;
            }

            byte[] correction = ReedSolomonAlgorithm.Encode(tabOctet, 7, ErrorCorrectionCodeType.QRCode);
            foreach (byte valeur in correction)
            {
                string val = Convert.ToString(valeur, 2);
                if (val.Length < 8)
                {
                    int différence = 8 - val.Length;
                    for (int i = 0; i < différence; i++)
                    {
                        code = code + '0';
                    }
                }
                code = code + val;
            }
            this.correction = correction;

        }

        public string Code
        { get { return code; } }

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
            if (a == '|')
            {

                valA = 0;
            }

            somme = valB + valA;
            return somme;
        }


        /// <summary>
        /// créer un objet de type MyImage reprensentant le QRcode
        /// </summary>
        /// <param name="nom"></param>
        public void Dessin(string nom)
        {
            Pixel[,] image = new Pixel[21, 21];


            //tracage des separateurs
            for (int i = 0; i < 7; i++)
            {
                image[i, 0] = new Pixel(0, 0, 0);
                image[0, i] = new Pixel(0, 0, 0);
                image[6, i] = new Pixel(0, 0, 0);
                image[i, 6] = new Pixel(0, 0, 0);
                image[20 - i, 0] = new Pixel(0, 0, 0);
                image[20, i] = new Pixel(0, 0, 0);
                image[14, i] = new Pixel(0, 0, 0);
                image[20 - i, 6] = new Pixel(0, 0, 0);
                image[i, 20] = new Pixel(0, 0, 0);
                image[0, 20 - i] = new Pixel(0, 0, 0);
                image[6, 20 - i] = new Pixel(0, 0, 0);
                image[i, 14] = new Pixel(0, 0, 0);
            }
            for (int i = 0; i < 5; i++)
            {
                image[1 + i, 1] = new Pixel(255, 255, 255);
                image[1, 1 + i] = new Pixel(255, 255, 255);
                image[5, 1 + i] = new Pixel(255, 255, 255);
                image[1 + i, 5] = new Pixel(255, 255, 255);
                image[19 - i, 1] = new Pixel(255, 255, 255);
                image[19, 1 + i] = new Pixel(255, 255, 255);
                image[15, 1 + i] = new Pixel(255, 255, 255);
                image[19 - i, 5] = new Pixel(255, 255, 255);
                image[1 + i, 19] = new Pixel(255, 255, 255);
                image[1, 19 - i] = new Pixel(255, 255, 255);
                image[5, 19 - i] = new Pixel(255, 255, 255);
                image[1 + i, 15] = new Pixel(255, 255, 255);
            }
            for (int i = 0; i < 3; i++)
            {
                image[2 + i, 2] = new Pixel(0, 0, 0);
                image[2, 2 + i] = new Pixel(0, 0, 0);
                image[4, 2 + i] = new Pixel(0, 0, 0);
                image[2 + i, 4] = new Pixel(0, 0, 0);
                image[18 - i, 2] = new Pixel(0, 0, 0);
                image[18, 2 + i] = new Pixel(0, 0, 0);
                image[16, 2 + i] = new Pixel(0, 0, 0);
                image[18 - i, 4] = new Pixel(0, 0, 0);
                image[2 + i, 18] = new Pixel(0, 0, 0);
                image[2, 18 - i] = new Pixel(0, 0, 0);
                image[4, 18 - i] = new Pixel(0, 0, 0);
                image[2 + i, 16] = new Pixel(0, 0, 0);
            }
            image[3, 3] = new Pixel(0, 0, 0);
            image[17, 3] = new Pixel(0, 0, 0);
            image[3, 17] = new Pixel(0, 0, 0);

            //patern de séparation
            for (int i = 0; i < 8; i++)
            {
                image[7, i] = new Pixel(255, 255, 255);
                image[i, 7] = new Pixel(255, 255, 255);
                image[13, i] = new Pixel(255, 255, 255);
                image[20 - i, 7] = new Pixel(255, 255, 255);
                image[i, 13] = new Pixel(255, 255, 255);
                image[7, 20 - i] = new Pixel(255, 255, 255);
            }

            //motifs de synchronisation
            for (int i = 8; i < 13; i++)
            {
                if (i % 2 == 0)
                {
                    image[i, 6] = new Pixel(0, 0, 0);
                    image[6, i] = new Pixel(0, 0, 0);
                }
                else
                {
                    image[6, i] = new Pixel(255, 255, 255);
                    image[i, 6] = new Pixel(255, 255, 255);
                }
            }
            for (int i = 0; i < 15; i++)
            {
                if (masque[i] == '1')
                {
                    if (i < 6) { image[8, i] = new Pixel(0,0,0); }
                    else if (i < 8) { image[8, 1 + i] = new Pixel(0,0,0); }
                    else if (i == 8) { image[7, 8] = new Pixel(0,0,0); }
                    else { image[14 - i, 8] = new Pixel(0,0,0); };
                    if (i < 7) { image[20 - i, 8] = new Pixel(0,0,0); }
                    else { image[8, 6 + i] = new Pixel(0,0,0); }
                }
                else
                {
                    if (i < 6) { image[8, i] = new Pixel(255,255,255); }
                    else if (i < 8) { image[8, 1 + i] = new Pixel(255,255,255); }
                    else if (i == 8) { image[7, 8] = new Pixel(255,255,255); }
                    else { image[14 - i, 8] = new Pixel(255,255,255); };
                    if (i < 7) { image[20 - i, 8] = new Pixel(255,255,255); }
                    else { image[8, 6 + i] = new Pixel(255,255,255); }
                }
            }



            //noir module
            image[13, 8] = new Pixel(0, 0, 0);



            //ecriture du code
            bool montee = true;
            int index = 0;
            for (int x = 20; x > 0; x -= 2)
            {
                if (x == 6) { x--; }
                if (montee)
                {
                    for (int y = 20; y >= 0; y--)
                    {

                        if (image[y, x] == null)
                        {
                            if (code[index] == (byte)0) { image[y, x] = new Pixel(255,255,255); }
                            else { image[y, x] = new Pixel(0,0,0); }
                            index++;
                        }
                        if (image[y, x - 1] == null)
                        {
                            if (code[index] == (byte)0) { image[y, x - 1] = new Pixel(255,255,255); }
                            else { image[y, x - 1] = new Pixel(0,0,0); }
                            index++;
                        }
                    }
                    montee = false;
                }
                else
                {
                    for (int y = 0; y < 21; y++)
                    {
                        if (image[y, x] == null)
                        {
                            if (code[index] == (byte)0) { image[y, x] = new Pixel(255,255,255); }
                            else { image[y, x] = new Pixel(0,0,0); }
                            index++;
                        }
                        if (image[y, x - 1] == null)
                        {
                            if (code[index] == (byte)0) { image[y, x - 1] = new Pixel(255,255,255); }
                            else { image[y, x - 1] = new Pixel(0,0,0); }
                            index++;
                        }
                    }
                    montee = true;
                }

            }
            MyImage QR = new MyImage(21, image);
            QR.Agrandissement(8);





        }
    }
}

