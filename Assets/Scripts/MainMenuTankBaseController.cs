using UnityEngine;

public class MainMenuTankBaseController : MonoBehaviour
{
	private MainMenuTankController tankController;

	private void Start()
	{
		tankController = base.transform.parent.GetComponent<MainMenuTankController>();
	}

	private void Update()
	{
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (tankController == null)
		{
			tankController = base.transform.parent.GetComponent<MainMenuTankController>();
		}
		if (col.transform.parent != null)
		{
			MainMenuTankController component = col.transform.parent.gameObject.GetComponent<MainMenuTankController>();
			tankController.ProcessOtherTankCollision(component);
		}
	}
}
