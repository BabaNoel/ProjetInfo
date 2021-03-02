using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace ProjetInfoGit
{
    class MyImage
    {
        #region Attribut
        //Attribut
        string Myfile;
        int taille;
        string type;
        int offset;
        int largeur;
        int hauteur;
        int nombrebitCouleur;
        Pixel[,] Matricepixel;
        #endregion

        #region Constructeur
        public MyImage(string Myfile)
        {
            byte[] myfile = File.ReadAllBytes(Myfile);
            byte[] tab = new byte[4];
            //type d’image 

            if (myfile[0] == 66 && myfile[1] == 77)            // on vérifie le type de l'image aux emplacements 0 et 1 du Header
            {
                this.type = "bmp";
            }

            //taille Offset
            for (int index = 0; index < 4; index++)              // on trouve l'Offset de l'image aux emplacements 10 à 14 du Header ( d'ou le + 10)
            {
                tab[index] = myfile[index + 10];
            }
            this.offset = Convertir_Endian_To_Int(tab);


            //taille du fichier 

            for (int index = 0; index < 4; index++)       // on trouve la taille de l'image aux index 2 à 5 du Header
            {
                tab[index] = myfile[index + 2];
            }
            this.taille = Convertir_Endian_To_Int(tab);


            //largeur et hauteur de l’image            // on trouve la largeur de l'image aux emplacements 18 à 22 du Header ( d'ou le + 18)
            for (int index = 0; index < 4; index++)
            {
                tab[index] = myfile[index + 18];
            }
            this.largeur = Convertir_Endian_To_Int(tab);
            for (int index = 0; index < 4; index++)            // on trouve la hauteur de l'image aux emplacements 22 à 26 du Header ( d'ou le + 22)
            {
                tab[index] = myfile[index + 22];
            }
            this.hauteur = Convertir_Endian_To_Int(tab);

            //nombre de bits par couleur  
            for (int index = 0; index < 2; index++)              // on trouve le nombre de pixel par couleurs de l'image aux emplacements 26 à 28 du Header ( d'ou le + 26)
            {
                tab[index] = myfile[index + 28];
            }
            this.nombrebitCouleur = Convertir_Endian_To_Int(tab);


            // on initialise les index pour la future matrice qui accueillera la nouvelle image sur laquelle on fera les modifications ( filtres, agrandissement, changement de couleurs ...)
            int iColonne = 0;
            int iLigne = 0;

            Matricepixel = new Pixel[hauteur, largeur]; // Affectation de la taille de la matrice
            for (int i = 54; i <= (taille - 3); i += 3) // Lecture des pixels ( 3 bytes un pour chaque couleur RVB (rouge vert bleu )
            {

                if (iColonne == (largeur))                 // on parcourt la matrice en entier (une fois le numéro colonne = largeur on passe a la ligne en dessous)
                {
                    iColonne = 0;
                    iLigne++;
                }
                Pixel data = new Pixel(myfile[i], myfile[i + 1], myfile[i + 2]);            // on organise les infos de l'image reçut en paramètre en les séparant en 3 pour avoir une matrice de pixel
                Matricepixel[iLigne, iColonne] = data;                                            // on remplit notre nouvelle matrice de pixel avec les infos de l'image
                iColonne++;
            }
        }
        #endregion

        #region Propriété
        //Propriété
        public Pixel[,] MatPixel
        {
            get { return this.Matricepixel; }
            set { this.Matricepixel = value; }
        }
        public int Taille
        {
            get { return this.taille; }
            set { this.taille = value; }
        }
        public int Offset
        {
            get { return this.offset; }
        }
        public int Largeur
        {
            get { return this.largeur; }
        }
        public int Hauteur
        {
            get { return this.hauteur; }
        }
        public int NombrebitC
        {
            get { return this.nombrebitCouleur; }
        }
        #endregion

        #region Méthodes
        //Méthodes
        public int Convertir_Endian_To_Int(byte[] tableau)
        {
            int multi;
            int résultat = 0;
            for (int i = 0; i < tableau.Length; i++)
            {
                multi = (int)Math.Pow(256, i);          // utilisation des puissances pour obtenir un multiplicateur que l'on utilisera pour finir la conversion (256^i) (256 valeurs pour 8 bits)
                résultat += tableau[i] * multi;         // multiplication de notre multiplicateur par la valeur du tableau pour finir de convertir notre endian en entier
            }

            return résultat;

        }

        public byte[] Convertir_Int_To_Endian(int Entier)
        {
            byte[] IntToEndian = BitConverter.GetBytes(Entier);
            return IntToEndian;
        }

        public void From_Image_To_File(string Myfile)
        {
            byte[] myfile = File.ReadAllBytes(Myfile);
            string ImageToByte = "ImageToByte.bmp";
            byte[] Var = new byte[myfile.Length];
            byte[] VarTemp = new byte[4];

            //Header
            Var[0] = 66;
            Var[1] = 77;

            VarTemp = Convertir_Int_To_Endian(taille);                // on récupère chacun leur tour les infos de l'image ( taille, hauteur,largeur...) et on les stockes dans une variables temporaire
            for (int i = 2; i <= 5; i++)
            {
                Var[i] = VarTemp[i - 2];                                // on utilise la variable temporaire pour remplir un tableau variable (VAR) que l'on va utiliser pour créer un nouveau fichier
            }

            VarTemp = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                Var[i] = VarTemp[i - 6];
            }

            VarTemp = Convertir_Int_To_Endian(Offset);
            for (int i = 10; i <= 13; i++)
            {
                Var[i] = VarTemp[i - 10];
            }

            //HeaderInfo
            VarTemp = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                Var[i] = VarTemp[i - 14];
            }

            VarTemp = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                Var[i] = VarTemp[i - 18];
            }

            VarTemp = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                Var[i] = VarTemp[i - 22];
            }

            Var[26] = 0;
            Var[27] = 0;

            VarTemp = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                Var[i] = VarTemp[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                Var[i] = 0;
            }

            //Image
            int compteur = 54;
            for (int iLigne = 0; iLigne < Matricepixel.GetLength(0); iLigne++)
            {
                for (int iColonne = 0; iColonne < Matricepixel.GetLength(1); iColonne++)
                {

                    Var[compteur] = Matricepixel[iLigne, iColonne].rouge;           //on attribut chaque couleurs à son emplacement
                    Var[compteur + 1] = Matricepixel[iLigne, iColonne].vert;
                    Var[compteur + 2] = Matricepixel[iLigne, iColonne].bleu;
                    compteur = compteur + 3;                                         // on avance de +3 pour les 3 couleurs pour chaque pixel
                }
            }

            File.WriteAllBytes(ImageToByte, Var);                       //on sauvegarde l'image(sous le nom ImageToByte)
            Process.Start(new ProcessStartInfo(ImageToByte) { UseShellExecute = true });
        }
        /// <summary>
        /// On va agrandir l'image n fois
        /// </summary>
        /// <param name="Myfile"></param>
        /// <returns></returns>
        public void Agrandir(string Myfile, int n)
        {
            byte[] myfile = File.ReadAllBytes(Myfile);
            string Agrandir = "agrandir.bmp";
            byte[] Var = new byte[offset + ((n * largeur) * (n * hauteur)) * 3];      // la taille du tableau change car il contient plus de pixel après un agrandissement
            byte[] VarTemp = new byte[4];

            //Header
            Var[0] = 66;
            Var[1] = 77;

            VarTemp = Convertir_Int_To_Endian(offset + (largeur * hauteur) * n); //la taille de l'image est donc plus grande
            for (int i = 2; i <= 5; i++)
            {
                Var[i] = VarTemp[i - 2];
            }

            VarTemp = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                Var[i] = VarTemp[i - 6];
            }

            VarTemp = Convertir_Int_To_Endian(offset);
            for (int i = 10; i <= 13; i++)
            {
                Var[i] = VarTemp[i - 10];
            }

            //HeaderInfo
            VarTemp = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                Var[i] = VarTemp[i - 14];
            }

            VarTemp = Convertir_Int_To_Endian(largeur * 3);
            for (int i = 18; i <= 21; i++)
            {
                Var[i] = VarTemp[i - 18];
            }

            VarTemp = Convertir_Int_To_Endian(hauteur * 3);
            for (int i = 22; i <= 25; i++)
            {
                Var[i] = VarTemp[i - 22];
            }

            Var[26] = 0;
            Var[27] = 0;

            VarTemp = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                Var[i] = VarTemp[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                Var[i] = 0;
            }

            //Image
            int compteur = 54;
            for (int Ligne = 0; Ligne < Matricepixel.GetLength(0); Ligne++)
            {
                for (int Colonne = 0; Colonne < Matricepixel.GetLength(1); Colonne++)
                {
                    for (int i = 0; i < n; i++)
                    {
                        Var[compteur + 3 * i] = Matricepixel[Ligne, Colonne].rouge;
                        Var[compteur + 1 + 3 * i] = Matricepixel[Ligne, Colonne].vert;
                        Var[compteur + 2 + 3 * i] = Matricepixel[Ligne, Colonne].bleu;
                    }
                    compteur = compteur + 3 * n;
                }
                for (int Colonne = 0; Colonne < Matricepixel.GetLength(1); Colonne++)
                {
                    for (int i = 0; i < n; i++)
                    {
                        Var[compteur + 3 * i] = Matricepixel[Ligne, Colonne].rouge;
                        Var[compteur + 1 + 3 * i] = Matricepixel[Ligne, Colonne].vert;
                        Var[compteur + 2 + 3 * i] = Matricepixel[Ligne, Colonne].bleu;
                    }
                    compteur = compteur + 3 * n;
                }
                for (int Colonne = 0; Colonne < Matricepixel.GetLength(1); Colonne++)
                {
                    for (int i = 0; i < n; i++)
                    {
                        Var[compteur + 3 * i] = Matricepixel[Ligne, Colonne].rouge;
                        Var[compteur + 1 + 3 * i] = Matricepixel[Ligne, Colonne].vert;
                        Var[compteur + 2 + 3 * i] = Matricepixel[Ligne, Colonne].bleu;
                    }
                    compteur = compteur + 3 * n;
                }

            }

            File.WriteAllBytes(Agrandir, Var);
            Process.Start(new ProcessStartInfo(Agrandir) { UseShellExecute = true });
        }

    }

    #endregion

}
