using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EffectsSpawner
{
	public enum ShardEffectType
	{
		wall,
		harderWall,
		specialWall,
		crateChunk
	}

	public enum EffectsSpawnerPreset
	{
		GameplayNoWeather,
		GameplayWithRain,
		GameplayWithSnow,
		MainMenu,
		RewardedVideoAndShopMenuAndPrizeScene,
		UpgradesShop
	}

	public enum SleeveType
	{
		Machinegun,
		Minigun,
		Shotgun,
		Cannon,
		Blaster
	}

	public enum EmotionEffectType
	{
		Exclamation,
		Question,
		Threedots,
		Smiley
	}

	internal static bool disableEffects;

	private ParticleSystem rainDropParticleSystem;

	private ParticleSystem rainFallParticleSystem;

	private ParticleSystem snowParticleSystem;

	private ParticleSystem fireworksParticleSystem;

	private ParticleSystem PrizeCoinPickupParticleSystem;

	private ParticleSystem tracksDirtParticleSystem;

	private ParticleSystem bulletDisappearParticleSystem;

	private ParticleSystem hitEffectParticleSystem;

	private ParticleSystem shotFlareParticleSystem;

	private ParticleSystem LevelCompleteFireworksFlareParticleSystem;

	private ParticleSystem smokeEffectParticleSystem;

	private ParticleSystem darkSmokeParticleSystem;

	private ParticleSystem wallShardParticleSystem;

	private ParticleSystem MissileLaunchSmokeParticleSystem;

	private ParticleSystem harderWallShardParticleSystem;

	private ParticleSystem specialWallShardParticleSystem;

	private ParticleSystem crateShardParticleSystem;

	private ParticleSystem BulletHitSparksEffectParticleSystem;

	private ParticleSystem laserHitParticleSystem;

	private ParticleSystem railgunEffectParticleSystem;

	private ParticleSystem enemyExplosion1ParticleSystem;

	private ParticleSystem explodedSpawnerParticleSystem;

	private ParticleSystem coinFlyoffBronzeParticleSystem;

	private ParticleSystem coinFlyoffGoldParticleSystem;

	private ParticleSystem coinFlyoffPlatinumParticleSystem;

	private ParticleSystem BushLeaveParticleSystem;

	private ParticleSystem wireSparkParticleSystem;

	private ParticleSystem UpgradesSparkFlareParticleSystem;

	private ParticleSystem exclamationMarkEffectParticleSystem;

	private ParticleSystem questionMarkEffectParticleSystem;

	private ParticleSystem threedotsEffectParticleSystem;

	private ParticleSystem smileyEffectParticleSystem;

	private ParticleSystem overUICoinFlyoffParticleSystem;

	private ParticleSystem OverUIAmmoBarFlyoffParticleSystem;

	private ParticleSystem OverUIHPBarFlyoffParticleSystem;

	private ParticleSystem overUIFlareParticleSystem;

	private ParticleSystem UpgradeAvailableFalloffParticleSystem;

	private ParticleSystem purchaseParticleSystem;

	private ParticleSystem bulletSleevesMachinegunParticleSystem;

	private ParticleSystem bulletSleevesMinigunParticleSystem;

	private ParticleSystem bulletSleevesShotgunParticleSystem;

	private ParticleSystem bulletSleevesBlasterParticleSystem;

	private ParticleSystem bulletSleevesCannonParticleSystem;

	private ParticleSystem SpawnerSpawnEffect_FlashParticleSystem;

	private ParticleSystem SpawnerSpawnEffect_SparksParticleSystem;

	private ParticleSystem ExplosionEffect_DarkSmokeParticleSystem;

	private ParticleSystem ExplosionEffect_FlashParticleSystem;

	private ParticleSystem ExplosionEffect_SparksParticleSystem;

	private List<ParticleSystem> allParticleSystems;

	private Vector3? rainDropParticleSystemCameraOffset;

	private Vector3? rainFallParticleSystemCameraOffset;

	private Vector3? snowParticleSystemCameraOffset;

	private List<EnemyTankController> enemiesShownSmileys;

	public bool WeatherSystemActive;

	private bool overUIcoinSizeSet;

	private bool DisableEffects => disableEffects || Time.timeScale < 0.1f;

	public EffectsSpawner(EffectsSpawnerPreset preset, TileMap.TilesType tilesType = TileMap.TilesType.SummerTiles)
	{
		disableEffects = false;
		WaveExploPostProcessing.ResetEffectsCount();
		allParticleSystems = new List<ParticleSystem>();
		switch (preset)
		{
		case EffectsSpawnerPreset.GameplayNoWeather:
			AddCommonGameplayParticleEffects(tilesType);
			break;
		case EffectsSpawnerPreset.GameplayWithRain:
		{
			WeatherSystemActive = true;
			rainDropParticleSystem = UnityEngine.Object.Instantiate(Prefabs.RainDropParticleSystemEffect).GetComponent<ParticleSystem>();
			allParticleSystems.Add(rainDropParticleSystem);
			rainFallParticleSystem = UnityEngine.Object.Instantiate(Prefabs.RainFallParticleSystemEffect).GetComponent<ParticleSystem>();
			allParticleSystems.Add(rainFallParticleSystem);
			int num = UnityEngine.Random.Range(600, 1000);
			//rainDropParticleSystem.GetComponent<ParticleSystem>().emission.rate = num;
			//rainFallParticleSystem.GetComponent<ParticleSystem>().emission.rate = num / 6;
			ResizeWeatherPS(rainDropParticleSystem);
			ResizeWeatherPS(rainFallParticleSystem);
			AddCommonGameplayParticleEffects(tilesType);
			break;
		}
		case EffectsSpawnerPreset.GameplayWithSnow:
			WeatherSystemActive = true;
			snowParticleSystem = UnityEngine.Object.Instantiate(Prefabs.SnowParticleSystemEffect).GetComponent<ParticleSystem>();
			allParticleSystems.Add(snowParticleSystem.GetComponent<ParticleSystem>());
			//snowParticleSystem.GetComponent<ParticleSystem>().emission.rate = UnityEngine.Random.Range(200, 1000);
			ResizeWeatherPS(snowParticleSystem);
			AddCommonGameplayParticleEffects(tilesType);
			break;
		case EffectsSpawnerPreset.MainMenu:
			AddExplosionParticleSystems();
			enemyExplosion1ParticleSystem = UnityEngine.Object.Instantiate(Prefabs.explodedTankEffectPS1).GetComponent<ParticleSystem>();
			allParticleSystems.Add(enemyExplosion1ParticleSystem);
			tracksDirtParticleSystem = UnityEngine.Object.Instantiate(Prefabs.tracksDirtEffectPS).GetComponent<ParticleSystem>();
			allParticleSystems.Add(tracksDirtParticleSystem);
			smokeEffectParticleSystem = UnityEngine.Object.Instantiate(Prefabs.smokeEffectPS).GetComponent<ParticleSystem>();
			allParticleSystems.Add(smokeEffectParticleSystem);
			break;
		case EffectsSpawnerPreset.RewardedVideoAndShopMenuAndPrizeScene:
			AddExplosionParticleSystems();
			overUIFlareParticleSystem = UnityEngine.Object.Instantiate(Prefabs.UIFlareParticleSystemEffect).GetComponent<ParticleSystem>();
			allParticleSystems.Add(overUIFlareParticleSystem);
			overUICoinFlyoffParticleSystem = UnityEngine.Object.Instantiate(Prefabs.overUICoinFlyoffParticleSystem).GetComponent<ParticleSystem>();
			allParticleSystems.Add(overUICoinFlyoffParticleSystem);
			smokeEffectParticleSystem = UnityEngine.Object.Instantiate(Prefabs.smokeEffectPS).GetComponent<ParticleSystem>();
			allParticleSystems.Add(smokeEffectParticleSystem);
			PrizeCoinPickupParticleSystem = UnityEngine.Object.Instantiate(Prefabs.PrizeCoinPickupParticleSystemEffect).GetComponent<ParticleSystem>();
			allParticleSystems.Add(PrizeCoinPickupParticleSystem);
			LevelCompleteFireworksFlareParticleSystem = UnityEngine.Object.Instantiate(Prefabs.LevelCompleteFireworksFlare).GetComponent<ParticleSystem>();
			allParticleSystems.Add(LevelCompleteFireworksFlareParticleSystem);
			fireworksParticleSystem = UnityEngine.Object.Instantiate(Prefabs.LevelCompleteFireworks).GetComponent<ParticleSystem>();
			allParticleSystems.Add(fireworksParticleSystem);
			break;
		case EffectsSpawnerPreset.UpgradesShop:
			purchaseParticleSystem = UnityEngine.Object.Instantiate(Prefabs.PurchaseParticleSystemEffect).GetComponent<ParticleSystem>();
			allParticleSystems.Add(purchaseParticleSystem);
			overUICoinFlyoffParticleSystem = UnityEngine.Object.Instantiate(Prefabs.overUICoinFlyoffParticleSystem).GetComponent<ParticleSystem>();
			allParticleSystems.Add(overUICoinFlyoffParticleSystem);
			UpgradeAvailableFalloffParticleSystem = UnityEngine.Object.Instantiate(Prefabs.UpgradeAvailableFalloffParticleSystemEffect).GetComponent<ParticleSystem>();
			allParticleSystems.Add(UpgradeAvailableFalloffParticleSystem);
			wireSparkParticleSystem = UnityEngine.Object.Instantiate(Prefabs.WireSparkParticleSystemEffect).GetComponent<ParticleSystem>();
			allParticleSystems.Add(wireSparkParticleSystem);
			UpgradesSparkFlareParticleSystem = UnityEngine.Object.Instantiate(Prefabs.UpgradesSparkFlare).GetComponent<ParticleSystem>();
			allParticleSystems.Add(UpgradesSparkFlareParticleSystem);
			LevelCompleteFireworksFlareParticleSystem = UnityEngine.Object.Instantiate(Prefabs.LevelCompleteFireworksFlare).GetComponent<ParticleSystem>();
			allParticleSystems.Add(LevelCompleteFireworksFlareParticleSystem);
			break;
		default:
			throw new Exception("no effects init routine specified to preset");
		}
		enemiesShownSmileys = new List<EnemyTankController>();
		foreach (ParticleSystem allParticleSystem in allParticleSystems)
		{
			if (allParticleSystem != rainFallParticleSystem && allParticleSystem != rainDropParticleSystem && allParticleSystem != snowParticleSystem)
			{
				allParticleSystem.transform.position = new Vector3(-200f, 200f, 0f);
			}
		}
	}

	private void ResizeWeatherPS(ParticleSystem ps)
	{
		ParticleSystem.ShapeModule shape = ps.shape;
		Vector3 scale = shape.scale;
		float x = scale.x * GameplayCommons.Instance.cameraController.initialCameraOrtoSize / 5f;
		Vector3 scale2 = shape.scale;
		float y = scale2.y * GameplayCommons.Instance.cameraController.initialCameraOrtoSize / 5f;
		Vector3 scale3 = shape.scale;
		shape.scale = new Vector3(x, y, scale3.z * GameplayCommons.Instance.cameraController.initialCameraOrtoSize / 5f);
	}

	private void AddExplosionParticleSystems()
	{
		ExplosionEffect_DarkSmokeParticleSystem = UnityEngine.Object.Instantiate(Prefabs.ExplosionEffect_DarkSmoke).GetComponent<ParticleSystem>();
		allParticleSystems.Add(ExplosionEffect_DarkSmokeParticleSystem);
		ExplosionEffect_FlashParticleSystem = UnityEngine.Object.Instantiate(Prefabs.ExplosionEffect_Flash).GetComponent<ParticleSystem>();
		allParticleSystems.Add(ExplosionEffect_FlashParticleSystem);
		ExplosionEffect_SparksParticleSystem = UnityEngine.Object.Instantiate(Prefabs.ExplosionEffect_Sparks).GetComponent<ParticleSystem>();
		allParticleSystems.Add(ExplosionEffect_SparksParticleSystem);
	}

	private void AddCommonGameplayParticleEffects(TileMap.TilesType tilesType)
	{
		overUIFlareParticleSystem = UnityEngine.Object.Instantiate(Prefabs.UIFlareParticleSystemEffect).GetComponent<ParticleSystem>();
		allParticleSystems.Add(overUIFlareParticleSystem);
		SpawnerSpawnEffect_FlashParticleSystem = UnityEngine.Object.Instantiate(Prefabs.SpawnerSpawnEffect_Flash).GetComponent<ParticleSystem>();
		allParticleSystems.Add(SpawnerSpawnEffect_FlashParticleSystem);
		SpawnerSpawnEffect_SparksParticleSystem = UnityEngine.Object.Instantiate(Prefabs.SpawnerSpawnEffect_Sparks).GetComponent<ParticleSystem>();
		allParticleSystems.Add(SpawnerSpawnEffect_SparksParticleSystem);
		AddExplosionParticleSystems();
		BulletHitSparksEffectParticleSystem = UnityEngine.Object.Instantiate(Prefabs.BulletHitSparksParticleSystemEffect).GetComponent<ParticleSystem>();
		allParticleSystems.Add(BulletHitSparksEffectParticleSystem);
		bulletSleevesMachinegunParticleSystem = UnityEngine.Object.Instantiate(Prefabs.BulletSleevesMachinegunParticleSystemEffect).GetComponent<ParticleSystem>();
		allParticleSystems.Add(bulletSleevesMachinegunParticleSystem);
		bulletSleevesMinigunParticleSystem = UnityEngine.Object.Instantiate(Prefabs.BulletSleevesMinigunParticleSystemEffect).GetComponent<ParticleSystem>();
		allParticleSystems.Add(bulletSleevesMinigunParticleSystem);
		bulletSleevesShotgunParticleSystem = UnityEngine.Object.Instantiate(Prefabs.BulletSleevesShotgunParticleSystemEffect).GetComponent<ParticleSystem>();
		allParticleSystems.Add(bulletSleevesShotgunParticleSystem);
		bulletSleevesCannonParticleSystem = UnityEngine.Object.Instantiate(Prefabs.BulletSleevesCannonParticleSystemEffect).GetComponent<ParticleSystem>();
		allParticleSystems.Add(bulletSleevesCannonParticleSystem);
		bulletSleevesBlasterParticleSystem = UnityEngine.Object.Instantiate(Prefabs.BulletSleevesBlasterParticleSystemEffect).GetComponent<ParticleSystem>();
		allParticleSystems.Add(bulletSleevesBlasterParticleSystem);
		switch (tilesType)
		{
		case TileMap.TilesType.SummerTiles:
			BushLeaveParticleSystem = UnityEngine.Object.Instantiate(Prefabs.BushLeaveParticleSystemEffect).GetComponent<ParticleSystem>();
			break;
		case TileMap.TilesType.DesertTiles:
			BushLeaveParticleSystem = UnityEngine.Object.Instantiate(Prefabs.BushLeaveParticleSystemEffectDesert).GetComponent<ParticleSystem>();
			break;
		case TileMap.TilesType.WinterTiles:
			BushLeaveParticleSystem = UnityEngine.Object.Instantiate(Prefabs.BushLeaveParticleSystemEffectWinter).GetComponent<ParticleSystem>();
			break;
		default:
			throw new Exception("no leaves for tiles type: " + tilesType.ToString());
		}
		allParticleSystems.Add(BushLeaveParticleSystem);
		explodedSpawnerParticleSystem = UnityEngine.Object.Instantiate(Prefabs.ExplodedSpawnerParticleSystemEffect).GetComponent<ParticleSystem>();
		allParticleSystems.Add(explodedSpawnerParticleSystem);
		enemyExplosion1ParticleSystem = UnityEngine.Object.Instantiate(Prefabs.explodedTankEffectPS1).GetComponent<ParticleSystem>();
		allParticleSystems.Add(enemyExplosion1ParticleSystem);
		railgunEffectParticleSystem = UnityEngine.Object.Instantiate(Prefabs.railgunBeamEffectPS).GetComponent<ParticleSystem>();
		allParticleSystems.Add(railgunEffectParticleSystem);
		laserHitParticleSystem = UnityEngine.Object.Instantiate(Prefabs.laserHitEffectPS).GetComponent<ParticleSystem>();
		allParticleSystems.Add(laserHitParticleSystem);
		MissileLaunchSmokeParticleSystem = UnityEngine.Object.Instantiate(Prefabs.MissileLaunchSmokeParticleSystemEffect).GetComponent<ParticleSystem>();
		allParticleSystems.Add(MissileLaunchSmokeParticleSystem);
		wallShardParticleSystem = UnityEngine.Object.Instantiate(Prefabs.wallShardEffectPS).GetComponent<ParticleSystem>();
		allParticleSystems.Add(wallShardParticleSystem);
		harderWallShardParticleSystem = UnityEngine.Object.Instantiate(Prefabs.harderWallShardEffectPS).GetComponent<ParticleSystem>();
		allParticleSystems.Add(harderWallShardParticleSystem);
		specialWallShardParticleSystem = UnityEngine.Object.Instantiate(Prefabs.specialWallShardEffectPS).GetComponent<ParticleSystem>();
		allParticleSystems.Add(specialWallShardParticleSystem);
		crateShardParticleSystem = UnityEngine.Object.Instantiate(Prefabs.crateShardEffectPS).GetComponent<ParticleSystem>();
		allParticleSystems.Add(crateShardParticleSystem);
		darkSmokeParticleSystem = UnityEngine.Object.Instantiate(Prefabs.darkSmokeEffectPS).GetComponent<ParticleSystem>();
		allParticleSystems.Add(darkSmokeParticleSystem);
		smokeEffectParticleSystem = UnityEngine.Object.Instantiate(Prefabs.smokeEffectPS).GetComponent<ParticleSystem>();
		allParticleSystems.Add(smokeEffectParticleSystem);
		shotFlareParticleSystem = UnityEngine.Object.Instantiate(Prefabs.shotFlareEffectPS).GetComponent<ParticleSystem>();
		allParticleSystems.Add(shotFlareParticleSystem);
		hitEffectParticleSystem = UnityEngine.Object.Instantiate(Prefabs.hitEffectPS).GetComponent<ParticleSystem>();
		allParticleSystems.Add(hitEffectParticleSystem);
		bulletDisappearParticleSystem = UnityEngine.Object.Instantiate(Prefabs.bulletDisappearEffectPS).GetComponent<ParticleSystem>();
		allParticleSystems.Add(bulletDisappearParticleSystem);
		tracksDirtParticleSystem = UnityEngine.Object.Instantiate(Prefabs.tracksDirtEffectPS).GetComponent<ParticleSystem>();
		allParticleSystems.Add(tracksDirtParticleSystem);
		exclamationMarkEffectParticleSystem = UnityEngine.Object.Instantiate(Prefabs.exclamationParticleSystemEffect).GetComponent<ParticleSystem>();
		allParticleSystems.Add(exclamationMarkEffectParticleSystem);
		questionMarkEffectParticleSystem = UnityEngine.Object.Instantiate(Prefabs.questionParticleSystemEffect).GetComponent<ParticleSystem>();
		allParticleSystems.Add(questionMarkEffectParticleSystem);
		threedotsEffectParticleSystem = UnityEngine.Object.Instantiate(Prefabs.threedotsParticleSystemEffect).GetComponent<ParticleSystem>();
		allParticleSystems.Add(threedotsEffectParticleSystem);
		smileyEffectParticleSystem = UnityEngine.Object.Instantiate(Prefabs.smileyParticleSystemEffect).GetComponent<ParticleSystem>();
		coinFlyoffGoldParticleSystem = UnityEngine.Object.Instantiate(Prefabs.CoinFlyoffParticleSystemGold).GetComponent<ParticleSystem>();
		allParticleSystems.Add(coinFlyoffGoldParticleSystem);
		coinFlyoffBronzeParticleSystem = UnityEngine.Object.Instantiate(Prefabs.CoinFlyoffParticleSystemBronze).GetComponent<ParticleSystem>();
		allParticleSystems.Add(coinFlyoffBronzeParticleSystem);
		coinFlyoffPlatinumParticleSystem = UnityEngine.Object.Instantiate(Prefabs.CoinFlyoffParticleSystemPlatinum).GetComponent<ParticleSystem>();
		allParticleSystems.Add(coinFlyoffPlatinumParticleSystem);
		overUICoinFlyoffParticleSystem = UnityEngine.Object.Instantiate(Prefabs.overUICoinFlyoffParticleSystem).GetComponent<ParticleSystem>();
		allParticleSystems.Add(overUICoinFlyoffParticleSystem);
		OverUIAmmoBarFlyoffParticleSystem = UnityEngine.Object.Instantiate(Prefabs.OverUIAmmoBarFlyoffParticleSystemEffect).GetComponent<ParticleSystem>();
		allParticleSystems.Add(OverUIAmmoBarFlyoffParticleSystem);
		OverUIHPBarFlyoffParticleSystem = UnityEngine.Object.Instantiate(Prefabs.OverUIHPBarFlyoffParticleSystemEffect).GetComponent<ParticleSystem>();
		allParticleSystems.Add(OverUIHPBarFlyoffParticleSystem);
		fireworksParticleSystem = UnityEngine.Object.Instantiate(Prefabs.LevelCompleteFireworks).GetComponent<ParticleSystem>();
		allParticleSystems.Add(fireworksParticleSystem);
		LevelCompleteFireworksFlareParticleSystem = UnityEngine.Object.Instantiate(Prefabs.LevelCompleteFireworksFlare).GetComponent<ParticleSystem>();
		allParticleSystems.Add(LevelCompleteFireworksFlareParticleSystem);
	}

	public void Update()
	{
		if (snowParticleSystem != null)
		{
			if (!snowParticleSystemCameraOffset.HasValue)
			{
				snowParticleSystemCameraOffset = snowParticleSystem.transform.position - Camera.main.transform.position;
			}
			snowParticleSystem.transform.position = Camera.main.transform.position + snowParticleSystemCameraOffset.Value;
			if (GameplayCommons.Instance.levelStateController.GameplayStopped && GameplayCommons.Instance.playersTankController.PlayerDead)
			{
				UnityEngine.Object.Destroy(snowParticleSystem);
			}
		}
		if (rainDropParticleSystem != null)
		{
			SoundManager.instance.UdateRainSound();
			if (Time.timeScale == 1f && UnityEngine.Random.value > 0.998f)
			{
				GameplayCommons.Instance.gameplayUIController.FlashScreen();
				SoundManager.instance.PlayThunderSound();
			}
			if (!rainDropParticleSystemCameraOffset.HasValue)
			{
				rainDropParticleSystemCameraOffset = rainDropParticleSystem.transform.position - Camera.main.transform.position;
				rainFallParticleSystemCameraOffset = rainFallParticleSystem.transform.position - Camera.main.transform.position;
			}
			rainDropParticleSystem.transform.position = Camera.main.transform.position + rainDropParticleSystemCameraOffset.Value;
			rainFallParticleSystem.transform.position = Camera.main.transform.position + rainFallParticleSystemCameraOffset.Value;
			if (GameplayCommons.Instance.levelStateController.GameplayStopped && GameplayCommons.Instance.playersTankController.PlayerDead)
			{
				UnityEngine.Object.Destroy(rainDropParticleSystem);
				UnityEngine.Object.Destroy(rainFallParticleSystem);
			}
		}
	}

	private Vector3 GetRandomWeatherElementSpawnPosition(Vector2 offset)
	{
		Vector3 position = Camera.main.transform.position;
		float x = position.x + offset.x - GlobalCommons.Instance.DynamicHorizontalScreenBorderPlusOneCell + GlobalCommons.Instance.DynamicHorizontalScreenBorderPlusOneCell * UnityEngine.Random.Range(0f, 2f);
		Vector3 position2 = Camera.main.transform.position;
		return new Vector3(x, position2.y + offset.y - GlobalCommons.Instance.DynamicVerticalScreenBorderDistancePlusOneCell + GlobalCommons.Instance.DynamicVerticalScreenBorderDistancePlusOneCell * UnityEngine.Random.Range(0f, 2f), 0f);
	}

	public void DisableAllParticles()
	{
		disableEffects = true;
		if (allParticleSystems == null)
		{
			return;
		}
		for (int i = 0; i < allParticleSystems.Count; i++)
		{
			ParticleSystem particleSystem = allParticleSystems[i];
			if (particleSystem != null)
			{
				particleSystem.gameObject.SetActive(value: false);
			}
		}
	}

	public void CreateExplosionEffect(Vector3 coords, float radius = 1f, bool playSound = true, bool spawnDarkSmoke = true)
	{
		if (SceneManager.GetActiveScene().name == "Gameplay")
		{
			for (int num = GameplayCommons.Instance.enemiesTracker.AllBushes.Count - 1; num >= 0; num--)
			{
				BushController bushController = GameplayCommons.Instance.enemiesTracker.AllBushes[num];
				if (Vector2.Distance(coords, bushController.transform.position) < 1.33f)
				{
					bushController.DestroyBush();
				}
			}
		}
		if (!DisableEffects)
		{
			if (spawnDarkSmoke)
			{
				ExplosionEffect_DarkSmokeParticleSystem.transform.position = coords;
				ExplosionEffect_DarkSmokeParticleSystem.Emit(6);
			}
			ExplosionEffect_FlashParticleSystem.transform.position = coords;
			ExplosionEffect_FlashParticleSystem.Emit(1);
			ExplosionEffect_SparksParticleSystem.transform.position = coords;
			ExplosionEffect_SparksParticleSystem.Emit(10);
			if (playSound)
			{
				SoundManager.instance.PlayExplosionSound();
			}
		}
	}

	public static bool IsFarFromCamera(Vector3 coords)
	{
		if (!GameplayCommons.Instance.playersTankController)
		{
			return false;
		}
		Vector3 position = Camera.main.transform.position;
		if (Mathf.Abs(position.x - coords.x) < GlobalCommons.Instance.DynamicHorizontalScreenBorderPlusOneCell)
		{
			Vector3 position2 = Camera.main.transform.position;
			if (Mathf.Abs(position2.y - coords.y) < GlobalCommons.Instance.DynamicVerticalScreenBorderDistancePlusOneCell)
			{
				return false;
			}
		}
		return true;
	}

	public void CreateSpawnerSpawnEffect(Vector3 coords, bool ignoreChecks = false)
	{
		if (!DisableEffects && (ignoreChecks || (!IsFarFromCamera(coords) && !GameplayCommons.Instance.visibilityController.CheckCoverNearPoint(coords))))
		{
			SpawnerSpawnEffect_FlashParticleSystem.transform.position = coords;
			SpawnerSpawnEffect_FlashParticleSystem.Emit(1);
			SpawnerSpawnEffect_SparksParticleSystem.transform.position = coords;
			SpawnerSpawnEffect_SparksParticleSystem.Emit(6);
		}
	}

	public void CreateShotFlareEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			shotFlareParticleSystem.transform.position = coords;
			shotFlareParticleSystem.Emit(1);
		}
	}

	public void CreateDarkSmokeEffect(Vector3 coords, int count, float scatterRadius)
	{
		if (!DisableEffects)
		{
			darkSmokeParticleSystem.transform.position = coords;
			darkSmokeParticleSystem.emission.SetBursts(new ParticleSystem.Burst[1]
			{
				new ParticleSystem.Burst(0f, (short)count)
			});
			ParticleSystem.ShapeModule shape = darkSmokeParticleSystem.shape;
			if (scatterRadius < 0.01f)
			{
				scatterRadius = 0.01f;
			}
			shape.radius = scatterRadius;
			darkSmokeParticleSystem.Emit(count);
		}
	}

	public void CreateLeavesFallEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			BushLeaveParticleSystem.transform.position = coords;
			BushLeaveParticleSystem.Emit(UnityEngine.Random.Range(12, 16));
		}
	}

	public void SpawnSmoke(Vector3 coords, int count, float scatterRadius)
	{
		if (!DisableEffects)
		{
			smokeEffectParticleSystem.transform.position = coords;
			smokeEffectParticleSystem.emission.SetBursts(new ParticleSystem.Burst[1]
			{
				new ParticleSystem.Burst(0f, (short)count)
			});
			ParticleSystem.ShapeModule shape = smokeEffectParticleSystem.shape;
			if (scatterRadius < 0.01f)
			{
				scatterRadius = 0.01f;
			}
			shape.radius = scatterRadius;
			smokeEffectParticleSystem.Emit(count);
		}
	}

	public void SpawnWallShards(Vector3 coords)
	{
		if (!DisableEffects)
		{
			wallShardParticleSystem.transform.position = coords;
			wallShardParticleSystem.Emit(5);
		}
	}

	public void SpawnMissileLaunchSmokeParticles(Vector3 coords, Quaternion rotation)
	{
		if (!DisableEffects)
		{
			MissileLaunchSmokeParticleSystem.transform.position = coords;
			MissileLaunchSmokeParticleSystem.transform.rotation = rotation;
			MissileLaunchSmokeParticleSystem.Emit(5);
		}
	}

	public void SpawnSpecialWallShards(Vector3 coords)
	{
		if (!DisableEffects)
		{
			specialWallShardParticleSystem.transform.position = coords;
			specialWallShardParticleSystem.Emit(5);
		}
	}

	public void SpawnPurchaseEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			purchaseParticleSystem.transform.position = coords;
			purchaseParticleSystem.Emit(100);
			CreateFlareEffect(coords);
		}
	}

	public void SpawnWireSparksEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			wireSparkParticleSystem.transform.position = coords;
			wireSparkParticleSystem.Emit(UnityEngine.Random.Range(7, 11));
			UpgradesSparkFlareParticleSystem.transform.position = coords;
			UpgradesSparkFlareParticleSystem.Emit(1);
		}
	}

	public void SpawnHarderWallShards(Vector3 coords)
	{
		if (!DisableEffects)
		{
			harderWallShardParticleSystem.transform.position = coords;
			harderWallShardParticleSystem.Emit(5);
		}
	}

	public void SpawnCrateShards(Vector3 coords)
	{
		if (!DisableEffects)
		{
			crateShardParticleSystem.transform.position = coords;
			crateShardParticleSystem.Emit(5);
		}
	}

	public void SpawnBulletHitSparksEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			BulletHitSparksEffectParticleSystem.transform.position = coords;
			BulletHitSparksEffectParticleSystem.Emit(UnityEngine.Random.Range(2, 5));
		}
	}

	public void SpawnProjectileSleevesEffect(Transform transform, SleeveType sleeveType)
	{
		if (!DisableEffects)
		{
			ParticleSystem particleSystem;
			switch (sleeveType)
			{
			case SleeveType.Cannon:
				particleSystem = bulletSleevesCannonParticleSystem;
				break;
			case SleeveType.Machinegun:
				particleSystem = bulletSleevesMachinegunParticleSystem;
				break;
			case SleeveType.Minigun:
				particleSystem = bulletSleevesMinigunParticleSystem;
				break;
			case SleeveType.Shotgun:
				particleSystem = bulletSleevesShotgunParticleSystem;
				break;
			case SleeveType.Blaster:
				particleSystem = bulletSleevesBlasterParticleSystem;
				break;
			default:
				throw new Exception("unknown sleeve type");
			}
			particleSystem.transform.position = transform.position;
			Transform transform2 = particleSystem.transform;
			Vector3 eulerAngles = transform.rotation.eulerAngles;
			transform2.rotation = Quaternion.Euler(0f - eulerAngles.z, 90f, 0f);
			particleSystem.Emit(1);
		}
	}

	public void CreateHitEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			hitEffectParticleSystem.transform.position = coords;
			hitEffectParticleSystem.Emit(1);
		}
	}

	public void CreateEmotionEffect(EmotionEffectType effectType, Vector3 coords, EnemyTankController etc)
	{
		bool isEditor = Application.isEditor;
		Vector2 vector;
		if (GameplayCommons.Instance.weaponsController.ActiveGuidedRocket != null)
		{
			Vector3 position = GameplayCommons.Instance.weaponsController.ActiveGuidedRocket.transform.position;
			float x = position.x;
			Vector3 position2 = GameplayCommons.Instance.weaponsController.ActiveGuidedRocket.transform.position;
			vector = new Vector2(x, position2.y);
		}
		else
		{
			Vector3 position3 = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
			float x2 = position3.x;
			Vector3 position4 = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
			vector = new Vector2(x2, position4.y);
		}
		Vector2 vector2 = vector;
		if (Mathf.Abs(coords.x - vector2.x) < GlobalCommons.Instance.DynamicHorizontalCameraBorder && Mathf.Abs(coords.y - vector2.y) < GlobalCommons.Instance.DynamicVerticalCameraBorder && !GameplayCommons.Instance.levelStateController.GameplayStopped && !GameplayCommons.Instance.visibilityController.CheckCoverNearPoint(coords))
		{
			SoundManager.instance.PlayEnemyEmojiSound();
		}
		if (GameplayCommons.Instance.playersTankController.PlayerDead)
		{
			if (enemiesShownSmileys.IndexOf(etc) == -1)
			{
				GameplayCommons.Instance.StartCoroutine(CreateSmileyEffect(etc));
				enemiesShownSmileys.Add(etc);
			}
			return;
		}
		switch (effectType)
		{
		case EmotionEffectType.Exclamation:
			CreateExclamationMarkEffect(coords);
			break;
		case EmotionEffectType.Question:
			CreateQuestionMarkEffect(coords);
			break;
		case EmotionEffectType.Threedots:
			CreateThreeDotsEffect(coords);
			break;
		}
	}

	private void CreateExclamationMarkEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			exclamationMarkEffectParticleSystem.transform.position = coords;
			exclamationMarkEffectParticleSystem.Emit(1);
		}
	}

	private IEnumerator CreateSmileyEffect(EnemyTankController etc)
	{
		yield return new WaitForSecondsRealtime(UnityEngine.Random.Range(0.2f, 0.85f));
		if (smileyEffectParticleSystem != null && etc != null)
		{
			Transform transform = smileyEffectParticleSystem.transform;
			Vector3 position = etc.TankBase.transform.position;
			float x = position.x;
			Vector3 position2 = etc.TankBase.transform.position;
			float y = position2.y + GlobalCommons.Instance.gridSize / 2f;
			Vector3 position3 = etc.TankBase.transform.position;
			transform.position = new Vector3(x, y, position3.z);
			smileyEffectParticleSystem.Emit(1);
		}
	}

	private void CreateQuestionMarkEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			questionMarkEffectParticleSystem.transform.position = coords;
			questionMarkEffectParticleSystem.Emit(1);
		}
	}

	private void CreateThreeDotsEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			threedotsEffectParticleSystem.transform.position = coords;
			threedotsEffectParticleSystem.Emit(1);
		}
	}

	public void CreateTankExplodedEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			enemyExplosion1ParticleSystem.transform.position = coords;
			enemyExplosion1ParticleSystem.Emit(UnityEngine.Random.Range(2, 5));
		}
	}

	public void CreateSpawnerExplodedEffect(Vector3 coords, int count = -1)
	{
		if (!DisableEffects)
		{
			int count2 = (count <= 0) ? UnityEngine.Random.Range(5, 6) : count;
			explodedSpawnerParticleSystem.transform.position = coords;
			explodedSpawnerParticleSystem.Emit(count2);
		}
	}

	public void CreateRailgunEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			railgunEffectParticleSystem.transform.position = coords;
			railgunEffectParticleSystem.Emit(1);
		}
	}

	public void CreateLaserHitEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			laserHitParticleSystem.transform.position = coords;
			laserHitParticleSystem.Emit(1);
		}
	}

	public void CreateTracksDirtEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			tracksDirtParticleSystem.transform.position = coords;
			tracksDirtParticleSystem.Emit(1);
		}
	}

	public void SpawnBulletDisappearEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			bulletDisappearParticleSystem.transform.position = coords;
			bulletDisappearParticleSystem.Emit(1);
		}
	}

	public void SpawnUpgradeAvailableIndicatorFalloff(Vector3 coords)
	{
		if (!DisableEffects)
		{
			UpgradeAvailableFalloffParticleSystem.transform.position = coords;
			UpgradeAvailableFalloffParticleSystem.Emit(1);
		}
	}

	public void SpawnOverUIFlare(Vector3 coords)
	{
		if (!DisableEffects)
		{
			overUIFlareParticleSystem.transform.position = coords;
			overUIFlareParticleSystem.Emit(1);
		}
	}

	public void SpawnOverUIHPBarFlyoffEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			OverUIHPBarFlyoffParticleSystem.transform.position = coords;
			float cameraDefaultScaleCoefficient = GameplayCommons.Instance.cameraController.GetCameraDefaultScaleCoefficient();
			OverUIHPBarFlyoffParticleSystem.transform.localScale = new Vector3(cameraDefaultScaleCoefficient, cameraDefaultScaleCoefficient, cameraDefaultScaleCoefficient);
			OverUIHPBarFlyoffParticleSystem.Emit(1);
		}
	}

	public void SpawnOverUIAmmoBarFlyoffEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			OverUIAmmoBarFlyoffParticleSystem.transform.position = coords;
			float cameraDefaultScaleCoefficient = GameplayCommons.Instance.cameraController.GetCameraDefaultScaleCoefficient();
			OverUIAmmoBarFlyoffParticleSystem.transform.localScale = new Vector3(cameraDefaultScaleCoefficient, cameraDefaultScaleCoefficient, cameraDefaultScaleCoefficient);
			OverUIAmmoBarFlyoffParticleSystem.Emit(1);
		}
	}

	public void SpawnOverUICoinFlyoffEffect(Vector3 coords, float xScale = 1f)
	{
		if (!DisableEffects)
		{
			//overUICoinFlyoffParticleSystem.shape.scale = new Vector3(2.316746f * xScale, 1f, 0.4561931f);
			Transform transform = overUICoinFlyoffParticleSystem.transform;
			Vector3 eulerAngles = Camera.main.transform.rotation.eulerAngles;
			transform.rotation = Quaternion.Euler(0f, 0f, eulerAngles.z);
			if (!overUIcoinSizeSet)
			{
				ParticleSystem.MinMaxCurve startSize = overUICoinFlyoffParticleSystem.main.startSize;
				startSize.constant = Camera.main.orthographicSize / 5f * 0.356f;
				//overUICoinFlyoffParticleSystem.main.startSize = startSize;
				overUIcoinSizeSet = true;
			}
			overUICoinFlyoffParticleSystem.transform.position = coords;
			if (SceneManager.GetActiveScene().name == "Gameplay" && GameplayCommons.Instance.playersTankController.PlayerDead)
			{
				float cameraScaleCoefficient = GameplayCommons.Instance.cameraController.GetCameraScaleCoefficient();
				overUICoinFlyoffParticleSystem.transform.localScale = new Vector3(cameraScaleCoefficient, cameraScaleCoefficient, cameraScaleCoefficient);
			}
			overUICoinFlyoffParticleSystem.Emit(1);
		}
	}

	public void SpawnPrizeCoinPickupEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			PrizeCoinPickupParticleSystem.transform.position = coords;
			PrizeCoinPickupParticleSystem.Emit(UnityEngine.Random.Range(2, 6));
		}
	}

	public void SpawnFireworksEffect(Vector3 coords)
	{
		if (!DisableEffects)
		{
			Color startColor = Color.red;
			switch (UnityEngine.Random.Range(0, 5))
			{
			case 0:
				startColor = Color.red;
				break;
			case 1:
				startColor = Color.green;
				break;
			case 2:
				startColor = Color.cyan;
				break;
			case 3:
				startColor = Color.magenta;
				break;
			case 4:
				startColor = Color.yellow;
				break;
			}
			float num = UnityEngine.Random.Range(0.5f, 0.8f);
			fireworksParticleSystem.transform.localScale = new Vector3(num, num, num);
			fireworksParticleSystem.gameObject.SetActive(value: true);
			fireworksParticleSystem.startColor = startColor;
			fireworksParticleSystem.transform.position = coords;
			fireworksParticleSystem.Emit(100);
			CreateFlareEffect(coords);
		}
	}

	public void CreateFlareEffect(Vector3 coords, float scaleFactor = 1f)
	{
		LevelCompleteFireworksFlareParticleSystem.transform.position = coords;
		LevelCompleteFireworksFlareParticleSystem.Emit(1);
	}

	public void SpawnCoinFlyoffEffect(Vector3 coords, BonusController.BonusType coinType)
	{
		if (!DisableEffects)
		{
			CreateHitEffect(coords);
			switch (coinType)
			{
			case BonusController.BonusType.bronzeCoinBonus:
				coinFlyoffBronzeParticleSystem.transform.position = coords;
				coinFlyoffBronzeParticleSystem.Emit(1);
				break;
			case BonusController.BonusType.goldenCoinBonus:
				coinFlyoffGoldParticleSystem.transform.position = coords;
				coinFlyoffGoldParticleSystem.Emit(1);
				break;
			case BonusController.BonusType.platinumCoinBonus:
				coinFlyoffPlatinumParticleSystem.transform.position = coords;
				coinFlyoffPlatinumParticleSystem.Emit(1);
				break;
			}
		}
	}

	public void SpawnExplosionDecal(Vector3 coords)
	{
		if (!DisableEffects)
		{
			GameObject gameObject;
			if ((bool)GameplayCommons.Instance)
			{
				gameObject = UnityEngine.Object.Instantiate(Prefabs.explosionDecalPrefab, new Vector3(coords.x, coords.y, GameplayCommons.Instance.explosionDecalZIndex), Quaternion.identity);
				GameplayCommons.Instance.explosionDecalZIndex -= 0.001f;
			}
			else
			{
				gameObject = UnityEngine.Object.Instantiate(Prefabs.explosionDecalPrefab, new Vector3(coords.x, coords.y, 0f), Quaternion.identity);
			}
			float num = UnityEngine.Random.Range(0.75f, 1f);
			Transform transform = gameObject.transform;
			float x = num;
			float y = num;
			Vector3 localScale = gameObject.transform.localScale;
			transform.localScale = new Vector3(x, y, localScale.z);
		}
	}
}
