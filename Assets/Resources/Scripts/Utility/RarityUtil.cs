public static class RarityUtil
{
	public enum Rarity
	{
		COMMON,
		UNCOMMON,
		MAGIC,
		EPIC,
		LEGENDARY,
		ANCIENT
	}

	public static float[] GetRarityColor(Rarity rarity)
    {
        float[] color = new float[3];

        switch(rarity)
        {
            case RarityUtil.Rarity.COMMON: // WHITE
                color[0] = 1f;
                color[1] = 1f;
                color[2] = 1f;
                break;
            case RarityUtil.Rarity.UNCOMMON: // GREEN
                color[0] = 0f;
                color[1] = 1f;
                color[2] = 0f;
                break;
            case RarityUtil.Rarity.MAGIC: // BLUE
                color[0] = 0f;
                color[1] = 0f;
                color[2] = 1f;
                break;
            case RarityUtil.Rarity.EPIC: // PURP
                color[0] = .6f;
                color[1] = 0f;
                color[2] = 1f;
                break;
            case RarityUtil.Rarity.LEGENDARY: // ORANGE
                color[0] = 1f;
                color[1] = .5f;
                color[2] = 0f;
                break;
            case RarityUtil.Rarity.ANCIENT: // RED
                color[0] = 1f;
                color[1] = 0f;
                color[2] = 0f;
                break;
        }

        return color;
    }

    public static float GetRaritySizeScalar(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.COMMON:
                return 1f;
            case Rarity.UNCOMMON:
                return 1.1f;
            case Rarity.MAGIC:
                return 1.2f;
            case Rarity.EPIC:
                return 1.3f;
            case Rarity.LEGENDARY:
                return 1.4f;
            case Rarity.ANCIENT:
                return 1.5f;
            default:
                return 1f;
        }
    }

    
}
