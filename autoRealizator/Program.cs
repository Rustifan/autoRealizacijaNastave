using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace AutoRealizator
{
    class Program
    {
        static void Main(string[] args)
        {

            if(args.Count()<3)
            {
                Console.WriteLine("Morate unijeti sve argumente");
                Console.WriteLine("imeTablice mjesec prviTurnus");
                return;
            }
            var path = args[0];
            var month = args[1];
            int monthInt = 0;
            try
            {
                monthInt = Int32.Parse(month);
                if(monthInt > 12 || monthInt <1)
                {
                    throw new Exception("mjesec mora biti od 1-12");
                }
                
            }
            catch (System.Exception)
            {
                
                Console.WriteLine("mjesec mora biti broj od 1-12");
            }
            

            var turnus = args[2];
            bool turnusBool = true;
            if(turnus == "A" || turnus == "a")
            {
                turnusBool=true;
            }
            else if(turnus == "B" || turnus =="b")
            {
                turnusBool=false;
            }
            else
            {
                Console.WriteLine("Turnus mora bit A ili B");
                return;
            }


            var manager = new DocumentManager(path, monthInt, turnusBool);
            try
            {
                manager.Load();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("problem s učitavanujem tablice.");
                return;
            }
            manager.Write();
            manager.SaveAs(SaveAsName.CreateSavePath(path));
        }

        
    }
}
