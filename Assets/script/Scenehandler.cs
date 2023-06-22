using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenehandler : MonoBehaviour
{
    public GameObject settingsPanel;
   public  void register()
    {
        SceneManager.LoadScene("scn_register");
    }

public void login(){
    SceneManager.LoadScene("scn_login");
}
public void scnloading(){
    SceneManager.LoadScene("scn_loading");
}

}
