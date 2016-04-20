using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PDollarGestureRecognizer;
using System;
using System.IO;
using UnityEngine.UI;

// Create line renderer component and set its property
//line = GetComponent<LineRenderer>();
//line.material = new Material(Shader.Find("Particles/Additive"));
//line.SetVertexCount(0);
//line.SetWidth(0.1f, 0.1f);
//line.SetColors(Color.green, Color.green);
//line.useWorldSpace = true;

public class PointInput : MonoBehaviour
{
    static List<Point> pc = new List<Point>();
    static Text staticText;
    public Text text;

    //vars
    private int CID = 0;
    //vars

    public static void Clear()
    {
        pc.Clear();
    }

    void Start()
    {
        WriteReadPC.LoadGestures();
        staticText = text;
    }

    // Update is called once per frame
    void Update()
    {
        // If we are over a ui element, do nothing
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

        // If we are holding the mouse down, draw
        if (Input.GetMouseButton(0))
        {
            RecordPoint();
        }
        
        // Else if we let go, increase the stroke id.        
        else if(Input.GetMouseButtonUp(0))
        {
            CID++;
        }

        if (Input.GetKeyDown(KeyCode.K))//temp using to write array to file DELETE LATER!
        {
            Detect();
        }
    }

    private void RecordPoint()
    {
        Vector2 mousePos = Input.mousePosition;
        pc.Add(new Point(mousePos.x, mousePos.y, CID));
    }

    public static void Detect()
    {
        print(pc.Count);

        Point[] pcArr = pc.ToArray();

        Gesture g = new Gesture(pc.ToArray());

        PlayerAI.DetectSpell(g, PlayerAI.loadedGestures.ToArray());

        Clear();
    }

    public void Save()
    {
        Point[] pcArr = pc.ToArray();

        Gesture g = new Gesture(pc.ToArray(), text.text);

        WriteReadPC.SaveGesture(g);

        text.text = "";
    }
}
