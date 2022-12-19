using SquardleSolver;

namespace SquardleTests;

[TestClass]
public class SquardleSolverTests
{
    private static String goodLetters = "ABCDEFGHI";

    private static string wordListPath = @"/Users/rob/dev/SquardleSolver/test_word_list.txt";

    [TestMethod]
    public void TestSizeCheckWithValidLetters()
    {
        BoardSolver board = new BoardSolver(wordListPath, goodLetters);
    }

    [TestMethod]
    public void TestSizeCheckWithInvalidLetters()
    {
        String badLetters = "ABCDE";

        BoardSolver board;

        Assert.ThrowsException<ArgumentException>( () => board = new BoardSolver(wordListPath, badLetters));


    }

    [TestMethod]
    public void TestBoardCorrectlyLoaded()
    {
        BoardSolver board = new BoardSolver(wordListPath, goodLetters);
        Assert.AreEqual("ABC\nDEF\nGHI\n", board.getBoard());
    }

    [TestMethod]
    public void TestNeighbourhoodBuilder()
    {
        BoardSolver board = new BoardSolver(wordListPath, goodLetters);
        Assert.AreEqual("1:3:4, 0:2:3:4:5, 1:4:5, \n0:1:4:6:7, 0:1:2:3:5:6:7:8, 1:2:4:7:8, \n3:4:7, 3:4:5:6:8, 4:5:7, \n",
                        board.getNeighbours());
    }


    [TestMethod]
    public void TestDictionaryFilters()
    {
        BoardSolver board = new BoardSolver(wordListPath, goodLetters);
        Assert.AreEqual(179763, board.wordListLength());

        board = new BoardSolver(wordListPath, "ABCDEFGHIJKLMNOP");
        Assert.AreEqual(334176, board.wordListLength());

    }

    [TestMethod]
    public void TestSolverCanFindMaxLengthWord()
    {
        BoardSolver board = new BoardSolver(wordListPath, "HTEZRONIOPAHMORP");
        List<String> solutions = board.solveBoard();
        Assert.AreEqual(true, solutions.Contains("ANTHROPOMORPHIZE"));
    }

    [TestMethod]
    public void TestSolverDoesntIncludeUnlinkedWords()
    {
        BoardSolver board = new BoardSolver(wordListPath, "HTEZRONIOPAHMORP");
        List<String> solutions = board.solveBoard();
        Assert.AreEqual(false, solutions.Contains("OPERA"));
        // OPERA is a valid english word, using only the letters in the
        // problem, but they aren't linked, so the word is not a valid solution
    }
}
