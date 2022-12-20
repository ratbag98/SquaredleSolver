/*
 * SquaredleSolver
 *
 * (c) 2022 Robert Rainthorpe
 *
 * Distributed under MIT licenc(s)e.
 *
 */

using SquaredleSolver;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

internal class Program
{
    private static string wordListPath = @"/Users/rob/dev/SquaredleSolver/word_list.txt";



    // Simple wrapper to call the solver.
    private static void Main(string[] args)
    {
        Options options = processArgs(args);

        // Console.WriteLine(options);



        PuzzleSolver board = new PuzzleSolver(options.path, options.letters);

        if (options.printGrid)
        {
            Console.WriteLine($"Puzzle grid:\n{board.getPuzzleGrid()}");
        }

        if (options.printNeighbourhood)
        {
            Console.WriteLine($"Neighbourhood:\n{board.getNeighbours()}");
        }

        // TODO should catch exception when letters are not square
        board.solvePuzzle();
        List<String> solutions = board.getSolutions(options.alphaSort, options.groupByLength);

        Console.WriteLine(string.Join("\n", solutions));

    }

    private static Options processArgs(String[] args)
    {
        List<String> argList = args.ToList<String>();

        Options options = new();

        // gradually remove items from front of args list
        while (argList.Count > 0)
        {
            String nextArg = argList.First<String>();
            if (nextArg == "--help" || nextArg == "-h")
            {
                help();
                Environment.Exit(-2);
            } else if (nextArg == "-a")
            {
                options.alphaSort = true;
                argList.RemoveAt(0);
            }
            else if (nextArg == "-g")
            {
                options.printGrid = true;
                argList.RemoveAt(0);
            }
            else if (nextArg == "-n")
            {
                options.printNeighbourhood = true;
                argList.RemoveAt(0);
            }
            else if (nextArg == "-l")
            {
                options.groupByLength = true;
                argList.RemoveAt(0);
            }
            else if (nextArg == "-d")
            {

                options.differentiate = true;
                argList.RemoveAt(0);
            }
            else if (nextArg == "-w")
            {
                argList.RemoveAt(0);

                if (argList.Count == 0)
                {
                    showOptions("Must provide a word list path with -w option");
                    Environment.Exit(-2);
                }

                options.path = argList.First<String>();
                if (options.path.StartsWith("-"))
                {
                    showOptions("Invalid word list path with -w option");
                    Environment.Exit(-2);
                }
                argList.RemoveAt(0);
            }
            else if (nextArg.StartsWith("-"))
            {
                showOptions("Unknown parameter");
                Environment.Exit(-2);
            }
            else if (Regex.IsMatch(nextArg, @"^[a-zA-Z]+$"))
            {
                options.letters = nextArg;
                argList.RemoveAt(0);

            }
        }

        // Must have some letters to play with
        if (options.letters == "")
        {
            showOptions("LETTERS not provided");
            Environment.Exit(-2);
        }
        return options;


    }

    private static void showOptions(String message)
    {
        // might get clever in future
        String programName = "SquaredleSolver";

        Console.WriteLine(message);

        Console.WriteLine("USAGE:\n");
        Console.WriteLine($"\t{programName} [OPTIONS] LETTERS");
        Console.WriteLine($"\t{programName} [OPTIONS] -w PATH LETTERS\n");
    }

    private static void help()
    {
        showOptions("");

        Console.WriteLine(
@"ARGS:
    <LETTERS>
        A string of letters. The length of the string must be a square number.
        For example 9, 16, 25 etc.

        The letters will form the puzzle's grid. So to produce the following
        puzzle grid you would specify ""ABCDEFGHI"" as the letters.

            ABC
            DEF
            GHI

    <PATH>
        A word list file.

OPTIONS:
    -a
        Sort solutions alphabetically, rather than ""how they emerged from the
        algorithm""

    -d
        Differentiate between common and uncommon words in the solution list.

    -g
        Print the puzzle grid before the solutions.

    -l
        Group the solutions by word length (default is unordered, ungrouped).
        Setting this option will automatically sort the list alphabetically.

    -n
        Print the neighbour list before the solution. Useful for debugging.
"
);
    }

    public struct Options
    {
        public Options()
        {
            path = wordListPath;
            printGrid = false;
            printNeighbourhood = false;
            groupByLength = false;
            differentiate = false;
            alphaSort = false;
            letters = "";

        }
        public String path;
        public Boolean printGrid;
        public Boolean printNeighbourhood;
        public Boolean groupByLength;
        public Boolean differentiate;
        public String letters;
        public Boolean alphaSort;

        public override String ToString()
        {
            return $"Word list path: {path}\nGrid: {printGrid}\nNeighbourhooud: {printNeighbourhood}\nLength group: {groupByLength}\nDifferentiate: {differentiate}\nLetters: {letters}\nAlpha Sort: {alphaSort}";

        }

    }
}
