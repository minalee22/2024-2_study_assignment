using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override MoveInfo[] GetMoves()
    {
        // --- TODO ---
        MoveInfo[] moves = new MoveInfo[4];
        moves[0] = new MoveInfo(1, 1, 7);
        moves[1] = new MoveInfo(1, -1, 7);
        moves[2] = new MoveInfo(-1, -1, 7);
        moves[3] = new MoveInfo(-1, 1, 7);
        return moves;
        // ------
    }
}