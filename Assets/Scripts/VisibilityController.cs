using System.Collections.Generic;
using UnityEngine;

public class VisibilityController : MonoBehaviour
{
	private List<VisibilityCover> visibilityCovers;

	private bool coversInitialized;

	private float playerSightDistance = 7f;

	private float playerSightDistanceSquared;

	private float rocketSightDistance = 3f;

	private float rocketSightDistanceSquared;

	private float gridCellCompensation;

	private int coverCheckIndex;

	private int coverCheckIndexMax = 3;

	private bool firstUpdateComplete;

	private int initialCoversCount;

	private bool proceedWithSmoothRevealing;

	public float UncoverPercentage
	{
		get
		{
			if (visibilityCovers == null)
			{
				return 0f;
			}
			return (float)visibilityCovers.Count / (float)initialCoversCount;
		}
	}

	private void Start()
	{
		playerSightDistanceSquared = playerSightDistance * playerSightDistance;
		rocketSightDistanceSquared = rocketSightDistance * rocketSightDistance;
		gridCellCompensation = Mathf.Sqrt(GlobalCommons.Instance.gridSize * GlobalCommons.Instance.gridSize + GlobalCommons.Instance.gridSize * GlobalCommons.Instance.gridSize) - GlobalCommons.Instance.gridSize / 10f;
	}

	private void Update()
	{
		if (!coversInitialized || visibilityCovers.Count == 0 || (Time.timeSinceLevelLoad <= TileFlyIn.AllTilesSetTime && firstUpdateComplete))
		{
			return;
		}
		if (!proceedWithSmoothRevealing)
		{
			bool isEditor = Application.isEditor;
			if (0 == 0)
			{
				goto IL_00ac;
			}
		}
		for (int i = 0; i < 3; i++)
		{
			if (visibilityCovers.Count == 0)
			{
				break;
			}
			int index = UnityEngine.Random.Range(0, visibilityCovers.Count - 1);
			if (visibilityCovers[index].HideAndRemoveCover(immediate: false, ignoreSpecial: true))
			{
				visibilityCovers.RemoveAt(index);
			}
		}
		goto IL_00ac;
		IL_00ac:
		Vector3 position;
		float num;
		if (GameplayCommons.Instance.weaponsController.ActiveGuidedRocket == null)
		{
			position = GameplayCommons.Instance.playersTankController.TankBase.transform.position;
			num = playerSightDistanceSquared;
		}
		else
		{
			position = GameplayCommons.Instance.weaponsController.ActiveGuidedRocket.transform.position;
			num = rocketSightDistanceSquared;
		}
		for (int num2 = visibilityCovers.Count - 1; num2 > -1; num2--)
		{
			int num3 = num2 % coverCheckIndexMax;
			if (num3 == coverCheckIndex || !firstUpdateComplete)
			{
				VisibilityCover visibilityCover = visibilityCovers[num2];
				Vector2 direction = visibilityCover.CachedPosition - position;
				if (!(direction.sqrMagnitude > num))
				{
					float magnitude = direction.magnitude;
					RaycastHit2D[] array = Physics2D.RaycastAll(position, direction, magnitude, LayerMasks.allObstacleTypesLayerMask);
					bool flag = false;
					for (int j = 0; j < array.Length; j++)
					{
						RaycastHit2D raycastHit2D = array[j];
						if (raycastHit2D.distance < magnitude - gridCellCompensation)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						visibilityCovers.RemoveAt(num2);
						visibilityCover.GetComponent<VisibilityCover>().HideAndRemoveCover();
					}
				}
			}
		}
		if (!firstUpdateComplete)
		{
			firstUpdateComplete = true;
		}
		coverCheckIndex++;
		if (coverCheckIndex == coverCheckIndexMax)
		{
			coverCheckIndex = 0;
		}
	}

	public GameObject FindCoverAtPoint(Vector3 searchPoint)
	{
		GameObject result = null;
		float num = GlobalCommons.Instance.gridSize / 10f;
		for (int i = 0; i < visibilityCovers.Count; i++)
		{
			VisibilityCover visibilityCover = visibilityCovers[i];
			Vector3 position = visibilityCover.transform.position;
			if (Mathf.Abs(position.x - searchPoint.x) < num)
			{
				Vector3 position2 = visibilityCover.transform.position;
				if (Mathf.Abs(position2.y - searchPoint.y) < num)
				{
					result = visibilityCover.gameObject;
					break;
				}
			}
		}
		return result;
	}

	public bool CheckCoverNearPoint(Vector3 searchPoint)
	{
		float num = GlobalCommons.Instance.gridSize / 2f;
		for (int i = 0; i < visibilityCovers.Count; i++)
		{
			VisibilityCover visibilityCover = visibilityCovers[i];
			if (Mathf.Abs(visibilityCover.CachedPosition.x - searchPoint.x) < num && Mathf.Abs(visibilityCover.CachedPosition.y - searchPoint.y) < num)
			{
				return true;
			}
		}
		return false;
	}

	public void Initialize(List<Vector3> coversCoords, List<Vector3> forceRectVisibilityCovers)
	{
		visibilityCovers = new List<VisibilityCover>();
		for (int i = 0; i < coversCoords.Count; i++)
		{
			Vector3 position = coversCoords[i];
			visibilityCovers.Add(UnityEngine.Object.Instantiate(Prefabs.visibilityCoverPrefab, position, Quaternion.identity).GetComponent<VisibilityCover>());
		}
		for (int j = 0; j < forceRectVisibilityCovers.Count; j++)
		{
			Vector3 position2 = forceRectVisibilityCovers[j];
			VisibilityCover component = UnityEngine.Object.Instantiate(Prefabs.visibilityCoverPrefab, position2, Quaternion.identity).GetComponent<VisibilityCover>();
			component.InitSpecialCover();
			visibilityCovers.Add(component);
		}
		for (int k = 0; k < visibilityCovers.Count; k++)
		{
			VisibilityCover visibilityCover = visibilityCovers[k];
			visibilityCover.InitNeighbors(visibilityCovers);
		}
		coversInitialized = true;
		initialCoversCount = visibilityCovers.Count;
	}

	public void RevealAllImmediate()
	{
		for (int num = visibilityCovers.Count - 1; num > -1; num--)
		{
			visibilityCovers[num].HideAndRemoveCover(immediate: true, ignoreSpecial: true);
			visibilityCovers.RemoveAt(num);
		}
	}

	public void SlowlyRevealAll()
	{
		proceedWithSmoothRevealing = true;
	}

	public void ReinitVisibilityCoversGraphics()
	{
		foreach (VisibilityCover visibilityCover in visibilityCovers)
		{
			visibilityCover.ReinitGraphics();
		}
	}

	public void UncoverPortion(float xmin, float xmax, float ymin, float ymax)
	{
		for (int num = visibilityCovers.Count - 1; num > -1; num--)
		{
			VisibilityCover visibilityCover = visibilityCovers[num];
			Vector3 position = visibilityCover.transform.position;
			if (position.x >= xmin)
			{
				Vector3 position2 = visibilityCover.transform.position;
				if (position2.x <= xmax)
				{
					Vector3 position3 = visibilityCover.transform.position;
					if (position3.y >= ymin)
					{
						Vector3 position4 = visibilityCover.transform.position;
						if (position4.y <= ymax)
						{
							visibilityCover.HideAndRemoveCover();
							visibilityCovers.RemoveAt(num);
						}
					}
				}
			}
		}
	}
}
