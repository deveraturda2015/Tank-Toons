using UnityEngine;

public class Materials
{
	internal static Material WallShardParticleMaterial;

	internal static Material HarderWallShardParticle;

	internal static Material SpecialWallShardParticle;

	internal static Material CrateChunkMaterial;

	internal static Material GroundMaterialSummer;

	internal static Material GroundMaterialDesert;

	internal static Material GroundMaterialMetal;

	internal static Material GroundMaterialWinter;

	internal static Material FlashWhiteMaterial;

	internal static Material WaveExploMaterial;

	public static void InitializeMaterials()
	{
		CrateChunkMaterial = (Resources.Load("Materials/CrateChunkMaterial") as Material);
		WallShardParticleMaterial = (Resources.Load("Materials/WallShardParticle") as Material);
		HarderWallShardParticle = (Resources.Load("Materials/HarderWallShardParticle") as Material);
		SpecialWallShardParticle = (Resources.Load("Materials/SpecialWallShardParticle") as Material);
		GroundMaterialSummer = (Resources.Load("Materials/GroundMaterialSummer") as Material);
		GroundMaterialDesert = (Resources.Load("Materials/GroundMaterialDesert") as Material);
		GroundMaterialMetal = (Resources.Load("Materials/GroundMaterialMetal") as Material);
		GroundMaterialWinter = (Resources.Load("Materials/GroundMaterialWinter") as Material);
		FlashWhiteMaterial = (Resources.Load("Materials/FlashWhiteMaterial") as Material);
		//WaveExploMaterial = new Material(Shader.Find("Custom/WaveExplo"));
	}
}
