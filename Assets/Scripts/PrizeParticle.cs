using UnityEngine;

public class PrizeParticle : MonoBehaviour
{
	public Sprite[] particleSprites;

	private float rotationFactor;

	private void Start()
	{
		base.transform.rotation = UnityEngine.Random.rotation;
		GetComponent<SpriteRenderer>().sprite = particleSprites[Random.Range(0, particleSprites.Length)];
		float num = UnityEngine.Random.Range(0.8f, 1.2f);
		Transform transform = base.transform;
		Vector3 localScale = base.transform.localScale;
		float x = localScale.x * num;
		Vector3 localScale2 = base.transform.localScale;
		float y = localScale2.y * num;
		Vector3 localScale3 = base.transform.localScale;
		transform.localScale = new Vector3(x, y, localScale3.z * num);
		rotationFactor = UnityEngine.Random.Range(15f, 30f);
		if (UnityEngine.Random.value > 0.5f)
		{
			rotationFactor *= -1f;
		}
	}

	private void Update()
	{
		base.transform.Rotate(new Vector3(rotationFactor, rotationFactor, rotationFactor));
		Vector3 position = base.transform.position;
		if (position.y < 0f - GlobalCommons.Instance.DynamicVerticalScreenBorderDistancePlusOneCell)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
