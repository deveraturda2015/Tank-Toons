using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CurrentTargetDisplayController : MonoBehaviour
{
	private Image currentTargetBackground;

	private Image CurrentTargetImage;

	private Text currentTargetTypeText;

	private Text currentTargetDetailsText;

	private Text CurrentTargetRewardText;

	private CanvasGroup panelCanvasGourp;

	public Sprite[] TargetSprites;

	private void Start()
	{
		currentTargetBackground = GameObject.Find("CurrentTargetBackground").GetComponent<Image>();
		panelCanvasGourp = currentTargetBackground.GetComponent<CanvasGroup>();
		panelCanvasGourp.alpha = 0f;
		currentTargetTypeText = currentTargetBackground.transform.Find("CurrentTargetTypeText").GetComponent<Text>();
		currentTargetDetailsText = currentTargetBackground.transform.Find("CurrentTargetDetailsText").GetComponent<Text>();
		CurrentTargetRewardText = currentTargetBackground.transform.Find("CurrentTargetRewardText").GetComponent<Text>();
		CurrentTargetImage = currentTargetBackground.transform.Find("CurrentTargetImage").GetComponent<Image>();
		LittleTarget currentTarget = GlobalCommons.Instance.globalGameStats.LittleTargetsTracker.CurrentTarget;
		CurrentTargetImage.sprite = TargetSprites[(int)currentTarget.TargetType];
		switch (currentTarget.TargetType)
		{
		case LittleTarget.TargetTypes.CollectCoins:
			currentTargetTypeText.text = LocalizationManager.Instance.GetLocalizedText("ObjectiveType1");
			break;
		case LittleTarget.TargetTypes.DestroyBricks:
			currentTargetTypeText.text = LocalizationManager.Instance.GetLocalizedText("ObjectiveType2");
			break;
		case LittleTarget.TargetTypes.DestroyCrates:
			currentTargetTypeText.text = LocalizationManager.Instance.GetLocalizedText("ObjectiveType3");
			break;
		case LittleTarget.TargetTypes.DestroySpawners:
			currentTargetTypeText.text = LocalizationManager.Instance.GetLocalizedText("ObjectiveType4");
			break;
		case LittleTarget.TargetTypes.DestroyTanks:
			currentTargetTypeText.text = LocalizationManager.Instance.GetLocalizedText("ObjectiveType5");
			break;
		case LittleTarget.TargetTypes.DestroyTowers:
			currentTargetTypeText.text = LocalizationManager.Instance.GetLocalizedText("ObjectiveType6");
			break;
		case LittleTarget.TargetTypes.ExplodeBarrels:
			currentTargetTypeText.text = LocalizationManager.Instance.GetLocalizedText("ObjectiveType7");
			break;
		default:
			throw new Exception("Unknown objective type to display text for");
		}
		currentTargetDetailsText.text = GlobalCommons.Instance.globalGameStats.LittleTargetsTracker.CurrentTargetValue.ToString() + "/" + currentTarget.ValueNeeded.ToString();
		CurrentTargetRewardText.text = GlobalCommons.Instance.globalGameStats.LittleTargetsTracker.CurrentLittleTargetCoinsBonus.ToString();
		if (GlobalCommons.Instance.gameplayMode != 0 && GlobalCommons.Instance.gameplayMode != GlobalCommons.GameplayModes.ArenaLevel)
		{
			DisableAndRemove();
			return;
		}
		Sequence sequence = DOTween.Sequence();
		sequence.SetUpdate(isIndependentUpdate: true);
		sequence.Append(panelCanvasGourp.DOFade(1f, 0.33f));
		sequence.AppendInterval(2f);
		sequence.Append(panelCanvasGourp.DOFade(0f, 0.33f));
		sequence.OnCompleteWithCoroutine(DisableAndRemove);
	}

	private void DisableAndRemove()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
