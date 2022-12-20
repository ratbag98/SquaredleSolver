using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SquaredleSolver
{
    public class PuzzleSolver
    {
        // array of letters in the puzzle, left to right, top to bottom
        private char[] puzzle;

        // length of a side of the puzzle board
        private int size;

        // array of four-letter and longer words
        private Trie words = new();


        // how many words in the dictionary
        // only really needed for tests
        private int wordCount;

        // array of neighbours of each letter. Each letter has a list of neighbour
        // indexes into the puzzle array
        public List<int>[] neighbours;

        // HashSet used since I wan't unique results
        private HashSet<String> solutions = new();

        // Main entry point for class. Construct a puzzle from a string of letters
        public PuzzleSolver(String wordListPath, String letters)
        {
            letters = letters.ToUpper();

            size = checkLettersSize(letters);

            readWordList(wordListPath);

            puzzle = new char[size * size];

            letters.CopyTo(puzzle);

            neighbours = new List<int>[puzzle.Length];

            for (int i = 0; i < neighbours.Length; i++)
            {
                neighbours[i] = new List<int>();
            }

            calculateNeighbours(ref neighbours);

            // Console.WriteLine("Puzzle looks like:\n" + getPuzzleGrid());
            // Console.WriteLine("Neighbourhood:\n" + getNeighbours());

        }



        // Letters must be square. If not, throw an exception.
        public int checkLettersSize(String letters)
        {
            double size = Math.Sqrt(Convert.ToDouble(letters.Length));

            // check square-ness
            if (size % 1 == 0)
            {
                return (int)size;
            }
            throw new ArgumentException("Number of letters should be 'square'. eg 3x3 = 9, 4x4=16.");
        }


        // Use a wordlist generated like:
        // rg -Nw '^[a-z]{4,}$' words.txt > ../SquaredleSolver/word_list.txt
        // to ensure only four-letter or more.
        public void readWordList(String path)
        {
            var reader = new StreamReader(File.OpenRead(path));

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (!String.IsNullOrEmpty(line) && line.Length <= size * size)
                {
                    wordCount++;
                    words.Put(line.ToUpper());
                }
            }

        }

        // Generate a list of valid words that abide by the rules of Squaredle,
        // in no particular order unless options are specified
        public List<String> puzzleSolutions(Boolean alphaSort = false, Boolean groupByLength = false)
        {

            // top-level is letter-by-letter as entered by user
            for (int index = 0; index < puzzle.Length; index++)
            {
                List<int> chain = new() {
                    index
                };

                checkLetterChains(chain);
            }
            return solutions.ToList();

        }


        // the recursive bit
        private void checkLetterChains(List<int> chain)
        {
            // shouldn't happen
            if (chain.Count > size * size)
            {
                return;
            }

            int cellIndex = chain.Last<int>();

            foreach (int neighbourIndex in neighbours[cellIndex])
            {
                // can only see neighbour once
                if (!chain.Contains(neighbourIndex))
                {
                    // add the next neighbour to the end of the chain
                    List<int> nextChain = chain.Concat<int>(new List<int>() { neighbourIndex }).ToList();

                    String candidate = wordFromChain(nextChain);

                    // we've got a match
                    if (words.Contains(candidate))
                    {
                        solutions.Add(candidate);
                    }

                    // worth carrying on with this chain?
                    if (words.StartsWith(candidate).Count > 0)
                    {
                        checkLetterChains(nextChain);
                    }
                }

            }


        }

        // convert chain of puzzle indexes into a "word" for lookup
        private String wordFromChain(List<int> chain)
        {
            var sb = new StringBuilder();

            foreach (var index in chain)
            {
                sb.Append(puzzle[index]);
            }
            return sb.ToString();
        }

        // check all surrounding cells for each cell in turn.
        // if they are "on the grid", add the index to the list of neighbours
        private void calculateNeighbours(ref List<int>[] neighbours)
        {
            // puzzle y coord
            for (int oy = 0; oy < size; oy++)
            {
                // puzzle x coord
                for (int ox = 0; ox < size; ox++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int x = -1; x <= 1; x++)
                        {
                            //Console.WriteLine($"Testing {a},{b},{x},{y}");
                            if (isValidNeighbour(ox, oy, x, y))
                            {
                                neighbours[idx(ox, oy)].Add(idx(ox + x, oy + y));
                            }
                        }
                    }
                }
            }
        }

        // neighbour within the puzzle grid and not the cell of interest
        private Boolean isValidNeighbour(int ox, int oy, int dx, int dy)
        {
            return oy + dy >= 0 &&
                ox + dx >= 0 &&
                oy + dy < size &&
                ox + dx < size &&
                !(dx == 0 && dy == 0);
        }


        // linear index to the referenced cell in the puzzle array
        private int idx(int x, int y)
        {
            return x + y * size;
        }


        // debug function to enable printing the neighbourhood
        public String getNeighbours()
        {
            var sb = new StringBuilder();

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    sb.Append(string.Join(":", neighbours[x + (y * size)]));
                    sb.Append(", ");

                }
                sb.Append("\n");

            }

            return sb.ToString();
        }

        // debug function to show grid based on letters entered
        public String getPuzzleGrid()
        {
            var sb = new StringBuilder();

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    sb.Append(puzzle[idx(x, y)]);
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }

        public int wordListLength() { return wordCount; }


    }
}
