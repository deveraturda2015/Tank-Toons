using System;
using UnityEngine;
using UnityEngine.UI;

public class BonusController : MonoBehaviour
{
	public enum BonusType
	{
		coinBonus,
		healthBonus,
		freezeBonus,
		bombBonus,
		ShotgunAmmoBonus,
		RicochetAmmoBonus,
		TripleAmmoBonus,
		ShockAmmoBonus,
		MinigunAmmoBonus,
		CannonAmmoBonus,
		HomingRocketAmmoBonus,
		LaserAmmoBonus,
		GuidedRocketAmmoBonus,
		RailgunAmmoBonus,
		MinesAmmoBonus,
		bronzeCoinBonus,
		goldenCoinBonus,
		platinumCoinBonus,
		invisibilityBonus,
		doubleDamageBonus,
		revealMapBonus
	}

	private float spawnedTimestamp;

	private float disappearTime;

	private Vector2 disappearTimeBounds = new Vector2(10f, 11f);

	private float fadeOutTime = 0.75f;

	private bool fadeBeforeDisappearing = true;

	public Sprite[] BombBonusSprites;

	public Sprite BronzeCoinBonus;

	public Sprite GoldenCoinBonus;

	public Sprite PlatinumCoinBonus;

	private bool isCoin;

	private Vector2 scatterImpulseBounds = new Vector2(0.02f, 0.04f);

	public BonusType bonusType;

	private bool attractsToPlayer;

	private bool attractsToPlayerOnLevelComplete;

	private bool attractToPlayerOnLevelCompleteLayerSet;

	private float playerAttractDistance = 3.5f;

	private float levelCompleteAttractSpeedFactor;

	private float lastTimeProcessedRaycastToPlayer;

	private float currentPlayerRaycastCheckDelay = 1f;

	private Vector2 playerRaycastDelayBounds = new Vector2(0.1f, 0.2f);

	private bool attractToPlayer;

	private float distanceFactor;

	private Rigidbody2D bonusRB;

	private SpriteRenderer bonusSR;

	internal bool doDestroyGameobject = true;

	private bool doesDisappear;

	public static BonusType[] activeBonusTypes = new BonusType[3]
	{
		BonusType.invisibilityBonus,
		BonusType.freezeBonus,
		BonusType.doubleDamageBonus
	};

	private readonly Vector2 FLYOUTPOSITION_AMMO = new Vector2(64.4f, 37f);

	private readonly Vector2 FLYOUTPOSITION_HP = new Vector2(-64.4f, 37f);

	private readonly Vector2 FLYOUTPOSITION_ACTIVEBONUS = new Vector2(0f, 437f);

	private void Start()
	{
		bonusRB = GetComponent<Rigidbody2D>();
		if (bonusSR == null)
		{
			bonusSR = GetComponent<SpriteRenderer>();
		}
		levelCompleteAttractSpeedFactor = UnityEngine.Random.Range(0.8f, 1.2f);
		disappearTime = UnityEngine.Random.Range(disappearTimeBounds.x, disappearTimeBounds.y);
		spawnedTimestamp = Time.fixedTime;
		if (bonusType == BonusType.bombBonus)
		{
			disappearTime = 1.2f;
			fadeBeforeDisappearing = false;
			doesDisappear = true;
		}
		GameplayCommons.Instance.enemiesTracker.Track(this);
	}

	private void ReinitPlayerRaycastDelay()
	{
		currentPlayerRaycastCheckDelay = UnityEngine.Random.Range(playerRaycastDelayBounds.x, playerRaycastDelayBounds.y);
	}

