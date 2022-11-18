# Project-RobbyTheRobot
> 420-510-DW Programming V

> Section 2

> By: Nolan Ganz, Vladyslav Berezhnyak, Matteo Robidoux

## Description
Robby is a robot who lives in a world consisting of a 10 x 10 grid, filled with cans. 
His job is to collect as many cans as possible, but the issue is that Robby isn't very smart
and as such he is not able to do it on his own. A Genetic Algorithm is used to evolve Robby
and to score his performance for a given amount of generations.

The selected theme for this project was Cookie Monster eating cookies in the same world.

## Prerequisites
Make sure you have the following installed:

    1. dotnet sdk 3.1


## How to run?
To start, you need to build the following projects in the root directory:

    $ dotnet build ./GeneticAlgorithm
    $ dotnet build ./RobbyTheRobot


Next, you need to evolve Robby (Cookie Monster) and generate the necessary files before running him through the visualizer.

In the root directory of the project:
    
    $ dotnet build ./RobbyIterationGenerator
    $ dotnet run ./RobbyIterationGenerator

After running this project, you will be prompted by the console to enter all the necessary parameters to evolve Robby for a number of generations that you will specify. The end result is a series of .txt files of every generation, Robby's so called evolution.

Next, you need to run the visualizer:

    $ dotnet build ./RobbyVisualizer
    $ dotnet run ./RobbyVisualizer

You should see Cookie Monster within a 10 x 10 grid. To make him start eating cookies, specify the directory where the .txt of the generations were saved by clicking the "Select Folder with Proper Data".

Now, Cookie Monster should run and eat cookies!
