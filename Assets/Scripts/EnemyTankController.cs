using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTankController : MonoBehaviour
{
	private enum EnemyState
	{
		spawning,
		idling,
		moving,
		chasingVisiblePlayer,
		chasingInvisiblePlayer,
		lostPlayer
	}

	public enum EnemyTypes
	{
		tank,
		turret
	}

	private enum StrafeDirection
	{
		Clockwise,
		CounterClockwise,
		DoNotStrafe
	}

	private EnemyState currentState;

	private float idleStarted;

	private float idleTimeout;

	private float idleTimeoutMIN = 1f;

	private float idleTimeoutMAX = 4f;

	private float lastTimeShownHP = -100f;

	private float hpDisplayTime = 2f;

	private int damagedTimeout;

	private SpriteRenderer hpsr;

	private GameObject hpBarUnd;

	private SpriteRenderer hpusr;

	private float turretRotationFactorNormal = 2f;

	private float turretRotationFactorHurry = 10f;

	private float lastTimeTurretRotated;

	private float turretRotationTimeout;

	private float turretRotationTimeoutMINNormal = 1.2f;

	private float turretRotationTimeoutMAXNormal = 2.5f;

	private float turretRotationTimeoutMINHurry = 0.2f;

	private float turretRotationTimeoutMAXHurry = 0.6f;

	private Vector2 turretTurnDirection;

	private float lastTimeSetTurretForcedRotationAngleMod;

	private float currentTurretForcedRotationAngleMod;

	private float chasingInvisiblePlayerTimestamp;

	private float chasingInvisiblePlayerTimeout;

	private Vector2 chasingInvisiblePlayerTimeoutBounds = new Vector2(4.5f, 7f);

	private float lostPlayerTimestamp;

	private float lostPlayerTimeout;

	private Vector2 lostPlayerTimeoutBounds = new Vector2(4.5f, 6f);

	private GameObject tankBase;

	private GameObject tankTurret;

	private Rigidbody2D baseRB;

	private Vector3? waypointToGo;

	private Vector3? waypointArrivedTo;

	private float travelWaypointReachedTreshhold = 0.4f;

	private float chaseWaypointReachedTreshhold = 0.1f;

	internal static float waypointSearchDistanceSquared;

	private float sightDistance = 7f;

	private float alertDistance;

	private float forcedSightDistance = 1.5f;

	private float sightAngle = 50f;

	private float moveForceNormal = 15f;

	private float moveForceHurry = 22f;

	private int stuckTimeout;

	private int stuckTimeoutMax = 30;

	private float stuckTreshhold = 0.01f;

	private Vector3 previousPosition;

	private Vector2? lastPlayerSeenPoint;

	private Vector2 playerChaseFailsafePoint;

	private GameObject enemyChaseBuiltThrough;

	private QueueLI<Vector2> playerChasePoints;

	private GameObject debugMovePoint;

	private int tellothersEnemyCheckIndex;

	private int tellothersEnemyCheckIndexMax = 5;

	private float lastTimeToldOthers;

	private float currentTellOthersInterval;

	private float tellOthersIntervalMin = 0.035f;

	private float tellOthersIntervalMax = 0.07f;

	private float tellOthersDistance = 6f;

	private bool noEnemiesAhead = true;

	private int enemiesAheadCheckTimer;

	private int enemiesAheadCheckTimerMin = 2;

	private int enemiesAheadCheckTimerMax = 6;

	private float hitPoints;

	private float hitPointsMax;

	private float playerSightAngle;

	private WeaponController weaponController;

	private EnemyTypes enemyType;

	private bool canMove;

	private bool immediateRolloutFlag;

	private GameObject hpBar;

	private float hpBarInitialScaleX;

	private float targetHPBarScaleX;

	private float hpBarScaleSpeed = 3f;

	private float turretShiftFactor;

	private GameObject freezeIce;

	private SpriteRenderer freezeIceSpriteRenderer;

	private float lastTimeHit = -100f;

	private float turretForcedPlayerSightTimeout = 1f;

	private bool enemyEnabled;

	private float lastTimeEnabled;

	private float disableTimeout = 5f;

	private float lastTimeEnabledChecked;

	private float enabledCheckTimeout;

	private float enabledCheckTimeoutMax = 0.1f;

	private int currentSightCheckInterval;

	private bool playerVisionState;

	private bool enemiesAheadState;

	private bool firstEnabledCheckCompleted;

	private float lastTimeSearchedForWaypointInLostPlayerState = -100500f;

	private float searchedForWaypointInLostPlayerStateInterval = 0.2f;

	private Vector2 searchedForWaypointInLostPlayerStateIntervalMinMax = new Vector2(0.2f, 0.3f);

	private int moneyToDispense;

	private bool isBoss;

	private int dirtSpewTicker;

	private float tracksAnimationTicker;

	private float tracksAnimationTickerMax = 0.05f;

	private SpriteRenderer turretSR;

	private SpriteRenderer baseSR;

	private int baseSpriteIndex;

	private float lastTimeEmittedSmoke;

	private float smokeEmitTimeout = 0.15f;

	public Sprite[] EnemyTurretsSprites;

	public Sprite[] EnemyTurretsFlashSprites;

	public Sprite[] EnemyBasesSprites1;

	public Sprite[] EnemyBasesSprites2;

	public Sprite[] EnemyBossTurretsSprites;

	public Sprite[] EnemyBossTurretsFlashSprites;

	public Sprite[] EnemyBossBasesSprites1;

	public Sprite[] EnemyBossBasesSprites2;

	public Sprite EnemyBaseFlash;

	public Sprite EnemyBossBaseFlash;

	public Sprite EnemyTowerBaseFlash;

	public Sprite EnemyTowerBase;

	private int flashFrames;

	private int flashFramesMax = GlobalCommons.FlashFrameCount;

	private float lastAlertTimestamp;

	private int wallBumpCheckTicker = 10;

	private float minimumPlayerDistanceWhileChasing;

	private int logicFramesToSkip;

	private int waypointCheckIndex;

	private int waypointCheckIndexMax = 4;

	private float sawPlayerTimestamp = float.MinValue;

	private float reactionTimeout;

	private bool doStrafe = true;

	private StrafeDirection strafeDirection = StrafeDirection.DoNotStrafe;

	private float strafeDirectionSwitchTimestamp = float.MinValue;

	private float strafeDirectionSwitchDelay;

	private float nudgedTimeStamp = float.MinValue;

	private const float NUDGE_TIMEOUT = 1.5f;

	private int interlockTimer;

	private int interlockTimerMax = 10;

	private float showQuestionMarkTimestamp = float.MinValue;

	private bool PlayerVisionState
	{
		get
		{
			return playerVisionState;
		}
		set
		{
			if (!playerVisionState && value && !IsInAHurry())
			{
				sawPlayerTimestamp = Time.fixedTime;
			}
			playerVisionState = value;
		}
	}

	public bool SeesPlayer => PlayerVisionState;

	public GameObject TankBase => tankBase;

	public GameObject TankTurret => tankTurret;

	public float PlayerAngle => playerSightAngle;

	private void Start()
	{
		if (GlobalCommons.Instance.globalGameStats.LevelsCompleted < 15 || GlobalCommons.Instance.globalGameStats.AvailableWeaponsCount < 2)
		{
			doStrafe = false;
		}
		currentSightCheckInterval = UnityEngine.Random.Range(4, 8);
		chasingInvisiblePlayerTimeout = UnityEngine.Random.Range(chasingInvisiblePlayerTimeoutBounds.x, chasingInvisiblePlayerTimeoutBounds.y);
		lostPlayerTimeout = UnityEngine.Random.Range(lostPlayerTimeoutBounds.x, lostPlayerTimeoutBounds.y);
		waypointSearchDistanceSquared = GlobalCommons.Instance.gridSize * 2f * (GlobalCommons.Instance.gridSize * 2f) * 2f + 0.2f;
		lastTimeEmittedSmoke = Time.fixedTime;
		lastTimeEnabled = Time.fixedTime;
		lastTimeEnabledChecked = Time.fixedTime;
		enabledCheckTimeout = enabledCheckTimeoutMax;
		lastTimeToldOthers = Time.fixedTime;
		InitBaseAndTurret();
		tankBase.transform.parent = base.transform;
		tankTurret.transform.parent = base.transform;
		if (enemyType == EnemyTypes.tank)
		{
			BodyRandomRotationSetter.RandomlyRotate(tankBase);
		}
		baseRB = tankBase.GetComponent<Rigidbody2D>();
		InitTurretRotation();
		SetCanMove();
		GameplayCommons.Instance.enemiesTracker.Track(this);
		InitializeHPBar();
		freezeIce = UnityEngine.Object.Instantiate(Prefabs.enemyFreezePrefab);
		freezeIceSpriteRenderer = freezeIce.GetComponent<SpriteRenderer>();
		freezeIceSpriteRenderer.enabled = false;
		freezeIce.transform.parent = base.transform;
		SetNewState(EnemyState.spawning);
		tankBase.SetActive(value: false);
		tankTurret.SetActive(value: false);
		if (isBoss)
		{
			MakeBoss();
		}
		float num = 0.8f;
		BoxCollider2D component = tankBase.GetComponent<BoxCollider2D>();
		BoxCollider2D boxCollider2D = component;
		Vector2 size = component.size;
		float x = size.x * num;
		Vector2 size2 = component.size;
		boxCollider2D.size = new Vector2(x, size2.y * num);
		InitializeTankParams();
		if (enemyType == EnemyTypes.tank)
		{
			tankBase.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
		}
		tankTurret.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f));
	}

	private void InitializeTankParams()
	{
		reactionTimeout = UnityEngine.Random.Range(0.1f, 0.2f);
		hitPointsMax = EnemiesBalance.GetEnemyHP(weaponController.WeaponType);
		if (enemyType == EnemyTypes.turret)
		{
			hitPointsMax *= GlobalBalance.turretHPCoeff;
		}
		if (isBoss)
		{
			hitPointsMax *= EnemiesBalance.BossHPCoeff;
		}
		sightDistance = 5f + 0.06f * (float)(GlobalCommons.Instance.SelectedLevelBalanceFactor - 1);
		if (sightDistance > 6f)
		{
			sightDistance = 6f;
		}
		alertDistance = sightDistance * 2f;
		turretRotationFactorNormal = 1.5f + 0.043f * (float)(GlobalCommons.Instance.SelectedLevelBalanceFactor - 1);
		if (turretRotationFactorNormal > 3f)
		{
			turretRotationFactorNormal = 3f;
		}
		turretRotationFactorHurry = 5f + 0.17f * (float)(GlobalCommons.Instance.SelectedLevelBalanceFactor - 1);
		if (turretRotationFactorHurry > 10f)
		{
			turretRotationFactorHurry = 10f;
		}
		if (enemyType == EnemyTypes.turret)
		{
			turretRotationFactorNormal *= 0.5f;
			turretRotationFactorHurry *= 0.5f;
		}
		moveForceNormal = 15f;
		if (weaponController.WeaponType == WeaponTypes.suicide)
		{
			moveForceHurry = 35f + 0.2f * (float)(GlobalCommons.Instance.SelectedLevelBalanceFactor - 1);
			if (moveForceHurry > 45f)
			{
				moveForceHurry = 45f;
			}
		}
		else
		{
			moveForceHurry = 16f + 0.4f * (float)(GlobalCommons.Instance.SelectedLevelBalanceFactor - 1);
			if (moveForceHurry > 30f)
			{
				moveForceHurry = 30f;
			}
		}
		tellOthersDistance = sightDistance;
		if (weaponController.WeaponType == WeaponTypes.gold)
		{
			moneyToDispense = 0;
		}
		else
		{
			moneyToDispense = MoneyLootCounter.GetEnemyMoneyLoot();
		}
		if (isBoss)
		{
			moneyToDispense *= 3;
		}
		sightAngle = 45f;
		hitPoints = hitPointsMax;
	}

	private void SpawnTracksDirt(float turnDirection, float angleDif)
	{
		dirtSpewTicker++;
		float num = float.MaxValue;
		int num2 = dirtSpewTicker % 2;
		if (angleDif > 90f)
		{
			switch (num2)
			{
			case 0:
				num = ((!(turnDirection < 0f)) ? 135f : (-135f));
				break;
			case 1:
				num = ((!(turnDirection < 0f)) ? (-45f) : 45f);
				break;
			}
		}
		else
		{
			switch (num2)
			{
			case 0:
				num = -135f;
				break;
			case 1:
				num = -45f;
				break;
			}
		}
		if (num != float.MaxValue)
		{
			Quaternion rotation = Quaternion.Euler(0f, 0f, num);
			Vector3 a = rotation * tankBase.transform.right;
			a.Normalize();
			a = ((!isBoss) ? (a * 0.5f) : (a * 0.68f));
			Vector3 position = tankBase.transform.position;
			float x = position.x + a.x;
			Vector3 position2 = tankBase.transform.position;
			float y = position2.y + a.y;
			Vector3 position3 = tankBase.transform.position;
			Vector3 coords = new Vector3(x, y, position3.z);
			GameplayCommons.Instance.effectsSpawner.CreateTracksDirtEffect(coords);
		}
	}

	internal void HidePlayerFromEnemy()
	{
		switch (currentState)
		{
		case EnemyState.spawning:
			break;
		case EnemyState.idling:
			break;
		case EnemyState.moving:
			break;
		case EnemyState.lostPlayer:
			break;
		case EnemyState.chasingInvisiblePlayer:
			SetNewState(EnemyState.lostPlayer);
			break;
		case EnemyState.chasingVisiblePlayer:
			SetNewState(EnemyState.lostPlayer);
			break;
		default:
			throw new Exception("undetermined state in hide player");
		}
	}

	private void MakeBoss()
	{
		if (enemyType != EnemyTypes.turret && weaponController.WeaponType != WeaponTypes.suicide && weaponController.WeaponType != WeaponTypes.gold)
		{
			weaponController.BossWeapon = true;
			float num = 1.3f;
			BoxCollider2D component = tankBase.GetComponent<BoxCollider2D>();
			component.size = new Vector2(1f / num, 1f / num);
		}
	}

	private void SetupWeapon(WeaponTypes weaponType)
	{
		switch (weaponType)
		{
		case WeaponTypes.cannon:
			weaponController = new EnemyCannonController(this);
			break;
		case WeaponTypes.gold:
			weaponController = new GoldenTankWeaponController(this);
			break;
		case WeaponTypes.homingRocket:
			weaponController = new EnemyHomingRocketController(this);
			break;
		case WeaponTypes.laser:
			weaponController = new LaserController(this);
			break;
		case WeaponTypes.machinegun:
			weaponController = new MachinegunController(this);
			break;
		case WeaponTypes.minigun:
			weaponController = new MinigunController(this);
			break;
		case WeaponTypes.railgun:
			weaponController = new EnemyRailgunController(this);
			break;
		case WeaponTypes.shotgun:
			weaponController = new ShotgunController(this);
			break;
		case WeaponTypes.suicide:
			weaponController = new EnemySuicideController(this);
			break;
		case WeaponTypes.ricochet:
			weaponController = new EnemyRicochetController(this);
			break;
		case WeaponTypes.triple:
			weaponController = new TripleController(this);
			break;
		case WeaponTypes.shocker:
			weaponController = new ShockerController(this);
			break;
		case WeaponTypes.random:
			SetRandomWeapon();
			break;
		default:
			throw new Exception("unknown enemy weapon type");
		}
		minimumPlayerDistanceWhileChasing = weaponController.MinimumPlayerDistanceWhileChasing * UnityEngine.Random.Range(0.9f, 1.1f);
	}

	private void InitBaseAndTurret()
	{
		GameObject original = (enemyType != 0) ? Prefabs.enemyTurretBasePrefab : Prefabs.enemyTankBasePrefab;
		GameObject enemyTurretPrefab = Prefabs.enemyTurretPrefab;
		tankBase = UnityEngine.Object.Instantiate(original, base.transform.position, Quaternion.identity);
		tankTurret = UnityEngine.Object.Instantiate(enemyTurretPrefab, base.transform.position, Quaternion.identity);
		turretSR = tankTurret.GetComponent<SpriteRenderer>();
		baseSR = tankBase.GetComponent<SpriteRenderer>();
		SetBaseAndTurretSprites();
		if (enemyType == EnemyTypes.turret)
		{
			Transform transform = tankTurret.transform;
			Vector3 localScale = tankTurret.transform.localScale;
			transform.localScale = new Vector3(1.2f, 1.2f, localScale.z);
		}
	}

	private void SetBaseAndTurretSprites()
	{
		baseSpriteIndex = (int)weaponController.WeaponType;
		if (enemyType == EnemyTypes.tank)
		{
			if (isBoss)
			{
				baseSR.sprite = EnemyBossBasesSprites1[baseSpriteIndex];
			}
			else
			{
				baseSR.sprite = EnemyBasesSprites1[baseSpriteIndex];
			}
		}
		else
		{
			baseSR.sprite = EnemyTowerBase;
		}
		if (isBoss)
		{
			turretSR.sprite = EnemyBossTurretsSprites[(int)weaponController.WeaponType];
		}
		else
		{
			turretSR.sprite = EnemyTurretsSprites[(int)weaponController.WeaponType];
		}
	}

	private void SetBaseAndTurretFlashSprites()
	{
		baseSpriteIndex = (int)weaponController.WeaponType;
		if (enemyType == EnemyTypes.tank)
		{
			if (isBoss)
			{
				baseSR.sprite = EnemyBossBaseFlash;
			}
			else
			{
				baseSR.sprite = EnemyBaseFlash;
			}
		}
		else
		{
			baseSR.sprite = EnemyTowerBaseFlash;
		}
		if (isBoss)
		{
			turretSR.sprite = EnemyBossTurretsFlashSprites[(int)weaponController.WeaponType];
		}
		else
		{
			turretSR.sprite = EnemyTurretsFlashSprites[(int)weaponController.WeaponType];
		}
	}

	private void SetRandomWeapon()
	{
		switch (UnityEngine.Random.Range(0, 10))
		{
		case 0:
			weaponController = new MachinegunController(this);
			break;
		case 1:
			weaponController = new ShotgunController(this);
			break;
		case 2:
			weaponController = new MinigunController(this);
			break;
		case 3:
			weaponController = new EnemyCannonController(this);
			break;
		case 4:
			weaponController = new EnemyHomingRocketController(this);
			break;
		case 5:
			weaponController = new LaserController(this);
			break;
		case 6:
			weaponController = new EnemyRailgunController(this);
			break;
		case 7:
			if (enemyType == EnemyTypes.tank)
			{
				weaponController = new EnemySuicideController(this);
			}
			else
			{
				SetRandomWeapon();
			}
			break;
		case 8:
			SetRandomWeapon();
			break;
		case 9:
			if (enemyType == EnemyTypes.tank)
			{
				weaponController = new GoldenTankWeaponController(this);
			}
			else
			{
				SetRandomWeapon();
			}
			break;
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

	private void SetCanMove()
	{
		switch (enemyType)
		{
		case EnemyTypes.tank:
			canMove = true;
			break;
		case EnemyTypes.turret:
			canMove = false;
			break;
		}
	}

	public void InitializeEnemy(EnemyTypes enemyType, WeaponTypes weaponType, bool isBoss)
	{
		this.enemyType = enemyType;
		this.isBoss = isBoss;
		SetupWeapon(weaponType);
	}

	public void RollOut()
	{
		immediateRolloutFlag = true;
	}

	public void ProcessPlayerCollision()
	{
		if (weaponController.WeaponType == WeaponTypes.suicide && !GameplayCommons.Instance.playersTankController.PlayerDead && !GameplayCommons.Instance.levelStateController.IsInvisibilityActive)
		{
			float num = EnemiesBalance.GetEnemyDamage(WeaponTypes.suicide);
			if (isBoss)
			{
				num *= EnemiesBalance.bossDamageCoeff;
			}
			ExplosionProcessor.ProcessExplosion(ExplosionProcessor.ExplosionTrigger.enemy, tankBase.transform.position, num, 5f, null, 1.3f, DamageTypes.suicideEnemyDamage);
			ProcessEnemyDestruction(spawnCoins: false, showExplosion: false);
		}
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
		lastTimeHit = Time.fixedTime;
		hitPoints -= damage;
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
		ProcessTellOthers(hitPoints == 0f);
		if (hitPoints == 0f)
		{
			ProcessEnemyDestruction();
		}
		else if (!GameplayCommons.Instance.levelStateController.IsInvisibilityActive)
		{
			checkStateAndSearchForPlayer();
		}
		else
		{
			SetNewState(EnemyState.lostPlayer);
		}
	}

	private void ProcessEnemyDestruction(bool spawnCoins = true, bool showExplosion = true)
	{
		GameplayCommons.Instance.levelStateController.ComboController.Kick();
		switch (enemyType)
		{
		case EnemyTypes.tank:
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TanksDestroyed);
			break;
		case EnemyTypes.turret:
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TowersDestroyed);
			break;
		}
		GameplayCommons.Instance.enemiesTracker.UnTrack(this);
		GameplayCommons.Instance.cameraController.ShakeCamera(6f);
		if (spawnCoins && moneyToDispense > 0)
		{
			BonusesSpawner.SpawnCoinsBonus(tankBase.transform.position, moneyToDispense);
		}
		if (showExplosion)
		{
			GameplayCommons.Instance.effectsSpawner.CreateExplosionEffect(tankBase.transform.position, 1.5f, (enemyType == EnemyTypes.tank) ? true : false);
			if (enemyType == EnemyTypes.turret)
			{
				SoundManager.instance.PlaySpawnerExplosionSound();
			}
		}
		GameplayCommons.Instance.effectsSpawner.SpawnSmoke(tankBase.transform.position, 5, 0.6f);
		GameplayCommons.Instance.effectsSpawner.SpawnExplosionDecal(tankBase.transform.position);
		GameplayCommons.Instance.effectsSpawner.CreateTankExplodedEffect(tankBase.transform.position);
		if (isBoss || enemyType == EnemyTypes.turret)
		{
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerExplodedEffect(tankBase.transform.position);
		}
		else
		{
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerExplodedEffect(tankBase.transform.position, UnityEngine.Random.Range(0, 3));
		}
		if (isBoss)
		{
			WaveExploPostProcessing.ShowEffectAt(Camera.main.WorldToScreenPoint(tankBase.transform.position));
		}
		else
		{
			GameplayCommons.Instance.effectsSpawner.CreateTankExplodedEffect(tankBase.transform.position);
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void checkStateAndSearchForPlayer()
	{
		if (currentState == EnemyState.chasingVisiblePlayer)
		{
			return;
		}
		chasingInvisiblePlayerTimestamp = Time.fixedTime;
		Vector2 direction = GameplayCommons.Instance.playersTankController.TankBase.transform.position - tankBase.transform.position;
		RaycastHit2D value = Physics2D.Raycast(tankBase.transform.position, direction, direction.magnitude, LayerMasks.allObstacleTypesLayerMask);
		if (value.collider == null)
		{
			if (canMove)
			{
				playerChaseFailsafePoint = Vector2.zero;
				enemyChaseBuiltThrough = null;
				playerChasePoints = new QueueLI<Vector2>();
				if (lastPlayerSeenPoint.HasValue)
				{
					playerChasePoints.Enqueue(lastPlayerSeenPoint.Value + (lastPlayerSeenPoint.Value - (Vector2)tankBase.transform.position).normalized * chaseWaypointReachedTreshhold);
				}
				else
				{
					playerChasePoints.Enqueue((Vector2)GameplayCommons.Instance.playersTankController.TankBase.transform.transform.position + ((Vector2)(GameplayCommons.Instance.playersTankController.TankBase.transform.transform.position - tankBase.transform.position)).normalized * chaseWaypointReachedTreshhold);
				}
				SetNewState(EnemyState.chasingInvisiblePlayer);
			}
			else
			{
				SetNewState(EnemyState.lostPlayer);
			}
		}
		else
		{
			Alert(value);
		}
	}

	public Vector3 GetBarrelPoint()
	{
		Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
		Vector3 a = rotation * tankTurret.transform.right;
		a.Normalize();
		a *= 0.5f;
		Vector3 position = tankBase.transform.position;
		float x = position.x + a.x;
		Vector3 position2 = tankBase.transform.position;
		float y = position2.y + a.y;
		Vector3 position3 = tankBase.transform.position;
		return new Vector3(x, y, position3.z);
	}

	private void InitTurretRotation()
	{
		lastTimeTurretRotated = Time.fixedTime;
		if (IsInAHurry())
		{
			turretRotationTimeout = UnityEngine.Random.Range(turretRotationTimeoutMINHurry, turretRotationTimeoutMAXHurry);
		}
		else
		{
			turretRotationTimeout = UnityEngine.Random.Range(turretRotationTimeoutMINNormal, turretRotationTimeoutMAXNormal);
		}
		float f = UnityEngine.Random.Range(0f, (float)Math.PI * 2f);
		turretTurnDirection = new Vector2(Mathf.Cos(f), Mathf.Sin(f)).normalized;
		if (enemyType == EnemyTypes.turret)
		{
			turretTurnDirection *= GlobalCommons.Instance.gridSize * 1.2f;
			if (Physics2D.Raycast(tankBase.transform.position, turretTurnDirection, turretTurnDirection.magnitude, LayerMasks.allObstacleTypesLayerMask).collider != null)
			{
				turretRotationTimeout = 0f;
			}
		}
	}

	public void ForceEnableEnemy()
	{
		UpdateEnabledState(forceEnable: true);
	}

	private void UpdateEnabledState(bool forceEnable = false)
	{
		if (!(Time.fixedTime - lastTimeEnabledChecked >= enabledCheckTimeout) && firstEnabledCheckCompleted)
		{
			return;
		}
		bool flag = false;
		if (!firstEnabledCheckCompleted)
		{
			firstEnabledCheckCompleted = true;
			flag = true;
		}
		lastTimeEnabledChecked = Time.fixedTime;
		enabledCheckTimeout = enabledCheckTimeoutMax + UnityEngine.Random.Range(0f, enabledCheckTimeoutMax);
		if (!forceEnable)
		{
			Vector3 position = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
			float x = position.x;
			Vector3 position2 = tankBase.transform.position;
			if (Mathf.Abs(x - position2.x) <= GlobalCommons.Instance.horizontalEnemyDisableTreshhold)
			{
				Vector3 position3 = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
				float y = position3.y;
				Vector3 position4 = tankBase.transform.position;
				if (Mathf.Abs(y - position4.y) <= GlobalCommons.Instance.verticalEnemyDisableTreshhold)
				{
					goto IL_01bd;
				}
			}
			if (GameplayCommons.Instance.weaponsController.ActiveGuidedRocket != null)
			{
				Vector3 position5 = GameplayCommons.Instance.weaponsController.ActiveGuidedRocket.transform.position;
				float x2 = position5.x;
				Vector3 position6 = tankBase.transform.position;
				if (Mathf.Abs(x2 - position6.x) <= GlobalCommons.Instance.horizontalEnemyDisableTreshhold)
				{
					Vector3 position7 = GameplayCommons.Instance.weaponsController.ActiveGuidedRocket.transform.position;
					float y2 = position7.y;
					Vector3 position8 = tankBase.transform.position;
					if (Mathf.Abs(y2 - position8.y) <= GlobalCommons.Instance.verticalEnemyDisableTreshhold)
					{
						goto IL_01bd;
					}
				}
			}
			if (currentState != EnemyState.chasingInvisiblePlayer && currentState != EnemyState.chasingVisiblePlayer && currentState != EnemyState.lostPlayer && !hpsr.enabled && Time.fixedTime - lastTimeEnabled >= disableTimeout)
			{
				if (enemyEnabled)
				{
					tankBase.SetActive(value: false);
					tankTurret.SetActive(value: false);
				}
				enemyEnabled = false;
			}
			return;
		}
		goto IL_01bd;
		IL_01bd:
		if (!enemyEnabled)
		{
			tankBase.SetActive(value: true);
			tankTurret.SetActive(value: true);
		}
		enemyEnabled = true;
		lastTimeEnabled = Time.fixedTime;
	}

	public bool IsEnemyEnabled()
	{
		return enemyEnabled;
	}

	private void UpdateStrafeState()
	{
		if (doStrafe && Time.fixedTime > strafeDirectionSwitchTimestamp + strafeDirectionSwitchDelay)
		{
			strafeDirectionSwitchTimestamp = Time.fixedTime;
			strafeDirectionSwitchDelay = UnityEngine.Random.Range(0.5f, 1.5f);
			if (Time.fixedTime > nudgedTimeStamp + 1.5f)
			{
				strafeDirection = (StrafeDirection)UnityEngine.Random.Range(0, 2);
			}
			else if (UnityEngine.Random.value > 0.5f)
			{
				strafeDirection = StrafeDirection.Clockwise;
			}
			else
			{
				strafeDirection = StrafeDirection.CounterClockwise;
			}
		}
	}

	private void Update()
	{
		UpdateEnabledState();
		UpdateStrafeState();
		if (!enemyEnabled)
		{
			return;
		}
		tankTurret.transform.position = tankBase.transform.position;
		UpdateHpBar();
		if (flashFrames > 0)
		{
			ProcessEnemyFlash();
		}
		else if (enemyType == EnemyTypes.tank)
		{
			AnimateTracks();
		}
		if (GameplayCommons.Instance.levelStateController.IsFreezeActive)
		{
			if (!freezeIceSpriteRenderer.enabled)
			{
				freezeIceSpriteRenderer.enabled = true;
				GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(tankBase.transform.position);
			}
			freezeIce.transform.position = tankBase.transform.position;
			weaponController.Update(isShooting: false);
			return;
		}
		EmitSmoke();
		if (freezeIceSpriteRenderer.enabled)
		{
			freezeIceSpriteRenderer.enabled = false;
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(tankBase.transform.position);
		}
		ProcessTurretRotationUpdate();
		CheckWaypointArrivedToDistance();
		UpdateSightState();
		if (logicFramesToSkip > 0)
		{
			logicFramesToSkip--;
			return;
		}
		logicFramesToSkip = UnityEngine.Random.Range(1, 3);
		switch (currentState)
		{
		case EnemyState.spawning:
			if (!immediateRolloutFlag)
			{
				SetNewState(EnemyState.idling);
			}
			else
			{
				SetNewState(EnemyState.moving);
			}
			break;
		case EnemyState.idling:
			if (canMove && Time.fixedTime - idleStarted > idleTimeout)
			{
				SetNewState(EnemyState.moving);
			}
			break;
		case EnemyState.lostPlayer:
			if (Time.fixedTime - lostPlayerTimestamp > lostPlayerTimeout)
			{
				SetNewState(EnemyState.idling);
			}
			if (showQuestionMarkTimestamp != float.MinValue && Time.fixedTime > showQuestionMarkTimestamp)
			{
				showQuestionMarkTimestamp = float.MinValue;
				EffectsSpawner effectsSpawner = GameplayCommons.Instance.effectsSpawner;
				Vector3 position = tankBase.transform.position;
				float x = position.x;
				Vector3 position2 = tankBase.transform.position;
				float y = position2.y + GlobalCommons.Instance.gridSize / 2f;
				Vector3 position3 = tankBase.transform.position;
				effectsSpawner.CreateEmotionEffect(EffectsSpawner.EmotionEffectType.Question, new Vector3(x, y, position3.z), this);
			}
			break;
		case EnemyState.chasingVisiblePlayer:
			if (PlayerVisionState)
			{
				ProcessTellOthers();
			}
			break;
		case EnemyState.chasingInvisiblePlayer:
			if (PlayerVisionState)
			{
				ProcessTellOthers();
			}
			wallBumpCheckTicker--;
			if (wallBumpCheckTicker <= 0)
			{
				wallBumpCheckTicker = 15 + UnityEngine.Random.Range(0, 10);
				if (!CheckNextWaypointVisible())
				{
					SetNewState(EnemyState.lostPlayer, skipEmoticon: true);
				}
			}
			if (Time.fixedTime - chasingInvisiblePlayerTimestamp > chasingInvisiblePlayerTimeout)
			{
				SetNewState(EnemyState.lostPlayer);
			}
			break;
		}
		SetPlayerVisionAndUpdateWeapon(PlayerVisionState, enemiesAheadState);
	}

	private void UpdateSightState()
	{
		if (currentSightCheckInterval == 0)
		{
			bool flag = PlayerVisionState;
			ProcessSightLogic();
			if (PlayerVisionState && flag != PlayerVisionState && (currentState == EnemyState.idling || currentState == EnemyState.moving || currentState == EnemyState.lostPlayer) && Time.fixedTime - lastAlertTimestamp > 0.2f && !GameplayCommons.Instance.levelStateController.IsInvisibilityActive && !GameplayCommons.Instance.levelStateController.IsFreezeActive)
			{
				EffectsSpawner effectsSpawner = GameplayCommons.Instance.effectsSpawner;
				Vector3 position = tankBase.transform.position;
				float x = position.x;
				Vector3 position2 = tankBase.transform.position;
				float y = position2.y + GlobalCommons.Instance.gridSize / 2f;
				Vector3 position3 = tankBase.transform.position;
				effectsSpawner.CreateEmotionEffect(EffectsSpawner.EmotionEffectType.Exclamation, new Vector3(x, y, position3.z), this);
			}
			currentSightCheckInterval = UnityEngine.Random.Range(4, 8);
			if (PlayerVisionState)
			{
				BushController.ResetHiddenTimestamp();
				lastPlayerSeenPoint = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
				if (enemyType == EnemyTypes.turret && !enemiesAheadState)
				{
					ProcessTellOthers();
				}
			}
		}
		else
		{
			currentSightCheckInterval--;
		}
	}

	public void ProcessOtherTankStuckBump()
	{
		DebugHelper.Log("BUMP!");
		if (currentState == EnemyState.chasingInvisiblePlayer || currentState == EnemyState.chasingVisiblePlayer)
		{
			SetNewState(EnemyState.lostPlayer);
		}
		else
		{
			SetNewState(EnemyState.moving);
		}
	}

	private void ProcessEnemyFlash()
	{
		flashFrames--;
		if (flashFrames > 0)
		{
			if (flashFrames == flashFramesMax - 1)
			{
				SetBaseAndTurretFlashSprites();
			}
		}
		else
		{
			SetBaseAndTurretSprites();
		}
	}

	private void EmitSmoke()
	{
		if (hitPoints / hitPointsMax <= 0.25f && Time.fixedTime - lastTimeEmittedSmoke >= smokeEmitTimeout)
		{
			lastTimeEmittedSmoke = Time.fixedTime;
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(baseSR.transform.position, 1, 0f);
		}
	}

	private void AnimateTracks()
	{
		Vector2 velocity = baseRB.velocity;
		if (Mathf.Abs(velocity.x) > 0.1f || Mathf.Abs(velocity.y) > 0.1f)
		{
			tracksAnimationTicker += Time.deltaTime;
		}
		if (!(tracksAnimationTicker > tracksAnimationTickerMax))
		{
			return;
		}
		do
		{
			tracksAnimationTicker -= tracksAnimationTickerMax;
		}
		while (tracksAnimationTicker > tracksAnimationTickerMax);
		if (isBoss)
		{
			if (baseSR.sprite == EnemyBossBasesSprites1[baseSpriteIndex])
			{
				baseSR.sprite = EnemyBossBasesSprites2[baseSpriteIndex];
			}
			else
			{
				baseSR.sprite = EnemyBossBasesSprites1[baseSpriteIndex];
			}
		}
		else if (baseSR.sprite == EnemyBasesSprites1[baseSpriteIndex])
		{
			baseSR.sprite = EnemyBasesSprites2[baseSpriteIndex];
		}
		else
		{
			baseSR.sprite = EnemyBasesSprites1[baseSpriteIndex];
		}
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
			Vector3 position = tankBase.transform.position;
			float x2 = position.x - 0.4f;
			Vector3 position2 = tankBase.transform.position;
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

	private void ProcessTellOthers(bool forceImmediate = false)
	{
		if (!enemyEnabled || (!forceImmediate && !(Time.fixedTime - lastTimeToldOthers > currentTellOthersInterval)))
		{
			return;
		}
		ReinitTellOthers();
		List<EnemyTankController> allEnemies = GameplayCommons.Instance.enemiesTracker.AllEnemies;
		for (int i = 0; i < GameplayCommons.Instance.enemiesTracker.AllEnemies.Count; i++)
		{
			EnemyTankController enemyTankController = GameplayCommons.Instance.enemiesTracker.AllEnemies[i];
			int num = i % tellothersEnemyCheckIndexMax;
			if ((num != tellothersEnemyCheckIndex && !forceImmediate) || !(enemyTankController.gameObject != base.gameObject) || enemyTankController.IsChasingPlayer())
			{
				continue;
			}
			Vector2 direction = enemyTankController.tankBase.transform.position - tankBase.transform.position;
			float magnitude = direction.magnitude;
			if (magnitude <= tellOthersDistance && Physics2D.Raycast(tankBase.transform.position, direction, magnitude, LayerMasks.allObstacleTypesLayerMask).collider == null)
			{
				if (!PlayerVisionState || enemyType != 0 || enemyTankController.enemyType != 0)
				{
					enemyTankController.Alert(null, alertedByAnotherEntity: true);
				}
				else
				{
					enemyTankController.Alert(null, alertedByAnotherEntity: true, this);
				}
			}
		}
		tellothersEnemyCheckIndex++;
		if (tellothersEnemyCheckIndex == tellothersEnemyCheckIndexMax)
		{
			tellothersEnemyCheckIndex = 0;
		}
	}

	public void Alert(RaycastHit2D? prevRaycastResult = default(RaycastHit2D?), bool alertedByAnotherEntity = false, EnemyTankController seeingEnemy = null)
	{
		if (!enemyEnabled)
		{
			return;
		}
		if (alertedByAnotherEntity)
		{
			lastAlertTimestamp = Time.fixedTime;
		}
		if ((currentState == EnemyState.idling || currentState == EnemyState.moving) && !GameplayCommons.Instance.levelStateController.IsInvisibilityActive && !GameplayCommons.Instance.levelStateController.IsFreezeActive)
		{
			EffectsSpawner effectsSpawner = GameplayCommons.Instance.effectsSpawner;
			Vector3 position = tankBase.transform.position;
			float x = position.x;
			Vector3 position2 = tankBase.transform.position;
			float y = position2.y + GlobalCommons.Instance.gridSize / 2f;
			Vector3 position3 = tankBase.transform.position;
			effectsSpawner.CreateEmotionEffect(EffectsSpawner.EmotionEffectType.Exclamation, new Vector3(x, y, position3.z), this);
		}
		if (currentState == EnemyState.chasingVisiblePlayer || currentState == EnemyState.chasingInvisiblePlayer)
		{
			return;
		}
		Vector2 direction = GameplayCommons.Instance.playersTankController.TankBase.transform.position - tankBase.transform.position;
		float magnitude = direction.magnitude;
		float num = alertDistance;
		if (magnitude < num)
		{
			num = magnitude;
		}
		RaycastHit2D raycastHit2D = (!prevRaycastResult.HasValue) ? Physics2D.Raycast(tankBase.transform.position, direction, num, LayerMasks.allObstacleTypesLayerMask) : prevRaycastResult.Value;
		bool flag = true;
		if (enemyType == EnemyTypes.tank)
		{
			RaycastHit2D[] array = Physics2D.RaycastAll(tankBase.transform.position, direction, num, LayerMasks.onlyEnemiesLayerMask);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].collider.gameObject != tankBase && array[i].collider.gameObject.transform.parent.GetComponent<EnemyTankController>().enemyType == EnemyTypes.turret)
				{
					flag = false;
					break;
				}
			}
		}
		if (raycastHit2D.collider == null && !GameplayCommons.Instance.levelStateController.IsInvisibilityActive)
		{
			lastPlayerSeenPoint = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
			if (canMove && flag)
			{
				SetNewState(EnemyState.chasingVisiblePlayer);
			}
			else
			{
				SetNewState(EnemyState.lostPlayer);
			}
		}
		else if (seeingEnemy != null && canMove)
		{
			playerChaseFailsafePoint = tankBase.transform.position;
			enemyChaseBuiltThrough = seeingEnemy.tankBase.gameObject;
			playerChasePoints = new QueueLI<Vector2>();
			Vector2 vector = (Vector2)seeingEnemy.tankBase.transform.position + ((Vector2)(seeingEnemy.tankBase.transform.position - tankBase.transform.position)).normalized * chaseWaypointReachedTreshhold;
			playerChasePoints.Enqueue(vector);
			playerChasePoints.Enqueue((Vector2)GameplayCommons.Instance.playersTankController.TankBase.transform.position + ((Vector2)GameplayCommons.Instance.playersTankController.TankBase.transform.position - vector).normalized * chaseWaypointReachedTreshhold);
			SetNewState(EnemyState.chasingInvisiblePlayer);
		}
		else if (currentState == EnemyState.idling || currentState == EnemyState.moving)
		{
			SetNewState(EnemyState.lostPlayer);
		}
		else if (currentState == EnemyState.lostPlayer)
		{
			lostPlayerTimestamp = Time.fixedTime;
		}
	}

	private void CheckWaypointArrivedToDistance()
	{
		Vector3? vector = waypointArrivedTo;
		if (vector.HasValue && ((Vector2)(waypointArrivedTo.Value - tankBase.transform.position)).magnitude > travelWaypointReachedTreshhold * 2f)
		{
			waypointArrivedTo = null;
		}
	}

	private void ProcessSightLogic()
	{
		if (GameplayCommons.Instance.playersTankController.PlayerDead)
		{
			PlayerVisionState = false;
			enemiesAheadState = false;
			return;
		}
		Vector2 vector = GameplayCommons.Instance.playersTankController.TankBase.transform.position - tankBase.transform.position;
		float magnitude = vector.magnitude;
		float num = sightDistance;
		if (enemyType == EnemyTypes.turret && IsInAHurry())
		{
			num *= 1.5f;
		}
		if (magnitude <= num)
		{
			if (GameplayCommons.Instance.playersTankController.PlayerDead)
			{
				PlayerVisionState = false;
				enemiesAheadState = false;
				return;
			}
			playerSightAngle = Vector2.Angle(tankTurret.transform.up, vector);
			if (magnitude <= forcedSightDistance)
			{
				PlayerVisionState = true;
				enemiesAheadState = false;
				return;
			}
			if (BushController.PlayerHidden && !IsInAHurry())
			{
				PlayerVisionState = false;
				enemiesAheadState = false;
				return;
			}
			float num2 = sightAngle;
			if (currentState == EnemyState.chasingInvisiblePlayer || currentState == EnemyState.chasingVisiblePlayer)
			{
				num2 *= 1.5f;
			}
			if (playerSightAngle <= num2)
			{
				RaycastHit2D[] array = Physics2D.RaycastAll(tankBase.transform.position, vector, magnitude, LayerMasks.allObstacleTypesAndEnemiesLayerMask);
				bool flag = false;
				bool flag2 = false;
				for (int i = 0; i < array.Length; i++)
				{
					RaycastHit2D raycastHit2D = array[i];
					if (Array.IndexOf(PhysicsLayers.obstaclesBlockingSightLayers, raycastHit2D.collider.gameObject.layer) != -1)
					{
						flag2 = true;
						break;
					}
					if (Array.IndexOf(PhysicsLayers.enemiesBlockingSightLayers, raycastHit2D.collider.gameObject.layer) == -1)
					{
						continue;
					}
					if (raycastHit2D.collider.gameObject != tankBase.gameObject)
					{
						flag = true;
					}
					for (int j = 0; j < GameplayCommons.Instance.enemiesTracker.AllEnemies.Count; j++)
					{
						EnemyTankController enemyTankController = GameplayCommons.Instance.enemiesTracker.AllEnemies[j];
						if (!(enemyTankController.tankBase == tankBase) && enemyTankController.tankBase == raycastHit2D.collider.gameObject && enemyTankController.enemyType == EnemyTypes.turret)
						{
							flag2 = true;
							break;
						}
					}
					if (flag2)
					{
						break;
					}
				}
				if (!flag2)
				{
					PlayerVisionState = true;
					enemiesAheadState = flag;
					return;
				}
			}
		}
		PlayerVisionState = false;
		enemiesAheadState = false;
	}

	private void SetPlayerVisionAndUpdateWeapon(bool val, bool enemiesAhead)
	{
		if (GameplayCommons.Instance.levelStateController.IsInvisibilityActive)
		{
			val = false;
		}
		if (val)
		{
			if (currentState != EnemyState.chasingVisiblePlayer)
			{
				SetNewState(EnemyState.chasingVisiblePlayer);
			}
		}
		else if (currentState == EnemyState.chasingVisiblePlayer)
		{
			playerChasePoints = new QueueLI<Vector2>();
			if (lastPlayerSeenPoint.HasValue)
			{
				playerChasePoints.Enqueue(lastPlayerSeenPoint.Value + (lastPlayerSeenPoint.Value - (Vector2)tankBase.transform.position).normalized * chaseWaypointReachedTreshhold);
			}
			else
			{
				playerChasePoints.Enqueue((Vector2)GameplayCommons.Instance.playersTankController.TankBase.transform.transform.position + ((Vector2)(GameplayCommons.Instance.playersTankController.TankBase.transform.transform.position - tankBase.transform.position)).normalized * chaseWaypointReachedTreshhold);
			}
			if (GameplayCommons.Instance.playersTankController.PlayerDead)
			{
				SetNewState(EnemyState.idling);
			}
			else if (canMove)
			{
				if (weaponController.InstantlyForgetPlayerOnSightLoss || GameplayCommons.Instance.levelStateController.IsInvisibilityActive)
				{
					SetNewState(EnemyState.lostPlayer);
				}
				else
				{
					SetNewState(EnemyState.chasingInvisiblePlayer);
				}
			}
			else
			{
				SetNewState(EnemyState.lostPlayer);
			}
		}
		bool isShooting = val;
		if (playerSightAngle > weaponController.ShootAngleTreshhold || enemiesAhead || Time.fixedTime < sawPlayerTimestamp + reactionTimeout)
		{
			isShooting = false;
		}
		Vector3 position = tankBase.transform.position;
		float x = position.x;
		Vector3 position2 = Camera.main.transform.position;
		if (!(Mathf.Abs(x - position2.x) > GlobalCommons.Instance.DynamicHorizontalScreenBorderPlusOneCell))
		{
			Vector3 position3 = tankBase.transform.position;
			float y = position3.y;
			Vector3 position4 = Camera.main.transform.position;
			if (!(Mathf.Abs(y - position4.y) > GlobalCommons.Instance.DynamicVerticalScreenBorderDistancePlusOneCell))
			{
				goto IL_0269;
			}
		}
		isShooting = false;
		goto IL_0269;
		IL_0269:
		weaponController.Update(isShooting);
	}

	private void FixedUpdate()
	{
		if (!enemyEnabled || GameplayCommons.Instance.levelStateController.IsFreezeActive)
		{
			return;
		}
		switch (currentState)
		{
		case EnemyState.moving:
		{
			Vector3? vector3 = waypointToGo;
			if (!vector3.HasValue)
			{
				waypointArrivedTo = null;
				SetNewState(EnemyState.moving);
			}
			else if (MoveTo(waypointToGo.Value) <= travelWaypointReachedTreshhold)
			{
				waypointArrivedTo = waypointToGo;
				SetNewState(EnemyState.idling);
			}
			break;
		}
		case EnemyState.chasingVisiblePlayer:
		{
			Vector2 v = GameplayCommons.Instance.playersTankController.TankBase.transform.position - tankBase.transform.position;
			if (!canMove)
			{
				break;
			}
			float magnitude = v.magnitude;
			if (Math.Abs(magnitude - minimumPlayerDistanceWhileChasing) > 0.3f)
			{
				if (magnitude > minimumPlayerDistanceWhileChasing)
				{
					MoveTo(GameplayCommons.Instance.playersTankController.TankBase.transform.position);
					break;
				}
				Vector3 position = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
				float x = v.x;
				float y = v.y;
				Vector3 position2 = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
				MoveTo(position - new Vector3(x, y, position2.z) * 2f);
				break;
			}
			Vector2? vector2 = null;
			switch (strafeDirection)
			{
			case StrafeDirection.Clockwise:
				vector2 = (Vector2)tankBase.transform.position + RotateVector2(v, 90f);
				break;
			case StrafeDirection.CounterClockwise:
				vector2 = (Vector2)tankBase.transform.position + RotateVector2(v, -90f);
				break;
			}
			if (vector2.HasValue)
			{
				MoveTo(vector2.Value, checkObstacles: true);
			}
			break;
		}
		case EnemyState.chasingInvisiblePlayer:
			BuildChaseQueue();
			if (playerChasePoints.Count > 0 && MoveTo(playerChasePoints.Peek()) <= chaseWaypointReachedTreshhold)
			{
				playerChasePoints.Dequeue();
			}
			if (playerChasePoints.Count == 0)
			{
				SetNewState(EnemyState.lostPlayer);
			}
			break;
		case EnemyState.lostPlayer:
		{
			if (!canMove)
			{
				break;
			}
			Vector3? vector = waypointToGo;
			if (!vector.HasValue)
			{
				if (Time.fixedTime - lastTimeSearchedForWaypointInLostPlayerState > searchedForWaypointInLostPlayerStateInterval)
				{
					lastTimeSearchedForWaypointInLostPlayerState = Time.fixedTime;
					searchedForWaypointInLostPlayerStateInterval = UnityEngine.Random.Range(searchedForWaypointInLostPlayerStateIntervalMinMax.x, searchedForWaypointInLostPlayerStateIntervalMinMax.y);
					waypointArrivedTo = null;
					waypointToGo = FindWaypointToGo();
				}
			}
			else if (MoveTo(waypointToGo.Value) <= travelWaypointReachedTreshhold)
			{
				waypointArrivedTo = waypointToGo;
				waypointToGo = null;
			}
			break;
		}
		}
	}

	private Vector2 RotateVector2(Vector2 v, float degrees)
	{
		float num = Mathf.Sin(degrees * ((float)Math.PI / 180f));
		float num2 = Mathf.Cos(degrees * ((float)Math.PI / 180f));
		float x = v.x;
		float y = v.y;
		v.x = num2 * x - num * y;
		v.y = num * x + num2 * y;
		return v;
	}

	private bool CheckNextWaypointVisible()
	{
		Vector2 direction = playerChasePoints.Peek() - (Vector2)tankBase.transform.position;
		return Physics2D.Raycast(tankBase.transform.position, direction, direction.magnitude, LayerMasks.allObstacleTypesLayerMask).collider == null;
	}

	private void BuildChaseQueue()
	{
		Vector2 last = playerChasePoints.Last;
		Vector2 direction = (Vector2)GameplayCommons.Instance.playersTankController.TankBase.transform.position - last;
		bool flag = false;
		float magnitude = direction.magnitude;
		if (magnitude > 2f)
		{
			flag = true;
		}
		RaycastHit2D raycastHit2D = Physics2D.Raycast(last, direction, direction.magnitude, LayerMasks.allObstacleTypesLayerMask);
		if (flag || raycastHit2D.collider != null)
		{
			playerChasePoints.Enqueue(last + direction.normalized * (magnitude + chaseWaypointReachedTreshhold));
		}
	}

	private void PerformStuckCheck()
	{
		if (IsMoving())
		{
			Vector3? vector = waypointToGo;
			if (vector.HasValue && !PlayerVisionState)
			{
				Vector3 position = tankBase.transform.position;
				if (Mathf.Abs(position.x - previousPosition.x) <= stuckTreshhold)
				{
					Vector3 position2 = tankBase.transform.position;
					if (Mathf.Abs(position2.y - previousPosition.y) <= stuckTreshhold)
					{
						stuckTimeout++;
						if (stuckTimeout >= stuckTimeoutMax)
						{
							waypointToGo = null;
							stuckTimeout = 0;
							if (currentState == EnemyState.chasingInvisiblePlayer)
							{
								SetNewState(EnemyState.lostPlayer);
							}
						}
						return;
					}
				}
				stuckTimeout = 0;
				return;
			}
		}
		stuckTimeout = 0;
	}

	private bool IsMoving()
	{
		switch (currentState)
		{
		case EnemyState.chasingVisiblePlayer:
			return true;
		case EnemyState.chasingInvisiblePlayer:
			return true;
		case EnemyState.lostPlayer:
			return true;
		case EnemyState.moving:
			return true;
		case EnemyState.idling:
			return false;
		case EnemyState.spawning:
			return false;
		default:
			throw new Exception("undetermined state in ismoving");
		}
	}

	private float MoveTo(Vector3 moveToPoint, bool checkObstacles = false)
	{
		Vector2 vector = moveToPoint - tankBase.transform.position;
		float magnitude = vector.magnitude;
		if (enemiesAheadCheckTimer > 0)
		{
			enemiesAheadCheckTimer--;
		}
		else
		{
			enemiesAheadCheckTimer = UnityEngine.Random.Range(enemiesAheadCheckTimerMin, enemiesAheadCheckTimerMax);
			LayerMask mask = LayerMasks.allObstacleTypesAndEnemiesLayerMask;
			if (GameplayCommons.Instance.playersTankController.PlayerDead)
			{
				mask = LayerMasks.enemiesAndPlayerLayerMask;
			}
			RaycastHit2D[] array = Physics2D.RaycastAll(tankBase.transform.position, vector, 1.15f, mask);
			noEnemiesAhead = true;
			RaycastHit2D[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				RaycastHit2D raycastHit2D = array2[i];
				if (raycastHit2D.collider.gameObject.layer == PhysicsLayers.PlayersTankBase)
				{
					noEnemiesAhead = false;
					break;
				}
				if (raycastHit2D.collider.gameObject.layer == PhysicsLayers.EnemyTankBase)
				{
					if (!(raycastHit2D.collider.gameObject != tankBase.gameObject))
					{
						continue;
					}
					EnemyTankController component = raycastHit2D.collider.gameObject.transform.parent.GetComponent<EnemyTankController>();
					if (component.enemyType != 0)
					{
						break;
					}
					noEnemiesAhead = false;
					if ((component.currentState == EnemyState.idling || component.currentState == EnemyState.moving) && (currentState == EnemyState.chasingVisiblePlayer || currentState == EnemyState.chasingInvisiblePlayer))
					{
						component.Alert();
					}
					component.Nudge();
					interlockTimer++;
					if (interlockTimer >= interlockTimerMax && component.interlockTimer >= interlockTimerMax)
					{
						interlockTimer = 0;
						ProcessOtherTankStuckBump();
						component.interlockTimer = 0;
						component.ProcessOtherTankStuckBump();
					}
					else if (currentState == EnemyState.chasingInvisiblePlayer && enemyChaseBuiltThrough == component.tankBase && playerChasePoints.Untouched)
					{
						Vector2[] array3 = playerChasePoints.ToArray();
						playerChasePoints.Clear();
						playerChasePoints.Enqueue(playerChaseFailsafePoint);
						for (int j = 0; j < array3.Length; j++)
						{
							playerChasePoints.Enqueue(array3[j]);
						}
						playerChaseFailsafePoint = Vector2.zero;
						enemyChaseBuiltThrough = null;
					}
					break;
				}
				if (checkObstacles)
				{
					noEnemiesAhead = false;
				}
			}
		}
		if (noEnemiesAhead)
		{
			interlockTimer = 0;
			if (Time.fixedTime > sawPlayerTimestamp + reactionTimeout)
			{
				float num = (!IsInAHurry()) ? moveForceNormal : moveForceHurry;
				baseRB.AddForce(vector.normalized * num, ForceMode2D.Force);
				RotateBodyToDirection(vector, baseRB, num / 2f);
			}
		}
		else
		{
			switch (currentState)
			{
			case EnemyState.lostPlayer:
				waypointArrivedTo = null;
				waypointToGo = null;
				break;
			case EnemyState.moving:
				waypointArrivedTo = null;
				waypointToGo = null;
				break;
			}
		}
		return magnitude;
	}

	private void Nudge()
	{
		nudgedTimeStamp = Time.fixedTime;
	}

	private bool IsChasingPlayer()
	{
		switch (currentState)
		{
		case EnemyState.chasingVisiblePlayer:
			return true;
		case EnemyState.chasingInvisiblePlayer:
			return true;
		default:
			return false;
		}
	}

	private bool IsInAHurry()
	{
		switch (currentState)
		{
		case EnemyState.chasingVisiblePlayer:
			return true;
		case EnemyState.chasingInvisiblePlayer:
			return true;
		case EnemyState.lostPlayer:
			return true;
		default:
			return false;
		}
	}

	private void ProcessTurretRotationUpdate()
	{
		if (PlayerVisionState && !GameplayCommons.Instance.levelStateController.IsInvisibilityActive)
		{
			Vector2 shootingVector = GameplayCommons.Instance.playersTankController.TankBase.transform.position - tankBase.transform.position;
			RotateTurret(shootingVector);
		}
		else if (enemyType == EnemyTypes.turret && Time.fixedTime - lastTimeHit <= turretForcedPlayerSightTimeout && !GameplayCommons.Instance.levelStateController.IsInvisibilityActive)
		{
			Vector2 v = GameplayCommons.Instance.playersTankController.TankBase.transform.position - tankBase.transform.position;
			if (Time.fixedTime - lastTimeSetTurretForcedRotationAngleMod > 0.25f)
			{
				lastTimeSetTurretForcedRotationAngleMod = Time.fixedTime;
				currentTurretForcedRotationAngleMod = UnityEngine.Random.Range(-35f, 35f);
			}
			Vector2 shootingVector2 = Quaternion.Euler(0f, 0f, currentTurretForcedRotationAngleMod) * v;
			RotateTurret(shootingVector2);
		}
		else
		{
			if (currentState != EnemyState.chasingInvisiblePlayer)
			{
				RotateTurret(turretTurnDirection);
			}
			else if (playerChasePoints.Count > 0)
			{
				RotateTurret(playerChasePoints.Peek() - (Vector2)tankBase.transform.position);
			}
			else
			{
				RotateTurret(GameplayCommons.Instance.playersTankController.TankBase.transform.position - tankBase.transform.position);
			}
			if (Time.fixedTime - lastTimeTurretRotated > turretRotationTimeout)
			{
				InitTurretRotation();
			}
		}
		if (turretShiftFactor > 0f)
		{
			Vector3 position = tankBase.transform.position;
			Vector3 a = Quaternion.Euler(0f, 0f, 90f) * tankTurret.transform.right;
			a.Normalize();
			a *= 0f - turretShiftFactor;
			position += a;
			turretShiftFactor -= 1.25f * Time.deltaTime;
			if (turretShiftFactor < 0f)
			{
				turretShiftFactor = 0f;
			}
			tankTurret.transform.position = position;
		}
	}

	public void SetMaxTurretShiftFactor()
	{
		turretShiftFactor = 0.1f;
	}

	private void RotateTurret(Vector2 shootingVector)
	{
		float z = Mathf.Atan2(shootingVector.y, shootingVector.x) * 57.29578f - 90f;
		if (enemyType == EnemyTypes.tank)
		{
			float num = (!IsInAHurry()) ? turretRotationFactorNormal : turretRotationFactorHurry;
			tankTurret.transform.rotation = Quaternion.Slerp(tankTurret.transform.rotation, Quaternion.Euler(0f, 0f, z), num * Time.deltaTime);
			return;
		}
		float num2 = 60f;
		if (IsInAHurry())
		{
			num2 = 150f;
		}
		tankTurret.transform.rotation = Quaternion.RotateTowards(tankTurret.transform.rotation, Quaternion.Euler(0f, 0f, z), num2 * Time.deltaTime);
	}

	private void RotateBodyToDirection(Vector2 targetVector, Rigidbody2D rb, float torqueFactor)
	{
		float num = Mathf.Abs(Vector2.Angle(tankBase.transform.up, targetVector));
		Vector3 vector = Vector3.Cross(rb.transform.up, targetVector);
		vector.Normalize();
		float z = vector.z;
		rb.AddTorque(torqueFactor * z * num / 90f);
		if (z != 0f)
		{
			SpawnTracksDirt(z, num);
			SpawnTracksDirt(z, num);
		}
	}

	public void Unfreeze()
	{
		if (enemyEnabled)
		{
			SetNewState(EnemyState.lostPlayer);
		}
		else
		{
			SetNewState(EnemyState.idling);
		}
	}

	private void SetNewState(EnemyState newState, bool skipEmoticon = false)
	{
		EnemyState enemyState = currentState;
		if (enemyState == EnemyState.lostPlayer)
		{
			showQuestionMarkTimestamp = float.MinValue;
		}
		switch (newState)
		{
		case EnemyState.idling:
			playerChasePoints = new QueueLI<Vector2>();
			if (currentState == EnemyState.lostPlayer && !GameplayCommons.Instance.levelStateController.IsFreezeActive)
			{
				EffectsSpawner effectsSpawner = GameplayCommons.Instance.effectsSpawner;
				Vector3 position = tankBase.transform.position;
				float x = position.x;
				Vector3 position2 = tankBase.transform.position;
				float y = position2.y + GlobalCommons.Instance.gridSize / 2f;
				Vector3 position3 = tankBase.transform.position;
				effectsSpawner.CreateEmotionEffect(EffectsSpawner.EmotionEffectType.Threedots, new Vector3(x, y, position3.z), this);
			}
			lastPlayerSeenPoint = null;
			currentState = EnemyState.idling;
			InitializeIdleState();
			break;
		case EnemyState.moving:
		{
			playerChasePoints = new QueueLI<Vector2>();
			waypointToGo = FindWaypointToGo();
			Vector3? vector = waypointToGo;
			if (vector.HasValue)
			{
				currentState = EnemyState.moving;
				break;
			}
			currentState = EnemyState.idling;
			InitializeIdleState();
			break;
		}
		case EnemyState.chasingVisiblePlayer:
			playerChasePoints = new QueueLI<Vector2>();
			ReinitTellOthers();
			waypointToGo = null;
			currentState = EnemyState.chasingVisiblePlayer;
			break;
		case EnemyState.lostPlayer:
			if (!GameplayCommons.Instance.levelStateController.IsFreezeActive && currentState != EnemyState.lostPlayer && !skipEmoticon)
			{
				showQuestionMarkTimestamp = Time.fixedTime + UnityEngine.Random.Range(1f, 2f);
			}
			playerChasePoints = new QueueLI<Vector2>();
			lastPlayerSeenPoint = null;
			waypointToGo = null;
			lostPlayerTimestamp = Time.fixedTime;
			currentState = EnemyState.lostPlayer;
			break;
		default:
			currentState = newState;
			break;
		case EnemyState.chasingInvisiblePlayer:
			chasingInvisiblePlayerTimestamp = Time.fixedTime;
			currentState = EnemyState.chasingInvisiblePlayer;
			break;
		}
	}

	private void ReinitTellOthers()
	{
		lastTimeToldOthers = Time.fixedTime;
		currentTellOthersInterval = UnityEngine.Random.Range(tellOthersIntervalMin, tellOthersIntervalMax);
	}

	private Vector3? FindWaypointToGo()
	{
		Vector2 velocity = tankBase.GetComponent<Rigidbody2D>().velocity;
		if (velocity.x == 0f)
		{
			Vector2 velocity2 = tankBase.GetComponent<Rigidbody2D>().velocity;
			if (velocity2.y == 0f)
			{
				List<Vector3> list = new List<Vector3>();
				List<Vector3> list2 = new List<Vector3>();
				waypointCheckIndex++;
				if (waypointCheckIndex == waypointCheckIndexMax)
				{
					waypointCheckIndex = 0;
				}
				for (int i = 0; i < GameplayCommons.Instance.levelStateController.Waypoints.Count; i++)
				{
					int num = i % waypointCheckIndexMax;
					if (num != waypointCheckIndex)
					{
						continue;
					}
					Vector3 vector = GameplayCommons.Instance.levelStateController.Waypoints[i];
					if (waypointArrivedTo == vector)
					{
						continue;
					}
					Vector2 vector2 = vector - tankBase.transform.position;
					if (!(vector2.SqrMagnitude() <= waypointSearchDistanceSquared))
					{
						continue;
					}
					float magnitude = vector2.magnitude;
					bool flag = false;
					RaycastHit2D[] array = Physics2D.RaycastAll(tankBase.transform.position, vector - tankBase.transform.position, magnitude, LayerMasks.allObstacleTypesAndEnemiesLayerMask);
					RaycastHit2D[] array2 = array;
					for (int j = 0; j < array2.Length; j++)
					{
						RaycastHit2D raycastHit2D = array2[j];
						if (raycastHit2D.collider.gameObject != tankBase.gameObject)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						continue;
					}
					bool flag2 = false;
					float num2 = 0.6f;
					foreach (EnemyTankController allEnemy in GameplayCommons.Instance.enemiesTracker.AllEnemies)
					{
						if (!(allEnemy == this))
						{
							if (flag2)
							{
								break;
							}
							Vector3 position = allEnemy.tankBase.transform.position;
							if (Mathf.Abs(position.x - vector.x) <= num2)
							{
								Vector3 position2 = allEnemy.tankBase.transform.position;
								if (Mathf.Abs(position2.y - vector.y) <= num2)
								{
									flag2 = true;
								}
							}
						}
					}
					foreach (DestructableObstacleController allDestObstacle in GameplayCommons.Instance.enemiesTracker.AllDestObstacles)
					{
						if (flag2)
						{
							break;
						}
						Vector3 position3 = allDestObstacle.transform.position;
						if (Mathf.Abs(position3.x - vector.x) <= num2)
						{
							Vector3 position4 = allDestObstacle.transform.position;
							if (Mathf.Abs(position4.y - vector.y) <= num2)
							{
								flag2 = true;
							}
						}
					}
					if (!flag2)
					{
						if (magnitude >= waypointSearchDistanceSquared / 2f)
						{
							list2.Add(vector);
						}
						else
						{
							list.Add(vector);
						}
					}
				}
				if (list.Count > 0 || list2.Count > 0)
				{
					if (list2.Count > 0)
					{
						return list2[UnityEngine.Random.Range(0, list2.Count)];
					}
					return list[UnityEngine.Random.Range(0, list.Count)];
				}
				return null;
			}
		}
		return null;
	}

	private void InitializeIdleState()
	{
		idleStarted = Time.fixedTime;
		idleTimeout = UnityEngine.Random.Range(idleTimeoutMIN, idleTimeoutMAX);
	}
}
