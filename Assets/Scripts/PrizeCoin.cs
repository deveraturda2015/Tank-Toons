using DG.Tweening;
using UnityEngine;

public class PrizeCoin : MonoBehaviour
{
	private PrizeSceneController psc;

	private SpriteRenderer coinSR;

	private void Start()
	{
		coinSR = GetComponent<SpriteRenderer>();
		coinSR.SetAlpha(0f);
		coinSR.DOFade(1f, 0.1f);
	}

	private void Update()
	{
	}

	public void InitCoin(PrizeSceneController psc)
	{
		this.psc = psc;
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.name == "FloorGO")
		{
			psc.effectsSpawner.SpawnPrizeCoinPickupEffect(base.transform.position);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
