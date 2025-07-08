using UnityEngine;

public class LayerMasks
{
	internal static LayerMask onlyIndestructableObstaclesLayerMask;

	internal static LayerMask onlyDestructableObstaclesLayerMask;

	internal static LayerMask onlyEnemiesLayerMask;

	internal static LayerMask allObstacleTypesLayerMask;

	internal static LayerMask allObstacleTypesAndEnemiesLayerMask;

	internal static LayerMask obstaclesEnemiesAndSpawnersLayerMask;

	internal static LayerMask obstaclesEnemiesSpawnersDestrObstaclesLayerMask;

	internal static LayerMask obstaclesAndPlayerLayerMask;

	internal static LayerMask obstaclesPlayerDestrObstaclesLayerMask;

	internal static LayerMask enemiesAndPlayerLayerMask;

	internal static LayerMask enemiesPlayerSpawnersDestructbleObstaclesLayerMask;

	internal static LayerMask enemiesPlayerMinesSpawnersDestructbleObstaclesLayerMask;

	public static void InitializeLayerMasks()
	{
		onlyIndestructableObstaclesLayerMask = 1 << PhysicsLayers.Obstacles;
		onlyDestructableObstaclesLayerMask = 1 << PhysicsLayers.DestructableObstacles;
		onlyEnemiesLayerMask = 1 << PhysicsLayers.EnemyTankBase;
		allObstacleTypesLayerMask = ((1 << PhysicsLayers.Obstacles) | (1 << PhysicsLayers.DestructableObstacles));
		allObstacleTypesAndEnemiesLayerMask = ((1 << PhysicsLayers.Obstacles) | (1 << PhysicsLayers.EnemyTankBase) | (1 << PhysicsLayers.DestructableObstacles));
		obstaclesAndPlayerLayerMask = ((1 << PhysicsLayers.Obstacles) | (1 << PhysicsLayers.PlayersTankBase));
		obstaclesPlayerDestrObstaclesLayerMask = ((1 << PhysicsLayers.Obstacles) | (1 << PhysicsLayers.PlayersTankBase) | (1 << PhysicsLayers.DestructableObstacles));
		enemiesAndPlayerLayerMask = ((1 << PhysicsLayers.EnemyTankBase) | (1 << PhysicsLayers.PlayersTankBase));
		enemiesPlayerSpawnersDestructbleObstaclesLayerMask = ((1 << PhysicsLayers.EnemyTankBase) | (1 << PhysicsLayers.PlayersTankBase) | (1 << PhysicsLayers.EnemiesSpawner) | (1 << PhysicsLayers.DestructableObstacles));
		enemiesPlayerMinesSpawnersDestructbleObstaclesLayerMask = ((1 << PhysicsLayers.EnemyTankBase) | (1 << PhysicsLayers.PlayersTankBase) | (1 << PhysicsLayers.EnemiesSpawner) | (1 << PhysicsLayers.DestructableObstacles) | (1 << PhysicsLayers.Mines));
		obstaclesEnemiesAndSpawnersLayerMask = ((1 << PhysicsLayers.Obstacles) | (1 << PhysicsLayers.EnemyTankBase) | (1 << PhysicsLayers.EnemiesSpawner));
		obstaclesEnemiesSpawnersDestrObstaclesLayerMask = ((1 << PhysicsLayers.Obstacles) | (1 << PhysicsLayers.EnemyTankBase) | (1 << PhysicsLayers.EnemiesSpawner) | (1 << PhysicsLayers.DestructableObstacles));
	}
}
