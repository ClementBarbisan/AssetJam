using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int score;
    public List<Card> currentCards;
    private Card _usedCard;
    public Card usedCard
    {
        get => _usedCard;
        set
        {
            if (_usedCard && value != null)
            {
                _usedCard.transform.position -= _usedCard.transform.up / 10f;
                ChessBoardManager.Instance.ResetBoard();
                foreach (Piece piece in _usedCard.pieces)
                {
                    piece.activate = false;
                }
                value.pieces = ChessBoardManager.Instance.GetPieces(value.type, value.color);
                foreach (Piece piece in value.pieces)
                {
                    piece.activate = true;
                    piece.OutlineSquares();
                }
            }
            _usedCard = value;
            if (_usedCard != null)
            {
                _usedCard.transform.position += _usedCard.transform.up / 10f;
            }
        }
    }

    public int nbCards = 5;
}