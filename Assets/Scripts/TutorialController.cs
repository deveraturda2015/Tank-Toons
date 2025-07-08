using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

internal class TutorialController
{
	private SpriteRenderer TutorialCrateSprite;

	private TextMeshPro TutorialCrateTextMesh;

	private SpriteRenderer TutorialBlocksSprite;

	private TextMeshPro TutorialBlocksTextMesh;

	private SpriteRenderer TutorialBarrelSprite;

	private TextMeshPro TutorialBarrelTextMesh;

	private SpriteRenderer TutorialSpawnerSprite;

	private TextMeshPro TutorialSpawnerTextMesh;

	private SpriteRenderer TutorialCoins;

	private TextMeshPro TutorialCoinsTextMesh;

	private SpriteRenderer TutorialBonuses;

	private TextMeshPro TutorialBonusesTextMesh1;

	private TextMeshPro TutorialBonusesTextMesh2;

	private TextMeshPro TutorialBonusesTextMesh3;

	private TextMeshPro TutorialBonusesTextMesh4;

	private TextMeshPro TutorialBonusesTextMesh5;

	private TextMeshPro TutorialBonusesTextMesh6;

	private float showHideTime = 0.33f;

	public bool ShootingEnabled;

	private int specialGatesCrashedCount;

	private bool shownDestroyBlocksTutorial;

	private bool hidDestroyBlocksTutorial;

	private GameObject tutorialCrate;

	private GameObject tutorialBarrel;

	private GameObject autoaimControllerGO;

	private bool shownBarrelsTutor;

	private bool spawnedEnemies;

	public TutorialController()
	{
		TutorialCrateSprite = GameObject.Find("TutorialCrateSprite").GetComponent<SpriteRenderer>();
		TutorialCrateSprite.SetAlpha(0f);
		TutorialCrateTextMesh = TutorialCrateSprite.transform.Find("TextMeshPro").GetComponent<TextMeshPro>();
		TutorialBarrelSprite = GameObject.Find("TutorialBarrelSprite").GetComponent<SpriteRenderer>();
		TutorialBarrelSprite.SetAlpha(0f);
		TutorialBarrelTextMesh = TutorialBarrelSprite.transform.Find("TextMeshPro").GetComponent<TextMeshPro>();
		TutorialBlocksSprite = GameObject.Find("TutorialBlocksSprite").GetComponent<SpriteRenderer>();
		TutorialBlocksSprite.SetAlpha(0f);
		TutorialBlocksTextMesh = TutorialBlocksSprite.transform.Find("TextMeshPro").GetComponent<TextMeshPro>();
		TutorialSpawnerSprite = GameObject.Find("TutorialSpawnerSprite").GetComponent<SpriteRenderer>();
		TutorialSpawnerSprite.SetAlpha(0f);
		TutorialSpawnerTextMesh = TutorialSpawnerSprite.transform.Find("TextMeshPro").GetComponent<TextMeshPro>();
		TutorialCoins = GameObject.Find("TutorialCoins").GetComponent<SpriteRenderer>();
		TutorialCoins.SetAlpha(0f);
		TutorialCoinsTextMesh = TutorialCoins.transform.Find("TextMeshPro").GetComponent<TextMeshPro>();
		TutorialBonuses = GameObject.Find("TutorialBonuses").GetComponent<SpriteRenderer>();
		TutorialBonuses.SetAlpha(0f);
		TutorialBonusesTextMesh1 = TutorialBonuses.transform.Find("TextMeshPro1").GetComponent<TextMeshPro>();
		TutorialBonusesTextMesh2 = TutorialBonuses.transform.Find("TextMeshPro2").GetComponent<TextMeshPro>();
		TutorialBonusesTextMesh3 = TutorialBonuses.transform.Find("TextMeshPro3").GetComponent<TextMeshPro>();
		TutorialBonusesTextMesh4 = TutorialBonuses.transform.Find("TextMeshPro4").GetComponent<TextMeshPro>();
		TutorialBonusesTextMesh5 = TutorialBonuses.transform.Find("TextMeshPro5").GetComponent<TextMeshPro>();
		TutorialBonusesTextMesh6 = TutorialBonuses.transform.Find("TextMeshPro6").GetComponent<TextMeshPro>();
		TutorialCrateSprite.enabled = false;
		TutorialBarrelSprite.enabled = false;
		TutorialBonuses.enabled = false;
		TutorialCoins.enabled = false;
		TutorialBlocksSprite.enabled = false;
		TutorialSpawnerSprite.enabled = false;
	}

