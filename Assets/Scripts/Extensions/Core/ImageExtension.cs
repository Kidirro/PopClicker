using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ImageExtension
{
    /// <summary>
    /// Set color with lerp.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="initial"></param>
    /// <param name="final"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public static IEnumerator SetColorWithLerp(this Image image, Color initial, Color final, float duration)
    {
        float percentage = 0;
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            percentage = timeElapsed / duration;
            image.color = Color.Lerp(initial, final, percentage);
            yield return null;
        }
    }
}
