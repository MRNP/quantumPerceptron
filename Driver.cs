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
            List<int[]> weights;

            Boolean RunOnIBM = false;

            System.Console.WriteLine("\n************\nInizio progetto reti..");
            Superposition();

            System.Console.WriteLine("\nLeggo dati..");
            var nData = ReadFile(X, d);

            System.Console.WriteLine("\n\nCalcolo errore sui pesi..");
            int ρ_best;
            ExtimateErrors(X, d, nData, out ρ_best, out weights);

            if (RunOnIBM) IBMQuantumExperienceRun();

            double[] resulting_weights = CallQuantumCode(ndimensions, weights, ρ_best);

            System.Console.WriteLine("\nSaving output..");
            SaveFile(resulting_weights);

        }

        private static double[] CallQuantumCode(int ndimensions, List<int[]> weights, int ρ_best)
        {
            var resulting_weights = new double[ndimensions + 1];
            using (var sim = new QuantumSimulator())
            {
                System.Console.WriteLine("\nRunning perceptron..");
                var (classical_register, tot) = Perceptron.Run(sim, weights[ρ_best][1], weights[ρ_best][2]).Result;
                int i = 0;
                foreach (int bit in classical_register)
                {
                    System.Console.WriteLine($"bit {i}:\tOnes: {(double)bit / tot * 100}%\tZeroes: {(double)(tot - bit) / tot * 100}%");
                    i++;
                }
                resulting_weights[0] = 1;
                resulting_weights[1] = (double)classical_register[0] / tot;
                resulting_weights[2] = (double)classical_register[1] / tot;
            }

            return resulting_weights;
        }

        private static void ExtimateErrors(List<double[]> X, List<double> d, int nData, out int ρ_best, out List<int[]> weights)
        {
            var ρ_register = new List<double>();
            ρ_best = 0;
            var ρ_current = 0;
            weights = new List<int[]>();
            Initializeweigths(weights);

            foreach (var weight in weights)
            {
                var ρ = 0;
                var i = 0;
                foreach (var x in X)
                {
                    //1 is bias
                    var h = weight[0] + x[0] * weight[1] + x[1] * weight[2];
                    //Rosenblatt Criterion
                    if (d[i] * h > 0)
                    {
                        ρ += 1;
                    }
                    i++;
                }
                ρ_register.Add((double)ρ / nData);
                if (ρ_register[ρ_current] > ρ_register[ρ_best])
                {
                    ρ_best = ρ_current;
                }
                ρ_current++;
            }

            int bit = 0;
            foreach (var ρ in ρ_register)
            {
                System.Console.Write($"({weights[bit][0]},{weights[bit][1]},{weights[bit][2]}) ----> ");
                System.Console.WriteLine("{0:F2}%", ρ);
                bit++;
            }
        }

        private static void Initializeweigths(List<int[]> weights)
        {
            weights.Add(new int[] { 1, 0, 0 });
            weights.Add(new int[] { 1, 0, 1 });
            weights.Add(new int[] { 1, 1, 0 });
            weights.Add(new int[] { 1, 1, 1 });
        }

        private static void Superposition()
        {
            using (var sim = new QuantumSimulator())
            {
                System.Console.WriteLine("\nEsempio sovrapposizione..");
                var (classical_register, super_register, tot) = SuperExample.Run(sim).Result;

                var first = classical_register[0];
                var second = classical_register[1];
                System.Console.WriteLine($"first qbit:\tOnes: {(double)first / tot * 100}%\tZeroes: {(double)(tot - first) / tot * 100}%");
                System.Console.WriteLine($"second qbit:\tOnes: {(double)second / tot * 100}%\tZeroes: {(double)(tot - second) / tot * 100}%");
                System.Console.WriteLine($"00: {(double)super_register[0]/tot}%");
                System.Console.WriteLine($"01: {(double)super_register[1]/tot}%");
                System.Console.WriteLine($"10: {(double)super_register[2]/tot}%");
                System.Console.WriteLine($"11: {(double)super_register[3]/tot}%");
            }
        }

        private static void IBMQuantumExperienceRun()
        {
            System.Console.WriteLine("\n\nRunning on IBM..");
            var apiKey = "9d9f6b1830e29a6465e20c85a5475f9eac96c5c7e816baefa19b8e344e4b32905863b61e88cd6618b22de8861ec984596462237cd8c8627fbb8cf02f229fd8d0";
            var factory = new IbmQx5(apiKey); //Using different Factory
            var result = OneShotPerceptron.Run(factory).Result;
            System.Console.WriteLine($"Result of sim is {result}");
        }

        private static void SaveFile(double[] weights)
        {
            using (var w = new StreamWriter("output.txt"))
            {
                for (double x = -2.5; x <2.5; x+=0.1){
                var line = string.Format("{0},{1}", x, -weights[0]/weights[2] - weights[1]/weights[2]*x);
                w.WriteLine(line);
                w.Flush();
                }
            }
        }

        private static int ReadFile(List<double[]> X, List<double> d)
        {
            int numLines = 0;
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
                    numLines++;
                }
            }
            return numLines;
        }
    }
}
