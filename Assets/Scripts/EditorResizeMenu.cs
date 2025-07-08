using UnityEngine;
using UnityEngine.UI;

public class EditorResizeMenu : MonoBehaviour
{
	private enum ResizeType
	{
		Shrink,
		Expand
	}

	private Button OKButton;

	private Text HeaderText;

	private Text WidthText;

	private Text HeightText;

	private Button ResizeUpwardButton;

	private Button ResizeRightButton;

	private Button ResizeLeftButton;

	private Button ResizeDownButton;

	private Button DownsizeRightButton;

	private Button DownsizeLeftButton;

	private Button DownsizeDownButton;

	private Button DownsizeUpButton;

	private LevelEditorController levelEditorController;

	private void Start()
	{
		OKButton = base.transform.Find("OKButton").GetComponent<Button>();
		OKButton.onClick.AddListener(delegate
		{
			OkButtonClick();
		});
		levelEditorController = UnityEngine.Object.FindObjectOfType<LevelEditorController>();
		ResizeUpwardButton = base.transform.Find("ResizeUpwardButton").GetComponent<Button>();
		ResizeUpwardButton.onClick.AddListener(delegate
		{
			ResizeClick(ResizeType.Expand, LevelEditorController.LevelResizeDirection.Up);
		});
		ResizeRightButton = base.transform.Find("ResizeRightButton").GetComponent<Button>();
		ResizeRightButton.onClick.AddListener(delegate
		{
			ResizeClick(ResizeType.Expand, LevelEditorController.LevelResizeDirection.Right);
		});
		ResizeLeftButton = base.transform.Find("ResizeLeftButton").GetComponent<Button>();
		ResizeLeftButton.onClick.AddListener(delegate
		{
			ResizeClick(ResizeType.Expand, LevelEditorController.LevelResizeDirection.Left);
		});
		ResizeDownButton = base.transform.Find("ResizeDownButton").GetComponent<Button>();
		ResizeDownButton.onClick.AddListener(delegate
		{
			ResizeClick(ResizeType.Expand, LevelEditorController.LevelResizeDirection.Down);
		});
		DownsizeRightButton = base.transform.Find("DownsizeRightButton").GetComponent<Button>();
		DownsizeRightButton.onClick.AddListener(delegate
		{
			ResizeClick(ResizeType.Shrink, LevelEditorController.LevelResizeDirection.Right);
		});
		DownsizeLeftButton = base.transform.Find("DownsizeLeftButton").GetComponent<Button>();
		DownsizeLeftButton.onClick.AddListener(delegate
		{
			ResizeClick(ResizeType.Shrink, LevelEditorController.LevelResizeDirection.Left);
		});
		DownsizeDownButton = base.transform.Find("DownsizeDownButton").GetComponent<Button>();
		DownsizeDownButton.onClick.AddListener(delegate
		{
			ResizeClick(ResizeType.Shrink, LevelEditorController.LevelResizeDirection.Down);
		});
		DownsizeUpButton = base.transform.Find("DownsizeUpButton").GetComponent<Button>();
		DownsizeUpButton.onClick.AddListener(delegate
		{
			ResizeClick(ResizeType.Shrink, LevelEditorController.LevelResizeDirection.Up);
		});
		HeaderText = base.transform.Find("HeaderText").GetComponent<Text>();
		WidthText = base.transform.Find("WidthText").GetComponent<Text>();
		HeightText = base.transform.Find("HeightText").GetComponent<Text>();
		RefreshDimensionsText();
	}

	private void ResizeClick(ResizeType resizeType, LevelEditorController.LevelResizeDirection resizeDirection)
	{
		switch (resizeType)
		{
		case ResizeType.Expand:
			if (levelEditorController.ExpandLevel(resizeDirection))
			{
				SoundManager.instance.PlayEnemyEmojiSound();
			}
			else
			{
				SoundManager.instance.PlayNotAvailableSound();
			}
			break;
		case ResizeType.Shrink:
			if (levelEditorController.ShrinkLevel(resizeDirection))
			{
				SoundManager.instance.PlayEnemyEmojiSound();
			}
			else
			{
				SoundManager.instance.PlayNotAvailableSound();
			}
			break;
		}
		RefreshDimensionsText();
	}

	private void RefreshDimensionsText()
	{
		Point2 levelDimensions = levelEditorController.GetLevelDimensions();
		WidthText.text = LocalizationManager.Instance.GetLocalizedText("EditorResizeWidth") + levelDimensions.x.ToString();
		HeightText.text = LocalizationManager.Instance.GetLocalizedText("EditorResizeHeight") + levelDimensions.y.ToString();
	}

	private void OkButtonClick()
	{
		SoundManager.instance.PlayButtonClickSound();
		base.gameObject.SetActive(value: false);
	}
}