	internal void SetAutoaimControllerGO(GameObject gameObject)
	{
		autoaimControllerGO = gameObject;
	}

	public void IncreaseSpecialGatesCrashed()
	{
		specialGatesCrashedCount++;
		switch (specialGatesCrashedCount)
		{
		case 3:
			HideDestroyBlocksTutorial();
			ShowCrateTutorialMessages();
			break;
		case 6:
			ShowBarrelTutorialMessage();
			break;
		case 9:
			ShowDestroySpawnerTutorial();
			break;
		}
	}

	private void SetTextMeshAlpha(TextMesh tm, float alpha)
	{
		Color color = tm.color;
		float r = color.r;
		Color color2 = tm.color;
		float g = color2.g;
		Color color3 = tm.color;
		tm.color = new Color(r, g, color3.b, alpha);
	}

	public void Update()
	{
		TextMeshPro tutorialCrateTextMesh = TutorialCrateTextMesh;
		Color color = TutorialCrateSprite.color;
		tutorialCrateTextMesh.alpha = color.a;
		TextMeshPro tutorialBarrelTextMesh = TutorialBarrelTextMesh;
		Color color2 = TutorialBarrelSprite.color;
		tutorialBarrelTextMesh.alpha = color2.a;
		TextMeshPro tutorialBlocksTextMesh = TutorialBlocksTextMesh;
		Color color3 = TutorialBlocksSprite.color;
		tutorialBlocksTextMesh.alpha = color3.a;
		TextMeshPro tutorialSpawnerTextMesh = TutorialSpawnerTextMesh;
		Color color4 = TutorialSpawnerSprite.color;
		tutorialSpawnerTextMesh.alpha = color4.a;
		TextMeshPro tutorialCoinsTextMesh = TutorialCoinsTextMesh;
		Color color5 = TutorialCoins.color;
		tutorialCoinsTextMesh.alpha = color5.a;
		TextMeshPro tutorialBonusesTextMesh = TutorialBonusesTextMesh1;
		Color color6 = TutorialBonuses.color;
		tutorialBonusesTextMesh.alpha = color6.a;
		TextMeshPro tutorialBonusesTextMesh2 = TutorialBonusesTextMesh2;
		Color color7 = TutorialBonuses.color;
		tutorialBonusesTextMesh2.alpha = color7.a;
		TextMeshPro tutorialBonusesTextMesh3 = TutorialBonusesTextMesh3;
		Color color8 = TutorialBonuses.color;
		tutorialBonusesTextMesh3.alpha = color8.a;
		TextMeshPro tutorialBonusesTextMesh4 = TutorialBonusesTextMesh4;
		Color color9 = TutorialBonuses.color;
		tutorialBonusesTextMesh4.alpha = color9.a;
		TextMeshPro tutorialBonusesTextMesh5 = TutorialBonusesTextMesh5;
		Color color10 = TutorialBonuses.color;
		tutorialBonusesTextMesh5.alpha = color10.a;
		TextMeshPro tutorialBonusesTextMesh6 = TutorialBonusesTextMesh6;
		Color color11 = TutorialBonuses.color;
		tutorialBonusesTextMesh6.alpha = color11.a;
		if (!spawnedEnemies)
		{
			spawnedEnemies = true;
			EnemyTankController component = Object.Instantiate(Prefabs.enemyTankPrefab, new Vector3(17f, 5f), Quaternion.identity).GetComponent<EnemyTankController>();
			component.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.machinegun, isBoss: false);
			component = Object.Instantiate(Prefabs.enemyTankPrefab, new Vector3(26f, 6f), Quaternion.identity).GetComponent<EnemyTankController>();
			component.InitializeEnemy(EnemyTankController.EnemyTypes.tank, WeaponTypes.machinegun, isBoss: false);
		}
		if ((bool)tutorialCrate)
		{
			Transform transform = TutorialCrateSprite.transform;
			Vector3 position = tutorialCrate.transform.position;
			float x = position.x + 0.65f;
			Vector3 position2 = tutorialCrate.transform.position;
			float y = position2.y + 1.8f;
			Vector3 position3 = TutorialCrateSprite.transform.position;
			transform.position = new Vector3(x, y, position3.z);
		}
		if ((bool)tutorialBarrel)
		{
			Transform transform2 = TutorialBarrelSprite.transform;
			Vector3 position4 = tutorialBarrel.transform.position;
			float x2 = position4.x - 0.75f;
			Vector3 position5 = tutorialBarrel.transform.position;
			float y2 = position5.y - 1.6f;
			Vector3 position6 = TutorialBarrelSprite.transform.position;
			transform2.position = new Vector3(x2, y2, position6.z);
		}
	}

	private void ShowCrateTutorialMessages()
	{
		TutorialCrateSprite.enabled = true;
		TutorialCrateSprite.SetAlpha(0f);
		SpriteRenderer tutorialCrateSprite = TutorialCrateSprite;
		Color color = TutorialCrateSprite.color;
		float r = color.r;
		Color color2 = TutorialCrateSprite.color;
		float g = color2.g;
		Color color3 = TutorialCrateSprite.color;
		tutorialCrateSprite.DOColor(new Color(r, g, color3.b, 1f), showHideTime);
		TutorialCoins.enabled = true;
		TutorialBonuses.enabled = true;
		TutorialBonuses.SetAlpha(0f);
		TutorialCoins.SetAlpha(0f);
		SpriteRenderer tutorialBonuses = TutorialBonuses;
		Color color4 = TutorialBonuses.color;
		float r2 = color4.r;
		Color color5 = TutorialBonuses.color;
		float g2 = color5.g;
		Color color6 = TutorialBonuses.color;
		tutorialBonuses.DOColor(new Color(r2, g2, color6.b, 1f), showHideTime);
		SpriteRenderer tutorialCoins = TutorialCoins;
		Color color7 = TutorialCoins.color;
		float r3 = color7.r;
		Color color8 = TutorialCoins.color;
		float g3 = color8.g;
		Color color9 = TutorialCoins.color;
		tutorialCoins.DOColor(new Color(r3, g3, color9.b, 1f), showHideTime);
		GameplayCommons.Instance.visibilityController.UncoverPortion(9.81f, 20.71f, 2.18f, 6.54f);
		DestructableObstacleController[] array = Object.FindObjectsOfType<DestructableObstacleController>();
		List<DestructableObstacleController> list = new List<DestructableObstacleController>();
		foreach (DestructableObstacleController destructableObstacleController in array)
		{
			if (destructableObstacleController.destructableObstacleType == DestructableObstacleTypes.bonusCrate)
			{
				list.Add(destructableObstacleController);
			}
		}
		GameObject gameObject = list[0].gameObject;
		for (int j = 1; j < list.Count; j++)
		{
			Vector3 position = list[j].transform.position;
			float x = position.x;
			Vector3 position2 = gameObject.transform.position;
			if (x < position2.x)
			{
				gameObject = list[j].gameObject;
			}
		}
		tutorialCrate = gameObject;
	}

	public void HideCrateTutorialMessages()
	{
		SpriteRenderer tutorialCrateSprite = TutorialCrateSprite;
		Color color = TutorialCrateSprite.color;
		float r = color.r;
		Color color2 = TutorialCrateSprite.color;
		float g = color2.g;
		Color color3 = TutorialCrateSprite.color;
		tutorialCrateSprite.DOColor(new Color(r, g, color3.b, 0f), showHideTime).SetDelay(0.25f);
	}

	private void ShowBarrelTutorialMessage()
	{
		shownBarrelsTutor = true;
		TutorialBarrelSprite.enabled = true;
		TutorialBarrelSprite.SetAlpha(0f);
		SpriteRenderer tutorialBarrelSprite = TutorialBarrelSprite;
		Color color = TutorialBarrelSprite.color;
		float r = color.r;
		Color color2 = TutorialBarrelSprite.color;
		float g = color2.g;
		Color color3 = TutorialBarrelSprite.color;
		tutorialBarrelSprite.DOColor(new Color(r, g, color3.b, 1f), showHideTime);
		GameplayCommons.Instance.visibilityController.UncoverPortion(21.8f, 30.52f, 0f, 8.72f);
		DestructableObstacleController[] array = Object.FindObjectsOfType<DestructableObstacleController>();
		List<DestructableObstacleController> list = new List<DestructableObstacleController>();
		foreach (DestructableObstacleController destructableObstacleController in array)
		{
			if (destructableObstacleController.destructableObstacleType == DestructableObstacleTypes.explosiveBarrel)
			{
				Vector3 position = destructableObstacleController.transform.position;
				if (position.y < 8f)
				{
					list.Add(destructableObstacleController);
				}
			}
		}
		GameObject gameObject = list[0].gameObject;
		for (int j = 1; j < list.Count; j++)
		{
			Vector3 position2 = list[j].transform.position;
			float x = position2.x;
			Vector3 position3 = gameObject.transform.position;
			if (x < position3.x)
			{
				gameObject = list[j].gameObject;
			}
		}
		tutorialBarrel = gameObject;
	}

	public void HideBarrelTutorialMessage()
	{
		SpriteRenderer tutorialBarrelSprite = TutorialBarrelSprite;
		Color color = TutorialBarrelSprite.color;
		float r = color.r;
		Color color2 = TutorialBarrelSprite.color;
		float g = color2.g;
		Color color3 = TutorialBarrelSprite.color;
		tutorialBarrelSprite.DOColor(new Color(r, g, color3.b, 0f), showHideTime);
	}

	public void ShowDestroyBlocksTutorial()
	{
		if (!shownDestroyBlocksTutorial && specialGatesCrashedCount <= 0)
		{
			shownDestroyBlocksTutorial = true;
			TutorialBlocksSprite.enabled = true;
			TutorialBlocksSprite.SetAlpha(0f);
			SpriteRenderer tutorialBlocksSprite = TutorialBlocksSprite;
			Color color = TutorialBlocksSprite.color;
			float r = color.r;
			Color color2 = TutorialBlocksSprite.color;
			float g = color2.g;
			Color color3 = TutorialBlocksSprite.color;
			tutorialBlocksSprite.DOColor(new Color(r, g, color3.b, 1f), showHideTime);
		}
	}

	private void HideDestroyBlocksTutorial()
	{
		if (!hidDestroyBlocksTutorial)
		{
			hidDestroyBlocksTutorial = true;
			TutorialBlocksSprite.DOKill();
			SpriteRenderer tutorialBlocksSprite = TutorialBlocksSprite;
			Color color = TutorialBlocksSprite.color;
			float r = color.r;
			Color color2 = TutorialBlocksSprite.color;
			float g = color2.g;
			Color color3 = TutorialBlocksSprite.color;
			tutorialBlocksSprite.DOColor(new Color(r, g, color3.b, 0f), showHideTime);
		}
	}

	private void ShowDestroySpawnerTutorial()
	{
		TutorialSpawnerSprite.enabled = true;
		TutorialSpawnerSprite.SetAlpha(0f);
		SpriteRenderer tutorialSpawnerSprite = TutorialSpawnerSprite;
		Color color = TutorialSpawnerSprite.color;
		float r = color.r;
		Color color2 = TutorialSpawnerSprite.color;
		float g = color2.g;
		Color color3 = TutorialSpawnerSprite.color;
		tutorialSpawnerSprite.DOColor(new Color(r, g, color3.b, 1f), showHideTime);
		autoaimControllerGO.SetActive(value: true);
		autoaimControllerGO.GetComponent<AutoaimTutorialController>().Initialize();
	}

	public void HideDestoySpawnerTutorial()
	{
		TutorialSpawnerSprite.DOKill();
		SpriteRenderer tutorialSpawnerSprite = TutorialSpawnerSprite;
		Color color = TutorialSpawnerSprite.color;
		float r = color.r;
		Color color2 = TutorialSpawnerSprite.color;
		float g = color2.g;
		Color color3 = TutorialSpawnerSprite.color;
		tutorialSpawnerSprite.DOColor(new Color(r, g, color3.b, 0f), showHideTime);
	}

	internal bool CheckDamageAllowed(DestructableObstacleTypes destructableObstacleType)
	{
		if (destructableObstacleType == DestructableObstacleTypes.specialWall && shownBarrelsTutor && GameplayCommons.Instance.levelStateController.currentGameStats.GameStatistics.GetStat(GameStatistics.Stat.BarrelsExploded) < 4)
		{
			return false;
		}
		return true;
	}
}
