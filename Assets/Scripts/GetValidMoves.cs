using System.Collections.Generic;
using UnityEngine;

public static class GetValidMoves
{
    public static List<Vector2Int> GetMoves(PieceControl piece, BoardManager boardManager)
    {
        List<Vector2Int> possibleMoves = new List<Vector2Int>();

        switch (piece.pieceType)
        {
            case PieceType.Pawn:
                possibleMoves.AddRange(GetPawnMoves(piece, boardManager));
                break;
            case PieceType.Rook:
                possibleMoves.AddRange(GetRookMoves(piece, boardManager));
                break;
            case PieceType.Knight:
                possibleMoves.AddRange(GetKnightMoves(piece, boardManager));
                break;
            case PieceType.Bishop:
                possibleMoves.AddRange(GetBishopMoves(piece, boardManager));
                break;
            case PieceType.Queen:
                possibleMoves.AddRange(GetQueenMoves(piece, boardManager));
                break;
            case PieceType.King:
                possibleMoves.AddRange(GetKingMoves(piece, boardManager));
                break;
            default:
                break;
        }

        return possibleMoves;
    }

    private static bool IsMoveBlocked(Vector2Int destination, PieceControl piece, BoardManager boardManager)
    {
        PieceControl destinationPiece = boardManager.GetPieceAt(destination);
        if (destinationPiece == null)
        {
            return false;
        }
        return destinationPiece.isWhite != piece.isWhite;
    }

    private static List<Vector2Int> GetPawnMoves(PieceControl piece, BoardManager boardManager)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        int direction = piece.isWhite ? 1 : -1;
        Vector2Int forward = new Vector2Int(piece.BoardPosition.x, piece.BoardPosition.y + direction);

        if (boardManager.IsSquareEmpty(forward))
        {
            moves.Add(forward);

            // Check if pawn can move two squares forward from starting position
            Vector2Int twoSquaresForward = new Vector2Int(piece.BoardPosition.x, piece.BoardPosition.y + direction * 2);
            bool canMoveTwoSquares = piece.isWhite && piece.BoardPosition.y == 1 || !piece.isWhite && piece.BoardPosition.y == 6;
            if (canMoveTwoSquares && boardManager.IsSquareEmpty(twoSquaresForward))
            {
                moves.Add(twoSquaresForward);
            }
        }

        // Check if pawn can capture diagonally
        Vector2Int leftCapture = new Vector2Int(piece.BoardPosition.x - 1, piece.BoardPosition.y + direction);
        Vector2Int rightCapture = new Vector2Int(piece.BoardPosition.x + 1, piece.BoardPosition.y + direction);
        if (boardManager.IsSquareOccupiedByOpponent(leftCapture, piece.isWhite))
        {
            moves.Add(leftCapture);
        }
        if (boardManager.IsSquareOccupiedByOpponent(rightCapture, piece.isWhite))
        {
            moves.Add(rightCapture);
        }

