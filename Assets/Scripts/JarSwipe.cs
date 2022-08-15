using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JarSwipe : Singleton<JarSwipe>, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Swipe properties"), SerializeField]
    private Vector2 _beginVector;

    public Vector2 BeginVector
    {
        get => new Vector2(_beginVector.x / _defaultResolution.x * Camera.main.pixelWidth, _beginVector.y / _defaultResolution.y * Camera.main.pixelHeight);
    }

    [SerializeField]
    private Vector2 _endVector;
    public Vector2 EndVector
    {
        get => new Vector2(_endVector.x / _defaultResolution.x * Camera.main.pixelWidth, _endVector.y / _defaultResolution.y * Camera.main.pixelHeight);
    }

    [SerializeField]
    private float _magnitude;

    [SerializeField]
    private Vector2 _defaultResolution;

    private bool _isStartSwipe = false;

    private float _startPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {

        Debug.Log(eventData.pressPosition);
        _isStartSwipe = ((eventData.pressPosition - BeginVector).magnitude < _magnitude);
        _startPosition = eventData.pressPosition.x;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Jar.Instance.SetPositionLidSub(Mathf.Max(0,eventData.position.x - _startPosition));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.position);
        if ((eventData.position - EndVector).magnitude < _magnitude && _isStartSwipe) StartCoroutine(Jar.Instance.Obosramtus());
        _isStartSwipe= false;
        Jar.Instance.ClearPosition();
    }
}
