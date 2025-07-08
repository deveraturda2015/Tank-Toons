using UnityEngine;

public class ExplosionProcessor
{
	public enum ExplosionTrigger
	{
		player,
		enemy,
		neutral
	}

	private static float LastTimePushedPlayer;

	public static ExplosionResult ProcessExplosion(ExplosionTrigger trigger, Vector3 center, float explosionDamage, float explosionImpulse, Vector3? fallbackCenter = default(Vector3?), float explosionRadius = 1f, DamageTypes damageType = DamageTypes.explosionDamage, Vector2[] fallbackPoints = null, bool playSound = true)
	{
		if (fallbackCenter.HasValue)
		{
			Collider2D[] array = Physics2D.OverlapPointAll(new Vector2(center.x, center.y), LayerMasks.allObstacleTypesLayerMask);
			if (array.Length > 0)
			{
				center = fallbackCenter.Value;
			}
		}
		else if (fallbackPoints != null)
		{
			Collider2D[] array2 = Physics2D.OverlapPointAll(new Vector2(center.x, center.y), LayerMasks.allObstacleTypesLayerMask);
			if (array2.Length > 0)
			{
				int num = fallbackPoints.Length;
				while (num-- > 0)
				{
					Vector2 vector = fallbackPoints[num];
					array2 = Physics2D.OverlapPointAll(vector, LayerMasks.allObstacleTypesLayerMask);
					if (array2.Length == 0)
					{
						center = vector;
						break;
					}
				}
			}
		}
		ExplosionResult explosionResult = new ExplosionResult();
		GameplayCommons.Instance.effectsSpawner.CreateExplosionEffect(center, explosionRadius, playSound);
		for (int i = 0; i < GameplayCommons.Instance.enemiesTracker.AllEnemies.Count; i++)
		{
			EnemyTankController enemyTankController = GameplayCommons.Instance.enemiesTracker.AllEnemies[i];
			if (!enemyTankController.IsEnemyEnabled() && Vector2.Distance(enemyTankController.TankBase.transform.position, center) <= explosionRadius + 2f)
			{
				enemyTankController.ForceEnableEnemy();
			}
		}
		Collider2D[] array3 = Physics2D.OverlapCircleAll(center, explosionRadius, LayerMasks.enemiesPlayerMinesSpawnersDestructbleObstaclesLayerMask);
		Collider2D[] array4 = array3;
		foreach (Collider2D collider2D in array4)
		{
			Rigidbody2D component = collider2D.gameObject.GetComponent<Rigidbody2D>();
			Vector3 v = collider2D.bounds.ClosestPoint(center);
			float num3 = Vector2.Distance(v, center);
			Vector2 vector2 = (collider2D.transform.position - center).normalized;
			float num4 = (explosionRadius - num3) / explosionRadius;
			float num5 = num4 * explosionDamage;
			if (!(Physics2D.Raycast(center, vector2, num3, LayerMasks.onlyIndestructableObstaclesLayerMask).collider == null))
			{
				continue;
			}
			if (collider2D.gameObject.layer == PhysicsLayers.EnemyTankBase && trigger != ExplosionTrigger.enemy)
			{
				EnemyTankController component2 = collider2D.transform.parent.gameObject.GetComponent<EnemyTankController>();
				component2.ApplyDamage(num5, DamageTypes.explosionDamage);
				explosionResult.EnemyOrSpawnerOrDestObstWasHit = true;
				explosionResult.EnemyOrSpawnerWasHit = true;
			}
			else if (collider2D.gameObject.layer == PhysicsLayers.PlayersTankBase && trigger != 0)
			{
				PlayersTankController component3 = collider2D.transform.parent.gameObject.GetComponent<PlayersTankController>();
				component3.ApplyDamage(num5, damageType);
				if (Time.fixedTime - LastTimePushedPlayer > 1f)
				{
					LastTimePushedPlayer = Time.fixedTime;
					Vector2 vector3 = vector2 * num4 / 4f;
					float num6 = GlobalCommons.Instance.gridSize / 4f;
					if (vector3.magnitude > num6)
					{
						vector3 = vector3.normalized * num6;
					}
					Transform transform = component3.TankBase.transform;
					Vector3 position = component3.TankBase.transform.position;
					float x = position.x + vector3.x;
					Vector3 position2 = component3.TankBase.transform.position;
					float y = position2.y + vector3.y;
					Vector3 position3 = component3.TankBase.transform.position;
					transform.position = new Vector3(x, y, position3.z);
				}
			}
			else if (collider2D.gameObject.layer == PhysicsLayers.EnemiesSpawner && trigger != ExplosionTrigger.enemy)
			{
				EnemySpawnerController component4 = collider2D.gameObject.GetComponent<EnemySpawnerController>();
				component4.ApplyDamage(num5, DamageTypes.explosionDamage);
				explosionResult.EnemyOrSpawnerOrDestObstWasHit = true;
				explosionResult.EnemyOrSpawnerWasHit = true;
			}
			else if (collider2D.gameObject.layer == PhysicsLayers.DestructableObstacles)
			{
				DestructableObstacleController component5 = collider2D.gameObject.GetComponent<DestructableObstacleController>();
				component5.ApplyDamage(num5, DamageTypes.explosionDamage);
				explosionResult.EnemyOrSpawnerOrDestObstWasHit = true;
			}
			if (component != null)
			{
				component.AddForce(vector2 * num4 * explosionImpulse, ForceMode2D.Impulse);
			}
		}
		return explosionResult;
	}
}
