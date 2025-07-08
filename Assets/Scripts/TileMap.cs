using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TileMap : MonoBehaviour
{
	public enum TileType
	{
		clear,
		grass,
		fullBlock,
		upperRightBlock,
		bottomRightBlock,
		upperLeftBlock,
		bottomLeftBlock,
		destructibleUpperRightBlock,
		destructibleBottomRightBlock,
		destructibleUpperLeftBlock,
		destructibleBottomLeftBlock,
		specialWallBlock,
		harderWallBlock,
		simpleWallBlock,
		crackedBlock
	}

	public enum TilesType
	{
		SummerTiles,
		WinterTiles,
		DesertTiles
	}

	private int size_x;

	private int size_y;

	private float tileSize = 1.09f;

	private MeshRenderer meshRenderer;

	private MeshFilter meshFilter;

	private List<Vector2> UVArray;

	private int iIndexCount;

	private int iVertCount;

	private List<Vector3> vertices;

	private List<int> triangles;

	private List<string> mml = new List<string>
	{
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000",
		"000000000000000000000000000000000"
	};

	private Dictionary<Vector2, int> destructableItemsStartingUVIndexes;

	public void InitTileMap(TilesType tilesType)
	{
		destructableItemsStartingUVIndexes = new Dictionary<Vector2, int>();
		meshRenderer = GetComponent<MeshRenderer>();
		switch (tilesType)
		{
		case TilesType.SummerTiles:
			meshRenderer.material = Materials.GroundMaterialSummer;
			break;
		case TilesType.DesertTiles:
			meshRenderer.material = Materials.GroundMaterialDesert;
			break;
		case TilesType.WinterTiles:
			meshRenderer.material = Materials.GroundMaterialWinter;
			break;
		}
		if (GameplayCommons.Instance != null)
		{
			size_y = GameplayCommons.Instance.currentLevel.Count;
			size_x = GameplayCommons.Instance.currentLevel[0].Length;
			Transform transform = base.transform;
			Vector3 position = base.transform.position;
			float x = position.x - 1f * tileSize + tileSize / 2f;
			Vector3 position2 = base.transform.position;
			float y = position2.y + (float)size_y * tileSize - 1f * tileSize + tileSize / 2f;
			Vector3 position3 = base.transform.position;
			transform.position = new Vector3(x, y, position3.z + 100f);
		}
		else
		{
			size_y = mml.Count;
			size_x = mml[0].Length;
			Transform transform2 = base.transform;
			Vector3 position4 = base.transform.position;
			float x2 = position4.x - 1f * tileSize + tileSize / 2f;
			Vector3 position5 = base.transform.position;
			float y2 = position5.y + (float)size_y * tileSize - 2f * tileSize + tileSize / 2f;
			Vector3 position6 = base.transform.position;
			transform2.position = new Vector3(x2, y2, position6.z + 100f);
		}
		BuildMesh();
	}

	public void ChangeLevelItemAndUV(Vector2 coords, TileType tileType)
	{
		int value = -1;
		StringBuilder stringBuilder = new StringBuilder(GameplayCommons.Instance.currentLevel[GameplayCommons.Instance.currentLevel.Count - 1 - Mathf.RoundToInt(coords.y)]);
		stringBuilder[Mathf.RoundToInt(coords.x)] = '0';
		GameplayCommons.Instance.currentLevel[GameplayCommons.Instance.currentLevel.Count - 1 - Mathf.RoundToInt(coords.y)] = stringBuilder.ToString();
		if (destructableItemsStartingUVIndexes.TryGetValue(coords, out value))
		{
			Vector2[] uVForTileType = GetUVForTileType(GetTileTypeFromItemChar(GameplayCommons.Instance.currentLevel[GameplayCommons.Instance.currentLevel.Count - 1 - Mathf.RoundToInt(coords.y)][Mathf.RoundToInt(coords.x)]));
			UVArray[value] = uVForTileType[0];
			UVArray[value + 1] = uVForTileType[1];
			UVArray[value + 2] = uVForTileType[2];
			UVArray[value + 3] = uVForTileType[3];
		}
		meshFilter.mesh.SetUVs(0, UVArray);
	}

	public void BuildMesh()
	{
		int num = size_x * size_y;
		vertices = new List<Vector3>();
		triangles = new List<int>();
		UVArray = new List<Vector2>();
		if (GameplayCommons.Instance == null)
		{
			for (int i = 0; i < size_x; i++)
			{
				for (int num2 = size_y - 1; num2 >= 0; num2--)
				{
					AddMeshFragment(i, num2);
				}
			}
		}
		Mesh mesh = new Mesh();
		mesh.SetVertices(vertices);
		mesh.SetTriangles(triangles, 0);
		mesh.MarkDynamic();
		meshFilter = GetComponent<MeshFilter>();
		meshFilter.mesh = mesh;
		meshFilter.mesh.SetUVs(0, UVArray);
	}

	public void AddMeshFragment(int x, int y, bool dummyItem = false)
	{
		char c = dummyItem ? '0' : ((!(GameplayCommons.Instance != null)) ? mml[size_y - 1 - y][x] : GameplayCommons.Instance.currentLevel[size_y - 1 - y][x]);
		if (c != 'f')
		{
			vertices.Add(new Vector3((float)x * tileSize, (float)y * tileSize, 0f));
			vertices.Add(new Vector3((float)(x + 1) * tileSize, (float)y * tileSize, 0f));
			vertices.Add(new Vector3((float)(x + 1) * tileSize, (float)(y + 1) * tileSize, 0f));
			vertices.Add(new Vector3((float)x * tileSize, (float)(y + 1) * tileSize, 0f));
			triangles.Add(iVertCount);
			triangles.Add(iVertCount + 1);
			triangles.Add(iVertCount + 2);
			triangles.Add(iVertCount);
			triangles.Add(iVertCount + 2);
			triangles.Add(iVertCount + 3);
			TileType tileType = TileType.clear;
			tileType = GetTileTypeFromItemChar(c);
			Vector2[] uVForTileType = GetUVForTileType(tileType);
			UVArray.Add(uVForTileType[0]);
			UVArray.Add(uVForTileType[1]);
			UVArray.Add(uVForTileType[2]);
			UVArray.Add(uVForTileType[3]);
			iVertCount += 4;
			iIndexCount += 6;
			destructableItemsStartingUVIndexes.Add(new Vector2(x, y), iVertCount - 4);
			if (meshFilter != null)
			{
				meshFilter.mesh.SetVertices(vertices);
				meshFilter.mesh.SetTriangles(triangles, 0);
				meshFilter.mesh.SetUVs(0, UVArray);
			}
		}
	}

	private TileType GetTileTypeFromItemChar(char itemChar)
	{
		switch (itemChar)
		{
		case '9':
			return TileType.fullBlock;
		case 'B':
			return TileType.upperRightBlock;
		case 'C':
			return TileType.upperLeftBlock;
		case 'D':
			return TileType.bottomRightBlock;
		case 'E':
			return TileType.bottomLeftBlock;
		case '1':
			return TileType.simpleWallBlock;
		case '8':
			return TileType.harderWallBlock;
		case 'e':
			return TileType.specialWallBlock;
		case 'a':
			return TileType.destructibleUpperRightBlock;
		case 'b':
			return TileType.destructibleUpperLeftBlock;
		case 'c':
			return TileType.destructibleBottomRightBlock;
		case 'd':
			return TileType.destructibleBottomLeftBlock;
		case 'f':
			return TileType.clear;
		case 'g':
			return TileType.crackedBlock;
		default:
			return TileType.grass;
		}
	}

	private Vector2[] ImageCoordsToUV(Vector2 imageCoords, bool randomRotation = false)
	{
		float num = 512f;
		float num2 = 80f;
		Vector2[] array = new Vector2[4]
		{
			new Vector2(imageCoords.x / num, (num - imageCoords.y) / num),
			new Vector2((imageCoords.x + num2) / num, (num - imageCoords.y) / num),
			new Vector2((imageCoords.x + num2) / num, (num - (imageCoords.y + num2)) / num),
			new Vector2(imageCoords.x / num, (num - (imageCoords.y + num2)) / num)
		};
		if (randomRotation)
		{
			int num3 = UnityEngine.Random.Range(0, 4);
			for (int i = 0; i < num3; i++)
			{
				array = shiftRight(array);
			}
		}
		return array;
	}

	private Vector2[] shiftRight(Vector2[] arr)
	{
		Vector2[] array = new Vector2[arr.Length];
		for (int i = 1; i < arr.Length; i++)
		{
			array[i] = arr[i - 1];
		}
		array[0] = arr[array.Length - 1];
		return array;
	}

	private Vector2[] GetUVForTileType(TileType tileType)
	{
		Vector2[] array = new Vector2[2]
		{
			new Vector2(2f, 338f),
			new Vector2(422f, 254f)
		};
		Vector2[] array2 = new Vector2[3]
		{
			new Vector2(254f, 86f),
			new Vector2(170f, 86f),
			new Vector2(86f, 86f)
		};
		Vector2[] array3 = new Vector2[19]
		{
			new Vector2(254f, 254f),
			new Vector2(170f, 254f),
			new Vector2(86f, 254f),
			new Vector2(2f, 254f),
			new Vector2(422f, 170f),
			new Vector2(338f, 170f),
			new Vector2(254f, 170f),
			new Vector2(170f, 170f),
			new Vector2(86f, 170f),
			new Vector2(2f, 170f),
			new Vector2(422f, 86f),
			new Vector2(338f, 86f),
			new Vector2(2f, 86f),
			new Vector2(422f, 2f),
			new Vector2(338f, 2f),
			new Vector2(254f, 2f),
			new Vector2(170f, 2f),
			new Vector2(86f, 2f),
			new Vector2(2f, 2f)
		};
		switch (tileType)
		{
		case TileType.fullBlock:
			return ImageCoordsToUV(array[UnityEngine.Random.Range(0, array.Length)], randomRotation: true);
		case TileType.crackedBlock:
			return ImageCoordsToUV(new Vector2(86f, 338f), randomRotation: true);
		case TileType.upperLeftBlock:
			return ImageCoordsToUV(new Vector2(170f, 422f));
		case TileType.upperRightBlock:
			return ImageCoordsToUV(new Vector2(86f, 422f));
		case TileType.bottomLeftBlock:
			return ImageCoordsToUV(new Vector2(338f, 422f));
		case TileType.bottomRightBlock:
			return ImageCoordsToUV(new Vector2(254f, 422f));
		case TileType.destructibleUpperLeftBlock:
			return ImageCoordsToUV(new Vector2(338f, 338f));
		case TileType.destructibleUpperRightBlock:
			return ImageCoordsToUV(new Vector2(254f, 338f));
		case TileType.destructibleBottomLeftBlock:
			return ImageCoordsToUV(new Vector2(2f, 422f));
		case TileType.destructibleBottomRightBlock:
			return ImageCoordsToUV(new Vector2(422f, 338f));
		case TileType.simpleWallBlock:
			return ImageCoordsToUV(new Vector2(422f, 422f));
		case TileType.harderWallBlock:
			return ImageCoordsToUV(new Vector2(170f, 338f));
		case TileType.specialWallBlock:
			return ImageCoordsToUV(new Vector2(338f, 254f));
		case TileType.clear:
			return ImageCoordsToUV(new Vector2(86f, 86f));
		case TileType.grass:
		{
			Vector2 imageCoords = (!(UnityEngine.Random.value > 0.5f)) ? array2[UnityEngine.Random.Range(0, array2.Length)] : array3[UnityEngine.Random.Range(0, array3.Length)];
			return ImageCoordsToUV(imageCoords);
		}
		default:
			throw new Exception("could not get uvs for tile");
		}
	}
}
