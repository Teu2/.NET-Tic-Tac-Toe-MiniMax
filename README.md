# .NET Tic Tac Toe
Tic Tac Toe WPF application in C# .NET 6.0, MiniMax was implemented using the pseudocode below learned during part of an AI course.

**MiniMax Algorithm Pseudocode**
```
function  minimax( node, depth, maximizingPlayer ) is
    if depth = 0 or node is a terminal node then
        return the heuristic value of node
    if maximizingPlayer then
        value := −∞
        for each child of node do
            value := max( value, minimax( child, depth − 1, FALSE ) )
        return value
    else (* minimizing player *)
        value := +∞
        for each child of node do
            value := min( value, minimax( child, depth − 1, TRUE ) )
        return value
```
