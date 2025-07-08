using UnityEngine;
using UnityEngine.UI;

public class LevelNumComponent : MonoBehaviour
{
	public bool RandomBG;

	public Sprite[] BGImages;

	private int levelNumber;

	public int LevelNumber
	{
		get
		{
			return levelNumber;
		}
		set
		{
			levelNumber = value;
		}
	}

	private void Start()
	{
		if (RandomBG)
		{
			GetComponent<Image>().sprite = BGImages[Random.Range(0, BGImages.Length)];
		}
	}
}
