// See https://aka.ms/new-console-template for more information

// Use a wordlist generated like:

// rg -Nw '^[a-z]{4,}$' words.txt > ../SquardleSolver/word_list.txt

// to ensure only four-letter or more.


using SquardleSolver;
using System.IO;
using System.Collections.Generic;
using System;

internal class Program
{
    private static string wordListPath = @"/Users/rob/dev/SquardleSolver/word_list.txt";

    private static void Main(string[] args)
    {
        if (args.Length > 0)
        {
            BoardSolver board = new BoardSolver(wordListPath, args[0].ToLower());
            Console.WriteLine(string.Join("\n", board.solveBoard()));
        }
        else
        {
            Console.WriteLine("Please provide a list of letters from a Squardle puzzle\nRead the puzzle from left to right, top to bottom.");
        }
    }
}