        return moves;
    }

    private static List<Vector2Int> GetRookMoves(PieceControl piece, BoardManager boardManager)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int direction in directions)
        {
            for (int i = 1; i <= 7; i++)
            {
                Vector2Int newPosition = piece.BoardPosition + direction * i;

                if (!boardManager.IsSquareWithinBounds(newPosition))
                    break;

                PieceControl targetPiece = boardManager.GetPieceAt(newPosition);
                if (targetPiece != null)
                {
                    if (targetPiece.isWhite != piece.isWhite)
                        moves.Add(newPosition);
                    break;
                }
                else
                {
                    moves.Add(newPosition);
                }

                // check if the current square has a piece of the same color as the rook
                if (targetPiece != null && targetPiece.isWhite == piece.isWhite)
                {
                    break;
                }
            }
        }

        return moves;
    }



    private static List<Vector2Int> GetKnightMoves(PieceControl piece, BoardManager boardManager)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        Vector2Int[] possibleMoves = new Vector2Int[]
        {
        new Vector2Int(-2, -1), new Vector2Int(-2, 1),
        new Vector2Int(-1, -2), new Vector2Int(-1, 2),
        new Vector2Int(1, -2), new Vector2Int(1, 2),
        new Vector2Int(2, -1), new Vector2Int(2, 1)
        };

        foreach (Vector2Int move in possibleMoves)
        {
            Vector2Int newPosition = piece.BoardPosition + move;
            if (boardManager.IsSquareWithinBounds(newPosition))
            {
                PieceControl pieceAtNewPosition = boardManager.GetPieceAt(newPosition);
                if (pieceAtNewPosition == null || pieceAtNewPosition.isWhite != piece.isWhite)
                {
                    moves.Add(newPosition);
                }
            }
        }

        return moves;
    }

    private static List<Vector2Int> GetBishopMoves(PieceControl piece, BoardManager boardManager)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        // Diagonal up-right
        for (int i = 1; i < 8; i++)
        {
            Vector2Int newPosition = piece.BoardPosition + new Vector2Int(i, i);
            if (!AddMoveIfValid(piece, newPosition, boardManager, moves)) break;
        }

        // Diagonal up-left
        for (int i = 1; i < 8; i++)
        {
            Vector2Int newPosition = piece.BoardPosition + new Vector2Int(-i, i);
            if (!AddMoveIfValid(piece, newPosition, boardManager, moves)) break;
        }

        // Diagonal down-right
        for (int i = 1; i < 8; i++)
        {
            Vector2Int newPosition = piece.BoardPosition + new Vector2Int(i, -i);
            if (!AddMoveIfValid(piece, newPosition, boardManager, moves)) break;
        }

        // Diagonal down-left
        for (int i = 1; i < 8; i++)
        {
            Vector2Int newPosition = piece.BoardPosition + new Vector2Int(-i, -i);
            if (!AddMoveIfValid(piece, newPosition, boardManager, moves)) break;
        }

        return moves;
    }



    private static bool AddMoveIfValid(PieceControl piece, Vector2Int position, BoardManager boardManager, List<Vector2Int> moves)
    {
        if (boardManager.IsSquareWithinBounds(position))
        {
            PieceControl pieceAtPosition = boardManager.GetPieceAt(position);
            if (pieceAtPosition == null)
            {
                moves.Add(position);
            }
            else
            {
                if (pieceAtPosition.isWhite != piece.isWhite)
                {
                    moves.Add(position);
                }
                return true; // Stop searching in this direction, as there's a piece blocking the way
            }
        }
        else
        {
            return true; // Stop searching in this direction, as the position is out of bounds
        }

        return false;
    }

    private static List<Vector2Int> GetQueenMoves(PieceControl piece, BoardManager boardManager)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        // Get rook moves
        List<Vector2Int> rookMoves = GetRookMoves(piece, boardManager);
        moves.AddRange(rookMoves);

        // Get bishop moves
        List<Vector2Int> bishopMoves = GetBishopMoves(piece, boardManager);
        moves.AddRange(bishopMoves);

        return moves;
    }

    private static List<Vector2Int> GetKingMoves(PieceControl piece, BoardManager boardManager)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        Vector2Int[] directions = new Vector2Int[]
        {
        new Vector2Int(1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(1, 1),
        new Vector2Int(1, -1),
        new Vector2Int(-1, 1),
        new Vector2Int(-1, -1)
        };

        Vector2Int currentPosition = piece.BoardPosition;

        foreach (Vector2Int direction in directions)
        {
            Vector2Int newPosition = currentPosition + direction;

            if (boardManager.IsSquareWithinBounds(newPosition))
            {
                PieceControl pieceAtDestination = boardManager.GetPieceAt(newPosition);

                if (pieceAtDestination == null || pieceAtDestination.isWhite != piece.isWhite)
                {
                    moves.Add(newPosition);
                }
            }
        }

        return moves;
    }

}
