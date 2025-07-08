using System;
using UnityEngine;

public class DestructableObstacleController : MonoBehaviour
{
	private float hitPoints;

	private float hitPointsMax;

	private GameObject hpBar;

	private float hpBarInitialScaleX;

	private float targetHPBarScaleX;

	private float hpBarScaleSpeed = 3f;

	private SpriteRenderer hpsr;

	private GameObject hpBarUnd;

	private SpriteRenderer hpusr;

	private float lastTimeShownHP = -100f;

	private float hpDisplayTime = 2f;

	private int damagedTimeout;

	private bool doNotShowHpBar;

	public DestructableObstacleTypes destructableObstacleType;

	private bool breaksOnLevelComplete;

	private bool isSpecialPrizeCrate;

	private float levelCompleteBreakTimeout;

	private float explosiveBarrelHitTimestamp = -1f;

	private float explosiveBarrelExplodeTimeout;

	public Sprite ObstacleSprite;

	public Sprite ObstacleFlashSprite;

	private DestructableObstacleTypes[] obstaclesToSpawnWithEffect = new DestructableObstacleTypes[2]
	{
		DestructableObstacleTypes.bonusCrate,
		DestructableObstacleTypes.explosiveBarrel
	};

	private DestructableObstacleTypes[] movableObstacles = new DestructableObstacleTypes[2]
	{
		DestructableObstacleTypes.bonusCrate,
		DestructableObstacleTypes.explosiveBarrel
	};

	private int flashFrames;

	private int flashFramesMax = GlobalCommons.FlashFrameCount;

	private SpriteRenderer obstacleSpriteRenderer;

	private bool isMovableObstacle = true;

	private const float BARREL_EXPLOSION_TIMEOUT_MIN = 0.2f;

	private const float BARREL_EXPLOSION_TIMEOUT_MAX = 0.6f;

	private static float LastTimeExplosiveBarrelTriggered;

	private void Start()
	{
		obstacleSpriteRenderer = GetComponent<SpriteRenderer>();
		levelCompleteBreakTimeout = UnityEngine.Random.Range(0.2f, 0.8f);
		InitDestructableObstacle();
		InitializeHPBar();
		base.transform.parent = null;
		GameplayCommons.Instance.enemiesTracker.Track(this);
		if (Array.IndexOf(obstaclesToSpawnWithEffect, destructableObstacleType) != -1)
		{
			Vector3 position = Camera.main.transform.position;
			float x = position.x;
			Vector3 position2 = base.transform.position;
			if (Mathf.Abs(x - position2.x) < GlobalCommons.Instance.horizontalCameraBorderWithCompensation)
			{
				Vector3 position3 = Camera.main.transform.position;
				float y = position3.y;
				Vector3 position4 = base.transform.position;
				if (Mathf.Abs(y - position4.y) < GlobalCommons.Instance.verticalCameraBorderWithCompensation)
				{
					GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(base.transform.position);
				}
			}
		}
		if (destructableObstacleType == DestructableObstacleTypes.explosiveBarrel)
		{
			explosiveBarrelExplodeTimeout = UnityEngine.Random.Range(0.2f, 0.6f);
		}
		if (destructableObstacleType == DestructableObstacleTypes.crackedIndestructibleWall)
		{
			doNotShowHpBar = true;
		}
	}

	public void ProcessAfterFlashSetup()
	{
		if (Array.IndexOf(movableObstacles, destructableObstacleType) == -1)
		{
			isMovableObstacle = false;
			obstacleSpriteRenderer.sprite = ObstacleFlashSprite;
			obstacleSpriteRenderer.enabled = false;
		}
	}

	public bool IsWallSegment()
	{
		if (destructableObstacleType == DestructableObstacleTypes.brickWall || destructableObstacleType == DestructableObstacleTypes.harderBrickWall || destructableObstacleType == DestructableObstacleTypes.specialWall || destructableObstacleType == DestructableObstacleTypes.crackedIndestructibleWall || destructableObstacleType == DestructableObstacleTypes.cornerBrickWall)
		{
			return true;
		}
		return false;
	}

