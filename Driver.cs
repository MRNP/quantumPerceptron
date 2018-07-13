using Microsoft.Quantum.Simulation.Core;
using Microsoft.Quantum.Simulation.Simulators;
using System.IO;
using System.Collections.Generic;
using System.Web;
using System;
using Qiskit;

namespace progettoreti
{
  class Driver
  {

    static void Main(string[] args)
        {
            var X = new List<double[]>();
            var d = new List<double>();
            int ndimensions = 2;

            Boolean RunOnIBM = false;

            System.Console.WriteLine("\n************\nInizio progetto reti..");
            Superposition();

            System.Console.WriteLine("\nLeggo dati..");
            ReadFile(X, d);

            var superposition = new int[] { 1, 1 };

            if (RunOnIBM) IBMQuantumExperienceRun();

            var weights = new Double[ndimensions + 1];
            using (var sim = new QuantumSimulator())
            {
                System.Console.WriteLine("\nRunning perceptron..");
                var (classical_register, tot) = Perceptron.Run(sim, superposition[0], superposition[1]).Result;
                int i = 0;
                foreach(int bit in classical_register){
                    System.Console.WriteLine($"bit {i}:\tOnes: {(double) bit / tot * 100}%\tZeroes: {(double)(tot - bit) / tot * 100}%");
                    i++;
                }
                weights[0] = 0;
                weights[1] = (double) classical_register[0] / tot;
                weights[2] = (double) classical_register[1] / tot;
            }

            SaveFile(weights);

        }

        private static void Superposition()
        {
            using (var sim = new QuantumSimulator())
            {
                System.Console.WriteLine("\nEsempio sovrapposizione..");
                var (first, second, tot) = SuperExample.Run(sim).Result;

                System.Console.WriteLine($"first bit:\tOnes: {(double)first / tot * 100}%\tZeroes: {(double)(tot - first) / tot * 100}%");
                System.Console.WriteLine($"second bit:\tOnes: {(double)second / tot * 100}%\tZeroes: {(double)(tot - second) / tot * 100}%");
            }
        }

        private static void IBMQuantumExperienceRun()
        {
            System.Console.WriteLine("\n\nRunning on IBM..");
            var apiKey = "YOUR_API_KEY..";
            var factory = new IbmQx5(apiKey); //Using different Factory
            var result = OneShotPerceptron.Run(factory).Result;
            System.Console.WriteLine($"Result of sim is {result}");
        }

        private static void SaveFile(double[] weights)
        {
            System.Console.WriteLine("\nSaving output..");
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
            System.Console.WriteLine("\nReading file.txt");
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
