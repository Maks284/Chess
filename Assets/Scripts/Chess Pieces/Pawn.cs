using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override List<Vector2Int> GetAvalibleMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> avalibleMove = new List<Vector2Int>();

        int direction = (team == 0) ? 1 : -1;

        if (board[currentX, currentY + direction] == null)
            avalibleMove.Add(new Vector2Int(currentX, currentY + direction));

        if (board[currentX, currentY + direction] == null)
        {
            if (team == 0 && currentY == 1 && board[currentX, currentY + (direction * 2)] == null)
                avalibleMove.Add(new Vector2Int(currentX, currentY + (direction * 2)));
            if (team == 1 && currentY == 6 && board[currentX, currentY + (direction * 2)] == null)
                avalibleMove.Add(new Vector2Int(currentX, currentY + (direction * 2)));
        }

        if (currentX != tileCountX - 1)
            if (board[currentX + 1, currentY + direction] != null && board[currentX + 1, currentY + direction].team != team)
                avalibleMove.Add(new Vector2Int(currentX + 1, currentY + direction));

        if (currentX != 0)
            if (board[currentX - 1, currentY + direction] != null && board[currentX - 1, currentY + direction].team != team)
                avalibleMove.Add(new Vector2Int(currentX - 1, currentY + direction));

        return avalibleMove;
    }
}
