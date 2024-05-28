# PiecesOnAGrid
A console application to solve the simple problem of finding all possible digits that can be generated from pieces on a grid.

## Approach
There were two approaches used to solve this problem:
- The faster of the two solutions involves using concepts from dynamic programming to build a matrix of possible moves, from the bottom up.
- The other solution involves using DFS to traverse the problem top-down, with memoization to speed up the search.

Both approaches were implemented in this project to allow us to double-check our results. At the end, the results from both approaches agreed with each other, implying that our approaches were implemented accurately.

## Design
- The architecture is simple, with no explicit data layer for persistent memory. Answers are calculated in-memory and printed to the console through a delegate.
- The code is built to be modular and to be able to be extended to custom pieces if necessary.
- Both the board and the accompanying pieces are defined in the config.json file.