	private void InitDestructableObstacle()
	{
		switch (destructableObstacleType)
		{
		case DestructableObstacleTypes.bonusCrate:
			hitPointsMax = GlobalBalance.GetCrateHP();
			breaksOnLevelComplete = true;
			break;
		case DestructableObstacleTypes.explosiveBarrel:
			hitPointsMax = GlobalBalance.GetExplBarrelHP();
			break;
		case DestructableObstacleTypes.brickWall:
			hitPointsMax = GlobalBalance.GetWallHP();
			break;
		case DestructableObstacleTypes.cornerBrickWall:
			hitPointsMax = GlobalBalance.GetWallHP() / 2f;
			break;
		case DestructableObstacleTypes.harderBrickWall:
			hitPointsMax = GlobalBalance.GetHarderWallHP();
			break;
		case DestructableObstacleTypes.crackedIndestructibleWall:
			hitPointsMax = GlobalBalance.GetCrackedIndestrWallHP();
			break;
		case DestructableObstacleTypes.specialWall:
			hitPointsMax = GlobalBalance.GetSpecWallHP();
			break;
		}
		hitPoints = hitPointsMax;
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
		if (doNotShowHpBar)
		{
			return;
		}
		if (Time.fixedTime - lastTimeShownHP < hpDisplayTime && hitPoints > 0f)
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
			hpBar.transform.rotation = Quaternion.identity;
			hpBarUnd.transform.position = new Vector3(v.x - 0.05f, v.y);
			hpBarUnd.transform.rotation = Quaternion.identity;
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

	private void Update()
	{
		UpdateHpBar();
		if (explosiveBarrelHitTimestamp > 0f && Time.fixedTime - explosiveBarrelHitTimestamp >= explosiveBarrelExplodeTimeout)
		{
			GameplayCommons.Instance.levelStateController.ComboController.Kick();
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.BarrelsExploded);
			GameplayCommons.Instance.cameraController.ShakeCamera(6f);
			SoundManager.instance.PlaySpawnerExplosionSound();
			ExplosionProcessor.ProcessExplosion(ExplosionProcessor.ExplosionTrigger.neutral, base.transform.position, GlobalBalance.GetExplBarrelDamage(), 5f, null, 2.2f, DamageTypes.explosiveBarrelDamage, null, playSound: false);
			GameplayCommons.Instance.effectsSpawner.SpawnExplosionDecal(base.transform.position);
			GameplayCommons.Instance.effectsSpawner.CreateTankExplodedEffect(base.transform.position);
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerExplodedEffect(base.transform.position);
            WaveExploPostProcessing.ShowEffectAt(Camera.main.WorldToScreenPoint(base.transform.position));
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			if (!isSpecialPrizeCrate && breaksOnLevelComplete && GameplayCommons.Instance.levelStateController.LevelCompletionPending && Time.fixedTime - GameplayCommons.Instance.levelStateController.LevelCompletedTimestamp >= levelCompleteBreakTimeout && IsGameplayModeEligibleForBreakOnLevelComplete())
			{
				ApplyDamage(100500f, DamageTypes.levelCompletePopDamage);
			}
			if (flashFrames > 0)
			{
				ProcessObstacleFlash();
			}
		}
	}

