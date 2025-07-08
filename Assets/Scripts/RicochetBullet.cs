using System.Collections.Generic;
using UnityEngine;

public class RicochetBullet : MonoBehaviour
{
	private Queue<Vector2> prevPositions;

	private float launchedTimestamp;

	private float destroyTimeout = 4f;

	private float explosionDamage = 5f;

	private float explosionImpulse = 4f;

	private float explosionRadius = 1f;

	private int collisionsLeft = 3;

	private bool isPlayerCannon = true;

	private int enemiesHit;

	private GameObject graphicsGO;

	public Sprite BulletSprite;

	private float VelocityMag;

	private int VelocityCheckTicker;

	private Rigidbody2D rb;

	internal static bool ProcessedVisibilityThisFrame;

	private int ProcessVisibilityTicker;

	private void Start()
	{
		graphicsGO = new GameObject();
		graphicsGO.AddComponent<SpriteRenderer>().sprite = BulletSprite;
		graphicsGO.transform.position = base.transform.position;
		prevPositions = new Queue<Vector2>();
		launchedTimestamp = Time.fixedTime;
		GameplayCommons.Instance.effectsSpawner.SpawnSmoke(base.transform.position, 1, 0f);
	}

	public void Initialize(Vector3 turretTransform, bool isPlayerCannon, bool bossCannonball = false)
	{
		this.isPlayerCannon = isPlayerCannon;
		Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
		Vector3 vector = rotation * turretTransform;
		rb = GetComponent<Rigidbody2D>();
		float d = 0.233f;
		rb.AddForce(vector.normalized * d, ForceMode2D.Impulse);
		VelocityMag = rb.velocity.magnitude;
		if (isPlayerCannon)
		{
			base.gameObject.layer = PhysicsLayers.PlayerBullets;
			collisionsLeft = PlayerBalance.ricochetBounceCountValues[GlobalCommons.Instance.globalGameStats.WeaponsLevels[12]];
			explosionDamage = PlayerBalance.ricochetExplosionDamageValues[GlobalCommons.Instance.globalGameStats.WeaponsLevels[12]];
			return;
		}
		base.gameObject.layer = PhysicsLayers.EnemyBullets;
		explosionDamage = EnemiesBalance.GetEnemyDamage(WeaponTypes.ricochet);
		if (bossCannonball)
		{
			explosionDamage *= EnemiesBalance.bossDamageCoeff;
		}
	}

	private void Update()
	{
		if (ProcessVisibilityTicker <= 0)
		{
			if (!ProcessedVisibilityThisFrame)
			{
				ProcessedVisibilityThisFrame = true;
				float num = 2f;
				VisibilityController visibilityController = GameplayCommons.Instance.visibilityController;
				Vector3 position = base.transform.position;
				float xmin = position.x - num;
				Vector3 position2 = base.transform.position;
				float xmax = position2.x + num;
				Vector3 position3 = base.transform.position;
				float ymin = position3.y - num;
				Vector3 position4 = base.transform.position;
				visibilityController.UncoverPortion(xmin, xmax, ymin, position4.y + num);
				ProcessVisibilityTicker = UnityEngine.Random.Range(4, 6);
			}
		}
		else
		{
			ProcessVisibilityTicker--;
		}
		graphicsGO.transform.position = base.transform.position;
		VelocityCheckTicker++;
		if (VelocityCheckTicker > 8)
		{
			VelocityCheckTicker = 0;
			rb.velocity = rb.velocity.normalized * VelocityMag;
		}
		if (Time.timeScale > 0.1f)
		{
			Transform transform = graphicsGO.transform;
			float x = UnityEngine.Random.Range(3f, 7.5f);
			float y = UnityEngine.Random.Range(3f, 7.5f);
			Vector3 localScale = base.transform.localScale;
			transform.localScale = new Vector3(x, y, localScale.z);
		}
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
		SoundManager.instance.PlayRicochetBounceSound();
		ExplosionResult explosionResult = ExplosionProcessor.ProcessExplosion(trigger, base.transform.position, explosionDamage, explosionImpulse, null, explosionRadius, DamageTypes.explosionDamage, prevPositions.ToArray(), playSound: false);
		if (isPlayerCannon && explosionResult.EnemyOrSpawnerOrDestObstWasHit)
		{
			enemiesHit++;
		}
		collisionsLeft--;
		if (collisionsLeft == 0)
		{
			if (enemiesHit > 0)
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesHit);
			}
			else
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesMissed);
			}
			WaveExploPostProcessing.ShowEffectAt(Camera.main.WorldToScreenPoint(base.transform.position));
			UnityEngine.Object.Destroy(graphicsGO);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
