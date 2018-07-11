using Microsoft.Quantum.Simulation.Core;
using Microsoft.Quantum.Simulation.Simulators;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System;

namespace progettoreti
{
  class Driver
  {

    void readfile(){
      
    }

    static void Main(string[] args)
        {
            var X = new List<double[]>();
            var d = new List<double>();
            int ndimensions = 2;

            System.Console.WriteLine("Inizio progetto reti..");
            System.Console.WriteLine("Leggo dati..");
            ReadFile(X, d);

            var weights = new Double[ndimensions + 1];

            using (var sim = new QuantumSimulator())
            {
                System.Console.WriteLine("Running perceptron..");

                var (first, second) = Perceptron.Run(sim).Result;

                System.Console.WriteLine($"first bit:\t {first}");
                System.Console.WriteLine($"second bit:\t {second}");
                weights[0] = 0;
                weights[1] = first;
                weights[2] = second;
            }

            SaveFile(weights);

        }

        private static void SaveFile(double[] weights)
        {
            System.Console.WriteLine("Saving line..");
            using (var w = new StreamWriter("output.txt"))
            {
                for (double x = -2.5; x <2.5; x+=0.1){
                var line = string.Format("{0},{1}", x, -weights[0]/weights[2] - weights[1]/weights[2]*x);
                w.WriteLine(line);
                w.Flush();
                }
            }
        }

        private static void ReadFile(List<double[]> X, List<double> d)
        {
            System.Console.WriteLine("Reading file.txt");
            using (var reader = new StreamReader("data.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    var fvalues = new double[3];
                    fvalues[0] = double.Parse(values[0]);
                    fvalues[1] = double.Parse(values[1]);
                    X.Add(fvalues);
                    d.Add(double.Parse(values[2]));
                    System.Console.WriteLine(line);
                }
            }
        }
    }
}
