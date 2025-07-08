using UnityEngine;

internal class TankCollisionWatcher
{
	public GameObject enemyTankGameObject;

	public float totalTime;

	public TankCollisionWatcher(GameObject col)
	{
		enemyTankGameObject = col;
	}

	public void IncreaseTime(float val)
	{
		totalTime += val;
	}
}
