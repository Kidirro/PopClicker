using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class Jar : Singleton<Jar>
{

    [Header("Properties"), SerializeField]
    private Image _bgImage;

    [SerializeField]
    private GameObject _fart;

    [SerializeField]
    private RectTransform _lidMain;

    [SerializeField]
    private RectTransform _lidSub;

    [SerializeField]
    private RectTransform _jarMain;

    [SerializeField]
    private List<TextMeshProUGUI> _textValueList = new List<TextMeshProUGUI>();

    [SerializeField]
    private List<GameObject> _openedList = new List<GameObject>();

    [SerializeField]
    private List<GameObject> _closedList = new List<GameObject>();

    [Header("Open properties"), SerializeField]
    private Color _openColorBG;

    [SerializeField]
    private Vector3 _openPosition;

    [SerializeField]
    private Vector3 _openScale;

    [Header("Close properties"), SerializeField]
    private Color _closeColorBG;

    [SerializeField]
    private Vector3 _closePosition;

    [SerializeField]
    private Vector3 _closeScale;

    [Space, SerializeField]
    private float _soundTimeAwait;

    private bool _isOpen = false;

    private Vector2 _lidSubStartPosition;
    private Vector2 _lidMainStartPosition;

    private Coroutine _lidSubMovement;
    private Coroutine _lidMainMovement;

    private bool _isCanMove = true;

    private void Start()
    {
        _lidSubStartPosition = _lidSub.transform.localPosition;
        _lidMainStartPosition = _lidMain.transform.localPosition;
        UpdateUI();
    }

    public void OnJarClick()
    {
        Debug.Log(TutorialManager.CurrentState);
        if (!TutorialManager.IsWasTutorial && !(TutorialManager.CurrentState == 2 || TutorialManager.CurrentState == 3)) return;

        if (!_isOpen)
        {

            if (PopManager.Instance.IsExistCollectPop()) PopManager.Instance.CollectAllPop();
            else
            {
                _isOpen = true;
                ChangeState(_isOpen);
            }
        }
        if (TutorialManager.CurrentState == 2 || TutorialManager.CurrentState == 3)
        {
            TutorialManager.CurrentState += 1;
            Debug.Log(TutorialManager.CurrentState);
            TutorialManager.Instance.UpdateStateObjects();
            TutorialManager.IsWasTutorial = true;
        }
    }

    public IEnumerator Obosramtus()
    {
        _isCanMove = false;
        if (TutorialManager.CurrentState == 4)
        {
            TutorialManager.CurrentState = 5;
            TutorialManager.Instance.UpdateStateObjects();
        }
        StartCoroutine(_lidMain.SetPositionWithLerpRect(_lidMainStartPosition, new Vector2(_lidMainStartPosition.x, Camera.main.pixelHeight / 2 + _lidMain.rect.height * 2), 0.5f));
        yield return new WaitForSeconds(1);

        float trasestsyaX = 50;

        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(_jarMain.SetPositionWithLerpRect(_jarMain.localPosition, new Vector2(_jarMain.localPosition.x + trasestsyaX, _jarMain.localPosition.y), 0.1f));
            trasestsyaX = -trasestsyaX;
            yield return new WaitForSeconds(0.1f);
        }

        StartCoroutine(_jarMain.SetPositionWithLerpRect(_jarMain.localPosition, _openPosition, 0.1f));
        yield return new WaitForSeconds(0.1f);

        _fart.SetActive(false);
        for (int i = 0; i < DataManager.Instance.CountCollectedPop; i++)
        {
            FartTransaction.Instance.SetTransaction(_jarMain.position, new Vector2(_jarMain.position.x, _jarMain.position.y + Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight)).y), 1);

            float rand = Random.Range(0f, _soundTimeAwait);
            SoundManager.Instance.PlayClip(DataManager.Instance.GetPopValue(i));
            UpdateText(DataManager.Instance.CountCollectedPop - i);
            yield return new WaitForSeconds(rand);
        }
        DataManager.Instance.ClearPopValue();
        UpdateText();
        yield return new WaitForSeconds(0.7f);
        StartCoroutine(_lidMain.SetPositionWithLerpRect(_lidMain.localPosition, _lidMainStartPosition, 0.5f));

        _isCanMove = true;
    }


    public void ChangeState(bool state)
    {

        if (!state)
        {

            foreach (GameObject gameObject in _closedList) StartCoroutine(gameObject.SetActive(true, 1));
            foreach (GameObject gameObject in _openedList) gameObject.SetActive(false);
            StartCoroutine(_bgImage.gameObject.SetActive(false, 1));
            StartCoroutine(_jarMain.SetPositionWithLerpRect(_jarMain.localPosition, _closePosition, 1));
            StartCoroutine(_jarMain.SetScaleWithLerpRect(_jarMain.localScale, _closeScale, 1));
            StartCoroutine(_bgImage.SetColorWithLerp(_openColorBG, _closeColorBG, 1));
        }
        else
        {
            foreach (GameObject gameObject in _openedList) StartCoroutine(gameObject.SetActive(true, 1));
            foreach (GameObject gameObject in _closedList) gameObject.SetActive(false);
            _bgImage.gameObject.SetActive(true);
            StartCoroutine(_jarMain.SetPositionWithLerpRect(_jarMain.localPosition, _openPosition, 1));
            StartCoroutine(_jarMain.SetScaleWithLerpRect(_jarMain.localScale, _openScale, 1));
            StartCoroutine(_bgImage.SetColorWithLerp(_closeColorBG, _openColorBG, 1));
        }
    }

    public void UpdateUI()
    {
        foreach (GameObject gameObject in _openedList) gameObject.SetActive(_isOpen);
        foreach (GameObject gameObject in _closedList) gameObject.SetActive(!_isOpen);
        UpdateText();
    }

    public void UpdateText()
    {
        foreach (TextMeshProUGUI text in _textValueList) text.text = DataManager.Instance.CountCollectedPop.ToString();
        _fart.SetActive(DataManager.Instance.CountCollectedPop > 0);

    }


    public void UpdateText(int value)
    {
        foreach (TextMeshProUGUI text in _textValueList) text.text = value.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine(Obosramtus());
    }

    public void ClearPosition()
    {
        if (_lidSubMovement != null) StopCoroutine(_lidSubMovement);
        _lidSubMovement = StartCoroutine(_lidSub.SetPositionWithLerpRect(_lidSub.localPosition, _lidSubStartPosition, 0.5f));
    }

    public void SetPositionLidSub(float x)
    {
        _lidSub.localPosition = new Vector2(_lidSubStartPosition.x + x, _lidSubStartPosition.y);
        Debug.Log(_lidSub.localPosition);
    }

    public void CloseJar()
    {
        if (!_isCanMove) return;
        Debug.Log(_isCanMove);
        if (TutorialManager.CurrentState == 4 || TutorialManager.CurrentState == 3) return;
        Debug.Log(TutorialManager.CurrentState);
        _isOpen = false;
        ChangeState(_isOpen);

    }
}
