using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BoardCreator
{
	public enum TileType
	{
		Wall,
		RoomFloor,
		CorridorFloor,
		SpecialWall
	}

	private int columns = 100;

	private int rows = 100;

	private int roomCount = 5;

	private IntRange roomWidth = new IntRange(6, 10);

	private IntRange roomHeight = new IntRange(6, 10);

	private IntRange corridorLength = new IntRange(2, 6);

	private TileType[][] tiles;

	private Room[] rooms;

	private Corridor[] corridors;

	private List<Vector2[]> verticalGates;

	private List<Vector2[]> horizontalGates;

	private List<List<Vector2>> spareRoomTilesCoords;

	private List<List<Vector2>> spareCorridorTilesCoords;

	private string spawners;

	private string turrets;

	private string bosses;

	private List<string> currentLevel;

	private int levelIndex;

	private float LoopFactor;

	private Corridor.LoopDirection loopDirection;

	private string GetSpawnersString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (levelIndex <= 23)
		{
			stringBuilder.Append('5');
		}
		if (levelIndex >= 11 && levelIndex <= 38)
		{
			stringBuilder.Append('h');
		}
		if (levelIndex >= 21)
		{
			if (levelIndex <= 25)
			{
				stringBuilder.Append('i');
			}
			else if (levelIndex <= 36)
			{
				stringBuilder.Append('i');
				stringBuilder.Append('i');
			}
			else if (levelIndex <= 45)
			{
				stringBuilder.Append('i');
			}
		}
		if (levelIndex >= 31)
		{
			if (levelIndex <= 35)
			{
				stringBuilder.Append('j');
			}
			else if (levelIndex <= 46)
			{
				stringBuilder.Append('j');
				stringBuilder.Append('j');
			}
			else if (levelIndex <= 50)
			{
				stringBuilder.Append('j');
			}
		}
		if (levelIndex >= 41)
		{
			if (levelIndex <= 45)
			{
				stringBuilder.Append('k');
			}
			else if (levelIndex <= 46)
			{
				stringBuilder.Append('k');
				stringBuilder.Append('k');
			}
			else if (levelIndex <= 50)
			{
				stringBuilder.Append('k');
				stringBuilder.Append('k');
				stringBuilder.Append('k');
			}
			else if (levelIndex <= 65)
			{
				stringBuilder.Append('k');
				stringBuilder.Append('k');
			}
		}
		if (levelIndex >= 51)
		{
			if (levelIndex <= 66)
			{
				stringBuilder.Append('l');
				stringBuilder.Append('l');
			}
			else if (levelIndex <= 70)
			{
				stringBuilder.Append('l');
				stringBuilder.Append('l');
				stringBuilder.Append('l');
			}
			else if (levelIndex <= 76)
			{
				stringBuilder.Append('l');
				stringBuilder.Append('l');
			}
		}
		if (levelIndex >= 61)
		{
			if (levelIndex <= 65)
			{
				stringBuilder.Append('m');
			}
			else if (levelIndex <= 70)
			{
				stringBuilder.Append('m');
				stringBuilder.Append('m');
			}
			else if (levelIndex <= 72)
			{
				stringBuilder.Append('m');
				stringBuilder.Append('m');
				stringBuilder.Append('m');
			}
			else if (levelIndex <= 76)
			{
				stringBuilder.Append('m');
				stringBuilder.Append('m');
				stringBuilder.Append('m');
				stringBuilder.Append('m');
			}
		}
		if (levelIndex >= 8)
		{
			stringBuilder.Append(stringBuilder.ToString());
		}
		return stringBuilder.ToString();
	}

	private string GetTurretsString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (levelIndex >= 8 && levelIndex <= 15)
		{
			stringBuilder.Append('u');
		}
		if (levelIndex >= 16 && levelIndex <= 30)
		{
			stringBuilder.Append('v');
		}
		if (levelIndex >= 21 && levelIndex <= 40)
		{
			stringBuilder.Append('w');
		}
		if (levelIndex >= 31 && levelIndex <= 50)
		{
			stringBuilder.Append('x');
		}
		if (levelIndex >= 41 && levelIndex <= 60)
		{
			stringBuilder.Append('y');
		}
		if (levelIndex >= 47 && levelIndex <= 76)
		{
			stringBuilder.Append('z');
		}
		if (levelIndex >= 52 && levelIndex <= 76)
		{
			stringBuilder.Append('A');
		}
		if (levelIndex >= 8)
		{
			stringBuilder.Append(stringBuilder.ToString());
		}
		return stringBuilder.ToString();
	}

	public List<string> GenerateLevel()
	{
		levelIndex = GlobalCommons.Instance.globalGameStats.LevelsCompleted + 1;
		spawners = GetSpawnersString();
		turrets = GetTurretsString();
		roomCount = spawners.Length;
		if (roomCount < 2)
		{
			roomCount = 2;
		}
		int num = 0;
		int num2 = 0;
		bool flag = false;
		do
		{
			if (num >= 3)
			{
				num = 0;
				roomCount++;
			}
			LoopFactor = UnityEngine.Random.Range(0.8f, 1f);
			loopDirection = Corridor.LoopDirection.Left;
			if (UnityEngine.Random.value > 0.5f)
			{
				loopDirection = Corridor.LoopDirection.Right;
			}
			spareRoomTilesCoords = new List<List<Vector2>>();
			spareCorridorTilesCoords = new List<List<Vector2>>();
			SetupTilesArray();
			CreateRoomsAndCorridors();
			SetTilesValuesForRooms();
			SetTilesValuesForCorridors();
			CheckGates();
			num++;
			num2++;
			if (num2 > 100)
			{
				throw new Exception("oh shit, could not create random level...");
			}
			if (!CheckOuterWalls())
			{
				continue;
			}
			PrepareStringLevel();
			if (SetLevelItems())
			{
				LevelUtils.CropLevel(currentLevel);
				if (CheckCroppedLevelWalls())
				{
					flag = true;
				}
			}
		}
		while (!flag);
		return currentLevel;
	}

	private bool CheckCroppedLevelWalls()
	{
		for (int i = 0; i < currentLevel.Count; i++)
		{
			if (i == 0 || i == currentLevel.Count - 1)
			{
				for (int j = 0; j < currentLevel[i].Length; j++)
				{
					if (!IsEligibleEdgeLevelItem(currentLevel[i][j]))
					{
						return false;
					}
				}
			}
			else if (!IsEligibleEdgeLevelItem(currentLevel[i][0]) || !IsEligibleEdgeLevelItem(currentLevel[i][currentLevel[i].Length - 1]))
			{
				return false;
			}
		}
		return true;
	}

	private bool IsEligibleEdgeLevelItem(char ch)
	{
		if (ch != 'f' && ch != '9')
		{
			return false;
		}
		return true;
	}

	private bool CheckOuterWalls()
	{
		for (int i = 0; i < columns; i++)
		{
			if (tiles[i][0] != 0)
			{
				return false;
			}
			if (tiles[i][rows - 1] != 0)
			{
				return false;
			}
			if (tiles[0][i] != 0)
			{
				return false;
			}
			if (tiles[rows - 1][i] != 0)
			{
				return false;
			}
		}
		return true;
	}

	private bool SetLevelItems()
	{
		Vector2 value = SetLevelItem('4', TileType.RoomFloor, 0).Value;
		int itemCount = roomCount;
		int itemCount2 = roomCount / 2 + UnityEngine.Random.Range(0, roomCount);
		int itemCount3 = roomCount / 2 + UnityEngine.Random.Range(0, roomCount);
		int itemCount4 = roomCount / 2;
		string text = spawners;
		foreach (char itemType in text)
		{
			if (!SetLevelItemPack(itemType, TileType.RoomFloor, 1, evadePlayersRoom: true, randomOrder: true, value))
			{
				return false;
			}
		}
		string text2 = turrets;
		foreach (char itemType2 in text2)
		{
			if (!SetLevelItemPack(itemType2, TileType.RoomFloor, 1, evadePlayersRoom: true, randomOrder: true, value))
			{
				return false;
			}
		}
		if (!SetLevelItemPack('7', TileType.RoomFloor, itemCount, evadePlayersRoom: false, randomOrder: true, null))
		{
			return false;
		}
		if (!SetLevelItemPack('1', TileType.RoomFloor, itemCount2, evadePlayersRoom: false, randomOrder: true, null))
		{
			return false;
		}
		if (!SetLevelItemPack('8', TileType.RoomFloor, itemCount3, evadePlayersRoom: false, randomOrder: true, null))
		{
			return false;
		}
		if (!SetLevelItemPack('6', TileType.RoomFloor, itemCount4, evadePlayersRoom: false, randomOrder: true, null))
		{
			return false;
		}
		if (((UnityEngine.Random.value > 0.8f) ? true : false) && !SetLevelItemPack('F', TileType.RoomFloor, 1, evadePlayersRoom: true, randomOrder: true, value))
		{
			return false;
		}
		return true;
	}

	private bool SetLevelItemPack(char itemType, TileType location, int itemCount, bool evadePlayersRoom, bool randomOrder, Vector2? evadePoint)
	{
		int start = 0;
		if (evadePlayersRoom)
		{
			start = 1;
		}
		List<int> list = GenerateIndexesList(start, roomCount);
		int num = 0;
		do
		{
			int index = randomOrder ? UnityEngine.Random.Range(0, list.Count) : 0;
			if (SetLevelItem(itemType, TileType.RoomFloor, list[index], evadePoint).HasValue)
			{
				itemCount--;
			}
			list.RemoveAt(index);
			if (list.Count == 0)
			{
				list = GenerateIndexesList(start, roomCount);
			}
			num++;
			if (num > 5000)
			{
				return false;
			}
		}
		while (itemCount > 0);
		return true;
	}

	private List<int> GenerateIndexesList(int start, int max)
	{
		List<int> list = new List<int>();
		for (int i = start; i < max; i++)
		{
			list.Add(i);
		}
		return list;
	}

	private Vector2? SetLevelItem(char itemType, TileType location, int locationIndex, Vector2? evadePoint = default(Vector2?))
	{
		List<Vector2> list = (location != TileType.CorridorFloor) ? spareRoomTilesCoords[locationIndex] : spareCorridorTilesCoords[locationIndex];
		if (list.Count == 0)
		{
			return null;
		}
		bool flag = false;
		int num = 0;
		int index;
		Vector2 value;
		do
		{
			index = UnityEngine.Random.Range(0, list.Count);
			value = list[index];
			if (evadePoint.HasValue)
			{
				Vector2 value2 = evadePoint.Value;
				if (Mathf.Abs(value2.x - value.x) <= 6f)
				{
					Vector2 value3 = evadePoint.Value;
					if (Mathf.Abs(value3.y - value.y) <= 6f)
					{
						flag = true;
						goto IL_00b9;
					}
				}
				flag = false;
			}
			goto IL_00b9;
			IL_00b9:
			num++;
			if (num == 10)
			{
				return null;
			}
		}
		while (flag);
		list.RemoveAt(index);
		string value4 = currentLevel[(int)value.x];
		StringBuilder stringBuilder = new StringBuilder(value4);
		stringBuilder[(int)value.y] = itemType;
		currentLevel[(int)value.x] = stringBuilder.ToString();
		return value;
	}

	private void PrepareStringLevel()
	{
		currentLevel = new List<string>();
		for (int i = 0; i < 100; i++)
		{
			string text = string.Empty;
			for (int j = 0; j < 100; j++)
			{
				TileType tileType = tiles[i][j];
				string empty = string.Empty;
				switch (tileType)
				{
				case TileType.CorridorFloor:
					empty = "0";
					break;
				case TileType.RoomFloor:
					empty = "0";
					break;
				case TileType.SpecialWall:
					empty = "e";
					break;
				case TileType.Wall:
					empty = "f";
					break;
				default:
					throw new Exception("unknown tile type lol");
				}
				text += empty;
			}
			currentLevel.Add(text);
		}
		for (int k = 0; k < currentLevel.Count; k++)
		{
			string value = currentLevel[k];
			StringBuilder stringBuilder = new StringBuilder(value);
			for (int l = 0; l < stringBuilder.Length; l++)
			{
				if (stringBuilder[l] == 'f' || stringBuilder[l] == '9')
				{
					continue;
				}
				if (l > 0 && stringBuilder[l - 1] == 'f')
				{
					stringBuilder[l - 1] = '9';
				}
				if (l < stringBuilder.Length - 1 && stringBuilder[l + 1] == 'f')
				{
					stringBuilder[l + 1] = '9';
				}
				if (k > 0)
				{
					string value2 = currentLevel[k - 1];
					StringBuilder stringBuilder2 = new StringBuilder(value2);
					if (stringBuilder2[l] == 'f')
					{
						stringBuilder2[l] = '9';
					}
					if (l > 0 && stringBuilder2[l - 1] == 'f')
					{
						stringBuilder2[l - 1] = '9';
					}
					if (l < stringBuilder2.Length - 1 && stringBuilder2[l + 1] == 'f')
					{
						stringBuilder2[l + 1] = '9';
					}
					currentLevel[k - 1] = stringBuilder2.ToString();
				}
				if (k < currentLevel.Count - 1)
				{
					string value3 = currentLevel[k + 1];
					StringBuilder stringBuilder3 = new StringBuilder(value3);
					if (stringBuilder3[l] == 'f')
					{
						stringBuilder3[l] = '9';
					}
					if (l > 0 && stringBuilder3[l - 1] == 'f')
					{
						stringBuilder3[l - 1] = '9';
					}
					if (l < stringBuilder3.Length - 1 && stringBuilder3[l + 1] == 'f')
					{
						stringBuilder3[l + 1] = '9';
					}
					currentLevel[k + 1] = stringBuilder3.ToString();
				}
			}
			currentLevel[k] = stringBuilder.ToString();
		}
	}

	private void SetupTilesArray()
	{
		tiles = new TileType[columns][];
		for (int i = 0; i < tiles.Length; i++)
		{
			tiles[i] = new TileType[rows];
		}
	}

	private void CreateRoomsAndCorridors()
	{
		rooms = new Room[roomCount];
		corridors = new Corridor[rooms.Length - 1];
		rooms[0] = new Room();
		corridors[0] = new Corridor();
		rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);
		corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, firstCorridor: true, LoopFactor, loopDirection);
		for (int i = 1; i < rooms.Length; i++)
		{
			rooms[i] = new Room();
			rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i - 1]);
			if (i < corridors.Length)
			{
				corridors[i] = new Corridor();
				corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, firstCorridor: false, LoopFactor, loopDirection);
			}
		}
	}

	private void SetTilesValuesForRooms()
	{
		for (int i = 0; i < rooms.Length; i++)
		{
			Room room = rooms[i];
			List<Vector2> list = new List<Vector2>();
			for (int j = 0; j < room.roomWidth; j++)
			{
				int num = room.xPos + j;
				for (int k = 0; k < room.roomHeight; k++)
				{
					int num2 = room.yPos + k;
					if (tiles[num][num2] != TileType.RoomFloor)
					{
						tiles[num][num2] = TileType.RoomFloor;
						list.Add(new Vector2(num, num2));
					}
				}
			}
			spareRoomTilesCoords.Add(list);
		}
	}

	private void SetTilesValuesForCorridors()
	{
		verticalGates = new List<Vector2[]>();
		horizontalGates = new List<Vector2[]>();
		bool flag = true;
		for (int i = 0; i < corridors.Length; i++)
		{
			Corridor corridor = corridors[i];
			List<Vector2> list = new List<Vector2>();
			if (i > 0)
			{
				flag = (UnityEngine.Random.value > 0.5f);
			}
			for (int j = 0; j < corridor.corridorLength; j++)
			{
				int num = corridor.startXPos;
				int num2 = corridor.startYPos;
				switch (corridor.direction)
				{
				case Direction.North:
					num2 += j;
					break;
				case Direction.East:
					num += j;
					break;
				case Direction.South:
					num2 -= j;
					break;
				case Direction.West:
					num -= j;
					break;
				}
				TileType tileType = ((j != 1 && j != corridor.corridorLength - 2) || !flag) ? TileType.CorridorFloor : TileType.SpecialWall;
				if (tileType == TileType.CorridorFloor && tiles[num][num2] != TileType.CorridorFloor)
				{
					list.Add(new Vector2(num, num2));
				}
				tiles[num][num2] = tileType;
				if (corridor.direction == Direction.North || corridor.direction == Direction.South)
				{
					int num3 = (num < columns - 1) ? (num + 1) : (num - 1);
					tiles[num3][num2] = tileType;
					if (tileType == TileType.SpecialWall)
					{
						horizontalGates.Add(new Vector2[3]
						{
							new Vector2(num, num2),
							new Vector2(num3, num2),
							new Vector2(i, i)
						});
					}
				}
				else
				{
					int num4 = (num2 < rows - 1) ? (num2 + 1) : (num2 - 1);
					tiles[num][num4] = tileType;
					if (tileType == TileType.SpecialWall)
					{
						verticalGates.Add(new Vector2[3]
						{
							new Vector2(num, num2),
							new Vector2(num, num4),
							new Vector2(i, i)
						});
					}
				}
			}
			spareCorridorTilesCoords.Add(list);
		}
	}

	private void CheckGates()
	{
		List<int> list = new List<int>();
		foreach (Vector2[] verticalGate in verticalGates)
		{
			Vector2 vector;
			Vector2 vector2;
			if (verticalGate[0].y < verticalGate[1].y)
			{
				vector = verticalGate[1];
				vector2 = verticalGate[0];
			}
			else
			{
				vector = verticalGate[0];
				vector2 = verticalGate[1];
			}
			if (((int)vector.y + 1 < rows && IsFloorType(tiles[(int)vector.x][(int)vector.y + 1])) || ((int)vector2.y - 1 >= 0 && IsFloorType(tiles[(int)vector2.x][(int)vector2.y - 1])) || list.IndexOf((int)verticalGate[2].x) != -1)
			{
				tiles[(int)vector.x][(int)vector.y] = TileType.CorridorFloor;
				tiles[(int)vector2.x][(int)vector2.y] = TileType.CorridorFloor;
			}
			else
			{
				list.Add((int)verticalGate[2].x);
			}
		}
		list = new List<int>();
		foreach (Vector2[] horizontalGate in horizontalGates)
		{
			Vector2 vector3;
			Vector2 vector4;
			if (horizontalGate[0].x < horizontalGate[1].x)
			{
				vector3 = horizontalGate[0];
				vector4 = horizontalGate[1];
			}
			else
			{
				vector3 = horizontalGate[1];
				vector4 = horizontalGate[0];
			}
			if (((int)vector3.x - 1 >= 0 && IsFloorType(tiles[(int)vector3.x - 1][(int)vector3.y])) || ((int)vector4.x + 1 < columns && IsFloorType(tiles[(int)vector4.x + 1][(int)vector4.y])) || list.IndexOf((int)horizontalGate[2].x) != -1)
			{
				tiles[(int)vector3.x][(int)vector3.y] = TileType.CorridorFloor;
				tiles[(int)vector4.x][(int)vector4.y] = TileType.CorridorFloor;
			}
			else
			{
				list.Add((int)horizontalGate[2].x);
			}
		}
	}

	private bool IsFloorType(TileType tile)
	{
		if (tile == TileType.CorridorFloor || tile == TileType.RoomFloor)
		{
			return true;
		}
		return false;
	}
}
