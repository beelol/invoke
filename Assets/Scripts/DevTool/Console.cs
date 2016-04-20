using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// The Console script is used to call commands in developer mode
/// for game testing and modding.
/// 
/// This script is attached to the GameController GameObject.
/// </summary>
public class Console : MonoBehaviour
{
    private enum MessageType {command, message}

    #region Variables

    Stats player;

    private string communication;
    private string command;
    public bool unlockCursor = false;

    //Quick references
    private string objectClickedName = "Console";
    private Transform objectClicked;

    public InputField input;
    public Image consoleBacking;
    public Image consoleBacking2;
    public Text consoleOutput;

    private RectTransform inputRect;
    private RectTransform consoleBackingRect;
    private RectTransform consoleBackingRect2;

    Vector2 inputPosition;
    Vector2 consoleBackingPosition;
    Vector2 consoleBackingPosition2;

    #endregion

    #region Functions

    /* Sets initial values. */
    void Start()
    {
        consoleOutput.text = "";
        inputRect = input.GetComponent<RectTransform>();
        consoleBackingRect = consoleBacking.GetComponent<RectTransform>();
        consoleBackingRect2 = consoleBacking2.GetComponent<RectTransform>();

        inputPosition = inputRect.anchoredPosition;
        consoleBackingPosition = consoleBackingRect.anchoredPosition;
        consoleBackingPosition2 = consoleBackingRect2.anchoredPosition;
        
        input.onEndEdit.AddListener(OnSubmit);

        player = GameController.player.GetComponent<Stats>();
    }

