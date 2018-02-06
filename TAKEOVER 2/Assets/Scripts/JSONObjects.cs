using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONObjects {

}

/// <summary>
/// Game state JSON Representation.
/// </summary>
[System.Serializable]
public class GameStateJSON {
	public MapJSON map = new MapJSON ();
	public PlayerJSON[] players;
	public ColourJSON[] collegeColours;
	public CombatEngineJSON combatEngine;
	public int numberOfPlayers;
	public int currentTurn;
	public int currentPlayer;
}

/// <summary>
/// Player JSON Representation.
/// </summary>
[System.Serializable]
public class PlayerJSON {
	public int college;
	public string name;
	public int bonusNum;
	public int noOfGangMembers;
	public int positionInArray; 
			

}

/// <summary>
/// Map JSON Representation.
/// </summary>
[System.Serializable]
public class MapJSON {
	public TileJSON[] tiles;
	public int numberOfTiles;
}

/// <summary>
/// Tile JSON Representation.
/// </summary>
[System.Serializable]
public class TileJSON {
	public int x;
	public int y;
	public int tileID;
	public int gangStrength;
	public int college;
	public GameObject gob;
	public bool pvc;
}

/// <summary>
/// Colour JSON Representation
/// </summary>
[System.Serializable]
public class ColourJSON {
	public float r;
	public float g;
	public float b;
	public float a;
}

/// <summary>
/// Combat Engine Representation
/// </summary>
[System.Serializable]
public class CombatEngineJSON {
	public double pvcBonus;
	public double hiddenDamageModifier;
}
