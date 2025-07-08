using UnityEngine;

public class TileFlyIn : MonoBehaviour
{
	private SpriteRenderer tileSr;

	private float alpha;

	private float scale;

	private float awakeTime;

	private Vector3 initialCoords;

	private Vector3 targetCoords;

	private float lerpPosition;

	private static float lerpTime = 0.25f;

	private Quaternion initialRotation;

	private static Vector2 awakeTimeBounds = new Vector2(0.1f, 1f);

	private int updateCounter;

	public bool LeaveAfterTilemapCreation;

	private bool tileSetIn;

	private bool playedSound;

	private int destroyDelayFrames = int.MinValue;

	internal static int destroyDelayFrameOffset = 1;

	private bool destroySROnly;

	public static float AllTilesSetTime => awakeTimeBounds.y + lerpTime;

	private void Start()
	{
		tileSr = GetComponent<SpriteRenderer>();
		SetSpriteAlpha(alpha);
		if ((bool)GetComponent<BoxCollider2D>() || (bool)GetComponent<PolygonCollider2D>())
		{
			destroySROnly = true;
		}
		Vector2 vector = GameplayCommons.Instance.playersTankController.transform.position;
		if (LeaveAfterTilemapCreation)
		{
			return;
		}
		float x = vector.x;
		Vector3 position = base.transform.position;
		if (!(Mathf.Abs(x - position.x) > GlobalCommons.Instance.DynamicHorizontalScreenBorderPlusOneCell))
		{
			float y = vector.y;
			Vector3 position2 = base.transform.position;
			if (!(Mathf.Abs(y - position2.y) > GlobalCommons.Instance.DynamicVerticalScreenBorderDistancePlusOneCell))
			{
				return;
			}
		}
		tileSetIn = true;
		tileSr.enabled = false;
	}

	private void Update()
	{
		if (tileSetIn)
		{
			if (destroyDelayFrames > 0)
			{
				destroyDelayFrames--;
				if (destroyDelayFrames == 0)
				{
					FinalizeTile();
				}
			}
			return;
		}
		if (updateCounter > 1)
		{
			if (Time.fixedTime > awakeTime)
			{
				if (!playedSound)
				{
					playedSound = true;
					SoundManager.instance.PlayTileSetInSound();
				}
				lerpPosition += Time.deltaTime / lerpTime;
				alpha = Mathf.Lerp(0f, 1f, lerpPosition);
				scale = Mathf.Lerp(0.33f, 1f, lerpPosition);
				SetSpriteAlpha(alpha);
				SetScale(scale);
				base.transform.rotation = Quaternion.Lerp(initialRotation, Quaternion.identity, lerpPosition);
				base.transform.position = Vector3.Lerp(initialCoords, targetCoords, lerpPosition);
				if (alpha == 1f && scale == 1f && base.transform.rotation == Quaternion.identity && base.transform.position == targetCoords)
				{
					base.transform.rotation = Quaternion.identity;
					tileSetIn = true;
				}
			}
		}
		else if (updateCounter == 1)
		{
			Initialize();
		}
		updateCounter++;
	}

	private void Initialize()
	{
		SetScale(scale);
		base.transform.rotation = (initialRotation = UnityEngine.Random.rotation);
		awakeTime = Time.fixedTime + UnityEngine.Random.Range(awakeTimeBounds.x, awakeTimeBounds.y);
		targetCoords = base.transform.position;
		Transform transform = base.transform;
		Vector3 position = base.transform.position;
		float x = position.x;
		Vector3 position2 = base.transform.position;
		float y = position2.y;
		Vector3 position3 = base.transform.position;
		transform.position = new Vector3(x, y, position3.z);
		initialCoords = base.transform.position;
	}

	private void SetSpriteAlpha(float val)
	{
		SpriteRenderer spriteRenderer = tileSr;
		Color color = tileSr.color;
		float r = color.r;
		Color color2 = tileSr.color;
		float g = color2.g;
		Color color3 = tileSr.color;
		spriteRenderer.color = new Color(r, g, color3.b, val);
	}

	private void SetScale(float val)
	{
		base.transform.localScale = new Vector3(val, val, val);
	}

	public void FinalizeTileState()
	{
		tileSr.enabled = false;
		destroyDelayFrames = destroyDelayFrameOffset;
		destroyDelayFrameOffset++;
	}

	public void FinalizeTile()
	{
		if (!LeaveAfterTilemapCreation)
		{
			if (destroySROnly)
			{
				UnityEngine.Object.Destroy(tileSr);
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		UnityEngine.Object.Destroy(this);
	}
}
