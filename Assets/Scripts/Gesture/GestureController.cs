using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PDollarGestureRecognizer;
public class GestureController : MonoBehaviour
{
    //Camera to draw for
    public Camera camera;

    // Line renderer componenent.
    private LineRenderer line;

    // Is the mouse being held?
    private bool isMouseHeld = false;

    // List of points for the line renderer
    private List<Vector3> pointsList = new List<Vector3>();

    // Stroke gameobject prefab
    public GameObject strokePrefab;

    // List of all strokes.
    public static List<GameObject> strokes = new List<GameObject>();

    // Have we already detected the gesture?
    private bool detectedGesture = false;

    // Time at which the mouse is pressed.
    float timeOnMousePress = 0;

    // List of points.
    static List<Point> pc = new List<Point>();

    // Current strokeID to assign.
    private int CID = 0;

    private Color originalColor = new Color(128, 0, 128);

    private Vector2 onPressedPosition = Vector2.zero;

    bool draw = false;

    // Loads gestures from file.
    void Start()
    {
        WriteReadPC.LoadGestures();
    }

    // Determines what the player is doing with the mouse.    
    void Update()
    {
        //if (!GameController.player) return;

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
       
        // If mouse button down, remove old line and set its color to green        
        if (Input.GetMouseButtonDown(0))
        {
            onPressedPosition = Input.mousePosition;

            timeOnMousePress = Time.time;
        }

        if (Input.GetMouseButton(0))
        {

            if (!isMouseHeld)
            {
                //print(Time.time + " - " + timeOnMousePress + " = " + (Time.time - timeOnMousePress));

                // Create a stroke if we are holding and not clicking.                

                PrepareStroke();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            // If the user was clicking:
            if (!isMouseHeld)
            {
                GameController.player.GetComponent<PlayerAI>().SetDestination(Input.mousePosition, false);
            }

            // If the user was drawing:
            else
            {
                CID++;
            }
            isMouseHeld = false;
            GetComponent<ParticleSystem>().Stop();
            draw = false;
        }

        // Drawing line when mouse is moving.
        if (draw)
        {
            RecordPoint();
            DrawGesture();
        }
        // Clear the strokes if we're not holding the mouse.
        else
        {
            ClearStrokes();
        }
    }

    public static void Detect()
    {
        Point[] pcArr = pc.ToArray();

        Gesture g = new Gesture(pc.ToArray());

        PlayerAI.DetectSpell(g, PlayerAI.loadedGestures.ToArray());

        Clear();
    }

    private void RecordPoint()
    {
        Vector2 mousePos = Input.mousePosition;
        pc.Add(new Point(mousePos.x, mousePos.y, CID));
    }

    private void PrepareStroke()
    {       
        bool resetDrawingColor = false;
        if (Vector2.Distance(Input.mousePosition, onPressedPosition) > 50f) { draw = true; resetDrawingColor = true; }

        if (resetDrawingColor)
        {
            ResetDrawingColor();
            resetDrawingColor = false;
        }

        //print(Vector2.Distance(Input.mousePosition, onPressedPosition));

        if (draw)
        {
            //if (Time.time - timeOnMousePress >= 0.08f)
            //{
                isMouseHeld = true;
                detectedGesture = false;
                GameObject stroke = Instantiate(strokePrefab);
                strokes.Add(stroke);
                line = stroke.GetComponent<LineRenderer>();
                line.material = new Material(Shader.Find("Particles/Additive"));
                line.SetWidth(0.1f, 0.1f);
                line.SetColors(originalColor, originalColor);
                line.useWorldSpace = true;

                pointsList.Clear();

                GetComponent<ParticleSystem>().Play();
            //}
        }
    }

    private void ResetDrawingColor()
    {
        if (strokes.Count > 0)
        {
            foreach (GameObject stroke in strokes)
            {
                stroke.GetComponent<LineRenderer>().material.SetColor("_TintColor", originalColor);
            }
        }
    }

    private void DrawGesture()
    {
        Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition); mousePos.z = 0;
        if (!pointsList.Contains(mousePos))
        {
            pointsList.Add(mousePos);
            line.SetVertexCount(pointsList.Count);
            line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
        }
    }

    private void ClearStrokes()
    {
        if (strokes.Count > 0)
        {
            foreach (GameObject stroke in strokes)
            {
                Color faded = stroke.GetComponent<LineRenderer>().material.GetColor("_TintColor");
                faded.a = Mathf.Lerp(faded.a, 0, 0.1f);
                stroke.GetComponent<LineRenderer>().material.SetColor("_TintColor", faded);

                if (!detectedGesture)
                {
                    if (stroke.GetComponent<LineRenderer>().material.GetColor("_TintColor").a <= 0.15)
                    {
                        Detect();
                        detectedGesture = true;
                    }
                }
            }
            if (detectedGesture)
            {
                DeleteStrokes();
            }
        }
    }

    public static void Clear()
    {        
        pc.Clear();
    }

    public static void DeleteStrokes()
    {
        if (strokes.Count > 0)
        {
            foreach (GameObject stroke in strokes)
            {
                Destroy(stroke);
            }
        }

        strokes.Clear();
    }
}