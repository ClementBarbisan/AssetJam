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
        Pawn = 2,
        Foul = 4,
        Tower = 6,
        Knight = 8,
        Queen = 10,
        King = 12
    }
    public Vector2Int pos;
    public PieceType type;
    private bool _activate;
    public bool activate
    {
        get
        {
            return _activate;
        }
        set
        {
            _activate = value;
            if (_activate)
            {
                anim.Play("RotPiece");
                anim["RotPiece"].speed = 1;
            }
            else
            {
                anim["RotPiece"].normalizedTime = 0;
                anim["RotPiece"].speed = 0;
            }
        }
    }

    private bool ping;
    private Animation anim;
   
    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!activate)
        {
            return;
        }
        ChessBoardManager.Instance.currentPiece = this;
        ChessBoardManager.Instance.ResetBoard();
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
                    if (x < 0 || y < 0 || x >= ChessBoardManager.Instance.sizeChess || y >= ChessBoardManager.Instance.sizeChess ||
                        x == pos.x && y == pos.y || CheckPiecePresent(x, y))
                        continue;
                    ChessBoardManager.Instance.chessBoard[x, y].SetPossiblePos();
                }
            }
        }
    }

    private void PawnCheck()
    {
        if (color == ColorPiece.Black)
        {
            if (pos.y == ChessBoardManager.Instance.sizeChess - 2)
            {
                if (!CheckPiecePresent(pos.x, pos.y - 1) && ChessBoardManager.Instance.chessBoard[pos.x, pos.y - 1].piece == null)
                {
                    ChessBoardManager.Instance.chessBoard[pos.x, pos.y - 1].SetPossiblePos();
                    if (!CheckPiecePresent(pos.x, pos.y - 2) && ChessBoardManager.Instance.chessBoard[pos.x, pos.y - 2].piece == null)
                    {
                        ChessBoardManager.Instance.chessBoard[pos.x, pos.y - 2].SetPossiblePos();
                    }
                }
            }
            else
            {
                if (CheckPosInsideBoard(pos.x, pos.y - 1) && !CheckPiecePresent(pos.x, pos.y - 1))
                {
                    ChessBoardManager.Instance.chessBoard[pos.x, pos.y - 1].SetPossiblePos();
                }
                
            }
            if (CheckPosInsideBoard(pos.x + 1, pos.y - 1) && ChessBoardManager.Instance.chessBoard[pos.x + 1, pos.y - 1].piece &&
                ChessBoardManager.Instance.chessBoard[pos.x + 1, pos.y - 1].piece.color != color)
            {
                ChessBoardManager.Instance.chessBoard[pos.x + 1, pos.y - 1].SetPossiblePos();
            }
            if (CheckPosInsideBoard(pos.x - 1, pos.y - 1) && ChessBoardManager.Instance.chessBoard[pos.x - 1, pos.y - 1].piece &&
                ChessBoardManager.Instance.chessBoard[pos.x - 1, pos.y - 1].piece.color != color)
            {
                ChessBoardManager.Instance.chessBoard[pos.x - 1, pos.y - 1].SetPossiblePos();
            }
        }
        else
        {
            if (pos.y == 1)
            {
                if (!CheckPiecePresent(pos.x, pos.y + 1) && ChessBoardManager.Instance.chessBoard[pos.x, pos.y + 1].piece == null)
                {
                    ChessBoardManager.Instance.chessBoard[pos.x, pos.y + 1].SetPossiblePos();
                    if (!CheckPiecePresent(pos.x, pos.y + 2) && ChessBoardManager.Instance.chessBoard[pos.x, pos.y + 2].piece == null)
                    {
                        ChessBoardManager.Instance.chessBoard[pos.x, pos.y + 2].SetPossiblePos();
                    }
                }
            }
            else
            {
                if (CheckPosInsideBoard(pos.x, pos.y + 1) && !CheckPiecePresent(pos.x, pos.y + 1))
                {
                    ChessBoardManager.Instance.chessBoard[pos.x, pos.y + 1].SetPossiblePos();
                }

                
            }
            if (CheckPosInsideBoard(pos.x + 1, pos.y + 1) && ChessBoardManager.Instance.chessBoard[pos.x + 1, pos.y + 1].piece &&
                ChessBoardManager.Instance.chessBoard[pos.x + 1, pos.y + 1].piece.color != color)
            {
                ChessBoardManager.Instance.chessBoard[pos.x + 1, pos.y + 1].SetPossiblePos();
            }
            if (CheckPosInsideBoard(pos.x - 1, pos.y + 1) && ChessBoardManager.Instance.chessBoard[pos.x - 1, pos.y + 1].piece &&
                ChessBoardManager.Instance.chessBoard[pos.x - 1, pos.y + 1].piece.color != color)
            {
                ChessBoardManager.Instance.chessBoard[pos.x - 1, pos.y + 1].SetPossiblePos();
            }
        }
    }

    private void KnightCheck()
    {
        int x = pos.x;
        int y = pos.y;
        if (CheckPosInsideBoard(x + 2, y + 1) && !CheckPiecePresent(x + 2, y + 1))
        {
            ChessBoardManager.Instance.chessBoard[x + 2, y + 1].SetPossiblePos();
        }
        if (CheckPosInsideBoard(x - 2, y + 1) && !CheckPiecePresent(x - 2, y + 1))
        {
            ChessBoardManager.Instance.chessBoard[x - 2, y + 1].SetPossiblePos();
        }
        if (CheckPosInsideBoard(x - 2, y - 1) && !CheckPiecePresent(x - 2, y - 1))
        {
            ChessBoardManager.Instance.chessBoard[x - 2, y - 1].SetPossiblePos();
        }
        if (CheckPosInsideBoard(x + 2, y - 1) && !CheckPiecePresent(x + 2, y - 1))
        {
            ChessBoardManager.Instance.chessBoard[x + 2, y - 1].SetPossiblePos();
        }
        if (CheckPosInsideBoard(x + 1, y + 2) && !CheckPiecePresent(x + 1, y + 2))
        {
            ChessBoardManager.Instance.chessBoard[x + 1, y + 2].SetPossiblePos();
        }
        if (CheckPosInsideBoard(x - 1, y + 2) && !CheckPiecePresent(x - 1, y + 2))
        {
            ChessBoardManager.Instance.chessBoard[x - 1, y + 2].SetPossiblePos();
        }
        if (CheckPosInsideBoard(x - 1, y - 2) && !CheckPiecePresent(x - 1, y - 2))
        {
            ChessBoardManager.Instance.chessBoard[x - 1, y - 2].SetPossiblePos();
        }
        if (CheckPosInsideBoard(x + 1, y - 2) && !CheckPiecePresent(x + 1, y - 2))
        {
            ChessBoardManager.Instance.chessBoard[x + 1, y - 2].SetPossiblePos();
        }
    }

    private bool CheckPosInsideBoard(int x, int y)
    {
        if (x >= 0 && x < ChessBoardManager.Instance.sizeChess && y >= 0 && y < ChessBoardManager.Instance.sizeChess)
            return (true);
        return (false);
    }

    private void TowerCheck()
    {
        for (int x = pos.x; x >= 0 && (ChessBoardManager.Instance.chessBoard[x, pos.y].piece == null || x == pos.x); x--)
        {
            if (x == pos.x)
                continue;
            if (CheckPiecePresent(x, pos.y) && x != pos.x)
                break;
            ChessBoardManager.Instance.chessBoard[x, pos.y].SetPossiblePos();
        }
        for (int x = pos.x; x < ChessBoardManager.Instance.sizeChess && (ChessBoardManager.Instance.chessBoard[x, pos.y].piece == null || x == pos.x); x++)
        {
            if (x == pos.x)
                continue;
            if (CheckPiecePresent(x, pos.y) && x != pos.x)
                break;
            ChessBoardManager.Instance.chessBoard[x, pos.y].SetPossiblePos();
        }
        for (int y = pos.y; y >= 0 && (ChessBoardManager.Instance.chessBoard[pos.x, y].piece == null || y == pos.y); y--)
        {
            if (y == pos.y)
                continue;
            if (CheckPiecePresent(pos.x, y))
                break;
            ChessBoardManager.Instance.chessBoard[pos.x, y].SetPossiblePos();
        }
        for (int y = pos.y; y < ChessBoardManager.Instance.sizeChess && (ChessBoardManager.Instance.chessBoard[pos.x, y].piece == null || y == pos.y); y++)
        {
            if (y == pos.y)
                continue;
            if (CheckPiecePresent(pos.x, y))
                break;
            ChessBoardManager.Instance.chessBoard[pos.x, y].SetPossiblePos();
        }
    }

    private void FoulCheck()
    {
        int x = pos.x;
        int y = pos.y;
        while (x >= 0 && y >= 0 && (ChessBoardManager.Instance.chessBoard[x, y].piece == null || (x == pos.x && y == pos.y)))
        {
            x--;
            y--;
            if (x < 0 || y < 0 || CheckPiecePresent(x, y))
                break;
            ChessBoardManager.Instance.chessBoard[x, y].SetPossiblePos();
        }
        x = pos.x;
        y = pos.y;
        while (x < ChessBoardManager.Instance.sizeChess && y < ChessBoardManager.Instance.sizeChess && (ChessBoardManager.Instance.chessBoard[x, y].piece == null || (x == pos.x && y == pos.y)))
        {
            x++;
            y++;
            if (x >= ChessBoardManager.Instance.sizeChess || y >= ChessBoardManager.Instance.sizeChess || CheckPiecePresent(x, y))
                break;
            ChessBoardManager.Instance.chessBoard[x, y].SetPossiblePos();
        }
        x = pos.x;
        y = pos.y;
        while (x < ChessBoardManager.Instance.sizeChess && y >= 0 && (ChessBoardManager.Instance.chessBoard[x, y].piece == null || (x == pos.x && y == pos.y)))
        {
            x++;
            y--;
            if (x >= ChessBoardManager.Instance.sizeChess || y < 0 || CheckPiecePresent(x, y))
                break;
            ChessBoardManager.Instance.chessBoard[x, y].SetPossiblePos();
        }
        x = pos.x;
        y = pos.y;
        while (x >= 0 && y < ChessBoardManager.Instance.sizeChess && (ChessBoardManager.Instance.chessBoard[x, y].piece == null || (x == pos.x && y == pos.y)))
        {
            x--;
            y++;
            if (x < 0 || y >= ChessBoardManager.Instance.sizeChess || CheckPiecePresent(x, y))
                break;
            ChessBoardManager.Instance.chessBoard[x, y].SetPossiblePos();
        }
    }
    
    private bool CheckPiecePresent(int x, int y)
    {
        if (ChessBoardManager.Instance.chessBoard[x, y].piece)
        {
            if (ChessBoardManager.Instance.chessBoard[x, y].piece.color == color)
            {
                return (true);
            }

            return (false);
        }
        return (false);
    }

    public void Destroy()
    {
        ChessBoardManager.Instance.currentPlayer.score += (int) type;
        Destroy(this.gameObject);
    }
}
