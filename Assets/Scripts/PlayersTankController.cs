using UnityEngine;

public class PlayersTankController : MonoBehaviour
{
	public const float LOW_PLAYER_HP_TRESHHOLD = 0.25f;

	public const float EXTREMELY_LOW_PLAYER_HP_TRESHHOLD = 0.1f;

	private Rigidbody2D baseRB;

	private GameObject tankBase;

	private GameObject tankTurret;

	private SpriteRenderer tankTurretSR;

	private SpriteRenderer tankBaseSR;

	private bool objectiveCompletePanelShown;

	public float hitPoints;

	private float hitPointsMax;

	private float deathTimeStamp;

	private float turretShiftFactor;

	private int slowdownTicker;

	private float movementSpeedFactor;

	private bool machinegunBarreldTicker;

	private int dirtSpewTicker;

	internal bool PlayerActive;

	private float tracksAnimationTicker;

	private float tracksAnimationTickerMax = 0.05f;

	public Sprite[] PlayerTurretsSprites;

	public Sprite[] PlayerBaseSprites;

	public Sprite[] PlayerTurretsFlashSprites;

	public Sprite PlayerBaseFlashSprite;

	public Sprite PlayersBaseDestroyedSprite;

	private int flashFrames;

	private int flashFramesMax = GlobalCommons.FlashFrameCount;

	private float lastTimeEmittedSmoke;

	private float smokeEmitTimeout = 0.08f;

	private float initialTurretScale;

	private float damageMultiplier = 1f;

	public float MaxHP => hitPointsMax;

	public float HPPercentage => hitPoints / hitPointsMax;

	//public bool PlayerDead => hitPoints <= 0f;
	public bool PlayerDead;

	public GameObject TankBase => tankBase;

	public GameObject TankTurret => tankTurret;

	private void Start()
	{
		lastTimeEmittedSmoke = Time.fixedTime;
		Object.Instantiate(Prefabs.aimingLinePrefab).GetComponent<AimingLineController>();
		hitPoints = (hitPointsMax = PlayerBalance.armorUpgradeValues[GlobalCommons.Instance.globalGameStats.TankArmorLevel]);
		tankBase = UnityEngine.Object.Instantiate(Prefabs.playerTankBasePrefab, base.transform.position, Quaternion.identity);
		tankTurret = UnityEngine.Object.Instantiate(Prefabs.playerTurretPrefab, base.transform.position, Quaternion.identity);
		tankTurretSR = tankTurret.GetComponent<SpriteRenderer>();
		tankBaseSR = tankBase.GetComponent<SpriteRenderer>();
		tankBase.transform.parent = base.transform;
		tankTurret.transform.parent = base.transform;
		BodyRandomRotationSetter.RandomlyRotate(tankBase);
		BodyRandomRotationSetter.RandomlyRotate(tankTurret);
		baseRB = tankBase.GetComponent<Rigidbody2D>();
		movementSpeedFactor = PlayerBalance.speedUpgradeValues[GlobalCommons.Instance.globalGameStats.TankSpeedLevel];
		BoxCollider2D component = tankBase.GetComponent<BoxCollider2D>();
		BoxCollider2D boxCollider2D = component;
		Vector2 size = component.size;
		float x = size.x * 0.833f;
		Vector2 size2 = component.size;
		boxCollider2D.size = new Vector2(x, size2.y * 0.833f);
		SetTurretSprite();
		SetAlpha(0f);
		Vector3 localScale = tankTurretSR.transform.localScale;
		initialTurretScale = localScale.x;
		damageMultiplier = ProgressIndicatorHelper.GetDamageMultiplier();
	}

	public void GainFullHealth()
	{
		hitPoints = (hitPointsMax = PlayerBalance.armorUpgradeValues[GlobalCommons.Instance.globalGameStats.TankArmorLevel]);
		GameplayCommons.Instance.gameplayUIController.UpdatePlayerHP(HPPercentage);
	}

