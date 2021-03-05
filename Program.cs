﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetInfoGit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Quel image voulez vous choisir ?");
            Console.WriteLine("Tapez 1 pour choisir Coco.");
            Console.WriteLine("Tapez 2 pour choisir lena.");
            string ChoixImage = Convert.ToString(Console.ReadLine());
            if (ChoixImage == "1" || ChoixImage == "2")
            {
                if(ChoixImage == "1" )
                ChoixImage = "coco.bmp";
                if (ChoixImage == "2")
                    ChoixImage = "lena.bmp";
            }
            else
            {
                Console.WriteLine("votre choix est invalide");
                Console.WriteLine("Votre image sera Coco.bmp par défaut");
                ChoixImage = "coco.bmp";
            }
            
            MyImage image = new MyImage(ChoixImage);
            System.Threading.Thread.Sleep(2000);
            ConsoleKeyInfo cki;
            do
            {
                Console.Clear();
                Console.WriteLine("Menu :\n\n"
                                 + "> 1: Mettre en Noir et Blanc \n"
                                 + "> 2: Mettre en dégradés de gris \n"
                                 + "> 3: inverser les couleurs \n"
                                 + "> 4: Réduire\n"
                                 + "> 5: Aggrandir\n"
                                 + "> 6: Faire une Rotation\n"
                                 + "> 7: Mettre en miroir\n"
                                 + "> 8: Matrice de convolution\n"
                                 + "\n");
                int exo = Convert.ToInt32(Console.ReadLine());
                string name;
                int valeur;
                switch (exo) 
                {
                    #region
                    case 1:
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp");
                        name = Console.ReadLine() + ".bmp";
                        image.NoirEtBlanc(name);
                        break;
                    case 2:
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp");
                        name = Console.ReadLine() + ".bmp";
                        image.DégradéGris(name);
                        break;
                    case 3:
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp");
                        name = Console.ReadLine() + ".bmp";
                        image.InverserCouleur(name);
                        break;
                    case 4:
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp");
                        name = Console.ReadLine() + ".bmp";
                        Console.WriteLine("Choisissez un rapport de réduction");
                        valeur = Convert.ToInt32(Console.ReadLine());
                        image.Reduction(name,valeur);
                        break;
                    case 5:
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp");
                        name = Console.ReadLine() + ".bmp";
                        Console.WriteLine("Choisissez un rapport d'aggrandissement");
                        valeur = Convert.ToInt32(Console.ReadLine());
                        if (valeur == 0)
                        {
                            Console.WriteLine("valeur invalide");
                            break;
                        }

                        image.Agrandir(name, valeur);
                        break;
                    case 6:
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp");
                        name = Console.ReadLine() + ".bmp";
                        Console.WriteLine("Choisissez un angle de rotation");
                        valeur = Convert.ToInt32(Console.ReadLine());
                        //image.Rotation(name, valeur);
                        break;
                    case 7:
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp");
                        name = Console.ReadLine() + ".bmp";
                        image.Miroir(name);
                        break;
                    case 8:
                        Console.WriteLine("Veuillez choisir l'effet de votre matrice de convolution");
                        Console.WriteLine("Tapez 1 pour Detection des bords.");
                        Console.WriteLine("Tapez 2 pour Renforcement des bords");
                        Console.WriteLine("Tapez 3 pour Augmentation du Contraste");
                        Console.WriteLine("Tapez 4 pour Repoussage");
                        Console.WriteLine("Tapez 5 pour Flou");
                        string effet = Convert.ToString(Console.ReadLine());
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp");
                        name = Console.ReadLine() + ".bmp";
                        if (effet == "1" || effet == "2" || effet == "3" || effet == "4" || effet == "5")
                        {
                            image.Convolution(name, effet);
                        }
                        else
                        {
                            Console.WriteLine("votre numéro d'effet est invalide");
                            break;
                        }
                        break;
                    default:
                        Console.WriteLine("Commande non valide, recommencez");
                        break;
                        #endregion 
                }
                Console.WriteLine("Tapez entrée pour retourner au menu ou tapez la touche échappe pour quitter\n");
                cki = Console.ReadKey();
            } while (cki.Key != ConsoleKey.Escape);
         }
    }
}
