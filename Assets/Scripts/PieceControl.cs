using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PieceControl : MonoBehaviour, IPointerClickHandler
{
    public PieceType pieceType;
    public bool isWhite;
    public Vector2Int BoardPosition;

    private ChessGameManager gameManager;
    private GlowControl glowControl;

    private void Awake()
    {
        gameManager = FindObjectOfType<ChessGameManager>();
        glowControl = GetComponent<GlowControl>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // If no piece is selected yet, select this piece.
        if (gameManager.selectedPiece != this)
        {
            if (gameManager.selectedPiece != null) gameManager.selectedPiece.Deselect();
            gameManager.selectedPiece = this;
            glowControl.ToggleGlow(true);
        }
        // If this piece is already selected, deselect it.
        else if (gameManager.selectedPiece == this)
        {
            gameManager.selectedPiece = null;
            glowControl.ToggleGlow(false);
        }
    }

    public void Deselect()
    {
        glowControl.ToggleGlow(false);
    }

    public List<Vector2Int> GetPossibleMoves(BoardManager boardManager)
    {
        return GetValidMoves.GetMoves(this, boardManager);
    }
}
