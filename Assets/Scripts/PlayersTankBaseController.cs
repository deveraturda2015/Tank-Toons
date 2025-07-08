using UnityEngine;

public class PlayersTankBaseController : MonoBehaviour
{
	private PlayersTankController ptc;

	private void Start()
	{
		ptc = base.transform.parent.GetComponent<PlayersTankController>();
	}

	private void Update()
	{
	}

	private void OnCollisionStay2D(Collision2D col)
	{
		if (col.collider.gameObject.layer == PhysicsLayers.DestructableObstacles)
		{
			Rigidbody2D component = col.collider.gameObject.GetComponent<Rigidbody2D>();
			if (component != null)
			{
				ptc.UpdateCollisionTicker();
			}
		}
		else if (col.collider.gameObject.layer == PhysicsLayers.EnemyTankBase)
		{
			ptc.UpdateCollisionTicker();
			if (!GameplayCommons.Instance.levelStateController.IsFreezeActive)
			{
				EnemyTankController component2 = col.collider.gameObject.transform.parent.gameObject.GetComponent<EnemyTankController>();
				component2.ProcessPlayerCollision();
			}
		}
	}
}
