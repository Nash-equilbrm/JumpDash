using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class GameData
    {
        public BestScore bestScore;
        public float volumnFx;
        public float volumnMusic;
    }

    [Serializable]
    public class BestScore
    {
        public float jumpDash;
        public float knifeHit;
    }
}
