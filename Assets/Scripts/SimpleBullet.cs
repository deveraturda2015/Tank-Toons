using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
	private float bulletDamage = 1f;

	private float launchedTimestamp;

	private float destroyTimeout = 0.5f;

	private bool playersBullet = true;

	private Vector2 bulletVelocity;

	private Rigidbody2D rb;

	private SpriteRenderer bulletSR;

	private void Start()
	{
	}

	public void Initialize(float angleFix, float forceFix, EnemyTankController etc, float bulletDamage)
	{
		destroyTimeout = 0.5f;
		playersBullet = true;
		if (!bulletSR)
		{
			bulletSR = GetComponent<SpriteRenderer>();
		}
		bulletSR.enabled = true;
		launchedTimestamp = Time.fixedTime;
		if (etc != null)
		{
			playersBullet = false;
		}
		this.bulletDamage = bulletDamage;
		Quaternion rotation = Quaternion.Euler(0f, 0f, 90f + UnityEngine.Random.Range((0f - angleFix) / 2f, angleFix / 2f));
		Vector3 vector = (!playersBullet) ? (rotation * etc.TankTurret.transform.right) : (rotation * GameplayCommons.Instance.playersTankController.TankTurret.transform.right);
		rb = GetComponent<Rigidbody2D>();
		float num = 0.26f + UnityEngine.Random.Range((0f - forceFix) / 2f, forceFix / 2f);
		rb.AddForce(vector.normalized * num, ForceMode2D.Impulse);
		bulletVelocity = rb.velocity;
		if (playersBullet)
		{
			base.gameObject.layer = PhysicsLayers.PlayerBullets;
		}
		else
		{
			base.gameObject.layer = PhysicsLayers.EnemyBullets;
		}
		destroyTimeout = 0.14f / num;
	}

	private void Update()
	{
	}

	private void FixedUpdate()
	{
		rb.velocity = bulletVelocity;
		if (Time.fixedTime - launchedTimestamp >= destroyTimeout)
		{
			if (playersBullet)
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesMissed);
			}
			GameplayCommons.Instance.effectsSpawner.SpawnBulletDisappearEffect(base.transform.position);
			base.gameObject.SetActive(value: false);
		}
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (UnityEngine.Random.value > 0.91f)
		{
			SoundManager.instance.PlayRicochetSound();
		}
		GameplayCommons.Instance.effectsSpawner.CreateHitEffect(base.transform.position);
		GameplayCommons.Instance.effectsSpawner.SpawnBulletHitSparksEffect(base.transform.position);
		bulletSR.enabled = false;
		if (col.gameObject.layer == PhysicsLayers.EnemyTankBase)
		{
			if (playersBullet)
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesHit);
			}
			EnemyTankController component = col.gameObject.transform.parent.GetComponent<EnemyTankController>();
			component.ApplyDamage(bulletDamage, DamageTypes.bulletDamage);
		}
		else if (col.gameObject.layer == PhysicsLayers.PlayersTankBase)
		{
			PlayersTankController component2 = col.gameObject.transform.parent.GetComponent<PlayersTankController>();
			component2.ApplyDamage(bulletDamage, DamageTypes.bulletDamage);
		}
		else if (col.gameObject.layer == PhysicsLayers.EnemiesSpawner)
		{
			if (playersBullet)
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesHit);
			}
			EnemySpawnerController component3 = col.gameObject.GetComponent<EnemySpawnerController>();
			component3.ApplyDamage(bulletDamage, DamageTypes.bulletDamage);
		}
		else if (col.gameObject.layer == PhysicsLayers.DestructableObstacles)
		{
			if (playersBullet)
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesHit);
			}
			DestructableObstacleController component4 = col.gameObject.GetComponent<DestructableObstacleController>();
			if (component4.IsWallSegment())
			{
				SoundManager.instance.PlayBulletHitObstacleSound();
			}
			component4.ApplyDamage(bulletDamage, DamageTypes.bulletDamage);
		}
		else if (col.gameObject.layer == PhysicsLayers.Obstacles)
		{
			if (playersBullet)
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesMissed);
			}
			SoundManager.instance.PlayBulletHitObstacleSound();
		}
		else if (playersBullet)
		{
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesMissed);
		}
		base.gameObject.SetActive(value: false);
	}
}