	private bool IsGameplayModeEligibleForBreakOnLevelComplete()
	{
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.EditorLevel || GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.CustomLevel)
		{
			return false;
		}
		return true;
	}

	private void ProcessObstacleFlash()
	{
		flashFrames--;
		if (flashFrames > 0)
		{
			if (flashFrames == flashFramesMax - 1)
			{
				if (isMovableObstacle)
				{
					obstacleSpriteRenderer.sprite = ObstacleFlashSprite;
				}
				else
				{
					obstacleSpriteRenderer.enabled = true;
				}
			}
		}
		else if (isMovableObstacle)
		{
			obstacleSpriteRenderer.sprite = ObstacleSprite;
		}
		else
		{
			obstacleSpriteRenderer.enabled = false;
		}
	}

	public void ApplyDamage(float amount, DamageTypes damageType)
	{
		if (hitPoints <= 0f)
		{
			return;
		}
		if (GameplayCommons.Instance.levelStateController.IsDoubleDamageActive)
		{
			amount *= 2f;
		}
		flashFrames = flashFramesMax;
		if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel && !GameplayCommons.Instance.tutorialController.CheckDamageAllowed(destructableObstacleType))
		{
			return;
		}
		hitPoints -= amount;
		if (hitPoints < 0f)
		{
			hitPoints = 0f;
		}
		damagedTimeout = 10;
		lastTimeShownHP = Time.fixedTime;
		float num = hitPoints / hitPointsMax;
		if (num > 0f && num < 0.05f)
		{
			num = 0.05f;
		}
		targetHPBarScaleX = num * hpBarInitialScaleX;
		Vector3 localScale = hpBar.transform.localScale;
		hpBarScaleSpeed = Mathf.Abs(localScale.x - targetHPBarScaleX) / 10f;
		switch (destructableObstacleType)
		{
		case DestructableObstacleTypes.explosiveBarrel:
			SoundManager.instance.PlayBarrelHitSound(damageType);
			break;
		case DestructableObstacleTypes.bonusCrate:
			SoundManager.instance.PlayCrateHitSound(damageType);
			break;
		}
		if (hitPoints != 0f)
		{
			return;
		}
		GameplayCommons.Instance.enemiesTracker.UnTrack(this);
		bool flag = true;
		Vector3 position = base.transform.position;
		float x = position.x / GlobalCommons.Instance.gridSize;
		float num2 = GameplayCommons.Instance.currentLevel.Count - 1;
		Vector3 position2 = base.transform.position;
		Vector3 v = new Vector2(x, Mathf.RoundToInt(num2 - position2.y / GlobalCommons.Instance.gridSize));
		switch (destructableObstacleType)
		{
		case DestructableObstacleTypes.bonusCrate:
			GameplayCommons.Instance.effectsSpawner.SpawnCrateShards(base.transform.position);
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(base.transform.position, 2, 0.3f);
			GameplayCommons.Instance.levelStateController.ComboController.Kick();
			if (damageType != DamageTypes.levelCompletePopDamage)
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.BonusCratesPopped);
			}
			SoundManager.instance.PlayCrateCrashSound();
			GameplayCommons.Instance.cameraController.ShakeCamera();
			if (isSpecialPrizeCrate)
			{
				BonusesSpawner.SpawnCoinsBonus(base.transform.position, MoneyLootCounter.GetSpecialCrateCoinsLoot(), increaseCoinCount: true);
			}
			else
			{
				BonusesSpawner.SpawnRandomBonus(base.transform.position);
			}
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(base.transform.position);
			if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel)
			{
				GameplayCommons.Instance.tutorialController.HideCrateTutorialMessages();
			}
			break;
		case DestructableObstacleTypes.explosiveBarrel:
			if (Time.fixedTime - LastTimeExplosiveBarrelTriggered > 1.2f)
			{
				explosiveBarrelHitTimestamp = Time.fixedTime - 0.6f;
			}
			else
			{
				explosiveBarrelHitTimestamp = Time.fixedTime;
			}
			LastTimeExplosiveBarrelTriggered = Time.fixedTime;
			flag = false;
			if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel)
			{
				GameplayCommons.Instance.tutorialController.HideBarrelTutorialMessage();
			}
			break;
		case DestructableObstacleTypes.brickWall:
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(base.transform.position, 2, 0.3f);
			GameplayCommons.Instance.effectsSpawner.CreateDarkSmokeEffect(base.transform.position, 3, 0.3f);
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.WallsDestroyed);
			SoundManager.instance.PlayBrickSound();
			GameplayCommons.Instance.effectsSpawner.SpawnWallShards(base.transform.position);
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(base.transform.position);
			GameplayCommons.Instance.cameraController.ShakeCamera(3f);
			GameplayCommons.Instance.tileMap.ChangeLevelItemAndUV(v, TileMap.TileType.grass);
			break;
		case DestructableObstacleTypes.cornerBrickWall:
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(base.transform.position, 1, 0.3f);
			GameplayCommons.Instance.effectsSpawner.CreateDarkSmokeEffect(base.transform.position, 2, 0.3f);
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.WallsDestroyed);
			SoundManager.instance.PlayBrickSound();
			GameplayCommons.Instance.effectsSpawner.SpawnWallShards(base.transform.position);
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(base.transform.position);
			GameplayCommons.Instance.cameraController.ShakeCamera(3f);
			GameplayCommons.Instance.tileMap.ChangeLevelItemAndUV(v, TileMap.TileType.grass);
			break;
		case DestructableObstacleTypes.crackedIndestructibleWall:
			GameplayCommons.Instance.visibilityController.ReinitVisibilityCoversGraphics();
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(base.transform.position, 2, 0.3f);
			GameplayCommons.Instance.effectsSpawner.CreateDarkSmokeEffect(base.transform.position, 3, 0.3f);
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.WallsDestroyed);
			SoundManager.instance.PlayBrickSound();
			GameplayCommons.Instance.effectsSpawner.SpawnSpecialWallShards(base.transform.position);
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(base.transform.position);
			GameplayCommons.Instance.cameraController.ShakeCamera(3f);
			GameplayCommons.Instance.tileMap.ChangeLevelItemAndUV(v, TileMap.TileType.grass);
			break;
		case DestructableObstacleTypes.harderBrickWall:
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(base.transform.position, 2, 0.3f);
			GameplayCommons.Instance.effectsSpawner.CreateDarkSmokeEffect(base.transform.position, 3, 0.3f);
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.WallsDestroyed);
			SoundManager.instance.PlayBrickSound();
			GameplayCommons.Instance.effectsSpawner.SpawnHarderWallShards(base.transform.position);
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(base.transform.position);
			GameplayCommons.Instance.cameraController.ShakeCamera(3f);
			GameplayCommons.Instance.tileMap.ChangeLevelItemAndUV(v, TileMap.TileType.grass);
			break;
		case DestructableObstacleTypes.specialWall:
		{
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(base.transform.position, 2, 0.3f);
			GameplayCommons.Instance.effectsSpawner.CreateDarkSmokeEffect(base.transform.position, 3, 0.3f);
			GameplayCommons.Instance.cameraController.ShakeCamera(3f);
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.WallsDestroyed);
			SoundManager.instance.PlayBrickSound();
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(base.transform.position);
			GameplayCommons.Instance.effectsSpawner.SpawnSpecialWallShards(base.transform.position);
			Vector3[] array = new Vector3[4];
			ref Vector3 reference = ref array[0];
			Vector3 position3 = base.transform.position;
			float x2 = position3.x + GlobalCommons.Instance.gridSize;
			Vector3 position4 = base.transform.position;
			float y = position4.y;
			Vector3 position5 = base.transform.position;
			reference = new Vector3(x2, y, position5.z);
			ref Vector3 reference2 = ref array[1];
			Vector3 position6 = base.transform.position;
			float x3 = position6.x - GlobalCommons.Instance.gridSize;
			Vector3 position7 = base.transform.position;
			float y2 = position7.y;
			Vector3 position8 = base.transform.position;
			reference2 = new Vector3(x3, y2, position8.z);
			ref Vector3 reference3 = ref array[2];
			Vector3 position9 = base.transform.position;
			float x4 = position9.x;
			Vector3 position10 = base.transform.position;
			float y3 = position10.y + GlobalCommons.Instance.gridSize;
			Vector3 position11 = base.transform.position;
			reference3 = new Vector3(x4, y3, position11.z);
			ref Vector3 reference4 = ref array[3];
			Vector3 position12 = base.transform.position;
			float x5 = position12.x;
			Vector3 position13 = base.transform.position;
			float y4 = position13.y - GlobalCommons.Instance.gridSize;
			Vector3 position14 = base.transform.position;
			reference4 = new Vector3(x5, y4, position14.z);
			Vector3[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Vector3 vector = array2[i];
				Collider2D[] array3 = Physics2D.OverlapPointAll(new Vector2(vector.x, vector.y), LayerMasks.onlyDestructableObstaclesLayerMask);
				foreach (Collider2D collider2D in array3)
				{
					DestructableObstacleController component = collider2D.GetComponent<DestructableObstacleController>();
					if (component.destructableObstacleType == DestructableObstacleTypes.specialWall)
					{
						component.ApplyDamage(100500f, DamageTypes.whatnotDamage);
					}
				}
			}
			if (GlobalCommons.Instance.gameplayMode == GlobalCommons.GameplayModes.TutorialLevel)
			{
				GameplayCommons.Instance.tutorialController.IncreaseSpecialGatesCrashed();
			}
			GameplayCommons.Instance.tileMap.ChangeLevelItemAndUV(v, TileMap.TileType.grass);
			break;
		}
		}
		if (flag)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	internal void InitSpecialPrizeCrate()
	{
		isSpecialPrizeCrate = true;
	}
}
