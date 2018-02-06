﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

/*
    CLASS: Map
    FUNCTION: Holds information about the game map
 */

namespace CRGames_game
{
    public class Map
    {
        // Store the tile prefab to istantiate
        public GameObject tilePrefab;

        // Store the GameObjects associated with tiles on the map
        public GameObject[] tileObjects;

        // The Sprite to use to represent gang members
        public GameObject gangMemberSprite;

		public GameObject camera;
	
        // Array of tiles
        Tile[] tiles;

        // Maximum number of tiles the Map can contain
        int size;
        int width;
        int height;

		public Map(int width, int height) {
			this.width = width;
			this.height = height;
			this.size = width * height;
			tiles = new Tile[size];

		}
		public Tile[] GetTiles(){ 
			return tiles;
		}
		 
        /// <summary>
        /// Creates a Map of given size and populates it with Tiles and a random PVCTile.
        /// </summary>
        /// <param name="width">The width of the map.</param>
        /// <param name="height">The height of the map.</param>
        /// <param name="sprites">The sprites to draw the map with.</param>
        /// <param name="tilePrefab">The prefab to use for creating tiles as GameObjects.</param>
        /// <param name="gangMemberSprite">The sprite to use to represent gang members.</param>
        public Map(int width, int height, Sprite[] sprites, GameObject tilePrefab, GameObject gangMemberSprite)
        {   
            // Initialise the map
            this.tilePrefab = tilePrefab;
            this.size = width * height;
            this.gangMemberSprite = gangMemberSprite;
            this.width = width;
            this.height = height; 

            // Create the arrays to store the Tiles and correponding GameObjects
            tiles = new Tile[size];
            tileObjects = new GameObject[size];

            // Create an object to hold all of the map tiles
            GameObject mapPivot = new GameObject();
            mapPivot.name = "Map Pivot";
            mapPivot.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
			camera = GameObject.Find ("Camera Wrap");
            // Create the map
            for (int y = 0; y < height; y++){
                for (int x = 0; x < width; x++){
                    // Instantiate the new tile
                    GameObject gob = GameObject.Instantiate<GameObject>(tilePrefab) as GameObject;
                    gob.name = "tile_" + x + "x" + y;
                    gob.transform.parent = mapPivot.transform;

                    // Get the renderer and set the tile sprite
                    SpriteRenderer rend = gob.GetComponent<SpriteRenderer>();
                    rend.sprite = sprites[x + (y * width)];

                    // Set the tile position
                    gob.transform.localPosition = new Vector3((0.75f * x), -(0.75f * y), 0.0f);
                    
                    // Add the interaction script to the tile
                    TileInteraction interact = gob.AddComponent<TileInteraction>() as TileInteraction;
                    // Set the sprite to use for gang members
                    interact.SetGangMemberSprite(gangMemberSprite);
                 

                    // Create a new Tile object to hold the properties of this tile
                    Tile tile = new Tile(x + (y * width), gob);
                    tile.x = x;
                    tile.y = y;
                    tiles[x + (y * width)] = tile;
                    tileObjects[x + (y * width)] = gob;

                    // Set the tile property of the TileInteraction component
                    interact.tile = tile;
                }
            }

            // Set the maximum width and height positions of the camera
            GameObject.FindGameObjectWithTag("MainCamera").transform.parent.gameObject.GetComponent<MapCamera>().SetMaxCoord(0.75f * width, 0.75f * height);

            // Create a PVC tile
            //generatePVC(); 
			System.Random rnd = new System.Random();
			int r = rnd.Next (0, tiles.Length);
			tiles [r].setPVC (true);
		
        }

		/// <summary>
		/// Adds a tile to the Map tile array in the location of the tile id.
		/// </summary>
		/// <param name="tile">Tile.</param>
		public void addTile(Tile tile) {
			tiles [tile.getID ()] = tile;
		}
        
        /// <summary>
        /// Returns a Tile with a given ID.
        /// </summary>
        /// <param name="id">The ID of the tile to retrieve.</param>
        /// <returns>The Tile with the given ID.</returns>
		public Tile getTileByID(int id){
			return tiles [id];
		}

        /// <summary>
        /// Returns a Tile at a given position.
        /// </summary>
        /// <param name="x">The x position of the Tile.</param>
        /// <param name="y">The y position of the Tile.</param>
        /// <returns>The Tile at the given position.</returns>
        public Tile getTileAtPosition(int x, int y){
			Tile pos = new Tile(0,new GameObject());
			for (int i = 0; i < size; i++)
			{
				if ((tiles [i].x == x) & (tiles [i].y == y) ) { 
					pos = tiles [i];
				}
			} 
			return pos;

        }

