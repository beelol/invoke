using UnityEngine;
using System.Collections;
using System;
using System.IO;
using PDollarGestureRecognizer;
using System.Collections.Generic;

public class WriteReadPC : MonoBehaviour {

    static string fileName = "gestures.dat";

    public static void SaveGesture(Gesture gesture)
    {
        StreamWriter sr = File.AppendText(fileName);       
        sr.WriteLine("GESTURE:\"" + gesture.Name + "\"" + " {");
        foreach (Point p in gesture.Points)
        {
            if (p != null)
            {
                print(p.X + ", " + p.Y);
                sr.WriteLine("\t" + p.X + ", " + p.Y + ", " + p.StrokeID);
            }
        }
        sr.WriteLine("}");
        sr.Close();
    }

    public static void LoadGestures()
    {
        string line = "";

        string name = "";

        Point point = null;

        List<Point> points = new List<Point>();

        TextAsset gestures = Resources.Load<TextAsset>("Gesture/gestures");

        

        //StreamReader sr = new StreamReader("gestures.dat");
        //while ((line = sr.ReadLine()) != null)
        StringReader sr = new StringReader(gestures.text);
        while ((line = sr.ReadLine()) != null)
        {
            if (line.Contains("GESTURE"))
            {
                int start = line.IndexOf("\"") + 1;
                int length = line.IndexOf("\"", start) - start;
                name = line.Substring(start, length);
                //print(name);
            }

            if (line.Contains(","))
            {
                string[] coords= line.Split(',');

                for(int i = 0; i < coords.Length; i++)
                {
                    coords[i] = coords[i].Trim();
                }

                float x = float.Parse(coords[0]);
                float y = float.Parse(coords[1]);
                int strokeID = int.Parse(coords[2]);

                point = new Point(x, y, strokeID);

                points.Add(point);
            }

            if (line.Contains("}"))
            {
                Point[] pointsArr = points.ToArray();

                Gesture g = new Gesture(pointsArr, name);

                PlayerAI.loadedGestures.Add(g);

                points.Clear();
            }
        }
        sr.Close();
    }
}
