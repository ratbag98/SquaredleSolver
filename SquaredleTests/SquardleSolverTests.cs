using SquaredleSolver;

namespace SquaredleTests;

[TestClass]
public class SquaredleSolverTests
{
    private static String goodLetters = "ABCDEFGHI";

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

        Assert.ThrowsException<ArgumentException>( () => puzzle = new PuzzleSolver(wordListPath, badLetters));


    }

    [TestMethod]
    public void TestPuzzleCorrectlyLoaded()
    {
        PuzzleSolver puzzle = new PuzzleSolver(wordListPath, goodLetters);
        Assert.AreEqual("ABC\nDEF\nGHI\n", puzzle.getPuzzleGrid());
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
        List<String> solutions = puzzle.puzzleSolutions();
        Assert.AreEqual(true, solutions.Contains("ANTHROPOMORPHIZE"));
    }

    [TestMethod]
    public void TestSolverDoesntIncludeUnlinkedWords()
    {
        PuzzleSolver puzzle = new PuzzleSolver(wordListPath, "HTEZRONIOPAHMORP");
        List<String> solutions = puzzle.puzzleSolutions();
        Assert.AreEqual(false, solutions.Contains("OPERA"));
        // OPERA is a valid english word, using only the letters in the
        // problem, but they aren't linked, so the word is not a valid solution
    }
}
