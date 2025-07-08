using UnityEngine;

internal class SurvivalSpawnedEnemy
{
	public WeaponTypes weaponType;

	public GameObject spawnedTank;

	public float chanceForBoss;

	public SurvivalSpawnedEnemy(WeaponTypes weaponType, float chanceForBoss)
	{
		this.weaponType = weaponType;
		this.chanceForBoss = chanceForBoss;
	}
}
