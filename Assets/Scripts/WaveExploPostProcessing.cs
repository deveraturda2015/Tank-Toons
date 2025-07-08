using DG.Tweening;
using UnityEngine;

public class WaveExploPostProcessing : MonoBehaviour
{
	private static int effectsCount;

	public Material mat = Materials.WaveExploMaterial;

	private float effectTime = 0.6f;

	protected float _radius;

	protected float _amplitude;

	public float radius
	{
		get
		{
			return _radius;
		}
		set
		{
			_radius = value;
			mat.SetFloat("_Radius", _radius);
		}
	}

	public float amplitude
	{
		get
		{
			return _amplitude;
		}
		set
		{
			_amplitude = value;
			mat.SetFloat("_Amplitude", _amplitude);
		}
	}

	public static void ResetEffectsCount()
	{
		effectsCount = 0;
	}

	public void StartIt(Vector2 center)
	{
		mat.SetFloat("_CenterX", center.x / (float)Screen.width);
		mat.SetFloat("_CenterY", center.y / (float)Screen.height);
		mat.SetFloat("_Radius", 0.2f);
		mat.SetFloat("_Amplitude", 0.05f);
		mat.SetFloat("_Aspect", (float)Screen.width / (float)Screen.height);
		radius = 0f;
		amplitude = 0.05f;
		DOTween.To(delegate(float value)
		{
			radius = value;
		}, 0f, 0.15f, effectTime).OnCompleteWithCoroutine(HandleComplete);
		DOTween.To(delegate(float value)
		{
			amplitude = value;
		}, 0.05f, 0f, effectTime);
	}

	protected void HandleComplete()
	{
		effectsCount--;
		UnityEngine.Object.Destroy(this);
	}

	public static void ShowEffectAt(Vector2 center)
	{
		//if (effectsCount <= 0 && !EffectsSpawner.disableEffects && GlobalCommons.Instance.globalGameStats.FancyExplosionsEffectEnabled)
		//{
		//	WaveExploPostProcessing waveExploPostProcessing = Camera.main.gameObject.AddComponent<WaveExploPostProcessing>();
		//	waveExploPostProcessing.StartIt(center);
		//	effectsCount++;
		//}
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, mat);
	}
}
