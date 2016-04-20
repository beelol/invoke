using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public GameObject[] mainMenu;

    public GameObject itemCanvas;

    public GameObject spellCanvas;

    public GameObject objectiveCanvas;

    public void Play()
    {
        Application.LoadLevel(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ShowMainMenu()
    {
        foreach (GameObject go in mainMenu)
        {
            go.SetActive(true);
        }
    }
    public void HideMainMenu()
    {
        foreach (GameObject go in mainMenu)
        {
            go.SetActive(false);
        }
    }

    public void ShowItems()
    {
        itemCanvas.SetActive(true);
    }

    public void HideItems()
    {
        itemCanvas.SetActive(false);
    }

    public void ShowSpells()
    {
        spellCanvas.SetActive(true);
    }

    public void HideSpells()
    {
        spellCanvas.SetActive(false);
    }

    public void ShowObjectives()
    {
        objectiveCanvas.SetActive(true);
    }

    public void HideObjectives()
    {
        objectiveCanvas.SetActive(false);
    }
}
