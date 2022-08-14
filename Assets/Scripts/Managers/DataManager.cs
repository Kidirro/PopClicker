using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public int CountCollectedPop
    {
        get => PlayerPrefs.GetInt("PopCollectedCount", 0);
        set => PlayerPrefs.SetInt("PopCollectedCount", value);
    }
    public string GetPopValue(int id)
    {
        return PlayerPrefs.GetString("PopCollected[" + id + "]", "");
    }

    public void SetPopValue(string value)
    {
        PlayerPrefs.SetString("PopCollected[" + CountCollectedPop + "]", value);
        CountCollectedPop += 1;
    }

    public void ClearPopValue()
    {
        CountCollectedPop = 0;
    }

    public void ShowPopData()
    {
        Debug.Log("Count:" + CountCollectedPop);
        for (int i = 0; i < CountCollectedPop; i++)
        {
            Debug.Log(i + " id :" + GetPopValue(i));
        }
    }
}
