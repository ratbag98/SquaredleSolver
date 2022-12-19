# SquardleSolver

NOTE: appalling C# code ahead - this was my very first C# program.

Noddy C# program to solve the daily Squardle puzzle (https://squaredle.app/)

Trie code from: https://github.com/AndrewMcShane/DevMakingSource
Word list from: https://github.com/dwyl/english-words.git

Usage:
```bash
./SquardleSolver gacsnewid
```

This will check that the letters can represent a Squardle grid. There 
should be a "square" number of letters (eg 3x3, 4x4, 5x5, etc). In 
this case the grid looks like:

```
GAC
SNE
WID
```

That is, the letters are read into the grid left to right, top to bottom.

If the numebr of letters is not square the program will quit.

Otherwise the grid is solved and the list of valid words printed to STDOUT.
