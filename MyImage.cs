﻿using System;
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

        public void From_Image_To_File(string name)
        {
            byte[] myfile = File.ReadAllBytes(Myfile);
            
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

            File.WriteAllBytes(name, Var);                       //on sauvegarde l'image(sous le nom ImageToByte)
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });
        }

        /// <summary>
        /// On va agrandir l'image n fois
        /// </summary>
        /// <param name="Myfile"></param>
        /// <returns></returns>
        public void Agrandir(string name, int n)
        {
            byte[] myfile = File.ReadAllBytes(Myfile);
           
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

            File.WriteAllBytes(name, Var);
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });

        } 

        /// <summary>
        /// Convertie l'image en dégradé de gris
        /// </summary>
        /// <param name="Myfile"></param>
        /// <returns></returns>
        public void DégradéGris(string name)
        {
            byte[] myfile = File.ReadAllBytes(Myfile);
            byte[] fichier = new byte[myfile.Length];                        
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
        /// <param name="Myfile"></param>
        /// <returns></returns>
        public void NoirEtBlanc(string name)
        {
            byte[] myfile = File.ReadAllBytes(Myfile);
           
            byte[] fichier = new byte[myfile.Length];
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
        /// Convertie l'image en miroir
        /// </summary>
        /// <param name="Myfile"></param>
        /// <returns></returns>
        public void Miroir(string name)
        {
            byte[] myfile = File.ReadAllBytes(Myfile);
            
            byte[] fichier = new byte[myfile.Length];
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
                    fichier[compteur] = Matricepixel[Ligne, Matricepixel.GetLength(1) - 1 - Colonne].rouge;
                    fichier[compteur + 1] = Matricepixel[Ligne, Matricepixel.GetLength(1) - 1 - Colonne].vert;
                    fichier[compteur + 2] = Matricepixel[Ligne, Matricepixel.GetLength(1) - 1 - Colonne].bleu;
                    compteur = compteur + 3;
                }
            }

            File.WriteAllBytes(name, fichier);
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });


        }
        
        //public void Rotation(string name, int angle)
        //{
        //    byte[] myfile = File.ReadAllBytes(Myfile);
        //    byte[] fichier = new byte[myfile.Length];
        //    byte[] stock = new byte[4];

        //    //on utilise le presque la meme methode que pour la fonction From_Image_To_File seulement la partie image change car on vient modifier la couleur de l'image
        //    //Header
        //    fichier[0] = 66;
        //    fichier[1] = 77;

        //    stock = Convertir_Int_To_Endian(taille);
        //    for (int i = 2; i <= 5; i++)
        //    {
        //        fichier[i] = stock[i - 2];
        //    }

        //    stock = Convertir_Int_To_Endian(0);
        //    for (int i = 6; i <= 9; i++)
        //    {
        //        fichier[i] = stock[i - 6];
        //    }

        //    stock = Convertir_Int_To_Endian(Offset);
        //    for (int i = 10; i <= 13; i++)
        //    {
        //        fichier[i] = stock[i - 10];
        //    }

        //    //HeaderInfo
        //    stock = Convertir_Int_To_Endian(40);
        //    for (int i = 14; i <= 17; i++)
        //    {
        //        fichier[i] = stock[i - 14];
        //    }

        //    stock = Convertir_Int_To_Endian(largeur);
        //    for (int i = 18; i <= 21; i++)
        //    {
        //        fichier[i] = stock[i - 18];
        //    }

        //    stock = Convertir_Int_To_Endian(hauteur);
        //    for (int i = 22; i <= 25; i++)
        //    {
        //        fichier[i] = stock[i - 22];
        //    }

        //    fichier[26] = 0;
        //    fichier[27] = 0;

        //    stock = Convertir_Int_To_Endian(nombrebitCouleur);
        //    for (int i = 28; i <= 29; i++)
        //    {
        //        fichier[i] = stock[i - 28];
        //    }

        //    for (int i = 30; i <= 53; i++)
        //    {
        //        fichier[i] = 0;
        //    }

        //    //Image
        //    int angle=angle*Math.PI/180;
        //    int H=Math.Abs(Math.Cos(angle))*this.hauteur+Math.Abs(Math.Sin(angle))*this.largeur;
        //    int L=Math.Abs(Math.Cos(angle))*this.largeur+Math.Abs(Math.Sin(angle))*this.hauteur;
        //    Pixel[,] image1 =new Pixel[H,L];
        //    for(int i=0; i<L;i++)
        //    {
        //            for (int j=0; i<H;i++)
        //            {
        //                image1[i,j]=Pixel(255,255,255);
        //                int X=Math.Cos(angle)*(i-H/2)-Math.Sin(angle)*(j-L/2)+H/2;
        //                int Y=Math.Sin(angle)*(i-H/2)-Math.Cos(angle)*(j-L/2)+L/2;
        //                if(X>=0 && Y>=0 && X<this.hauteur && Y<this.largeur)
        //                {
        //                    image1[X,Y]=image[i,j];
        //                }
        //            }                    
        //    }

        //    File.WriteAllBytes(name, fichier);
        //    Process.Start(new ProcessStartInfo(name) { UseShellExecute = true }); 

        //}

        public void InverserCouleur(string name)
        {
            byte[] myfile = File.ReadAllBytes(Myfile);
            
            byte[] fichier = new byte[myfile.Length];
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

                        fichier[compteur] = Convert.ToByte(255 - Matricepixel[Ligne,Colonne].rouge);
                        fichier[compteur + 1] = Convert.ToByte(255 - Matricepixel[Ligne, Colonne].vert);
                        fichier[compteur + 2] = Convert.ToByte(255 - Matricepixel[Ligne, Colonne].bleu);
                        compteur = compteur + 3;
                   
                }
            }

            File.WriteAllBytes(name, fichier);
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });


        }

        public void Reduction(string name, int n)
        {
            if (n > hauteur | n > largeur)
            {
                Console.WriteLine("La réduction demandée est trop importante");
                return;
            }
            byte[] myfile = File.ReadAllBytes(this.Myfile);
            byte[] Var = new byte[offset + (largeur / n * hauteur / n) * 3];
            byte[] VarTemp = new byte[4];

            //Header
            Var[0] = 66;
            Var[1] = 77;

            VarTemp = Convertir_Int_To_Endian(offset + ((largeur / n) * (hauteur / n)) * 3);
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

            VarTemp = Convertir_Int_To_Endian(largeur / n);
            for (int i = 18; i <= 21; i++)
            {
                Var[i] = VarTemp[i - 18];
            }

            VarTemp = Convertir_Int_To_Endian(hauteur / n);
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
            for (int iLigne = 0; iLigne < Matricepixel.GetLength(0); iLigne += n)
            {
                for (int iColonne = 0; iColonne < Matricepixel.GetLength(1); iColonne += n)
                {

                    Var[compteur] = Matricepixel[iLigne, iColonne].rouge;
                    Var[compteur + 1] = Matricepixel[iLigne, iColonne].vert;
                    Var[compteur + 2] = Matricepixel[iLigne, iColonne].bleu;
                    compteur = compteur + 3;

                }
            }

            File.WriteAllBytes(name, Var);
            Process.Start(new ProcessStartInfo(name) { UseShellExecute = true });
        }
    }

    #endregion

}
