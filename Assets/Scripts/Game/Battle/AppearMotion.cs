using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearMotion : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(ApearMenuAnimation(this.gameObject));
    }

    IEnumerator ApearMenuAnimation(GameObject menu)
    {
        RectTransform ObjRectTransform = menu.GetComponent<RectTransform>();

        Vector2 startPosition = ObjRectTransform.anchoredPosition;
        Vector2 targetPosition = new Vector2(-500f, 0f);

        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            ObjRectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ObjRectTransform.anchoredPosition = targetPosition;
        yield break;
    }
}
