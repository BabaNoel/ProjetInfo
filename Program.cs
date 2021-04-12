using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetInfoGit
{
    class Program
    {
        static int ConvertisseurASCII(char a, char b)
        {
            int somme = 0;
            int valA = 0;
            int valB = 0;
            if (a =='A' || b=='A')
            {
                valA = 45 * 10;
                valB = 10;
            }
            if (a =='B' || b=='B')
            {
                valA = 45 * 11;
                valB = 11;
            }
            if (a == 'C' || b == 'C')
            {
                valA = 45 * 12;
                valB = 12;
            }
            if (a == 'D' || b == 'D')
            {
                valA = 45 * 13;
                valB = 13;
            }
            if (a == 'E' || b == 'E')
            {
                valA = 45 * 14;
                valB = 14;
            }
            if (a == 'F' || b == 'F')
            {
                valA = 45 * 15;
                valB = 15;
            }
            if (a == 'G' || b == 'G')
            {
                valA = 45 * 16;
                valB = 16;
            }
            if (a == 'H' || b == 'H')
            {
                valA = 45 * 17;
                valB = 17;
            }
            if (a == 'I' || b == 'I')
            {
                valA = 45 * 18;
                valB = 18;
            }
            if (a == 'J' || b == 'J')
            {
                valA = 45 * 19;
                valB = 19;
            }
            if (a == 'K' || b == 'K')
            {
                valA = 45 * 20;
                valB = 20;
            }
            if (a == 'L' || b == 'L')
            {
                valA = 45 * 21;
                valB = 21;
            }
            if (a == 'M' || b == 'M')
            {
                valA = 45 * 22;
                valB = 22;
            }
            if (a == 'N' || b == 'N')
            {
                valA = 45 * 23;
                valB = 23;
            }
            if (a == 'O' || b == 'O')
            {
                valA = 45 * 24;
                valB = 24;
            }
            if (a == 'P' || b == 'P')
            {
                valA = 45 * 25;
                valB = 25;
            }
            if (a == 'Q' || b == 'Q')
            {
                valA = 45 * 26;
                valB = 26;
            }
            if (a == 'R' || b == 'R')
            {
                valA = 45 * 27;
                valB = 27;
            }
            if (a == 'S' || b == 'S')
            {
                valA = 45 * 28;
                valB = 28;
            }
            if (a == 'T' || b == 'T')
            {
                valA = 45 * 29;
                valB = 29;
            }
            if (a == 'U' || b == 'U')
            {
                valA = 45 * 30;
                valB = 30;
            }
            if (a == 'V' || b == 'V')
            {
                valA = 45 * 31;
                valB = 31;
            }
            if (a == 'W' || b == 'W')
            {
                valA = 45 * 32;
                valB = 32;
            }
            if (a == 'X' || b == 'X')
            {
                valA = 45 * 33;
                valB = 33;
            }
            if (a == 'Y' || b == 'Y')
            {
                valA = 45 * 34;
                valB = 34;
            }
            if (a == 'Z' || b == 'Z')
            {
                valA = 45 * 35;
                valB = 35;
            }
            if (a == ' ' || b == ' ')
            {
                valA = 45 * 36;
                valB = 36;
            }

            somme = valB + valA;
            return somme;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Quel image voulez vous choisir ?");
            Console.WriteLine("Tapez 1 pour choisir Coco.");
            Console.WriteLine("Tapez 2 pour choisir Lena.");
            string ChoixImage = Convert.ToString(Console.ReadLine());
            if (ChoixImage == "1" || ChoixImage == "2")
            {
                if (ChoixImage == "1")
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
            System.Threading.Thread.Sleep(1500);
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
                                 + "> 6: Faire une Rotation (NE MARCHE PAS ENCORE)\n"
                                 + "> 7: Mettre en miroir\n"
                                 + "> 8: Matrice de convolution\n"
                                 + "> 9: Histogramme\n"
                                 + "> 10: Fractale de Mandelbrot\n"
                                 + "> 11: Fractale de Julia\n" 
                                 + "> 12: Stéganographie\n"
                                 + "> 13: QR Code\n"
                                 + "\n");
                int exo = Convert.ToInt32(Console.ReadLine());
                string name;
                int valeur;
                switch (exo)
                {
                    #region
                    case 1:
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp)");
                        name = Console.ReadLine() + ".bmp";
                        image.NoirEtBlanc(name);
                        break;
                    case 2:
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp)");
                        name = Console.ReadLine() + ".bmp";
                        image.DégradéGris(name);
                        break;
                    case 3:
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp)");
                        name = Console.ReadLine() + ".bmp";
                        image.InverserCouleur(name);
                        break;
                    case 4:
                        Console.WriteLine("Choisissez un rapport de réduction (sauf multiples de trois");
                        valeur = Convert.ToInt32(Console.ReadLine());
                        if (valeur <= 0)
                        {
                            Console.WriteLine("Saisie invalide");
                            break;
                        }
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp)");
                        name = Console.ReadLine() + ".bmp";
                        image.Reduction(name, valeur);
                        break;
                    case 5:
                        Console.WriteLine("Choisissez un rapport d'aggrandissement");
                        valeur = Convert.ToInt32(Console.ReadLine());
                        if (valeur == 0)
                        {
                            Console.WriteLine("valeur invalide");
                            break;
                        }
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp)");
                        name = Console.ReadLine() + ".bmp";
                        image.Agrandir(name, valeur);
                        break;
                    case 6:
                        Console.WriteLine("Choisissez un angle de rotation");
                        valeur = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp)");
                        name = Console.ReadLine() + ".bmp";
                        image.Rotation(name, valeur);
                        break;
                    case 7:
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp)");
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
                        Console.WriteLine("Veuillez choisir le nom que vous souhaitez donner à votre image (sans le .bmp)");
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
                    case 9:
                        Console.WriteLine("Voilà l'affiche de l'histogramme de votre image");
                        image.Histograme();
                        break;
                    case 10:
                        Console.WriteLine("Veuillez choisir le nom que vous souhaitez donner à votre image (sans le .bmp)");
                        name = Console.ReadLine() + ".bmp";
                        Console.WriteLine("Veuillez choisir le nombre d'itération à effectuer");
                        int itérationMaxM = Convert.ToInt32(Console.ReadLine());
                        image.Mandelbrot(name, itérationMaxM);
                        break;
                    case 11:
                        Console.WriteLine("Veuillez choisir le nom que vous souhaitez donner à votre image (sans le .bmp)");
                        name = Console.ReadLine() + ".bmp";
                        Console.WriteLine("Veuillez choisir le nombre d'itération à effectuer");
                        int itérationMaxJ = Convert.ToInt32(Console.ReadLine());
                        image.Julia(name, itérationMaxJ);
                        break;
                    case 12:
                        Console.WriteLine("Quel image voulez vous cacher dans " + ChoixImage);
                        Console.WriteLine("Tapez 1 pour choisir Coco.");
                        Console.WriteLine("Tapez 2 pour choisir Lena.");
                        string Choix = Convert.ToString(Console.ReadLine());
                        if (Choix == "1" || Choix == "2")
                        {
                            if (Choix == "1")
                                Choix = "coco.bmp";
                            if (Choix == "2")
                                Choix = "lena.bmp";
                        }
                        else
                        {
                            Console.WriteLine("votre choix est invalide");
                            Console.WriteLine("Votre image sera Coco.bmp par défaut");
                            Choix = "coco.bmp";
                        }
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp");
                        name = Console.ReadLine() + ".bmp";
                        MyImage image2 = new MyImage(Choix);
                        image.Sténographie(image2, name);
                        break;
                    case 13:
                        Encoding u8 = Encoding.UTF8;
                        Console.WriteLine("Quel phrase voulez vous inscrire ?");
                        string phrase = Convert.ToString(Console.ReadLine());
                       string phraseMaj= phrase.ToUpper();
                        List<bool> list;
                        for(int i =0;i<phraseMaj.Length-1;i++)
                        {
                            char a = phraseMaj[i];
                            char b = phraseMaj[i + 1];
                            int compteur = ConvertisseurASCII(a, b);
                            Console.WriteLine(" " + compteur + "\n");
                            list = Convert.ToString(compteur, 2).PadLeft(11, '0').Select(c => c == '1' ? true : false).ToList();
                            for (int j = 0; j < list.Count; j++)
                        {
                            Console.Write(list[j]+ " " );
                        }
                        }
                        Console.WriteLine();
                        
                       
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
