using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//[CustomEditor(typeof(ceDungeonGenerator))]
public class ceDungeonGeneratorEditor 
	: Editor 
{
	public override void OnInspectorGUI()
	{	
		ceDungeonGenerator dG 
			= target as ceDungeonGenerator;		
		
		dG.DungeonSize 
			= (ceDungeonGenerator.enDungeonSize)EditorGUILayout.EnumPopup("Dungeon Size", dG.DungeonSize);		
		
		dG.DungeonRoomSize 
			= (ceDungeonGenerator.enDungeonRoomSize)EditorGUILayout.EnumPopup("Max Room Size", dG.DungeonRoomSize);
		
		dG.MaxRoomCount
			= EditorGUILayout.IntField("Max Room Count", dG.MaxRoomCount);	
		
		dG.Corridors 
			= (ceDungeonGenerator.enCorridors)EditorGUILayout.EnumPopup("Corridors", dG.Corridors);
		
		dG.Twists 
			= (ceDungeonGenerator.enTwists)EditorGUILayout.EnumPopup("Twists", dG.Twists);

		dG.DeadEnds 
			= (ceDungeonGenerator.enDeadEnds)EditorGUILayout.EnumPopup("Dead Ends", dG.DeadEnds);
		
		dG.GenerateCeiling
			= EditorGUILayout.Toggle("Ceiling?", dG.GenerateCeiling);
		
		EditorGUILayout.Separator();
		
        //dG.playerObject 
        //    = (GameObject)EditorGUILayout.ObjectField 
        //        ("Player", dG.playerObject, typeof(GameObject), true);
		
		EditorGUILayout.Separator();
		
        // Tries to do something that doesn't work blah blah.
		//dG.monsterObjects
		//	= (List<GameObject>)EditorGUILayout.ObjectField
        //        ("Monster Prefabs", dG.monsterObjects, typeof(List<GameObject>), true);
		
		dG.monstersInCorridors
			= EditorGUILayout.Toggle("Monsters in Corridors?", dG.monstersInCorridors);
		
		dG.monstersInRooms
			= EditorGUILayout.Toggle("Monsters in Rooms?", dG.monstersInRooms);
		
		dG.monsterAppearChance
			= EditorGUILayout.IntField("Appear Chance (%)", dG.monsterAppearChance);	
		
		EditorGUILayout.Separator();
				
		dG.floorPrefab 
			= (GameObject)EditorGUILayout.ObjectField 
				("Floor Prefab", dG.floorPrefab, typeof(GameObject), true);
		
		dG.wallsPrefab 
			= (GameObject)EditorGUILayout.ObjectField 
				("Walls Prefab", dG.wallsPrefab, typeof(GameObject), true);
		
		dG.doorsPrefab 
			= (GameObject)EditorGUILayout.ObjectField 
				("Doors Prefab", dG.doorsPrefab, typeof(GameObject), true);
		
		EditorGUILayout.Separator();
		
		dG.cellSpacingFactor
			= EditorGUILayout.FloatField("Cell Spacing", dG.cellSpacingFactor);
		
		dG.floorHeightOffset
			= EditorGUILayout.FloatField("Floor Height", dG.floorHeightOffset);
		
		dG.wallsHeightOffset
			= EditorGUILayout.FloatField("Walls Height", dG.wallsHeightOffset);
		
		dG.doorsHeightOffset
			= EditorGUILayout.FloatField("Doors Height", dG.doorsHeightOffset);
		
		dG.roofsHeightOffset
			= EditorGUILayout.FloatField("Roofs Height", dG.roofsHeightOffset);		
		
		EditorGUILayout.Separator();
		
		if(GUILayout.Button("Generate", GUILayout.Width(200)))
		{
			dG.DestroyDungeon();
			dG.CreateDungeon ();			
		}
		
		if(GUILayout.Button("Destroy", GUILayout.Width(200)))
		{
			dG.DestroyDungeon();			
		}
		
		if (GUI.changed)
			EditorUtility.SetDirty (target);
	}	
}
