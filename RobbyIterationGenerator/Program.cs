using System;
using System.Diagnostics;
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
            Stopwatch timer = new Stopwatch();
            timer.Start();

            

            timer.Stop();
            Console.WriteLine($"\nGeneration has stopped. Time Elapsed: {timer.Elapsed}");

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
            int numActions = GetInputFromUser<Int32>("\nHow many actions is Robby allowed to take?", (string input, out Int32 output) => {
                return Int32.TryParse(input, out output) && output > 0;
            });

            int numTestGrids = GetInputFromUser<Int32>("\nHow many test grids should Robby be tested on?", (string input, out Int32 output) => {
                return Int32.TryParse(input, out output) && output > 0;
            });

            int gridSize = GetInputFromUser<Int32>("\nHow big should the test grids be?", (string input, out Int32 output) => {
                return Int32.TryParse(input, out output) && output > 0;
            });

            
            return Robby.CreateRobby();
        }
    }
}
