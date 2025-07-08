using UnityEngine;

public class RailgunBeam : MonoBehaviour
{
	private LineRenderer lr;

	private float activatedTime;

	private float duration = 0.3f;

	private float damage = 1f;

	private float damageDepletionFactor = 0.6f;

	private void Awake()
	{
		lr = GetComponent<LineRenderer>();
		lr.SetWidth(0.15f, 0.15f);
		lr.sortingLayerName = "LaserRailgunBeams";
		activatedTime = Time.fixedTime;
	}

	public void InitializeBeam(EnemyTankController etc, bool isPlayerBeam, bool bossBeam = false)
	{
		Quaternion rotation = Quaternion.Euler(0f, 0f, 90f);
		Vector3 vector;
		LayerMask mask;
		if (isPlayerBeam)
		{
			vector = rotation * GameplayCommons.Instance.playersTankController.TankTurret.transform.right;
			mask = LayerMasks.obstaclesEnemiesSpawnersDestrObstaclesLayerMask;
			damage = PlayerBalance.railgunDamageValues[GlobalCommons.Instance.globalGameStats.WeaponsLevels[8]];
		}
		else
		{
			damage = EnemiesBalance.GetEnemyDamage(WeaponTypes.railgun);
			if (bossBeam)
			{
				damage *= EnemiesBalance.bossDamageCoeff;
			}
			vector = rotation * etc.TankTurret.transform.right;
			mask = LayerMasks.obstaclesPlayerDestrObstaclesLayerMask;
		}
		Vector3 vector2 = (!isPlayerBeam) ? etc.GetBarrelPoint() : GameplayCommons.Instance.playersTankController.GetBarrelPoint();
		RaycastHit2D[] array = Physics2D.RaycastAll(vector2, vector, 9f, mask);
		Vector3[] array2 = new Vector3[2]
		{
			vector2,
			vector2 + vector * 9f
		};
		bool flag = false;
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			RaycastHit2D raycastHit2D = array[i];
			if (raycastHit2D.collider.transform.parent != null)
			{
				if (raycastHit2D.collider.gameObject.layer == PhysicsLayers.EnemyTankBase)
				{
					EnemyTankController component = raycastHit2D.collider.transform.parent.GetComponent<EnemyTankController>();
					component.ApplyDamage(damage, DamageTypes.railgunDamage);
					flag = true;
					num++;
					damage *= damageDepletionFactor;
				}
				else if (raycastHit2D.collider.gameObject.layer == PhysicsLayers.PlayersTankBase)
				{
					PlayersTankController component2 = raycastHit2D.collider.transform.parent.gameObject.GetComponent<PlayersTankController>();
					component2.ApplyDamage(damage, DamageTypes.railgunDamage);
					array2[1] = raycastHit2D.point;
					break;
				}
			}
			else if (raycastHit2D.collider.gameObject.layer == PhysicsLayers.EnemiesSpawner)
			{
				EnemySpawnerController component3 = raycastHit2D.collider.gameObject.GetComponent<EnemySpawnerController>();
				component3.ApplyDamage(damage, DamageTypes.railgunDamage);
				flag = true;
				num++;
				damage *= damageDepletionFactor;
			}
			else if (raycastHit2D.collider.gameObject.layer == PhysicsLayers.DestructableObstacles)
			{
				DestructableObstacleController component4 = raycastHit2D.collider.gameObject.GetComponent<DestructableObstacleController>();
				component4.ApplyDamage(damage, DamageTypes.railgunDamage);
				flag = true;
				damage *= damageDepletionFactor;
			}
			else if (raycastHit2D.collider.gameObject.layer == PhysicsLayers.Obstacles)
			{
				array2[1] = raycastHit2D.point;
				break;
			}
		}
		if (isPlayerBeam)
		{
			if (flag)
			{
				SoundManager.instance.PlayEnemyHitSound(DamageTypes.railgunDamage);
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesHit);
			}
			else
			{
				GlobalCommons.Instance.globalGameStats.GameStatistics.IncreaseStat(GameStatistics.Stat.TimesMissed);
			}
			GlobalCommons.Instance.globalGameStats.GameStatistics.UpdateMaxStat(GameStatistics.Stat.RailgunSimultaneousHits, num);
		}
		GameplayCommons.Instance.effectsSpawner.SpawnSmoke(array2[1], 2, 0f);
		GameplayCommons.Instance.effectsSpawner.CreateHitEffect(array2[1]);
		lr.SetPositions(array2);
		CreateSparkEffects(array2);
	}

	private void CreateSparkEffects(Vector3[] points)
	{
		Vector3 vector = new Vector3(points[0].x, points[0].y, points[0].z);
		float num = Vector2.Distance(points[0], points[1]);
		int num2 = Mathf.CeilToInt(num * 2f);
		Vector3 b = (points[1] - points[0]) / num2;
		for (int i = 0; i < num2 - 2; i++)
		{
			vector += b;
			GameplayCommons.Instance.effectsSpawner.CreateRailgunEffect(vector);
		}
	}

	private void Update()
	{
		float num = Time.fixedTime - activatedTime;
		if (num >= duration)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		float a = (duration - num) / duration;
		Color white = Color.white;
		white.a = a;
		lr.SetColors(white, white);
	}
}
