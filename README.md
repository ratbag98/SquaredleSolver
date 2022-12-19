# SquaredleSolver

## About this project

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
`./SquardleSolver/SquardleSolver/bin/Release/net7.0`) and put it somewhere
useful.

Currently you need to modify the `wordListPath` to point to the included
`word_list.txt` file.

TODO add configuration of `wordListPath`.

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
./SquardleSolver GACSNEWID
```

(upper or lower-case, live a little).

The program will check that the letters can represent a Squardle grid. There
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
rg -Nw '^[a-z]{4,}$' words.txt > ../SquardleSolver/word_list.txt
```

to trim the list appropriately.

## Roadmap

- [ ] Configuration for `wordListPath`
- [ ] Expose the board and neighbour list via command line flags
- [ ] Optionally Group solutions by word length (as used in the puzzle)
- [ ] Optionally separate results for "common" vs "uncommon" words
- [ ] Command-line help
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

Project link: <https://github.com/ratbag98/SquardleSolver.git>

## Acknowledgements

- Trie code from: <https://github.com/AndrewMcShane/DevMakingSource>
- Word list from: <https://github.com/dwyl/english-words.git>
