using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHidePanel : MonoBehaviour
{
    private bool _transitioning = false;
    private bool _show;
    [SerializeField] private Transform _positionShow;
    [SerializeField] private Transform _positionHide;
    [SerializeField] private float _speed = 0.1f;
    public void HideShowPanel()
    {
        if (_transitioning)
        {
            return;
        }

        _transitioning = true;
        if (_show)
        {
            StartCoroutine(HidePanel());
            _show = false;
        }
        else
        {
            StartCoroutine(ShowPanel());
            _show = true;
        }
    }

    private IEnumerator ShowPanel()
    {
        float posApply = 0;
        Vector3 posStart = transform.position;
        while (posApply <= 1.1f)
        {
            transform.position = Vector3.Lerp(posStart, _positionShow.position, posApply);
            posApply += Time.deltaTime * _speed;
            yield return null;
        }
        _transitioning = false;
    }

    private IEnumerator HidePanel()
    {
        float posApply = 0;
        Vector3 posStart = transform.position;
        while (posApply <= 1.1f)
        {
            transform.position = Vector3.Lerp(posStart, _positionHide.position, posApply);
            posApply += Time.deltaTime * _speed;
            yield return null;
        }
        _transitioning = false;
    }
}
