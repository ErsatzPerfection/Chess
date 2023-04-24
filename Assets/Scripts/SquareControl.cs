using UnityEngine;
using UnityEngine.EventSystems;

public class SquareControl : MonoBehaviour, IPointerClickHandler
{
    private ChessGameManager gameManager;
    private BoardManager boardManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<ChessGameManager>();
        boardManager = FindObjectOfType<BoardManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // If a piece is selected
        if (gameManager.selectedPiece != null)
        {
            int x = Mathf.RoundToInt(transform.position.x / boardManager.squareSize);
            int y = Mathf.RoundToInt(transform.position.z / boardManager.squareSize);
            Vector2Int destination = new Vector2Int(x, y);

            // Check if the destination is a valid move for the selected piece
            if (gameManager.selectedPiece.GetPossibleMoves(boardManager).Contains(destination))
            {
                boardManager.MovePiece(gameManager.selectedPiece, destination);
                gameManager.selectedPiece.Deselect();
                gameManager.selectedPiece = null;
            }
        }
    }
}
