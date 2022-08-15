using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Pop : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _lockObjects = new List<GameObject>();

    [SerializeField]
    private List<GameObject> _unlockObjects = new List<GameObject>();

    [Header("Properties"), HideInInspector]
    public int Id;

    [SerializeField]
    private TextMeshProUGUI _priceText;

    public string Sound;

    [SerializeField]
    private bool _isDefaultUnlock;

    public bool IsPopUnlock
    {
        get => _isDefaultUnlock || PlayerPrefs.GetInt("PopUnlock" + Id, 0) == 1;
        set => PlayerPrefs.SetInt("PopUnlock" + Id, (value) ? 1 : 0);
    }

    [Header("Objects"), SerializeField]
    private TextMeshProUGUI _timerText;


    private DateTime _lastTimeClick
    {
        get => DateTime.Parse(PlayerPrefs.GetString("LastTimeClick" + Id, DateTime.MinValue.ToString()));
        set => PlayerPrefs.SetString("LastTimeClick" + Id, value.ToString());
    }

    public bool IsPopReady => _lastTimeClick.AddHours(PopManager.Instance.CooldownTime) < DateTime.Now;

    private void UpdateTimer()
    {
        if (IsPopReady)
        {
            _timerText.text = "";
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(ITimerProcess());

        }
    }

    private void UpdateStateUI()
    {
        foreach (GameObject obj in _unlockObjects) obj.SetActive(IsPopUnlock);
        foreach (GameObject obj in _lockObjects) obj.SetActive(!IsPopUnlock);
    }

    private IEnumerator ITimerProcess()
    {
        DateTime targetTime = DateTime.MinValue;
        if (!TutorialManager.IsWasTutorial && TutorialManager.CurrentState == 1) targetTime = _lastTimeClick.AddSeconds(2);
        else targetTime = _lastTimeClick.AddHours(PopManager.Instance.CooldownTime);
        
        while (targetTime > DateTime.Now)
        {
            TimeSpan timeSpan = targetTime - DateTime.Now;
            _timerText.text = timeSpan.ToString(@"hh\:mm\:ss");
            yield return new WaitForSeconds((float)timeSpan.Milliseconds / 1000f);
        }
        if (!TutorialManager.IsWasTutorial && TutorialManager.CurrentState == 1)
        {
            TutorialManager.CurrentState = 2;
            TutorialManager.Instance.UpdateStateObjects();
            _lastTimeClick = DateTime.MinValue;
        }
            _timerText.text = "";
        
    }


    private void Start()
    {
        UpdateTimer();
        UpdateStateUI();
        UpdateTextValue();
    }

    public void PopClick()
    {
        Debug.Log(TutorialManager.CurrentState);
        Debug.Log(IsPopReady);
        Debug.Log(_lastTimeClick);
        if (!IsPopReady) return;
        if (!TutorialManager.IsWasTutorial && TutorialManager.CurrentState != 0) return;
        _lastTimeClick = DateTime.Now;
        SoundManager.Instance.PlayClip(Sound);
        if (!TutorialManager.IsWasTutorial && TutorialManager.CurrentState == 0)
        {
            TutorialManager.CurrentState = 1;
            TutorialManager.Instance.UpdateStateObjects();
        }
        UpdateTimer();
    }

    public void CollectPop()
    {
        _lastTimeClick = DateTime.Now;
        FartTransaction.Instance.SetTransactionToJar(this.transform.position, new Vector2(0.5f, 0.5f), Vector2.zero, 0, true);
        UpdateTimer();
    }

    public void Unlock()
    {
        if (DataManager.Instance.CountCollectedPop < PopManager.Instance.UnlockPrice) return;

        SoundManager.Instance.PlayClip("Collect");
        Debug.Log("Unlocked!!");
        DataManager.Instance.CountCollectedPop -= PopManager.Instance.UnlockPrice;
        IsPopUnlock = true;

        Jar.Instance.UpdateText();
        UpdateStateUI();
        PopManager.Instance.UpdateTexts();
    }

    public void UpdateTextValue()
    { 
        _priceText.text = "Вам надо " + PopManager.Instance.UnlockPrice + " пуков чтобы открыть каку";
    }
}
