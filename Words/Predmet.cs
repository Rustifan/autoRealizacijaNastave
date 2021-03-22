using System.Collections.Generic;

namespace Words
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
        public List<PredmetClass> lista { get; set; }
    }
}