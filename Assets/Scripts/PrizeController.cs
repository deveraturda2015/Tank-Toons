using UnityEngine;

public class PrizeController : MonoBehaviour
{
	private PrizeSceneController prizeSceneController;

	private void Start()
	{
		prizeSceneController = UnityEngine.Object.FindObjectOfType<PrizeSceneController>();

	}

	private void Update()
	{
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.name == "FloorGO")
		{
			prizeSceneController.ProcessPrizeFloorTouch(col);
		}
	}
}
