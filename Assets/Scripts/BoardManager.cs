using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject squarePrefab;
    public GameObject[] piecePrefabs;
    public int boardSize = 8;
    public float squareSize = 1.0f;
    public GameObject[,] squares;
    public Material whiteSquareMaterial;
    public Material blackSquareMaterial;
    private ChessGameManager chessGameManager;

    private void Awake()
    {
        chessGameManager = GetComponent<ChessGameManager>();
        CreateAndInitializeSquares();
        PlacePieces();
    }

    private void CreateAndInitializeSquares()
    {
        squares = new GameObject[boardSize, boardSize];

        for (int x = 0; x < boardSize; x++)
        {
            for (int y = 0; y < boardSize; y++)
            {
                GameObject square = Instantiate(squarePrefab, new Vector3(x * squareSize, -0.75f, y * squareSize), Quaternion.identity, transform);
                square.name = $"Square {x} {y}";

                // Assign the appropriate material based on the square's position
                Material squareMaterial = (x + y) % 2 == 0 ? whiteSquareMaterial : blackSquareMaterial;
                square.GetComponent<Renderer>().material = squareMaterial;
                square.AddComponent<SquareControl>();
                square.AddComponent<GlowControl>();
                squares[x, y] = square;
            }
        }
    }


    private void PlacePieces()
    {
        // Implement piece placement logic here
    }

    public GameObject GetSquare(Vector2Int position)
    {
        return squares[position.x, position.y];
    }

    public void MovePiece(PieceControl piece, Vector2Int targetPosition)
    {
        Vector3 targetWorldPosition = GetSquare(targetPosition).transform.position;
        targetWorldPosition.y = piece.transform.position.y;
        piece.transform.position = targetWorldPosition;

        // Update the piece's internal position
        piece.BoardPosition = targetPosition;
    }


    public bool IsSquareEmpty(Vector2Int position)
    {
        if (!IsSquareWithinBounds(position))
        {
            return false;
        }

        GameObject square = squares[position.x, position.y];
        PieceControl piece = square.GetComponentInChildren<PieceControl>();

        return piece == null;
    }

    public bool IsSquareOccupiedByOpponent(Vector2Int position, bool isWhite)
    {
        if (!IsSquareWithinBounds(position))
        {
            return false;
        }

        PieceControl piece = GetPieceAt(position);

        return piece != null && piece.isWhite != isWhite;
    }


    public bool IsSquareWithinBounds(Vector2Int position)
    {
        return position.x >= 0 && position.x < 8 && position.y >= 0 && position.y < 8;
    }

    public PieceControl GetPieceAt(Vector2Int position)
    {
        if (!IsSquareWithinBounds(position))
        {
            return null;
        }

        GameObject square = squares[position.x, position.y];
        PieceControl piece = square.GetComponentInChildren<PieceControl>();

        return piece;
    }
}
