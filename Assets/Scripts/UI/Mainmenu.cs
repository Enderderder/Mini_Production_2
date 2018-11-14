using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;

public class Mainmenu : MonoBehaviour {

    public GameObject menuCanvas;
    public GameObject howToPlayCanvas;

    private void Start()
    {
        menuCanvas.SetActive(true);
        howToPlayCanvas.SetActive(false);
    }

    public void Button_Start()
    {
        SceneManager.LoadScene("DarrenScene");
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (InputManager.ActiveDevice.GetControl(InputControlType.Action2).WasPressed)
        {
            if (howToPlayCanvas.activeSelf)
            {
                menuCanvas.SetActive(true);
                howToPlayCanvas.SetActive(false);
            }
        }
    }

    public void Button_Controls()
    {
        // Change to controls menu
        menuCanvas.SetActive(false);
        howToPlayCanvas.SetActive(true);
    }

    public void Button_Exit()
    {
        Application.Quit();
    }
}
