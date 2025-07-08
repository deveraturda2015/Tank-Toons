using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
	private List<string> currentLevel;

	private bool phaseOneInitialized;

	private bool phaseTwoInitialized;

	private bool phaseThreeInitialized;

	private bool currentTargetDisplayed;

	private const float PlayerSpawnTimeOffset = 0.1f;

	private const float ItemsSpawnTimeOffset = 0.2f;

	private List<Vector3> visibilityCoversCoords;

	private List<Vector3> forceRectVisibilityCoversCoords;

	private List<IntVector2> secretRoomCoords;

	private List<LevelSpawnItem> LevelSpawnItems;

	private bool LevelSpawnItemsSorted;

	private IntVector2 playerSpawnPt;

	public Sprite GrassSprite;

	public Sprite SnowSprite;

	public Sprite DesertSprite;

	private bool fightSignShown;

	private TileMap.TilesType tilesType;

	private List<TileFlyIn> tilesFlyIns = new List<TileFlyIn>();

	private System.Random bushesSeed;

	private int updateIndex;

	private List<char> unknownChars;

	private void Start()
	{
		InitializeCurrentLevel();
		GameplayCommons.Instance.currentLevel = currentLevel;
		GameplayCommons.Instance.tileMap.InitTileMap(tilesType);
		GameplayCommons.Instance.tileMap.GetComponent<MeshRenderer>().enabled = false;
	}

	private void SpawnBush(Vector3 spawnPoint)
	{
		GameObject gameObject;
		switch (tilesType)
		{
		case TileMap.TilesType.DesertTiles:
			gameObject = UnityEngine.Object.Instantiate(Prefabs.BushDesertPrefab, spawnPoint, Quaternion.identity);
			break;
		case TileMap.TilesType.SummerTiles:
			gameObject = UnityEngine.Object.Instantiate(Prefabs.BushPrefab, spawnPoint, Quaternion.identity);
			break;
		case TileMap.TilesType.WinterTiles:
			gameObject = UnityEngine.Object.Instantiate(Prefabs.BushSnowPrefab, spawnPoint, Quaternion.identity);
			break;
		default:
			throw new Exception("no bush specified for tile type " + tilesType.ToString());
		}
		float num = UnityEngine.Random.Range(0.9f, 1f);
		gameObject.GetComponent<SpriteRenderer>().color = new Color(num, num, num);
	}

	private void Update()
	{
		updateIndex++;
		if (updateIndex == 2)
		{
			GameplayCommons.Instance.RemoveLoadingSign();
		}
		if (!phaseOneInitialized)
		{
			phaseOneInitialized = true;
			List<Vector3> list = new List<Vector3>();
			List<Vector3> list2 = new List<Vector3>();
			visibilityCoversCoords = new List<Vector3>();
			forceRectVisibilityCoversCoords = new List<Vector3>();
			unknownChars = new List<char>();
			for (int i = 0; i < currentLevel.Count; i++)
			{
				string text = currentLevel[i];
				char[] array = new char[text.Length];
				using (StringReader stringReader = new StringReader(text))
				{
					stringReader.Read(array, 0, text.Length);
					for (int j = 0; j < array.Length; j++)
					{
						Vector3 vector = new Vector3((float)j * GlobalCommons.Instance.gridSize, (float)i * GlobalCommons.Instance.gridSize, 0f);
						bool flag = false;
						if (secretRoomCoords != null)
						{
							foreach (IntVector2 secretRoomCoord in secretRoomCoords)
							{
								IntVector2 current = secretRoomCoord;
								if (current.x == j && current.y == i)
								{
									flag = true;
									break;
								}
							}
						}
						if (array[j] != '9' && array[j] != 'f')
						{
							SpawnGrassTile(vector);
							list2.Add(vector);
						}
						if (array[j] != 'f')
						{
							if (flag)
							{
								forceRectVisibilityCoversCoords.Add(vector);
							}
							else
							{
								visibilityCoversCoords.Add(vector);
								list.Add(vector);
							}
						}
						switch (array[j])
						{
						case '0':
						{
							float num = 0.95f;
							float num2 = 0.88f;
							float num3 = num;
							switch (GlobalCommons.Instance.gameplayMode)
							{
							case GlobalCommons.GameplayModes.RegularLevel:
								num3 = num - (float)GlobalCommons.Instance.ActualSelectedLevel / 75f * (num - num2);
								if (num3 < num2)
								{
									num3 = num2;
								}
								break;
							case GlobalCommons.GameplayModes.EditorLevel:
								num3 = 1f;
								break;
							case GlobalCommons.GameplayModes.CustomLevel:
								num3 = 1f;
								break;
							default:
								throw new Exception("unknown game mode");
							case GlobalCommons.GameplayModes.ArenaLevel:
							case GlobalCommons.GameplayModes.SurvivalLevel:
							case GlobalCommons.GameplayModes.TutorialLevel:
								break;
							}
							if (!flag && (float)bushesSeed.Next() > num3 * 2.14748365E+09f)
							{
								SpawnBush(vector);
							}
							break;
						}
						case 'I':
							SpawnBush(vector);
							break;
						case '1':
							UnityEngine.Object.Instantiate(Prefabs.brickWall, vector, Quaternion.identity);
							break;
						case '4':
							playerSpawnPt = new IntVector2(j, i);
							GameplayCommons.Instance.playersTankController = UnityEngine.Object.Instantiate(Prefabs.playersTankPrefab, vector, Quaternion.identity).GetComponent<PlayersTankController>();
							break;
						case '8':
							UnityEngine.Object.Instantiate(Prefabs.harderBrickWall, vector, Quaternion.identity);
							break;
						case '9':
						{
							Quaternion rotation = Quaternion.Euler(0f, 0f, 90 * UnityEngine.Random.Range(0, 4));
							SpriteRenderer component2 = UnityEngine.Object.Instantiate(Prefabs.indestructableWall, vector, rotation).GetComponent<SpriteRenderer>();
							tilesFlyIns.Add(component2.GetComponent<TileFlyIn>());
							break;
						}
						case 'a':
							UnityEngine.Object.Instantiate(Prefabs.cornerTopRight, vector, Quaternion.identity);
							break;
						case 'b':
							UnityEngine.Object.Instantiate(Prefabs.cornerTopLeft, vector, Quaternion.identity);
							break;
						case 'c':
							UnityEngine.Object.Instantiate(Prefabs.cornerBottomRight, vector, Quaternion.identity);
							break;
						case 'd':
							UnityEngine.Object.Instantiate(Prefabs.cornerBottomLeft, vector, Quaternion.identity);
							break;
						case 'e':
							UnityEngine.Object.Instantiate(Prefabs.specialWall, vector, Quaternion.identity);
							break;
						case 'g':
							UnityEngine.Object.Instantiate(Prefabs.crackedIndestructableWall, vector, Quaternion.identity);
							break;
						case 'B':
						{
							TileFlyIn component = UnityEngine.Object.Instantiate(Prefabs.indestructibleCornerTopRight, vector, Quaternion.identity).GetComponent<TileFlyIn>();
							tilesFlyIns.Add(component);
							break;
						}
						case 'C':
						{
							TileFlyIn component = UnityEngine.Object.Instantiate(Prefabs.indestructibleCornerTopLeft, vector, Quaternion.identity).GetComponent<TileFlyIn>();
							tilesFlyIns.Add(component);
							break;
						}
						case 'D':
						{
							TileFlyIn component = UnityEngine.Object.Instantiate(Prefabs.indestructibleCornerBottomRight, vector, Quaternion.identity).GetComponent<TileFlyIn>();
							tilesFlyIns.Add(component);
							break;
						}
						case 'E':
						{
							TileFlyIn component = UnityEngine.Object.Instantiate(Prefabs.indestructibleCornerBottomLeft, vector, Quaternion.identity).GetComponent<TileFlyIn>();
							tilesFlyIns.Add(component);
							break;
						}
						default:
							if (unknownChars.IndexOf(array[j]) == -1)
							{
								unknownChars.Add(array[j]);
							}
							break;
						}
					}
				}
			}
			GameplayCommons.Instance.levelStateController.SetWaypoints(list);
			GameplayCommons.Instance.visibilityController.Initialize(visibilityCoversCoords, forceRectVisibilityCoversCoords);
			SurvivalModeController survivalModeController = UnityEngine.Object.FindObjectOfType<SurvivalModeController>();
			if ((bool)survivalModeController)
			{
				survivalModeController.Initialize(list2);
			}
		}
		if (!phaseTwoInitialized && Time.timeSinceLevelLoad > TileFlyIn.AllTilesSetTime + 0.1f)
		{
			GameplayCommons.Instance.gameplayUIController.FlashScreen();
			GameplayCommons.Instance.tileMap.GetComponent<MeshRenderer>().enabled = true;
			for (int k = 0; k < GameplayCommons.Instance.enemiesTracker.AllDestObstacles.Count; k++)
			{
				DestructableObstacleController destructableObstacleController = GameplayCommons.Instance.enemiesTracker.AllDestObstacles[k];
				destructableObstacleController.ProcessAfterFlashSetup();
			}
			for (int l = 0; l < GameplayCommons.Instance.enemiesTracker.AllBushes.Count; l++)
			{
				BushController bushController = GameplayCommons.Instance.enemiesTracker.AllBushes[l];
				bushController.ShowBush();
			}
			for (int m = 0; m < tilesFlyIns.Count; m++)
			{
				tilesFlyIns[m].FinalizeTileState();
			}
			phaseTwoInitialized = true;
			GameplayCommons.Instance.playersTankController.ActivatePlayer();
		}
		if (!currentTargetDisplayed && Time.timeSinceLevelLoad > 0.5f)
		{
			currentTargetDisplayed = true;
			if (GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.TutorialLevel && GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.CustomLevel && GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.EditorLevel)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.CurrentTargetController);
				gameObject.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
			}
		}
		if (!phaseThreeInitialized && Time.timeSinceLevelLoad > TileFlyIn.AllTilesSetTime + 0.2f)
		{
			if (!fightSignShown)
			{
				fightSignShown = true;
				GameObject gameObject2 = UnityEngine.Object.Instantiate(Prefabs.FightSign, Vector3.zero, Quaternion.identity);
				gameObject2.transform.SetParent(GameObject.Find("Canvas").transform, worldPositionStays: false);
			}
			string spawnItems = "3567H2hijklmnopqrstFuvwxyzAXYZ!*#$%^&(RSTUVWOPQ";
			if (LevelSpawnItems == null)
			{
				LevelSpawnItems = new List<LevelSpawnItem>();
				for (int n = 0; n < currentLevel.Count; n++)
				{
					string text2 = currentLevel[n];
					char[] array2 = new char[text2.Length];
					using (StringReader stringReader2 = new StringReader(text2))
					{
						stringReader2.Read(array2, 0, text2.Length);
						for (int num4 = 0; num4 < array2.Length; num4++)
						{
							if (spawnItems.IndexOf(array2[num4]) != -1)
							{
								LevelSpawnItems.Add(new LevelSpawnItem(array2[num4], new IntVector2(num4, n)));
							}
						}
					}
				}
				return;
			}
			List<char> totallyUnknownChars = new List<char>();
			unknownChars.ForEach(delegate(char ch)
			{
				if (spawnItems.IndexOf(ch) == -1 && totallyUnknownChars.IndexOf(ch) == -1)
				{
					totallyUnknownChars.Add(ch);
				}
			});
			unknownChars = totallyUnknownChars;
			if (totallyUnknownChars.Count > 1)
			{
				GlobalCommons.Instance.StateFaderController.ChangeSceneTo("MainMenu");
				phaseThreeInitialized = true;
				MainMenuController.FailedLevelMessagePending = true;
				return;
			}
			if (!LevelSpawnItemsSorted)
			{
				LevelSpawnItemsSorted = true;
				Vector2 playerPt = new Vector2(playerSpawnPt.x, playerSpawnPt.y);
				LevelSpawnItems.Sort(delegate(LevelSpawnItem x, LevelSpawnItem y)
				{
					Vector2 vector3 = playerPt - new Vector2(x.coords.x, x.coords.y);
					Vector2 vector4 = playerPt - new Vector2(y.coords.x, y.coords.y);
					return vector3.sqrMagnitude.CompareTo(vector4.sqrMagnitude);
				});
			}
			if (LevelSpawnItems.Count > 0)
			{
				for (int num5 = 0; num5 < 3; num5++)
				{
					if (LevelSpawnItems.Count == 0)
					{
						break;
					}
					LevelSpawnItem levelSpawnItem = LevelSpawnItems[0];
					LevelSpawnItems.RemoveAt(0);
					int x2 = levelSpawnItem.coords.x;
					int y2 = levelSpawnItem.coords.y;
					Vector3 vector2 = new Vector3((float)x2 * GlobalCommons.Instance.gridSize, (float)y2 * GlobalCommons.Instance.gridSize, 0f);
					switch (levelSpawnItem.itemChar)
					{
					case '3':
					{
						EnemyTankController component37 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component37.InitializeEnemy(EnemyTankController.EnemyTypes.turret, WeaponTypes.random, isBoss: false);
						break;
					}
					case '5':
						InstantiateSpawner(1, vector2);
						break;
					case '6':
						UnityEngine.Object.Instantiate(Prefabs.explodingBarrelPrefab, vector2, Quaternion.identity);
						break;
					case '7':
						UnityEngine.Object.Instantiate(Prefabs.bonusCratePrefab, vector2, Quaternion.identity);
						break;
					case 'H':
					{
						DestructableObstacleController component36 = UnityEngine.Object.Instantiate(Prefabs.bonusCratePrefab, vector2, Quaternion.identity).GetComponent<DestructableObstacleController>();
						component36.InitSpecialPrizeCrate();
						break;
					}
					case '2':
					{
						EnemyTankController component35 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component35.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.random, isBoss: false);
						break;
					}
					case 'h':
						InstantiateSpawner(2, vector2);
						break;
					case 'i':
						InstantiateSpawner(3, vector2);
						break;
					case 'j':
						InstantiateSpawner(4, vector2);
						break;
					case 'k':
						InstantiateSpawner(5, vector2);
						break;
					case 'l':
						InstantiateSpawner(6, vector2);
						break;
					case 'm':
						InstantiateSpawner(7, vector2);
						break;
					case 'O':
						InstantiateSpawner(8, vector2);
						break;
					case 'P':
						InstantiateSpawner(9, vector2);
						break;
					case 'Q':
						InstantiateSpawner(10, vector2);
						break;
					case 'n':
					{
						EnemyTankController component34 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component34.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.machinegun, isBoss: true);
						break;
					}
					case 'o':
					{
						EnemyTankController component33 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component33.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.shotgun, isBoss: true);
						break;
					}
					case 'p':
					{
						EnemyTankController component32 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component32.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.minigun, isBoss: true);
						break;
					}
					case 'q':
					{
						EnemyTankController component31 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component31.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.cannon, isBoss: true);
						break;
					}
					case 'r':
					{
						EnemyTankController component30 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component30.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.homingRocket, isBoss: true);
						break;
					}
					case 's':
					{
						EnemyTankController component29 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component29.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.laser, isBoss: true);
						break;
					}
					case 't':
					{
						EnemyTankController component28 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component28.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.railgun, isBoss: true);
						break;
					}
					case 'U':
					{
						EnemyTankController component27 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component27.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.ricochet, isBoss: true);
						break;
					}
					case 'V':
					{
						EnemyTankController component26 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component26.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.triple, isBoss: true);
						break;
					}
					case 'W':
					{
						EnemyTankController component25 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component25.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.shocker, isBoss: true);
						break;
					}
					case 'F':
					{
						EnemyTankController component24 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component24.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.gold, isBoss: false);
						break;
					}
					case 'u':
					{
						EnemyTankController component23 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component23.InitializeEnemy(EnemyTankController.EnemyTypes.turret, WeaponTypes.machinegun, isBoss: false);
						break;
					}
					case 'v':
					{
						EnemyTankController component22 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component22.InitializeEnemy(EnemyTankController.EnemyTypes.turret, WeaponTypes.shotgun, isBoss: false);
						break;
					}
					case 'w':
					{
						EnemyTankController component21 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component21.InitializeEnemy(EnemyTankController.EnemyTypes.turret, WeaponTypes.minigun, isBoss: false);
						break;
					}
					case 'x':
					{
						EnemyTankController component20 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component20.InitializeEnemy(EnemyTankController.EnemyTypes.turret, WeaponTypes.cannon, isBoss: false);
						break;
					}
					case 'y':
					{
						EnemyTankController component19 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component19.InitializeEnemy(EnemyTankController.EnemyTypes.turret, WeaponTypes.homingRocket, isBoss: false);
						break;
					}
					case 'z':
					{
						EnemyTankController component18 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component18.InitializeEnemy(EnemyTankController.EnemyTypes.turret, WeaponTypes.laser, isBoss: false);
						break;
					}
					case 'A':
					{
						EnemyTankController component17 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component17.InitializeEnemy(EnemyTankController.EnemyTypes.turret, WeaponTypes.railgun, isBoss: false);
						break;
					}
					case 'R':
					{
						EnemyTankController component16 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component16.InitializeEnemy(EnemyTankController.EnemyTypes.turret, WeaponTypes.ricochet, isBoss: false);
						break;
					}
					case 'S':
					{
						EnemyTankController component15 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component15.InitializeEnemy(EnemyTankController.EnemyTypes.turret, WeaponTypes.triple, isBoss: false);
						break;
					}
					case 'T':
					{
						EnemyTankController component14 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component14.InitializeEnemy(EnemyTankController.EnemyTypes.turret, WeaponTypes.shocker, isBoss: false);
						break;
					}
					case 'X':
					{
						EnemyTankController component13 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component13.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.machinegun, isBoss: false);
						break;
					}
					case 'Y':
					{
						EnemyTankController component12 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component12.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.shotgun, isBoss: false);
						break;
					}
					case 'Z':
					{
						EnemyTankController component11 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component11.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.minigun, isBoss: false);
						break;
					}
					case '!':
					{
						EnemyTankController component10 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component10.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.cannon, isBoss: false);
						break;
					}
					case '*':
					{
						EnemyTankController component9 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component9.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.homingRocket, isBoss: false);
						break;
					}
					case '#':
					{
						EnemyTankController component8 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component8.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.laser, isBoss: false);
						break;
					}
					case '$':
					{
						EnemyTankController component7 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component7.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.railgun, isBoss: false);
						break;
					}
					case '%':
					{
						EnemyTankController component6 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component6.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.ricochet, isBoss: false);
						break;
					}
					case '^':
					{
						EnemyTankController component5 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component5.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.triple, isBoss: false);
						break;
					}
					case '&':
					{
						EnemyTankController component4 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component4.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.shocker, isBoss: false);
						break;
					}
					case '(':
					{
						EnemyTankController component3 = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector2, Quaternion.identity).GetComponent<EnemyTankController>();
						component3.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.suicide, isBoss: false);
						break;
					}
					}
				}
			}
			else
			{
				phaseThreeInitialized = true;
			}
		}
		if (phaseOneInitialized && phaseTwoInitialized && phaseThreeInitialized && currentTargetDisplayed)
		{
			GameplayCommons.Instance.LevelFullyInitialized = true;
			UnityEngine.Object.Destroy(this);
		}
	}

	private void InstantiateSpawner(int level, Vector3 spawnPoint)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.enemiesSpawnerPrefab, spawnPoint, Quaternion.identity);
		gameObject.GetComponent<EnemySpawnerController>().InitializeSpawner(level);
	}

	private void SpawnGrassTile(Vector3 spawnPoint)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.grassTilePrefab, spawnPoint, Quaternion.identity);
		tilesFlyIns.Add(gameObject.GetComponent<TileFlyIn>());
		RandomizeSRBrightness(gameObject);
		switch (GlobalCommons.Instance.gameplayMode)
		{
		case GlobalCommons.GameplayModes.ArenaLevel:
			gameObject.GetComponent<SpriteRenderer>().sprite = GetSpriteForTileType(tilesType);
			break;
		case GlobalCommons.GameplayModes.TutorialLevel:
			gameObject.GetComponent<SpriteRenderer>().sprite = GrassSprite;
			break;
		case GlobalCommons.GameplayModes.RegularLevel:
			gameObject.GetComponent<SpriteRenderer>().sprite = GetSpriteForTileType(tilesType);
			break;
		case GlobalCommons.GameplayModes.SurvivalLevel:
			gameObject.GetComponent<SpriteRenderer>().sprite = GetSpriteForTileType(tilesType);
			break;
		case GlobalCommons.GameplayModes.EditorLevel:
			gameObject.GetComponent<SpriteRenderer>().sprite = GetSpriteForTileType(tilesType);
			break;
		case GlobalCommons.GameplayModes.CustomLevel:
			gameObject.GetComponent<SpriteRenderer>().sprite = GetSpriteForTileType(tilesType);
			break;
		default:
			throw new Exception("non determined tile type for game mode " + GlobalCommons.Instance.gameplayMode.ToString());
		}
	}

	private Sprite GetSpriteForTileType(TileMap.TilesType tilesType)
	{
		switch (tilesType)
		{
		case TileMap.TilesType.WinterTiles:
			return SnowSprite;
		case TileMap.TilesType.SummerTiles:
			return GrassSprite;
		case TileMap.TilesType.DesertTiles:
			return DesertSprite;
		default:
			throw new Exception("unknown tile type");
		}
	}

	private void RandomizeSRBrightness(GameObject go)
	{
		float num = 0.8f + UnityEngine.Random.Range(0f, 0.2f);
		go.GetComponent<SpriteRenderer>().color = new Color(num, num, num);
	}

	private int TilesTypeToSeasonFactor(TileMap.TilesType tilesType)
	{
		switch (tilesType)
		{
		case TileMap.TilesType.SummerTiles:
			return 1;
		case TileMap.TilesType.DesertTiles:
			return 2;
		case TileMap.TilesType.WinterTiles:
			return 0;
		default:
			throw new Exception("Cannot convert to season factor");
		}
	}

	private void InitializeCurrentLevel()
	{
		int num = Enum.GetNames(typeof(TileMap.TilesType)).Length;
		bushesSeed = new System.Random(UnityEngine.Random.Range(1, int.MaxValue));
		int num2;
		switch (GlobalCommons.Instance.gameplayMode)
		{
		case GlobalCommons.GameplayModes.ArenaLevel:
			num2 = UnityEngine.Random.Range(0, num);
			break;
		case GlobalCommons.GameplayModes.RegularLevel:
			bushesSeed = new System.Random(GlobalCommons.Instance.ActualSelectedLevel);
			num2 = GlobalCommons.Instance.ActualSelectedLevel % num;
			break;
		case GlobalCommons.GameplayModes.SurvivalLevel:
			num2 = UnityEngine.Random.Range(0, num);
			break;
		case GlobalCommons.GameplayModes.TutorialLevel:
			bushesSeed = new System.Random(4);
			num2 = 1;
			break;
		case GlobalCommons.GameplayModes.EditorLevel:
			num2 = TilesTypeToSeasonFactor(LevelEditorController.LevelToWorkWithTilesType);
			break;
		case GlobalCommons.GameplayModes.CustomLevel:
			num2 = TilesTypeToSeasonFactor(LevelEditorController.LoadedCustomLevelTilesType);
			break;
		default:
			throw new Exception("unknown mode");
		}
		switch (num2)
		{
		case 0:
			tilesType = TileMap.TilesType.WinterTiles;
			break;
		case 1:
			tilesType = TileMap.TilesType.SummerTiles;
			break;
		case 2:
			tilesType = TileMap.TilesType.DesertTiles;
			break;
		}
		GameplayCommons.Instance.InitializeEffectsSpawner(tilesType);
		switch (GlobalCommons.Instance.gameplayMode)
		{
		case GlobalCommons.GameplayModes.RegularLevel:
			currentLevel = (from item in GlobalCommons.Instance.levelsContainer.GetLevelByIndex(GlobalCommons.Instance.ActualSelectedLevel - 1)
				select (string)item.Clone()).ToList();
			AddSecretRoom();
			break;
		case GlobalCommons.GameplayModes.ArenaLevel:
		{
			BoardCreator boardCreator = new BoardCreator();
			currentLevel = boardCreator.GenerateLevel();
			AddSecretRoom();
			break;
		}
		case GlobalCommons.GameplayModes.SurvivalLevel:
			currentLevel = GlobalCommons.Instance.levelsContainer.GetSurvivalLevel();
			break;
		case GlobalCommons.GameplayModes.TutorialLevel:
			currentLevel = GlobalCommons.Instance.levelsContainer.GetTutorialLevel();
			break;
		case GlobalCommons.GameplayModes.EditorLevel:
			currentLevel = new List<string>();
			for (int j = 0; j < LevelEditorController.LevelToWorkWith.Count; j++)
			{
				currentLevel.Add(LevelEditorController.LevelToWorkWith[j]);
			}
			break;
		case GlobalCommons.GameplayModes.CustomLevel:
			currentLevel = new List<string>();
			for (int i = 0; i < LevelEditorController.LoadedCustomLevel.Count; i++)
			{
				currentLevel.Add(LevelEditorController.LoadedCustomLevel[i]);
			}
			break;
		default:
			throw new Exception("unknown game mode");
		}
	}

	private void AddSecretRoom()
	{
		if (GameplayCommons.Instance.effectsSpawner.WeatherSystemActive || UnityEngine.Random.value < 0.7f)
		{
			return;
		}
		for (int i = 0; i < currentLevel.Count; i++)
		{
			string value = currentLevel[i];
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("fffff");
			stringBuilder.Append(value);
			stringBuilder.Append("fffff");
			currentLevel[i] = stringBuilder.ToString();
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		for (int j = 0; j < currentLevel[0].Length; j++)
		{
			stringBuilder2.Append("f");
		}
		string item = stringBuilder2.ToString();
		for (int k = 0; k < 5; k++)
		{
			currentLevel.Add(item);
			currentLevel.Insert(0, item);
		}
		List<IntVector2> list = new List<IntVector2>();
		for (int l = 0; l < currentLevel.Count; l++)
		{
			string text = currentLevel[l];
			for (int m = 0; m < text.Length; m++)
			{
				if (text[m] == '9')
				{
					list.Add(new IntVector2(m, l));
				}
			}
		}
		list = Randomize(list);
		for (int n = 0; n < list.Count && !TryAddingSecretRoomAtPoint(list[n]); n++)
		{
		}
	}

	private bool TryAddingSecretRoomAtPoint(IntVector2 point)
	{
		List<IntVector2> list = new List<IntVector2>();
		List<IntVector2> list2 = new List<IntVector2>();
		if (currentLevel[point.y][point.x - 1] == '9' && currentLevel[point.y][point.x + 1] == '9')
		{
			if (currentLevel[point.y + 1][point.x] == '0')
			{
				list2.Add(new IntVector2(point.x, point.y - 3));
			}
			if (currentLevel[point.y - 1][point.x] == '0')
			{
				list2.Add(new IntVector2(point.x, point.y + 3));
			}
		}
		if (currentLevel[point.y - 1][point.x] == '9' && currentLevel[point.y + 1][point.x] == '9')
		{
			if (currentLevel[point.y][point.x + 1] == '0')
			{
				list2.Add(new IntVector2(point.x - 3, point.y));
			}
			if (currentLevel[point.y][point.x - 1] == '0')
			{
				list2.Add(new IntVector2(point.x + 3, point.y));
			}
		}
		foreach (IntVector2 item in list2)
		{
			if (CheckPointEligibleForSecretRoom(item))
			{
				list.Add(item);
			}
		}
		if (list.Count > 0)
		{
			SpawnSecretRoomAt(list[UnityEngine.Random.Range(0, list.Count)], point);
			return true;
		}
		return false;
	}

	private bool CheckPointEligibleForSecretRoom(IntVector2 point)
	{
		for (int i = point.x - 2; i < point.x + 3; i++)
		{
			for (int j = point.y - 2; j < point.y + 3; j++)
			{
				if (currentLevel[j][i] != 'f')
				{
					return false;
				}
			}
		}
		return true;
	}

	private void SpawnSecretRoomAt(IntVector2 roomCoords, IntVector2 entryPoint)
	{
		DebugHelper.Log("spawned secret room at " + roomCoords.ToString());
		secretRoomCoords = new List<IntVector2>();
		List<IntVector2> list = new List<IntVector2>();
		for (int i = roomCoords.y - 2; i < roomCoords.y + 3; i++)
		{
			StringBuilder stringBuilder = new StringBuilder(currentLevel[i]);
			for (int j = roomCoords.x - 2; j < roomCoords.x + 3; j++)
			{
				if (i == roomCoords.y - 2 || i == roomCoords.y + 2)
				{
					stringBuilder[j] = '9';
				}
				else if (j == roomCoords.x - 2 || j == roomCoords.x + 2)
				{
					stringBuilder[j] = '9';
				}
				else
				{
					stringBuilder[j] = '0';
					list.Add(new IntVector2(j, i));
				}
				secretRoomCoords.Add(new IntVector2(j, i));
			}
			currentLevel[i] = stringBuilder.ToString();
		}
		ReplaceLevelChar(entryPoint, 'g');
		if (roomCoords.x > entryPoint.x)
		{
			ReplaceLevelChar(new IntVector2(entryPoint.x + 1, entryPoint.y), '0');
			ReplaceLevelChar(new IntVector2(entryPoint.x + 1, entryPoint.y + 1), '0');
			ReplaceLevelChar(new IntVector2(entryPoint.x + 1, entryPoint.y - 1), '0');
		}
		else if (roomCoords.x < entryPoint.x)
		{
			ReplaceLevelChar(new IntVector2(entryPoint.x - 1, entryPoint.y), '0');
			ReplaceLevelChar(new IntVector2(entryPoint.x - 1, entryPoint.y + 1), '0');
			ReplaceLevelChar(new IntVector2(entryPoint.x - 1, entryPoint.y - 1), '0');
		}
		else if (roomCoords.y > entryPoint.y)
		{
			ReplaceLevelChar(new IntVector2(entryPoint.x, entryPoint.y + 1), '0');
			ReplaceLevelChar(new IntVector2(entryPoint.x + 1, entryPoint.y + 1), '0');
			ReplaceLevelChar(new IntVector2(entryPoint.x - 1, entryPoint.y + 1), '0');
		}
		else if (roomCoords.y < entryPoint.y)
		{
			ReplaceLevelChar(new IntVector2(entryPoint.x, entryPoint.y - 1), '0');
			ReplaceLevelChar(new IntVector2(entryPoint.x + 1, entryPoint.y - 1), '0');
			ReplaceLevelChar(new IntVector2(entryPoint.x - 1, entryPoint.y - 1), '0');
		}
		list = Randomize(list);
		int num = UnityEngine.Random.Range(3, 5);
		for (int k = 0; k < num; k++)
		{
			IntVector2 position = list[k];
			ReplaceLevelChar(position, 'H');
		}
		IntVector2 playerPosition = GetPlayerPosition();
		LevelUtils.CropLevel(currentLevel);
		IntVector2 playerPosition2 = GetPlayerPosition();
		IntVector2 intVector = new IntVector2(playerPosition2.x - playerPosition.x, playerPosition2.y - playerPosition.y);
		for (int l = 0; l < secretRoomCoords.Count; l++)
		{
			List<IntVector2> list2 = secretRoomCoords;
			int index = l;
			IntVector2 intVector2 = secretRoomCoords[l];
			int x = intVector2.x + intVector.x;
			IntVector2 intVector3 = secretRoomCoords[l];
			list2[index] = new IntVector2(x, intVector3.y + intVector.y);
		}
	}

	private IntVector2 GetPlayerPosition()
	{
		for (int i = 0; i < currentLevel.Count; i++)
		{
			for (int j = 0; j < currentLevel[0].Length; j++)
			{
				if (currentLevel[i][j] == '4')
				{
					return new IntVector2(j, i);
				}
			}
		}
		throw new Exception("cant find player in level builder");
	}

	private void ReplaceLevelChar(IntVector2 position, char value)
	{
		StringBuilder stringBuilder = new StringBuilder(currentLevel[position.y]);
		stringBuilder[position.x] = value;
		currentLevel[position.y] = stringBuilder.ToString();
	}

	public static List<T> Randomize<T>(List<T> list)
	{
		List<T> list2 = new List<T>();
		System.Random random = new System.Random();
		while (list.Count > 0)
		{
			int index = random.Next(0, list.Count);
			list2.Add(list[index]);
			list.RemoveAt(index);
		}
		return list2;
	}
}
