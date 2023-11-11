using System.Collections.Generic;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    [SerializeField] private float _tileSize = 1.0f;
    [SerializeField] private float _yOffset = 0.01f;
    [SerializeField] private float _deathSize = 0.2f;
    [SerializeField] private float _deathSpacing = 0.2f;
    [SerializeField] private float _dragOffset = 1f;
    [SerializeField] private Material _tileMaterial;
    [SerializeField] private Material[] _teamMaterial;
    [SerializeField] private GameObject[] _prefabs;

    private const int TileCountX = 8;
    private const int TileCountY = 8;

    private ChessPiece[,] _chessPieces;
    private GameObject[,] _tiles;
    private Camera _camera;
    private Vector2Int _currenHover;
    private ChessPiece _currentlyDragging;
    private List<ChessPiece> _deadWhite = new List<ChessPiece>();
    private List<ChessPiece> _deadBlack = new List<ChessPiece>();
    private List<Vector2Int> _avalibleMoves = new List<Vector2Int>();
    private bool _isWhiteTurn;

    private void Awake()
    {
        _isWhiteTurn = true;
            
        GenerateAllTiles(_tileSize, TileCountX, TileCountY);
        SpawnAllChessPieces();
        PositionAllPieces();
    }

    private void Update()
    {
        if (!_camera)
        {
            _camera = Camera.main;
            return;
        }

        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, LayerMask.GetMask("Tile", "Hover", "Highlight")))
        {
            Vector2Int hitPosition = LookUpTilePosition(hit.transform.gameObject);

            if (_currenHover == -Vector2Int.one)
            {
                _currenHover = hitPosition;
                _tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            if (_currenHover != hitPosition)
            {
                _tiles[_currenHover.x, _currenHover.y].layer = (ContainsValidMove(ref _avalibleMoves, _currenHover)) ? LayerMask.NameToLayer("Highlight") : LayerMask.NameToLayer("Tile");
                _currenHover = hitPosition;
                _tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (_chessPieces[hitPosition.x, hitPosition.y] != null)
                {
                    if ((_chessPieces[hitPosition.x, hitPosition.y].team == 0 && _isWhiteTurn) || (_chessPieces[hitPosition.x, hitPosition.y].team == 1 && !_isWhiteTurn))
                    {
                        _currentlyDragging = _chessPieces[hitPosition.x, hitPosition.y];
                        _avalibleMoves = _currentlyDragging.GetAvalibleMoves(ref _chessPieces, TileCountX, TileCountY);
                        HighlightTiles();
                    }
                }
            }

            if (_currentlyDragging != null && Input.GetMouseButtonUp(0))
            {
                Vector2Int previousPosition = new Vector2Int(_currentlyDragging.currentX, _currentlyDragging.currentY);

                bool validMove = MoveTo(_currentlyDragging, hitPosition.x, hitPosition.y);

                if (!validMove)
                    _currentlyDragging.SetPosition(GetTileCenter(previousPosition.x, previousPosition.y));

                _currentlyDragging = null;
                RemoveHighlightTiles();

            }
        }
        else
        {
            if (_currenHover != -Vector2Int.one)
            {
                _tiles[_currenHover.x, _currenHover.y].layer = (ContainsValidMove(ref _avalibleMoves, _currenHover)) ? LayerMask.NameToLayer("Highlight") : LayerMask.NameToLayer("Tile");
                _currenHover = -Vector2Int.one;
            }

            if (_currentlyDragging && Input.GetMouseButtonUp(0))
            {
                _currentlyDragging.SetPosition(GetTileCenter(_currentlyDragging.currentX, _currentlyDragging.currentY));
                _currentlyDragging = null;
                RemoveHighlightTiles();
            }
        }

        if (_currentlyDragging)
        {
            Plane horizontalPlane = new Plane(Vector3.up, Vector3.up * _yOffset);
            float distance = 0.0f;
            if (horizontalPlane.Raycast(ray, out distance))
                _currentlyDragging.SetPosition(ray.GetPoint(distance) + Vector3.up * _dragOffset);

        }
    }

    private void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY)
    {
        _tiles = new GameObject[tileCountX, tileCountY];

        for (int x = 0; x < tileCountX; x++)
            for (int y = 0; y < tileCountY; y++)
                _tiles[x, y] = GenerateSingleTile(tileSize, x, y);
    }

    private GameObject GenerateSingleTile(float tileSize, int x, int y)
    {
        GameObject tileObject = new GameObject(string.Format("X:{0} Y:{1}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = _tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, 0, y * tileSize);
        vertices[1] = new Vector3(x * tileSize, 0, (y + 1) * tileSize);
        vertices[2] = new Vector3((x + 1) * tileSize, 0, y * tileSize);
        vertices[3] = new Vector3((x + 1) * tileSize, 0, (y + 1) * tileSize);

        int[] tris = new int[] {0, 1, 2, 1, 3, 2};

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        tileObject.layer = LayerMask.NameToLayer("Tile");
        tileObject.AddComponent<BoxCollider>();

        return tileObject;
    }

    private void SpawnAllChessPieces()
    {
        _chessPieces = new ChessPiece[TileCountX, TileCountY];

        int whiteTeam = 0, blackTeam = 1;

        _chessPieces[0, 0] = SpawnSinglePiece(ChessPieceType.Rook, whiteTeam);
        _chessPieces[1, 0] = SpawnSinglePiece(ChessPieceType.Knight, whiteTeam);
        _chessPieces[2, 0] = SpawnSinglePiece(ChessPieceType.Bishop, whiteTeam);
        _chessPieces[3, 0] = SpawnSinglePiece(ChessPieceType.Queen, whiteTeam);
        _chessPieces[4, 0] = SpawnSinglePiece(ChessPieceType.King, whiteTeam);
        _chessPieces[5, 0] = SpawnSinglePiece(ChessPieceType.Bishop, whiteTeam);
        _chessPieces[6, 0] = SpawnSinglePiece(ChessPieceType.Knight, whiteTeam);
        _chessPieces[7, 0] = SpawnSinglePiece(ChessPieceType.Rook, whiteTeam);

        for (int i = 0; i < TileCountX; i++)
            _chessPieces[i, 1] = SpawnSinglePiece(ChessPieceType.Pawn, whiteTeam);

        _chessPieces[0, 7] = SpawnSinglePiece(ChessPieceType.Rook, blackTeam);
        _chessPieces[1, 7] = SpawnSinglePiece(ChessPieceType.Knight, blackTeam);
        _chessPieces[2, 7] = SpawnSinglePiece(ChessPieceType.Bishop, blackTeam);
        _chessPieces[3, 7] = SpawnSinglePiece(ChessPieceType.Queen, blackTeam);
        _chessPieces[4, 7] = SpawnSinglePiece(ChessPieceType.King, blackTeam);
        _chessPieces[5, 7] = SpawnSinglePiece(ChessPieceType.Bishop, blackTeam);
        _chessPieces[6, 7] = SpawnSinglePiece(ChessPieceType.Knight, blackTeam);
        _chessPieces[7, 7] = SpawnSinglePiece(ChessPieceType.Rook, blackTeam);

        for (int i = 0; i < TileCountX; i++)
            _chessPieces[i, 6] = SpawnSinglePiece(ChessPieceType.Pawn, blackTeam);
    }
    
    private ChessPiece SpawnSinglePiece(ChessPieceType type, int team)
    {
        ChessPiece chessPiece = Instantiate(_prefabs[(int)type - 1], transform).GetComponent<ChessPiece>();

        chessPiece.type = type;
        chessPiece.team = team;
        chessPiece.GetComponent<MeshRenderer>().material = _teamMaterial[team];

        return chessPiece;
    }

    private void PositionAllPieces()
    {
        for (int x = 0; x < TileCountX; x++)
            for (int y = 0; y < TileCountY; y++)
                if (_chessPieces[x, y] != null)
                    PositionSinglePiece(x, y, true);
    }

    private void PositionSinglePiece(int x, int y, bool forse = false)
    {
        _chessPieces[x, y].currentX = x;
        _chessPieces[x, y].currentY = y;
        _chessPieces[x, y].SetPosition(GetTileCenter(x, y), forse);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        return new Vector3(x * _tileSize, _yOffset, y * _tileSize) + new Vector3(_tileSize / 2, 0, _tileSize / 2);
    }

    private void HighlightTiles()
    {
        for (int i = 0; i < _avalibleMoves.Count; i++)
            _tiles[_avalibleMoves[i].x, _avalibleMoves[i].y].layer = LayerMask.NameToLayer("Highlight");
    }

    private void RemoveHighlightTiles()
    {
        for (int i = 0; i < _avalibleMoves.Count; i++)
            _tiles[_avalibleMoves[i].x, _avalibleMoves[i].y].layer = LayerMask.NameToLayer("Tile");

        _avalibleMoves.Clear();
    }

    private bool ContainsValidMove(ref List<Vector2Int> moves, Vector2 pos)
    {
        for (int i = 0; i < moves.Count; i++)
            if (moves[i].x == pos.x && moves[i].y == pos.y)
                return true;

        return false;
    }

    private bool MoveTo(ChessPiece chessPiece, int x, int y)
    {
        if (!ContainsValidMove(ref _avalibleMoves, new Vector2(x, y)))
            return false;

        Vector2Int previousPosition = new Vector2Int(chessPiece.currentX, chessPiece.currentY);

        if (_chessPieces[x, y] != null)
        {
            ChessPiece otherChessPiece = _chessPieces[x, y];

            if (chessPiece.team == otherChessPiece.team)
                return false;

            if (otherChessPiece.team == 0)
            {
                if (otherChessPiece.type == ChessPieceType.King)
                    CheckMate(1);

                _deadWhite.Add(otherChessPiece);
                otherChessPiece.SetScale(Vector3.one * _deathSize);
                otherChessPiece.SetPosition(new Vector3(8 * _tileSize, _yOffset, -1 * _tileSize) 
                    + new Vector3(_tileSize / 2, 0, _tileSize / 2) + (Vector3.forward * _deathSpacing) * _deadWhite.Count); 
            }
            else
            {
                if (otherChessPiece.type == ChessPieceType.King)
                    CheckMate(0);

                _deadBlack.Add(otherChessPiece);
                otherChessPiece.SetScale(Vector3.one * _deathSize);
                otherChessPiece.SetPosition(new Vector3(-1 * _tileSize, _yOffset, 8 * _tileSize) 
                    + new Vector3(_tileSize / 2, 0, _tileSize / 2) + (Vector3.back * _deathSpacing) * _deadBlack.Count);
            }
        }

        _chessPieces[x, y] = chessPiece;
        _chessPieces[previousPosition.x, previousPosition.y] = null;

        PositionSinglePiece(x, y);

        _isWhiteTurn = !_isWhiteTurn;

        return true;
    }

    private Vector2Int LookUpTilePosition(GameObject hitInfos)
    {
        for (int x = 0; x < TileCountX; x++)
            for (int y = 0; y < TileCountY; y++)
                if (_tiles[x, y] == hitInfos)
                    return new Vector2Int(x, y);

        return -Vector2Int.one;
    }

    private void CheckMate(int team)
    {
        DisplayVictory(team);
    }

    private void DisplayVictory(int winningTeam)
    {
        
    }

    public void OnResetButtonClick()
    { 

    }

    public void OnExitButtonClick()
    {
        
    }
}
