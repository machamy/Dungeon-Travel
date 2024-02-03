using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AutoScroll : MonoBehaviour
{
    public int height = 80, amount = 6;
    public int posY = 0;
    public int posN, posNInScreen = 0;

    private void Update()
    {
        posN = EventSystem.current.currentSelectedGameObject.
            transform.parent.GetSiblingIndex();
        posNInScreen = posN - posY / height;

        if (posNInScreen > amount)
        {
            posY += height;
            posNInScreen = amount;
        }
        else if (posNInScreen < 0)
        {
            posY -= height;
            posNInScreen = 0;
        }

        transform.localPosition = new Vector3(316.5f, posY, 0);
    }
}