    /* Handles displaying Console. */
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F1))
    //    {
    //        Interface.showConsole = !Interface.showConsole;

    //        if (Interface.showConsole)
    //        {
    //            EventSystem.current.SetSelectedGameObject(input.gameObject, null);
    //            Control.allowAlphaKeyCommands = false;
    //        }
    //        else 
    //        { 
    //            //EventSystem.current.SetSelectedGameObject(null, null);                
    //            Control.allowAlphaKeyCommands = true;
    //        }
    //        Control.ToggleAllowPlayerControls(!Interface.showConsole);

    //        inputPosition = inputRect.anchoredPosition;
    //        consoleBackingPosition = consoleBackingRect.anchoredPosition;
    //        consoleBackingPosition2 = consoleBackingRect2.anchoredPosition;
    //    }

    //    if (Input.GetMouseButtonDown(0) && Interface.showConsole)
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;
    //        if (Physics.Raycast(ray, out hit, 1000))
    //        {
    //            objectClicked = hit.transform;
    //            objectClickedName = "\"" + hit.transform.name + "\"";
    //        }
    //    }

    //    if (Interface.showConsole)
    //    {
    //        UITools.Translate(inputRect,
    //            new Vector2(inputRect.anchoredPosition.x,
    //                23-256),
    //                Time.deltaTime * 10);

    //        UITools.Translate(consoleBackingRect,
    //            new Vector2(consoleBackingRect.anchoredPosition.x,
    //                -128),
    //                Time.deltaTime * 10);

    //        UITools.Translate(consoleBackingRect2,
    //            new Vector2(consoleBackingRect2.anchoredPosition.x,
    //                -128),
    //                Time.deltaTime * 10);

    //        UITools.Translate(consoleOutput.rectTransform,
    //            new Vector2(consoleOutput.rectTransform.anchoredPosition.x,
    //                144-256),
    //                Time.deltaTime * 10);
    //    }
    //    if (!Interface.showConsole)
    //    {
    //        UITools.Translate(inputRect,
    //            new Vector2(inputRect.anchoredPosition.x,
    //                23),
    //                Time.deltaTime * 10);

    //        UITools.Translate(consoleBackingRect,
    //            new Vector2(consoleBackingRect.anchoredPosition.x,
    //                128),
    //                Time.deltaTime * 10);

    //        UITools.Translate(consoleBackingRect2,
    //            new Vector2(consoleBackingRect2.anchoredPosition.x,
    //                128),
    //                Time.deltaTime * 10);

    //        UITools.Translate(consoleOutput.rectTransform,
    //            new Vector2(consoleOutput.rectTransform.anchoredPosition.x,
    //                144),
    //                Time.deltaTime * 10);
    //    }
    //}

    /* Called upon submitting command. */
    void OnSubmit(string line)
    {
        DisplayCommand(input.text, MessageType.command);
        input.text = "";
        //EventSystem.current.SetSelectedGameObject(input.gameObject, null);
    }

    /* Displays a command or a message. */
    void DisplayCommand(string message, MessageType MT)
    {
        //Only send a message if the textfield isn't empty.
        //send message
        command = message;
        consoleOutput.text = " > " + message + "\n" + consoleOutput.text;

        //This string is displayed by the label in the CommLogWindow.

        //now run command
        if(MT == MessageType.command)
            RunCommand(command);

    }

    /* Executes a command with passed string. */
    void RunCommand(string command)
    {
        command = command.ToLower();
        char[] delimiterChars = { ' ' };
        string[] words = command.Split(delimiterChars);

        if (words.Length < 1)
            return;

        switch (words[0])
        {
            case "selected":
                if (words.Length < 2)
                {
                    DisplayCommand("\"" + command + "\" is not a valid command.", MessageType.message);
                    break;
                }
                switch (words[1])
                {
                    case "setposition":
                        if (words.Length < 5)
                        {
                            DisplayCommand("\"" + command + "\" is not a valid command.", MessageType.message);
                            break;
                        }
                        
                        {
                            bool isNumericOne;
                            bool isNumericTwo;
                            bool isNumericThree;
                            float vectorX;
                            float vectorY;
                            float vectorZ;
                            isNumericOne = float.TryParse(words[2], out vectorX);
                            isNumericTwo = float.TryParse(words[3], out vectorY);
                            isNumericThree = float.TryParse(words[4], out vectorZ);
                            if (isNumericOne && isNumericTwo && isNumericThree)
                            {
                                objectClicked.transform.position = new Vector3(vectorX, vectorY, vectorZ);
                            }
                        }
                        break;
                }
                break;

            case "player":
                if (words.Length < 2)
                {
                    DisplayCommand("\"" + command + "\" is not a valid command.", MessageType.message);
                    break;
                }

                switch (words[1])
                {
                    //case "sethealth":
                    //    {
                    //        if (words.Length < 3)
                    //        {
                    //            DisplayCommand("\"" + command + "\" is not a valid command.", MessageType.message);
                    //            break;
                    //        }

                    //        int newHealth;
                    //        bool isNumeric = int.TryParse(words[2], out newHealth);

                    //        if (isNumeric)
                    //            player.SetHealth(newHealth);
                    //        else
                    //        {
                    //            DisplayCommand("\"" + command + "\" is not a valid command.", MessageType.message);
                    //            break;
                    //        }
                    //    }
                    //    break;

                    //case "setspeed":
                    //    {
                    //        if (words.Length < 3)
                    //        {
                    //            DisplayCommand("\"" + command + "\" is not a valid command.", MessageType.message);
                    //            break;
                    //        }

                    //        float newSpeed;
                    //        bool isNumeric = float.TryParse(words[2], out newSpeed);

                    //        if (isNumeric)
                    //            player.SetSpeed(newSpeed);
                    //        else
                    //        {
                    //           DisplayCommand("\"" + command + "\" is not a valid command.", MessageType.message);
                    //            break;
                    //        }
                    //    }
                    //    break;

                    case "setposition":
                        if (words.Length < 5)
                        {
                            DisplayCommand("\"" + command + "\" is not a valid command.", MessageType.message);
                            break;
                        }
                        {
                            bool isNumericOne;
                            bool isNumericTwo;
                            bool isNumericThree;
                            float vectorX;
                            float vectorY;
                            float vectorZ;
                            isNumericOne = float.TryParse(words[2], out vectorX);
                            isNumericTwo = float.TryParse(words[3], out vectorY);
                            isNumericThree = float.TryParse(words[4], out vectorZ);
                            if (isNumericOne && isNumericTwo && isNumericThree)
                            {
                                player.gameObject.transform.position = new Vector3(vectorX, vectorY, vectorZ);
                            }
                        }
                        break;                    
                }
                break;

            //case "kill":
            //    player.stats.TakeDamage(Player.entity.stats.maxHealth, "console", null);
            //    break;

            case "exit":
                if (!Application.isEditor && !Application.isWebPlayer)
                    Application.Quit();
                else
                    DisplayCommand("You may not quit using this client.", MessageType.message);
                break;

            case "clear":
                if (words.Length < 2)
                {
                    DisplayCommand("\"" + command + "\" is not a valid command.", MessageType.message);
                    break;
                }

                {
                    int count = 0;
                    string objectTag = words[1];
                    GameObject[] objectList;
                    objectList = GameObject.FindGameObjectsWithTag(objectTag);
                    foreach (GameObject objectInList in objectList)
                    {
                        count++;
                        GameObject.Destroy(objectInList);
                    }
                    DisplayCommand("Removed " + count + " GameObjects with tag \"" + objectTag + ".\"", MessageType.message);
                }
                break;

            default:
                DisplayCommand("\"" + command + "\" is not a valid command.", MessageType.message);
                break;
        }

    }

    #endregion
}
