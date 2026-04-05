using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        [NotNull] private readonly Dictionary<long, List<InnStationLevelRefObj>> _m_innStationLevelRefDict = new Dictionary<long, List<InnStationLevelRefObj>>();
        [NotNull] private readonly Dictionary<long, List<InnDishLevelRefObj>> _m_innDishLevelRefDict = new Dictionary<long, List<InnDishLevelRefObj>>();


        private void _initInn()
        {
            innLevelRefCore.refList.Sort((_a, _b) => _a.level.CompareTo(_b.level));
            innMedalLevelRefCore.refList.Sort((_a, _b) => _a.level.CompareTo(_b.level));
            innDishRefCore.dealAllRef(_dishRef =>
            {
                if (_dishRef == null)
                    return;

                foreach (long stationId in _dishRef.need_station_id_list)
                {
                    InnStationRefObj stationRef = innStationRefCore.getRef(stationId);
                    if (stationRef == null)
                        continue;
                    
                    stationRef.dish_id_list ??= new List<long>();
                    stationRef.dish_id_list.Add(_dishRef.id);
                }
            });
            
            foreach (InnStationLevelRefObj refObj in innStationLevelRefCore.refList)
            {
                if (refObj == null)
                    continue;
                
                List<InnStationLevelRefObj> levelList = _m_innStationLevelRefDict.getValueDefinitely(refObj.station_id);
                levelList.Add(refObj);
            }
            foreach (List<InnStationLevelRefObj> levelList in _m_innStationLevelRefDict.Values)
            {
                levelList.Sort((_a, _b) => _a.level.CompareTo(_b.level));
            }
            
            foreach (InnDishLevelRefObj refObj in innDishLevelRefCore.refList)
            {
                if (refObj == null)
                    continue;
                
                List<InnDishLevelRefObj> levelList = _m_innDishLevelRefDict.getValueDefinitely(refObj.group_id);
                levelList.Add(refObj);
            }
            foreach (List<InnDishLevelRefObj> levelList in _m_innDishLevelRefDict.Values)
            {
                levelList.Sort((_a, _b) => _a.level.CompareTo(_b.level));
            }
        }
        
        
        public InnLevelRefObj getInnLevelRef(int _level)
        {
            if (innLevelRefCore.refList.Count <= 0)
                return null;
            
            int firstLevel = (int) innLevelRefCore.refList[0].level;
            return innLevelRefCore.refList.SafeGet(_level - firstLevel);
        }
        public InnMedalLevelRefObj getInnMedalLevelRef(int _level)
        {
            if (innMedalLevelRefCore.refList.Count <= 0)
                return null;
            
            int firstLevel = (int) innMedalLevelRefCore.refList[0].level;
            return innMedalLevelRefCore.refList.SafeGet(_level - firstLevel);
        }
        public InnStationLevelRefObj getInnStationLevelRef(long _stationId, int _level)
        {
            List<InnStationLevelRefObj> levelList = _m_innStationLevelRefDict.GetValueOrDefault(_stationId);
            if (levelList.Count <= 0)
                return null;
            
            int firstLevel = levelList[0].level;
            return levelList.SafeGet(_level - firstLevel);
        }
        public InnDishLevelRefObj getInnDishLevelRef(long _groupId, int _level)
        {
            List<InnDishLevelRefObj> levelList = _m_innDishLevelRefDict.GetValueOrDefault(_groupId);
            if (levelList.Count <= 0)
                return null;
            
            int firstLevel = levelList[0].level;
            return levelList.SafeGet(_level - firstLevel);
        }
    }
}