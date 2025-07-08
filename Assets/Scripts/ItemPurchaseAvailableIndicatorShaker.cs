using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ItemPurchaseAvailableIndicatorShaker : MonoBehaviour
{
	private Image newLevelItemImage;

	private float newLevelItemShakeTimestamp;

	private float newLevelItemRotationPunch = 10f;

	private void Start()
	{
		newLevelItemImage = GetComponent<Image>();
		ReinitShakeTimeStamp(initial: true);
	}

	private void ReinitShakeTimeStamp(bool initial = false)
	{
		if (initial)
		{
			newLevelItemShakeTimestamp = Time.fixedTime + UnityEngine.Random.Range(0.5f, 2f);
		}
		else
		{
			newLevelItemShakeTimestamp = Time.fixedTime + UnityEngine.Random.Range(1.5f, 3f);
		}
	}

	private void Update()
	{
		if (Time.fixedTime > newLevelItemShakeTimestamp)
		{
			ReinitShakeTimeStamp();
			newLevelItemRotationPunch *= -1f;
			newLevelItemImage.transform.DOKill();
			newLevelItemImage.transform.DOPunchRotation(new Vector3(0f, 0f, newLevelItemRotationPunch), 0.25f);
		}
	}
}
