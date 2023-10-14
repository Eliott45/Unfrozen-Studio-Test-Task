using System;
using System.Collections.Generic;

namespace Missions.Data
{
    [Serializable]
    public struct MissionReward : ICloneable
    {
        public List<string> UnlockedHeroes;
        public int HeroScore;
        public List<MissionHeroRewardScore> OtherHeroScore;
        
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}