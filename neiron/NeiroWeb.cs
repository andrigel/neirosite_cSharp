using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;

namespace MyNeiroWeb
{
    class NeiroWeb
    {
        private const int          defaultNeironCount  =           32;
        public  const int          neironInArrayWidth  =           10;
        public  const int          neironInArrayHeight =           10;
        private const string       memory              = "memory.txt";
        private       List<Neiron> neironArray         =         null;


        public NeiroWeb()
        {
            neironArray = InitWeb();            
        }

        private static List<Neiron> InitWeb()
        {
            if (!File.Exists(memory)) return new List<Neiron>();
            string[] lines = File.ReadAllLines(memory);
            if (lines.Length == 0)    return new List<Neiron>();
            string jStr = lines[0];
            JavaScriptSerializer json = new JavaScriptSerializer();
            List<Object> objects = json.Deserialize<List<Object>>(jStr);
            List<Neiron> res = new List<Neiron>();
            foreach (var o in objects) res.Add(NeironCreate((Dictionary<string,Object>)o));
            return res;
        }
        public string CheckLetter(int[,] arr)
        {
            string res = null;
            double max = 0;
            foreach (var n in neironArray)
            {
                double d = n.GetRes(arr);
                if (d > max)
                {
                    max = d;
                    res = n.GetName();
                }
            }
            return res;
        }

        public void SaveJson()
        {
            JavaScriptSerializer json = new JavaScriptSerializer();
            string jStr = json.Serialize(neironArray);
            System.IO.StreamWriter file = new System.IO.StreamWriter(memory);
            file.WriteLine(jStr);
            file.Close();
        }      

        public string[] GetLetters()
        {
            var res = new List<string>();
            for (int i = 0; i < neironArray.Count; i++) res.Add(neironArray[i].GetName());
            res.Sort();
            return res.ToArray();
        }

        public void SetTraining(string trainingName, int[,] data)
        {
            Neiron neiron = neironArray.Find(v => v.name.Equals(trainingName));
            if (neiron == null)
            {
                neiron = new Neiron();
                neiron.Clear(trainingName, neironInArrayWidth, neironInArrayHeight);
                neironArray.Add(neiron);
            }
            int TrainingNumber = neiron.Training(data);             
        }

        private static Neiron NeironCreate(Dictionary<string, object> o)
        {
            Neiron res = new Neiron();
            res.name = (string)o["name"];
            res.TrainingNumber = (int)o["TrainingNumber"];
            Object[] massData = (Object[])o["mass"];
            int arrSize = (int)Math.Sqrt(massData.Length);
            res.mass = new double[arrSize, arrSize];
            int index = 0;
            for (int n = 0; n < res.mass.GetLength(0); n++)
                for (int m = 0; m < res.mass.GetLength(1); m++)
                {
                    res.mass[n, m] = Double.Parse(massData[index].ToString());
                    index++;
                }
            return res;
        }
        public string GetLettersFromList()
        {
            string str = "";
            foreach (var c in this.neironArray) str += c.GetName() + " - вивчено " + c.GetTrainingNumber().ToString() + " разів\n";
            return str;
        }
    }
}
