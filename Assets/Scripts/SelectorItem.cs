using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SelectorItem
{
	private Vector2 originalAnchoredPosition;

	private RectTransform rt;

	private Image img;

	internal GameObject go;

	private Vector2 inPosition;

	private Vector2 outPosition;

	private Material initialMaterial;

	private Image ItemImage;

	private CanvasGroup CanvasGroup;

	public SelectorItem(Vector2 originalAnchoredPosition, GameObject itemGO)
	{
		this.originalAnchoredPosition = originalAnchoredPosition;
		rt = itemGO.GetComponent<RectTransform>();
		CanvasGroup = itemGO.AddComponent<CanvasGroup>();
		ItemImage = itemGO.GetComponent<Image>();
		CanvasGroup.alpha = 0f;
		outPosition = originalAnchoredPosition * 1.1f;
		inPosition = originalAnchoredPosition * 0.85f;
		go = rt.gameObject;
		img = go.GetComponent<Image>();
		initialMaterial = img.material;
	}

	public void FlashItem()
	{
		img.material = Materials.FlashWhiteMaterial;
		img.material.SetFloat("_FlashAmount", 0f);
		float num = 1.5f;
		RectTransform rectTransform = img.rectTransform;
		Vector3 localScale = img.rectTransform.localScale;
		float x = localScale.x * num;
		Vector3 localScale2 = img.rectTransform.localScale;
		float y = localScale2.y * num;
		Vector3 localScale3 = img.rectTransform.localScale;
		rectTransform.localScale = new Vector3(x, y, localScale3.z);
		float num2 = 0.2f;
		img.rectTransform.DOScale(1f, num2);
		img.material.SetFloat("_FlashAmount", 1f);
		img.material.DOFloat(0f, "_FlashAmount", num2);
		ItemImage.DOKill();
		CanvasGroup.DOFade(0f, 0.2f).SetDelay(num2);
	}

	public void SetInitialMaterial()
	{
		img.material = initialMaterial;
	}

	public void FadeIn(float delay)
	{
		float duration = 0.2f;
		rt.DOKill();
		rt.anchoredPosition = inPosition;
		rt.DOAnchorPos(originalAnchoredPosition, duration).SetDelay(delay);
		ItemImage.DOKill();
		CanvasGroup.DOFade(1f, duration).SetDelay(delay);
	}

	public void FadeOut(float delay)
	{
		float duration = 0.2f;
		rt.DOKill();
		rt.DOAnchorPos(outPosition, duration).SetDelay(delay);
		ItemImage.DOKill();
		CanvasGroup.DOFade(0f, duration).SetDelay(delay);
	}
}
