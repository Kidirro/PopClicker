using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{

    public static bool IsWasTutorial
    {
        get => PlayerPrefs.GetInt("IsWasTutorial", 0) == 1;
        set => PlayerPrefs.SetInt("IsWasTutorial", (value) ? 1 : 0);
    }

    public static int CurrentState = 0;

    [SerializeField]
    private List<ListWrapper> _lockedGameObjects = new List<ListWrapper>();

    [SerializeField]
    private List<ListWrapper> _tutorialGameObjects = new List<ListWrapper>();

    [Serializable]
    public class ListWrapper
    {
        public List<GameObject> List;
    }

    private void Start()
    {
        if (IsWasTutorial) return;
        UpdateStateObjects();
    }

    public void UpdateStateObjects()
    {
        if (CurrentState > 0)
        {

            
          foreach (GameObject gameObject in _lockedGameObjects[CurrentState - 1].List)
            {
                gameObject.SetActive(true);
            }
          foreach (GameObject gameObject in _tutorialGameObjects[CurrentState - 1].List)
            {
                gameObject.SetActive(false);
            }
        }
        if (CurrentState < 5)
        {

            foreach (GameObject gameObject in _lockedGameObjects[CurrentState].List)
            {
                gameObject.SetActive(false);
            }
            foreach (GameObject gameObject in _tutorialGameObjects[CurrentState].List)
            {
                gameObject.SetActive(true);
            }
        }
    }
}
