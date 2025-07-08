using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
	private Queue<Vector2> prevPositions;

	private float explosionDamage = 6f;

	private float explosionImpulse = 5f;

	private float explosionRadius = 1f;

	private float launchedTimestamp;

	private float destroyTimeout = 4f;

	private bool isPlayerCannon = true;

	private void Start()
	{
		prevPositions = new Queue<Vector2>();
		launchedTimestamp = Time.fixedTime;
		GameplayCommons.Instance.effectsSpawner.SpawnSmoke(base.transform.position, 1, 0f);
	}

	public void Initialize(float angleFix, float forceFix, Vector3 turretTransform, bool isPlayerCannon, bool bossCannonball = false)
	{
		this.isPlayerCannon = isPlayerCannon;
		Quaternion rotation = Quaternion.Euler(0f, 0f, 90f + UnityEngine.Random.Range((0f - angleFix) / 2f, angleFix / 2f));
		Vector3 vector = rotation * turretTransform;
		Rigidbody2D component = GetComponent<Rigidbody2D>();
		float num = 0.26f + UnityEngine.Random.Range((0f - forceFix) / 2f, forceFix / 2f);
		component.AddForce(vector.normalized * num, ForceMode2D.Impulse);
		if (isPlayerCannon)
		{
			base.gameObject.layer = PhysicsLayers.PlayerBullets;
			explosionDamage = PlayerBalance.cannonExplosionDamageValues[GlobalCommons.Instance.globalGameStats.WeaponsLevels[3]];
		}
		else
		{
			base.gameObject.layer = PhysicsLayers.EnemyBullets;
			explosionDamage = EnemiesBalance.GetEnemyDamage(WeaponTypes.cannon);
			if (bossCannonball)
			{
				explosionDamage *= EnemiesBalance.bossDamageCoeff;
			}
		}
		destroyTimeout = 0.14f / num;
	}

	private void FixedUpdate()
	{
		if (Time.fixedTime - launchedTimestamp >= destroyTimeout)
		{
			ExplodeCannonball();
			return;
		}
		prevPositions.Enqueue(base.transform.position);
		if (prevPositions.Count > 5)
		{
			prevPositions.Dequeue();
		}
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		ExplodeCannonball();
	}

	private void ExplodeCannonball()
	{
		if (isPlayerCannon)
		{
			GameplayCommons.Instance.cameraController.ShakeCamera(2f);
		}
		GameplayCommons.Instance.effectsSpawner.SpawnSmoke(base.transform.position, 6, 0.8f);
		ExplosionProcessor.ExplosionTrigger trigger = ExplosionProcessor.ExplosionTrigger.player;
		if (!isPlayerCannon)
		{
			trigger = ExplosionProcessor.ExplosionTrigger.enemy;
		}
		ExplosionResult explosionResult = ExplosionProcessor.ProcessExplosion(trigger, base.transform.position, explosionDamage, explosionImpulse, null, explosionRadius, DamageTypes.explosionDamage, prevPositions.ToArray());
		if (isPlayerCannon)
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
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
