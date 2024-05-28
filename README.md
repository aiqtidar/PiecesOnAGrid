# PiecesOnAGrid
A console application to solve the simple problem of finding all possible digits that can be generated from pieces on a grid.

## Approach
There were two approaches used to solve this problem:
- The faster solution involves concepts from dynamic programming to build a matrix of possible moves, from the bottom up.
- The other solution involves using DFS to traverse the problem top-down, with memoization to speed up the solution.

Both approaches were implemented and the results from each was used to check the accuracy of the solution. At the end, both approaches yielded the same results for each piece - implying that the solutions were accurately implemented.
