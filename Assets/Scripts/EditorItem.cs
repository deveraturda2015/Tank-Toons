using System;
using UnityEngine;
using UnityEngine.UI;

public class EditorItem : MonoBehaviour
{
	private char itemChar = 'f';

	public Sprite[] EditorSpawnerSprites;

	public Sprite[] EditorTurretSprites;

	public Sprite[] EditorBossSprites;

	public Sprite[] EditorEnemySprites;

	public Sprite EditorPlayerSpawnSprite;

	public Sprite EditorEmptySprite;

	public Sprite WallSprite;

	public Sprite HarderWallSprite;

	public Sprite IndestructibleWallSprite;

	public Sprite IndestructibleWallCrackedSprite;

	public Sprite SpecialWallSprite;

	public Sprite BonusCrateSprite;

	public Sprite ExplosiveBarrelSprite;

	public Sprite CornerBottomLeftSprite;

	public Sprite CornerBottomRightSprite;

	public Sprite CornerTopRightSprite;

	public Sprite CornerTopLeftSprite;

	public Sprite IndestructibleCornerBottomLeftSprite;

	public Sprite IndestructibleCornerBottomRightSprite;

	public Sprite IndestructibleCornerTopRightSprite;

	public Sprite IndestructibleCornerTopLeftSprite;

	public Sprite GrassSprite;

	public RectTransform rectTransform;

	private Image imageComponent;

	public Sprite Bush;

	public Sprite LockedItemSprite;

	public Sprite UnderConstructionItemSprite;

	private Color normalColor = new Color(1f, 1f, 1f);

	internal Vector2 AnchoredPosition;

	private IntVector2 gridPosition = new IntVector2(0, 0);

	internal bool Visible
	{
		get
		{
			return base.gameObject.activeInHierarchy;
		}
		set
		{
			base.gameObject.SetActive(value);
		}
	}

	public char ItemChar => itemChar;

	internal void SetPosition(Vector2 setCoord)
	{
		rectTransform.position = setCoord;
		AnchoredPosition = rectTransform.anchoredPosition;
	}

	public void SetGridPosition(int posX, int posY)
	{
		gridPosition = new IntVector2(posX, posY);
	}

	internal void InitializeItem()
	{
		rectTransform = GetComponent<RectTransform>();
		imageComponent = GetComponent<Image>();
	}

	public void UpdateItem(char newChar)
	{
		itemChar = newChar;
		Sprite itemSpriteFromChar = GetItemSpriteFromChar(newChar);
		if (imageComponent.sprite != itemSpriteFromChar)
		{
			imageComponent.sprite = itemSpriteFromChar;
		}
		if (itemChar == '0')
		{
			System.Random random = new System.Random(Mathf.CeilToInt(Mathf.Abs(gridPosition.x * -12 + gridPosition.y * 17)));
			float num = (float)random.Next(800000, 900000) / 1000000f;
			imageComponent.color = new Color(num, num, num);
		}
		else if (itemChar == '0' || itemChar == '6' || itemChar == '7')
		{
			System.Random random2 = new System.Random(Mathf.CeilToInt(Mathf.Abs(gridPosition.x * -12 + gridPosition.y * 17)));
			float num2 = (float)random2.Next(850000, 950000) / 1000000f;
			imageComponent.color = new Color(num2, num2, num2);
		}
		else
		{
			imageComponent.color = normalColor;
		}
	}

	public Sprite GetItemSpriteFromChar(char itemChar)
	{
		Sprite sprite = null;
		if (EditorItemSelectionMenu.LockedItems.Contains(itemChar.ToString()))
		{
			return UnderConstructionItemSprite;
		}
		switch (itemChar)
		{
		case 'I':
			return Bush;
		case '5':
			return EditorSpawnerSprites[0];
		case 'h':
			return EditorSpawnerSprites[1];
		case 'i':
			return EditorSpawnerSprites[2];
		case 'j':
			return EditorSpawnerSprites[3];
		case 'k':
			return EditorSpawnerSprites[4];
		case 'l':
			return EditorSpawnerSprites[5];
		case 'm':
			return EditorSpawnerSprites[6];
		case 'u':
			return EditorTurretSprites[0];
		case 'v':
			return EditorTurretSprites[1];
		case 'w':
			return EditorTurretSprites[2];
		case 'x':
			return EditorTurretSprites[3];
		case 'y':
			return EditorTurretSprites[4];
		case 'z':
			return EditorTurretSprites[5];
		case 'A':
			return EditorTurretSprites[6];
		case 'n':
			return EditorBossSprites[0];
		case 'o':
			return EditorBossSprites[1];
		case 'p':
			return EditorBossSprites[2];
		case 'q':
			return EditorBossSprites[3];
		case 'r':
			return EditorBossSprites[4];
		case 's':
			return EditorBossSprites[5];
		case 't':
			return EditorBossSprites[6];
		case '1':
			return WallSprite;
		case '8':
			return HarderWallSprite;
		case '9':
			return IndestructibleWallSprite;
		case 'g':
			return IndestructibleWallCrackedSprite;
		case 'e':
			return SpecialWallSprite;
		case '0':
			return GrassSprite;
		case '7':
			return BonusCrateSprite;
		case '6':
			return ExplosiveBarrelSprite;
		case '4':
			return EditorPlayerSpawnSprite;
		case 'd':
			return CornerBottomLeftSprite;
		case 'c':
			return CornerBottomRightSprite;
		case 'b':
			return CornerTopLeftSprite;
		case 'a':
			return CornerTopRightSprite;
		case 'f':
			return EditorEmptySprite;
		case 'B':
			return IndestructibleCornerTopRightSprite;
		case 'C':
			return IndestructibleCornerTopLeftSprite;
		case 'D':
			return IndestructibleCornerBottomRightSprite;
		case 'E':
			return IndestructibleCornerBottomLeftSprite;
		case 'O':
			return EditorSpawnerSprites[7];
		case 'P':
			return EditorSpawnerSprites[8];
		case 'Q':
			return EditorSpawnerSprites[9];
		case 'R':
			return EditorTurretSprites[7];
		case 'S':
			return EditorTurretSprites[8];
		case 'T':
			return EditorTurretSprites[9];
		case 'U':
			return EditorBossSprites[7];
		case 'V':
			return EditorBossSprites[8];
		case 'W':
			return EditorBossSprites[9];
		case 'X':
			return EditorEnemySprites[0];
		case 'Y':
			return EditorEnemySprites[1];
		case 'Z':
			return EditorEnemySprites[2];
		case '!':
			return EditorEnemySprites[3];
		case '*':
			return EditorEnemySprites[4];
		case '#':
			return EditorEnemySprites[5];
		case '$':
			return EditorEnemySprites[6];
		case '%':
			return EditorEnemySprites[7];
		case '^':
			return EditorEnemySprites[8];
		case '&':
			return EditorEnemySprites[9];
		case '(':
			return EditorEnemySprites[10];
		default:
			throw new Exception("cannot find sprite for level item char: " + itemChar.ToString());
		}
	}
}
