using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;

public class Deck : MonoBehaviour
{
    [SerializeField] private List<Card> deckBase = new List<Card>();
    [SerializeField] private int nbDeck = 4;
    [SerializeField] private ChessBoardManager manager;
    private float sizeCard;
    private List<Card> deck;
    [FormerlySerializedAs("_speed")] [SerializeField] public float speed = 2;

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
            Card card = Instantiate(deck[0], transform.position, Quaternion.identity, manager.playerOne.transform);
            card.transform.localScale *= 2;
            card.transform.localRotation = Quaternion.Euler(0, 0, 90);
            manager.playerOne.currentCards.Add(card);
            card.gameObject.SetActive(true);
            card.DisableCard();
            deck.RemoveAt(0);
        }

        for (int i = manager.playerTwo.nbCards; i > 0; i--)
        {
            Card card = Instantiate(deck[0], transform.position, Quaternion.identity, manager.playerTwo.transform);
            card.transform.localScale *= 2;
            card.transform.localRotation = Quaternion.Euler(0, 0, 90);
            manager.playerTwo.currentCards.Add(card);
            card.gameObject.SetActive(false);
            card.DisableCard();
            deck.RemoveAt(0);
        }
    }

    public void GetCard(Player player)
    {
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
        while (posApply < 1f)
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
        DisplayCards(manager.currentPlayer);
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
        while (posApply < 1f)
        {
            index = 0;
            foreach (Card card in currentPlayer.currentCards)
            {
                card.transform.position = Vector3.Lerp(startPos[index], pos[index], posApply);
                card.transform.localRotation = Quaternion.Euler(Vector3.Lerp(startRot, new Vector3(0, 0, 0), posApply));
                index++;
                yield return null;
            }
            posApply += Time.deltaTime * speed;
        }

        foreach (Card card in currentPlayer.currentCards)
        {
            card.EnableCard();
        }
    }
}
    