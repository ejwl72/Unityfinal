using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void SceneGame()
    {
        SceneManager.LoadScene("Navmesh_monster");
    }
    public void Option()
    {
        
    }
    public void Exit()
    {
        Application.Quit();
        Debug.Log("game exit");
    }
}
