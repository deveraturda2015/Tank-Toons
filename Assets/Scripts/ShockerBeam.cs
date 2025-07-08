using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShockerBeam : MonoBehaviour
{
	private List<SpriteRenderer> HitPointSpriteRenderersList;

	private float damagePerSecond = 0.1f;

	private bool playersBeam = true;

	private EnemyTankController enemyTankController;

	private bool bossWeapon;

	private int maxTargets = 4;

	private List<SpriteRenderer> laserHitPointSpriteRenderers;

	private float lastTimeEmittedSmoke;

	private float smokeEmitTimeout = 0.12f;

	private LineRenderer rayLineRenderer;

	private float laserBeamWidth = 0.15f;

	private void Start()
	{
		laserHitPointSpriteRenderers = new List<SpriteRenderer>();
		for (int i = 0; i < 2; i++)
		{
			SpriteRenderer component = UnityEngine.Object.Instantiate(Prefabs.BeamHitPointPrefab).GetComponent<SpriteRenderer>();
			laserHitPointSpriteRenderers.Add(component);
		}
		if (playersBeam)
		{
			damagePerSecond = PlayerBalance.ShockerDamageValues[GlobalCommons.Instance.globalGameStats.WeaponsLevels[14]];
		}
		rayLineRenderer = UnityEngine.Object.Instantiate(Prefabs.ShockerBeamGraphics).GetComponent<LineRenderer>();
		rayLineRenderer.SetWidth(laserBeamWidth, laserBeamWidth);
		rayLineRenderer.gameObject.SetActive(value: false);
	}

	public void InitializeEnemyBeam(EnemyTankController etc, bool bossWeapon)
	{
		this.bossWeapon = bossWeapon;
		playersBeam = false;
		enemyTankController = etc;
	}

	public void ProcessShooting(bool isShooting)
	{
		laserHitPointSpriteRenderers.ForEach(delegate(SpriteRenderer itm)
		{
			if (itm.gameObject.activeInHierarchy != isShooting)
			{
				itm.gameObject.SetActive(isShooting);
			}
		});
		if (isShooting)
		{
			if (!rayLineRenderer.gameObject.activeInHierarchy)
			{
				rayLineRenderer.gameObject.SetActive(value: true);
			}
			UpdateShootingState();
		}
		else if (rayLineRenderer.gameObject.activeInHierarchy)
		{
			rayLineRenderer.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (!playersBeam && enemyTankController == null)
		{
			laserHitPointSpriteRenderers.ForEach(delegate(SpriteRenderer itm)
			{
				UnityEngine.Object.Destroy(itm.gameObject);
			});
			UnityEngine.Object.Destroy(rayLineRenderer.gameObject);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void UpdateShootingState()
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
		List<EnemyTankController> list = new List<EnemyTankController>();
		List<EnemySpawnerController> list2 = new List<EnemySpawnerController>();
		List<DestructableObstacleController> list3 = new List<DestructableObstacleController>();
		List<GameObject> list4 = new List<GameObject>();
		List<Vector3> list5 = new List<Vector3>();
		list5.Add(vector);
		list5.Add(vector + vector2 * 9f);
		bool flag = false;
		float num = (!playersBeam) ? (EnemiesBalance.GetEnemyDamage(WeaponTypes.laser) * Time.deltaTime) : (damagePerSecond * Time.deltaTime);
		if (bossWeapon)
		{
			num *= EnemiesBalance.bossDamageCoeff;
		}
		if (raycastHit2D.collider != null)
		{
			list5.RemoveAt(list5.Count - 1);
			list5.Add(raycastHit2D.point);
			if (raycastHit2D.collider.transform.parent != null)
			{
				if (raycastHit2D.collider.gameObject.layer == PhysicsLayers.EnemyTankBase)
				{
					EnemyTankController component = raycastHit2D.collider.transform.parent.GetComponent<EnemyTankController>();
					component.ApplyDamage(num, DamageTypes.shockDamage);
					list.Add(component);
					list4.Add(raycastHit2D.collider.gameObject);
				}
				else if (raycastHit2D.collider.gameObject.layer == PhysicsLayers.PlayersTankBase)
				{
					PlayersTankController component2 = raycastHit2D.collider.transform.parent.gameObject.GetComponent<PlayersTankController>();
					component2.ApplyDamage(num, DamageTypes.shockDamage);
				}
			}
			else if (raycastHit2D.collider.gameObject.layer == PhysicsLayers.EnemiesSpawner)
			{
				EnemySpawnerController component3 = raycastHit2D.collider.gameObject.GetComponent<EnemySpawnerController>();
				component3.ApplyDamage(num, DamageTypes.shockDamage);
				list2.Add(component3);
				list4.Add(raycastHit2D.collider.gameObject);
			}
			else if (raycastHit2D.collider.gameObject.layer == PhysicsLayers.DestructableObstacles)
			{
				DestructableObstacleController component4 = raycastHit2D.collider.gameObject.GetComponent<DestructableObstacleController>();
				component4.ApplyDamage(num, DamageTypes.shockDamage);
				list3.Add(component4);
			}
		}
		if (playersBeam)
		{
			if (list.Count > 0 || list2.Count > 0 || list3.Count > 0)
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesHit);
			}
			else
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesMissed);
			}
			if (list.Count > 0 || list2.Count > 0)
			{
				GameObject gameObject = null;
				do
				{
					if (list.Count + list2.Count >= maxTargets)
					{
						continue;
					}
					List<EnemyTankController> allEnemies = GameplayCommons.Instance.enemiesTracker.AllEnemies;
					List<EnemySpawnerController> allSpawners = GameplayCommons.Instance.enemiesTracker.AllSpawners;
					allEnemies = (from x in allEnemies
						where x.IsEnemyEnabled()
						select x).ToList();
					gameObject = FindClosestTarget(list4, list5.Last(), (from x in allEnemies
						select x.TankBase).ToList());
					if (gameObject != null)
					{
						list4.Add(gameObject);
						list5.Add(gameObject.transform.position);
						EnemyTankController component5 = gameObject.transform.parent.GetComponent<EnemyTankController>();
						component5.ApplyDamage(num, DamageTypes.shockDamage);
						list.Add(component5);
						continue;
					}
					gameObject = FindClosestTarget(list4, list5.Last(), (from x in allSpawners
						select x.gameObject).ToList());
					if (gameObject != null)
					{
						list4.Add(gameObject);
						list5.Add(gameObject.transform.position);
						EnemySpawnerController component6 = gameObject.GetComponent<EnemySpawnerController>();
						component6.ApplyDamage(num, DamageTypes.shockDamage);
						list2.Add(component6);
					}
				}
				while (gameObject != null && list.Count + list2.Count >= maxTargets);
			}
		}
		rayLineRenderer.positionCount = list5.Count;
		rayLineRenderer.SetPositions(list5.ToArray());
		if (Time.timeScale > 0f)
		{
			float num2 = UnityEngine.Random.Range(0.33f, 1f);
			float num3 = UnityEngine.Random.Range(0.33f, 1f);
			rayLineRenderer.startColor = new Color(num2, num2, num2);
			rayLineRenderer.endColor = new Color(num3, num3, num3);
			rayLineRenderer.startWidth = laserBeamWidth * UnityEngine.Random.Range(0.1f, 2f);
			rayLineRenderer.endWidth = laserBeamWidth * UnityEngine.Random.Range(0.1f, 2f);
		}
		if (UnityEngine.Random.value > 0.66f)
		{
			for (int i = 1; i < list5.Count; i++)
			{
				Vector3 coords = list5[i - 1] + (list5[i] - list5[i - 1]) * UnityEngine.Random.Range(0f, 1f);
				GameplayCommons.Instance.effectsSpawner.CreateLaserHitEffect(coords);
			}
		}
		if (Time.fixedTime - lastTimeEmittedSmoke >= smokeEmitTimeout)
		{
			for (int j = 0; j < list5.Count; j++)
			{
				GameplayCommons.Instance.effectsSpawner.SpawnSmoke(list5[j], 1, 0f);
			}
			lastTimeEmittedSmoke = Time.fixedTime;
		}
		for (int k = 0; k < 2; k++)
		{
			laserHitPointSpriteRenderers[k].transform.position = list5[k];
			Transform transform2 = laserHitPointSpriteRenderers[k].transform;
			float x2 = UnityEngine.Random.Range(3f, 7.5f);
			float y = UnityEngine.Random.Range(3f, 7.5f);
			Vector3 localScale = laserHitPointSpriteRenderers[k].transform.localScale;
			transform2.localScale = new Vector3(x2, y, localScale.z);
		}
	}

	private GameObject FindClosestTarget(List<GameObject> gameobjectsToExclude, Vector3 fromPoint, List<GameObject> gameObjects)
	{
		float num = float.MaxValue;
		GameObject result = null;
		for (int i = 0; i < gameObjects.Count; i++)
		{
			GameObject gameObject = gameObjects[i];
			if (!gameobjectsToExclude.Contains(gameObject))
			{
				Vector2 direction = gameObject.transform.position - fromPoint;
				float magnitude = direction.magnitude;
				if (magnitude <= 5f && magnitude < num && Physics2D.Raycast(fromPoint, direction, magnitude, LayerMasks.allObstacleTypesLayerMask).collider == null)
				{
					result = gameObject;
					num = magnitude;
				}
			}
		}
		return result;
	}
}
