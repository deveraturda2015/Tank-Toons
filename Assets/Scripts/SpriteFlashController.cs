using UnityEngine;

public class SpriteFlashController : MonoBehaviour
{
	private SpriteRenderer spriteRenderer;

	private float spriteFlashAmount;

	private int spriteFlashFrames;

	private bool fadeInInProgress;

	private float spriteAlpha
	{
		get
		{
			Color color = spriteRenderer.color;
			return color.a;
		}
		set
		{
			SpriteRenderer obj = spriteRenderer;
			Color color = spriteRenderer.color;
			float r = color.r;
			Color color2 = spriteRenderer.color;
			float g = color2.g;
			Color color3 = spriteRenderer.color;
			obj.color = new Color(r, g, color3.b, value);
		}
	}

	public bool SpawnCompleted => !fadeInInProgress;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (fadeInInProgress)
		{
			ProcessFadeInLogic();
		}
		else
		{
			ProcessFlashLogic();
		}
	}

	private void ProcessFadeInLogic()
	{
		if (spriteAlpha < 1f)
		{
			spriteAlpha += Time.deltaTime / 0.25f;
		}
		if (!(spriteAlpha >= 1f))
		{
			return;
		}
		if (spriteAlpha > 1f)
		{
			spriteAlpha = 1f;
		}
		if (spriteFlashAmount > 0f)
		{
			spriteFlashAmount -= Time.deltaTime / 0.25f;
			if (spriteFlashAmount < 0f)
			{
				spriteFlashAmount = 0f;
			}
			spriteRenderer.material.SetFloat("_FlashAmount", spriteFlashAmount);
			if (spriteFlashAmount == 0f)
			{
				fadeInInProgress = false;
			}
		}
	}

	private void ProcessFlashLogic()
	{
		if (spriteFlashFrames > 0)
		{
			spriteFlashFrames--;
			if (spriteFlashFrames > 0)
			{
				spriteRenderer.material.SetFloat("_FlashAmount", 1f);
			}
			else
			{
				spriteRenderer.material.SetFloat("_FlashAmount", 0f);
			}
		}
	}

	public void ProcessEnemyFadeIn()
	{
		fadeInInProgress = true;
		spriteFlashAmount = 1f;
		spriteRenderer.material.SetFloat("_FlashAmount", 1f);
		spriteAlpha = 0f;
	}

	public void ProcessFlash()
	{
		if (!fadeInInProgress)
		{
			spriteFlashFrames = 3;
		}
	}
}
