using UnityEngine;

public class Sounds
{
	internal static AudioClip machinegunShot;

	internal static AudioClip minigunShot;

	internal static AudioClip ammoPickup;

	internal static AudioClip barrelHit;

	internal static AudioClip buttonClick;

	internal static AudioClip coinPickup;

	internal static AudioClip crateCrash;

	internal static AudioClip crateHit;

	internal static AudioClip freezing;

	internal static AudioClip healthPickup;

	internal static AudioClip laserStart;

	internal static AudioClip levelSelect;

	internal static AudioClip shopBuy;

	internal static AudioClip unfreeze;

	internal static AudioClip weaponSwitchSnd;

	internal static AudioClip minePlaceSnd;

	internal static AudioClip railgunShotSnd;

	internal static AudioClip brickSound;

	internal static AudioClip bulletHitObstacle;

	internal static AudioClip cannonFire;

	internal static AudioClip shotgunFire;

	internal static AudioClip enemySpawn;

	internal static AudioClip fightVoice;

	internal static AudioClip bonusWearoff;

	internal static AudioClip cantPlaceMine;

	internal static AudioClip newAchievement;

	internal static AudioClip prizePunch;

	internal static AudioClip thunder;

	internal static AudioClip prizeDrop;

	internal static AudioClip fireworkSound;

	internal static AudioClip bombBeep;

	internal static AudioClip notAvailable;

	internal static AudioClip enemyEmojiSound;

	internal static AudioClip radarBonusPickup;

	internal static AudioClip bonusPickup;

	internal static AudioClip swooshIn;

	internal static AudioClip swooshOut;

	internal static AudioClip engineIdleLoop;

	internal static AudioClip engineRevving;

	internal static AudioClip rainLoop;

	internal static AudioClip prizeBreak;

	internal static AudioClip tileSetIn;

	internal static AudioClip homingMissileLaunch;

	internal static AudioClip spawnerExplosion;

	internal static AudioClip prizeBoxPop;

	internal static AudioClip rewadWinSound;

	internal static AudioClip ricochetBounce;

	internal static AudioClip ricochetShot;

	internal static AudioClip[] enemyHitSounds;

	internal static AudioClip[] spawnerHitSounds;

	internal static AudioClip[] coinSounds;

	internal static AudioClip[] sparkSounds;

	internal static AudioClip[] ricochetSounds;

	public static AudioClip[] explosions;

	internal static AudioClip[] sleeves;

	internal static AudioClip[] musicMenus;

	internal static AudioClip[] musicGameplay;

	internal static AudioClip[] musicJingles;