        /// <summary>
        /// Returns the ID of a given Tile.
        /// </summary>
        /// <param name="tile">The Tile to get the ID of.</param>
        /// <returns>The ID of the given Tile.</returns>
        public int getTileId(Tile tile) {
            return tile.getID();
        }

        /// <summary>
        /// Returns the gang strength of a given Tile.
        /// </summary>
        /// <param name="tile">The tile to get the gang strength of.</param>
        /// <returns>The gang strength of a given Tile.</returns>
        public int getGangStrength(Tile tile)
        {
            return tile.getGangStrength();
        }

        /// <summary>
        /// Moves all gang members from a location Tile to a destination Tile. Returns false if no gang members at location.
        /// </summary>
        /// <param name="location">The tile to move gang members from.</param>
        /// <param name="destination">The tile to move gang members to.</params>
        /// <returns>True if movement is successful, False otherwise.</returns>
        public bool moveGangMember(Tile location, Tile destination)
        {
            // If the original tile hasn't got gang members to move return false
            if (location.getGangStrength() == 0)
            {
                return false;
            }
            else
            {
                // Move gang members and return true
                destination.setGangStrength(destination.getGangStrength() + location.getGangStrength());
                location.setGangStrength(0);
                return true;
            }
        }

        /// <summary>
        /// Generates a PVC tile at a random location in the map.
        /// </summary>
        public void generatePVC() {
            // Create a new random number generator and generate a random number
            System.Random rng = new System.Random();
            int rand = rng.Next(size);

            // Create a new PVC tile and set its properties
			PVCTile tile = new PVCTile(tiles[rand].getID(), tiles[rand].GetObject());
            tile.x = tiles[rand].x;
            tile.y = tiles[rand].y;
            tile.setColor(Color.yellow);
                
            // Add the new PVCTile to the tiles array
            tiles[rand] = tile;
        }

		/// <summary>
		/// Resets the colours of all the tiles.
		/// </summary>
		public void resetColours(Color[] collegeColours) {
			for (int i = 0; i < size; i++)
			{
				tiles[i].resetColor(collegeColours);
			}
		}


		public void getPlayers(Player player){
			player.ClearOwnedTiles ();
			for (int i = 0; i < size; i++)
			{
				if (tiles [i].getCollege () == player.GetCollege()) { 
					player.AddOwnedTiles (tiles [i]);
				}
			} 
		}
		public void centerCam(Player player){
			int[] coords;
			coords = new int[2];
			foreach(Tile tile in player.GetOwnedTiles())
			{
				if (tile.getGangStrength() > 0) { 
					coords [0] = tile.x;
					coords [1] = tile.y; 
					camera.BroadcastMessage("centerPos",coords);
				}
			} 
		}
		public void mapfog(Player player,Color[] collegeColours){ 
			for (int i = 0; i < size; i++) {
				bool fogON = true;
				if (fogON) { 
					tiles [i].setColor (Color.black);
					tiles [i].GetObject ().GetComponent<TileInteraction> ().textenabled = false;
				}
			}
			foreach(Tile tile in player.GetOwnedTiles()){
				foreach (Tile adj in getAdjacentList(tile)){ 
					adj.resetColor(collegeColours); 
					adj.GetObject ().GetComponent<TileInteraction> ().textenabled = true;
				} 
			}
		} 
		public void tilefog(Tile tile,Color[] collegeColours){
			foreach (Tile adj in getAdjacentList(tile)){ 
				adj.resetColor(collegeColours); 
				adj.GetObject ().GetComponent<TileInteraction> ().textenabled = true;
			} 
		}
        /// <summary>
        /// Resets all tiles.
        /// </summary>
        /// <returns>True if reset is successful, False otherwise.</returns>
        public bool reset()
        {
            for (int i = 0; i < size; i++)
            {
                tiles[i].reset();
            }
            return true;
        }

        /// <summary>
        /// Gets the number of Tiles in the Map.
        /// </summary>
        /// <returns>Number of Tiles in the Map.</returns>
		public int getNumberOfTiles(){
			return tiles.Length;
		}

