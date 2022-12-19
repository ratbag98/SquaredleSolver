using System;
using System.Collections.Generic;
using System.Text;

namespace SquardleSolver
{
    public class BoardSolver
    {
        // length of a side of the board
        private int size;

        // array of four-letter and longer words
        private Trie words = new();

        private HashSet<String> solutions = new();


        // array of letters in the board, left to right, top to bottom
        public char[] Board { get; }

        private int wordCount;


        // array of neighbours of each letter. Each letter has a list of neighbour
        // indexes into the Board array
        public List<int>[] neighbours;

        // Main entry point for class. Construct a board from a string of letters
        public BoardSolver(String wordListPath, String letters)
        {
            letters = letters.ToUpper();

            size = checkLettersSize(letters);

            readWordList(wordListPath);

            Board = new char[size * size];

            letters.CopyTo(Board);

            neighbours = new List<int>[Board.Length];

            for (int i = 0; i < neighbours.Length; i++)
            {
                neighbours[i] = new List<int>();
            }

            calculateNeighbours(ref neighbours);

            // Console.WriteLine("Board looks like:\n" + getBoard());
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

        public List<String> solveBoard()
        {

            for (int i = 0; i < Board.Length; i++)
            {
                findWords(i);
            }
            return solutions.ToList();

        }

        // setup top-level search for a given cell
        private void findWords(int index)
        {
            List<int> chain = new()
            {
                index
            };

            recurse(chain, 0);
        }

        private void recurse(List<int> chain, int wordIndex)
        {
            // shouldn't happen
            if (chain.Count > size * size)
            {
                return;
            }

            List<int> currentChain = chain;

            int cellIndex = chain.Last<int>();

            foreach (var neighbourIndex in neighbours[cellIndex])
            {
                // can only see neighbour once
                if (!chain.Contains(neighbourIndex))
                {
                    List<int> nextChain = chain.Concat<int>(new List<int>() { neighbourIndex }).ToList();

                    String candidate = wordFromChain(nextChain);

                    if (words.Contains(candidate))
                    {
                        solutions.Add(candidate);
                    }

                    if (words.StartsWith(candidate).Count > 0)
                    {
                        recurse(nextChain, 0);
                    }
                    // no point recursing if nothing starts with these letters
                }

            }


        }

        // convert chain of board indexes into a "word" for lookup
        private String wordFromChain(List<int> chain)
        {
            var sb = new StringBuilder();

            foreach (var index in chain)
            {
                sb.Append(Board[index]);
            }
            return sb.ToString();
        }

        // check all surrounding cells for each cell in turn.
        // if they are "on the grid", add the index to the list of neighbours
        private void calculateNeighbours(ref List<int>[] neighbours)
        {
            // board y coord
            for (int b = 0; b < size; b++)
            {
                // board x coord
                for (int a = 0; a < size; a++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        for (int x = -1; x <= 1; x++)
                        {
                            //Console.WriteLine($"Testing {a},{b},{x},{y}");
                            if (isValidNeighbour(a, b, x, y))
                            {
                                neighbours[idx(a, b)].Add(idx(a + x, b + y));
                            }
                        }
                    }
                }
            }
        }

        // neighbour within the board and not the cell of interest
        private Boolean isValidNeighbour(int ox, int oy, int dx, int dy)
        {
            return oy + dy >= 0 &&
                ox + dx >= 0 &&
                oy + dy < size &&
                ox + dx < size &&
                !(dx == 0 && dy == 0);
        }


        // linear index to the referenced cell in the board array
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

        // debug function to show board based on letters entered
        public String getBoard()
        {
            var sb = new StringBuilder();

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    sb.Append(Board[idx(x, y)]);
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }

        public int wordListLength() { return wordCount;  }


    }
}