	public static void InitializeSounds()
	{
		newAchievement = (Resources.Load("Sounds/newAchievement") as AudioClip);
		machinegunShot = (Resources.Load("Sounds/machinegunShot") as AudioClip);
		minigunShot = (Resources.Load("Sounds/minigunShot") as AudioClip);
		ammoPickup = (Resources.Load("Sounds/ammoPickup") as AudioClip);
		barrelHit = (Resources.Load("Sounds/barrelHit") as AudioClip);
		buttonClick = (Resources.Load("Sounds/buttonClick") as AudioClip);
		coinPickup = (Resources.Load("Sounds/coinPickup") as AudioClip);
		crateCrash = (Resources.Load("Sounds/crateCrash") as AudioClip);
		crateHit = (Resources.Load("Sounds/crateHit") as AudioClip);
		freezing = (Resources.Load("Sounds/freezing") as AudioClip);
		healthPickup = (Resources.Load("Sounds/healthPickup") as AudioClip);
		laserStart = (Resources.Load("Sounds/laserStart") as AudioClip);
		levelSelect = (Resources.Load("Sounds/levelSelect") as AudioClip);
		shopBuy = (Resources.Load("Sounds/shopBuy") as AudioClip);
		unfreeze = (Resources.Load("Sounds/unfreeze") as AudioClip);
		weaponSwitchSnd = (Resources.Load("Sounds/weaponSwitchSnd") as AudioClip);
		minePlaceSnd = (Resources.Load("Sounds/minePlaceSnd") as AudioClip);
		railgunShotSnd = (Resources.Load("Sounds/railgunShotSnd") as AudioClip);
		brickSound = (Resources.Load("Sounds/brickSound") as AudioClip);
		bulletHitObstacle = (Resources.Load("Sounds/bulletHitObstacle") as AudioClip);
		cannonFire = (Resources.Load("Sounds/cannonFire") as AudioClip);
		homingMissileLaunch = (Resources.Load("Sounds/homingMissileLaunch") as AudioClip);
		shotgunFire = (Resources.Load("Sounds/shotgunFire") as AudioClip);
		enemySpawn = (Resources.Load("Sounds/enemySpawn") as AudioClip);
		fightVoice = (Resources.Load("Sounds/fightVoice") as AudioClip);
		bonusWearoff = (Resources.Load("Sounds/bonusWearoff") as AudioClip);
		cantPlaceMine = (Resources.Load("Sounds/cantPlaceMine") as AudioClip);
		prizePunch = (Resources.Load("Sounds/prizePunch") as AudioClip);
		thunder = (Resources.Load("Sounds/thunder") as AudioClip);
		prizeDrop = (Resources.Load("Sounds/prizeDrop") as AudioClip);
		fireworkSound = (Resources.Load("Sounds/fireworkSound") as AudioClip);
		bombBeep = (Resources.Load("Sounds/bombBeep") as AudioClip);
		notAvailable = (Resources.Load("Sounds/notAvailable") as AudioClip);
		enemyEmojiSound = (Resources.Load("Sounds/enemyEmojiSound") as AudioClip);
		radarBonusPickup = (Resources.Load("Sounds/radarBonusPickup") as AudioClip);
		bonusPickup = (Resources.Load("Sounds/bonusPickup") as AudioClip);
		swooshIn = (Resources.Load("Sounds/swooshIn") as AudioClip);
		swooshOut = (Resources.Load("Sounds/swooshOut") as AudioClip);
		engineIdleLoop = (Resources.Load("Sounds/engineIdleLoop") as AudioClip);
		engineRevving = (Resources.Load("Sounds/engineRevving") as AudioClip);
		rainLoop = (Resources.Load("Sounds/rainLoop") as AudioClip);
		prizeBreak = (Resources.Load("Sounds/prizeBreak") as AudioClip);
		tileSetIn = (Resources.Load("Sounds/tileSetIn") as AudioClip);
		spawnerExplosion = (Resources.Load("Sounds/spawnerExplosion") as AudioClip);
		prizeBoxPop = (Resources.Load("Sounds/prizeBoxPop") as AudioClip);
		rewadWinSound = (Resources.Load("Sounds/rewadWinSound") as AudioClip);
		ricochetBounce = (Resources.Load("Sounds/ricochetBounce") as AudioClip);
		ricochetShot = (Resources.Load("Sounds/ricochetShot") as AudioClip);
		enemyHitSounds = new AudioClip[3]
		{
			Resources.Load("Sounds/enemyHit1") as AudioClip,
			Resources.Load("Sounds/enemyHit2") as AudioClip,
			Resources.Load("Sounds/enemyHit3") as AudioClip
		};
		explosions = new AudioClip[4]
		{
			Resources.Load("Sounds/explosion1") as AudioClip,
			Resources.Load("Sounds/explosion2") as AudioClip,
			Resources.Load("Sounds/explosion3") as AudioClip,
			Resources.Load("Sounds/explosion4") as AudioClip
		};
		sleeves = new AudioClip[3]
		{
			Resources.Load("Sounds/MachinegunSleeve1") as AudioClip,
			Resources.Load("Sounds/MachinegunSleeve2") as AudioClip,
			Resources.Load("Sounds/MachinegunSleeve3") as AudioClip
		};
		spawnerHitSounds = new AudioClip[3]
		{
			Resources.Load("Sounds/spawnerHit1") as AudioClip,
			Resources.Load("Sounds/spawnerHit2") as AudioClip,
			Resources.Load("Sounds/spawnerHit3") as AudioClip
		};
		ricochetSounds = new AudioClip[3]
		{
			Resources.Load("Sounds/ricochet1") as AudioClip,
			Resources.Load("Sounds/ricochet2") as AudioClip,
			Resources.Load("Sounds/ricochet3") as AudioClip
		};
		musicMenus = new AudioClip[2]
		{
			Resources.Load("Music/menus/menus_catface") as AudioClip,
			Resources.Load("Music/menus/menus_swingbitbrawl") as AudioClip
		};
		musicGameplay = new AudioClip[3]
		{
			Resources.Load("Music/ingame/ingame_surface") as AudioClip,
			Resources.Load("Music/ingame/ingame_backbonebreaks") as AudioClip,
			Resources.Load("Music/ingame/ingame_everybodybounce") as AudioClip
		};
		musicJingles = new AudioClip[2]
		{
			Resources.Load("Music/jingles/victory") as AudioClip,
			Resources.Load("Music/jingles/loss") as AudioClip
		};
		coinSounds = new AudioClip[9]
		{
			Resources.Load("Sounds/coin1") as AudioClip,
			Resources.Load("Sounds/coin2") as AudioClip,
			Resources.Load("Sounds/coin3") as AudioClip,
			Resources.Load("Sounds/coin4") as AudioClip,
			Resources.Load("Sounds/coin5") as AudioClip,
			Resources.Load("Sounds/coin6") as AudioClip,
			Resources.Load("Sounds/coin7") as AudioClip,
			Resources.Load("Sounds/coin8") as AudioClip,
			Resources.Load("Sounds/coin9") as AudioClip
		};
		sparkSounds = new AudioClip[3]
		{
			Resources.Load("Sounds/spark1") as AudioClip,
			Resources.Load("Sounds/spark2") as AudioClip,
			Resources.Load("Sounds/spark3") as AudioClip
		};
	}
}
