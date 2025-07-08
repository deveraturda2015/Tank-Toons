using System.Collections.Generic;
using UnityEngine;

public class VisibilityCover : MonoBehaviour
{
	private SpriteRenderer sr;

	private bool proceedWithDisappear;

	private float alpha = 1f;

	private List<VisibilityCover> crossNeighbors;

	public Sprite SmoothEdgedSprite;

	private bool smoothEdged;

	private bool isSpecialCover;

	public Vector3 CachedPosition;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		CachedPosition = base.transform.position;
	}

	private void Update()
	{
		if (proceedWithDisappear)
		{
			alpha -= 4f * Time.deltaTime;
			if (alpha < 0f)
			{
				alpha = 0f;
			}
			SpriteRenderer spriteRenderer = sr;
			Color color = sr.color;
			float r = color.r;
			Color color2 = sr.color;
			float g = color2.g;
			Color color3 = sr.color;
			spriteRenderer.color = new Color(r, g, color3.b, alpha);
			if (alpha == 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	private void SwitchSpriteToSmoothEdged()
	{
		if (sr == null)
		{
			sr = GetComponent<SpriteRenderer>();
		}
		if (!isSpecialCover)
		{
			sr.sprite = SmoothEdgedSprite;
		}
		smoothEdged = true;
	}

	public void ReinitGraphics()
	{
		if (isSpecialCover)
		{
			sr.sprite = SmoothEdgedSprite;
		}
	}

	public bool HideAndRemoveCover(bool immediate = false, bool ignoreSpecial = false)
	{
		if (ignoreSpecial && isSpecialCover)
		{
			return false;
		}
		if (proceedWithDisappear)
		{
			return false;
		}
		if (crossNeighbors != null)
		{
			for (int i = 0; i < crossNeighbors.Count; i++)
			{
				crossNeighbors[i].NotifyOfNeighborDisappearance(this);
			}
		}
		Vector3 position = base.transform.position;
		float x = position.x / GlobalCommons.Instance.gridSize;
		float num = GameplayCommons.Instance.currentLevel.Count - 1;
		Vector3 position2 = base.transform.position;
		Vector2 vector = new Vector2(x, Mathf.RoundToInt(num - position2.y / GlobalCommons.Instance.gridSize));
		GameplayCommons.Instance.tileMap.AddMeshFragment(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
		if (immediate)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return true;
		}
		proceedWithDisappear = true;
		return true;
	}

	public void NotifyOfNeighborDisappearance(VisibilityCover neighborCover)
	{
		crossNeighbors.Remove(neighborCover);
		if (!smoothEdged)
		{
			SwitchSpriteToSmoothEdged();
		}
	}

	public void InitNeighbors(List<VisibilityCover> allCovers)
	{
		crossNeighbors = new List<VisibilityCover>();
		for (int i = 0; i < allCovers.Count; i++)
		{
			VisibilityCover visibilityCover = allCovers[i];
			if (!(visibilityCover == this))
			{
				Vector3 position = base.transform.position;
				float x = position.x;
				Vector3 position2 = visibilityCover.transform.position;
				float num = Mathf.Abs(x - position2.x);
				Vector3 position3 = base.transform.position;
				float y = position3.y;
				Vector3 position4 = visibilityCover.transform.position;
				float num2 = Mathf.Abs(y - position4.y);
				if (num <= GlobalCommons.Instance.gridSize * 1.1f && num2 <= GlobalCommons.Instance.gridSize * 1.1f && (num == 0f || num2 == 0f))
				{
					crossNeighbors.Add(visibilityCover);
				}
			}
		}
		if (crossNeighbors.Count < 4)
		{
			SwitchSpriteToSmoothEdged();
		}
	}

	internal void InitSpecialCover()
	{
		isSpecialCover = true;
	}
}
