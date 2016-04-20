using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// The Control script provides a singleton reference to modify the player's ability
/// to control the game.
/// 
/// This script is attached to the GameController GameObject
/// </summary>

public class Control : MonoBehaviour
{

    #region Variables

    public static bool allowPlayerControls = true;
    public static bool inStrafeMode = false;
    public static bool allowMenuControls = true;
    public static bool allowAlphaKeyCommands = true;
    public GameObject playerObject;

    #endregion

    #region Functions
    /* Instantiates a GameObject. */
    public static GameObject CreateGameObject(GameObject gameobject, Vector3 pos, Quaternion rot)
    {
        return (GameObject)Instantiate(gameobject, pos, rot);
    }

    /* Sets the player's ability to control his units to true or false.
     * The purpose of this is to disable when using UI features, upon death,
     * or in other situations where it may be required.
     */
    public static void ToggleAllowPlayerControls(bool state)
    {
        if (state) { 
            {
                allowPlayerControls = true;
            }
        }
        else
        {
            allowPlayerControls = false;
        }
    }

    #endregion

}
