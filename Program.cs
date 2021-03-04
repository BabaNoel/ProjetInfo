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
            string coco = "coco.bmp";
            string lena = "lena.bmp";
            MyImage Lena = new MyImage(lena);
            MyImage Coco = new MyImage(coco);
            Coco.From_Image_To_File(coco);
            
        }
                  
    }
}
