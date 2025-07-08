using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorItemSelectionMenu : MonoBehaviour
{
	private LevelEditorController lec;

	private GameObject selectedItemGO;

	private int hideCtr;

	private bool isLocked;

	private List<SelectorItem> selectorItems;

	private Image backgroundImage;

	private Text HeaderText;

	internal static readonly string LockedItems = "OPQUVWRST%^&";

	public void InitializeMenu()
	{
		backgroundImage = GetComponent<Image>();
		backgroundImage.SetAlpha(0f);
		HeaderText = GameObject.Find("HeaderText").GetComponent<Text>();
		HeaderText.SetAlpha(0f);
		lec = UnityEngine.Object.FindObjectOfType<LevelEditorController>();
		int num = 0;
		int num2 = 0;
		float num3 = 75f;
		int num4 = 0;
		Vector3 vector = new Vector3(0f, 155f, 0f);
		selectorItems = new List<SelectorItem>();
		foreach (KeyValuePair<string, char> buttonNamesToLevelChar in LevelEditorController.ButtonNamesToLevelChars)
		{
			if (buttonNamesToLevelChar.Value != '@')
			{
				num4 = ((num2 != 0) ? 10 : 9);
				Vector3 v = new Vector3(vector.x - (float)(num4 / 2) * num3 + (float)num * num3 + num3 / 2f, vector.y - (float)num2 * num3, vector.z);
				if (num2 == 0)
				{
					v = new Vector3(v.x - num3, v.y, v.z);
				}
				num++;
				if (num == num4)
				{
					num = 0;
					num2++;
				}
				GameObject gameObject = UnityEngine.Object.Instantiate(Prefabs.levelEditorItemPrefab);
				gameObject.name = buttonNamesToLevelChar.Key;
				gameObject.transform.SetParent(base.transform, worldPositionStays: false);
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.anchoredPosition = v;
				Image component2 = gameObject.GetComponent<Image>();
				component2.raycastTarget = true;
				component2.sprite = gameObject.GetComponent<EditorItem>().GetItemSpriteFromChar(buttonNamesToLevelChar.Value);
				Button buttonComponent = gameObject.AddComponent<Button>();
				if (LockedItems.Contains(LevelEditorController.ButtonNamesToLevelChars[buttonComponent.name].ToString()))
				{
					buttonComponent.onClick.AddListener(delegate
					{
						ComingSoonItemClick();
					});
				}
				else if (CheckButtonAvailable(buttonComponent))
				{
					buttonComponent.onClick.AddListener(delegate
					{
						ItemButtonClick(buttonComponent);
					});
				}
				else
				{
					buttonComponent.onClick.AddListener(delegate
					{
						UnavailableItemClick();
					});
					GameObject gameObject2 = UnityEngine.Object.Instantiate(Prefabs.levelLockedIconPrefab);
					gameObject2.transform.SetParent(gameObject.transform, worldPositionStays: false);
					gameObject2.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
					RectTransform component3 = gameObject2.GetComponent<RectTransform>();
					float num5 = 0.6f;
					RectTransform rectTransform = component3;
					float x = num5;
					float y = num5;
					Vector3 localScale = component3.localScale;
					rectTransform.localScale = new Vector3(x, y, localScale.z);
				}
				SelectorItem item = new SelectorItem(component.anchoredPosition, gameObject);
				selectorItems.Add(item);
			}
		}
	}

	private void ComingSoonItemClick()
	{
		if (!isLocked)
		{
			SoundManager.instance.PlayCantPlaceMineSound();
		}
	}

	private bool CheckButtonAvailable(Button buttonToCheck)
	{
		if (GlobalCommons.Instance.globalGameStats.IsPayingPlayer || (GlobalCommons.Instance.globalGameStats.DoubleCoinsPurchased ? true : false))
		{
			return true;
		}
		if (buttonToCheck.name.Contains("Spawner"))
		{
			int num = int.Parse(buttonToCheck.name.Substring(7));
			if (num > GlobalCommons.Instance.globalGameStats.EditorItemsLevel)
			{
				return false;
			}
		}
		if (buttonToCheck.name.Contains("Turret"))
		{
			int num2 = int.Parse(buttonToCheck.name.Substring(6));
			if (num2 > GlobalCommons.Instance.globalGameStats.EditorItemsLevel)
			{
				return false;
			}
		}
		if (buttonToCheck.name.Contains("Boss"))
		{
			int num3 = int.Parse(buttonToCheck.name.Substring(4));
			if (num3 > GlobalCommons.Instance.globalGameStats.EditorItemsLevel)
			{
				return false;
			}
		}
		if (buttonToCheck.name.Contains("EnemySimpleTank"))
		{
			int num4 = int.Parse(buttonToCheck.name.Substring(15));
			if (num4 > GlobalCommons.Instance.globalGameStats.EditorItemsLevel)
			{
				return false;
			}
		}
		return true;
	}

	public void FadeIn()
	{
		isLocked = false;
		float num = 0.1f;
		backgroundImage.DOFade(1f, num);
		HeaderText.DOFade(1f, num);
		float num2 = num;
		for (int i = 0; i < selectorItems.Count; i++)
		{
			selectorItems[i].FadeIn(num2);
			num2 += 0.003f;
		}
	}

	public void FadeOut()
	{
		isLocked = true;
		float num = 0f;
		for (int i = 0; i < selectorItems.Count; i++)
		{
			if (!(selectorItems[i].go == selectedItemGO))
			{
				selectorItems[i].FadeOut(num);
				num += 0.003f;
			}
		}
		HeaderText.DOFade(0f, 0.1f).SetDelay(num);
		backgroundImage.DOFade(0f, 0.6f).SetDelay(num).OnCompleteWithCoroutine(FinalizeFadeout);
	}

	private void FinalizeFadeout()
	{
		base.gameObject.SetActive(value: false);
	}

	private void UnavailableItemClick()
	{
		if (!isLocked)
		{
			SoundManager.instance.PlayCantPlaceMineSound();
			GlobalCommons.Instance.MessagesController.ShowSimpleMessage(LocalizationManager.Instance.GetLocalizedText("EditorItemLockedTxt"));
		}
	}

	private void ItemButtonClick(Button buttonClicked)
	{
		if (isLocked)
		{
			return;
		}
		selectedItemGO = buttonClicked.gameObject;
		for (int i = 0; i < selectorItems.Count; i++)
		{
			if (selectorItems[i].go == buttonClicked.gameObject)
			{
				selectorItems[i].FlashItem();
			}
			else
			{
				selectorItems[i].SetInitialMaterial();
			}
		}
		SoundManager.instance.PlayButtonClickSound();
		lec.SelectItem(LevelEditorController.ButtonNamesToLevelChars[buttonClicked.name], buttonClicked.image.sprite);
		FadeOut();
	}
}
