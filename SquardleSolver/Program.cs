/*
 * SquaredleSolver
 * 
 * (c) 2022 Robert Rainthorpe
 * 
 * Distributed under MIT licenc(s)e.
 * 
 */

using SquardleSolver;
using System.IO;
using System.Collections.Generic;
using System;

internal class Program
{
    private static string wordListPath = @"/Users/rob/dev/SquardleSolver/word_list.txt";

    // Simple wrapper to call the solver.
    private static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            PuzzleSolver board = new PuzzleSolver(wordListPath, args[0].ToLower());
            Console.WriteLine(string.Join("\n", board.puzzleSolutions()));
        }
        else
        {
            Console.WriteLine("Please provide a list of letters from a Squardle puzzle\nRead the puzzle from left to right, top to bottom.");
        }
    }
}