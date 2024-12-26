using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Knight.cs
public class Knight : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        MoveInfo[] moves = new MoveInfo[8];
        moves[0] = new MoveInfo(1, 2, 1);
        moves[1] = new MoveInfo(2, 1, 1);
        moves[2] = new MoveInfo(2, -1, 1);
        moves[3] = new MoveInfo(1, -2, 1);
        moves[4] = new MoveInfo(-1, -2, 1);
        moves[5] = new MoveInfo(-2, -1, 1);
        moves[6] = new MoveInfo(-2, 1, 1);
        moves[7] = new MoveInfo(-1, 2, 1);
        return moves;
        // ------
    }
}