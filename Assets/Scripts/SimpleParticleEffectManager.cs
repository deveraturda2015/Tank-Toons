using UnityEngine;

public class SimpleParticleEffectManager : MonoBehaviour
{
	private ParticleSystem ps;

	private void Start()
	{
		ps = GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		if (!ps.IsAlive())
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
