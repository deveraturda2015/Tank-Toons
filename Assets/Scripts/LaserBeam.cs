using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
	private LineRenderer lr;

	private float damagePerSecond = 0.1f;

	private bool playersBeam = true;

	private EnemyTankController enemyTankController;

	private float lastTimeEmittedSmoke;

	private float smokeEmitTimeout = 0.12f;

	private bool sparkEmitFlag;

	private float particleEmitCounter;

	private float particleEmitCounterMax = 4f;

	private bool bossWeapon;

	private float laserBeamWidth = 0.15f;

	private List<SpriteRenderer> laserHitPointSpriteRenderers;

	public bool GetEnabled => lr.enabled;

	public bool BeamEnabled => lr.enabled;

	private void Start()
	{
		laserHitPointSpriteRenderers = new List<SpriteRenderer>();
		for (int i = 0; i < 2; i++)
		{
			SpriteRenderer component = UnityEngine.Object.Instantiate(Prefabs.BeamHitPointPrefab).GetComponent<SpriteRenderer>();
			laserHitPointSpriteRenderers.Add(component);
		}
		lastTimeEmittedSmoke = Time.fixedTime;
		lr = GetComponent<LineRenderer>();
		lr.SetWidth(laserBeamWidth, laserBeamWidth);
		lr.enabled = false;
		lr.sortingLayerName = "LaserRailgunBeams";
		if (playersBeam)
		{
			damagePerSecond = PlayerBalance.LaserDamageValues[GlobalCommons.Instance.globalGameStats.WeaponsLevels[7]];
		}
	}

	public void InitializeEnemyBeam(EnemyTankController etc, bool bossWeapon)
	{
		this.bossWeapon = bossWeapon;
		playersBeam = false;
		enemyTankController = etc;
	}

	public void SetEnabled(bool val)
	{
		if (!val && lr.enabled)
		{
			if (playersBeam)
			{
				GameplayCommons.Instance.effectsSpawner.SpawnSmoke(GameplayCommons.Instance.playersTankController.GetBarrelPoint(), 1, 0f);
			}
			else
			{
				GameplayCommons.Instance.effectsSpawner.SpawnSmoke(enemyTankController.GetBarrelPoint(), 1, 0f);
			}
		}
		if (val && !lr.enabled)
		{
			updatePosition();
		}
		lr.enabled = val;
		laserHitPointSpriteRenderers[0].enabled = val;
		laserHitPointSpriteRenderers[1].enabled = val;
	}

	private void Update()
	{
		if (!playersBeam && enemyTankController == null)
		{
			foreach (SpriteRenderer laserHitPointSpriteRenderer in laserHitPointSpriteRenderers)
			{
				laserHitPointSpriteRenderer.enabled = false;
				laserHitPointSpriteRenderer.gameObject.SetActive(value: false);
			}
			laserHitPointSpriteRenderers = null;
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (lr.enabled)
		{
			updatePosition();
		}
	}

	private void updatePosition()
	{
		Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
		Transform transform = GameplayCommons.Instance.playersTankController.TankTurret.transform;
		LayerMask mask = LayerMasks.obstaclesEnemiesSpawnersDestrObstaclesLayerMask;
		if (!playersBeam)
		{
			transform = enemyTankController.TankTurret.transform;
			mask = LayerMasks.obstaclesPlayerDestrObstaclesLayerMask;
		}
		Vector3 vector = (!playersBeam) ? enemyTankController.GetBarrelPoint() : GameplayCommons.Instance.playersTankController.GetBarrelPoint();
		Vector3 vector2 = rotation * transform.right;
		RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, vector2, 9f, mask);
		Vector3[] array = new Vector3[2]
		{
			vector,
			vector + vector2 * 9f
		};
		bool flag = false;
		if (raycastHit2D.collider != null)
		{
			array[1] = raycastHit2D.point;
			float num = (!playersBeam) ? (EnemiesBalance.GetEnemyDamage(WeaponTypes.laser) * Time.deltaTime) : (damagePerSecond * Time.deltaTime);
			if (bossWeapon)
			{
				num *= EnemiesBalance.bossDamageCoeff;
			}
			if (raycastHit2D.collider.transform.parent != null)
			{
				if (raycastHit2D.collider.gameObject.layer == PhysicsLayers.EnemyTankBase)
				{
					EnemyTankController component = raycastHit2D.collider.transform.parent.GetComponent<EnemyTankController>();
					component.ApplyDamage(num, DamageTypes.laserDamage);
					flag = true;
				}
				else if (raycastHit2D.collider.gameObject.layer == PhysicsLayers.PlayersTankBase)
				{
					PlayersTankController component2 = raycastHit2D.collider.transform.parent.gameObject.GetComponent<PlayersTankController>();
					component2.ApplyDamage(num, DamageTypes.laserDamage);
				}
			}
			else if (raycastHit2D.collider.gameObject.layer == PhysicsLayers.EnemiesSpawner)
			{
				EnemySpawnerController component3 = raycastHit2D.collider.gameObject.GetComponent<EnemySpawnerController>();
				component3.ApplyDamage(num, DamageTypes.laserDamage);
				flag = true;
			}
			else if (raycastHit2D.collider.gameObject.layer == PhysicsLayers.DestructableObstacles)
			{
				DestructableObstacleController component4 = raycastHit2D.collider.gameObject.GetComponent<DestructableObstacleController>();
				component4.ApplyDamage(num, DamageTypes.laserDamage);
				flag = true;
			}
		}
		if (playersBeam)
		{
			if (flag)
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesHit);
			}
			else
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesMissed);
			}
		}
		if (Time.timeScale > 0f)
		{
			float num2 = UnityEngine.Random.Range(0.5f, 1f);
			float num3 = UnityEngine.Random.Range(0.5f, 1f);
			lr.startColor = new Color(num2, num2, num2);
			lr.endColor = new Color(num3, num3, num3);
			lr.startWidth = laserBeamWidth * UnityEngine.Random.Range(0.1f, 2f);
			lr.endWidth = laserBeamWidth * UnityEngine.Random.Range(0.1f, 2f);
		}
		if (particleEmitCounter == 0f)
		{
			particleEmitCounter = particleEmitCounterMax;
			Vector3 coords = array[0] + (array[1] - array[0]) * UnityEngine.Random.Range(0f, 1f);
			GameplayCommons.Instance.effectsSpawner.CreateLaserHitEffect(coords);
		}
		else
		{
			particleEmitCounter -= 1f;
		}
		lr.SetPositions(array);
		if (Time.fixedTime - lastTimeEmittedSmoke >= smokeEmitTimeout)
		{
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(array[1], 1, 0f);
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(array[0], 1, 0f);
			lastTimeEmittedSmoke = Time.fixedTime;
		}
		if (Time.timeScale > 0.1f)
		{
			for (int i = 0; i < 2; i++)
			{
				laserHitPointSpriteRenderers[i].transform.position = array[i];
				Transform transform2 = laserHitPointSpriteRenderers[i].transform;
				float x = UnityEngine.Random.Range(3f, 7.5f);
				float y = UnityEngine.Random.Range(3f, 7.5f);
				Vector3 localScale = laserHitPointSpriteRenderers[i].transform.localScale;
				transform2.localScale = new Vector3(x, y, localScale.z);
			}
		}
	}
}
