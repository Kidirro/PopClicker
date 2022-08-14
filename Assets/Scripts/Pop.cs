using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Pop : MonoBehaviour
{
    [Header("Properties"),HideInInspector]
    public int Id;

    public string Sound;

    [SerializeField]
    private bool _isDefaultUnlock;

    public bool IsPopUnlock
    {
        get => _isDefaultUnlock || PlayerPrefs.GetInt("PopUnlock" + Id, 0) == 1;
        set => PlayerPrefs.SetInt("PopUnlock" + Id, (value)?1:0);
    }

    [Header("Objects"), SerializeField]
    private TextMeshProUGUI _timerText;


    private DateTime _lastTimeClick {
        get => DateTime.Parse(PlayerPrefs.GetString("LastTimeClick" + Id, DateTime.MinValue.ToString()));
        set => PlayerPrefs.SetString("LastTimeClick" + Id, value.ToString());
    }

    public bool IsPopReady => _lastTimeClick.AddHours(PopManager.Instance.CooldownTime) < DateTime.Now;

    private void UpdateTimer()
    {
        if (IsPopReady )
        {
            _timerText.text = "";
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(ITimerProcess());

        }
    }

    private IEnumerator ITimerProcess()
    {
        DateTime targetTime = _lastTimeClick.AddHours(PopManager.Instance.CooldownTime);
        while (targetTime > DateTime.Now)
        {
            TimeSpan timeSpan = targetTime - DateTime.Now;
            _timerText.text = timeSpan.ToString(@"hh\:mm\:ss");
            yield return new WaitForSeconds((float)timeSpan.Milliseconds / 1000f);
        }

        _timerText.text = "";
    }


    private void OnEnable()
    {
        UpdateTimer();
    }

    public void PopClick()
    {
        if (!IsPopUnlock)
        {
            Debug.Log("Blocked");
            if (DataManager.Instance.CountCollectedPop >= PopManager.Instance.UnlockPrice)
            {
                Debug.Log("Unlocked!!");
                IsPopUnlock = true;
                DataManager.Instance.CountCollectedPop -= PopManager.Instance.UnlockPrice;
            }
            return;
        }
        if (!IsPopReady) return;
        _lastTimeClick = DateTime.Now;
        SoundManager.Instance.PlayClip(Sound);
        UpdateTimer();
    }

    public void CollectPop()
    {
        _lastTimeClick = DateTime.Now;
        FartTransaction.Instance.SetTransaction(this.transform.position);
        UpdateTimer();
    }
}
