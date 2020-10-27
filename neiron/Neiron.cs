using System;

namespace MyNeiroWeb
{
    class Neiron
    {
        public  string name;
        public  double[,] mass;
        public  int TrainingNumber;

         public Neiron() {}
         public string GetName() { return name; }
         public int GetTrainingNumber() { return TrainingNumber; }
        public void Clear(string name, int x, int y)
         {
             this.name = name;
             mass = new double[x,y];
             for (int n = 0; n < mass.GetLength(0); n++)
                 for (int m = 0; m < mass.GetLength(1); m++) mass[n, m] = 0;
             TrainingNumber = 0;
         }

         public double GetRes(int[,] data){
             if (mass.GetLength(0) != data.GetLength(0) || mass.GetLength(1) != data.GetLength(1)) return -1;
             double res = 0;
             for (int n = 0; n < mass.GetLength(0); n++)
                 for (int m = 0; m < mass.GetLength(1); m++) 
                     res += 1 - Math.Abs(mass[n, m] - data[n, m]);
             return res / (mass.GetLength(0) * mass.GetLength(1));
         }

         public int Training(int[,] data)
         {
             if (data == null || mass.GetLength(0) != data.GetLength(0) || mass.GetLength(1) != data.GetLength(1)) return TrainingNumber;
             TrainingNumber++;
             for (int n = 0; n < mass.GetLength(0); n++)
                 for (int m = 0; m < mass.GetLength(1); m++)
                 {
                     double v = data[n, m] == 0 ? 0 : 1;
                     mass[n, m] += 2 * (v - 0.5f) / TrainingNumber;
                     if (mass[n, m] > 1) mass[n, m] = 1;
                     if (mass[n, m] < 0) mass[n, m] = 0;
                 }
             return TrainingNumber;
         }
       
    }


}
