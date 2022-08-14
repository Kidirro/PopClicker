using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class Jar : MonoBehaviour
{

    [Header("Properties"), SerializeField]
    private Image _bgImage;

    [SerializeField]
    private RectTransform _rectTransform;

    [Header("Open properties"), SerializeField]
    private Color _openColorBG;

    [SerializeField]
    private Vector3 _openPosition;

    [Header("Close properties"), SerializeField]
    private Color _closeColorBG;

    [SerializeField]
    private Vector3 _closePosition;

    [Space, SerializeField]
    private float _soundTimeAwait;

    private bool _isOpen = false;

    public void OnJarClick()
    {
        if (PopManager.Instance.IsExistCollectPop() && !_isOpen) PopManager.Instance.CollectAllPop();
        else
        {
            //StartCoroutine(IHellProcess());

            if (_isOpen)
            {
                StartCoroutine(_bgImage.gameObject.SetActive(false, 1));
                StartCoroutine(_rectTransform.SetPositionWithLerpRect(_rectTransform.localPosition, _closePosition, 1));
                StartCoroutine(_bgImage.SetColorWithLerp(_openColorBG, _closeColorBG, 1));
            }
            else
            {
                _bgImage.gameObject.SetActive(true);
                StartCoroutine(_rectTransform.SetPositionWithLerpRect(_rectTransform.localPosition, _openPosition, 1));
                StartCoroutine(_bgImage.SetColorWithLerp(_closeColorBG, _openColorBG, 1));
            }
            _isOpen = !_isOpen;
        }
    }

    public IEnumerator IHellProcess()
    {
        for (int i = 0; i < DataManager.Instance.CountCollectedPop; i++)
        {
            FartTransaction.Instance.SetTransaction(_rectTransform.position, new Vector2( _rectTransform.position.x, _rectTransform.position.y + Camera.main.ScreenToWorldPoint(new Vector2(0, 1500)).y), 1);

            float rand = Random.Range(0f, _soundTimeAwait);
            SoundManager.Instance.PlayClip(DataManager.Instance.GetPopValue(i));

            yield return new WaitForSeconds(rand);
        }
        DataManager.Instance.ClearPopValue();
    }


}
