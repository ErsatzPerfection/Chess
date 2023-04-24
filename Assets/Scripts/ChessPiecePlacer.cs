using UnityEngine;

public class ChessPiecePlacer : MonoBehaviour
{
    public GameObject kingPrefab;
    public GameObject queenPrefab;
    public GameObject rookPrefab;
    public GameObject bishopPrefab;
    public GameObject knightPrefab;
    public GameObject pawnPrefab;

    public Material blackMaterial;

    private int boardSize = 8;
    private float squareSize = 1.0f;

    void Start()
    {
        PlaceChessPieces();
    }

    void PlaceChessPieces()
    {
        for (int i = 0; i < boardSize; i++)
        {
            InstantiatePiece(pawnPrefab, 1, i, false);
            InstantiatePiece(pawnPrefab, 6, i, true);
        }

        InstantiatePiece(rookPrefab, 0, 0, false);
        InstantiatePiece(rookPrefab, 0, 7, false);
        InstantiatePiece(rookPrefab, 7, 0, true);
        InstantiatePiece(rookPrefab, 7, 7, true);

        InstantiatePiece(knightPrefab, 0, 1, false);
        InstantiatePiece(knightPrefab, 0, 6, false);
        InstantiatePiece(knightPrefab, 7, 1, true);
        InstantiatePiece(knightPrefab, 7, 6, true);

        InstantiatePiece(bishopPrefab, 0, 2, false);
        InstantiatePiece(bishopPrefab, 0, 5, false);
        InstantiatePiece(bishopPrefab, 7, 2, true);
        InstantiatePiece(bishopPrefab, 7, 5, true);

        InstantiatePiece(queenPrefab, 0, 3, false);
        InstantiatePiece(queenPrefab, 7, 3, true);

        InstantiatePiece(kingPrefab, 0, 4, false);
        InstantiatePiece(kingPrefab, 7, 4, true);
    }

    void InstantiatePiece(GameObject prefab, int row, int col, bool isBlack)
    {
        Vector3 position = new Vector3(col * squareSize, 0, row * squareSize);
        GameObject piece = Instantiate(prefab, position, Quaternion.identity);

        PieceControl pieceControl = piece.GetComponent<PieceControl>();
        pieceControl.isWhite = !isBlack;
        pieceControl.BoardPosition = new Vector2Int(col, row);
        pieceControl.pieceType = GetPieceType(prefab);

        if (isBlack)
        {
            ChangeMaterial(piece, blackMaterial);
            piece.transform.Rotate(0, 180, 0);
        }

        piece.transform.SetParent(transform);
    }

    PieceType GetPieceType(GameObject prefab)
    {
        if (prefab == kingPrefab) return PieceType.King;
        if (prefab == queenPrefab) return PieceType.Queen;
        if (prefab == rookPrefab) return PieceType.Rook;
        if (prefab == bishopPrefab) return PieceType.Bishop;
        if (prefab == knightPrefab) return PieceType.Knight;
        if (prefab == pawnPrefab) return PieceType.Pawn;
        return default(PieceType);
    }
    void ChangeMaterial(GameObject obj, Material material)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.material = material;
        }
    }
}
