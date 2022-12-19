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
            string boardLetters = args[0].ToLower();

            double size = Math.Sqrt(Convert.ToDouble(boardLetters.Length));

            // check square-ness
            if (size % 1 == 0)
            {
                BoardSolver board = new BoardSolver(wordListPath, (int) size, boardLetters);

                Console.WriteLine("Board looks like:\n" + board.getBoard());
                Console.WriteLine("Neighbourhood:\n" + board.getNeighbours());

                board.solveBoard();
            } else
            {
                Console.WriteLine("Letters provided don't make a square board (eg 3x3, 4x4, etc).");
            }
        } else
        {
            Console.WriteLine("Please provide a list of letters from a Squardle puzzle\nRead the puzzle from left to right, top to bottom.");
        }
    }

    private static bool isSquare(double input)
    {
        double result = Math.Sqrt(input);
        return result % 1 == 0;
    }
}