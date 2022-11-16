using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
<<<<<<< HEAD
=======
using System.Threading.Tasks;
>>>>>>> 12e437be16da65ebcb0970848bf7ff83bb41f467
using RobbyTheRobot;

namespace RobbyIterationGenerator
{
    //Delegate that returns true if the conversion was successful
    delegate bool Converter<E>(string input, out E output);
    class Program
    {
        private static IRobbyTheRobot robby;
        static void Main(string[] args)
        {
<<<<<<< HEAD
            Console.WriteLine("***ROBBY ITERATION GENERATION***");
=======
>>>>>>> 12e437be16da65ebcb0970848bf7ff83bb41f467
            Stopwatch timer = new Stopwatch();
            timer.Start();

            robby = MakeRobby();

<<<<<<< HEAD
            string path = GetInputFromUser<string>("Please enter the folder where you would like to store the output files.", (string input, out string output) => {
                output = input;
                if (new Regex("^[0-9a-zA-Z_:\\-\\\\\\.\\/\\s]+$").Match(input).Success)
=======
            //TODO -> Add the file event listener

            string path = GetInputFromUser<string>("Please enter the folder where you would like to store the output files.", (string input, out string output) => {
                output = input;
                if (new Regex("^[0-9a-zA-Z_\\-\\\\\\.\\/\\s]+$").Match(input).Success)
>>>>>>> 12e437be16da65ebcb0970848bf7ff83bb41f467
                {
                    try {
                        Directory.CreateDirectory(input);
                        return true;
                    } 
                    catch (Exception e)
                    {
                        return false;
                    }
                }
                return false;
            });
<<<<<<< HEAD

            robby.FileWrittenEvent += new FileHandler((metadata) => {
                Console.WriteLine("\n" + metadata);
            });

            Console.WriteLine("\n[Press ctrl + c to stop at any time]");
            
            robby.GeneratePossibleSolutions(path);

            timer.Stop();
            Console.WriteLine($"\nGeneration has stopped. Time Elapsed: {timer.Elapsed}");
=======
            
            Task thread = Task.Run(() => {
                robby.GeneratePossibleSolutions(path);
            });

            bool running = true;

            while (running)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                
                if (key == ConsoleKey.Escape)
                {
                    running = false;
                }
            }

            //TODO stop the running task


            //MAYBE TODO -> Remove the file event listener?

            timer.Stop();
            Console.WriteLine($"\nGeneration has stopped. Time Elapsed: {timer.Elapsed}");

>>>>>>> 12e437be16da65ebcb0970848bf7ff83bb41f467
        }

        /// <summary>
        /// Ask the user for a given input until is valid
        /// </summary>
        /// <return>The input converted to the correct type</treturn>
        private static E GetInputFromUser<E>(string message, Converter<E> converter)
        {
            Console.WriteLine(message);
            string input = Console.ReadLine();
            E output;
            while(!converter(input, out output))
            {
                Console.WriteLine("\nInvalid input, please try again");
                input = Console.ReadLine();
            }
            return output;
        }

        private static IRobbyTheRobot MakeRobby()
        {
            //TODO 
            //Make sure the validation is good
            //Input the retrieved values into the Robby creator
            //Hard code the other values inside Robby creator (length/number of genes, grid size, etc.)
<<<<<<< HEAD
            int numActions = GetInputFromUser<Int32>("\nHow many actions is Robby allowed to take? [Minimum >= 10]", (string input, out Int32 output) => {
                return Int32.TryParse(input, out output) && output >= 10;
            });

            int numTestGrids = GetInputFromUser<Int32>("\nHow many test grids should Robby be tested on? [Minimum >= 1]", (string input, out Int32 output) => {
                return Int32.TryParse(input, out output) && output > 0;
            });

            int gridSize = GetInputFromUser<Int32>("\nWhat should be the size of both dimensions of the grid? [Minimum >= 10]", (string input, out Int32 output) => {
                return Int32.TryParse(input, out output) && output >= 10;
            });

            int populationSize = GetInputFromUser<Int32>("\nHow many Robbys should there be per generation? [Minimum >= 10]", (string input, out Int32 output) => {
                return Int32.TryParse(input, out output) && output >= 10;
            });

            int numGenerations = GetInputFromUser<Int32>("\nHow many generations of Robby should there be? [Minimum >= 1]", (string input, out Int32 output) => {
                return Int32.TryParse(input, out output) && output > 0;
            });

            double mutationRate = GetInputFromUser<Double>("\nWhat mutation rate would you like? [Minimum > 0 , maximum < 1]", (string input, out Double output) => {
                return Double.TryParse(input, out output) && output >= 0 && output <= 1;
            });

            double eliteRate = GetInputFromUser<Double>("\nWhat elite rate would you like? [Minimum > 0 , maximum < 1]", (string input, out Double output) => {
                return Double.TryParse(input, out output) && output >= 0 && output <= 1;
            });

            return Robby.CreateRobby(numActions, numTestGrids, gridSize, numGenerations, mutationRate, eliteRate, populationSize);
=======
            int numActions = GetInputFromUser<Int32>("\nHow many actions is Robby allowed to take?", (string input, out Int32 output) => {
                return Int32.TryParse(input, out output) && output > 0;
            });

            int numTestGrids = GetInputFromUser<Int32>("\nHow many test grids should Robby be tested on?", (string input, out Int32 output) => {
                return Int32.TryParse(input, out output) && output > 0;
            });

            int populationSize = GetInputFromUser<Int32>("\nHow many Robbys should there be per generation?", (string input, out Int32 output) => {
                return Int32.TryParse(input, out output) && output > 0;
            });

            int numGenerations = GetInputFromUser<Int32>("\nHow many generations of Robby should there be?", (string input, out Int32 output) => {
                return Int32.TryParse(input, out output) && output > 0;
            });

            double eliteRate = GetInputFromUser<Double>("\nWhat elite rate would you like?", (string input, out Double output) => {
                return Double.TryParse(input, out output) && output >= 0 && output <= 1;
            });

            double mutationRate = GetInputFromUser<Double>("\nWhat mutation rate would you like?", (string input, out Double output) => {
                return Double.TryParse(input, out output) && output >= 0 && output <= 1;
            });

            return Robby.CreateRobby();
>>>>>>> 12e437be16da65ebcb0970848bf7ff83bb41f467
        }
    }
}