        /// <summary>
        /// Set a Tile at a given ID to a given Tile.
        /// </summary>
        /// <param name="id">The ID of the tile to update.</param>
        /// <param name="tile">The updated tile.</param>
        public void setTile(int id, Tile tile){
            // TODO search for the array location based on ID,
            // seeing as IDs may change in future
            tiles[id] = tile;
        }

		/// <summary>
		/// Returns an array of tiles adjacent to the location tile. Null elements are outside of the map.
		/// </summary>
		/// <returns>The adjacent tiles, or null.</returns>
		/// <param name="location">The tile to use as the initial location.</param>
        /// <returns>An array of Tiles adjacent to the initial Tile.</returns>
		public Tile[] getAdjacent(Tile location)
		{
            // Create an array of Tiles to return
			Tile[] adjacents = new Tile[4];

			// Ensures edges do not have adjacent tiles out of bounds
			if (location.x != 0) {
				adjacents [0] = tiles [location.x - 1 + (location.y * width)];
			} else {
				adjacents [0] = null;
			}

			if (location.y != 0) {
				adjacents [1] = tiles [location.x + ((location.y - 1) * width)];
			} else {
				adjacents [1] = null;
			}

			if (location.x != width - 1) {
				adjacents [2] = tiles [location.x + 1 + (location.y * width)];
			} else {
				adjacents [2] = null;
			}

			if (location.y != height - 1) {
				adjacents [3] = tiles [location.x + ((location.y + 1) * width)];
			} else {
				adjacents [3] = null;
			}

       
			return adjacents;
		} 

		public List<Tile> getAdjacentList(Tile location)
		{
			// Create an array of Tiles to return
			List<Tile> adjacents = new List<Tile>();

			// Ensures edges do not have adjacent tiles out of bounds
			if (location.x != 0) {
				adjacents.Add(tiles [location.x - 1 + (location.y * width)]);
			}

			if (location.y != 0) {
				adjacents.Add(tiles [location.x + ((location.y - 1) * width)]);
			}

			if (location.x != width - 1) {
				adjacents.Add(tiles[location.x + 1 + (location.y * width)]);
			}

			if (location.y != height - 1) {
				adjacents.Add(tiles [location.x + ((location.y + 1) * width)]);
			}


			return adjacents;
		}

		/// <summary>
		/// Returns whether a destination tile is adjacent to a location tile.
		/// </summary>
		/// <returns><c>true</c>, if adjacent, <c>false</c> otherwise.</returns>
		/// <param name="location">Location.</param>
		/// <param name="destination">Destination.</param>
		public bool isAdjacent(Tile location, Tile destination)
		{
			if (((location.x == destination.x) && (location.y == destination.y + 1 || location.y == destination.y - 1)) ||
			    ((location.y == destination.y) && (location.x == destination.x + 1 || location.x == destination.x - 1))) {
				return true;
			} else {
				return false;
			}
		}

        /// <summary>
        /// Gets the width of the map.
        /// </summary>
        /// <returns>The map width.</returns>
        public int getWidth(){
            return this.width;
        }

        /// <summary>
        /// Gives each of the players ownership of a random area of the map.
        /// </summary>
        /// <param name="playersList">The list of players to place on the map.</param>
		public void populateRandomGangMembers(List<Player> playersList) {
            // Iterate through all players
			foreach(Player player in playersList) {
                // Generate a random x and y position
                int x = UnityEngine.Random.Range(1, 22);
                int y = UnityEngine.Random.Range(1, 11);

                // Set the ownership of all tiles at the generated position in a cross pattern
                this.getTileAtPosition(x,y).setCollege(player.GetCollege());
                this.getTileAtPosition(x +1, y).setCollege(player.GetCollege());
                this.getTileAtPosition(x, y+1).setCollege(player.GetCollege());
                this.getTileAtPosition(x-1, y).setCollege(player.GetCollege());
                this.getTileAtPosition(x, y -1).setCollege(player.GetCollege());

                // Add all of the assigned tiles to the player's list of owned tiles
                player.AddOwnedTiles(this.getTileAtPosition(x, y));
                player.AddOwnedTiles(this.getTileAtPosition(x +1, y));
                player.AddOwnedTiles(this.getTileAtPosition(x, y +1));
                player.AddOwnedTiles(this.getTileAtPosition(x-1, y));
                player.AddOwnedTiles(this.getTileAtPosition(x, y-1));

                // Set the gang strength of the central tile to 2
                this.getTileAtPosition(x, y).setGangStrength(2);
            }
		}
    }
}
