using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopManager : Singleton<PopManager>
{
    [SerializeField]
    private List<Pop> _popsList = new List<Pop>();

    public float CooldownTime;

    public int UnlockPrice
    {
        get
        {
            int res = 0;
            foreach (Pop pop in _popsList) res += (pop.IsPopUnlock) ? _unlockPricePoop:0;

            return _unlockPriceDefault + res;
        }
    }


    [SerializeField]
    private int _unlockPriceDefault;
    [SerializeField]
    private int _unlockPricePoop;


    private void Awake()
    {
        InitPops();
    }

    public void InitPops()
    {
        for (int i =0; i < _popsList.Count; i++)
        {
            _popsList[i].Id = i;
        }
    }
    
    public void CollectAllPop()
    {
        int count = 0;
        foreach (Pop pop in _popsList)
        {
            if (pop.IsPopReady && pop.IsPopUnlock)
            {
                pop.CollectPop();
                count += 1;
                DataManager.Instance.SetPopValue(pop.Sound);
            }
        }
        StartCoroutine(IPlayCollectSound(count));
        DataManager.Instance.ShowPopData();
    }

    public bool IsExistCollectPop()
    {
        foreach (Pop pop in _popsList)
        {
            if (pop.IsPopReady && pop.IsPopUnlock) return true;
        }
        return false;
    }

    private IEnumerator IPlayCollectSound(int count)
    {
        for (int i =0; i < count; i++)
        {

            SoundManager.Instance.PlayClip("Collect");
            yield return new WaitForSeconds(0.1f);
        }
    }


    public void UpdateTexts()
    {
        foreach (Pop pop in _popsList) pop.UpdateTextValue();
    }
}
