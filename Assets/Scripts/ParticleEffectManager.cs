using UnityEngine;

public class ParticleEffectManager : MonoBehaviour
{
	private ParticleSystem[] allParticles;

	private void Start()
	{
		allParticles = base.gameObject.GetComponentsInChildren<ParticleSystem>();
	}

	private void Update()
	{
		for (int i = 0; i < allParticles.Length; i++)
		{
			if (allParticles[i].IsAlive())
			{
				return;
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
