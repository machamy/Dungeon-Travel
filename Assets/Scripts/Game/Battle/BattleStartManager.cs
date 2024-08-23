using UnityEngine;
using UnityEngine.SceneManagement;
public class BattleStartManager : MonoBehaviour
{
    private GameObject battlePrefab;
    public GameObject battleStartManager;
    public bool isEncounter;

    private void Awake()
    {
        battlePrefab = Resources.Load<GameObject>("BattlePrefabs/battlePrefab");
        Instantiate(battlePrefab, new Vector3(0,0,0), Quaternion.identity);
    }
}
