using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour
{
    public GameObject MainMenuUI;
    public GameObject HelpMenuUI;
    public GameObject StartMenuUI;
    public GameObject CreditsMenuUI;
    private ParticleSystem em;


    public void Start()
    {
        em = GetComponentInChildren<ParticleSystem>();
        em.Play();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void LoadStartMenu()
    {
        em.Play();
        StartMenuUI.SetActive(true);
        HelpMenuUI.SetActive(false);
        MainMenuUI.SetActive(false);
        CreditsMenuUI.SetActive(false);

    }
    public void LoadMainMenu()
    {
        MainMenuUI.SetActive(true);
        HelpMenuUI.SetActive(false);
        StartMenuUI.SetActive(false);
        CreditsMenuUI.SetActive(false);

    }
    public void LoadHelpMenu()
    {
        HelpMenuUI.SetActive(true);
        MainMenuUI.SetActive(false);
        StartMenuUI.SetActive(false);
        CreditsMenuUI.SetActive(false);

    }

    public void LoadCreditsMenu()
    {
        HelpMenuUI.SetActive(false);
        MainMenuUI.SetActive(false);
        StartMenuUI.SetActive(false);
        CreditsMenuUI.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
