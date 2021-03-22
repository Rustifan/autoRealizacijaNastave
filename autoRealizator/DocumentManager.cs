using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using NetModular.DocX.Core;


namespace AutoRealizator
{
    
    public class DocumentManager
    {
        private readonly List<DayOfWeek?> days = new();
        private readonly int month;
        private readonly bool firstTurnus;
        private readonly string path;
        private DocX doc;
        private Table table;

        private readonly Raspored raspored;
        public DocumentManager(string path, int month, bool firstTurnus)
        {
            this.month = month;
            this.firstTurnus = firstTurnus;
            this.path = path;
            var jsonString = File.ReadAllText("./raspored.json");
            try
            {
                raspored = JsonSerializer.Deserialize<Raspored>(jsonString);
                
            }
            catch(Exception err)
            {
                Console.WriteLine(err);
            }
        }
        public bool Load()
        {
            doc = DocX.Load(path);
            table = doc.Tables[0];
           
            Row row = null;
            foreach(var r in table.Rows)
            {
                if(r.Cells.Count > 4)
                {
                    row = r;
                    break;
                }
            }
            days.Add(null);
            days.Add(null);

            for(int i = 2; i < row.Cells.Count;i++)
            {
                days.Add(null);
                var text = row.Cells[i].Paragraphs[0].Text;


                if(text.Length>0 || !text.Contains("’"))
                {
                    var num = text.Split('.')[0];
                    
                    int day;
                    if(int.TryParse(num, out day))
                    {
                        var dayOfWeek = new DateTime(2021, month, day).DayOfWeek;
                        days[i] = dayOfWeek;
                    }
                }
                

            }

            return true;
        }

        public void Write()
        {
            int rowIndex = 2;
            var lista = raspored.lista;
            int grandSum = 0;
           
            doc.ReplaceText("Ime i prezime nastavnika:", "Ime i prezime nastavnika: "+raspored.imeIPrezime);
            doc.ReplaceText("Odjel:    __________", "Odjel: "+raspored.odjel);
            doc.ReplaceText("_________________________  ", raspored.imeIPrezime);

            foreach(var predmet in lista)
            {

                var row = table.Rows[rowIndex];
                var cells = row.Cells;
                


                InsertParagraph(cells[0], predmet.predmet);
                InsertParagraph(cells[1], predmet.razred);

                
                
                
                

                bool turnus = firstTurnus;
                int sum = 0;
                bool firstEntry = false;

                for(int i = 2; i < cells.Count; i++)
                {
                    var dayOfWeek = days[i];
                    List<int> brojSati;
                    if(turnus)
                    {
                        brojSati = predmet.A;
                    }
                    else
                    {
                        brojSati = predmet.B;
                    }
                    
                    if(dayOfWeek!=null)
                    {
                        
                        
                        var praznik = cells[i].Shading.GetBrightness()<1;
                            

                        if((int)dayOfWeek-1<brojSati.Count && (int)dayOfWeek !=0 && !praznik)
                        {
                            
                            
                            int broj= brojSati[(int)dayOfWeek-1];
                            if(broj>0)
                            {
                                firstEntry = true;
                                InsertParagraph(cells[i],broj.ToString());
                                sum+=broj;
                            }
                        }
                        if(dayOfWeek == DayOfWeek.Sunday && firstEntry)
                        {
                            turnus = !turnus;
                        }
                        
                    }
                }
                InsertParagraph(cells[^1], sum.ToString());
                grandSum+= sum;
                rowIndex++;
            }
            var lastRow = table.Rows[^2];
            var lastCell = lastRow.Cells[^1];
            InsertParagraph(lastCell, grandSum.ToString());
            
        }

        private void InsertParagraph(Cell cell, string text)
        {
            var paragraph = cell.Paragraphs.FirstOrDefault();
            if(paragraph == null)
            {
                paragraph = cell.InsertParagraph();
            }
            
            paragraph.SpacingAfter(0.2);
            paragraph.SpacingBefore(0.2);
            var textFormatting = new Formatting
            {
                Size=8
            };
            paragraph.InsertText(text,false,textFormatting);

        }
        public void SaveAs(string path)
        {
            doc.SaveAs(path);
        }
    }
}