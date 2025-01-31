using System;

namespace Game
{
    [Serializable]
    public class LevelDesignConfig
    {
        public float initialBlockSpeed;
        public float maxBlockSpeed;
        public int minBlockLength;
        public int maxBlockLength;
        public float spawnIntervalInSeconds;
        public int freeContinueTotal;
    }
}
