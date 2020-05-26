using System;
using System.Collections.Generic;
using System.Linq;

namespace MontyHall
{
    internal class Program
    {
        private static readonly Door[] doors = new Door[3];
        private static readonly Random random = new Random();
        private static bool userWantsToChangeDoor;
        private static List<Simulation> simulations = new List<Simulation>();

        private static void Main(string[] args)
        {
            do
            {
                var numberOfSimulations = StartGame();

                SetUserWantsToChangeDoors();

                for (var i = 0; i < numberOfSimulations; i++) StartSimulation();

                var numberOfWins = simulations.Count(x => x.UserGuessedCorrectly);
                Console.WriteLine($"Number of wins: {numberOfWins}");

                simulations = new List<Simulation>();
            } while (true);
        }

        private static void StartSimulation()
        {
            CreateDoors();


            var userChosenDoor = random.Next(0, 3);
            PresenterOpenDoor(userChosenDoor);


            if (userWantsToChangeDoor)
            {
                var oldUserDoorChoice = userChosenDoor;
                do
                {
                    userChosenDoor = random.Next(0, 3);
                } while (userChosenDoor == oldUserDoorChoice || doors[userChosenDoor].PresenterOpened);
            }

            var userGuessedCorrectly = doors[userChosenDoor].IsCarDoor;
            simulations.Add(new Simulation
                {UserChangedDoor = userWantsToChangeDoor, UserGuessedCorrectly = userGuessedCorrectly});
        }

        private static void PresenterOpenDoor(int userChosenDoor)
        {
            var doorPresenterOpens = 0;
            do
            {
                doorPresenterOpens = random.Next(0, 3);
            } while (doors[doorPresenterOpens].IsCarDoor || doorPresenterOpens == userChosenDoor);

            doors[doorPresenterOpens].PresenterOpened = true;
        }

        private static void CreateDoors()
        {
            doors[0] = new Door();
            doors[1] = new Door();
            doors[2] = new Door();

            var doorWithCarNumber = random.Next(0, 3);

            doors[doorWithCarNumber].IsCarDoor = true;
        }

        private static int StartGame()
        {
            int numberOfSimulations;

            Console.WriteLine("Welcome to Monty Hall.");

            bool userEnterValidNumber;
            string userInput;
            do
            {
                Console.WriteLine("Enter a number between 1-1000");
                userInput = Console.ReadLine();
                userEnterValidNumber = int.TryParse(userInput, out numberOfSimulations);
            } while (!userEnterValidNumber || int.Parse(userInput) <= 0 || int.Parse(userInput) >= 1001);


            return numberOfSimulations;
        }

        private static void SetUserWantsToChangeDoors()
        {
            string userInput;

            do
            {
                Console.WriteLine("Do you want to change the door in the simulations? Y/N)");
                userInput = Console.ReadLine();
            } while (userInput != "y" && userInput != "Y" && userInput != "n" && userInput != "N");

            userWantsToChangeDoor = userInput == "y" || userInput == "Y";
        }
    }
}