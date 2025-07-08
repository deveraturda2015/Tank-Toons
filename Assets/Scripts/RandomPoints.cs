using UnityEngine;

public class RandomPoints
{
	public static Vector3 GetRandomMovePoint()
	{
		return new Vector3(0f - GlobalCommons.Instance.DynamicHorizontalCameraBorder + GlobalCommons.Instance.gridSize + Random.Range(0f, GlobalCommons.Instance.DynamicHorizontalCameraBorder * 2f - 2f * GlobalCommons.Instance.gridSize), 0f - GlobalCommons.Instance.DynamicVerticalCameraBorder + GlobalCommons.Instance.gridSize + Random.Range(0f, GlobalCommons.Instance.DynamicVerticalCameraBorder * 2f - 2f * GlobalCommons.Instance.gridSize), 0f);
	}

	public static Vector3 GetRandomOffScreenPoint()
	{
		Vector3 result = new Vector3(0f - GlobalCommons.Instance.DynamicHorizontalScreenBorderPlusOneCell + Random.Range(0f, GlobalCommons.Instance.DynamicHorizontalScreenBorderPlusOneCell * 2f), 0f - GlobalCommons.Instance.DynamicVerticalScreenBorderDistancePlusOneCell + Random.Range(0f, GlobalCommons.Instance.DynamicVerticalScreenBorderDistancePlusOneCell * 2f), 0f);
		switch (Random.Range(1, 5))
		{
		case 1:
			result.x = 0f - GlobalCommons.Instance.DynamicHorizontalScreenBorderPlusOneCell;
			break;
		case 2:
			result.x = GlobalCommons.Instance.DynamicHorizontalScreenBorderPlusOneCell;
			break;
		case 3:
			result.y = 0f - GlobalCommons.Instance.DynamicVerticalScreenBorderDistancePlusOneCell;
			break;
		case 4:
			result.y = GlobalCommons.Instance.DynamicVerticalScreenBorderDistancePlusOneCell;
			break;
		}
		return result;
	}
}
