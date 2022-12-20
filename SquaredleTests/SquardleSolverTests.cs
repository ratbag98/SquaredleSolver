using SquaredleSolver;

namespace SquaredleTests;

[TestClass]
public class SquaredleSolverTests
{
    private static String goodLetters = "DEFABCGHI";

    private static string wordListPath = @"/Users/rob/dev/SquaredleSolver/test_word_list.txt";

    [TestMethod]
    public void TestSizeCheckWithValidLetters()
    {
        PuzzleSolver puzzle = new PuzzleSolver(wordListPath, goodLetters);
    }

    [TestMethod]
    public void TestSizeCheckWithInvalidLetters()
    {
        String badLetters = "ABCDE";

        PuzzleSolver puzzle;

        Assert.ThrowsException<ArgumentException>(() => puzzle = new PuzzleSolver(wordListPath, badLetters));


    }

    [TestMethod]
    public void TestPuzzleCorrectlyLoaded()
    {
        PuzzleSolver puzzle = new PuzzleSolver(wordListPath, goodLetters);
        Assert.AreEqual("DEF\nABC\nGHI\n", puzzle.getPuzzleGrid());
    }

    [TestMethod]
    public void TestNeighbourhoodBuilder()
    {
        PuzzleSolver puzzle = new PuzzleSolver(wordListPath, goodLetters);
        Assert.AreEqual("1:3:4, 0:2:3:4:5, 1:4:5, \n0:1:4:6:7, 0:1:2:3:5:6:7:8, 1:2:4:7:8, \n3:4:7, 3:4:5:6:8, 4:5:7, \n",
                        puzzle.getNeighbours());
    }


    [TestMethod]
    public void TestDictionaryFilters()
    {
        PuzzleSolver puzzle = new PuzzleSolver(wordListPath, goodLetters);
        Assert.AreEqual(179763, puzzle.wordListLength());

        puzzle = new PuzzleSolver(wordListPath, "ABCDEFGHIJKLMNOP");
        Assert.AreEqual(334176, puzzle.wordListLength());

    }

    [TestMethod]
    public void TestSolverCanFindMaxLengthWord()
    {
        PuzzleSolver puzzle = new PuzzleSolver(wordListPath, "HTEZRONIOPAHMORP");
        puzzle.solvePuzzle();
        List<String> solutions = puzzle.getSolutions();
        Assert.AreEqual(true, solutions.Contains("ANTHROPOMORPHIZE"));
    }

    [TestMethod]
    public void TestSolverDoesntIncludeUnlinkedWords()
    {
        PuzzleSolver puzzle = new PuzzleSolver(wordListPath, "HTEZRONIOPAHMORP");
        puzzle.solvePuzzle();
        List<String> solutions = puzzle.getSolutions();
        Assert.AreEqual(false, solutions.Contains("OPERA"));
        // OPERA is a valid english word, using only the letters in the
        // problem, but they aren't linked, so the word is not a valid solution
    }

    [TestMethod]
    public void TestGettingSolutions()
    {
        PuzzleSolver puzzle = new PuzzleSolver(wordListPath, goodLetters);

        // haven't solved the puzzle yet!
        Assert.ThrowsException<InvalidOperationException>(() => puzzle.getSolutions());


        puzzle.solvePuzzle();
        var throwaway = puzzle.getSolutions();
    }

    [TestMethod]
    public void TestAlphabeticalSortSwitchedOff()
    {
        // top-left cell will be D, so unsorted should start D, sorted, A
        PuzzleSolver puzzle = new PuzzleSolver(wordListPath, goodLetters);
        puzzle.solvePuzzle();


        List<String> solutions = puzzle.getSolutions(false);

        Assert.IsTrue(solutions.First<String>().StartsWith("D"));
    }

    [TestMethod]
    public void TestAlphabeticalSortSwitchedOn()
    {
        // Arrange
        // top-left cell will be D, so unsorted should start D, sorted, A
        PuzzleSolver puzzle = new PuzzleSolver(wordListPath, goodLetters);
        puzzle.solvePuzzle();


        // Action
        List<String> solutions = puzzle.getSolutions(true);

        
        // Asserts

        // this pairs each item in the list with the next item
        // then compares the strings - the first item should always be less
        // than the second if the list is sorted
        var ordered = solutions.Zip(solutions.Skip(1), (a, b) => new { a, b })
                        .All(p => String.Compare(p.a, p.b) <0);

        Assert.IsTrue(ordered);
    }
}
