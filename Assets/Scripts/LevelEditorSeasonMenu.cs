using UnityEngine;
using UnityEngine.UI;

public class LevelEditorSeasonMenu : MonoBehaviour
{
	public Sprite ItemCheckedSprite;

	public Sprite ItemUncheckedSprite;

	private Button OKButton;

	private Text SummerText;

	private Text DesertText;

	private Text WinterText;

	private Button SummerButton;

	private Button DesertButton;

	private Button WinterButton;

	private Image BushImage;

	private LevelEditorController lec;

	private void Start()
	{
		lec = UnityEngine.Object.FindObjectOfType<LevelEditorController>();
		BushImage = base.transform.Find("BushImage").GetComponent<Image>();
		OKButton = base.transform.Find("OKButton").GetComponent<Button>();
		OKButton.onClick.AddListener(delegate
		{
			OKButtonClick();
		});
		SummerText = base.transform.Find("SummerText").GetComponent<Text>();
		DesertText = base.transform.Find("DesertText").GetComponent<Text>();
		WinterText = base.transform.Find("WinterText").GetComponent<Text>();
		SummerButton = base.transform.Find("SummerButton").GetComponent<Button>();
		SummerButton.onClick.AddListener(delegate
		{
			SelectSeason(TileMap.TilesType.SummerTiles);
		});
		DesertButton = base.transform.Find("DesertButton").GetComponent<Button>();
		DesertButton.onClick.AddListener(delegate
		{
			SelectSeason(TileMap.TilesType.DesertTiles);
		});
		WinterButton = base.transform.Find("WinterButton").GetComponent<Button>();
		WinterButton.onClick.AddListener(delegate
		{
			SelectSeason(TileMap.TilesType.WinterTiles);
		});
		RefreshCheckboxesState();
	}

	private void RefreshCheckboxesState()
	{
		SummerButton.image.sprite = ItemUncheckedSprite;
		DesertButton.image.sprite = ItemUncheckedSprite;
		WinterButton.image.sprite = ItemUncheckedSprite;
		switch (LevelEditorController.LevelToWorkWithTilesType)
		{
		case TileMap.TilesType.SummerTiles:
			SummerButton.image.sprite = ItemCheckedSprite;
			BushImage.sprite = lec.BushSummerSprite;
			break;
		case TileMap.TilesType.DesertTiles:
			DesertButton.image.sprite = ItemCheckedSprite;
			BushImage.sprite = lec.BushDesertSprite;
			break;
		case TileMap.TilesType.WinterTiles:
			WinterButton.image.sprite = ItemCheckedSprite;
			BushImage.sprite = lec.BushWinterSprite;
			break;
		}
	}

	private void SelectSeason(TileMap.TilesType tilesType)
	{
		LevelEditorController.LevelToWorkWithTilesType = tilesType;
		SoundManager.instance.PlayButtonClickSound();
		RefreshCheckboxesState();
	}

	private void OKButtonClick()
	{
		lec.RefreshSceneryButtonSprite();
		SoundManager.instance.PlayButtonClickSound();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	private void Update()
	{
	}
}
