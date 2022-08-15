using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerAnimation : MonoBehaviour
{


    private void OnEnable()
    {
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        Vector2 startVect = Camera.main.ScreenToWorldPoint(JarSwipe.Instance.BeginVector);
        Vector2 endVect = Camera.main.ScreenToWorldPoint(JarSwipe.Instance.EndVector);

        while (true)
        {
            StartCoroutine(this.gameObject.transform.SetPositionWithLerp(startVect, endVect, 1));
            yield return new WaitForSeconds(2f);
        }
    }
}
