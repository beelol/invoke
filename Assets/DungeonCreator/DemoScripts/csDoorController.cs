using UnityEngine;
using System.Collections;

public class csDoorController 
	: MonoBehaviour 
{
	
	private bool opening = false;


	public void OnTriggerEnter(Collider other)
	{
		opening = true;				
	}
	
	public void OnTriggerExit(Collider other)
	{        
        opening = false;				
	}
	
	public void Update()
	{
		Transform door = transform.FindChild("door");
		
		if (opening)
		{
			door.position = Vector3.Lerp(door.position, transform.TransformPoint(new Vector3(0, -0.985f, 0)), Time.deltaTime * 5);
		}
		else
		{
            door.position = Vector3.Lerp(door.position, transform.TransformPoint(new Vector3(0, 0, 0)), Time.deltaTime * 5);	
		}
	}
}
