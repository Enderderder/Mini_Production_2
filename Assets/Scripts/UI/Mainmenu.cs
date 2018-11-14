using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mainmenu : MonoBehaviour {

    //public Image 

    public void Button_Start()
    {
        SceneManager.LoadScene("DarrenScene");
    }

    public void Button_Controls()
    {
        // Change to controls menu
    }

    public void Button_Exit()
    {
        Application.Quit();
    }
}
