using System;

namespace Game
{
    [Serializable]
    public class GameConfig
    {
        public JumpDashConfig jumpDash;
        public KnifeHitConfig knifeHit;
    }

    #region Jump Dash
    [Serializable]
    public class JumpDashConfig
    {
        public string name;
        public int minBlockLength;
        public int maxBlockLength;
        public float playerJumpDuration;
        public int freeContinueTotal;
        public JumpDashLevel[] levels;
    }

    [Serializable]
    public class JumpDashLevel
    {
        public int upperBound;
        public float blockSpeed;
        public float spawnIntervalInSeconds;
    }
    #endregion


    [Serializable]
    public class KnifeHitConfig
    {
        public string name;
        public int freeContinueTotal;
        public float knifeLaunchDuration;
        public KnifeHitLevel[] levels;
    }

    [Serializable]
    public class KnifeHitLevel
    {
        public int upperBound;
        public float[] targetSpeedRange;
        public float changeTargetSpeedEverySeconds;
        public int[] obstacle;
        public int[] bonus;
        public int knifeCount;
    }
}
