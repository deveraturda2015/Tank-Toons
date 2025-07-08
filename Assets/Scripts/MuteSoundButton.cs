using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MuteSoundButton : MonoBehaviour
{
	public Sprite[] ButtonSprites;

	private Button muteButton;

	private Image buttonImage;

	private float initialBtnScale;

	private void Start()
	{
		muteButton = GetComponent<Button>();
        muteButton.onClick.AddListener(delegate
        {
            MuteButtonClick();
        });
        buttonImage = GetComponent<Image>();
		UpdateButtonSprite();
		Vector3 localScale = base.transform.localScale;
		initialBtnScale = localScale.x;
	}

	public void MuteButtonClick()
	{
		float num = 0.2f * initialBtnScale;
		RectTransform component = muteButton.GetComponent<RectTransform>();
		component.DOKill(complete: true);
		component.DOPunchScale(new Vector3(num, num, num), 0.25f);
		component.SetSiblingIndex(component.gameObject.transform.parent.childCount - 1);
		SoundManager.instance.ToggleSoundMute();
		UpdateButtonSprite();
		SoundManager.instance.PlayButtonClickSound();
	}

	private void UpdateButtonSprite()
	{
		Sprite sprite = (!SoundManager.soundMuted) ? ButtonSprites[0] : ButtonSprites[1];
		buttonImage.sprite = sprite;
	}
}
