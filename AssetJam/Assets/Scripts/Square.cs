using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Square : MonoBehaviour
{
   
    public Piece piece;
    public bool possiblePos;
    public Vector2Int pos;
    [SerializeField] private GameObject _possiblePosHint;
    // Start is called before the first frame update
    void Start()
    {
        _possiblePosHint.SetActive(false);
    }

    public void SetPossiblePos()
    {
        _possiblePosHint.SetActive(true);
    }

    public void UnsetPossiblePos()
    {
        _possiblePosHint.SetActive(false);
    }
}
