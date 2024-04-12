using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IPointerClickHandler
{
    public ColorPiece color;

    public enum ColorPiece
    {
        White,
        Black
    }
    public enum PieceType
    {
        Pawn,
        Foul,
        Tower,
        Knight,
        Queen,
        King
    }
    public ChessBoardManager boardManager;
    public Vector2Int pos;
    public PieceType type;

    public bool activate;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!activate)
        {
            return;
        }
        boardManager.currentPiece = this;
        boardManager.ResetBoard();
        activate = true;
        OutlineSquares();
    }

    public void OutlineSquares()
    {
        if (type == PieceType.Pawn)
        {
            PawnCheck();
        }
        else if (type == PieceType.Foul)
        {
            FoulCheck();
        }
        else if (type == PieceType.Tower)
        {
            TowerCheck();
        }
        else if (type == PieceType.Knight)
        {
            KnightCheck();
        }
        else if (type == PieceType.Queen)
        {
            TowerCheck();
            FoulCheck();
        }
        else if (type == PieceType.King)
        {
            for (int x = pos.x - 1; x < pos.x + 2; x++)
            {
                for (int y = pos.y - 1; y < pos.y + 2; y++)
                {
                    if (x < 0 || y < 0 || x >= boardManager.sizeChess || y >= boardManager.sizeChess ||
                        x == pos.x && y == pos.y || CheckPiecePresent(x, y))
                        continue;
                    boardManager.chessBoard[x, y].SetPossiblePos();
                }
            }
        }
    }

    private void PawnCheck()
    {
        if (color == ColorPiece.Black)
        {
            if (pos.y == boardManager.sizeChess - 2)
            {
                if (!CheckPiecePresent(pos.x, pos.y - 1) && boardManager.chessBoard[pos.x, pos.y - 1].piece == null)
                {
                    boardManager.chessBoard[pos.x, pos.y - 1].SetPossiblePos();
                    if (!CheckPiecePresent(pos.x, pos.y - 2) && boardManager.chessBoard[pos.x, pos.y - 2].piece == null)
                    {
                        boardManager.chessBoard[pos.x, pos.y - 2].SetPossiblePos();
                    }
                }
            }
            else
            {
                if (CheckPosInsideBoard(pos.x, pos.y - 1) && !CheckPiecePresent(pos.x, pos.y - 1) && boardManager.chessBoard[pos.x, pos.y - 1].piece == null)
                {
                    boardManager.chessBoard[pos.x, pos.y - 1].SetPossiblePos();
                }
                
            }
            if (CheckPosInsideBoard(pos.x + 1, pos.y - 1) && boardManager.chessBoard[pos.x + 1, pos.y - 1].piece &&
                boardManager.chessBoard[pos.x + 1, pos.y - 1].piece.color != color)
            {
                boardManager.chessBoard[pos.x + 1, pos.y - 1].SetPossiblePos();
            }
            if (CheckPosInsideBoard(pos.x - 1, pos.y - 1) && boardManager.chessBoard[pos.x - 1, pos.y - 1].piece &&
                boardManager.chessBoard[pos.x - 1, pos.y - 1].piece.color != color)
            {
                boardManager.chessBoard[pos.x - 1, pos.y - 1].SetPossiblePos();
            }
        }
        else
        {
            if (pos.y == 1)
            {
                if (!CheckPiecePresent(pos.x, pos.y + 1) && boardManager.chessBoard[pos.x, pos.y + 1].piece == null)
                {
                    boardManager.chessBoard[pos.x, pos.y + 1].SetPossiblePos();
                    if (!CheckPiecePresent(pos.x, pos.y + 2) && boardManager.chessBoard[pos.x, pos.y + 2].piece == null)
                    {
                        boardManager.chessBoard[pos.x, pos.y + 2].SetPossiblePos();
                    }
                }
            }
            else
            {
                if (CheckPosInsideBoard(pos.x, pos.y + 1) && !CheckPiecePresent(pos.x, pos.y + 1) && boardManager.chessBoard[pos.x, pos.y + 1].piece == null)
                {
                    boardManager.chessBoard[pos.x, pos.y + 1].SetPossiblePos();
                }

                
            }
            if (CheckPosInsideBoard(pos.x + 1, pos.y + 1) && boardManager.chessBoard[pos.x + 1, pos.y + 1].piece &&
                boardManager.chessBoard[pos.x + 1, pos.y + 1].piece.color != color)
            {
                boardManager.chessBoard[pos.x + 1, pos.y + 1].SetPossiblePos();
            }
            if (CheckPosInsideBoard(pos.x - 1, pos.y + 1) && boardManager.chessBoard[pos.x - 1, pos.y + 1].piece &&
                boardManager.chessBoard[pos.x - 1, pos.y + 1].piece.color != color)
            {
                boardManager.chessBoard[pos.x - 1, pos.y + 1].SetPossiblePos();
            }
        }
    }

    private void KnightCheck()
    {
        int x = pos.x;
        int y = pos.y;
        if (CheckPosInsideBoard(x + 2, y + 1) && !CheckPiecePresent(x + 2, y + 1))
        {
            boardManager.chessBoard[x + 2, y + 1].SetPossiblePos();
        }
        if (CheckPosInsideBoard(x - 2, y + 1) && !CheckPiecePresent(x - 2, y + 1))
        {
            boardManager.chessBoard[x - 2, y + 1].SetPossiblePos();
        }
        if (CheckPosInsideBoard(x - 2, y - 1) && !CheckPiecePresent(x - 2, y - 1))
        {
            boardManager.chessBoard[x - 2, y - 1].SetPossiblePos();
        }
        if (CheckPosInsideBoard(x + 2, y - 1) && !CheckPiecePresent(x + 2, y - 1))
        {
            boardManager.chessBoard[x + 2, y - 1].SetPossiblePos();
        }
        if (CheckPosInsideBoard(x + 1, y + 2) && !CheckPiecePresent(x + 1, y + 2))
        {
            boardManager.chessBoard[x + 1, y + 2].SetPossiblePos();
        }
        if (CheckPosInsideBoard(x - 1, y + 2) && !CheckPiecePresent(x - 1, y + 2))
        {
            boardManager.chessBoard[x - 1, y + 2].SetPossiblePos();
        }
        if (CheckPosInsideBoard(x - 1, y - 2) && !CheckPiecePresent(x - 1, y - 2))
        {
            boardManager.chessBoard[x - 1, y - 2].SetPossiblePos();
        }
        if (CheckPosInsideBoard(x + 1, y - 2) && !CheckPiecePresent(x + 1, y - 2))
        {
            boardManager.chessBoard[x + 1, y - 2].SetPossiblePos();
        }
    }

    private bool CheckPosInsideBoard(int x, int y)
    {
        if (x >= 0 && x < boardManager.sizeChess && y >= 0 && y < boardManager.sizeChess)
            return (true);
        return (false);
    }

    private void TowerCheck()
    {
        for (int x = pos.x; x >= 0 && boardManager.chessBoard[x, pos.y].piece == null; x--)
        {
            if (CheckPiecePresent(x, pos.y))
                break;
            boardManager.chessBoard[x, pos.y].SetPossiblePos();
        }
        for (int x = pos.x; x < boardManager.sizeChess && boardManager.chessBoard[x, pos.y].piece == null; x++)
        {
            if (CheckPiecePresent(x, pos.y))
                break;
            boardManager.chessBoard[x, pos.y].SetPossiblePos();
        }
        for (int y = pos.y; y >= 0 && boardManager.chessBoard[pos.x, y].piece == null; y--)
        {
            if (CheckPiecePresent(pos.x, y))
                break;
            boardManager.chessBoard[pos.x, y].SetPossiblePos();
        }
        for (int y = pos.y; y < boardManager.sizeChess && boardManager.chessBoard[pos.x, y].piece == null; y++)
        {
            if (CheckPiecePresent(pos.x, y))
                break;
            boardManager.chessBoard[pos.x, y].SetPossiblePos();
        }
    }

    private void FoulCheck()
    {
        int x = pos.x;
        int y = pos.y;
        while (x >= 0 && y >= 0 && boardManager.chessBoard[x, y].piece == null)
        {
            x--;
            y--;
            if (x < 0 || y < 0 || CheckPiecePresent(x, y))
                break;
            boardManager.chessBoard[x, y].SetPossiblePos();
        }
        x = pos.x;
        y = pos.y;
        while (x < boardManager.sizeChess && y < boardManager.sizeChess && boardManager.chessBoard[x, y].piece == null)
        {
            x++;
            y++;
            if (x >= boardManager.sizeChess || y >= boardManager.sizeChess || CheckPiecePresent(x, y))
                break;
            boardManager.chessBoard[x, y].SetPossiblePos();
        }
    }
    
    private bool CheckPiecePresent(int x, int y)
    {
        if (boardManager.chessBoard[x, y].piece)
        {
            if (boardManager.chessBoard[x, y].piece.color == color)
            {
                return (true);
            }

            return (false);
        }
        return (false);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
        boardManager.currentPlayer.score += (int) type;
    }
}
