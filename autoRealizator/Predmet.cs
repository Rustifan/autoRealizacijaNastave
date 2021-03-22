using System.Collections.Generic;

namespace AutoRealizator
{
    
    public class PredmetClass
    {
        public string razred { get; set; }
        public string predmet { get; set; }

        public List<int> A {get; set;} 
        public List<int> B {get; set;} 

    }

    public class Raspored
    {
        public string imeIPrezime{get; set;}
        public string odjel { get; set; }
        public List<PredmetClass> lista { get; set; }
        
    }
}