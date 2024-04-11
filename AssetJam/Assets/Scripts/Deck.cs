using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private List<Card> deckBase = new List<Card>();
    [SerializeField] private int nbDeck = 4;
    [SerializeField] private ChessBoardManager manager;
    private float sizeCard;
    private List<Card> deck;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < deckBase.Count; i++)
        {
            deckBase[i].manager = manager;
        }
        deck = new List<Card>(deckBase.Count * nbDeck);
        for (int i = 0; i < nbDeck; i++)
        {
            deck.AddRange(deckBase);
        }

        deck.Shuffle();
        for (int i = manager.playerOne.nbCards; i > 0; i--)
        {
            Card card = Instantiate(deck[0],manager.playerOne.transform);
            card.transform.localScale *= 2;
            card.transform.position = manager.playerOne.transform.position + deck[^1].transform.right
                * card.GetComponent<SpriteRenderer>().size.x * 2 * manager.playerOne.currentCards.Count;
            manager.playerOne.currentCards.Add(card);
            card.gameObject.SetActive(true);
            deck.RemoveAt(0);
        }

        for (int i = manager.playerTwo.nbCards; i > 0; i--)
        {
            Card card = Instantiate(deck[0], manager.playerTwo.transform);
            card.transform.localScale *= 2;
            card.transform.position = manager.playerOne.transform.position + deck[^1].transform.right
                * card.GetComponent<SpriteRenderer>().size.x * 2 * manager.playerOne.currentCards.Count;
            manager.playerTwo.currentCards.Add(card);
            card.gameObject.SetActive(false);
            deck.RemoveAt(0);
        }
    }

    public void GetCard(Player player)
    {
        while (player.currentCards.Count < player.nbCards)
        {
            Card card = Instantiate(deck[0], player.transform);
            card.transform.localScale *= 2;
            card.transform.position = manager.playerOne.transform.position + deck[^1].transform.right
                * card.GetComponent<SpriteRenderer>().size.x * 2 * manager.playerOne.currentCards.Count;
            player.currentCards.Add(card);
            deck.RemoveAt(0);
        }
    }

    public void HideCards(Player player)
    {
        foreach (Card card in player.currentCards)
        {
            card.gameObject.SetActive(false);
        }
    }
    
    public void DisplayCards(Player currentPlayer)
    {
        foreach (Card card in currentPlayer.currentCards)
        {
            card.transform.position = currentPlayer.transform.position +
              currentPlayer.transform.right * card.GetComponent<SpriteRenderer>().size.x * 2 *
              currentPlayer.currentCards.IndexOf(card);
            card.gameObject.SetActive(true);
        }
    }
}
    