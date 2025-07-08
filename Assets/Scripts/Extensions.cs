using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class Extensions
{
	public static void SetAlpha(this Image p_image, float p_transparency)
	{
		if (p_image != null)
		{
			Color color = p_image.color;
			color.a = p_transparency;
			p_image.color = color;
		}
	}

	public static void SetAlpha(this TextMeshProUGUI p_image, float p_transparency)
	{
		if (p_image != null)
		{
			Color color = p_image.color;
			color.a = p_transparency;
			p_image.color = color;
		}
	}

	public static void SetAlpha(this Text p_text, float p_transparency)
	{
		if (p_text != null)
		{
			Color color = p_text.color;
			color.a = p_transparency;
			p_text.color = color;
		}
	}

	public static void SetAlpha(this SpriteRenderer p_sr, float p_transparency)
	{
		if (p_sr != null)
		{
			Color color = p_sr.color;
			color.a = p_transparency;
			p_sr.color = color;
		}
	}

	public static Tweener OnCompleteWithCoroutine(this Tweener tweener, Action action)
	{
		return tweener.OnComplete(delegate
		{
			GlobalCommons.ProcessWithCoroutine(action);
		});
	}

	public static Tweener OnStartWithCoroutine(this Tweener tweener, Action action)
	{
		return tweener.OnStart(delegate
		{
			GlobalCommons.ProcessWithCoroutine(action);
		});
	}

	public static Sequence OnStartWithCoroutine(this Sequence sequence, Action action)
	{
		return sequence.OnStart(delegate
		{
			GlobalCommons.ProcessWithCoroutine(action);
		});
	}

	public static Sequence OnCompleteWithCoroutine(this Sequence sequence, Action action)
	{
		return sequence.OnComplete(delegate
		{
			GlobalCommons.ProcessWithCoroutine(action);
		});
	}

	public static void DelayedCallWithCoroutine(float delay, Action action)
	{
		DOVirtual.DelayedCall(delay, delegate
		{
			GlobalCommons.ProcessWithCoroutine(action);
		});
	}
}
