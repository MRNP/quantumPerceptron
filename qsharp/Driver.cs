using Microsoft.Quantum.Simulation.Core;
using Microsoft.Quantum.Simulation.Simulators;

namespace qsharp
{
    class Driver
    {
        static void Main(string[] args)
        {
using (var sim = new QuantumSimulator())
            {
                    System.Console.WriteLine("Grover..");
                    System.Console.WriteLine(Grover.Run(sim).Result);
            }

            System.Console.WriteLine("\n\nPress Enter to continue...\n\n");
            System.Console.ReadLine();
        }
    }
}