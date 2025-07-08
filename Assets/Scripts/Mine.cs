using UnityEngine;

public class Mine : MonoBehaviour
{
	private float explosionDamage = 6f;

	private float explosionImpulse = 5f;

	private float explosionRadius = 1f;

	private float explosiveBarrelHitTimestamp = -1f;

	private float explosiveBarrelExplodeTimeout;

	private bool exploded;

	private void Start()
	{
		explosionDamage = PlayerBalance.mineExplosionDamageValues[GlobalCommons.Instance.globalGameStats.WeaponsLevels[5]];
		explosiveBarrelExplodeTimeout = UnityEngine.Random.Range(0.05f, 0.15f);
	}

	private void Update()
	{
		if (explosiveBarrelHitTimestamp > 0f && Time.fixedTime - explosiveBarrelHitTimestamp >= explosiveBarrelExplodeTimeout)
		{
			ActuallyExplode();
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (LayerMask.LayerToName(other.gameObject.layer) == "EnemyTankBase")
		{
			Explode();
		}
		else if (LayerMask.LayerToName(other.gameObject.layer) == "DestructableObstacles")
		{
			Explode();
		}
	}

	public void Explode()
	{
		if (!exploded)
		{
			exploded = true;
			explosiveBarrelHitTimestamp = Time.fixedTime;
		}
	}

	private void ActuallyExplode()
	{
		if (ExplosionProcessor.ProcessExplosion(ExplosionProcessor.ExplosionTrigger.player, base.transform.position, explosionDamage, explosionImpulse, null, explosionRadius).EnemyOrSpawnerOrDestObstWasHit)
		{
			GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesHitEnemyWithMine);
		}
		MinesController.AllMines.Remove(base.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
