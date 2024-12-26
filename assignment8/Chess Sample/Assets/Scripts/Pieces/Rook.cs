using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Rook.cs
public class Rook : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        MoveInfo[] moves = new MoveInfo[4];
        moves[0] = new MoveInfo(0, 1, 7);
        moves[1] = new MoveInfo(1, 0, 7);
        moves[2] = new MoveInfo(0, -1, 7);
        moves[3] = new MoveInfo(-1, 0, 7);
        return moves;
        // ------
    }
}