	public float GetPlayerDamageMultiplier()
	{
		return damageMultiplier;
	}

	internal void ActivatePlayer()
	{
		SetAlpha(1f);
		GameplayCommons.Instance.effectsSpawner.CreateSpawnerSpawnEffect(tankBase.transform.position);
		PlayerActive = true;
		GameplayCommons.Instance.playerActivationTime = Time.fixedTime;
	}

	internal void SetAlpha(float val)
	{
		tankBaseSR.SetAlpha(val);
		tankTurretSR.SetAlpha(val);
	}

	public Vector2 GetTankVelocity()
	{
		return baseRB.velocity;
	}

	public void SetTurretSprite(bool doBounce = false)
	{
		Sprite sprite = PlayerTurretsSprites[(int)GameplayCommons.Instance.weaponsController.SelectedWeapon];
		if (tankTurretSR.sprite != sprite)
		{
			tankTurretSR.sprite = sprite;
		}
		if (doBounce)
		{
			float num = initialTurretScale + 0.3f;
			tankTurretSR.transform.localScale = new Vector3(num, num, num);
		}
	}

	public void SetTurretFlashSprite()
	{
		tankTurretSR.sprite = PlayerTurretsFlashSprites[(int)GameplayCommons.Instance.weaponsController.SelectedWeapon];
	}

