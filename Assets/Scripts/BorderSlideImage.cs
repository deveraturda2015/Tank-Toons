using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BorderSlideImage : MonoBehaviour
{
	public enum SlidePosition
	{
		Up,
		Right,
		Down,
		Left
	}

	private Image borderSlideImage;

	public SlidePosition currentPosition;

	private Vector2 parentRectDimensions;

	private float moveSpeed = 1000f;

	private bool spawnChildren = true;

	private int childrenToSpawn;

	private int childrenToSpawnMax = 10;

	private float nextChildAlpha;

	private void Start()
	{
		borderSlideImage = GetComponent<Image>();
		childrenToSpawn = childrenToSpawnMax;
		RectTransform component = base.transform.parent.GetComponent<RectTransform>();
		parentRectDimensions = new Vector2(component.rect.width - borderSlideImage.rectTransform.rect.width, component.rect.height - borderSlideImage.rectTransform.rect.height);
		if (spawnChildren)
		{
			float num = 0.66f;
			borderSlideImage.SetAlpha(num);
			while (childrenToSpawn > 0)
			{
				childrenToSpawn--;
				nextChildAlpha += num / (float)childrenToSpawnMax;
				GameObject gameObject = UnityEngine.Object.Instantiate(base.gameObject);
				gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, nextChildAlpha);
				gameObject.GetComponent<BorderSlideImage>().Initialize(spawnChildren: false);
				gameObject.transform.SetParent(base.transform.parent, worldPositionStays: false);
				DoCoordsStep(deltaTimeIrrelevant: true);
				DoCoordsStep(deltaTimeIrrelevant: true);
			}
		}
		borderSlideImage.enabled = false;
	}

	public void Initialize(bool spawnChildren)
	{
		this.spawnChildren = spawnChildren;
	}

	private void Update()
	{
		DoCoordsStep(deltaTimeIrrelevant: true);
	}

	public void Activate()
	{
		borderSlideImage.enabled = true;
		Color color = borderSlideImage.color;
		float a = color.a;
		borderSlideImage.SetAlpha(0f);
		borderSlideImage.DOFade(a, 0.5f);
	}

	private void DoCoordsStep(bool deltaTimeIrrelevant = false)
	{
		float num = (!deltaTimeIrrelevant) ? (moveSpeed * Time.deltaTime) : 10f;
		switch (currentPosition)
		{
		case SlidePosition.Up:
		{
			Vector2 anchoredPosition5 = borderSlideImage.rectTransform.anchoredPosition;
			float num4 = anchoredPosition5.x + num;
			float num5 = 0f;
			if (num4 >= parentRectDimensions.x / 2f)
			{
				num5 = parentRectDimensions.x / 2f - num4;
				num4 = parentRectDimensions.x / 2f;
				currentPosition = SlidePosition.Right;
			}
			RectTransform rectTransform3 = borderSlideImage.rectTransform;
			float x = num4;
			Vector2 anchoredPosition6 = borderSlideImage.rectTransform.anchoredPosition;
			rectTransform3.anchoredPosition = new Vector2(x, anchoredPosition6.y - num5);
			break;
		}
		case SlidePosition.Right:
		{
			Vector2 anchoredPosition3 = borderSlideImage.rectTransform.anchoredPosition;
			float num2 = anchoredPosition3.y - num;
			float num3 = 0f;
			if (num2 <= (0f - parentRectDimensions.y) / 2f)
			{
				num3 = (0f - parentRectDimensions.y) / 2f - num2;
				num2 = (0f - parentRectDimensions.y) / 2f;
				currentPosition = SlidePosition.Down;
			}
			RectTransform rectTransform2 = borderSlideImage.rectTransform;
			Vector2 anchoredPosition4 = borderSlideImage.rectTransform.anchoredPosition;
			rectTransform2.anchoredPosition = new Vector2(anchoredPosition4.x - num3, num2);
			break;
		}
		case SlidePosition.Down:
		{
			Vector2 anchoredPosition7 = borderSlideImage.rectTransform.anchoredPosition;
			float num4 = anchoredPosition7.x - num;
			float num5 = 0f;
			if (num4 <= (0f - parentRectDimensions.x) / 2f)
			{
				num5 = (0f - parentRectDimensions.x) / 2f - num4;
				num4 = (0f - parentRectDimensions.x) / 2f;
				currentPosition = SlidePosition.Left;
			}
			RectTransform rectTransform4 = borderSlideImage.rectTransform;
			float x2 = num4;
			Vector2 anchoredPosition8 = borderSlideImage.rectTransform.anchoredPosition;
			rectTransform4.anchoredPosition = new Vector2(x2, anchoredPosition8.y + num5);
			break;
		}
		case SlidePosition.Left:
		{
			Vector2 anchoredPosition = borderSlideImage.rectTransform.anchoredPosition;
			float num2 = anchoredPosition.y + num;
			float num3 = 0f;
			if (num2 >= parentRectDimensions.y / 2f)
			{
				num3 = parentRectDimensions.y / 2f - num2;
				num2 = parentRectDimensions.y / 2f;
				currentPosition = SlidePosition.Up;
			}
			RectTransform rectTransform = borderSlideImage.rectTransform;
			Vector2 anchoredPosition2 = borderSlideImage.rectTransform.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(anchoredPosition2.x + num3, num2);
			break;
		}
		}
	}
}
