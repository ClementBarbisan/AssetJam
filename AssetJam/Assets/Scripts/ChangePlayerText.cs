using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangePlayerText : MonoBehaviour
{
    [SerializeField] private string _playerOneText = "Player One";
    [SerializeField] private string _playerTwoText = "Player Two";
    private TextMeshProUGUI _textTurn;
    [SerializeField] private TextMeshProUGUI _scorePlayerOne;
    [SerializeField] private TextMeshProUGUI _scorePlayerTwo;
    private void Awake()
    {
        _textTurn = GetComponent<TextMeshProUGUI>();
    }

    public void SetPlayerOneText()
    {
        _textTurn.text = _playerOneText + " turn";
        _scorePlayerOne.text = "Score " + _playerOneText + " : " + ChessBoardManager.Instance.playerOne.score.ToString("000");
        _scorePlayerTwo.text = "Score " + _playerTwoText + " : " + ChessBoardManager.Instance.playerTwo.score.ToString("000");
    }
    
    public void SetPlayerTwoText()
    {
        _textTurn.text = _playerTwoText + " turn";
        _scorePlayerOne.text = "Score " + _playerOneText + " : " + ChessBoardManager.Instance.playerOne.score.ToString("000");
        _scorePlayerTwo.text = "Score " + _playerTwoText + " : " + ChessBoardManager.Instance.playerTwo.score.ToString("000");
    }
}
