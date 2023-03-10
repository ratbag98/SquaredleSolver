# SquaredleSolver

## About this project

NOTE: abandoning this project and C# in general since MS have deliberately
hobbled VScode's C# environment. Seems like Embrace Extend Extinguish is
still a thing. Shame, the language is very pleasant and performs well.

With that in mind, go and check my [Python version](https://github.com/ratbag98/PySquaredle) instead.

NOTE: appalling C# code ahead - this was my very first C# program.

Noddy C# program to solve the daily [Squaredle puzzle](https://squaredle.app/)

The puzzle involve a grid of letters. You start from any letter in the grid
and then join them continuously and without repetition to make words of four
of more letters. You have to find all the "normal" words in the grid in order
to complete the puzzle. There are additional "bonus" words, generally less
common, and these are used to differentiate contestants on the high score table.

The puzzle is susceptible to a recursive word-list "attack" and this simple
C# project does exactly that.

## Getting Started

The project was built inside Visual Studio for Mac. If you're not using that
weird beast then I can't help you.

Clone the repo and add the solution to Visual Studio. Build (probably in
Release configuration) and away you go.). Find the executable (in
`./SquaredleSolver/SquaredleSolver/bin/Release/net7.0`) and put it somewhere
useful.

Specify the word list path with the `-w` option. Run the program with the
`--help` option for more details.

### Running the solver

We're going to use a grid that looks like:

```text
GAC
SNE
WID
```

So we run the solver with the letters, organised as a single string reading
the grid from left to right, top to bottom:

```bash
./SquaredleSolver GACSNEWID
```

(upper or lower-case, live a little).

The program will check that the letters can represent a Squaredle grid. There
should be a "square" number of letters (eg 3x3, 4x4, 5x5, etc). If the number
of letters is not square the program will quit.

Assuming you have entered a valid list of letters, the solver will come back
with all the valid words that can be made within the rules of Squaredle.

In this case the start of the results looks like:

```text
GANE
GAEN
GAED
GNIDE
ACNE
ACNED
ACED
ASWING
ASIDE
...
```

The ordering is primitive: I start with each letter in the list and recurse
depth-first from there. Once I've exhausted that letter it's on to the next
in the list you provided.

Note: the word list I'm using is hefty, but it still omits some valid words.
I'll endeavour to update it. If you've got a better word list you can
substitute it. You should trim short words from it (only include four letters
and more). I used:

```bash
rg -Nw '^[a-z]{4,}$' words.txt > ../SquaredleSolver/word_list.txt
```

to trim the list appropriately.

## Basic program logic

The board is represented as a character array. Most of the code uses
indexes into this array. A function is provided that maps between grid
coordinates and indexes.

The word list is stored as a "trie" which allows for efficient searching for
words that start with a set of letters.

The solution algorithm iterates over each letter in the character array. Using
the character it then recursively generates chains of letters. The next letter
in the chain is selected from the last item in the chain's "neighbours". For
example if we have a grid of letters:

```text
ABC
DEF
GHI
```

then the character array looks like:

```text
012345678
ABCDEFGHI
```

(the numbers are the index into the array).

We'll start with the first letter, A, index 0. To calculate the neighbours,
imagine this grid of indexes:

```text
012
345
678
```

So letter A, top-left in the grid, has three neighbours: 1,3 and 4. These
translate into the letters B, D and E. So the chain recursion routine will
check 01, 03, 04 in turn. It actually works depth first, but this shows the
flattened logic in the function itself.

The chain recursion will check chains like 0124, 0125, 0145, 0143, 01436 etc.

The chains are converted back to strings by dereferencing the puzzle character
array (ABCE, ABCF, ABEF, etc.). If the word is in the trie structure it gets added to the solution. If
there are words in the trie that start with the candidate "word" (string of
letters), then we keep searching down the chain (ie call the recursive function
with the chain and add new neighbours).

If there are no further words in the trie that start with our current chain's
letters then we go to the next neighbour and recurse into that chain.

The recursion unwinds as chains fail to match anything in the trie.

The recursion is called once per letter in the puzzle grid and once all the
letters have been checked the `HashSet` of solutions is returned.

A `HashSet` is used since the same word can be found in multiple ways sometimes.
Whilst this is interesting to know, it doesn't help solve the puzzle so
repetitions are dropped.

### Possible things to try in the code as mental exercises

- Pass the neighbour list in and avoid an iteration (chop item off local neighbour list as we go)
- The trie's logic is a little opaque, there may be an optimisation to avoid some recursion.
- Relax the uniqueness requirement, in case we want to show the solution in some graphical fashion on the grid and to include alternatives.
- Are Iterators appropriate for the character array? We might have one to iterate in a linear fashion and another to scan through the grid left-to-right, top-to-bottom?

## Roadmap

- [x] Configuration for `wordListPath`
- [x] Expose the board and neighbour list via command line flags
- [x] Optionally Group solutions by word length (as used in the puzzle)
- [x] Sort solutions alphabetically rather than recursively
- [ ] Optionally separate results for "common" vs "uncommon" words
- [x] Command-line help
- [ ] Command-line argument aggregation (`-dlnw path`)
- [ ] Keep word list up to date
- [ ] Some graphical pizazz to show word formation in the grid
- [ ] Live update of the search like in the movies
- [ ] Reverse the logic somewhat in order to generate puzzles

## Contributing

Not looking for contributions right now. This is a learning exercise as
I start my journey in C#. Once it's "finished" I'll be happy to accept
contributions.

## License

Distributed under the MIT Licence. See LICENSE for more information. No comments
about English vs American English spelling, thanks.

## Contact

Project link: <https://github.com/ratbag98/SquaredleSolver.git>

## Acknowledgements

- Trie code from: <https://github.com/AndrewMcShane/DevMakingSource>
- Word list from: <https://github.com/dwyl/english-words.git>
