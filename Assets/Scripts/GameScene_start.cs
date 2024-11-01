using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScene_start : MonoBehaviour
{
   
    void Start()
    {
        
    }

   
    void Update()
    {
        
    }

   public void StartGame()
   {
        SceneManager.LoadScene("Game");
   }
}
