using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class loadscene : MonoBehaviour
{
    // Start is called before the first frame update
    public void start()
    {
        SceneManager.LoadScene("start");
    }

    public void lobby(){
        SceneManager.LoadScene(4);
    }

    public void game(){
        
        SceneManager.LoadScene(5);
    }
    
    public void register(){
        SceneManager.LoadScene(2);
    }
    
    public void login(){
        SceneManager.LoadScene(3);
    }

     
    public void win(){
        SceneManager.LoadScene(6);
    }
}
