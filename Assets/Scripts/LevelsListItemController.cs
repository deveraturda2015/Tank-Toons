using UnityEngine;
using UnityEngine.UI;

public class LevelsListItemController : MonoBehaviour
{
	private UserLevelInfo UserLevelInfo;

	private Text LevelItemText;

	private Button ButtonComponent;

	private RectTransform RectTransform;

	private UserLevelsMenuController UserLevelsMenuController;

	internal Vector2 ItemCoords;

	internal void Init(UserLevelsMenuController userLevelsMenuController, float yPos)
	{
		RectTransform = GetComponent<RectTransform>();
		RectTransform rectTransform = RectTransform;
		Vector2 anchoredPosition = RectTransform.anchoredPosition;
		rectTransform.anchoredPosition = new Vector2(anchoredPosition.x, yPos);
		ItemCoords = RectTransform.anchoredPosition;
		UserLevelsMenuController = userLevelsMenuController;
		LevelItemText = base.transform.Find("LevelItemText").GetComponent<Text>();
		ButtonComponent = GetComponent<Button>();
	}

	internal void FeedLevelInfo(UserLevelInfo userLevelInfo)
	{
		UserLevelInfo = userLevelInfo;
		LevelItemText.text = "\"" + UserLevelInfo.level_name + "\" - " + UserLevelInfo.nickname;
		ButtonComponent.onClick.RemoveAllListeners();
		ButtonComponent.onClick.AddListener(ProcessClick);
	}

	private void ProcessClick()
	{
		UserLevelsMenuController.ProcessLevelItemClick(UserLevelInfo);
	}
}
