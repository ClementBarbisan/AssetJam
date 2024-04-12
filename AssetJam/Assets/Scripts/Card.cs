using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Piece.ColorPiece color;
    public Piece.PieceType type;

    public ChessBoardManager manager;

    private List<Piece> pieces;
    private bool doubleClick;


    public void OnPointerClick(PointerEventData eventData)
    {
        manager.currentPlayer.usedCard = this;
        manager.currentPiece = null;
        if (manager.currentPlayer.usedCard == this && doubleClick)
        {
            manager.currentPlayer.currentCards.Remove(manager.currentPlayer.usedCard);
            Destroy(manager.currentPlayer.usedCard.gameObject);
            manager.deck.GetCard(manager.currentPlayer);
            manager.deck.DisplayCards(manager.currentPlayer);
            manager.currentPiece = null;
            
        }
        if (pieces.Count == 1)
        {
            manager.currentPiece = pieces[0];
        }

        StartCoroutine(WaitDoubleClick());
    }

    private IEnumerator WaitDoubleClick()
    {
        doubleClick = true;
        yield return new WaitForSeconds(0.5f);
        doubleClick = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (manager.currentPlayer.usedCard)
            return;
        manager.currentPiece = null;
        pieces = manager.GetPieces(type, color);
        foreach (Piece piece in pieces)
        {
            piece.activate = true;
            piece.OutlineSquares();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (manager.currentPlayer.usedCard)
            return;
        manager.ResetBoard();
    }
}
