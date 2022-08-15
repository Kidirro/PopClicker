using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField]
    private float _duration;

    [SerializeField]
    private Image _loadingBar;

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

        float time = 0;
        while (time < _duration)
        {
            time += 0.1f;
            _loadingBar.fillAmount = time / _duration;
            yield return new WaitForSeconds(0.1f);
        }

        asyncOperation.allowSceneActivation = true;
    }
}
