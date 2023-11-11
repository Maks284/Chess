using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public override List<Vector2Int> GetAvalibleMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> avalibleMove = new List<Vector2Int>();

        if (currentX + 1 < tileCountX)
        {
            if (board[currentX + 1, currentY] == null)
                avalibleMove.Add(new Vector2Int(currentX + 1, currentY));
            else if (board[currentX + 1, currentY].team != team)
                avalibleMove.Add(new Vector2Int(currentX + 1, currentY));

            if (currentY + 1 < tileCountY)
                if (board[currentX + 1, currentY + 1] == null)
                    avalibleMove.Add(new Vector2Int(currentX + 1, currentY + 1));   
                else if (board[currentX + 1, currentY + 1].team != team)
                    avalibleMove.Add(new Vector2Int(currentX + 1, currentY + 1));

            if (currentY - 1 >= 0)
                if (board[currentX + 1, currentY - 1] == null)
                    avalibleMove.Add(new Vector2Int(currentX + 1, currentY - 1));
                else if (board[currentX + 1, currentY - 1].team != team)
                    avalibleMove.Add(new Vector2Int(currentX + 1, currentY - 1));
        }

        if (currentX - 1 >= 0)
        {
            if (board[currentX - 1, currentY] == null)
                avalibleMove.Add(new Vector2Int(currentX - 1, currentY));
            else if (board[currentX - 1, currentY].team != team)
                avalibleMove.Add(new Vector2Int(currentX - 1, currentY));

            if (currentY + 1 < tileCountY)
                if (board[currentX - 1, currentY + 1] == null)
                    avalibleMove.Add(new Vector2Int(currentX - 1, currentY + 1));
                else if (board[currentX - 1, currentY + 1].team != team)
                    avalibleMove.Add(new Vector2Int(currentX - 1, currentY + 1));

            if (currentY - 1 >= 0)
                if (board[currentX - 1, currentY - 1] == null)
                    avalibleMove.Add(new Vector2Int(currentX - 1, currentY - 1));
                else if (board[currentX - 1, currentY - 1].team != team)
                    avalibleMove.Add(new Vector2Int(currentX - 1, currentY - 1));
        }

        if (currentY + 1 < tileCountY)
        {
            if (board[currentX , currentY + 1] == null)
                avalibleMove.Add(new Vector2Int(currentX, currentY + 1));
            else if (board[currentX , currentY + 1].team != team)
                avalibleMove.Add(new Vector2Int(currentX, currentY + 1));
        }

        if (currentY - 1 >= 0)
        {
            if (board[currentX, currentY - 1] == null)
                avalibleMove.Add(new Vector2Int(currentX, currentY - 1));
            else if (board[currentX, currentY - 1].team != team)
                avalibleMove.Add(new Vector2Int(currentX , currentY - 1));
        }

        return avalibleMove;
    }
}
