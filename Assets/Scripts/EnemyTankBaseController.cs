using System.Collections.Generic;
using UnityEngine;

public class EnemyTankBaseController : MonoBehaviour
{
	private List<TankCollisionWatcher> tankCollisionWatchers = new List<TankCollisionWatcher>();

	private const float BUMP_PREVENT_TIME_MAX = 1f;

	private EnemyTankController parentTankController;

	private void Start()
	{
		parentTankController = base.transform.parent.gameObject.GetComponent<EnemyTankController>();
	}

	private void OnCollisionStay2D(Collision2D col)
	{
		if (!(col.gameObject.GetComponent<EnemyTankBaseController>() != null))
		{
			return;
		}
		for (int i = 0; i < tankCollisionWatchers.Count; i++)
		{
			TankCollisionWatcher tankCollisionWatcher = tankCollisionWatchers[i];
			if (tankCollisionWatcher.enemyTankGameObject == col.gameObject)
			{
				tankCollisionWatcher.IncreaseTime(Time.fixedDeltaTime);
				if (tankCollisionWatcher.totalTime >= 1f && parentTankController != null)
				{
					parentTankController.ProcessOtherTankStuckBump();
					tankCollisionWatchers.RemoveAt(i);
				}
				return;
			}
		}
		TankCollisionWatcher item = new TankCollisionWatcher(col.gameObject);
		tankCollisionWatchers.Add(item);
	}

	private void OnCollisionExit2D(Collision2D col)
	{
		if (!(col.gameObject.GetComponent<EnemyTankBaseController>() != null))
		{
			return;
		}
		int num = 0;
		while (true)
		{
			if (num < tankCollisionWatchers.Count)
			{
				TankCollisionWatcher tankCollisionWatcher = tankCollisionWatchers[num];
				if (tankCollisionWatcher.enemyTankGameObject == col.gameObject)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		tankCollisionWatchers.RemoveAt(num);
	}
}
