using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour, IPointerClickHandler
{
    private Square _parent;
    // Start is called before the first frame update
    void Start()
    {
        _parent = GetComponentInParent<Square>();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!gameObject.activeSelf || !_parent.managerBoard.currentPiece.activate)
            return;
        gameObject.SetActive(false);
        _parent.managerBoard.currentPiece.transform.position = this.transform.position - this.transform.forward / 10;
        _parent.managerBoard
            .chessBoard[_parent.managerBoard.currentPiece.pos.x, _parent.managerBoard.currentPiece.pos.y].piece = null;
        _parent.managerBoard.currentPiece.pos = _parent.pos;
        if (_parent.piece)
        {
            _parent.piece.Destroy();
        }
        _parent.piece = _parent.managerBoard.currentPiece;
        _parent.managerBoard.ResetBoard();
        _parent.managerBoard.SwitchPlayer();
    }
}
