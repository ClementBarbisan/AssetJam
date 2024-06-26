using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChessBoardManager : MonoBehaviour
{
    public static ChessBoardManager Instance;
    [SerializeField] public Deck deck;
    [FormerlySerializedAs("_playerOne")] public Player playerOne;
    [FormerlySerializedAs("_playerTwo")] public Player playerTwo;
    [SerializeField] private GameObject _blackSquare;
    [SerializeField] private GameObject _whiteSquare;
    [FormerlySerializedAs("_sizeChess")] public int sizeChess = 8;
    [SerializeField] private GameObject _pawnWhite;
    [SerializeField] private GameObject _foulWhite;
    [SerializeField] private GameObject _towerWhite;
    [SerializeField] private GameObject _knightWhite;
    [SerializeField] private GameObject _queenWhite;
    [SerializeField] private GameObject _kingWhite;
    [SerializeField] private GameObject _pawnBlack;
    [SerializeField] private GameObject _foulBlack;
    [SerializeField] private GameObject _towerBlack;
    [SerializeField] private GameObject _knightBlack;
    [SerializeField] private GameObject _queenBlack;
    [SerializeField] private GameObject _kingBlack;
    private float _sizeSquare;
    [SerializeField] private ChangePlayerText _playerText;
    public Square[,] chessBoard;
    [HideInInspector]
    public Player currentPlayer;
    [HideInInspector]
    public Piece currentPiece;
    [HideInInspector]
    public bool kingTaken;
    [SerializeField] private Image _canvasEnd;
    [SerializeField] private TextMeshProUGUI _textEnd;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = playerOne;
        SpriteRenderer sprite = _blackSquare.GetComponent<SpriteRenderer>();
        _sizeSquare = sprite.size.x;
        chessBoard = new Square[sizeChess, sizeChess];
        for (int j = 0; j < sizeChess; j++)
        {
            for (int i = 0; i < sizeChess; i++)
            {
                if (i % 2 == 0)
                {
                    if (j % 2 != 0)
                    {
                        chessBoard[i, j] = Instantiate(_whiteSquare, (_whiteSquare.transform.right * _sizeSquare * (i - sizeChess / 2)) +
                             (_whiteSquare.transform.up * _sizeSquare * (j - sizeChess / 2 + 1) + transform.position), Quaternion.identity, 
                            this.transform).GetComponent<Square>();
                    }
                    else
                    {
                        chessBoard[i, j] = Instantiate(_blackSquare, (_blackSquare.transform.right * _sizeSquare * (i - sizeChess / 2)) +
                          (_blackSquare.transform.up * _sizeSquare * (j - sizeChess / 2 + 1)) + transform.position, Quaternion.identity, 
                            this.transform).GetComponent<Square>();
                    }
                   
                }
                else
                {
                    if (j % 2 == 0)
                    {
                        chessBoard[i, j] = Instantiate(_whiteSquare, (_whiteSquare.transform.right * _sizeSquare * (i - sizeChess / 2)) +
                          (_whiteSquare.transform.up * _sizeSquare * (j - sizeChess / 2 + 1)) + transform.position, Quaternion.identity, 
                            this.transform).GetComponent<Square>();
                    }
                    else
                    {
                        chessBoard[i, j] = Instantiate(_blackSquare, (_blackSquare.transform.right * _sizeSquare * (i - sizeChess / 2)) +
                           (_blackSquare.transform.up * _sizeSquare * (j - sizeChess / 2 + 1)) + transform.position, Quaternion.identity, 
                            this.transform).GetComponent<Square>();
                    }
                }

                chessBoard[i, j].pos = new Vector2Int(i, j);
                if (j == 0)
                {
                    if (i == 0 || i == sizeChess - 1)
                    {
                        chessBoard[i, j].piece =
                            Instantiate(_towerWhite, chessBoard[i, j].transform.position -chessBoard[i, j].transform.forward / 10, Quaternion.identity,
                                chessBoard[i, j].transform).GetComponent<Piece>();
                    }
                    else if (i == 1 || i == sizeChess - 2)
                    {
                        chessBoard[i, j].piece =
                            Instantiate(_knightWhite, chessBoard[i, j].transform.position -chessBoard[i, j].transform.forward / 10, Quaternion.identity, 
                                chessBoard[i, j].transform).GetComponent<Piece>();
                    }
                    else if (i == 2 || i == sizeChess - 3)
                    {
                        chessBoard[i, j].piece =
                            Instantiate(_foulWhite, chessBoard[i, j].transform.position -chessBoard[i, j].transform.forward / 10, Quaternion.identity, 
                                chessBoard[i, j].transform).GetComponent<Piece>();
                    }
                    else if (i == 3)
                    {
                        chessBoard[i, j].piece =
                            Instantiate(_queenWhite, chessBoard[i, j].transform.position -chessBoard[i, j].transform.forward / 10, Quaternion.identity, 
                                chessBoard[i, j].transform).GetComponent<Piece>();
                    }
                    else if (i == 4)
                    {
                        chessBoard[i, j].piece =
                            Instantiate(_kingWhite, chessBoard[i, j].transform.position -chessBoard[i, j].transform.forward / 10, Quaternion.identity, 
                                chessBoard[i, j].transform).GetComponent<Piece>();
                    }
                    chessBoard[i, j].piece.pos = new Vector2Int(i, j);
                }
                else if (j == 1)
                {
                     chessBoard[i, j].piece =
                         Instantiate(_pawnWhite, chessBoard[i, j].transform.position -chessBoard[i, j].transform.forward / 10, Quaternion.identity, 
                             chessBoard[i, j].transform).GetComponent<Piece>();
                     chessBoard[i, j].piece.pos = new Vector2Int(i, j);
                }
                else if (j == sizeChess - 2)
                {
                     chessBoard[i, j].piece =
                         Instantiate(_pawnBlack, chessBoard[i, j].transform.position -chessBoard[i, j].transform.forward / 10, Quaternion.identity, 
                             chessBoard[i, j].transform).GetComponent<Piece>();
                     chessBoard[i, j].piece.pos = new Vector2Int(i, j);
                }
                else if (j == sizeChess - 1)
                {
                    if (i == 0 || i == sizeChess - 1)
                    {
                        chessBoard[i, j].piece =
                            Instantiate(_towerBlack, chessBoard[i, j].transform.position -chessBoard[i, j].transform.forward / 10, Quaternion.identity, 
                                chessBoard[i, j].transform).GetComponent<Piece>();
                    }
                    else if (i == 1 || i == sizeChess - 2)
                    {
                        chessBoard[i, j].piece =
                            Instantiate(_knightBlack, chessBoard[i, j].transform.position -chessBoard[i, j].transform.forward / 10, Quaternion.identity, 
                                chessBoard[i, j].transform).GetComponent<Piece>();
                    }
                    else if (i == 2 || i == sizeChess - 3)
                    {
                        chessBoard[i, j].piece =
                            Instantiate(_foulBlack, chessBoard[i, j].transform.position -chessBoard[i, j].transform.forward / 10, Quaternion.identity, 
                                chessBoard[i, j].transform).GetComponent<Piece>();
                    }
                    else if (i == 3)
                    {
                        chessBoard[i, j].piece =
                            Instantiate(_queenBlack, chessBoard[i, j].transform.position -chessBoard[i, j].transform.forward / 10, Quaternion.identity, 
                                chessBoard[i, j].transform).GetComponent<Piece>();
                    }
                    else if (i == 4)
                    {
                        chessBoard[i, j].piece =
                            Instantiate(_kingBlack, chessBoard[i, j].transform.position -chessBoard[i, j].transform.forward / 10, Quaternion.identity, 
                                chessBoard[i, j].transform).GetComponent<Piece>();
                    }
                    chessBoard[i, j].piece.pos = new Vector2Int(i, j);
                }
            }
        }

        deck.DisplayCards(currentPlayer);
    }

    public void ResetBoard()
    {
        for (int i = 0; i < sizeChess; i++)
        {
            for (int j = 0; j < sizeChess; j++)
            {
                chessBoard[i, j].UnsetPossiblePos();
                if (chessBoard[i, j].piece)
                {
                    chessBoard[i, j].piece.activate = false;
                }
            }
        }
    }
    
    public void SwitchPlayer()
    {
        ResetBoard();
        currentPlayer.currentCards.Remove(currentPlayer.usedCard);
        Destroy(currentPlayer.usedCard.gameObject);
        currentPlayer.usedCard = null;
        deck.GetCard(currentPlayer);
        deck.HideCards(currentPlayer);
        if (currentPlayer == playerOne)
        {
            _playerText.SetPlayerTwoText();
            currentPlayer = playerTwo;
        }
        else
        {
            _playerText.SetPlayerOneText();
            currentPlayer = playerOne;
        }
    }

    public List<Piece> GetPieces(Piece.PieceType type, Piece.ColorPiece color)
    {
        List<Piece> pieces = new List<Piece>();
        for (int i = 0; i < sizeChess; i++)
        {
            for (int j = 0; j < sizeChess; j++)
            {
                if (chessBoard[i, j].piece && chessBoard[i, j].piece.color == color &&
                    chessBoard[i, j].piece.type == type)
                {
                    pieces.Add(chessBoard[i, j].piece);
                }
            }
        }

        return (pieces);
    }

    public void EndGame()
    {
        if (currentPlayer == playerTwo)
        {
            _playerText.SetPlayerTwoText();
        }
        else
        {
            _playerText.SetPlayerOneText();
        }
        if (playerOne.score > playerTwo.score)
        {
            _textEnd.text = "Player One Won with " + playerOne.score + " !";
        }
        else if (playerTwo.score > playerOne.score)
        {
            _textEnd.text = "Player Two Won " + playerTwo.score + " !";
        }
        else if (deck.deck.Count == 0 && !kingTaken)
        {
            _textEnd.text = "It's a draw !";
        }
        else if (currentPlayer == playerOne)
        {
            _textEnd.text = "Player One Won " + playerOne.score + " !";
        }
        else
        {
            _textEnd.text = "Player Two Won " + playerTwo.score + " !";
        }
        _canvasEnd.gameObject.SetActive(true);
        
    }
}
