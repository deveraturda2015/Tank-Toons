using UnityEngine;

public class SimpleTapAwayNotification : MonoBehaviour
{
	private void Start()
	{
		Object.FindObjectOfType<Canvas>().transform.Find("CustomLevelButton").transform.SetAsLastSibling();
	}

	private void Update()
	{
	}

	private void Remove()
	{
	}
}
