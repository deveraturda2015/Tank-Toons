using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HomingRocket : MonoBehaviour
{
	private Rigidbody2D rocketRigidBody;

	private GameObject currentTarget;

	private float lastTimeSearchedTarget;

	private float targetSearchInterval;

	private float lastTimeRenewedTarget;

	private float targetRenewInterval;

	private float torqueFactor = 0.006f;

	private float linearMoveFactor = 0.3f;

	private bool playersRocket = true;

	private Vector3 prevPosition;

	private float explosionDamage = 6f;

	private float explosionImpulse = 2.5f;

	private float explosionRadius = 0.66f;

	private float launchedTimestamp;

	private float destroyTimeout = 4f;

	private float lastTimeEmittedSmoke;

	private Vector3 smokeEmitCoord;

	private float smokeEmitTimeout = 0.1f;

	private bool bossRocket;

	private void Start()
	{
		GameplayCommons.Instance.enemiesTracker.Track(this);
		smokeEmitCoord = base.transform.position;
		lastTimeEmittedSmoke = Time.fixedTime;
		launchedTimestamp = Time.fixedTime;
		rocketRigidBody = GetComponent<Rigidbody2D>();
		targetSearchInterval = UnityEngine.Random.Range(0.2f, 0.3f);
		targetRenewInterval = targetSearchInterval * 2f;
		lastTimeSearchedTarget = Time.fixedTime;
		lastTimeRenewedTarget = Time.fixedTime;
		if (playersRocket)
		{
			base.transform.rotation = GameplayCommons.Instance.playersTankController.TankTurret.transform.rotation;
			SelectTarget(force: true);
			explosionDamage = PlayerBalance.homingRocketExplosionDamageValues[GlobalCommons.Instance.globalGameStats.WeaponsLevels[4]];
		}
		else
		{
			explosionDamage = EnemiesBalance.GetEnemyDamage(WeaponTypes.homingRocket);
			if (bossRocket)
			{
				explosionDamage *= EnemiesBalance.bossDamageCoeff;
			}
		}
		Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
		Vector3 vector = rotation * base.transform.right;
		rocketRigidBody.AddForce(vector.normalized * 0.1f, ForceMode2D.Impulse);
	}

	private void Update()
	{
		if (Time.fixedTime - lastTimeEmittedSmoke >= smokeEmitTimeout)
		{
			lastTimeEmittedSmoke = Time.fixedTime;
			GameplayCommons.Instance.effectsSpawner.SpawnSmoke(base.transform.position, 1, 0f);
		}
	}

	public void SetEnemysRocket(Quaternion turretRotation, bool bossRocket)
	{
		this.bossRocket = bossRocket;
		destroyTimeout = 3f;
		base.transform.rotation = turretRotation;
		playersRocket = false;
		base.gameObject.layer = PhysicsLayers.EnemyBullets;
	}

	private void FixedUpdate()
	{
		if (Time.fixedTime - launchedTimestamp >= destroyTimeout)
		{
			if (!playersRocket)
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.EnemyRocketsDodged);
			}
			ProcessDestruction();
			return;
		}
		if (playersRocket)
		{
			SelectTarget(force: false);
			if (currentTarget != null)
			{
				FaceTarget();
			}
		}
		else if (!GameplayCommons.Instance.levelStateController.IsInvisibilityActive)
		{
			FaceTarget();
		}
		Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
		Vector3 vector = rotation * base.transform.right;
		rocketRigidBody.AddForce(vector.normalized * linearMoveFactor);
		prevPosition = base.transform.position;
	}

	private void FaceTarget()
	{
		Vector2 vector;
		if (playersRocket)
		{
			vector = currentTarget.transform.position - base.transform.position;
		}
		else
		{
			vector = GameplayCommons.Instance.playersTankController.TankBase.transform.position - base.transform.position;
			if (Physics2D.Raycast(base.transform.position, vector, vector.magnitude, LayerMasks.allObstacleTypesLayerMask).collider != null)
			{
				return;
			}
		}
		float num = Mathf.Abs(Vector2.Angle(base.transform.up, vector)) / 90f;
		Vector3 vector2 = Vector3.Cross(rocketRigidBody.transform.up, vector);
		vector2.Normalize();
		float z = vector2.z;
		rocketRigidBody.AddTorque(torqueFactor * z * num);
	}

	private void SelectTarget(bool force)
	{
		if (!force && Time.fixedTime - lastTimeSearchedTarget < targetSearchInterval)
		{
			return;
		}
		if (currentTarget != null)
		{
			if (Time.fixedTime - lastTimeRenewedTarget < targetRenewInterval)
			{
				return;
			}
			lastTimeRenewedTarget = Time.fixedTime;
		}
		lastTimeSearchedTarget = Time.fixedTime;
		List<EnemyTankController> allEnemies = GameplayCommons.Instance.enemiesTracker.AllEnemies;
		List<EnemySpawnerController> allSpawners = GameplayCommons.Instance.enemiesTracker.AllSpawners;
		allEnemies = (from x in allEnemies
			where x.IsEnemyEnabled()
			select x).ToList();
		base.gameObject.layer = PhysicsLayers.PlayerBullets;
		GameObject x2 = FindClosestTarget((from x in allEnemies
			select x.TankBase).ToList());
		if (x2 != null)
		{
			base.gameObject.layer = PhysicsLayers.PlayerBulletsNoSpawners;
		}
		else
		{
			x2 = FindClosestTarget((from x in allSpawners
				select x.gameObject).ToList());
		}
		if (x2 != null)
		{
			currentTarget = x2;
		}
		else
		{
			currentTarget = null;
		}
	}

	private GameObject FindClosestTarget(List<GameObject> gameObjects)
	{
		float num = float.MaxValue;
		GameObject result = null;
		for (int i = 0; i < gameObjects.Count; i++)
		{
			GameObject gameObject = gameObjects[i];
			Vector2 vector = gameObject.transform.position - base.transform.position;
			float magnitude = vector.magnitude;
			float f = Vector2.Angle(base.transform.up, vector);
			if (Mathf.Abs(f) <= 40f && magnitude <= 5f && magnitude < num && Physics2D.Raycast(base.transform.position, vector, magnitude, LayerMasks.allObstacleTypesLayerMask).collider == null)
			{
				result = gameObject;
				num = magnitude;
			}
		}
		return result;
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		ProcessDestruction();
	}

	public void ProcessDestruction()
	{
		if (playersRocket)
		{
			GameplayCommons.Instance.cameraController.ShakeCamera(2f);
		}
		ExplosionProcessor.ExplosionTrigger trigger = ExplosionProcessor.ExplosionTrigger.player;
		if (!playersRocket)
		{
			trigger = ExplosionProcessor.ExplosionTrigger.enemy;
		}
		ExplosionResult explosionResult = ExplosionProcessor.ProcessExplosion(trigger, base.transform.position, explosionDamage, explosionImpulse, prevPosition, explosionRadius);
		if (playersRocket)
		{
			if (explosionResult.EnemyOrSpawnerOrDestObstWasHit)
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesHit);
			}
			else
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesMissed);
			}
		}
		GameplayCommons.Instance.enemiesTracker.UnTrack(this);
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
