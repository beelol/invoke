using UnityEngine;
using System.Collections;
/// <summary>
/// Attached to player, draws healthbars of players above them.
/// accesses Stats.cs to find healthbarlength
/// 
/// accessed by PlayerName.cs
/// </summary>
public class PlayerLabel : MonoBehaviour {
	//Variables

    public Texture healthCircle;

	public Texture healthTex;

	public Texture manaTex;

	private Camera myCamera;

	private Transform myTransform;

	private Transform triggerTransform;

	private Vector3 worldPosition = new Vector3();

	private Vector3 screenPosition = new Vector3();
		
	private int barTop = 20;

	private int healthBarHeight = 5;

	private int healthBarLeft = 50;

	private float healthBarLength;

	private float adjustment = 1;

	private GUIStyle myStyle = new GUIStyle();

    public Stats stats;
	//variables

	void Start () {

		myTransform = transform;

        myCamera = Camera.main;

		myStyle.fontSize = 12;

		myStyle.fontStyle = FontStyle.Bold;

		//Don't allow text to extend beyond width of label

		myStyle.clipping = TextClipping.Clip;

	}
	
	// Update is called once per frame
	void Update () {
	
		//access Stats.cs to figure out how long the health bar should be
		//if health bar lengh <=1 hbr = 1 avoid glitch
		
		stats = transform.GetComponent<Stats>();

		if (stats.currentHealth < 1) { 
			healthBarLength = 1;
		}
        else if (stats.currentHealth >= 1)
        {
            healthBarLength = (stats.currentHealth / stats.maxHealth) * 50;
		}

	}

	void OnGUI(){

		//stuff for fps don't display if behind the player

		worldPosition = new Vector3 (myTransform.position.x, myTransform.position.y, myTransform.position.z);

		screenPosition = myCamera.WorldToScreenPoint (worldPosition);


		GUI.Box(new Rect(screenPosition.x - healthBarLeft/2, Screen.height - screenPosition.y - barTop, 50, healthBarHeight), "");

		GUI.DrawTexture(new Rect (screenPosition.x - healthBarLeft/2, Screen.height - screenPosition.y - barTop, healthBarLength, healthBarHeight), healthTex);


		//draw player's name

		//GUI.Label (new Rect (screenPosition.x - labelWidth / 2 + 30, Screen.height - screenPosition.y - labelTop - 10, labelWidth, labelHeight), playerName, myStyle);

	}


















}
