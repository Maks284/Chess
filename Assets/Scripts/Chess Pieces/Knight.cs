using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
{
    public override List<Vector2Int> GetAvalibleMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> avalibleMove = new List<Vector2Int>();

        int x = currentX + 1;
        int y = currentY + 2;
        if (x < tileCountX && y < tileCountY)
            if (board[x, y] == null || board[x, y].team != team)
                avalibleMove.Add(new Vector2Int(x, y));

        x = currentX + 2;
        y = currentY + 1;
        if (x < tileCountX && y < tileCountY)
            if (board[x, y] == null || board[x, y].team != team)
                avalibleMove.Add(new Vector2Int(x, y));

        x = currentX - 1;
        y = currentY + 2;
        if (x >= 0 && y < tileCountY)
            if (board[x, y] == null || board[x, y].team != team)
                avalibleMove.Add(new Vector2Int(x, y));

        x = currentX - 2;
        y = currentY + 1;
        if (x >= 0 && y < tileCountY)
            if (board[x, y] == null || board[x, y].team != team)
                avalibleMove.Add(new Vector2Int(x, y));

        x = currentX + 1;
        y = currentY - 2;
        if (x < tileCountX && y >= 0)
            if (board[x, y] == null || board[x, y].team != team)
                avalibleMove.Add(new Vector2Int(x, y));


        x = currentX + 2;
        y = currentY - 1;
        if (x < tileCountX && y >= 0)
            if (board[x, y] == null || board[x, y].team != team)
                avalibleMove.Add(new Vector2Int(x, y));

        x = currentX - 1;
        y = currentY - 2;
        if (x >= 0 && y >= 0)
            if (board[x, y] == null || board[x, y].team != team)
                avalibleMove.Add(new Vector2Int(x, y));


        x = currentX - 2;
        y = currentY - 1;
        if (x >= 0 && y >= 0)
            if (board[x, y] == null || board[x, y].team != team)
                avalibleMove.Add(new Vector2Int(x, y));

        return avalibleMove;
    }
}