	public void SetupCoin(BonusType coinType)
	{
		isCoin = true;
		doesDisappear = true;
		GameplayCommons.Instance.enemiesTracker.Track(this);
		attractToPlayerOnLevelCompleteLayerSet = false;
		disappearTime = UnityEngine.Random.Range(disappearTimeBounds.x, disappearTimeBounds.y);
		ApplyScatterImpulse();
		attractsToPlayerOnLevelComplete = true;
		attractsToPlayer = true;
		ReinitPlayerRaycastDelay();
		if (bonusSR == null)
		{
			bonusSR = GetComponent<SpriteRenderer>();
		}
		bonusSR.SetAlpha(1f);
		spawnedTimestamp = Time.fixedTime;
		float num = UnityEngine.Random.Range(0.85f, 1.15f);
		Transform transform = base.transform;
		float x = num;
		float y = num;
		Vector3 localScale = base.transform.localScale;
		transform.localScale = new Vector3(x, y, localScale.z);
		bonusSR = GetComponent<SpriteRenderer>();
		bonusType = coinType;
		switch (coinType)
		{
		case BonusType.bronzeCoinBonus:
			bonusSR.sprite = BronzeCoinBonus;
			break;
		case BonusType.goldenCoinBonus:
			bonusSR.sprite = GoldenCoinBonus;
			break;
		case BonusType.platinumCoinBonus:
			bonusSR.sprite = PlatinumCoinBonus;
			break;
		}
	}

	public void ApplyScatterImpulse()
	{
		float f = UnityEngine.Random.Range(0f, (float)Math.PI * 2f);
		Vector2 vector = new Vector2(Mathf.Cos(f), Mathf.Sin(f));
		float d = UnityEngine.Random.Range(scatterImpulseBounds.x, scatterImpulseBounds.y);
		Rigidbody2D component = base.gameObject.GetComponent<Rigidbody2D>();
		component.AddForce(vector.normalized * d / 100f, ForceMode2D.Impulse);
	}

	private void Update()
	{
		if (attractsToPlayerOnLevelComplete && GameplayCommons.Instance.levelStateController.LevelCompletionPending && !attractToPlayerOnLevelCompleteLayerSet)
		{
			attractToPlayerOnLevelCompleteLayerSet = true;
			base.gameObject.layer = PhysicsLayers.HitPlayerOnly;
		}
		float num = disappearTime - (Time.fixedTime - spawnedTimestamp);
		if (this.bonusType == BonusType.bombBonus)
		{
			float num2 = num / (disappearTime / 3f);
			if ((double)num2 - Math.Truncate(num2) > 0.25)
			{
				bonusSR.sprite = BombBonusSprites[0];
			}
			else
			{
				if (bonusSR.sprite != BombBonusSprites[1])
				{
					SoundManager.instance.PlayBombBeepSound();
				}
				bonusSR.sprite = BombBonusSprites[1];
			}
		}
		if (!doesDisappear || !(Time.fixedTime - spawnedTimestamp > disappearTime - fadeOutTime))
		{
			return;
		}
		if (fadeBeforeDisappearing)
		{
			if (!isCoin || !GameplayCommons.Instance.levelStateController.LevelCompletionPending)
			{
				SpriteRenderer spriteRenderer = bonusSR;
				Color color = bonusSR.color;
				float r = color.r;
				Color color2 = bonusSR.color;
				float g = color2.g;
				Color color3 = bonusSR.color;
				spriteRenderer.color = new Color(r, g, color3.b, num / fadeOutTime);
			}
			else
			{
				SpriteRenderer spriteRenderer2 = bonusSR;
				Color color4 = bonusSR.color;
				float r2 = color4.r;
				Color color5 = bonusSR.color;
				float g2 = color5.g;
				Color color6 = bonusSR.color;
				spriteRenderer2.color = new Color(r2, g2, color6.b, 1f);
			}
		}
		if (num <= 0f && (!isCoin || !GameplayCommons.Instance.levelStateController.LevelCompletionPending))
		{
			BonusType bonusType = this.bonusType;
			if (bonusType == BonusType.bombBonus)
			{
				ExplodeBomb();
			}
			DestroyBonus();
		}
	}

