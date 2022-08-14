using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopManager : Singleton<PopManager>
{
    [SerializeField]
    private List<Pop> _popsList = new List<Pop>();

    public float CooldownTime;

    public int UnlockPrice;


    private void Start()
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
        foreach (Pop pop in _popsList)
        {
            if (pop.IsPopReady && pop.IsPopUnlock)
            {
                pop.CollectPop();
                DataManager.Instance.SetPopValue(pop.Sound);
            }
        }
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

   
}
