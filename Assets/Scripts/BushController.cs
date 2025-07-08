using DG.Tweening;
using UnityEngine;

public class BushController : MonoBehaviour
{
	private bool destroyed;

	private SpriteRenderer bushSR;

	private const float PLAYERHALFWIDTH = 0.445f;

	private const float BUSHHALFWIDTH = 0.6f;

	private static float playerAndBushHalfWidthsCombined = 1.04500008f;

	private static float playerArea = 0.673285f;

	public Vector3 cachedPosition;

	private int showFrameCounter = int.MinValue;

	internal static int showFrameOffset = 1;

	private static float hiddenTimestamp;

	private const float PLAYER_HIDE_TIMEOUT = 0.75f;

	private static bool playerHidden;

	private static bool playerHiddenToReport;

	private static int framesToSkip = 5;

	public static bool PlayerHidden => playerHiddenToReport;

	private void Start()
	{
		base.transform.DOScale(0.5f, 0f);
		bushSR = GetComponent<SpriteRenderer>();
		bushSR.DOFade(0f, 0f);
		GameplayCommons.Instance.enemiesTracker.Track(this);
		cachedPosition = base.transform.position;
	}

	private void Update()
	{
		if (showFrameCounter > 0)
		{
			showFrameCounter--;
			if (showFrameCounter == 0)
			{
				ActivateBush();
			}
		}
	}

	public void ShowBush()
	{
		showFrameCounter = showFrameOffset;
		showFrameOffset++;
	}

	private void ActivateBush()
	{
		if (EffectsSpawner.IsFarFromCamera(base.transform.position))
		{
			base.transform.localScale = new Vector3(1f, 1f, 1f);
			bushSR.SetAlpha(1f);
			return;
		}
		float duration = 0.7f;
		float delay = UnityEngine.Random.Range(0.1f, 0.5f);
		base.transform.DOScale(1f, duration).SetEase(Ease.OutQuad).SetUpdate(isIndependentUpdate: true)
			.SetDelay(delay);
		bushSR.DOFade(1f, duration).SetEase(Ease.OutQuad).SetUpdate(isIndependentUpdate: true)
			.SetDelay(delay);
	}

	public void DestroyBush()
	{
		if (!destroyed)
		{
			destroyed = true;
			GameplayCommons.Instance.effectsSpawner.CreateLeavesFallEffect(base.transform.position);
			GameplayCommons.Instance.enemiesTracker.UnTrack(this);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public static void CheckPlayerHidden()
	{
		if (framesToSkip > 0)
		{
			framesToSkip--;
			return;
		}
		framesToSkip = UnityEngine.Random.Range(5, 10);
		Vector3 position = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
		float num = 0f;
		for (int i = 0; i < GameplayCommons.Instance.enemiesTracker.AllBushes.Count; i++)
		{
			BushController bushController = GameplayCommons.Instance.enemiesTracker.AllBushes[i];
			if (Mathf.Abs(bushController.cachedPosition.x - position.x) < playerAndBushHalfWidthsCombined && Mathf.Abs(bushController.cachedPosition.y - position.y) < playerAndBushHalfWidthsCombined)
			{
				num += GetPlayerBushHiddenArea(bushController.cachedPosition, position);
			}
		}
		if (num >= playerArea)
		{
			if (!playerHidden)
			{
				playerHidden = true;
				hiddenTimestamp = Time.fixedTime;
			}
			playerHiddenToReport = (Time.fixedTime - 0.75f > hiddenTimestamp);
		}
		else
		{
			playerHiddenToReport = false;
			playerHidden = false;
		}
	}

	public static void ResetHiddenTimestamp()
	{
		if (playerHidden)
		{
			hiddenTimestamp = Time.fixedTime;
		}
	}

	private static float GetPlayerBushHiddenArea(Vector3 bushPosition, Vector3 playerPosition)
	{
		float num = Mathf.Max(bushPosition.x - 0.6f, playerPosition.x - 0.445f);
		float num2 = Mathf.Min(bushPosition.x + 0.6f, playerPosition.x + 0.445f);
		float num3 = Mathf.Max(bushPosition.y - 0.6f, playerPosition.y - 0.445f);
		float num4 = Mathf.Min(bushPosition.y + 0.6f, playerPosition.y + 0.445f);
		return Mathf.Abs(num2 - num) * Mathf.Abs(num4 - num3);
	}
}
