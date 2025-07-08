using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AdsToRevive : MonoBehaviour
{
    public static AdsToRevive instance { get; private set; }

    public GameObject ReviveUI;

    private bool alreadyRevived;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ReviveUI.SetActive(false);
    }

    public void OpenReviveUI()
    {
        if (!alreadyRevived)
        {
            GameplayCommons.Instance.TogglePause();

            ReviveUI.SetActive(true);
        }
        else
        {
            NoRevive();
        }
    }

    public void DoRevive()
    {
        AdmobManager.instance.ShowRewardedAd(() =>
        {
            GameplayCommons.Instance.TogglePause();

            ReviveUI.SetActive(false);
            alreadyRevived = true;

            GameplayCommons.Instance.playersTankController.GainFullHealth();
        });       
    }

    public void NoRevive()
    {
        ReviveUI.SetActive(false);

        GameplayCommons.Instance.playersTankController.PlayerDead = true;
        GameplayCommons.Instance.playersTankController.DestroyPlayerTank();
    }



}
