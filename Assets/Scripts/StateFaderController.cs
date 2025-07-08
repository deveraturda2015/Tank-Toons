using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StateFaderController
{
	private enum State
	{
		Idle,
		FadingIn,
		FadingOut
	}

	private State CurrentState;

	private Image FadeImage;

	private Canvas CurrentCanvas;

	internal const float FADE_DURATION = 0.17f;

	private bool IsFading;

	private SceneTransfer CurrentSceneTransfer;

	private List<SceneTransfer> ScenesToTransferTo = new List<SceneTransfer>();

	internal static List<string> ScenesIgnoringFadeout = new List<string>
	{
		"LoadingScene",
		"PlayAdScene",
		"PlayRewardedAdScene"
	};

	internal static List<string> ScenesIgnoringFadein = new List<string>
	{
		"Gameplay",
		"PlayAdScene",
		"PlayRewardedAdScene"
	};

	internal bool IsCurrentlyFading => IsFading;

	public StateFaderController()
	{
		CurrentCanvas = Object.FindObjectOfType<Canvas>();
		SceneManager.activeSceneChanged += OnActiveSceneChanged;
	}

	internal Canvas GetCurrentCanvas()
	{
		return CurrentCanvas;
	}

	internal void Update()
	{
		if (!IsFading && CurrentState == State.Idle && ScenesToTransferTo.Count > 0)
		{
			CurrentSceneTransfer = ScenesToTransferTo[0];
			ScenesToTransferTo.RemoveAt(0);
			FadeOut();
		}
	}

	private void FadeOut()
	{
		IsFading = true;
		CurrentState = State.FadingOut;
		if (CurrentCanvas == null)
		{
			CurrentCanvas = Object.FindObjectOfType<Canvas>();
		}
		string CurrentlyActiveSceneName = SceneManager.GetActiveScene().name;
		if (CurrentCanvas != null && !ScenesIgnoringFadeout.Exists((string itm) => string.Equals(itm, CurrentlyActiveSceneName)))
		{
			FadeImage = Object.Instantiate(Prefabs.FadeImage).GetComponent<Image>();
			FadeImage.transform.SetParent(CurrentCanvas.transform, worldPositionStays: false);
			FadeImage.transform.SetAsLastSibling();
			FadeImage.SetAlpha(0f);
			FadeImage.DOFade(1f, (!CurrentSceneTransfer.Immediate) ? 0.17f : 0.1f).OnComplete(ProcessFadeOutComplete);
		}
		else
		{
			ProcessFadeOutComplete();
		}
	}

	private void ProcessFadeOutComplete()
	{
		SceneManager.LoadScene(CurrentSceneTransfer.SceneName);
	}

	private void OnActiveSceneChanged(Scene arg0, Scene arg1)
	{
		CurrentState = State.FadingIn;
		CurrentCanvas = Object.FindObjectOfType<Canvas>();
		string CurrentlyActiveSceneName = SceneManager.GetActiveScene().name;
		if (CurrentCanvas != null && !ScenesIgnoringFadein.Exists((string itm) => string.Equals(itm, CurrentlyActiveSceneName)))
		{
			FadeImage = Object.Instantiate(Prefabs.FadeImage).GetComponent<Image>();
			FadeImage.transform.SetParent(CurrentCanvas.transform, worldPositionStays: false);
			FadeImage.transform.SetAsLastSibling();
			FadeImage.SetAlpha(1f);
			FadeImage.DOFade(0f, (CurrentSceneTransfer == null || !CurrentSceneTransfer.Immediate) ? 0.17f : 0.1f).OnComplete(ProcessFadeInComplete);
		}
		else
		{
			ProcessFadeInComplete();
		}
		ConfirmationWindow.Active = false;
		SimpleUIMessageController.Active = false;
	}

	private void ProcessFadeInComplete()
	{
		if (FadeImage != null && FadeImage.gameObject != null)
		{
			UnityEngine.Object.Destroy(FadeImage.gameObject);
		}
		CurrentState = State.Idle;
		IsFading = false;
	}

	internal void ChangeSceneTo(string name, bool immediate = false)
	{
		ScenesToTransferTo.Add(new SceneTransfer
		{
			Immediate = immediate,
			SceneName = name
		});
	}
}
