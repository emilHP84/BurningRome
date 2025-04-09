using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    private List<GameObject> PlayerAlive = new List<GameObject>();

    private void Start()
    {

    }

    public void WaitingNewPlayer()
    {

    }

    public void AddNewPlayer(GameObject obj)
    {
        PlayerAlive.Add();
    }

    public void SetLooser(int playerID)
    {
        PlayerAlive.Remove(PlayerAlive[playerID]);
    }

    public void CheckVictory()
    {

    }

    public void SetVictory()
    {
        SceneManager.LoadScene("VictoryScene");
    }

}
