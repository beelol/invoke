using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// This script is attached to the player's parenting gameobject.
/// 
/// This script displays a hearts representing health at the top right of the screen.
/// 
/// This script accesses Stats.cs to retrieve health information. Stats.cs is attached to the same gameobject. 
/// </summary>

public class StatDisplay : MonoBehaviour
{
    #region Variables

    // Spawn position of heart.
    private Vector2 firstPos = new Vector2(-30, -30);

    // The heart to display.
    public GameObject heartPrefab;

    // The player's stats component.
    private Stats stats;

    // The last heart that was spawned.
    private GameObject lastHeart;

    // Width of heart rect transform.
    private float width;

    // Height of heart rect transform.
    private float height;

    // Primary canvas.
    GameObject canvas;

    // List of hearts for access when changing hp.
    List<GameObject> hearts = new List<GameObject>();

    #endregion

    #region Functions
    void Start()
    {
        canvas = GameObject.Find("Canvas");

        width = heartPrefab.GetComponent<RectTransform>().rect.width;

        height = heartPrefab.GetComponent<RectTransform>().rect.height;

        stats = GetComponent<Stats>();

        UpdateInitialHealth();

    }

    #region Health Updaters

    //#region Uncomment To Test Health

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Minus))
    //    {
    //        GetComponent<Stats>().ChangeMaxHealth(-1);
    //    }

    //    if (Input.GetKeyDown(KeyCode.PageUp))
    //    {
    //        GetComponent<Stats>().ChangeMaxHealth(1);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Insert))
    //    {
    //        GetComponent<Stats>().ChangeHealth(-1);
    //    }

    //    if (Input.GetKeyDown(KeyCode.Home))
    //    {
    //        GetComponent<Stats>().ChangeHealth(1);
    //    }
    //}
    //#endregion

    /* Create hearts for player's beginning health. */
    private void UpdateInitialHealth()
    {
        for (int i = 0; i < stats.maxHealth; i++)
        {
            if (i == 0)
            {
                lastHeart = Instantiate(heartPrefab);
                lastHeart.transform.SetParent(canvas.transform);
                lastHeart.GetComponent<RectTransform>().anchoredPosition = firstPos;
            }
            else
            {
                if (i % 10 == 0)
                {
                    GameObject newHeart = Instantiate(heartPrefab);
                    newHeart.transform.SetParent(canvas.transform);
                    newHeart.GetComponent<RectTransform>().anchoredPosition = hearts[i-10].GetComponent<RectTransform>().anchoredPosition - new Vector2(0, height);
                    lastHeart = newHeart;
                }
                else
                {
                    GameObject newHeart = Instantiate(heartPrefab);
                    newHeart.transform.SetParent(canvas.transform);
                    newHeart.GetComponent<RectTransform>().anchoredPosition = hearts[hearts.Count - 1].GetComponent<RectTransform>().anchoredPosition - new Vector2(width, 0);
                    lastHeart = newHeart;
                }
            }
            hearts.Add(lastHeart);
        }

        for (int i = 0; i < stats.currentHealth; i++)
        {
            hearts[i].transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    /* Updates the display of max health. */
    public void UpdateMaxHealth(int oldHealth)
    {
        // If we are gaining max health:
        if (stats.maxHealth > oldHealth)
        {
            // Then add from the old health's index, up to the new health.
            for (int i = oldHealth; i < stats.maxHealth; i++)
            {
                if (i % 10 == 0)
                {
                    GameObject newHeart = Instantiate(heartPrefab);
                    newHeart.transform.SetParent(canvas.transform);
                    newHeart.GetComponent<RectTransform>().anchoredPosition = hearts[i - 10].GetComponent<RectTransform>().anchoredPosition - new Vector2(0, height);
                    lastHeart = newHeart;
                }
                else
                {
                    GameObject newHeart = Instantiate(heartPrefab);
                    newHeart.transform.SetParent(canvas.transform);
                    newHeart.GetComponent<RectTransform>().anchoredPosition = hearts[hearts.Count-1].GetComponent<RectTransform>().anchoredPosition - new Vector2(width, 0);
                    lastHeart = newHeart;
                }
                hearts.Add(lastHeart);
            }
        }
        // If we are reducing max health:
        else if (stats.maxHealth < oldHealth)
        {
            // Then remove from the new health's index up to the old one.
            for (int i = stats.maxHealth; i < oldHealth; i++)
            {
                if (hearts.Count > 0)
                {
                    GameObject removedHeart = hearts[hearts.Count - 1];
                    hearts.RemoveAt(hearts.Count - 1);
                    Destroy(removedHeart);
                }
            }
        }
    }

    /* Updates the display of current health. */
    public void UpdateCurrentHealth(int oldHealth)
    {
        // If we are healing:
        if (stats.currentHealth > oldHealth)
        {
            // Then add from the old health's index, up to the new health.
            for (int i = oldHealth; i < stats.currentHealth; i++)
            {
                hearts[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        // If we are taking damage:
        else if (stats.currentHealth < oldHealth)
        {
            // Then remove from the new health's index up to the old one.
            for (int i = stats.currentHealth; i < oldHealth; i++)
            {
                hearts[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    #endregion

    #endregion
}