	private void DestroyBonus()
	{
		GameplayCommons.Instance.enemiesTracker.UnTrack(this);
		if (doDestroyGameobject)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private void FixedUpdate()
	{
		if (attractsToPlayer)
		{
			AttractToPlayer();
		}
	}

	private void AttractToPlayer()
	{
		if (!GameplayCommons.Instance.playersTankController.PlayerDead)
		{
			Vector2 playerVector = GameplayCommons.Instance.playersTankController.TankBase.transform.position - base.transform.position;
			UpdatePlayerRaycastResult(playerVector);
			if (attractToPlayer || (attractsToPlayerOnLevelComplete && GameplayCommons.Instance.levelStateController.LevelCompletionPending))
			{
				bonusRB.AddForce(playerVector.normalized * distanceFactor / 100f);
			}
		}
	}

	private void UpdatePlayerRaycastResult(Vector2 playerVector)
	{
		if (!(Time.fixedTime - lastTimeProcessedRaycastToPlayer > currentPlayerRaycastCheckDelay))
		{
			return;
		}
		ReinitPlayerRaycastDelay();
		lastTimeProcessedRaycastToPlayer = Time.fixedTime;
		float magnitude = playerVector.magnitude;
		float num = (!attractsToPlayerOnLevelComplete || !GameplayCommons.Instance.levelStateController.LevelCompletionPending) ? playerAttractDistance : 100500f;
		if (magnitude < num)
		{
			attractToPlayer = (Physics2D.Raycast(base.transform.position, playerVector, magnitude, LayerMasks.allObstacleTypesLayerMask).collider == null);
		}
		else
		{
			attractToPlayer = false;
		}
		if (!attractToPlayer && (!attractsToPlayerOnLevelComplete || !GameplayCommons.Instance.levelStateController.LevelCompletionPending))
		{
			return;
		}
		distanceFactor = (num - magnitude) / num * 1f;
		if (attractsToPlayerOnLevelComplete && GameplayCommons.Instance.levelStateController.LevelCompletionPending)
		{
			levelCompleteAttractSpeedFactor += 0.04f;
			distanceFactor *= magnitude / 10f * levelCompleteAttractSpeedFactor;
			if (distanceFactor < 0.7f)
			{
				distanceFactor = 0.7f;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		ProcessContact(col.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		ProcessContact(col.gameObject);
	}

	private void CreateFlyoff()
	{
		Vector2 targetPosition;
		switch (bonusType)
		{
		case BonusType.coinBonus:
			return;
		case BonusType.bombBonus:
			return;
		case BonusType.bronzeCoinBonus:
			return;
		case BonusType.goldenCoinBonus:
			return;
		case BonusType.platinumCoinBonus:
			return;
		case BonusType.CannonAmmoBonus:
			targetPosition = FLYOUTPOSITION_AMMO;
			break;
		case BonusType.doubleDamageBonus:
			targetPosition = FLYOUTPOSITION_ACTIVEBONUS;
			break;
		case BonusType.revealMapBonus:
			targetPosition = FLYOUTPOSITION_ACTIVEBONUS;
			break;
		case BonusType.freezeBonus:
			targetPosition = FLYOUTPOSITION_ACTIVEBONUS;
			break;
		case BonusType.GuidedRocketAmmoBonus:
			targetPosition = FLYOUTPOSITION_AMMO;
			break;
		case BonusType.healthBonus:
			targetPosition = FLYOUTPOSITION_HP;
			break;
		case BonusType.HomingRocketAmmoBonus:
			targetPosition = FLYOUTPOSITION_AMMO;
			break;
		case BonusType.invisibilityBonus:
			targetPosition = FLYOUTPOSITION_ACTIVEBONUS;
			break;
		case BonusType.LaserAmmoBonus:
			targetPosition = FLYOUTPOSITION_AMMO;
			break;
		case BonusType.MinesAmmoBonus:
			targetPosition = FLYOUTPOSITION_AMMO;
			break;
		case BonusType.MinigunAmmoBonus:
			targetPosition = FLYOUTPOSITION_AMMO;
			break;
		case BonusType.RailgunAmmoBonus:
			targetPosition = FLYOUTPOSITION_AMMO;
			break;
		case BonusType.ShotgunAmmoBonus:
			targetPosition = FLYOUTPOSITION_AMMO;
			break;
		case BonusType.RicochetAmmoBonus:
			targetPosition = FLYOUTPOSITION_AMMO;
			break;
		case BonusType.TripleAmmoBonus:
			targetPosition = FLYOUTPOSITION_AMMO;
			break;
		case BonusType.ShockAmmoBonus:
			targetPosition = FLYOUTPOSITION_AMMO;
			break;
		default:
			throw new Exception("unknown bonus flyout position:" + bonusType.ToString());
		}
		Image component = UnityEngine.Object.Instantiate(Prefabs.BonusFlyoffImagePrefab).GetComponent<Image>();
		component.GetComponent<BonusFlyoff>().InitFlyoff(bonusSR.transform.position, targetPosition, bonusSR.sprite);
	}

	private void ProcessContact(GameObject go)
	{
		if (go.layer != PhysicsLayers.PlayersTankBase || GameplayCommons.Instance.playersTankController.PlayerDead)
		{
			return;
		}
		switch (bonusType)
		{
		case BonusType.coinBonus:
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.CoinsCollected);
			SoundManager.instance.PlayCoinPickupSound();
			GameplayCommons.Instance.levelStateController.currentGameStats.PickupMoney(10);
			break;
		case BonusType.bronzeCoinBonus:
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.CoinsCollected);
			SoundManager.instance.PlayCoinPickupSound();
			GameplayCommons.Instance.levelStateController.currentGameStats.PickupMoney(5);
			GameplayCommons.Instance.effectsSpawner.SpawnCoinFlyoffEffect(base.transform.position, BonusType.bronzeCoinBonus);
			break;
		case BonusType.goldenCoinBonus:
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.CoinsCollected);
			SoundManager.instance.PlayCoinPickupSound();
			GameplayCommons.Instance.levelStateController.currentGameStats.PickupMoney(20);
			GameplayCommons.Instance.effectsSpawner.SpawnCoinFlyoffEffect(base.transform.position, BonusType.goldenCoinBonus);
			break;
		case BonusType.platinumCoinBonus:
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.CoinsCollected);
			SoundManager.instance.PlayCoinPickupSound();
			GameplayCommons.Instance.levelStateController.currentGameStats.PickupMoney(50);
			GameplayCommons.Instance.effectsSpawner.SpawnCoinFlyoffEffect(base.transform.position, BonusType.platinumCoinBonus);
			break;
		case BonusType.healthBonus:
			if (GameplayCommons.Instance.playersTankController.HPPercentage >= 1f)
			{
				return;
			}
			GameplayCommons.Instance.cameraController.ShakeCamera();
			SoundManager.instance.PlayHealthPickupSound();
			GameplayCommons.Instance.playersTankController.PickupHealthBonus();
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(GameplayCommons.Instance.playersTankController.TankBase.transform.position);
			break;
		case BonusType.freezeBonus:
			GameplayCommons.Instance.cameraController.ShakeCamera();
			SoundManager.instance.PlayCrateFreezeSound();
			GameplayCommons.Instance.levelStateController.FreezeEnemies();
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(GameplayCommons.Instance.playersTankController.TankBase.transform.position);
			break;
		case BonusType.invisibilityBonus:
			SoundManager.instance.PlayBonusPickupSound();
			GameplayCommons.Instance.cameraController.ShakeCamera();
			GameplayCommons.Instance.levelStateController.ProcessInvisibilityPickup();
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(GameplayCommons.Instance.playersTankController.TankBase.transform.position);
			break;
		case BonusType.doubleDamageBonus:
			SoundManager.instance.PlayBonusPickupSound();
			GameplayCommons.Instance.cameraController.ShakeCamera();
			GameplayCommons.Instance.levelStateController.ProcessDoubleDamagePickup();
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(GameplayCommons.Instance.playersTankController.TankBase.transform.position);
			break;
		case BonusType.revealMapBonus:
			SoundManager.instance.PlayBonusPickupSound();
			SoundManager.instance.PlayRadarBonusSound();
			GameplayCommons.Instance.cameraController.ShakeCamera();
			GameplayCommons.Instance.gameplayUIController.FlashScreen();
			GameplayCommons.Instance.visibilityController.SlowlyRevealAll();
			GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(GameplayCommons.Instance.playersTankController.TankBase.transform.position);
			break;
		case BonusType.ShotgunAmmoBonus:
			if (GameplayCommons.Instance.weaponsController.GetAmmoPercentageForWeapon(WeaponTypes.shotgun) == 1f)
			{
				return;
			}
			SoundManager.instance.PlayAmmoPickupSound();
			GameplayCommons.Instance.weaponsController.CollectAmmoBonus(WeaponTypes.shotgun);
			break;
		case BonusType.RicochetAmmoBonus:
			if (GameplayCommons.Instance.weaponsController.GetAmmoPercentageForWeapon(WeaponTypes.ricochet) == 1f)
			{
				return;
			}
			SoundManager.instance.PlayAmmoPickupSound();
			GameplayCommons.Instance.weaponsController.CollectAmmoBonus(WeaponTypes.ricochet);
			break;
		case BonusType.TripleAmmoBonus:
			if (GameplayCommons.Instance.weaponsController.GetAmmoPercentageForWeapon(WeaponTypes.triple) == 1f)
			{
				return;
			}
			SoundManager.instance.PlayAmmoPickupSound();
			GameplayCommons.Instance.weaponsController.CollectAmmoBonus(WeaponTypes.triple);
			break;
		case BonusType.ShockAmmoBonus:
			if (GameplayCommons.Instance.weaponsController.GetAmmoPercentageForWeapon(WeaponTypes.shocker) == 1f)
			{
				return;
			}
			SoundManager.instance.PlayAmmoPickupSound();
			GameplayCommons.Instance.weaponsController.CollectAmmoBonus(WeaponTypes.shocker);
			break;
		case BonusType.MinigunAmmoBonus:
			if (GameplayCommons.Instance.weaponsController.GetAmmoPercentageForWeapon(WeaponTypes.minigun) == 1f)
			{
				return;
			}
			SoundManager.instance.PlayAmmoPickupSound();
			GameplayCommons.Instance.weaponsController.CollectAmmoBonus(WeaponTypes.minigun);
			break;
		case BonusType.CannonAmmoBonus:
			if (GameplayCommons.Instance.weaponsController.GetAmmoPercentageForWeapon(WeaponTypes.cannon) == 1f)
			{
				return;
			}
			SoundManager.instance.PlayAmmoPickupSound();
			GameplayCommons.Instance.weaponsController.CollectAmmoBonus(WeaponTypes.cannon);
			break;
		case BonusType.HomingRocketAmmoBonus:
			if (GameplayCommons.Instance.weaponsController.GetAmmoPercentageForWeapon(WeaponTypes.homingRocket) == 1f)
			{
				return;
			}
			SoundManager.instance.PlayAmmoPickupSound();
			GameplayCommons.Instance.weaponsController.CollectAmmoBonus(WeaponTypes.homingRocket);
			break;
		case BonusType.LaserAmmoBonus:
			if (GameplayCommons.Instance.weaponsController.GetAmmoPercentageForWeapon(WeaponTypes.laser) == 1f)
			{
				return;
			}
			SoundManager.instance.PlayAmmoPickupSound();
			GameplayCommons.Instance.weaponsController.CollectAmmoBonus(WeaponTypes.laser);
			break;
		case BonusType.GuidedRocketAmmoBonus:
			if (GameplayCommons.Instance.weaponsController.GetAmmoPercentageForWeapon(WeaponTypes.guidedRocket) == 1f)
			{
				return;
			}
			SoundManager.instance.PlayAmmoPickupSound();
			GameplayCommons.Instance.weaponsController.CollectAmmoBonus(WeaponTypes.guidedRocket);
			break;
		case BonusType.RailgunAmmoBonus:
			if (GameplayCommons.Instance.weaponsController.GetAmmoPercentageForWeapon(WeaponTypes.railgun) == 1f)
			{
				return;
			}
			SoundManager.instance.PlayAmmoPickupSound();
			GameplayCommons.Instance.weaponsController.CollectAmmoBonus(WeaponTypes.railgun);
			break;
		case BonusType.MinesAmmoBonus:
			if (GameplayCommons.Instance.weaponsController.GetAmmoPercentageForWeapon(WeaponTypes.mines) == 1f)
			{
				return;
			}
			SoundManager.instance.PlayAmmoPickupSound();
			GameplayCommons.Instance.weaponsController.CollectAmmoBonus(WeaponTypes.mines);
			break;
		case BonusType.bombBonus:
			ExplodeBomb();
			break;
		}
		DestroyBonus();
		CreateFlyoff();
	}

	private void ExplodeBomb()
	{
		GameplayCommons.Instance.effectsSpawner.SpawnExplosionDecal(base.transform.position);
		ExplosionProcessor.ProcessExplosion(ExplosionProcessor.ExplosionTrigger.neutral, base.transform.position, GlobalBalance.GetExplBarrelDamage(), 5f, null, 1.3f, DamageTypes.bombBonusDamage);
		GameplayCommons.Instance.cameraController.ShakeCamera();
	}
}
