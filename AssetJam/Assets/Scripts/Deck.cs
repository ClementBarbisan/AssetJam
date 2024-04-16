using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Deck : MonoBehaviour
{
    [SerializeField] private List<Card> deckBase = new List<Card>();
    [SerializeField] private int nbDeck = 4;
    private float sizeCard;
    public List<Card> deck;
    [FormerlySerializedAs("_speed")] [SerializeField] public float speed = 2;

    [SerializeField] private TextMeshProUGUI _textNbCards;
    // Start is called before the first frame update
    void Awake()
    {
        deck = new List<Card>(deckBase.Count * nbDeck);
        for (int i = 0; i < nbDeck; i++)
        {
            deck.AddRange(deckBase);
        }

        deck.Shuffle();
        for (int i = ChessBoardManager.Instance.playerOne.nbCards; i > 0; i--)
        {
            Card card = Instantiate(deck[0], transform.position, Quaternion.identity, ChessBoardManager.Instance.playerOne.transform);
            card.transform.localScale *= 2;
            card.transform.localRotation = Quaternion.Euler(0, 0, 90);
            ChessBoardManager.Instance.playerOne.currentCards.Add(card);
            card.gameObject.SetActive(true);
            card.DisableCard();
            deck.RemoveAt(0);
        }

        for (int i = ChessBoardManager.Instance.playerTwo.nbCards; i > 0; i--)
        {
            Card card = Instantiate(deck[0], transform.position, Quaternion.identity, ChessBoardManager.Instance.playerTwo.transform);
            card.transform.localScale *= 2;
            card.transform.localRotation = Quaternion.Euler(0, 0, 90);
            ChessBoardManager.Instance.playerTwo.currentCards.Add(card);
            card.gameObject.SetActive(false);
            card.DisableCard();
            deck.RemoveAt(0);
        }
        _textNbCards.text = "Cards in deck : " + deck.Count;
    }

    public void GetCard(Player player)
    {
        _textNbCards.text = "Cards in deck : " + deck.Count;
        if (deck.Count == 0)
        {
            ChessBoardManager.Instance.EndGame();
            return;
        }
        while (player.currentCards.Count < player.nbCards)
        {
            Card card = Instantiate(deck[0], transform.position, Quaternion.identity, player.transform);
            card.transform.localScale *= 2;
            card.transform.localRotation = Quaternion.Euler(0, 0, 90);
            player.currentCards.Add(card);
            deck.RemoveAt(0);
        }
    }

    public void HideCards(Player player)
    {
        StartCoroutine(HideCardsToDeck(player));
    }

    private IEnumerator HideCardsToDeck(Player player)
    {
        float posApply = 0;
        Vector3[] pos = new Vector3[player.currentCards.Count];
        int index = 0;
        Vector3 startRot = player.currentCards[0].transform.rotation.eulerAngles;
        foreach (Card card in player.currentCards)
        {
            pos[index] = card.transform.position;
            card.DisableCard();
            index++;
        }
        yield return new WaitForEndOfFrame();
        while (posApply <= 1.1f)
        {
            index = 0;
            foreach (Card card in player.currentCards)
            {
                card.transform.position = Vector3.Lerp(pos[index], transform.position, posApply);
                card.transform.localRotation = Quaternion.Euler(Vector3.Lerp(startRot, new Vector3(0, 0, 90), posApply));
                index++;
            }
            posApply += Time.deltaTime * speed;
            yield return null;
        }
        foreach (Card card in player.currentCards)
        {
            card.gameObject.SetActive(false);
        } 
        DisplayCards(ChessBoardManager.Instance.currentPlayer);
    }

    public void DisplayCards(Player currentPlayer)
    {
        foreach (Card card in currentPlayer.currentCards)
        {
            card.gameObject.SetActive(true);
        }

        StartCoroutine(DisplayCardsToDeck(currentPlayer));
    }

    private IEnumerator DisplayCardsToDeck(Player currentPlayer)
    {
        float posApply = 0;
        Vector3[] pos = new Vector3[currentPlayer.currentCards.Count];
        Vector3[] startPos = new Vector3[currentPlayer.currentCards.Count];
        int index = 0;
        Vector3 startRot = currentPlayer.currentCards[0].transform.rotation.eulerAngles;
        foreach (Card card in currentPlayer.currentCards)
        {
            pos[index] = currentPlayer.transform.position +
                         currentPlayer.transform.right * card.GetComponent<SpriteRenderer>().size.x * 2 *
                         currentPlayer.currentCards.IndexOf(card) - currentPlayer.transform.right *
                         card.GetComponent<SpriteRenderer>().size.x * 2 * (currentPlayer.currentCards.Count / 2);
            startPos[index] = card.transform.position;
            index++;
        }

        yield return new WaitForEndOfFrame();
        while (posApply <= 1.1f)
        {
            index = 0;
            foreach (Card card in currentPlayer.currentCards)
            {
                card.transform.position = Vector3.Lerp(startPos[index], pos[index], posApply);
                card.transform.localRotation = Quaternion.Euler(Vector3.Lerp(startRot, new Vector3(0, 0, 0), posApply));
                index++;
            }
            posApply += Time.deltaTime * speed;
            yield return null;
        }

        foreach (Card card in currentPlayer.currentCards)
        {
            card.EnableCard();
        }
    }
}
    