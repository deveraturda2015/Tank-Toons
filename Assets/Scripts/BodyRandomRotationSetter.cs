using UnityEngine;

public class BodyRandomRotationSetter
{
	public static void RandomlyRotate(GameObject go)
	{
		Vector3 eulerAngles = go.transform.eulerAngles;
		eulerAngles.z = Random.Range(0f, 360f);
		go.transform.eulerAngles = eulerAngles;
	}
}
