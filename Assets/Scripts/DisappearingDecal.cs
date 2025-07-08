using UnityEngine;

public class DisappearingDecal : MonoBehaviour
{
	private const float removeTimeout = 10f;

	private const float forcedRemoveTimeout = 20f;

	private float spawnedTimestamp;

	private float alphaEffectTime = 1f;

	private SpriteRenderer sr;

	public Sprite ExplosionDecal2;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		if (UnityEngine.Random.value > 0.5f)
		{
			sr.sprite = ExplosionDecal2;
		}
		spawnedTimestamp = Time.fixedTime;
	}

	private void Update()
	{
		float num = Time.fixedTime - spawnedTimestamp;
		if (num > 10f && IsOffScreen())
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else if (num > 20f - alphaEffectTime)
		{
			float num2 = (20f - num) / alphaEffectTime;
			if (num2 >= 0f)
			{
				SpriteRenderer spriteRenderer = sr;
				Color color = sr.color;
				float r = color.r;
				Color color2 = sr.color;
				float g = color2.g;
				Color color3 = sr.color;
				spriteRenderer.color = new Color(r, g, color3.b, num2);
			}
			if (num > 20f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	private bool IsOffScreen()
	{
		Vector3 position = base.transform.position;
		float x = position.x;
		Vector3 position2 = Camera.main.transform.position;
		int result;
		if (!(Mathf.Abs(x - position2.x) > GlobalCommons.Instance.DynamicHorizontalScreenBorderPlusOneCell))
		{
			Vector3 position3 = base.transform.position;
			float y = position3.y;
			Vector3 position4 = Camera.main.transform.position;
			result = ((Mathf.Abs(y - position4.y) > GlobalCommons.Instance.DynamicVerticalScreenBorderDistancePlusOneCell) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}
}
