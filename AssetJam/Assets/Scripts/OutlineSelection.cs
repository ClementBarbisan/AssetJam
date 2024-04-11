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
        if (!gameObject.activeSelf)
            return;
        gameObject.SetActive(false);
        _parent.managerBoard.currentPiece.transform.position = this.transform.position - this.transform.forward / 10;
        _parent.managerBoard.currentPiece.pos = _parent.pos;
        if (_parent.piece)
        {
            _parent.piece.Destroy();
        }
        _parent.managerBoard.ResetBoard();
        _parent.managerBoard.SwitchPlayer();
    }
}
