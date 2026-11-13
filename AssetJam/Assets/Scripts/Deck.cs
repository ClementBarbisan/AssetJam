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

    // slot : index in the hand where the new card goes (the place left by the played/discarded card)
    public void GetCard(Player player, int slot = -1)
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
            if (slot >= 0 && slot <= player.currentCards.Count)
            {
                player.currentCards.Insert(slot, card);
            }
            else
            {
                player.currentCards.Add(card);
            }
            deck.RemoveAt(0);
        }
    }

    // The hand flies back to the deck, then the other player's hand is dealt.
    // Each card always comes back to its own slot (its index in the hand).
    public void HideCards(Player player)
    {
        StartCoroutine(HideCardsToDeck(player));
    }

    private IEnumerator HideCardsToDeck(Player player)
    {
        List<Card> cards = new List<Card>(player.currentCards);
        float posApply = 0;
        Vector3[] startPos = new Vector3[cards.Count];
        Vector3[] startRot = new Vector3[cards.Count];
        for (int index = 0; index < cards.Count; index++)
        {
            startPos[index] = cards[index].transform.position;
            startRot[index] = cards[index].transform.localRotation.eulerAngles;
            cards[index].DisableCard();
        }
        yield return new WaitForEndOfFrame();
        while (posApply < 1f)
        {
            posApply += Time.deltaTime * speed;
            for (int index = 0; index < cards.Count; index++)
            {
                cards[index].transform.position = Vector3.Lerp(startPos[index], transform.position, posApply);
                cards[index].transform.localRotation = Quaternion.Euler(Vector3.Lerp(startRot[index], new Vector3(0, 0, 90), posApply));
            }
            yield return null;
        }
        foreach (Card card in cards)
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

    // Every card goes from where it is (the deck) to the slot matching its index in the hand.
    private IEnumerator DisplayCardsToDeck(Player currentPlayer)
    {
        List<Card> cards = new List<Card>(currentPlayer.currentCards);
        float posApply = 0;
        Vector3[] pos = new Vector3[cards.Count];
        Vector3[] startPos = new Vector3[cards.Count];
        Vector3[] startRot = new Vector3[cards.Count];
        for (int index = 0; index < cards.Count; index++)
        {
            Card card = cards[index];
            float width = card.GetComponent<SpriteRenderer>().size.x * 2;
            pos[index] = currentPlayer.transform.position +
                         currentPlayer.transform.right * width * (index - cards.Count / 2);
            startPos[index] = card.transform.position;
            startRot[index] = card.transform.localRotation.eulerAngles;
            card.DisableCard();
        }

        yield return new WaitForEndOfFrame();
        while (posApply < 1f)
        {
            posApply += Time.deltaTime * speed;
            for (int index = 0; index < cards.Count; index++)
            {
                cards[index].transform.position = Vector3.Lerp(startPos[index], pos[index], posApply);
                cards[index].transform.localRotation = Quaternion.Euler(Vector3.Lerp(startRot[index], Vector3.zero, posApply));
            }
            yield return null;
        }

        foreach (Card card in cards)
        {
            card.EnableCard();
        }
    }
}
