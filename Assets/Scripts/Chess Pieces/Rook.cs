using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
{
    public override List<Vector2Int> GetAvalibleMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> avalibleMove = new List<Vector2Int>();

        for (int i = currentY - 1; i >= 0; i--)
        {
            if (board[currentX, i] == null)
                avalibleMove.Add(new Vector2Int(currentX, i));

            if (board[currentX, i] != null)
            {
                if(board[currentX, i].team != team)
                    avalibleMove.Add(new Vector2Int(currentX, i));

                break;
            }
        }

        for (int i = currentY + 1; i < tileCountY; i++)
        {
            if (board[currentX, i] == null)
                avalibleMove.Add(new Vector2Int(currentX, i));

            if (board[currentX, i] != null)
            {
                if(board[currentX, i].team != team)
                    avalibleMove.Add(new Vector2Int(currentX, i));

                break;
            }
        }

        for (int i = currentX - 1; i >= 0; i--)
        {
            if (board[i, currentY] == null)
                avalibleMove.Add(new Vector2Int(i, currentY));

            if (board[i, currentY] != null)
            {
                if(board[i, currentY].team != team)
                    avalibleMove.Add(new Vector2Int(i, currentY));

                break;
            }
        }

        for (int i = currentX + 1; i < tileCountX; i++)
        {
            if (board[i, currentY] == null)
                avalibleMove.Add(new Vector2Int(i, currentY));

            if (board[i, currentY] != null)
            {
                if(board[i, currentY].team != team)
                    avalibleMove.Add(new Vector2Int(i, currentY));

                break;
            }
        }

        return avalibleMove;
    }   
}
