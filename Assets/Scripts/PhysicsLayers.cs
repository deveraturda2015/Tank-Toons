using UnityEngine;

public class PhysicsLayers
{
	public static int PlayersTankBase;

	public static int EnemyTankBase;

	public static int PlayerBullets;

	public static int EnemyBullets;

	public static int Obstacles;

	public static int DestructableObstacles;

	public static int EnemiesSpawner;

	public static int HitPlayerOnly;

	public static int PlayerBulletsNoSpawners;

	public static int Mines;

	internal static int[] enemiesBlockingSightLayers;

	internal static int[] obstaclesBlockingSightLayers;

	public static void InitializePhysicsLayers()
	{
		PlayersTankBase = LayerMask.NameToLayer("PlayersTankBase");
		EnemyTankBase = LayerMask.NameToLayer("EnemyTankBase");
		PlayerBullets = LayerMask.NameToLayer("PlayerBullets");
		EnemyBullets = LayerMask.NameToLayer("EnemyBullets");
		Obstacles = LayerMask.NameToLayer("Obstacles");
		DestructableObstacles = LayerMask.NameToLayer("DestructableObstacles");
		EnemiesSpawner = LayerMask.NameToLayer("EnemiesSpawner");
		HitPlayerOnly = LayerMask.NameToLayer("HitPlayerOnly");
		PlayerBulletsNoSpawners = LayerMask.NameToLayer("PlayerBulletsNoSpawners");
		Mines = LayerMask.NameToLayer("Mines");
		enemiesBlockingSightLayers = new int[1]
		{
			EnemyTankBase
		};
		obstaclesBlockingSightLayers = new int[2]
		{
			Obstacles,
			DestructableObstacles
		};
	}
}
