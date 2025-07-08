using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
	private float lastTimeSpawned;

	private float spawnInterval;

	private int maxActiveEnemies = 4;

	private float hitPoints;

	private float hitPointsMax;

	private GameObject hpBar;

	private float hpBarInitialScaleX;

	private float targetHPBarScaleX;

	private float hpBarScaleSpeed = 3f;

	private float lastTimeShownHP = -100f;

	private float hpDisplayTime = 2f;

	private int damagedTimeout;

	private SpriteRenderer hpsr;

	private GameObject hpBarUnd;

	private SpriteRenderer hpusr;

	private List<EnemyTankController> spawnedTanks;

	private GameObject freezeIce;

	private SpriteRenderer freezeIceSpriteRenderer;

	private float lastTimeToldOthers;

	private float currentTellOthersInterval;

	private float tellOthersIntervalMin = 0.5f;

	private float tellOthersIntervalMax = 1f;

	private float tellOthersDistance = 9f;

	private SpriteRenderer spawnerSprite;

	private int spawnerLevel = 1;

	private bool processedInitialSpawnCheck;

	private List<WeaponTypes> enemiesToSpawn;

	private SpriteRenderer itemsIndicatorSR;

	public Sprite[] SpawnerSprites;

	public Sprite[] DamagedSpawnerSprites;

	public Sprite[] SpawnerItemsIndicatorsSprites;

	public Sprite FullTileFlash;

	private int flashFrames;

	private int flashFramesMax = GlobalCommons.FlashFrameCount;

	private int currentFrame;

	private int initialSpawnCheckFrame;

	private bool upperVisibilityCoverInitialized;

	private GameObject upperVisibilityCover;

	private bool sawLevelMode;

	private void Start()
	{
		initialSpawnCheckFrame = UnityEngine.Random.Range(2, 10);
		lastTimeSpawned = 0f;
		spawnInterval = 2.9f + UnityEngine.Random.Range(0f, 0.2f);
		spawnedTanks = new List<EnemyTankController>();
		InitHP();
		GameplayCommons.Instance.enemiesTracker.Track(this);
		InitializeHPBar();
		freezeIce = UnityEngine.Object.Instantiate(Prefabs.enemyFreezePrefab);
		freezeIceSpriteRenderer = freezeIce.GetComponent<SpriteRenderer>();
		freezeIceSpriteRenderer.enabled = false;
		freezeIce.transform.parent = base.transform;
		InitializeGraphics();
		Vector3 position = base.transform.position;
		float x = position.x + 0.19f;
		Vector3 position2 = base.transform.position;
		float y = position2.y - 0.46f;
		Vector3 position3 = base.transform.position;
		Vector3 position4 = new Vector3(x, y, position3.z);
		itemsIndicatorSR = UnityEngine.Object.Instantiate(Prefabs.spawnerItemsIndicatorPrefab, position4, Quaternion.identity).GetComponent<SpriteRenderer>();
		itemsIndicatorSR.transform.parent = base.transform;
		UpdateItemsIndicator();
	}

	private void UpdateItemsIndicator()
	{
		int num = enemiesToSpawn.Count;
		if (num >= SpawnerItemsIndicatorsSprites.Length)
		{
			num = SpawnerItemsIndicatorsSprites.Length - 1;
		}
		itemsIndicatorSR.sprite = SpawnerItemsIndicatorsSprites[num];
	}

	public void InitializeSpawner(int level)
	{
		spawnerLevel = level;
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.RegularLevel && level > GlobalCommons.Instance.globalGameStats.EditorItemsLevel)
		{
			GlobalCommons.Instance.globalGameStats.EditorItemsLevel = level;
			LevelSelectionMenuController.ShowNewEditorItemsAvailableMessage = true;
		}
		switch (level)
		{
		case 1:
			if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.RegularLevel && GlobalCommons.Instance.ActualSelectedLevel == 1)
			{
				enemiesToSpawn = new List<WeaponTypes>
				{
					WeaponTypes.machinegun,
					WeaponTypes.machinegun,
					WeaponTypes.shotgun
				};
			}
			else if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.RegularLevel && GlobalCommons.Instance.ActualSelectedLevel == 2)
			{
				enemiesToSpawn = new List<WeaponTypes>
				{
					WeaponTypes.machinegun,
					WeaponTypes.machinegun,
					WeaponTypes.machinegun,
					WeaponTypes.shotgun
				};
			}
			else if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.RegularLevel && GlobalCommons.Instance.ActualSelectedLevel == 3)
			{
				enemiesToSpawn = new List<WeaponTypes>
				{
					WeaponTypes.machinegun,
					WeaponTypes.machinegun,
					WeaponTypes.machinegun,
					WeaponTypes.shotgun
				};
			}
			else if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.RegularLevel && GlobalCommons.Instance.ActualSelectedLevel == 4)
			{
				enemiesToSpawn = new List<WeaponTypes>
				{
					WeaponTypes.machinegun,
					WeaponTypes.machinegun,
					WeaponTypes.shotgun,
					WeaponTypes.shotgun
				};
			}
			else if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.RegularLevel && GlobalCommons.Instance.ActualSelectedLevel == 5)
			{
				enemiesToSpawn = new List<WeaponTypes>
				{
					WeaponTypes.machinegun,
					WeaponTypes.machinegun,
					WeaponTypes.shotgun,
					WeaponTypes.shotgun,
					WeaponTypes.minigun
				};
			}
			else if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel)
			{
				enemiesToSpawn = new List<WeaponTypes>
				{
					WeaponTypes.machinegun,
					WeaponTypes.machinegun
				};
			}
			else
			{
				enemiesToSpawn = new List<WeaponTypes>
				{
					WeaponTypes.machinegun,
					WeaponTypes.machinegun,
					WeaponTypes.machinegun,
					WeaponTypes.shotgun,
					WeaponTypes.shotgun,
					WeaponTypes.minigun
				};
			}
			break;
		case 2:
			enemiesToSpawn = new List<WeaponTypes>
			{
				WeaponTypes.machinegun,
				WeaponTypes.machinegun,
				WeaponTypes.shotgun,
				WeaponTypes.shotgun,
				WeaponTypes.minigun,
				WeaponTypes.cannon
			};
			break;
		case 3:
			enemiesToSpawn = new List<WeaponTypes>
			{
				WeaponTypes.machinegun,
				WeaponTypes.shotgun,
				WeaponTypes.shotgun,
				WeaponTypes.minigun,
				WeaponTypes.cannon,
				WeaponTypes.homingRocket
			};
			break;
		case 4:
			enemiesToSpawn = new List<WeaponTypes>
			{
				WeaponTypes.shotgun,
				WeaponTypes.shotgun,
				WeaponTypes.minigun,
				WeaponTypes.cannon,
				WeaponTypes.homingRocket,
				WeaponTypes.suicide,
				WeaponTypes.suicide
			};
			break;
		case 5:
			enemiesToSpawn = new List<WeaponTypes>
			{
				WeaponTypes.shotgun,
				WeaponTypes.minigun,
				WeaponTypes.cannon,
				WeaponTypes.homingRocket,
				WeaponTypes.suicide,
				WeaponTypes.suicide,
				WeaponTypes.laser
			};
			break;
		case 6:
			enemiesToSpawn = new List<WeaponTypes>
			{
				WeaponTypes.minigun,
				WeaponTypes.cannon,
				WeaponTypes.homingRocket,
				WeaponTypes.suicide,
				WeaponTypes.suicide,
				WeaponTypes.laser,
				WeaponTypes.railgun
			};
			break;
		case 7:
			enemiesToSpawn = new List<WeaponTypes>
			{
				WeaponTypes.cannon,
				WeaponTypes.cannon,
				WeaponTypes.homingRocket,
				WeaponTypes.suicide,
				WeaponTypes.laser,
				WeaponTypes.laser,
				WeaponTypes.railgun
			};
			break;
		case 8:
			enemiesToSpawn = new List<WeaponTypes>
			{
				WeaponTypes.cannon,
				WeaponTypes.cannon,
				WeaponTypes.homingRocket,
				WeaponTypes.suicide,
				WeaponTypes.laser,
				WeaponTypes.railgun,
				WeaponTypes.ricochet
			};
			break;
		case 9:
			enemiesToSpawn = new List<WeaponTypes>
			{
				WeaponTypes.cannon,
				WeaponTypes.homingRocket,
				WeaponTypes.suicide,
				WeaponTypes.laser,
				WeaponTypes.railgun,
				WeaponTypes.ricochet,
				WeaponTypes.triple
			};
			break;
		case 10:
			enemiesToSpawn = new List<WeaponTypes>
			{
				WeaponTypes.homingRocket,
				WeaponTypes.suicide,
				WeaponTypes.laser,
				WeaponTypes.railgun,
				WeaponTypes.ricochet,
				WeaponTypes.triple,
				WeaponTypes.shocker
			};
			break;
		}
		int num = 15;
		if (GlobalCommons.Instance.gameplayMode != 0 || GlobalCommons.Instance.ActualSelectedLevel < num || GlobalCommons.Instance.globalGameStats.AvailableWeaponsCount <= 1)
		{
			return;
		}
		int num2 = GlobalCommons.Instance.ActualSelectedLevel - num;
		if (num2 % 5 == 0)
		{
			DebugHelper.Log("saw level!!!!!!!!!!");
			sawLevelMode = true;
			enemiesToSpawn.Add(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)]);
			if (UnityEngine.Random.value > 0.5f)
			{
				enemiesToSpawn.Add(enemiesToSpawn[Random.Range(0, enemiesToSpawn.Count)]);
			}
		}
	}

	private void CheckInitialSpawnNecessary()
	{
		Vector2[] array = new Vector2[8];
		ref Vector2 reference = ref array[0];
		Vector3 position = base.transform.position;
		float x = position.x + GlobalCommons.Instance.gridSize;
		Vector3 position2 = base.transform.position;
		reference = new Vector2(x, position2.y + GlobalCommons.Instance.gridSize);
		ref Vector2 reference2 = ref array[1];
		Vector3 position3 = base.transform.position;
		float x2 = position3.x + GlobalCommons.Instance.gridSize;
		Vector3 position4 = base.transform.position;
		reference2 = new Vector2(x2, position4.y);
		ref Vector2 reference3 = ref array[2];
		Vector3 position5 = base.transform.position;
		float x3 = position5.x + GlobalCommons.Instance.gridSize;
		Vector3 position6 = base.transform.position;
		reference3 = new Vector2(x3, position6.y - GlobalCommons.Instance.gridSize);
		ref Vector2 reference4 = ref array[3];
		Vector3 position7 = base.transform.position;
		float x4 = position7.x;
		Vector3 position8 = base.transform.position;
		reference4 = new Vector2(x4, position8.y + GlobalCommons.Instance.gridSize);
		ref Vector2 reference5 = ref array[4];
		Vector3 position9 = base.transform.position;
		float x5 = position9.x;
		Vector3 position10 = base.transform.position;
		reference5 = new Vector2(x5, position10.y - GlobalCommons.Instance.gridSize);
		ref Vector2 reference6 = ref array[5];
		Vector3 position11 = base.transform.position;
		float x6 = position11.x - GlobalCommons.Instance.gridSize;
		Vector3 position12 = base.transform.position;
		reference6 = new Vector2(x6, position12.y + GlobalCommons.Instance.gridSize);
		ref Vector2 reference7 = ref array[6];
		Vector3 position13 = base.transform.position;
		float x7 = position13.x - GlobalCommons.Instance.gridSize;
		Vector3 position14 = base.transform.position;
		reference7 = new Vector2(x7, position14.y);
		ref Vector2 reference8 = ref array[7];
		Vector3 position15 = base.transform.position;
		float x8 = position15.x - GlobalCommons.Instance.gridSize;
		Vector3 position16 = base.transform.position;
		reference8 = new Vector2(x8, position16.y - GlobalCommons.Instance.gridSize);
		Vector2[] array2 = array;
		List<Vector2> list = new List<Vector2>();
		float radius = GlobalCommons.Instance.gridSize / 10f;
		for (int i = 0; i < array2.Length; i++)
		{
			Vector2 item = array2[i];
			Collider2D[] array3 = Physics2D.OverlapCircleAll(new Vector2(item.x, item.y), radius, LayerMasks.obstaclesEnemiesSpawnersDestrObstaclesLayerMask);
			if (array3.Length == 0)
			{
				list.Add(item);
			}
		}
		int num = 3 - spawnedTanks.Count;
		if (num > enemiesToSpawn.Count)
		{
			num = enemiesToSpawn.Count;
		}
		if (num > 0 && !GameplayCommons.Instance.spawnedEnemyThisFrame)
		{
			if (list.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				Vector2 vector = list[index];
				list.RemoveAt(index);
				float x9 = vector.x;
				float y = vector.y;
				Vector3 position17 = base.transform.position;
				Vector3 value = new Vector3(x9, y, position17.z);
				SpawnEnemy(value);
			}
			num--;
		}
		if (num == 0)
		{
			processedInitialSpawnCheck = true;
		}
	}

	private void InitializeGraphics()
	{
		Animator component = GetComponent<Animator>();
		spawnerSprite = GetComponent<SpriteRenderer>();
		SetSpawnerSprite();
	}

	private void SetSpawnerSprite()
	{
		if (flashFrames <= 0)
		{
			if (hitPoints < hitPointsMax / 2f)
			{
				spawnerSprite.sprite = DamagedSpawnerSprites[spawnerLevel - 1];
			}
			else
			{
				spawnerSprite.sprite = SpawnerSprites[spawnerLevel - 1];
			}
		}
	}

	private void InitializeHPBar()
	{
		hpBar = UnityEngine.Object.Instantiate(Prefabs.enemyHPBarPrefab);
		hpBar.transform.parent = base.transform;
		hpsr = hpBar.GetComponent<SpriteRenderer>();
		hpsr.enabled = false;
		hpBarUnd = UnityEngine.Object.Instantiate(Prefabs.enemyHPBarUnderlyingPrefab);
		hpBarUnd.transform.parent = base.transform;
		hpusr = hpBarUnd.GetComponent<SpriteRenderer>();
		hpusr.enabled = false;
		Vector3 localScale = hpBar.transform.localScale;
		hpBarInitialScaleX = localScale.x;
		targetHPBarScaleX = hpBarInitialScaleX;
	}

	private void UpdateHpBar()
	{
		if (Time.fixedTime - lastTimeShownHP < hpDisplayTime)
		{
			if (!hpsr.enabled)
			{
				hpsr.enabled = true;
				hpusr.enabled = true;
			}
			Vector3 localScale = hpBar.transform.localScale;
			if (localScale.x != targetHPBarScaleX)
			{
				Transform transform = hpBar.transform;
				Vector3 localScale2 = hpBar.transform.localScale;
				float x = FMath.MoveFloatStepClamp(localScale2.x, targetHPBarScaleX, hpBarScaleSpeed);
				Vector3 localScale3 = hpBar.transform.localScale;
				float y = localScale3.y;
				Vector3 localScale4 = hpBar.transform.localScale;
				transform.localScale = new Vector3(x, y, localScale4.z);
			}
			Vector3 position = base.transform.position;
			float x2 = position.x - 0.4f;
			Vector3 position2 = base.transform.position;
			Vector2 v = new Vector2(x2, position2.y + 0.7f);
			if (damagedTimeout > 0)
			{
				damagedTimeout--;
				float num = (float)damagedTimeout * GlobalCommons.Instance.HPBarShakeFactor;
				float num2 = num;
				float num3 = num;
				if (UnityEngine.Random.value > 0.5f)
				{
					num2 *= -1f;
				}
				if (UnityEngine.Random.value > 0.5f)
				{
					num3 *= -1f;
				}
				v = new Vector2(v.x + num2, v.y + num3);
			}
			hpBar.transform.position = v;
			hpBarUnd.transform.position = new Vector3(v.x - 0.05f, v.y);
			float num4 = 2f - (Time.fixedTime - lastTimeShownHP);
			float a = 1f;
			if ((double)num4 < 0.5)
			{
				a = num4 / 0.5f;
			}
			SpriteRenderer spriteRenderer = hpsr;
			Color color = hpsr.color;
			float r = color.r;
			Color color2 = hpsr.color;
			float g = color2.g;
			Color color3 = hpsr.color;
			spriteRenderer.color = new Color(r, g, color3.b, a);
			SpriteRenderer spriteRenderer2 = hpusr;
			Color color4 = hpusr.color;
			float r2 = color4.r;
			Color color5 = hpusr.color;
			float g2 = color5.g;
			Color color6 = hpusr.color;
			spriteRenderer2.color = new Color(r2, g2, color6.b, a);
		}
		else if (hpsr.enabled)
		{
			hpsr.enabled = false;
			hpusr.enabled = false;
		}
	}

	private void InitHP()
	{
		hitPointsMax = GlobalBalance.GetSpawnerHP();
		hitPoints = hitPointsMax;
	}

	public void ApplyDamage(float damage, DamageTypes type)
	{
		if (hitPoints <= 0f)
		{
			return;
		}
		if (GameplayCommons.Instance.levelStateController.IsDoubleDamageActive)
		{
			damage *= 2f;
		}
		SoundManager.instance.PlayEnemyHitSound(type);
		flashFrames = flashFramesMax;
		ProcessTellOthers();
		hitPoints -= damage;
		if (hitPoints < hitPointsMax / 2f && hitPoints + damage >= hitPointsMax / 2f && hitPoints > 0f)
		{
			GameplayCommons.Instance.cameraController.ShakeCamera(6f);
			SoundManager.instance.PlaySpawnerExplosionSound();
			GameplayCommons.Instance.effectsSpawner.CreateExplosionEffect(base.transform.position, 1.5f, playSound: false);
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(base.transform.position, 4, 0.6f);
			SetSpawnerSprite();
		}
		if (hitPoints < 0f)
		{
			hitPoints = 0f;
		}
		damagedTimeout = 10;
		lastTimeShownHP = Time.fixedTime;
		if (hitPoints > 0f)
		{
			float num = hitPoints / hitPointsMax;
			if (num < 0.05f)
			{
				num = 0.05f;
			}
			targetHPBarScaleX = num * hpBarInitialScaleX;
			Vector3 localScale = hpBar.transform.localScale;
			hpBarScaleSpeed = Mathf.Abs(localScale.x - targetHPBarScaleX) / 10f;
		}
		if (hitPoints == 0f)
		{
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.SpawnersDestroyed);
			UnityEngine.Object.Destroy(base.gameObject);
			GameplayCommons.Instance.levelStateController.ComboController.Kick();
			GameplayCommons.Instance.enemiesTracker.UnTrack(this);
			GameplayCommons.Instance.cameraController.ShakeCamera(12f);
			SoundManager.instance.PlaySpawnerExplosionSound();
			GameplayCommons.Instance.effectsSpawner.CreateExplosionEffect(base.transform.position, 2f, playSound: false);
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(base.transform.position, 5, 0.6f);
			GameplayCommons.Instance.effectsSpawner.SpawnExplosionDecal(base.transform.position);
			WaveExploPostProcessing.ShowEffectAt(Camera.main.WorldToScreenPoint(base.transform.position));
			BonusesSpawner.SpawnCoinsBonus(base.transform.position, MoneyLootCounter.GetSpawnerMoneyLoot());
			for (int i = 0; i < enemiesToSpawn.Count; i++)
			{
				BonusesSpawner.SpawnCoinsBonus(base.transform.position, MoneyLootCounter.GetEnemyMoneyLoot());
			}
			GameplayCommons.Instance.gameplayUIController.FlashScreen();
			GameplayCommons.Instance.effectsSpawner.CreateTankExplodedEffect(base.transform.position);
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerExplodedEffect(base.transform.position);
			if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel)
			{
				GameplayCommons.Instance.tutorialController.HideDestoySpawnerTutorial();
			}
		}
	}

	private void ReinitTellOthers()
	{
		lastTimeToldOthers = Time.fixedTime;
		currentTellOthersInterval = UnityEngine.Random.Range(tellOthersIntervalMin, tellOthersIntervalMax);
	}

	private void ProcessTellOthers()
	{
		if (Time.fixedTime - lastTimeToldOthers > currentTellOthersInterval)
		{
			ReinitTellOthers();
			List<EnemyTankController> allEnemies = GameplayCommons.Instance.enemiesTracker.AllEnemies;
			foreach (EnemyTankController item in allEnemies)
			{
				Vector2 direction = item.TankBase.transform.position - base.transform.position;
				float magnitude = direction.magnitude;
				if (magnitude <= tellOthersDistance && Physics2D.Raycast(base.transform.position, direction, magnitude, LayerMasks.allObstacleTypesLayerMask).collider == null)
				{
					item.Alert();
				}
			}
		}
	}

	private void Update()
	{
		if (!processedInitialSpawnCheck)
		{
			if (currentFrame < initialSpawnCheckFrame)
			{
				currentFrame++;
			}
			else
			{
				CheckInitialSpawnNecessary();
			}
		}
		UpdateHpBar();
		if (flashFrames > 0)
		{
			ProcessObstacleFlash();
		}
		if (GameplayCommons.Instance.levelStateController.IsFreezeActive)
		{
			if (!freezeIceSpriteRenderer.enabled)
			{
				freezeIceSpriteRenderer.enabled = true;
			}
			freezeIce.transform.position = base.transform.position;
			lastTimeSpawned += Time.deltaTime;
			return;
		}
		if (freezeIceSpriteRenderer.enabled)
		{
			freezeIceSpriteRenderer.enabled = false;
		}
		for (int num = spawnedTanks.Count - 1; num >= 0; num--)
		{
			EnemyTankController x = spawnedTanks[num];
			if (x == null)
			{
				if (spawnedTanks.Count == maxActiveEnemies && lastTimeSpawned + spawnInterval <= Time.fixedTime)
				{
					lastTimeSpawned = Time.fixedTime + UnityEngine.Random.Range(0.33f, 0.66f);
				}
				spawnedTanks.RemoveAt(num);
			}
		}
		if (enemiesToSpawn.Count > 0 && !GameplayCommons.Instance.playersTankController.PlayerDead)
		{
			ProcessEnemySpawn();
		}
	}

	private void ProcessEnemySpawn()
	{
		if (GameplayCommons.Instance.spawnedEnemyThisFrame || !(Time.fixedTime - lastTimeSpawned >= spawnInterval) || spawnedTanks.Count >= maxActiveEnemies)
		{
			return;
		}
		List<EnemyTankController> allEnemies = GameplayCommons.Instance.enemiesTracker.AllEnemies;
		bool flag = true;
		for (int i = 0; i < allEnemies.Count; i++)
		{
			EnemyTankController enemyTankController = allEnemies[i];
			Vector3 position = enemyTankController.TankBase.transform.position;
			float x = position.x;
			Vector3 position2 = base.transform.position;
			if (Mathf.Abs(x - position2.x) < 1f)
			{
				Vector3 position3 = enemyTankController.TankBase.transform.position;
				float y = position3.y;
				Vector3 position4 = base.transform.position;
				if (Mathf.Abs(y - position4.y) < 1f)
				{
					flag = false;
					break;
				}
			}
		}
		if (flag)
		{
			SpawnEnemy();
			Vector2 vector;
			if (GameplayCommons.Instance.weaponsController.ActiveGuidedRocket != null)
			{
				Vector3 position5 = GameplayCommons.Instance.weaponsController.ActiveGuidedRocket.transform.position;
				float x2 = position5.x;
				Vector3 position6 = GameplayCommons.Instance.weaponsController.ActiveGuidedRocket.transform.position;
				vector = new Vector2(x2, position6.y);
			}
			else
			{
				Vector3 position7 = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
				float x3 = position7.x;
				Vector3 position8 = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
				vector = new Vector2(x3, position8.y);
			}
			Vector2 vector2 = vector;
			if (!upperVisibilityCoverInitialized)
			{
				InitializeUpperVisibilityCover();
			}
			Vector3 position9 = base.transform.position;
			if (Mathf.Abs(position9.x - vector2.x) < GlobalCommons.Instance.DynamicHorizontalCameraBorder)
			{
				Vector3 position10 = base.transform.position;
				if (Mathf.Abs(position10.y - vector2.y) < GlobalCommons.Instance.DynamicVerticalCameraBorder && upperVisibilityCover == null)
				{
					SoundManager.instance.PlayEnemySpawnSound();
				}
			}
		}
		else
		{
			lastTimeSpawned += 0.2f;
		}
	}

	private void InitializeUpperVisibilityCover()
	{
		upperVisibilityCoverInitialized = true;
		upperVisibilityCover = GameplayCommons.Instance.visibilityController.FindCoverAtPoint(base.transform.position);
	}

	private void ProcessObstacleFlash()
	{
		flashFrames--;
		if (flashFrames > 0)
		{
			if (flashFrames == flashFramesMax - 1)
			{
				spawnerSprite.sprite = FullTileFlash;
			}
		}
		else
		{
			SetSpawnerSprite();
		}
	}

	private void SpawnEnemy(Vector3? specSpawnPosition = default(Vector3?))
	{
		GameplayCommons.Instance.spawnedEnemyThisFrame = true;
		Vector3 vector = base.transform.position;
		if (specSpawnPosition.HasValue)
		{
			vector = specSpawnPosition.Value;
		}
		int index = UnityEngine.Random.Range(0, enemiesToSpawn.Count);
		WeaponTypes weaponType = enemiesToSpawn[index];
		enemiesToSpawn.RemoveAt(index);
		UpdateItemsIndicator();
		EnemyTankController component = UnityEngine.Object.Instantiate(Prefabs.enemyTankPrefab, vector, Quaternion.identity).GetComponent<EnemyTankController>();
		component.InitializeEnemy(EnemyTankController.EnemyTypes.tank, weaponType, isBoss: false);
		if (!specSpawnPosition.HasValue)
		{
			component.RollOut();
		}
		spawnedTanks.Add(component);
		lastTimeSpawned = Time.fixedTime;
		if (!specSpawnPosition.HasValue)
		{
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(vector);
		}
	}
}
