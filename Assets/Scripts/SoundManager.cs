using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public enum MusicType
	{
		GameplayMusic,
		MenusMusic,
		JingleMusic
	}

	public static SoundManager instance;

	private AudioSource musicAudioSource;

	private AudioSource tankEngineAudioSource;

	private AudioSource rainAudioSource;

	private AudioSource laserStartAudioSource;

	private AudioSource interfaceSoundsAudioSource;

	private AudioSource[] gameplayAudioSources;

	private int currentGamempayAudiosourceIndex;

	private int gamempayAudiosources = 20;

	internal DamageTypes[] ignoreHitSoundWeapons = new DamageTypes[2]
	{
		DamageTypes.laserDamage,
		DamageTypes.shockDamage
	};

    public static bool musicMuted;

    public static bool soundMuted;

	private float lastTimePlayedEnemyHitSound;

	private float lastTimePlayedSpawnerHitSound;

	private float lastTimePlayedPlayerHitSound;

	private float lastTimePlayedCrateHitSound;

	private float lastTimePlayedBarrelHitSound;

	private float lastTimePlayedObstacleHitSound;

	private float lastTimePlayedCoinCountSound;

	private float lastTimePlayedPrizeDropSound;

	private float lastTimePlayedTileSetIn;

	private float hitSoundTimeout = 0.05f;

	private float coinCountSoundTimeout = 0.075f;

	private float prizeDropSoundTimeout = 0.1f;

	private float tileSetInSoundTimeout = 0.1f;

	private int menusMusicIndex;

	private int ingameMusicIndex;

	private int tankMovingFrames;

	private float hitSoundsVolume = 0.9f;

	private bool RestoreMusicFlag;

	private MusicType LastPlayedMusicType = MusicType.MenusMusic;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			Initialize();
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.DestroyImmediate(base.gameObject);
		}
	}

	internal void StopAllAudioSources()
	{
		UnityEngine.Object.FindObjectsOfType<AudioSource>().ToList().ForEach(delegate(AudioSource itm)
		{
			itm.Stop();
		});
	}

	public void RestoreMusic()
	{
		RestoreMusicFlag = true;
	}

	private void Initialize()
	{
		interfaceSoundsAudioSource = base.gameObject.AddComponent<AudioSource>();
		interfaceSoundsAudioSource.spatialBlend = 0f;
		musicAudioSource = base.gameObject.AddComponent<AudioSource>();
		musicAudioSource.spatialBlend = 0f;
		musicAudioSource.loop = true;
		gameplayAudioSources = new AudioSource[gamempayAudiosources];
		for (int i = 0; i < gamempayAudiosources; i++)
		{
			AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
			audioSource.spatialBlend = 0f;
			gameplayAudioSources[i] = audioSource;
		}
	}

	public void ToggleSoundMute()
	{
		soundMuted = !soundMuted;
	}

	public void ToggleMusicMute(MusicType musicType)
	{
		musicMuted = !musicMuted;
		ToggleMusic(musicType);
	}

	public void StopMusic()
	{
		musicAudioSource.Stop();
	}

	public void FadeOutMusic()
	{
		if (!musicMuted && 0 == 0)
		{
			musicAudioSource.DOKill();
			musicAudioSource.DOFade(0f, 0.2f).OnCompleteWithCoroutine(FinalizeMusicFadeout);
		}
	}

	private void FinalizeMusicFadeout()
	{
	}

	public void ToggleMusic(MusicType musicType, bool levelResolutionIsCompleted = false, bool doFadeIn = true)
	{
		if (musicMuted || false || Application.isEditor)
		{
			if (musicAudioSource.isPlaying)
			{
				musicAudioSource.Stop();
			}
			return;
		}
		LastPlayedMusicType = musicType;
		if (musicAudioSource.isPlaying)
		{
			switch (musicType)
			{
			case MusicType.GameplayMusic:
				if (Array.IndexOf(Sounds.musicGameplay, musicAudioSource.clip) != -1)
				{
					if (musicAudioSource.volume == 0f)
					{
						musicAudioSource.Play();
						musicAudioSource.DOFade(1f, 0.2f);
					}
					return;
				}
				break;
			case MusicType.MenusMusic:
				if (Array.IndexOf(Sounds.musicMenus, musicAudioSource.clip) != -1)
				{
					if (musicAudioSource.volume == 0f)
					{
						musicAudioSource.Play();
						musicAudioSource.DOFade(1f, 0.2f);
					}
					return;
				}
				break;
			default:
				throw new Exception("cannot check music playing for " + musicType);
			case MusicType.JingleMusic:
				break;
			}
		}
		AudioClip audioClip;
		switch (musicType)
		{
		case MusicType.GameplayMusic:
			audioClip = Sounds.musicGameplay[ingameMusicIndex];
			ingameMusicIndex++;
			if (ingameMusicIndex == Sounds.musicGameplay.Length)
			{
				ingameMusicIndex = 0;
			}
			musicAudioSource.loop = true;
			break;
		case MusicType.MenusMusic:
			audioClip = Sounds.musicMenus[menusMusicIndex];
			menusMusicIndex++;
			if (menusMusicIndex == Sounds.musicMenus.Length)
			{
				menusMusicIndex = 0;
			}
			musicAudioSource.loop = true;
			break;
		case MusicType.JingleMusic:
			audioClip = ((!levelResolutionIsCompleted) ? Sounds.musicJingles[1] : Sounds.musicJingles[0]);
			musicAudioSource.loop = false;
			break;
		default:
			throw new Exception("cannot find music for " + musicType);
		}
		if (!musicAudioSource.isPlaying || !(musicAudioSource.clip == audioClip))
		{
			musicAudioSource.clip = audioClip;
			if (doFadeIn)
			{
				musicAudioSource.DOKill();
				musicAudioSource.volume = 0f;
				musicAudioSource.DOFade(1f, 0.2f);
			}
			else
			{
				musicAudioSource.DOKill();
				musicAudioSource.volume = 1f;
			}
			musicAudioSource.Play();
		}
	}

	public void UdateRainSound()
	{
		if (rainAudioSource == null)
		{
			rainAudioSource = base.gameObject.AddComponent<AudioSource>();
			rainAudioSource.spatialBlend = 0f;
			rainAudioSource.volume = 0.5f;
			rainAudioSource.loop = true;
			rainAudioSource.clip = Sounds.rainLoop;
		}
		if (soundMuted || GameplayCommons.Instance.GamePaused || GameplayCommons.Instance.levelStateController.GameplayStopped || Time.timeScale < 0.01f)
		{
			if (rainAudioSource.isPlaying)
			{
				rainAudioSource.Stop();
			}
		}
		else if (!rainAudioSource.isPlaying)
		{
			rainAudioSource.Play();
		}
	}

	public void Update()
	{
		if (RestoreMusicFlag)
		{
			RestoreMusicFlag = false;
			ToggleMusic(LastPlayedMusicType);
		}
	}

	public void UpdateEngineSound(bool tankMoving)
	{
		if (!GlobalCommons.Instance.globalGameStats.EngineSoundEnabled)
		{
			if (tankEngineAudioSource != null)
			{
				tankEngineAudioSource.Stop();
				tankEngineAudioSource = null;
			}
			return;
		}
		if (tankEngineAudioSource == null)
		{
			tankEngineAudioSource = base.gameObject.AddComponent<AudioSource>();
			tankEngineAudioSource.spatialBlend = 0f;
			tankEngineAudioSource.volume = 0.2f;
			tankEngineAudioSource.loop = true;
		}
		if (soundMuted || GameplayCommons.Instance.GamePaused || GameplayCommons.Instance.levelStateController.GameplayStopped || Time.timeScale < 0.01f || GameplayCommons.Instance.playersTankController.PlayerDead)
		{
			if (tankEngineAudioSource.isPlaying)
			{
				tankEngineAudioSource.Stop();
			}
			return;
		}
		if (tankMoving)
		{
			tankMovingFrames++;
		}
		else
		{
			tankMovingFrames = 0;
		}
		if (tankMovingFrames > 2)
		{
			if (tankEngineAudioSource.clip != Sounds.engineRevving)
			{
				tankEngineAudioSource.clip = Sounds.engineRevving;
				tankEngineAudioSource.Play();
			}
			else if (!tankEngineAudioSource.isPlaying)
			{
				tankEngineAudioSource.Play();
			}
		}
		else if (tankEngineAudioSource.clip != Sounds.engineIdleLoop)
		{
			tankEngineAudioSource.clip = Sounds.engineIdleLoop;
			tankEngineAudioSource.Play();
		}
		else if (!tankEngineAudioSource.isPlaying)
		{
			tankEngineAudioSource.Play();
		}
	}

	public void PlayMachinegunShotSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.machinegunShot, 1f, 0.3f);
		}
	}

	public void PlayFightVoiceSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.fightVoice);
		}
	}

	public void PlayBonusWearoffSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.bonusWearoff);
		}
	}

	public void PlayRicochetShotSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.ricochetShot);
		}
	}

	public void PlayRicochetBounceSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.ricochetBounce);
		}
	}

	public void PlayBombBeepSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.bombBeep, 0.4f);
		}
	}

	public void PlayNotAvailableSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.notAvailable);
		}
	}

	public void PlayPrizeBreakSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.prizeBreak);
		}
	}

	public void PlayEnemyEmojiSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.enemyEmojiSound, 1f, 0.1f);
		}
	}

	public void PlayCantPlaceMineSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.cantPlaceMine);
		}
	}

	public void PlayShooshOutSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.swooshOut);
		}
	}

	public void PlaySwooshInSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.swooshIn);
		}
	}

	public void PlayMinigunShotSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.minigunShot, 0.75f, 0.3f);
		}
	}

	public void PlayTileSetInSound()
	{
		if (!soundMuted && !(Time.fixedTime - lastTimePlayedTileSetIn < tileSetInSoundTimeout))
		{
			lastTimePlayedTileSetIn = Time.fixedTime;
			playGameplaySound(Sounds.tileSetIn, 0.75f, 0.05f);
		}
	}

	public void PlayNewAchievementSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.newAchievement);
		}
	}

	public void PlayPrizePunchSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.prizePunch, 1f, 0.2f);
		}
	}

	public void PlayThunderSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.thunder, 1f, 0.2f);
		}
	}

	public void PlayFireworkSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.fireworkSound, 1f, 0.2f);
		}
	}

	public void PlayPrizeDropSound()
	{
		if (!soundMuted && !(Time.fixedTime - lastTimePlayedPrizeDropSound < prizeDropSoundTimeout))
		{
			lastTimePlayedPrizeDropSound = Time.fixedTime;
			playGameplaySound(Sounds.prizeDrop, 1f, 0.1f);
		}
	}

	public void PlayExplosionSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.explosions[UnityEngine.Random.Range(0, Sounds.explosions.Length)]);
		}
	}

	public void PlaySleeveFallSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.sleeves[UnityEngine.Random.Range(0, Sounds.sleeves.Length)], 0.28f, 0.1f, -0.2f);
		}
	}

	public void PlaySpawnerExplosionSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.spawnerExplosion, 1.2f);
		}
	}

	public void PlayPrizeBoxPopSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.prizeBoxPop, 0.8f, 0.1f);
		}
	}

	public void PlayRewadWinSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.rewadWinSound);
		}
	}

	public void PlayEnemySpawnSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.enemySpawn);
		}
	}

	public void PlaySparkSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.sparkSounds[UnityEngine.Random.Range(0, Sounds.sparkSounds.Length)]);
		}
	}

	public void PlayAmmoPickupSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.ammoPickup, 0.7f);
		}
	}

	public void PlayBonusPickupSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.bonusPickup);
		}
	}

	public void PlayRadarBonusSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.radarBonusPickup);
		}
	}

	public void PlayBrickSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.brickSound);
		}
	}

	public void PlayCannonShotSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.cannonFire, 0.7f);
		}
	}

	public void PlayShotgunShotSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.shotgunFire, 0.6f, 0.1f);
		}
	}

	public void PlayHomingMissileShotSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.homingMissileLaunch, 0.8f);
		}
	}

	public void PlayBulletHitObstacleSound()
	{
		if (!soundMuted && !(Time.fixedTime - lastTimePlayedObstacleHitSound < hitSoundTimeout))
		{
			lastTimePlayedObstacleHitSound = Time.fixedTime;
			playGameplaySound(Sounds.bulletHitObstacle, 0.8f, 0.3f);
		}
	}

	public void PlayRicochetSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.ricochetSounds[UnityEngine.Random.Range(0, Sounds.ricochetSounds.Length)], UnityEngine.Random.Range(0.17f, 0.3f), 0.1f);
		}
	}

	public void PlayRailgunShotSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.railgunShotSnd);
		}
	}

	public void PlayMinePlaceSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.minePlaceSnd);
		}
	}

	public void PlayBarrelHitSound(DamageTypes damageType)
	{
		if (!soundMuted && !(Time.fixedTime - lastTimePlayedBarrelHitSound < hitSoundTimeout))
		{
			lastTimePlayedBarrelHitSound = Time.fixedTime;
			if (Array.IndexOf(ignoreHitSoundWeapons, damageType) == -1)
			{
				playGameplaySound(Sounds.barrelHit, 1f, 0.3f);
			}
		}
	}

	public void PlayCoinPickupSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.coinPickup, 1f, 0.2f);
		}
	}

	public void PlayCoinCountSound()
	{
		if (!soundMuted && !(Time.fixedTime - lastTimePlayedCoinCountSound < coinCountSoundTimeout))
		{
			lastTimePlayedCoinCountSound = Time.fixedTime;
			playGameplaySound(Sounds.coinSounds[UnityEngine.Random.Range(0, Sounds.coinSounds.Length)]);
		}
	}

	public void PlayCrateCrashSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.crateCrash);
		}
	}

	public void PlayCrateHitSound(DamageTypes damageType)
	{
		if (!soundMuted && !(Time.fixedTime - lastTimePlayedCrateHitSound < hitSoundTimeout))
		{
			lastTimePlayedCrateHitSound = Time.fixedTime;
			if (Array.IndexOf(ignoreHitSoundWeapons, damageType) == -1)
			{
				playGameplaySound(Sounds.crateHit, 1f, 0.3f);
			}
		}
	}

	public void PlayCrateFreezeSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.freezing);
		}
	}

	public void PlayHealthPickupSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.healthPickup);
		}
	}

	public void PlayLaserStartSound()
	{
		if (!soundMuted)
		{
			if (laserStartAudioSource == null)
			{
				laserStartAudioSource = base.gameObject.AddComponent<AudioSource>();
				laserStartAudioSource.spatialBlend = 0f;
				laserStartAudioSource.volume = 0.7f;
				laserStartAudioSource.loop = false;
				laserStartAudioSource.clip = Sounds.laserStart;
			}
			if (!laserStartAudioSource.isPlaying)
			{
				laserStartAudioSource.Play();
			}
		}
	}

	public void PlayUnfreezeSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.unfreeze);
		}
	}

	public void PlayWeaponSwitchSound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.weaponSwitchSnd);
		}
	}

	public void PlayEnemyHitSound(DamageTypes damageType)
	{
		if (!soundMuted && !(Time.fixedTime - lastTimePlayedEnemyHitSound < hitSoundTimeout))
		{
			lastTimePlayedEnemyHitSound = Time.fixedTime;
			if (Array.IndexOf(ignoreHitSoundWeapons, damageType) == -1)
			{
				playGameplaySound(GetRandomAudioclipFromArray(Sounds.enemyHitSounds), hitSoundsVolume, 0.3f);
			}
		}
	}

	public void PlayButtonClickSound()
	{
		if (!soundMuted)
		{
			interfaceSoundsAudioSource.clip = Sounds.buttonClick;
			interfaceSoundsAudioSource.Play();
		}
	}

	public void PlayLevelSelectSound()
	{
		if (!soundMuted)
		{
			interfaceSoundsAudioSource.clip = Sounds.levelSelect;
			interfaceSoundsAudioSource.Play();
		}
	}

	public void PlayShopBuySound()
	{
		if (!soundMuted)
		{
			playGameplaySound(Sounds.shopBuy, 0.75f);
		}
	}

	private AudioClip GetRandomAudioclipFromArray(AudioClip[] audioClips)
	{
		return audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
	}

	private void playGameplaySound(AudioClip ac, float volume = 1f, float pitchRandomizationFactor = 0f, float pitchShift = 0f)
	{
		currentGamempayAudiosourceIndex++;
		if (currentGamempayAudiosourceIndex == gameplayAudioSources.Length)
		{
			currentGamempayAudiosourceIndex = 0;
		}
		gameplayAudioSources[currentGamempayAudiosourceIndex].clip = ac;
		gameplayAudioSources[currentGamempayAudiosourceIndex].volume = volume;
		if (pitchRandomizationFactor != 0f)
		{
			gameplayAudioSources[currentGamempayAudiosourceIndex].pitch = UnityEngine.Random.Range(1f - pitchRandomizationFactor, 1f + pitchRandomizationFactor) + pitchShift;
		}
		else
		{
			gameplayAudioSources[currentGamempayAudiosourceIndex].pitch = 1f;
		}
		gameplayAudioSources[currentGamempayAudiosourceIndex].Play();
	}
}
