using UnityEngine;
using UnityEngine.SceneManagement;
public class BattleStartManager : MonoBehaviour
{
    public bool isEncounter;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Encounter();
    }
    
    public void Encounter()
    {
        isEncounter = true;
        SceneChange();
    }

    public void SymbolAttack()
    {
        isEncounter = false;
        SceneChange();
    }

    public void SceneChange()
    {
        //SceneManager.LoadScene("BattleScene");

        GameObject battlemanagerGO = GameObject.Find("BattleSystem");
        BattleManager battlemanager = battlemanagerGO.GetComponent<BattleManager>();
        battlemanager.isEncounter = isEncounter;

        Debug.Log("isEncounter: " + isEncounter);
        Destroy(this.gameObject);
    }
}
