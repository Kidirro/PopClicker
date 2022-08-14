using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField]
    private float _duration; 
    
    public static AsyncOperation asyncOperation = null;

    private void Start()
    {
        StartCoroutine(ILoadProcess());
    }

    private IEnumerator ILoadProcess()
    {
        SoundManager.Instance.PlayClip("FartLoading");
       asyncOperation= SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;
        yield return new WaitForSeconds(_duration);
        asyncOperation.allowSceneActivation = true;
    }
}
