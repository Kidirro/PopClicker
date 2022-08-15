using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FartTransaction : Singleton<FartTransaction>
{
    [SerializeField]
    private float _duration;

    [SerializeField]
    private GameObject _coinPrefab;

    [SerializeField]
    private GameObject _coinGoal;

    [SerializeField]
    private List<Transform> _parentTransform = new List<Transform>();

    private List<GameObject> _coinList = new List<GameObject>();

    private int _currentCoin = 0;

    public void SetTransaction(Vector2 initial, Vector2 target, int id = 0)
    {
        _currentCoin++;
        if (_coinList.Count < _currentCoin)
        {
            GameObject coin = Instantiate(_coinPrefab);
            _coinList.Add(coin);
        }

        _coinList[_currentCoin - 1].transform.SetParent(_parentTransform[id]);
        _coinList[_currentCoin - 1].transform.localScale = Vector3.one;
        _coinList[_currentCoin - 1].transform.position = Vector3.zero;
        StartCoroutine(IETransaction(_coinList[_currentCoin - 1], initial,  target, false));
    } 
    
    public void SetTransactionToJar(Vector2 initial,Vector2 initialSize,Vector2 targetSize, int id =0, bool isNeedUpdate = false)
    {
        _currentCoin++;
        if (_coinList.Count < _currentCoin)
        {
            GameObject coin = Instantiate(_coinPrefab);
            _coinList.Add(coin);
        }

        _coinList[_currentCoin - 1].transform.SetParent(_parentTransform[id]);
        _coinList[_currentCoin - 1].transform.localScale = Vector3.one;
        _coinList[_currentCoin - 1].transform.position = Vector3.zero;
        StartCoroutine(IETransaction(_coinList[_currentCoin - 1], initial, _coinGoal.transform.position, isNeedUpdate));
        StartCoroutine(IEScale(_coinList[_currentCoin - 1], initialSize, targetSize));
    }  

    private IEnumerator IETransaction(GameObject coin, Vector2 initial, Vector2 target, bool isNeedUpdate)
    {
        coin.SetActive(true);
        StartCoroutine(coin.transform.SetPositionWithLerp(initial, target, _duration));
        yield return new WaitForSeconds(_duration);
        coin.SetActive(false);
        _currentCoin -= 1;
        if (isNeedUpdate) Jar.Instance.UpdateText();
    }   
    
    private IEnumerator IEScale(GameObject coin, Vector2 initial, Vector2 target)
    {
        coin.SetActive(true);
        StartCoroutine(coin.transform.ScaleWithLerp(initial, target, _duration));
        yield return new WaitForSeconds(_duration);
    }
}
