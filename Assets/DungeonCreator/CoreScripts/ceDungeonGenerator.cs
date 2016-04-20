using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Apex.WorldGeometry;

//[ExecuteInEditMode()]
public class ceDungeonGenerator
	: MonoBehaviour 
{	
	#region Enums
	
		public enum enDungeonSize
		{
			Small   = 8,
			Medium  = 12,
			Large   = 16,
			Huge    = 24,
		};
	
		public enum enDungeonRoomSize
		{
			Small,
			Medium,
			Large,
		};
		
		public enum enCorridors
		{
			Maze 		 = 30,
			Some		 = 60,
			ConnectRooms = 90,		
		}
		
		public enum enDeadEnds
		{
			Allow  = 0,
			Remove = 100,
		}
	
		public enum enTwists
		{
			Straight = 0,
			Minor    = 50,
			Major    = 100,			
		};
	
	#endregion
	
	#region Members
	
		private enDungeonSize     m_dungeonSize     = enDungeonSize.Huge;
		private enDungeonRoomSize m_dungeonRoomSize = enDungeonRoomSize.Large;
		private enCorridors		  m_corridors       = enCorridors.ConnectRooms;
		private enDeadEnds        m_deadEnds        = enDeadEnds.Allow;
		private enTwists 		  m_twists          = enTwists.Minor;		
	
		public GameObject floorPrefab;
		public GameObject wallsPrefab;
		public GameObject doorsPrefab;
        public GameObject playerPrefab;
        public GameObject loadingScreen;
        public GameObject dungeonExit;
        GameObject playerObject;

		public List<GameObject> monsterObjects;

        // These only spawn on the 3rd floor and up.
        public List<GameObject> hardMonsterObjects;

        public List<GameObject> lootObjects;

        public GameObject gameWorld;

		public bool monstersInCorridors = true;
		public bool monstersInRooms 	= true;
        public int monsterAppearChance = 40;
	
		public float cellSpacingFactor = 1.0f;
		
		public float floorHeightOffset;
		public float wallsHeightOffset;
		public float doorsHeightOffset;
		public float roofsHeightOffset;
		
		private int m_maxRoomCount     = 20;		
		public bool GenerateCeiling = false;

        // Initialize apex path grid?
        public bool InitializeGrid = true;

        // Are we generating the first level or regenerating for the next?
        private bool firstGeneration = true;

        // The floor we are currently on.
        int floor = -1;

        public Text floorText;

        Texture2D minimap;

        public GameObject bossLevel;
        
        public GameObject bossPrefab;

        public bool generateBossLevel = false;

        public GameObject audioPlayer;

	#endregion
	
	#region Properties
	
		public enDungeonSize DungeonSize
		{
			get { return m_dungeonSize;  }
			set { m_dungeonSize = value; }
		}
		
		public enDungeonRoomSize DungeonRoomSize
		{
			get { return m_dungeonRoomSize;  }
			set { m_dungeonRoomSize = value; }
		}
	
		public int MaxRoomCount
		{
			get { return m_maxRoomCount; }
			set
			{
				m_maxRoomCount = Mathf.Clamp(value, 1, 25);
			}
		}
		
		public enCorridors Corridors
		{
			get { return m_corridors;  }
			set { m_corridors = value; }
		}
		
		public enDeadEnds DeadEnds
		{
			get { return m_deadEnds;  }
			set { m_deadEnds = value; }
		}
	
		public enTwists Twists
		{
			get { return m_twists;  }
			set { m_twists = value; }
		}
	
	#endregion
	
	#region Functions

    void Start()
    {
        CreateDungeon();
    }

    //void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.G) && floor < 4){
    //        CreateDungeon();
    //    }
    //}

	public void DestroyDungeon()
	{
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }	
	}
	
	public void CreateDungeon()
	{
        if (floor == 4 && !generateBossLevel) return;

        loadingScreen.SetActive(true);

        DestroyDungeon();

        if (generateBossLevel)
        {
            audioPlayer.GetComponent<AudioPlayer>().StopPlayingMusic();

            bossLevel.SetActive(true);
            if (!playerObject)
            {
                playerObject = Instantiate(playerPrefab, bossLevel.transform.FindChild("PlayerSpawn").position, Quaternion.identity) as GameObject;
            }
            playerObject.transform.position = bossLevel.transform.FindChild("PlayerSpawn").position;
            playerObject.GetComponent<PlayerAI>().dest = playerObject.transform.position;

            GameObject boss = Instantiate(bossPrefab, bossLevel.transform.FindChild("BossSpawn").position, Quaternion.identity) as GameObject;

            if (InitializeGrid)
            {
                // Disable in case we have already made a grid.
                gameWorld.GetComponent<GridComponent>().Disable(100);

                // Initialize grid.
                gameWorld.GetComponent<GridComponent>().Initialize(100, (ignored) => OnGridWasInitialized());
            }

            generateBossLevel = false;
            return;
        }

        if (floor >= 1)
        {
            foreach (GameObject monster in hardMonsterObjects)
            {
                if(!monsterObjects.Contains(monster)) monsterObjects.Add(monster);
            }
        }

		csDungeonRoomGenerator rG
			= new csDungeonRoomGenerator();
		
		int roomSizeMin = 2;
		int roomSizeMax = 2;
		
		switch (m_dungeonRoomSize)
		{
			case enDungeonRoomSize.Small:
				roomSizeMin = 2;
				roomSizeMax = 2;
				break;
			case enDungeonRoomSize.Medium:
				roomSizeMin = 2;
				roomSizeMax = 3;
				break;
			case enDungeonRoomSize.Large:
				roomSizeMin = 2;
				roomSizeMax = 4;
				break;
		}
		
		rG.Constructor
			(m_maxRoomCount,
			 roomSizeMin,
		     roomSizeMax,
			 roomSizeMin,
			 roomSizeMax);
		
		csDungeonGenerator dG 
			= new csDungeonGenerator();
		
		dG.Constructor
			((int)m_dungeonSize,
			 (int)m_dungeonSize,
			 (int)m_twists,
		     (int)m_corridors,
			 (int)m_deadEnds,
			 rG);		 
		
		csDungeon dungeon 
			= dG.Generate();
		
		int[,] tiles 
			= csDungeonGenerator.ExpandToTiles
				(dungeon);
		
		if (this.floorPrefab && this.wallsPrefab && this.doorsPrefab)
		{			
			for (int k=0;k<tiles.GetUpperBound(0);k++)
			{
				for (int l=0;l<tiles.GetUpperBound(1);l++)
				{	
					float x = k * cellSpacingFactor;
					float y = l * cellSpacingFactor;
					
					if (tiles[k,l] != (int)csDungeonCell.TileType.Rock && 
						tiles[k,l] != (int)csDungeonCell.TileType.Void &&
                        tiles[k,l] != (int)csDungeonCell.TileType.OuterRock)
					{
						GameObject tile 
							= (GameObject)Instantiate
								(this.floorPrefab, new Vector3(x, this.floorHeightOffset, y), Quaternion.identity);
						
						tile.transform.parent 
							= this.transform;
						
						if (this.GenerateCeiling)
						{
						
							tile = (GameObject)Instantiate
									   (this.floorPrefab, new Vector3(x, this.roofsHeightOffset, y), Quaternion.identity);
							
							tile.transform.parent 
								= this.transform;
						}
					}	
					
					if (tiles[k,l] == (int)csDungeonCell.TileType.Rock)
					{
						GameObject tile 
							= (GameObject)Instantiate
								(this.wallsPrefab, new Vector3(x, this.wallsHeightOffset ,y), Quaternion.identity);
						
						tile.transform.parent 
							= this.transform;
					}

                    if (tiles[k, l] == (int)csDungeonCell.TileType.OuterRock)
                    {
                        GameObject tile
                            = (GameObject)Instantiate
                                (this.wallsPrefab, new Vector3(x, this.wallsHeightOffset, y), Quaternion.identity);

                        tile.transform.parent
                            = this.transform;

                        Destroy(tile.GetComponent<BoxCollider>());
                    }	 
					
					if (tiles[k,l] == (int)csDungeonCell.TileType.DoorNS)
					{
						GameObject tile 
							= (GameObject)Instantiate
								(this.doorsPrefab, new Vector3(x, this.doorsHeightOffset ,y), Quaternion.identity);
						
						tile.transform.parent 
							= this.transform;
					}	
					
					if (tiles[k,l] == (int)csDungeonCell.TileType.DoorEW)
					{
						GameObject tile
							= (GameObject)Instantiate
								(this.doorsPrefab, new Vector3(x, this.doorsHeightOffset ,y), Quaternion.AngleAxis(90.0f, Vector3.up));
						
						tile.transform.parent 
							= this.transform;
					}	
				}
			}

            // Spawn rare chest loot.
            SpawnLoot(tiles);

            if (InitializeGrid)
            {
                // Disable in case we have already made a grid.
                gameWorld.GetComponent<GridComponent>().Disable(100);

                // Initialize grid.
                gameWorld.GetComponent<GridComponent>().Initialize(100, (ignored) => OnGridWasInitialized(tiles));
            }
		}
	}

    #region Generation Functions

    #region Player and Exit Positioning Functions

    /* Positions the player and the exit. */
    private void PositionPlayer(int[,] tiles)
    {
        // Used to create exit.
        Vector2 farthestTile = Vector2.zero;

        // Player tile.
        Vector2 playerTile = Vector2.zero;

        // If we're just starting the game, create the player.
        SpawnPlayer();

        // Deactivate the player so we can't do anything when loading.
        playerObject.SetActive(false);

        // Find where to position the player.
        PickPlayerTile(tiles, ref farthestTile, ref playerTile);

        // Generates the exit.
        GenerateExit(tiles, ref farthestTile, ref playerTile);

        // Reactivate the player so we can play!
        playerObject.SetActive(true);
        playerObject.GetComponent<PlayerAI>().dest = playerObject.transform.position;
    }

    /* Instantiate player and pass it to required scripts. */
    private void SpawnPlayer()
    {
        if (firstGeneration)
        {
            playerObject = Instantiate(playerPrefab);
            GameController.player = playerObject;
            Camera.main.transform.GetComponent<SmoothFollow>().target = playerObject.transform;
        }
    }

    /* Choose a tile in a dead end and put the player there. */
    private void PickPlayerTile(int[,] tiles, ref Vector2 farthestTile, ref Vector2 playerTile)
    {
        if (playerObject)
        {
            for (int k = 1; k < tiles.GetUpperBound(0) - 1; k++)
            {
                for (int l = 1; l < tiles.GetUpperBound(1) - 1; l++)
                {
                    if (tiles[k, l] == (int)csDungeonCell.TileType.Corridor)
                    {
                        int deadEndWeight = 0;

                        if (tiles[k - 1, l - 1] == (int)csDungeonCell.TileType.Rock) deadEndWeight++;
                        if (tiles[k, l - 1] == (int)csDungeonCell.TileType.Rock) deadEndWeight++;
                        if (tiles[k + 1, l - 1] == (int)csDungeonCell.TileType.Rock) deadEndWeight++;
                        if (tiles[k - 1, l] == (int)csDungeonCell.TileType.Rock) deadEndWeight++;
                        if (tiles[k + 1, l] == (int)csDungeonCell.TileType.Rock) deadEndWeight++;
                        if (tiles[k - 1, l + 1] == (int)csDungeonCell.TileType.Rock) deadEndWeight++;
                        if (tiles[k, l + 1] == (int)csDungeonCell.TileType.Rock) deadEndWeight++;
                        if (tiles[k + 1, l + 1] == (int)csDungeonCell.TileType.Rock) deadEndWeight++;

                        if (deadEndWeight == 7)
                        {
                            //tiles[k, l] = (int)csDungeonCell.TileType.PlayerTile;
                            playerObject.transform.position = new Vector3(k * cellSpacingFactor, 0.06498781f, l * cellSpacingFactor);
                            playerTile = new Vector2(k, l);
                            farthestTile = playerTile;
                        }
                    }
                }
            }
        }
    }
    
    /* Generates dungeon exit at farthest room tile. */
    private void GenerateExit(int[,] tiles, ref Vector2 farthestTile, ref Vector2 playerTile)
    {
        // Find the exit.
        for (int k = 1; k < tiles.GetUpperBound(0) - 1; k++)
        {
            for (int l = 1; l < tiles.GetUpperBound(1) - 1; l++)
            {
                if (tiles[k, l] != (int)csDungeonCell.TileType.Rock &&
                    tiles[k, l] != (int)csDungeonCell.TileType.Void &&
                    tiles[k, l] != (int)csDungeonCell.TileType.OuterRock &&
                    tiles[k, l] != (int)csDungeonCell.TileType.DoorEW &&
                    tiles[k, l] != (int)csDungeonCell.TileType.DoorNS &&
                    tiles[k, l] != (int)csDungeonCell.TileType.Corridor)
                {
                    if (Vector2.Distance(new Vector2(k, l), playerTile) > (Vector2.Distance(farthestTile, playerTile)))
                    {
                        farthestTile = new Vector2(k, l);
                    }
                }
            }
        }

        Vector3 exitPos = new Vector3(farthestTile.x * cellSpacingFactor, 0.06498781f, farthestTile.y * cellSpacingFactor);
        GameObject exit = Instantiate(dungeonExit, exitPos, dungeonExit.transform.rotation) as GameObject;
        exit.transform.SetParent(transform);
    }

    #endregion

    #region Feature Generation Functions

    /* Spawns loot on room tiles next to walls. Change this to spawn against the wall. */
    private void SpawnLoot(int[,] tiles)
    {
        Vector3 OffsetToWall = Vector3.zero;

        Quaternion lootRotation = Quaternion.identity;

        for (int k = 1; k < tiles.GetUpperBound(0) - 1; k++)
        {
            for (int l = 1; l < tiles.GetUpperBound(1) - 1; l++)
            {
                if (tiles[k, l] == (int)csDungeonCell.TileType.Room)
                {
                    int deadEndWeight = 0;

                    if (tiles[k, l - 1] == (int)csDungeonCell.TileType.Rock)
                    {
                        OffsetToWall.z = -.5f;
                        lootRotation = Quaternion.Euler(0, 90, 0);
                        deadEndWeight++; 
                    }

                    if (tiles[k - 1, l] == (int)csDungeonCell.TileType.Rock)
                    {
                        OffsetToWall.x = -.55f;
                        lootRotation = Quaternion.Euler(0, 180, 0);
                        deadEndWeight++;
                    }

                    if (tiles[k + 1, l] == (int)csDungeonCell.TileType.Rock)
                    {
                        OffsetToWall.x = .55f;
                        lootRotation = Quaternion.Euler(0, 180, 0);
                        deadEndWeight++;
                    }

                    if (tiles[k, l + 1] == (int)csDungeonCell.TileType.Rock)
                    {
                        OffsetToWall.z = .5f;
                        lootRotation = Quaternion.Euler(0, 90, 0);
                        deadEndWeight++;
                    }

                    if (deadEndWeight > 1)
                    {
                        float chance = Random.Range(0f, 1f);

                        if (chance < .8f)
                        {
                            Vector3 lootPosition = new Vector3(k * cellSpacingFactor, 0.06498781f, l * cellSpacingFactor) + OffsetToWall;

                            int lootToSpawn = Random.Range(0, lootObjects.Count);

                            GameObject lootObject = Instantiate(lootObjects[lootToSpawn], lootPosition, lootRotation) as GameObject;
                            lootObject.transform.parent = transform;
                        }
                    }
                }
            }
        }
    }

    /* Generates monsters in a level based on percent chance. */
    private void GenerateMonsters(int[,] tiles)
    {
        for (int k = 1; k < tiles.GetUpperBound(0) - 1; k++)
        {
            for (int l = 1; l < tiles.GetUpperBound(1) - 1; l++)
            {
                if (tiles[k, l] == (int)csDungeonCell.TileType.Room && monstersInRooms == true)
                {
                    if (Random.Range(0, 100) < monsterAppearChance)
                    {
                       
                        Vector3 position = new Vector3(k * cellSpacingFactor, 0.05f, l * cellSpacingFactor);
                        if (Vector3.Distance(playerObject.transform.position, position) > 1)
                        {
                            int monsterToSpawn = Random.Range(0, monsterObjects.Count);
                            GameObject monster
                                = (GameObject)
                                    Instantiate(monsterObjects[monsterToSpawn], position, Quaternion.identity);

                            monster.GetComponent<EnemyAI>().damage += floor;
                            monster.GetComponent<Stats>().maxHealth += floor;
                            monster.GetComponent<Stats>().currentHealth += floor;

                            monster.transform.parent = transform;
                        }
                    }
                }

                if (tiles[k, l] == (int)csDungeonCell.TileType.Corridor && monstersInCorridors == true)
                {
                    if (Random.Range(0, 100) < monsterAppearChance)
                    {
                        Vector3 position = new Vector3(k * cellSpacingFactor, 0.05f, l * cellSpacingFactor);
                        if (Vector3.Distance(playerObject.transform.position, position) > 1)
                        {
                            int monsterToSpawn = Random.Range(0, monsterObjects.Count);
                            GameObject monster
                                = (GameObject)
                                    Instantiate(monsterObjects[monsterToSpawn], position, Quaternion.identity);

                            monster.transform.parent = transform;
                        }
                    }
                }
            }
        }
        // Tell the script that we are no longer on the first level.
        firstGeneration = false;
    }

    #endregion

    #endregion

    void OnGridWasInitialized(int[,] tiles)
    {
        floor++;

        Debug.Log("Initialized grid.");        

        if (!generateBossLevel)
        {
            // Find a dead end to place the player in
            PositionPlayer(tiles);

            // After generating dungeon and pathfinding grid, generate the monsters.
            GenerateMonsters(tiles);
        }        

        // If on 4th floor (index + 1), then next floor is boss.
        if (floor == 3)
        {
            generateBossLevel = true;
        }

        Invoke("HideLoadingScreen", 1);
    }

    void OnGridWasInitialized()
    {
        floor++;

        AudioPlayer.FadeInMusic(audioPlayer.GetComponent<AudioSource>());

        audioPlayer.GetComponent<AudioPlayer>().LoopBossMusic();

        Debug.Log("Initialized grid.");
       
        Invoke("HideLoadingScreen", 1);
    }

    #region Loading and Text Visuals

    private void HideLoadingScreen()
    {
        loadingScreen.SetActive(false);

        if (floor < 4)
        {
            StartCoroutine(UITools.TypeText(floorText, "Floor " + (floor + 1), .05f));
        }
        else if (floor == 4)
        {
            StartCoroutine(UITools.TypeText(floorText, "boss 1", .05f));
        }

        Invoke("HideFloorText", 3);
    }

    private void HideFloorText()
    {
        StartCoroutine(UITools.UnTypeText(floorText, .05f));
    }

    #endregion


    #endregion
}
