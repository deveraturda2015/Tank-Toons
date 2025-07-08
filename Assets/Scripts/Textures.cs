using UnityEngine;

public class Textures
{
	internal static Texture2D summerTerrainTiles;

	public static void InitializeTextures()
	{
		summerTerrainTiles = Resources.Load<Texture2D>("Sprites/GroundAtlases/Grass");
	}
}
