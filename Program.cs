using System;
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
            //titre
            Console.WriteLine("_|_|_|_| _|_|_|_| _|_|_|_| _|_|_|_| _|    _|  _|_|_|  _|    _| _|_|_|_|\n"
                               + "_|    _| _|          _|    _|    _| _|    _| _|       _|    _| _|        \n"
                               + "_|_|_|   _|_|_|      _|    _|    _| _|    _| _|       _|_|_|_| _|_|_|\n"
                               + "_|    _| _|          _|    _|    _| _|    _| _|       _|    _| _|\n"
                               + "_|    _| _|_|_|_|    _|    _|_|_|_| _|_|_|_|  _|_|_|  _|    _| _|_|_|_| ");
            Console.WriteLine("\n\n");
            Console.WriteLine("Bienvenue sur Retouche, votre programme de retouches photos et de création.");
            Console.ReadLine();
            Console.Clear();
            int limite = 0;
            string Image=null;
            while (Image == null)//tant que l'on n'a pas assigné de valeur à l'image
            {
                //Titre 
                Console.WriteLine("_|_|_|_| _|_|_|_| _|_|_|_| _|_|_|_| _|    _|  _|_|_|  _|    _| _|_|_|_|\n"
                                   + "_|    _| _|          _|    _|    _| _|    _| _|       _|    _| _|        \n"
                                   + "_|_|_|   _|_|_|      _|    _|    _| _|    _| _|       _|_|_|_| _|_|_|\n"
                                   + "_|    _| _|          _|    _|    _| _|    _| _|       _|    _| _|\n"
                                   + "_|    _| _|_|_|_|    _|    _|_|_|_| _|_|_|_|  _|_|_|  _|    _| _|_|_|_| ");
                Console.WriteLine("\n\n");
                //Choix image
                Console.WriteLine("Quel image voulez vous choisir ?");
                Console.WriteLine("Tapez 1 pour choisir Coco.");
                Console.WriteLine("Tapez 2 pour choisir Lena.");
                int ChoixImage = Convert.ToInt32(Console.ReadLine());
                if (ChoixImage == 1 || ChoixImage == 2 )
                {
                    if (ChoixImage == 1)
                        Image = "coco.bmp";
                    if (ChoixImage == 2)
                        Image = "lena.bmp";
                }
                else
                {
                    Console.WriteLine("votre choix est invalide, recommencez");
                    Console.ReadLine();
                    Console.Clear();
                }
                if(limite==3)
                {
                    Console.WriteLine("Votre image sera Coco par défault");
                    Image = "coco.bmp";
                }
                limite++;
                
            }
            MyImage image = new MyImage(Image);
            System.Threading.Thread.Sleep(1500);
            ConsoleKeyInfo cki;
            do
            {
                Console.Clear();
                //titre
                Console.WriteLine("_|_|_|_| _|_|_|_| _|_|_|_| _|_|_|_| _|    _|  _|_|_|  _|    _| _|_|_|_|\n"
                                    + "_|    _| _|          _|    _|    _| _|    _| _|       _|    _| _|        \n"
                                    + "_|_|_|   _|_|_|      _|    _|    _| _|    _| _|       _|_|_|_| _|_|_|\n"
                                    + "_|    _| _|          _|    _|    _| _|    _| _|       _|    _| _|\n"
                                    + "_|    _| _|_|_|_|    _|    _|_|_|_| _|_|_|_|  _|_|_|  _|    _| _|_|_|_| ");
                Console.WriteLine("\n");
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
                                 + "> 14: Coutour stylisé de fractale\n"
                                 + "> 15: Changer d'image\n"
                                 + "\n");
                int numero = Convert.ToInt32(Console.ReadLine());
                string name;
                int valeur;
                switch (numero)
                {
                    #region
                    case 1:
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp)");
                        name = Console.ReadLine() + ".bmp";
                        image.NoirEtBlanc(name);
                        image = new MyImage(name);
                        break;
                    case 2:
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp)");
                        name = Console.ReadLine() + ".bmp";
                        image.DégradéGris(name);
                        image = new MyImage(name);
                        break;
                    case 3:
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp)");
                        name = Console.ReadLine() + ".bmp";
                        image.InverserCouleur(name);
                        image = new MyImage(name); 
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
                        image = new MyImage(name); 
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
                        image = new MyImage(name); 
                        break;
                    case 6:
                        Console.WriteLine("Choisissez un angle de rotation");
                        valeur = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp)");
                        name = Console.ReadLine() + ".bmp";
                        image.Rotation(name, valeur);
                        image = new MyImage(name); 
                        break;
                    case 7:
                        Console.WriteLine("Veuillez choisi le nom que vous souhaitez donner à votre image (sans le .bmp)");
                        name = Console.ReadLine() + ".bmp";
                        image.Miroir(name);
                        image = new MyImage(name); 
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
                        image = new MyImage(name); 
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
                        image = new MyImage(name);
                        break;
                    case 11:
                        Console.WriteLine("Veuillez choisir le nom que vous souhaitez donner à votre image (sans le .bmp)");
                        name = Console.ReadLine() + ".bmp";
                        Console.WriteLine("Veuillez choisir le nombre d'itération à effectuer");
                        int itérationMaxJ = Convert.ToInt32(Console.ReadLine());
                        image.Julia(name, itérationMaxJ);
                        image = new MyImage(name); 
                        break;
                    case 12:
                        Console.WriteLine("Quel image voulez vous cacher dans " + Image);
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
                        image = new MyImage(name); 
                        break;
                    case 13:
                        Encoding u8 = Encoding.UTF8;
                        Console.WriteLine("Quel phrase voulez vous inscrire ?");
                        string phrase = Convert.ToString(Console.ReadLine());
                        QRcode QR = new QRcode(phrase,1);
                        Console.WriteLine(QR.Code);
                        QR.Dessin("QRCODE");
                        //list = Convert.ToString(compteur, 2).PadLeft(11, '0').Select(c => c == '1' ? true : false).ToList();
                        break;
                    case 14:
                        Console.WriteLine("Veuillez choisir le nom que vous souhaitez donner à votre image (sans le .bmp)");
                        name = Console.ReadLine() + ".bmp";
                        Console.WriteLine("Veuillez choisir le nombre d'itération à effectuer. Nous vous recommandons une valeur entre 15 et 30 pour un bel effet");
                        int itérationMax = Convert.ToInt32(Console.ReadLine());
                        image.ContourDeMandelbrot(name, itérationMax);
                        image = new MyImage(name);
                        break;
                    case 15:
                        Image = null;
                        while (Image == null)//tant que l'on n'a pas assigné de valeur à l'image
                        {
                            //Choix image
                            Console.WriteLine("Quel image voulez vous importer?");
                            Console.WriteLine("Tapez 1 pour choisir Coco.");
                            Console.WriteLine("Tapez 2 pour choisir Lena.");
                            int ChoixImage = Convert.ToInt32(Console.ReadLine());
                            if (ChoixImage == 1 || ChoixImage == 2)
                            {
                                if (ChoixImage == 1)
                                    Image = "coco.bmp";
                                if (ChoixImage == 2)
                                    Image = "lena.bmp";
                            }
                            else
                            {
                                Console.WriteLine("votre choix est invalide, recommencez");
                                Console.ReadLine();
                                Console.Clear();
                            }
                            if (limite == 3)
                            {
                                Console.WriteLine("Votre image sera Coco par défault");
                                Image = "coco.bmp";
                            }
                            limite++;

                        }
                        image = new MyImage(Image);
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
