using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace ProjetInfoGit
{
    public class MyImage
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
            this.Myfile = Myfile;
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

        public MyImage(int dimension, Pixel[,] Matricepixel)
        {
            this.type = "bmp";
            this.hauteur = dimension;
            this.largeur = dimension;
            this.taille = dimension * dimension * 3+ 54;
            this.nombrebitCouleur = 24;
            this.offset = 54;
            this.Matricepixel = Matricepixel;

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

        #region TD2
        /// <summary>
        /// convertit des little endian en entier
        /// </summary>
        /// <param name="tableau"></param>
        /// <returns>int</returns>
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

        /// <summary>
        /// Convertit des entiers en little endians
        /// </summary>
        /// <param name="Entier"></param>
        /// <returns>byte[]</returns>
        public byte[] Convertir_Int_To_Endian(int Entier)
        {
            byte[] IntToEndian = BitConverter.GetBytes(Entier);
            return IntToEndian;
        }

        /// <summary>
        /// convertit un tableau de pixel en fichier
        /// </summary>
        /// <param name="name"></param>
        public void From_Image_To_File(string name)
        {
            byte[] fichier = new byte[taille];
            byte[] stock = new byte[4];

            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(this.taille);                // on récupère chacun leur tour les infos de l'image ( taille, hauteur,largeur...) et on les stockes dans une variables temporaire
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];                                // on utilise la variable temporaire pour remplir un tableau variable (VAR) que l'on va utiliser pour créer un nouveau fichier
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(Offset);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }

            stock = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

            //Image
            int compteur = 54;
            for (int iLigne = 0; iLigne < Matricepixel.GetLength(0); iLigne++)
            {
                for (int iColonne = 0; iColonne < Matricepixel.GetLength(1); iColonne++)
                {

                    fichier[compteur] = Matricepixel[iLigne, iColonne].rouge;           //on attribut chaque couleurs à son emplacement
                    fichier[compteur + 1] = Matricepixel[iLigne, iColonne].vert;
                    fichier[compteur + 2] = Matricepixel[iLigne, iColonne].bleu;
                    compteur = compteur + 3;                                         // on avance de +3 pour les 3 couleurs pour chaque pixel
                }
            }

            File.WriteAllBytes(name, fichier);                       //on sauvegarde l'image(sous le nom ImageToByte)
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });
        }


        #endregion

        #region TD3
        /// <summary>
        /// On va agrandir l'image n fois
        /// </summary>
        /// <param name="name"></param>
        /// <param name ="n"></param>
        /// <returns></returns>
        public void Agrandir(string name, int n)
        {

            byte[] fichier = new byte[this.offset + ((n * this.largeur) * (n * this.hauteur)) * 3];      // la taille du tableau change car il contient plus de pixel après un agrandissement
            byte[] stock = new byte[4];

            //Header
            fichier[0] = 66;
            fichier[1] = 77;
            stock = Convertir_Int_To_Endian(this.taille); //la taille de l'image est donc plus grande
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(this.offset);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(this.largeur*n);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }


            stock = Convertir_Int_To_Endian(this.hauteur*n);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(this.nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

           
            //Image
            int compteur = 54;
            for (int Ligne = 0; Ligne < this.Matricepixel.GetLength(0); Ligne++)
            {
                for (int iterateur = 0; iterateur < n; iterateur++)
                {

                    for (int Colonne = 0; Colonne < this.Matricepixel.GetLength(1); Colonne++)
                    {
                        for (int i = 0; i < n; i++)
                        {
                            fichier[compteur + 3 * i] = this.Matricepixel[Ligne, Colonne].rouge;
                            fichier[compteur + 1 + 3 * i] = this.Matricepixel[Ligne, Colonne].vert;
                            fichier[compteur + 2 + 3 * i] = this.Matricepixel[Ligne, Colonne].bleu;
                        }
                        compteur = compteur + 3 * n;
                    }
                }


            }

            File.WriteAllBytes(name, fichier);
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });

        }
       
        /// <summary>
        /// Convertie l'image en dégradé de gris
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void DégradéGris(string name)
        {
           
            byte[] fichier = new byte[taille];
            byte[] stock = new byte[4];

            //on utilise le presque la meme methode que pour la fonction From_Image_To_File seulement la partie image change car on vient modifier la couleur de l'image
            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(taille);
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(Offset);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }

            stock = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

            //Image
            int compteur = 54;
            for (int Ligne = 0; Ligne < Matricepixel.GetLength(0); Ligne++)
            {
                for (int Colonne = 0; Colonne < Matricepixel.GetLength(1); Colonne++)
                {
                    // on prends la moyenne des 3 couleurs et on la donne pour chaque sous-pixels
                    byte gris = Convert.ToByte((Matricepixel[Ligne, Colonne].rouge + Matricepixel[Ligne, Colonne].vert + Matricepixel[Ligne, Colonne].bleu) / 3);  // formule pour un dégradé de gris : (rouge + vert + bleu)/3
                    fichier[compteur] = gris;
                    fichier[compteur + 1] = gris;
                    fichier[compteur + 2] = gris;
                    compteur = compteur + 3;
                }
            }

            File.WriteAllBytes(name, fichier);
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });


        }

        /// <summary>
        /// Convertie l'image en noir et blanc
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void NoirEtBlanc(string name)
        {
            
            byte[] fichier = new byte[this.taille];
            byte[] stock = new byte[4];

            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(taille);
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(Offset);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }

            stock = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

            //Image
            int compteur = 54;
            for (int Ligne = 0; Ligne < Matricepixel.GetLength(0); Ligne++)
            {
                for (int Colonne = 0; Colonne < Matricepixel.GetLength(1); Colonne++)
                {
                    int valeur_moyenne_pixel = (Matricepixel[Ligne, Colonne].rouge + Matricepixel[Ligne, Colonne].vert + Matricepixel[Ligne, Colonne].bleu) / 3;
                    if (valeur_moyenne_pixel <= 128)
                    {
                        //si le pixel est inferieur a 128, alors il est noir donc 0
                        fichier[compteur] = 0;
                        fichier[compteur + 1] = 0;
                        fichier[compteur + 2] = 0;
                        compteur = compteur + 3;
                    }
                    else
                    {
                        //sinon il est blanc donc =255
                        fichier[compteur] = 255;
                        fichier[compteur + 1] = 255;
                        fichier[compteur + 2] = 255;
                        compteur = compteur + 3;
                    }
                }
            }

            File.WriteAllBytes(name, fichier);
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });


        }

        /// <summary>
        /// Convertit l'image en miroir
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public void Miroir(string name, char sens)
        {
            byte[] fichier = new byte[taille];
            byte[] stock = new byte[4];


            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(taille);
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(offset);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }

            stock = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

            //Image
            Pixel[,] Matrice = new Pixel[Matricepixel.GetLength(0), Matricepixel.GetLength(1)];
            if (sens == 'V')// miroir par rapport à une droite verticale
            {
                for (int j = 0; j < this.hauteur; j++)
                {
                    for (int i = 0; i < this.largeur; i++)
                    {
                        Matrice[j, i] = Matricepixel[j, this.largeur - 1 - i];
                    }
                }

            }
            else//miroir par rapport à une droite horinzontale
            {
                for (int i = 0; i < this.largeur; i++)
                {
                    for (int j = 0; j < this.hauteur; j++)
                    {
                        Matrice[j, i] = Matricepixel[this.hauteur - 1 - j, i];
                    }
                }

            }
            int compteur = 54;
            for (int Ligne = 0; Ligne < Matrice.GetLength(0); Ligne++)
            {
                for (int Colonne = 0; Colonne < Matrice.GetLength(1); Colonne++)
                {
                    fichier[compteur] = Matrice[Ligne, Colonne].rouge;
                    fichier[compteur + 1] = Matrice[Ligne, Colonne].vert;
                    fichier[compteur + 2] = Matrice[Ligne, Colonne].bleu;
                    compteur = compteur + 3;
                }
            }



            File.WriteAllBytes(name, fichier);
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });


        }

        /// <summary>
        /// Permet de retourner l'image a 90, 180 ou 270 degrés
        /// </summary>
        /// <param name="Myfile"></param>
        /// <returns></returns>
        public void Rotation(string Myfile)
        {


            byte[] myfile = File.ReadAllBytes(Myfile);
            string Rotation90 = "rotation90.bmp";
            string Rotation180 = "rotation180.bmp";
            string Rotation270 = "rotation270.bmp";
            byte[] fichier = new byte[myfile.Length];
            byte[] stock = new byte[4];

            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(taille);
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(Offset);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }

            stock = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

            //Image
            int compteur = 54;
            int nombreRot = 0;
            Console.WriteLine("Quel type de rotation voulez vous faire (choisir entre 90 180 270)");       //on utilise la meme méthode que précédement 
            nombreRot = Convert.ToInt32(Console.ReadLine());
            if (nombreRot == 90)
            {
                for (int iColonne = Matricepixel.GetLength(1) - 1; iColonne >= 0; iColonne--)   // on va juste changer l'ordre de parcours de notre matrice pour obtenir la rotation suivant l'angale voulu
                {                                                                        // pour 90 degres on parcourt en premier les colonne et on inverse le sens de parcours des colonnes
                    for (int iLigne = 0; iLigne < Matricepixel.GetLength(0); iLigne++)
                    {

                        fichier[compteur] = Matricepixel[iLigne, iColonne].rouge;
                        fichier[compteur + 1] = Matricepixel[iLigne, iColonne].vert;
                        fichier[compteur + 2] = Matricepixel[iLigne, iColonne].bleu;
                        compteur = compteur + 3;
                    }
                }
                File.WriteAllBytes(Rotation90, fichier);
                Process.Start(new ProcessStartInfo(Rotation90) { UseShellExecute = true });
            }
            if (nombreRot == 180)
            {
                for (int iLigne = Matricepixel.GetLength(0) - 1; iLigne >= 0; iLigne--)                        // pour 180 degres on inverse juste  le sens de parcours des lignes
                {
                    for (int iColonne = Matricepixel.GetLength(1) - 1; iColonne >= 0; iColonne--)
                    {

                        fichier[compteur] = Matricepixel[iLigne, iColonne].rouge;
                        fichier[compteur + 1] = Matricepixel[iLigne, iColonne].vert;
                        fichier[compteur + 2] = Matricepixel[iLigne, iColonne].bleu;
                        compteur = compteur + 3;
                    }
                }
                File.WriteAllBytes(Rotation180, fichier);
                Process.Start(new ProcessStartInfo(Rotation180) { UseShellExecute = true });
            }
            if (nombreRot == 270)
            {
                for (int iColonne = 0; iColonne < Matricepixel.GetLength(1); iColonne++)         // pour 270 degres on parcours en premier les colonne et on inverse le sens de parcours des lignes ( inverse de 90)
                {
                    for (int iLigne = Matricepixel.GetLength(0) - 1; iLigne >= 0; iLigne--)
                    {

                        fichier[compteur] = Matricepixel[iLigne, iColonne].rouge;
                        fichier[compteur + 1] = Matricepixel[iLigne, iColonne].vert;
                        fichier[compteur + 2] = Matricepixel[iLigne, iColonne].bleu;
                        compteur = compteur + 3;
                    }
                }
                File.WriteAllBytes(Rotation270, fichier);
                Process.Start(new ProcessStartInfo(Rotation270) { UseShellExecute = true });
            }
        }

        /// <summary>
        /// reduit la taille d'une image
        /// </summary>
        /// <param name="name"></param>
        /// <param name="n"></param>
        public void Reduction(string name, int n)
        {
            if (n > hauteur | n > largeur)
            {
                Console.WriteLine("La réduction demandée est trop importante");
                return;
            }
            this.taille = offset + (largeur / n * hauteur / n) * 3;
            byte[] fichier = new byte[taille];
            byte[] stock = new byte[4];

            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(offset + ((largeur / n) * (hauteur / n)) * 3);
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(offset);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }
            this.largeur = largeur / n;
            stock = Convertir_Int_To_Endian(this.largeur);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }
            this.hauteur = hauteur / n;
            stock = Convertir_Int_To_Endian(this.hauteur);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

            //Image
            int compteur = 54;
            for (int iLigne = 0; iLigne < Matricepixel.GetLength(0); iLigne += n)
            {
                for (int iColonne = 0; iColonne < Matricepixel.GetLength(1); iColonne += n)
                {

                    fichier[compteur] = Matricepixel[iLigne, iColonne].rouge;
                    fichier[compteur + 1] = Matricepixel[iLigne, iColonne].vert;
                    fichier[compteur + 2] = Matricepixel[iLigne, iColonne].bleu;
                    compteur = compteur + 3;

                }
            }
            File.WriteAllBytes(name, fichier);
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });
        }

        #endregion

        #region Innovation
        /// <summary>
        /// Inverse les couleur de l'image
        /// </summary>
        /// <param name="name"></param>
        public void InverserCouleur(string name)
        {
            
            byte[] fichier = new byte[taille];
            byte[] stock = new byte[4];


            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(taille);
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(Offset);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }

            stock = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

            //Image
            int compteur = 54;
            for (int Ligne = 0; Ligne < Matricepixel.GetLength(0); Ligne++)
            {
                for (int Colonne = 0; Colonne < Matricepixel.GetLength(1); Colonne++)
                {

                    fichier[compteur] = Convert.ToByte(255 - Matricepixel[Ligne, Colonne].rouge);
                    fichier[compteur + 1] = Convert.ToByte(255 - Matricepixel[Ligne, Colonne].vert);
                    fichier[compteur + 2] = Convert.ToByte(255 - Matricepixel[Ligne, Colonne].bleu);
                    compteur = compteur + 3;

                }
            }

            File.WriteAllBytes(name, fichier);
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });


        }


        /// <summary>
        /// Permet de régler l'exposition de la photo
        /// </summary>
        /// <param name="Myfile"></param>
        /// <param name="expo"></param>
        /// <returns></returns>
        public void Exposition(string Myfile, int expo)
        {
            byte[] myfile = File.ReadAllBytes(Myfile);
            string Expo = "Exposition.bmp";
            byte[] fichier = new byte[myfile.Length];
            byte[] stock = new byte[4];

            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(taille);  //même méthode que précédemment
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(Offset);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }

            stock = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

            //Image
            int compteur = 54;
            for (int iLigne = 0; iLigne < Matricepixel.GetLength(0); iLigne++)
            {
                for (int iColonne = 0; iColonne < Matricepixel.GetLength(1); iColonne++)
                {
                    int nombreR = Convert.ToInt32(Matricepixel[iLigne, iColonne].rouge);    // on convertie la valeur du pixel rouge en entier
                    nombreR = nombreR + expo;                                            // on lui ajoute une valeur d'exposition
                    if (nombreR > 255)                                        //on gère les execeptions
                    {
                        nombreR = 255;
                    }
                    if (nombreR < 0)
                    {
                        nombreR = 0;
                    }
                    byte PixR = Convert.ToByte(nombreR);            // on convertie notre valeur en byte pour pouvoir remplir la matrice de pixel ( on fais pareil pour les deux autres couleurs )

                    int nombreB = Convert.ToInt32(Matricepixel[iLigne, iColonne].bleu);
                    nombreB = nombreB + expo;
                    if (nombreB > 255)
                    {
                        nombreB = 255;
                    }
                    if (nombreB < 0)
                    {
                        nombreB = 0;
                    }
                    byte PixB = Convert.ToByte(nombreB);

                    int nombreV = Convert.ToInt32(Matricepixel[iLigne, iColonne].vert);
                    nombreV = nombreV + expo;
                    if (nombreV > 255)
                    {
                        nombreV = 255;
                    }
                    if (nombreV < 0)
                    {
                        nombreV = 0;
                    }
                    byte PixV = Convert.ToByte(nombreV);

                    fichier[compteur] = PixR;
                    fichier[compteur + 1] = PixV;
                    fichier[compteur + 2] = PixB;
                    compteur = compteur + 3;
                }
            }

            File.WriteAllBytes(Expo, fichier);
            Process.Start(new ProcessStartInfo(Expo) { UseShellExecute = true });
        }

        /// <summary>
        /// Permet de régler les ombres de la photo
        /// </summary>
        /// <param name="Myfile"></param>
        /// <param name="ombre"></param>
        /// <returns></returns>
        public void Ombre(string Myfile, int ombre)
        {
            byte[] myfile = File.ReadAllBytes(Myfile);
            string Shadow = "Ombre.bmp";
            byte[] fichier = new byte[myfile.Length];
            byte[] stock = new byte[4];

            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(taille);  //même méthode que précédemment 
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(Offset);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }

            stock = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

            //Image
            int compteur = 54;
            for (int iLigne = 0; iLigne < Matricepixel.GetLength(0); iLigne++)
            {
                for (int iColonne = 0; iColonne < Matricepixel.GetLength(1); iColonne++)
                {
                    int nombreR = Convert.ToInt32(Matricepixel[iLigne, iColonne].rouge);
                    if (nombreR < 75)                                        // meme méthode que pour l'exposition sauf qu'ici on ne cible que les couleurs en dessous d'un certain seuil(ici 75 qui se trouve etre la meilleur valeur pour voir une vrai différence d'apres nos différents test )
                    {
                        nombreR = nombreR + ombre;
                        if (nombreR > 255)
                        {
                            nombreR = 255;
                        }
                        if (nombreR < 0)
                        {
                            nombreR = 0;
                        }

                    }
                    byte PixR = Convert.ToByte(nombreR);

                    int nombreB = Convert.ToInt32(Matricepixel[iLigne, iColonne].bleu);
                    if (nombreB < 75)
                    {
                        nombreB = nombreB + ombre;
                        if (nombreB > 255)
                        {
                            nombreB = 255;
                        }
                        if (nombreB < 0)
                        {
                            nombreB = 0;
                        }

                    }
                    byte PixB = Convert.ToByte(nombreB);

                    int nombreV = Convert.ToInt32(Matricepixel[iLigne, iColonne].vert);
                    if (nombreV < 75)
                    {
                        nombreV = nombreV + ombre;
                        if (nombreV > 255)
                        {
                            nombreV = 255;
                        }
                        if (nombreV < 0)
                        {
                            nombreV = 0;
                        }

                    }
                    byte PixV = Convert.ToByte(nombreV);

                    fichier[compteur] = PixR;
                    fichier[compteur + 1] = PixV;
                    fichier[compteur + 2] = PixB;
                    compteur = compteur + 3;
                }
            }

            File.WriteAllBytes(Shadow, fichier);
            Process.Start(new ProcessStartInfo(Shadow) { UseShellExecute = true });
        }

        /// <summary>
        /// Permet de régler les hautes lumière de la photo
        /// </summary>
        /// <param name="Myfile"></param>
        /// /// <param name="HL"></param>
        /// <returns></returns>
        public void HautesLumiere(string Myfile, int HL)
        {
            byte[] myfile = File.ReadAllBytes(Myfile);
            string HighLights = "HautesLumières.bmp";
            byte[] fichier = new byte[myfile.Length];
            byte[] stock = new byte[4];

            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(taille);
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(Offset);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }

            stock = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

            //Image
            int compteur = 54;
            for (int iLigne = 0; iLigne < Matricepixel.GetLength(0); iLigne++)
            {
                for (int iColonne = 0; iColonne < Matricepixel.GetLength(1); iColonne++)
                {
                    int nombreR = Convert.ToInt32(Matricepixel[iLigne, iColonne].rouge);
                    if (nombreR > 150)                                       // on fait l'inverse de la méthode pour les ombres
                    {
                        nombreR = nombreR + HL;
                        if (nombreR > 255)
                        {
                            nombreR = 255;
                        }
                        if (nombreR < 0)
                        {
                            nombreR = 0;
                        }

                    }
                    byte PixR = Convert.ToByte(nombreR);

                    int nombreB = Convert.ToInt32(Matricepixel[iLigne, iColonne].bleu);
                    if (nombreB > 150)
                    {
                        nombreB = nombreB + HL;
                        if (nombreB > 255)
                        {
                            nombreB = 255;
                        }
                        if (nombreB < 0)
                        {
                            nombreB = 0;
                        }

                    }
                    byte PixB = Convert.ToByte(nombreB);

                    int nombreV = Convert.ToInt32(Matricepixel[iLigne, iColonne].vert);
                    if (nombreV > 150)
                    {
                        nombreV = nombreV + HL;
                        if (nombreV > 255)
                        {
                            nombreV = 255;
                        }
                        if (nombreV < 0)
                        {
                            nombreV = 0;
                        }

                    }
                    byte PixV = Convert.ToByte(nombreV);

                    fichier[compteur] = PixR;
                    fichier[compteur + 1] = PixV;
                    fichier[compteur + 2] = PixB;
                    compteur = compteur + 3;
                }
            }

            File.WriteAllBytes(HighLights, fichier);
            Process.Start(new ProcessStartInfo(HighLights) { UseShellExecute = true });
        }

        /// <summary>
        /// Permet de régler la teinte(Hue), la Saturation et la luminance de l'image
        /// </summary>
        /// <param name="Myfile"></param>
        /// <returns></returns>
        public void HSL(string Myfile)
        {
            double Teinte;
            double Saturation;
            double Luminance;
            double TempLumi1;
            double TempLumi2;
            double TeinteFinale;
            double tempR;
            double tempV;
            double tempB;
            double max;
            double TeinteDegré;
            Console.WriteLine("Comment voulez vous changer la teinte de votre rouge");
            Console.WriteLine("Rentrez une valeur entre -100 et 100");
            double tempR2 = Convert.ToDouble(Console.ReadLine());
            tempR2 = tempR2 / 100;
            Console.WriteLine("Comment voulez vous changer la teinte de votre Vert");
            Console.WriteLine("Rentrez une valeur entre -100 et 100");
            double tempV2 = Convert.ToDouble(Console.ReadLine());
            tempV2 = tempV2 / 100;
            Console.WriteLine("Comment voulez vous changer la teinte de votre bleu");
            Console.WriteLine("Rentrez une valeur entre -100 et 100");
            double tempB2 = Convert.ToDouble(Console.ReadLine());
            tempB2 = tempB2 / 100;
            Console.WriteLine("Comment voulez vous changer la saturation de votre image");
            Console.WriteLine("Rentrez une valeur entre -100 et 100");
            double Sat2 = Convert.ToDouble(Console.ReadLine());
            Sat2 = Sat2 / 100;
            Console.WriteLine("Comment voulez vous changer la Luminance de votre image");
            Console.WriteLine("Rentrez une valeur entre -100 et 100");
            double Lum2 = Convert.ToDouble(Console.ReadLine());
            Lum2 = Lum2 / 100;
            byte[] myfile = File.ReadAllBytes(Myfile);
            string HSL = "HSL.bmp";
            byte[] fichier = new byte[myfile.Length];
            byte[] stock = new byte[4];

            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(taille);
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(Offset);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }

            stock = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

            //Image
            int compteur = 54;
            for (int iLigne = 0; iLigne < Matricepixel.GetLength(0); iLigne++)
            {
                for (int iColonne = 0; iColonne < Matricepixel.GetLength(1); iColonne++)
                {
                    

                    double nombreR = Convert.ToInt32((Matricepixel[iLigne, iColonne].rouge));
                    double nombreV = Convert.ToInt32((Matricepixel[iLigne, iColonne].vert));
                    double nombreB = Convert.ToInt32((Matricepixel[iLigne, iColonne].bleu));
                    // Console.WriteLine("Rouge = " + nombreR);
                    //Console.WriteLine("Vert = " + nombreV);
                    //Console.WriteLine("Bleu = " + nombreB);
                    
                    nombreR = nombreR / 255;
                    nombreV = nombreV / 255;
                    nombreB = nombreB / 255;
                    double maxRGB = Math.Max(Math.Max(nombreR, nombreV), nombreB);
                    double minRGB = Math.Min(Math.Min(nombreR, nombreV), nombreB);
                    Luminance = Convert.ToDouble((maxRGB + minRGB) / 2);
                    Luminance = Math.Round(Luminance, 2);
                    Luminance = Luminance + Lum2;
                   // int PourcentageLuminance = Convert.ToInt32(Luminance * 100);

                    Saturation = (maxRGB - minRGB) / (maxRGB + minRGB);
                    Saturation = Math.Round(Saturation, 2);
                    Saturation = Saturation + Sat2;
                    
                    TempLumi1 = Luminance * (1 + Saturation);
                    TempLumi2 = 2 * Luminance - TempLumi1;
                    //Console.WriteLine("Luminance " + Luminance);
                    // Console.WriteLine("Pourcentatge " + PourcentageLuminance);

                    if (Luminance < 0.5)
                    {
                        Saturation = (maxRGB - minRGB) / (maxRGB + minRGB);
                        Saturation = Math.Round(Saturation, 2);
                        Saturation = Saturation + Sat2;
                        
                        //   int PourcentageSaturation = Convert.ToInt32(Saturation * 100);
                        //Console.WriteLine("Saturation " + Saturation);
                        //Console.WriteLine("Pourcentage " + PourcentageSaturation);
                        TempLumi1 = Luminance * (1 + Saturation);
                        TempLumi2 = 2 * Luminance - TempLumi1;
                    }
                    if (Luminance == 0.5)
                    {
                        Saturation = (maxRGB - minRGB) / (maxRGB + minRGB);
                        Saturation = Math.Round(Saturation, 2);
                        Saturation = Saturation + Sat2;
                        
                        // int PourcentageSaturation = Convert.ToInt32(Saturation * 100);
                        //Console.WriteLine("Saturation " + Saturation);
                        //Console.WriteLine("Pourcentage " + PourcentageSaturation);
                        TempLumi1 = (Luminance + Saturation) - (Luminance*Saturation);
                        TempLumi2 = 2 * Luminance - TempLumi1;

                    }
                    if (Luminance > 0.5)
                    {
                        Saturation = (maxRGB - minRGB) / (2- maxRGB - minRGB);
                        Saturation = Math.Round(Saturation, 2);
                        Saturation = Saturation + Sat2;
                        
                        //int PourcentageSaturation = Convert.ToInt32(Saturation * 100);
                        //WriteLine("Saturation " + Saturation);
                        //Console.WriteLine("Pourcentage " + PourcentageSaturation);
                        TempLumi1 = (Luminance + Saturation) - (Luminance * Saturation);
                        TempLumi2 = 2 * Luminance - TempLumi1;
                    }
                    if (nombreR > nombreB && nombreR > nombreV)
                    {
                        max = nombreR;
                        Teinte = Convert.ToDouble((nombreV - nombreB) / (maxRGB - minRGB));
                        Teinte = Math.Round(Teinte, 2);
                        if (Teinte < 0)
                        {
                            TeinteDegré = (Teinte * 60) + 360;
                        }
                        else
                        {
                            TeinteDegré = Teinte * 60;
                        }

                        TeinteDegré = Math.Round(TeinteDegré);
                        //int PourcentageTeinte = Convert.ToInt32(Teinte * 100);
                        //Console.WriteLine("Teinte Degrés entier = " + TeinteDegré);
                        //Console.WriteLine("Teinte " + Teinte);
                        //Console.WriteLine("PourcentageTeinte " + PourcentageTeinte);
                        TeinteFinale = Teinte / 360;
                        tempR = TeinteFinale + 0.333;
                        tempR = tempR + tempR2;
                        tempV = TeinteFinale;
                        tempV = tempV + tempV2;
                        tempB = TeinteFinale - 0.333;
                        tempB = tempB + tempB2;
                        if (tempR < 0)
                        {
                            tempR = tempR + 1;
                        }
                        if (tempB < 0)
                        {
                            tempB = tempB + 1;
                        }
                        if (tempV < 0)
                        {
                            tempV = tempV + 1;
                        }
                        if (tempR > 1)
                        {
                            tempR = tempR - 1;
                        }
                        if (tempB > 1)
                        {
                            tempB = tempB - 1;
                        }
                        if (tempV > 1)
                        {
                            tempV = tempV - 1;
                        }


                        if (6 * tempR < 1)
                        {
                            nombreR = TempLumi2 + (TempLumi1 - TempLumi2) * 6 * tempR;
                        }
                        if (2 * tempR < 1)
                        {
                            nombreR = TempLumi1;
                        }
                        if (3 * tempR < 2)
                        {
                            nombreR = TempLumi2 + (TempLumi1 - TempLumi2) * (0.666-tempR)*6;
                        }
                        if(3 * tempR >2)
                        {
                            nombreR = TempLumi2;
                        }
                    }
                    if (nombreV > nombreR && nombreV > nombreB)
                    {
                        max = nombreV;
                        Teinte = Convert.ToDouble(2 + (nombreB - nombreR) / (maxRGB - minRGB));
                        Teinte = Math.Round(Teinte, 2);
                        if (Teinte < 0)
                        {
                            TeinteDegré = (Teinte * 60) + 360;
                        }
                        else
                        {
                            TeinteDegré = Teinte * 60;
                        }

                        TeinteDegré = Math.Round(TeinteDegré);
                       // int PourcentageTeinte = Convert.ToInt32(Teinte * 100);
                        //Console.WriteLine("Teinte " + Teinte);
                        //Console.WriteLine("PourcentageTeinte " + PourcentageTeinte);
                        TeinteFinale = Teinte / 360;
                        tempR = TeinteFinale + 0.333;
                        tempR = tempR + tempR2;
                        tempV = TeinteFinale;
                        tempV = tempV + tempV2;
                        tempB = TeinteFinale - 0.333;
                        tempB = tempB + tempB2;

                        if (tempR < 0)
                        {
                            tempR = tempR + 1;
                        }
                        if (tempB < 0)
                        {
                            tempB = tempB + 1;
                        }
                        if (tempV < 0)
                        {
                            tempV = tempV + 1;
                        }
                        if (tempR > 1)
                        {
                            tempR = tempR - 1;
                        }
                        if (tempB > 1)
                        {
                            tempB = tempB - 1;
                        }
                        if (tempV > 1)
                        {
                            tempV = tempV - 1;
                        }


                        if (6 * tempV < 1)
                        {
                            nombreV = TempLumi2 + (TempLumi1 - TempLumi2) * 6 * tempV;
                        }
                        if (2 * tempV < 1)
                        {
                            nombreV = TempLumi1;
                        }
                        if (3 * tempV < 2)
                        {
                            nombreV = TempLumi2 + (TempLumi1 - TempLumi2) * (0.666 - tempV) * 6;
                        }
                        if (3 * tempV > 2)
                        {
                            nombreV = TempLumi2;
                        }
                    }
                    if (nombreB > nombreR && nombreB > nombreV)
                    {
                        max = nombreB;
                        Teinte = Convert.ToDouble(4 + (nombreR - nombreV) / (maxRGB - minRGB));
                        Teinte = Math.Round(Teinte, 2);
                        if (Teinte < 0)
                        {
                            TeinteDegré = (Teinte * 60) + 360;
                        }
                        else
                        {
                            TeinteDegré = Teinte * 60;
                        }

                        TeinteDegré = Math.Round(TeinteDegré);
                       // int PourcentageTeinte = Convert.ToInt32(Teinte * 100);
                        //Console.WriteLine("Teinte " + Teinte);
                        //Console.WriteLine("PourcentageTeinte " + PourcentageTeinte);
                        TeinteFinale = Teinte / 360;
                        tempR = TeinteFinale + 0.333;
                        tempR = tempR + tempR2;
                        tempV = TeinteFinale;
                        tempV = tempV + tempV2;
                        tempB = TeinteFinale - 0.333;
                        tempB = tempB + tempB2;

                        if (tempR < 0)
                        {
                            tempR = tempR + 1;
                        }
                        if (tempB < 0)
                        {
                            tempB = tempB + 1;
                        }
                        if (tempV < 0)
                        {
                            tempV = tempV + 1;
                        }
                        if (tempR > 1)
                        {
                            tempR = tempR - 1;
                        }
                        if (tempB > 1)
                        {
                            tempB = tempB - 1;
                        }
                        if (tempV > 1)
                        {
                            tempV = tempV - 1;
                        }


                        if (6 * tempB < 1)
                        {
                            nombreB = TempLumi2 + (TempLumi1 - TempLumi2) * 6 * tempB;
                        }
                        if (2 * tempB < 1)
                        {
                            nombreB = TempLumi1;
                        }
                        if (3 * tempB < 2)
                        {
                            nombreB = TempLumi2 + (TempLumi1 - TempLumi2) * (0.666 - tempB) * 6;
                        }
                        if (3 * tempB > 2)
                        {
                            nombreB = TempLumi2;
                        }

                    }
                    nombreR = nombreR * 255;
                    nombreV = nombreV * 255;
                    nombreB = nombreB * 255;
                    if (nombreR > 255)
                    {
                        nombreR = 255;
                    }
                    if (nombreR < 0)
                    {
                        nombreR = 0;
                    }
                    if (nombreB > 255)
                    {
                        nombreB = 255;
                    }
                    if (nombreB < 0)
                    {
                        nombreB = 0;
                    }
                    if (nombreV > 255)
                    {
                        nombreV = 255;
                    }
                    if (nombreV < 0)
                    {
                        nombreV = 0;
                    }


                    byte PixR = Convert.ToByte(nombreR);
                    byte PixV = Convert.ToByte(nombreV);
                    byte PixB = Convert.ToByte(nombreB);

                    fichier[compteur] = PixR;
                    fichier[compteur + 1] = PixV;
                    fichier[compteur + 2] = PixB;
                    compteur = compteur + 3;
                }
            }

            File.WriteAllBytes(HSL, fichier);
            Process.Start(new ProcessStartInfo(HSL) { UseShellExecute = true });
        } 
        

        /// <summary>
        /// dessine le contour de la fractale de Mandelbrot
        /// </summary>
        /// <param name="name"></param>
        /// <param name="itérationMax"></param>
        public void ContourDeMandelbrot(string name, int itérationMax)
        {

            byte[] fichier = new byte[taille];
            byte[] stock = new byte[4];
            //On remplit la matrice de noir
            Remplissage(this.Matricepixel);

            // on définit le cadre de la fractale
            double x1 = -2.1;
            double x2 = 0.6;
            double y1 = -1.2;
            double y2 = 1.2;

            //on definit les zooms en fonction de la taille de notre image
            double zoom_X = this.hauteur / (x2 - x1);
            double zoom_Y = this.largeur / (y2 - y1);

            //creation de la fractale



            for (int x = 0; x < this.hauteur; x++)
            {
                for (int y = 0; y < this.largeur; y++)
                {
                    Complexe c = new Complexe(x / zoom_X + x1, y / zoom_Y + y1);
                    Complexe z = new Complexe(0, 0);
                    int a = 0;
                    while (a < itérationMax || z.Module() * z.Module() > 4)
                    {
                        double tmp = z.Réelle;
                        z.Réelle = z.Réelle * z.Réelle - z.Imaginaire * z.Imaginaire + c.Réelle;
                        z.Imaginaire = 2 * z.Imaginaire * tmp + c.Imaginaire;
                        a = a + 1;
                    }
                    if (a == itérationMax)
                    {
                        this.Matricepixel[x, y] = new Pixel(255, 255, 255);
                    }
                }
            }


            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(taille);                // on récupère chacun leur tour les infos de l'image ( taille, hauteur,largeur...) et on les stockes dans une variables temporaire
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];                                // on utilise la variable temporaire pour remplir un tableau variable (VAR) que l'on va utiliser pour créer un nouveau fichier
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(Offset);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }

            stock = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

            //Image

            int compteur = 54;
            for (int iLigne = 0; iLigne < Matricepixel.GetLength(0); iLigne++)
            {
                for (int iColonne = 0; iColonne < Matricepixel.GetLength(1); iColonne++)
                {

                    fichier[compteur] = Matricepixel[iLigne, iColonne].rouge;           //on attribut chaque couleurs à son emplacement
                    fichier[compteur + 1] = Matricepixel[iLigne, iColonne].vert;
                    fichier[compteur + 2] = Matricepixel[iLigne, iColonne].bleu;
                    compteur = compteur + 3;                                         // on avance de +3 pour les 3 couleurs pour chaque pixel
                }
            }
            File.WriteAllBytes(name, fichier);                       //on sauvegarde l'image(sous le nom ImageToByte)
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });
        }
        #endregion

        #region TD4
        /// <summary>
        /// donne le pixel issus du calcul de convolution pour noyau, la matrice de l'effet choisi
        /// </summary>
        /// <param name="pixel"></param>
        /// /// <param name ="matrice_convolution"></param>
        /// <param name="effet"></param>
        /// <returns></returns>

        Pixel ApplicationConvolution(Pixel[,] pixel, int[,] matrice_convolution, int x, int y, string effet)
        {
            byte rouge = 0;
            byte vert = 0;
            byte bleu = 0;
            int InfoRouge = 0;
            int InfoVert = 0;
            int InfoBleu = 0;

            for (int i = 0; i < 3; i++)                            // Matrice de convolution = 3*3 donc nos index i et j sont inférieur à 3
            {
                for (int j = 0; j < 3; j++)
                {
                    if (effet == "5")
                    {
                        InfoRouge += rouge + (pixel[(x - 1) + i, (y - 1) + j].rouge) * matrice_convolution[i, j] / 9;
                        InfoVert += vert + (pixel[(x - 1) + i, (y - 1) + j].vert) * matrice_convolution[i, j] / 9;
                        InfoBleu += bleu + (pixel[(x - 1) + i, (y - 1) + j].bleu) * matrice_convolution[i, j] / 9;
                    }
                    else
                    {
                        InfoRouge += rouge + (pixel[(x - 1) + i, (y - 1) + j].rouge) * matrice_convolution[i, j];      //Principe des matrices de convolution 
                        InfoVert += vert + (pixel[(x - 1) + i, (y - 1) + j].vert) * matrice_convolution[i, j];         //On multiplie le noyau (La matrice 3*3 de notre matrice image) avec la matrice de convolution
                        InfoBleu += bleu + (pixel[(x - 1) + i, (y - 1) + j].bleu) * matrice_convolution[i, j];         //on additione les résultats (d'ou le += pour avoir la valeur de sortie)
                    }
                }
            }
            if (InfoRouge > 255)                         // on applique des valeur arbitraire pour éviter les erreur d'index la valeur maximale d'une couleur étant compris en 0 et 255
            {                                            // si la valeur donnée aprés passage de la matruce de convolution est supérieur à 255, on applique la valeur 255 à la couleur et inversement pour des valeurs inférieur à 0 
                rouge = 255;
            }
            else if (InfoRouge < 0)
            {
                rouge = 0;
            }
            else
            {
                rouge = Convert.ToByte(InfoRouge);
            }

            if (InfoVert > 255)                 // on répete ceci pour toute les couleurs afin d'éviter toute erreur d'index
            {
                vert = 255;
            }
            else if (InfoVert < 0)
            {
                vert = 0;
            }
            else
            {
                vert = Convert.ToByte(InfoVert);
            }

            if (InfoBleu > 255)
            {
                bleu = 255;
            }
            else if (InfoBleu < 0)
            {
                bleu = 0;
            }
            else
            {
                bleu = Convert.ToByte(InfoBleu);
            }


            Pixel pixelTemp = new Pixel(rouge, vert, bleu);   // notre nouvelle matrice prend alors les valeur trouvés en utilisant la formule précédante 

            return pixelTemp;            // on retourne la nouvelle matrice
        }

        /// <summary>
        /// Construit l'image résultant de la convolution par un effet choisi
        /// </summary>
        /// <param name="name"></param>
        /// <param name="effet"></param>
        public void Convolution(string name, string effet)
        {
            // on utilise la meme forme que toute les méthode que précédamment pour obtenir les info de l'image et les stocker dans une variable
            byte[] fichier = new byte[taille];
            byte[] stock = new byte[4];

            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(taille);
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(Offset);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }

            stock = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

            //Image
            int[,] effetConvolution = null;                 // on initialise la matrice de convolution (3*3)
            if (effet == "1")
            {
                int[,] detection = new int[,] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };       // Matrice de détection trouvée sur internet
                effetConvolution = detection;
            }
            if (effet == "2")
            {
                int[,] renforcement = new int[,] { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } }; // Matrice de renforcement trouvée sur internet
                effetConvolution = renforcement;
            }
            if (effet == "3")
            {
                int[,] contraste = new int[,] { { 0, -1, 0 }, { -1, 5, -1 }, { 0, -1, 0 } }; // Matrice de contraste trouvée sur internet
                effetConvolution = contraste;
            }
            if (effet == "4")
            {
                int[,] repoussage = new int[,] { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } }; // Matrice de repoussage trouvée sur internet
                effetConvolution = repoussage;
            }
            if (effet == "5")
            {
                int[,] flou = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } }; // Matrice de flou trouvée sur internet
                effetConvolution = flou;
            }

            int compteur = 54;
            for (int iLigne = 0; iLigne < Matricepixel.GetLength(0); iLigne++)
            {
                for (int iColonne = 0; iColonne < Matricepixel.GetLength(1); iColonne++)       // on parcout en ligne et colonne notre matrice
                {
                    if (iLigne == 0 || iColonne == 0 || iLigne == (Matricepixel.GetLength(0) - 1) || iColonne == (Matricepixel.GetLength(1) - 1)) //exception dans ces cas la on ne change rien à la matrice de base (pixel de départ ou 1 pixel avant la fin)
                    {
                        fichier[compteur] = Matricepixel[iLigne, iColonne].rouge;
                        fichier[compteur + 1] = Matricepixel[iLigne, iColonne].vert;
                        fichier[compteur + 2] = Matricepixel[iLigne, iColonne].bleu;
                        compteur = compteur + 3;
                    }
                    else
                    {
                        Pixel pixelTemp = ApplicationConvolution(Matricepixel, effetConvolution, iLigne, iColonne, effet); // on applique la nouvelle matrice à notre matrice existante
                        fichier[compteur] = pixelTemp.rouge;
                        fichier[compteur + 1] = pixelTemp.vert;
                        fichier[compteur + 2] = pixelTemp.bleu;
                        compteur = compteur + 3;
                    }

                }
            }

            File.WriteAllBytes(name, fichier);
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });
        }



        #endregion

        #region TD5
        /// <summary>
        /// Méthode qui nous permet de remplir une matrice de noir 
        /// </summary>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public void Remplissage(Pixel[,] pixel)
        {
            for (int i = 0; i < pixel.GetLength(0); i++)
            {
                for (int j = 0; j < pixel.GetLength(1); j++)
                {
                    pixel[i, j] = new Pixel(0, 0, 0);
                }
            }
        }

        /// <summary>
        /// Méthode permettant d'afficher l'histogramme de couleur d'une imag
        /// </summary>
        /// <returns></returns>
        public void Histograme()
        {
            int[] HistogrameRouge = new int[256];  // on initialise 3 tableaux pour chacune des couleurs d'un Pixel ( Rouge Vert Bleu) et on leur attribue une taille de 256 car la couleurs est codé sur 8 bit donc 256 valeurs
            int[] HistogrameVert = new int[256];
            int[] HistogrameBleu = new int[256];
            for (int iLigne = 0; iLigne < Matricepixel.GetLength(0); iLigne++)  // on va parcourir nos lignes et nos colonnes des différents tableau pour les remplir avec les pixel de leurs couleurs correspondantes (Histogramme rouge avec des pixels rouges ...) 
            {
                for (int iColonne = 0; iColonne < Matricepixel.GetLength(1); iColonne++)
                {
                    HistogrameRouge[Matricepixel[iLigne, iColonne].rouge]++;          // remplissage des tableaux 
                    HistogrameVert[Matricepixel[iLigne, iColonne].vert]++;
                    HistogrameBleu[Matricepixel[iLigne, iColonne].bleu]++;
                }
            }

            int ValeurMax = 0;    // pour notre histogramme on à besoin de définir la valeur max dans les tableau afin de construire notre histogramme

            for (int index = 0; index < 256; index++)
            {

                if (ValeurMax < HistogrameRouge[index])    // compare les valeur des tableau avec la valeur max, si la valeur du tableau est plus grande cela devient la valeur max
                {                                          // on répete ceci pour tout les tableaux
                    ValeurMax = HistogrameRouge[index];
                }
                if (ValeurMax < HistogrameVert[index])
                {
                    ValeurMax = HistogrameVert[index];
                }
                if (ValeurMax < HistogrameBleu[index])
                {
                    ValeurMax = HistogrameBleu[index];
                }
            }

            Pixel[,] HRouge = new Pixel[ValeurMax, 256]; // on créer une nouvelle matrice de pixel ayant pour grandeur la valeur max et la valeur max de couleur 256 ( ligne = valeur max pour avoir la distribution des couleurs)
            Pixel[,] HVert = new Pixel[ValeurMax, 256];
            Pixel[,] HBleu = new Pixel[ValeurMax, 256];
            Remplissage(HRouge);                   // On rempli nos matrices de pixels avec des pixel noirs afin d'éviter quelle ne soit nulle        
            Remplissage(HVert);
            Remplissage(HBleu);


            for (int iColonne = 0; iColonne < 255; iColonne++)  // on parcourt les tableaux Histogrames des différentes couleurs et on remplit les matrices de couleurs par leurs couleurs correspondantes
            {
                for (int iLigne = 0; iLigne < HistogrameRouge[iColonne]; iLigne++)
                {
                    HRouge[iLigne, iColonne] = new Pixel(0, 0, 255);
                }
            }
            for (int iColonne = 0; iColonne < 255; iColonne++)
            {
                for (int iLigne = 0; iLigne < HistogrameVert[iColonne]; iLigne++)
                {
                    HVert[iLigne, iColonne] = new Pixel(0, 255, 0);
                }
            }
            for (int iColonne = 0; iColonne < 255; iColonne++)
            {
                for (int iLigne = 0; iLigne < HistogrameBleu[iColonne]; iLigne++)
                {
                    HBleu[iLigne, iColonne] = new Pixel(255, 0, 0);
                }
            }

            byte[] fichier = new byte[ValeurMax * 256 * 3 + 54]; //on se base sur la même méthode que précédament, cette fois ci le tableau de byte est aux dimension ValeurMax * 256 (valeur couleur)*3 (3 couleurs) +54 (pour le Header et les info de l'image)
            byte[] stock = new byte[4];
            string rouge = "rouge.bmp";
            string vert = "vert.bmp";
            string bleu = "bleu.bmp";

            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(ValeurMax * 256 * 3 + 54);
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(54);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(HVert.GetLength(1));  //ici on peux mettre n'importe quelle matrice de pixel histogramme car elles ont la même taille
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }

            stock = Convertir_Int_To_Endian(ValeurMax);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }


            int compteur = 54;
            for (int iLigne = 0; iLigne < HRouge.GetLength(0); iLigne++)                        // on utilise la même méthode que pour les méthodes NoirBlanc par exemple
            {
                for (int iColonne = 0; iColonne < HRouge.GetLength(1); iColonne++)
                {
                    fichier[compteur] = HRouge[iLigne, iColonne].rouge;                      //on rempli le tableau de byte avec la matrice de pixel histograme 
                    fichier[compteur + 1] = HRouge[iLigne, iColonne].vert;                   // on fait pareil pour les 2 autres couleurs
                    fichier[compteur + 2] = HRouge[iLigne, iColonne].bleu;
                    compteur += 3;
                }
            }
            File.WriteAllBytes(rouge, fichier);
            Process.Start(new ProcessStartInfo(rouge) { UseShellExecute = true });
            compteur = 54;
            for (int iLigne = 0; iLigne < HVert.GetLength(0); iLigne++)
            {
                for (int iColonne = 0; iColonne < HVert.GetLength(1); iColonne++)
                {
                    fichier[compteur] = HVert[iLigne, iColonne].rouge;
                    fichier[compteur + 1] = HVert[iLigne, iColonne].vert;
                    fichier[compteur + 2] = HVert[iLigne, iColonne].bleu;
                    compteur += 3;
                }
            }

            File.WriteAllBytes(vert, fichier);
            Process.Start(new ProcessStartInfo(vert) { UseShellExecute = true });

            compteur = 54;
            for (int iLigne = 0; iLigne < HBleu.GetLength(0); iLigne++)
            {
                for (int iColonne = 0; iColonne < HBleu.GetLength(1); iColonne++)
                {
                    fichier[compteur] = HBleu[iLigne, iColonne].rouge;
                    fichier[compteur + 1] = HBleu[iLigne, iColonne].vert;
                    fichier[compteur + 2] = HBleu[iLigne, iColonne].bleu;
                    compteur += 3;
                }
            }
            File.WriteAllBytes(bleu, fichier);
            Process.Start(new ProcessStartInfo(bleu) { UseShellExecute = true });




        }

        /// <summary>
        /// Permet de faire le lien en 2 byte et en créer un seul au final avec les informations des 2
        /// </summary>
        /// <param name="b1"></param>
        /// <param name="b2">(Méthode BibMath donnée sur le projet </param>
        /// <returns></returns>
        public byte LienByte(byte b1, byte b2)
        {
            string nb1 = Convert.ToString(b1, 2).PadLeft(8, '0');   // on convertit notre Byte 1 en chaine de caractère dans la base 2
                                                                    // aligne les caractères à droite en remplissant les espaces manquant avec des 0 pour obtenir un total de 8 caractères
            string nb2 = Convert.ToString(b2, 2).PadLeft(8, '0');

            string somme = nb1.Substring(0, 4) + nb2.Substring(0, 4); //fait la somme des sous chaines de caractéres ( sous chaine partant de 0 et allant à 4) des 2 bytes (Méthode BibMath : http://www.bibmath.net/crypto/index.php?action=affiche&quoi=stegano/cacheimage ) 
            byte résultat = Convert.ToByte(somme, 2); //convertie le résultat de la somme des sous chaine de caractère en byte depuis la base 2
            return résultat;
        }

        /// <summary>
        /// Permet de cacher une image dans une autre
        /// </summary>
        /// <param name="pixel2"></param>
        /// <param name ="name"></param>
        /// <returns></returns>
        public void Sténographie(MyImage pixel2, string name)
        {

            int offsetPixel2 = pixel2.offset;           // on initialise les paramètre de notre nouvelle image
            int taillePixel2 = pixel2.taille;
            int hauteurPixel2 = pixel2.hauteur;
            int largeurPixel2 = pixel2.largeur;
            byte[] stock = new byte[4];


            if (taillePixel2 < taille)       //on gère les exceptions si la taille/largeur ... est inférieur à notre image de base, cela devient sa nouvelle taille.  
            {
                taillePixel2 = taille;
            }
            if (hauteurPixel2 < hauteur)
            {
                hauteurPixel2 = hauteur;
            }
            if (largeurPixel2 < largeur)
            {
                largeurPixel2 = largeur;
            }
            if (taillePixel2 > taille)       //on gère les exceptions si la taille/largeur ... est inférieur à notre image de base, cela devient sa nouvelle taille.  
            {
                taillePixel2 = taille;
            }
            if (hauteurPixel2 > hauteur)
            {
                hauteurPixel2 = hauteur;
            }
            if (largeurPixel2 > largeur)
            {
                largeurPixel2 = largeur;
            }

            byte[] fichier = new byte[taillePixel2];

            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(taillePixel2);       // méthode utilisé précédemment on remplace juste les varaiables par les variables de la nouvelle image
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(offsetPixel2);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(largeurPixel2);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }

            stock = Convertir_Int_To_Endian(hauteurPixel2);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

            //Image
            int compteur = 54;

            for (int iLigne = 0; iLigne < hauteurPixel2; iLigne++)             // on parcours en ligne et en colone jusqua parcourir toute la nouvelle image
            {
                for (int iColonne = 0; iColonne < largeurPixel2; iColonne++)
                {
                    int compteur2 = 0;
                    if (hauteur - 1 < iLigne)
                    {
                        fichier[compteur] = LienByte(0, pixel2.Matricepixel[iLigne, iColonne].rouge);        //On fait le lien entre les donnée de l'image 1 et de l'image 2 avec notre fonction LienByte
                        fichier[compteur + 1] = LienByte(0, pixel2.Matricepixel[iLigne, iColonne].vert);     // on remplit les espace vide de l'image de base afin jusqu'a qu'elle les meme dimension que la nouvelle image 
                        fichier[compteur + 2] = LienByte(0, pixel2.Matricepixel[iLigne, iColonne].bleu);     // on l'a rempli de manière à ce que la couleur résultante soit beaucoup plus sombre que la couleur de base
                        compteur2++;                                                              //exemple si la couleur de l'image 2 est de 255 , la couleur résultante sera de 15 ( 0000 1111)
                    }
                    if (largeur - 1 < iColonne)
                    {
                        fichier[compteur] = LienByte(0, pixel2.Matricepixel[iLigne, iColonne].rouge);
                        fichier[compteur + 1] = LienByte(0, pixel2.Matricepixel[iLigne, iColonne].vert);
                        fichier[compteur + 2] = LienByte(0, pixel2.Matricepixel[iLigne, iColonne].bleu);
                        compteur2++;
                    }
                    if (pixel2.hauteur - 1 < iLigne)
                    {
                        fichier[compteur] = LienByte(Matricepixel[iLigne, iColonne].rouge, 0);        //méme méthode mais cette fois ci les couleurs seront que légerement changé
                        fichier[compteur + 1] = LienByte(Matricepixel[iLigne, iColonne].vert, 0);     // exemple si la couleur de l'image 1 est de 255 la couleur résultante sera de 240 ( 1111 0000)
                        fichier[compteur + 2] = LienByte(Matricepixel[iLigne, iColonne].bleu, 0);
                        compteur2++;
                    }
                    if (pixel2.largeur - 1 < iColonne)
                    {
                        fichier[compteur] = LienByte(Matricepixel[iLigne, iColonne].rouge, 0);
                        fichier[compteur + 1] = LienByte(Matricepixel[iLigne, iColonne].vert, 0);
                        fichier[compteur + 2] = LienByte(Matricepixel[iLigne, iColonne].bleu, 0);
                        compteur2++;
                    }
                    if (compteur2 == 0)                                                             // une fois quelle on les meme dimension,on "supperpose" les deux image pour n'en former plus qu'une
                    {
                        fichier[compteur] = LienByte(Matricepixel[iLigne, iColonne].rouge, pixel2.Matricepixel[iLigne, iColonne].rouge);
                        fichier[compteur + 1] = LienByte(Matricepixel[iLigne, iColonne].vert, pixel2.Matricepixel[iLigne, iColonne].vert);
                        fichier[compteur + 2] = LienByte(Matricepixel[iLigne, iColonne].bleu, pixel2.Matricepixel[iLigne, iColonne].bleu);
                    }
                    compteur = compteur + 3;


                }
            }
            File.WriteAllBytes(name, fichier);
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });
        }

        /// <summary>
        /// dessine la fractale de mandelbrot
        /// </summary>
        /// <param name="name"></param>
        /// <param name="itérationMax"></param>
        public void Mandelbrot(string name, int itérationMax)
        {
           
            byte[] fichier = new byte[taille];
            byte[] stock = new byte[4];
            //On remplit la matrice de noir
            Remplissage(this.Matricepixel);

            // on définit le cadre de la fractale
            double x1 = -2.1;
            double x2 = 0.6;
            double y1 = -1.2;
            double y2 = 1.2;

            //on definit les zooms en fonction de la taille de notre image
            double zoom_X = this.hauteur / (x2 - x1);
            double zoom_Y = this.largeur / (y2 - y1);

            //creation de la fractale
            
           
           
            for(int x=0; x < this.hauteur;x++)
            {
                for (int y = 0; y < this.largeur; y++)
                {
                    Complexe c = new Complexe(x / zoom_X + x1, y / zoom_Y + y1);
                    Complexe z = new Complexe(0, 0);
                    int a = 0;
                    while (a < itérationMax && z.Module() * z.Module() < 4 )
                    {
                        double tmp = z.Réelle;
                        z.Réelle = z.Réelle * z.Réelle - z.Imaginaire * z.Imaginaire + c.Réelle;
                        z.Imaginaire = 2 * z.Imaginaire * tmp + c.Imaginaire;
                        a = a + 1;
                    }
                    if (a == itérationMax)
                    {
                        this.Matricepixel[x, y] = new Pixel(255,255,255);
                    }
                }
            }


            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(taille);               
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];                                
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(Offset);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }

            stock = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

            //Image

            int compteur = 54;
            for (int iLigne = 0; iLigne < Matricepixel.GetLength(0); iLigne++)
            {
                for (int iColonne = 0; iColonne < Matricepixel.GetLength(1); iColonne++)
                {

                    fichier[compteur] = Matricepixel[iLigne, iColonne].rouge;           
                    fichier[compteur + 1] = Matricepixel[iLigne, iColonne].vert;
                    fichier[compteur + 2] = Matricepixel[iLigne, iColonne].bleu;
                    compteur = compteur + 3;                                         
                }
            }
            File.WriteAllBytes(name, fichier);                       
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });
        }
        
        /// <summary>
        /// dessine un des ensembles de Julia
        /// </summary>
        /// <param name="name"></param>
        /// <param name="itérationMax"></param>
        public void Julia(string name, int itérationMax)
        {
            
            byte[] fichier = new byte[taille];
            byte[] stock = new byte[4];
            //On remplit la matrice de noir
            Remplissage(this.Matricepixel);

            // on définit le cadre de la fractale
            double x1 = -1;
            double x2 = 1;
            double y1 = -1.2;
            double y2 = 1.2;

            //on definit les zooms en fonction de la taille de notre image
            double zoom_X = this.hauteur / (x2 - x1);
            double zoom_Y = this.largeur / (y2 - y1);

            //creation de la fractale



            for (int x = 0; x < this.hauteur; x++)
            {
                for (int y = 0; y < this.largeur; y++)
                {
                    Complexe c = new Complexe(0.285,0.01);
                    Complexe z = new Complexe(x / zoom_X + x1, y / zoom_Y + y1);
                    int a = 0;
                    while (a < itérationMax && z.Module() * z.Module() < 4)
                    {
                        double tmp = z.Réelle;
                        z.Réelle = z.Réelle * z.Réelle - z.Imaginaire * z.Imaginaire + c.Réelle;
                        z.Imaginaire = 2 * z.Imaginaire * tmp + c.Imaginaire;
                        a = a + 1;
                    }
                    if (a == itérationMax)
                    {
                        this.Matricepixel[x, y] = new Pixel(255, 255, 255);
                    }
                }
            }


            //Header
            fichier[0] = 66;
            fichier[1] = 77;

            stock = Convertir_Int_To_Endian(taille);                
            for (int i = 2; i <= 5; i++)
            {
                fichier[i] = stock[i - 2];                                
            }

            stock = Convertir_Int_To_Endian(0);
            for (int i = 6; i <= 9; i++)
            {
                fichier[i] = stock[i - 6];
            }

            stock = Convertir_Int_To_Endian(Offset);
            for (int i = 10; i <= 13; i++)
            {
                fichier[i] = stock[i - 10];
            }

            //HeaderInfo
            stock = Convertir_Int_To_Endian(40);
            for (int i = 14; i <= 17; i++)
            {
                fichier[i] = stock[i - 14];
            }

            stock = Convertir_Int_To_Endian(largeur);
            for (int i = 18; i <= 21; i++)
            {
                fichier[i] = stock[i - 18];
            }

            stock = Convertir_Int_To_Endian(hauteur);
            for (int i = 22; i <= 25; i++)
            {
                fichier[i] = stock[i - 22];
            }

            fichier[26] = 0;
            fichier[27] = 0;

            stock = Convertir_Int_To_Endian(nombrebitCouleur);
            for (int i = 28; i <= 29; i++)
            {
                fichier[i] = stock[i - 28];
            }

            for (int i = 30; i <= 53; i++)
            {
                fichier[i] = 0;
            }

            //Image

            int compteur = 54;
            for (int iLigne = 0; iLigne < Matricepixel.GetLength(0); iLigne++)
            {
                for (int iColonne = 0; iColonne < Matricepixel.GetLength(1); iColonne++)
                {

                    fichier[compteur] = Matricepixel[iLigne, iColonne].rouge;          
                    fichier[compteur + 1] = Matricepixel[iLigne, iColonne].vert;
                    fichier[compteur + 2] = Matricepixel[iLigne, iColonne].bleu;
                    compteur = compteur + 3;                                         
                }
            }
            File.WriteAllBytes(name, fichier);                       
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });
        }
        #endregion

    }

}    



#endregion