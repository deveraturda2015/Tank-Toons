using DG.Tweening;
using UnityEngine;

public class AutoaimPointerController : MonoBehaviour
{
	private SpriteRenderer pointerSR;

	private bool isEnabled;

	private float fadeTime = 0.15f;

	private float multiplicationFactor = 10f;

	private float initialScale;

	private float bounceScale = 1.2f;

	private void Start()
	{
		pointerSR = GetComponent<SpriteRenderer>();
		pointerSR.transform.localScale = pointerSR.transform.localScale * 1.35f;
		Vector3 localScale = pointerSR.transform.localScale;
		initialScale = localScale.x;
		pointerSR.SetAlpha(0f);
	}

	public void BouncePointer()
	{
		if (isEnabled && !(Time.timeScale < 0.01f))
		{
			pointerSR.transform.DOKill();
			Transform transform = pointerSR.transform;
			float x = bounceScale;
			float y = bounceScale;
			Vector3 localScale = pointerSR.transform.localScale;
			transform.localScale = new Vector3(x, y, localScale.z);
			pointerSR.transform.DOScale(initialScale, 0.12f);
			float z = UnityEngine.Random.Range(-13f, 13f);
			pointerSR.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, z));
			pointerSR.transform.DORotate(Vector3.zero, 0.12f);
		}
	}

	private void Update()
	{
		if (GameplayCommons.Instance.playersTankController.TankBase == null || Time.timeScale < 0.01f)
		{
			return;
		}
		if ((GameplayCommons.Instance.touchesController.ShootTouchController.ShowAutoaimCursor() || !Input.touchSupported) && !GameplayCommons.Instance.playersTankController.PlayerDead && GameplayCommons.Instance.playersTankController.PlayerActive && !GameplayCommons.Instance.levelStateController.GameplayStopped)
		{
			Vector3? autoaimVectorOrPosition = AutoAimHelper.GetAutoaimVectorOrPosition(getPosition: true);
			Vector3 value = autoaimVectorOrPosition.HasValue ? (value = autoaimVectorOrPosition.Value) : (value = GameplayCommons.Instance.playersTankController.TankBase.transform.position);
			if (AutoAimHelper.CheckTargetChangedFlag())
			{
				multiplicationFactor = 20f;
			}
			if (multiplicationFactor > 0f)
			{
				multiplicationFactor -= 30f * Time.deltaTime;
				if (multiplicationFactor < 0f)
				{
					multiplicationFactor = 0f;
				}
			}
			if (!isEnabled)
			{
				isEnabled = true;
				pointerSR.DOKill();
				pointerSR.DOFade(1f, fadeTime);
				base.transform.position = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
				float num = 1.3f;
				base.transform.DOKill();
				Transform transform = base.transform;
				float x = initialScale * num;
				float y = initialScale * num;
				Vector3 localScale = pointerSR.transform.localScale;
				transform.localScale = new Vector3(x, y, localScale.z);
				base.transform.DOScale(initialScale, fadeTime);
			}
			else
			{
				float x2 = value.x;
				Vector3 position = base.transform.position;
				float num2 = (x2 + position.x * multiplicationFactor) / (multiplicationFactor + 1f);
				float y2 = value.y;
				Vector3 position2 = base.transform.position;
				float num3 = (y2 + position2.y * multiplicationFactor) / (multiplicationFactor + 1f);
				Transform transform2 = base.transform;
				float x3 = num2;
				float y3 = num3;
				Vector3 position3 = base.transform.position;
				transform2.position = new Vector3(x3, y3, position3.z);
			}
		}
		else if (isEnabled)
		{
			isEnabled = false;
			pointerSR.DOKill();
			pointerSR.DOFade(0f, fadeTime);
		}
	}
}