	private void Update()
	{
		UpdateTurretPosition();
		EmitSmoke();
		BushController.CheckPlayerHidden();
		Vector3 localScale = tankTurretSR.transform.localScale;
		if (localScale.x != initialTurretScale)
		{
			Vector3 localScale2 = tankTurretSR.transform.localScale;
			float num = FMath.MoveFloatStepClamp(localScale2.x, initialTurretScale, 0.05f);
			tankTurretSR.transform.localScale = new Vector3(num, num, num);
		}
		if (PlayerActive)
		{
			SoundManager.instance.UpdateEngineSound(baseRB.velocity.sqrMagnitude > 5f);
		}
		if (PlayerDead)
		{
			if (GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.TutorialLevel && !objectiveCompletePanelShown && Time.fixedTime - deathTimeStamp > 0.2f)
			{
				objectiveCompletePanelShown = true;
				if (GlobalCommons.Instance.globalGameStats.LittleTargetsTracker.ProcessAfterLevelCheck())
				{
					GameObject.Find("ObjectiveCompletePanelController").GetComponent<ObjectiveCompletePanelController>().Activate();
				}
			}
			if (!GameplayCommons.Instance.levelStateController.GameplayStopped && Time.fixedTime - deathTimeStamp >= 2f)
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.SecondsSpentPlaying, Mathf.CeilToInt(Time.timeSinceLevelLoad));
				GameplayCommons.Instance.levelStateController.ShowLevelResultsPanel();
			}
		}
		else
		{
			if (UpdateVelocity())
			{
				RotateGameobjectToVector(GameplayCommons.Instance.touchesController.ShootingVector, tankTurret);
			}
			if (flashFrames > 0)
			{
				ProcessPlayerFlash();
			}
			else
			{
				AnimateTracks();
			}
		}
	}

	private void ProcessPlayerFlash()
	{
		flashFrames--;
		if (flashFrames > 0)
		{
			if (flashFrames == flashFramesMax - 1)
			{
				tankBaseSR.sprite = PlayerBaseFlashSprite;
				SetTurretFlashSprite();
			}
		}
		else
		{
			tankBaseSR.sprite = PlayerBaseSprites[0];
			SetTurretSprite();
		}
	}

	private void AnimateTracks()
	{
		Vector2 velocity = baseRB.velocity;
		if (velocity != Vector2.zero)
		{
			tracksAnimationTicker += Time.deltaTime;
		}
		if (tracksAnimationTicker > tracksAnimationTickerMax)
		{
			do
			{
				tracksAnimationTicker -= tracksAnimationTickerMax;
			}
			while (tracksAnimationTicker > tracksAnimationTickerMax);
			if (tankBaseSR.sprite == PlayerBaseSprites[0])
			{
				tankBaseSR.sprite = PlayerBaseSprites[1];
			}
			else
			{
				tankBaseSR.sprite = PlayerBaseSprites[0];
			}
		}
	}

	public void PickupHealthBonus()
	{
		if (!PlayerDead)
		{
			hitPoints += hitPointsMax / 3.86f;
			if (hitPoints > hitPointsMax)
			{
				hitPoints = hitPointsMax;
			}
			GameplayCommons.Instance.gameplayUIController.UpdatePlayerHP(HPPercentage);
		}
	}

	private void EmitSmoke()
	{
		if (hitPoints / hitPointsMax <= 0.25f && Time.fixedTime - lastTimeEmittedSmoke >= smokeEmitTimeout)
		{
			lastTimeEmittedSmoke = Time.fixedTime;
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(tankBase.transform.position, 1, 0f);
		}
	}

	private bool UpdateVelocity()
	{
		if (GameplayCommons.Instance.weaponsController.ActiveGuidedRocket != null || GameplayCommons.Instance.weaponsController.GuidedRocketAftershotPeriodActive || GameplayCommons.Instance.levelStateController.GameplayStopped || !PlayerActive)
		{
			return false;
		}
		baseRB.velocity = GameplayCommons.Instance.touchesController.MovementVector * movementSpeedFactor;
		if (slowdownTicker > 0)
		{
			baseRB.velocity /= 2f;
			slowdownTicker--;
		}
		return true;
	}

	private void UpdateTurretPosition()
	{
		Vector3 vector = tankBase.transform.position;
		if (turretShiftFactor > 0f)
		{
			Vector3 a = Quaternion.Euler(0f, 0f, 90f) * tankTurret.transform.right;
			a.Normalize();
			a *= 0f - turretShiftFactor;
			vector += a;
			turretShiftFactor -= 1.25f * Time.deltaTime;
			if (turretShiftFactor < 0f)
			{
				turretShiftFactor = 0f;
			}
		}
		tankTurret.transform.position = vector;
	}

	public Vector3 GetBarrelPoint()
	{
		Quaternion rotation;
		if (GameplayCommons.Instance.weaponsController.SelectedWeapon == WeaponTypes.machinegun)
		{
			float z = (!machinegunBarreldTicker) ? 77f : 103f;
			rotation = Quaternion.Euler(0f, 0f, z);
			machinegunBarreldTicker = !machinegunBarreldTicker;
		}
		else
		{
			rotation = Quaternion.Euler(0f, 0f, 90f);
		}
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
			a *= 0.5f;
			Vector3 position = tankBase.transform.position;
			float x = position.x + a.x;
			Vector3 position2 = tankBase.transform.position;
			float y = position2.y + a.y;
			Vector3 position3 = tankBase.transform.position;
			Vector3 coords = new Vector3(x, y, position3.z);
			GameplayCommons.Instance.effectsSpawner.CreateTracksDirtEffect(coords);
		}
	}

	public void SetMaxTurretShiftFactor()
	{
		turretShiftFactor = 0.1f;
	}

	public void ApplyDamage(float amount, DamageTypes damageType)
	{
		if (hitPoints <= 0f || false || GameplayCommons.Instance.levelStateController.LevelCompletionPending)
		{
			return;
		}
		if (damageType != DamageTypes.bombBonusDamage && damageType != DamageTypes.explosiveBarrelDamage && damageType != DamageTypes.suicideEnemyDamage)
		{
			amount *= damageMultiplier;
		}
		flashFrames = flashFramesMax;
		if (damageType != DamageTypes.laserDamage)
		{
			SoundManager.instance.PlayEnemyHitSound(damageType);
		}
		if (GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.TutorialLevel)
		{
			hitPoints -= amount;
		}
		if (hitPoints <= 0f)
		{
			if (AdmobManager.RewardAdAvailable)
			{
				AdsToRevive.instance.OpenReviveUI();
			}
			else
			{
				GameplayCommons.Instance.playersTankController.PlayerDead = true;
				GameplayCommons.Instance.playersTankController.DestroyPlayerTank();
			}

			hitPoints = 0f;
		}
		GameplayCommons.Instance.gameplayUIController.UpdatePlayerHP(HPPercentage);
		if (hitPoints == 0f)
		{

			//SoundManager.instance.ToggleMusic(SoundManager.MusicType.JingleMusic);
			//tankTurretSR.enabled = false;
			//SpriteRenderer component = tankBase.GetComponent<SpriteRenderer>();
			//component.sprite = PlayersBaseDestroyedSprite;
			//component.sortingLayerName = "TankMovePointer";
			//GameplayCommons.Instance.cameraController.ShakeCamera(9f);
			//deathTimeStamp = Time.fixedTime;
			//GameplayCommons.Instance.effectsSpawner.CreateExplosionEffect(tankBase.transform.position);
			//GameplayCommons.Instance.effectsSpawner.SpawnSmoke(tankBase.transform.position, 5, 0.6f);
			//GameplayCommons.Instance.effectsSpawner.SpawnExplosionDecal(tankBase.transform.position);
			//WaveExploPostProcessing.ShowEffectAt(Camera.main.WorldToScreenPoint(tankBase.transform.position));
			//GameplayCommons.Instance.levelStateController.HidePlayerFromAllEnemies();
			//if (GameplayCommons.Instance.weaponsController.ActiveGuidedRocket != null)
			//{
			//	GameplayCommons.Instance.weaponsController.ActiveGuidedRocket.DestroyRocket();
			//}
			//Vector3 position = tankBase.transform.position;
			//float x = position.x + GlobalCommons.Instance.gridSize / 4f;
			//Vector3 position2 = tankBase.transform.position;
			//float y = position2.y;
			//Vector3 position3 = tankBase.transform.position;
			//Vector3 coords = new Vector3(x, y, position3.z);
			//Vector3 position4 = tankBase.transform.position;
			//float x2 = position4.x - GlobalCommons.Instance.gridSize / 4f;
			//Vector3 position5 = tankBase.transform.position;
			//float y2 = position5.y;
			//Vector3 position6 = tankBase.transform.position;
			//Vector3 coords2 = new Vector3(x2, y2, position6.z);
			//Vector3 position7 = tankBase.transform.position;
			//float x3 = position7.x;
			//Vector3 position8 = tankBase.transform.position;
			//float y3 = position8.y + GlobalCommons.Instance.gridSize / 4f;
			//Vector3 position9 = tankBase.transform.position;
			//Vector3 coords3 = new Vector3(x3, y3, position9.z);
			//Vector3 position10 = tankBase.transform.position;
			//float x4 = position10.x;
			//Vector3 position11 = tankBase.transform.position;
			//float y4 = position11.y - GlobalCommons.Instance.gridSize / 4f;
			//Vector3 position12 = tankBase.transform.position;
			//Vector3 coords4 = new Vector3(x4, y4, position12.z);
			//BonusesSpawner.SpawnCoinsBonus(coords, UnityEngine.Random.Range(10, 150));
			//BonusesSpawner.SpawnCoinsBonus(coords2, UnityEngine.Random.Range(10, 150));
			//BonusesSpawner.SpawnCoinsBonus(coords3, UnityEngine.Random.Range(10, 150));
			//BonusesSpawner.SpawnCoinsBonus(coords4, UnityEngine.Random.Range(10, 150));
		}
	}

	public void DestroyPlayerTank()
	{
		SoundManager.instance.ToggleMusic(SoundManager.MusicType.JingleMusic);
		tankTurretSR.enabled = false;
		SpriteRenderer component = tankBase.GetComponent<SpriteRenderer>();
		component.sprite = PlayersBaseDestroyedSprite;
		component.sortingLayerName = "TankMovePointer";
		GameplayCommons.Instance.cameraController.ShakeCamera(9f);
		deathTimeStamp = Time.fixedTime;
		GameplayCommons.Instance.effectsSpawner.CreateExplosionEffect(tankBase.transform.position);
		GameplayCommons.Instance.effectsSpawner.SpawnSmoke(tankBase.transform.position, 5, 0.6f);
		GameplayCommons.Instance.effectsSpawner.SpawnExplosionDecal(tankBase.transform.position);
		WaveExploPostProcessing.ShowEffectAt(Camera.main.WorldToScreenPoint(tankBase.transform.position));
		GameplayCommons.Instance.levelStateController.HidePlayerFromAllEnemies();
		if (GameplayCommons.Instance.weaponsController.ActiveGuidedRocket != null)
		{
			GameplayCommons.Instance.weaponsController.ActiveGuidedRocket.DestroyRocket();
		}
		Vector3 position = tankBase.transform.position;
		float x = position.x + GlobalCommons.Instance.gridSize / 4f;
		Vector3 position2 = tankBase.transform.position;
		float y = position2.y;
		Vector3 position3 = tankBase.transform.position;
		Vector3 coords = new Vector3(x, y, position3.z);
		Vector3 position4 = tankBase.transform.position;
		float x2 = position4.x - GlobalCommons.Instance.gridSize / 4f;
		Vector3 position5 = tankBase.transform.position;
		float y2 = position5.y;
		Vector3 position6 = tankBase.transform.position;
		Vector3 coords2 = new Vector3(x2, y2, position6.z);
		Vector3 position7 = tankBase.transform.position;
		float x3 = position7.x;
		Vector3 position8 = tankBase.transform.position;
		float y3 = position8.y + GlobalCommons.Instance.gridSize / 4f;
		Vector3 position9 = tankBase.transform.position;
		Vector3 coords3 = new Vector3(x3, y3, position9.z);
		Vector3 position10 = tankBase.transform.position;
		float x4 = position10.x;
		Vector3 position11 = tankBase.transform.position;
		float y4 = position11.y - GlobalCommons.Instance.gridSize / 4f;
		Vector3 position12 = tankBase.transform.position;
		Vector3 coords4 = new Vector3(x4, y4, position12.z);
		BonusesSpawner.SpawnCoinsBonus(coords, UnityEngine.Random.Range(10, 150));
		BonusesSpawner.SpawnCoinsBonus(coords2, UnityEngine.Random.Range(10, 150));
		BonusesSpawner.SpawnCoinsBonus(coords3, UnityEngine.Random.Range(10, 150));
		BonusesSpawner.SpawnCoinsBonus(coords4, UnityEngine.Random.Range(10, 150));
	}

	private void FixedUpdate()
	{
		if (!PlayerDead && !GameplayCommons.Instance.levelStateController.GameplayStopped && UpdateVelocity())
		{
			RotateBodyToDirection(GameplayCommons.Instance.touchesController.MovementVector, baseRB, 12f);
		}
	}

	private void RotateGameobjectToVector(Vector2 directionVector, GameObject obj)
	{
		float z = Mathf.Atan2(directionVector.y, directionVector.x) * 57.29578f - 90f;
		float num = 30f;
		if (Input.touchSupported)
		{
			if (GameplayCommons.Instance.touchesController.ShootTouchController.IsAutoaiming())
			{
				num = 12f;
				num *= GameplayCommons.Instance.weaponsController.SelectedWeaponController.GetAutoaimTurretRotationSpeedMod();
			}
		}
		else
		{
			num = 12f;
			num *= GameplayCommons.Instance.weaponsController.SelectedWeaponController.GetAutoaimTurretRotationSpeedMod();
		}
		obj.transform.rotation = Quaternion.RotateTowards(obj.transform.rotation, Quaternion.Euler(0f, 0f, z), num * Time.deltaTime * 90f);
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

	public void UpdateCollisionTicker()
	{
		slowdownTicker = 3;
	}
}
