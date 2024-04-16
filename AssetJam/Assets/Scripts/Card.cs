using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Piece.ColorPiece color;
    public Piece.PieceType type;

    public ChessBoardManager manager;

    public List<Piece> pieces;
    private bool doubleClick;
    private SphereCollider _collider;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (manager.currentPlayer.usedCard == this)
        {
            StartCoroutine(HideCard(this));
            return;
        }
        manager.currentPlayer.usedCard = this;
        manager.currentPiece = null;
        if (pieces.Count == 1)
        {
            manager.currentPiece = pieces[0];
        }
    }

    public void EnableCard()
    {
        _collider.enabled = true;
    }

    public void DisableCard()
    {
        _collider.enabled = false;
    }
    
    public IEnumerator HideCard(Card card)
    {
        DisableCard();
        float posApply = 0f;
        Vector3 startPos = card.transform.position;
        Vector3 startRot = card.transform.localRotation.eulerAngles;
        while (posApply < 1f)
        {
            card.transform.position = Vector3.Lerp(startPos, manager.deck.transform.position, posApply);
            card.transform.localRotation = Quaternion.Euler(Vector3.Slerp(startRot, new Vector3(0, 0, 90), posApply));
            posApply += Time.deltaTime * manager.deck.speed;
            yield return null;
        }
        manager.currentPiece = null;
        manager.SwitchPlayer();
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
