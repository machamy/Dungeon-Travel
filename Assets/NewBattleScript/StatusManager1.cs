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

    List<UnitHolder> friendlyCharacter;
    List<UnitHolder> enemyCharacter;

    public void Inistialize(List<UnitHolder> friendly, List<UnitHolder> enemy)
    {
        gameObject.SetActive(true);
        friendlyCharacter = friendly;
        enemyCharacter = enemy;

        int position = 0;
        foreach (UnitHolder c in friendlyCharacter)
        {
            if(c != null)
            {
                friendlyStatusUpdater[position].Initialize(c);
            }
            position++;
        }

        position = 0;
        foreach (UnitHolder c in enemyCharacter)
        {
            if(c != null)
            {
                enemyStatusUpdater[position].Initialize(c);
            }
            position++;
        }
    }

    public IEnumerator DestroyAll()
    {
        foreach (StatusUpdater updater in friendlyStatusUpdater) updater.Destroy();
        foreach (StatusUpdater updater in enemyStatusUpdater) updater.Destroy();
        yield return null;
    }
}
