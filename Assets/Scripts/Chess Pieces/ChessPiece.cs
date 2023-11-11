using System.Collections.Generic;
using UnityEngine;

public enum ChessPieceType
{
    None = 0,
    Pawn = 1,
    Rook = 2,
    Knight = 3,
    Bishop = 4,
    Queen = 5,
    King = 6,
}

public class ChessPiece : MonoBehaviour
{
    public int team;
    public int currentX;
    public int currentY;
    public ChessPieceType type;

    private Vector3 _desiredPosition;
    private Vector3 _desiredScale = Vector3.one * 5;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, _desiredPosition, Time.deltaTime * 10); 
        transform.localScale = Vector3.Lerp(transform.localScale, _desiredScale, Time.deltaTime * 10);
    }

    public virtual List<Vector2Int> GetAvalibleMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    { 
        List<Vector2Int> avalibleMove = new List<Vector2Int>();

        avalibleMove.Add(new Vector2Int(3, 3));
        avalibleMove.Add(new Vector2Int(3, 4));
        avalibleMove.Add(new Vector2Int(4, 3));
        avalibleMove.Add(new Vector2Int(4, 4));

        return avalibleMove;
    }

    public virtual void SetPosition(Vector3 position, bool forse = false)
    {
        _desiredPosition = position;

        if (forse)
            transform.position = _desiredPosition;
    }

    public virtual void SetScale(Vector3 scale, bool forse = false)
    {
        _desiredScale = scale;

        if (forse)
            transform.localScale = _desiredScale;
    }
}
