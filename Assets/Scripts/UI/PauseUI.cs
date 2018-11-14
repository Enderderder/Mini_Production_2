using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public GameObject m_pauseUICanvas;
    public GameObject m_howToPlayCanvas;

    private void Start()
    {
        // Disable the pause menu at the beginning
        m_pauseUICanvas.SetActive(false);
        m_howToPlayCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Esc"))
        {
            if (m_howToPlayCanvas.activeSelf)
            {
                m_pauseUICanvas.SetActive(true);
                m_howToPlayCanvas.SetActive(false);
            }
            else
            {
                TogglePauseMenu();
            }
        }
    }

    /*
     * Change the state of the pause menu
     */
    private void TogglePauseMenu()
    {
        if (m_pauseUICanvas.activeSelf)
        {
            m_pauseUICanvas.SetActive(false);
            m_howToPlayCanvas.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else
        {
            m_pauseUICanvas.SetActive(true);
            m_howToPlayCanvas.SetActive(false);
            Time.timeScale = 0.0f;
        }
    }

    /*
     * Disable the pause menu and resume the game
     */
    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        m_pauseUICanvas.SetActive(false);
    }

    public void ControlsButton()
    {
        m_howToPlayCanvas.SetActive(true);
        m_pauseUICanvas.SetActive(false);
    }

    /*
     * Go back to main menu
     */
    public void BackToMainMenu()
    {
        // Set the time scale back to normal
        Time.timeScale = 1.0f;
        m_pauseUICanvas.SetActive(false);

        // Load the scene back to main menu
        SceneManager.LoadScene("Menu");
    }


}