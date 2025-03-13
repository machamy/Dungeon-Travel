using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Entity;

public class StatusManager1 : MonoBehaviour
{
    [SerializeField]
    List<StatusUpdater> friendlyStatusUpdater;
    [SerializeField]
    List<StatusUpdater> enemyStatusUpdater;

    List<Character> friendlyCharacter;
    List<Character> enemyCharacter;

    public void Inistialize(List<Character> friendly, List<Character> enemy)
    {
        gameObject.SetActive(true);
        friendlyCharacter = friendly;
        enemyCharacter = enemy;

        int position = 0;
        foreach (Character c in friendlyCharacter)
        {
            if(c != null)
            {
                friendlyStatusUpdater[position].Initialize(c);
            }
            position++;
        }

        position = 0;
        foreach (Character c in enemyCharacter)
        {
            if(c != null)
            {
                enemyStatusUpdater[position].Initialize(c);
            }
            position++;
        }
    }
}
