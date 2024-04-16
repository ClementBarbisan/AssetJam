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
        if (!gameObject.activeSelf || !ChessBoardManager.Instance.currentPiece.activate)
            return;
        gameObject.SetActive(false);
        ChessBoardManager.Instance.currentPiece.transform.position = this.transform.position - this.transform.forward / 10;
        ChessBoardManager.Instance
            .chessBoard[ChessBoardManager.Instance.currentPiece.pos.x, ChessBoardManager.Instance.currentPiece.pos.y].piece = null;
        ChessBoardManager.Instance.currentPiece.pos = _parent.pos;
        if (_parent.piece)
        {
            _parent.piece.Destroy();
            if (_parent.piece.type == Piece.PieceType.King)
            {
                ChessBoardManager.Instance.EndGame();
                return;
            }
        }
        _parent.piece = ChessBoardManager.Instance.currentPiece;
        ChessBoardManager.Instance.ResetBoard();
        ChessBoardManager.Instance.SwitchPlayer();
    }
}
