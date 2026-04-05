using System.Collections.Generic;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        [NotNull] private readonly Dictionary<long, List<MuseumItemLevelRefObj>> _m_museumItemLevelRefDict = new Dictionary<long, List<MuseumItemLevelRefObj>>();
        [NotNull] private readonly Dictionary<long, List<MuseumItemUpgradeCostRefObj>> _m_museumItemUpgradeCostRefDict = new Dictionary<long, List<MuseumItemUpgradeCostRefObj>>();


        private void _initMuseum()
        {
            museumItemRefCore.dealAllRef(_itemRef =>
            {
                if (_itemRef == null)
                    return;

                _itemRef.quality_ref = getQuality(ENPQualityClass.MUSEUM_ITEM, _itemRef.quality);
                _itemRef.quality_ext_ref = qualityExtRefCore.getRef((long)_itemRef.quality);
            });
            
            foreach (MuseumItemLevelRefObj refObj in museumItemLevelRefCore.refList)
            {
                if (refObj == null)
                    continue;

                List<MuseumItemLevelRefObj> levelList = _m_museumItemLevelRefDict.getValueDefinitely(refObj.museum_id);
                levelList.Add(refObj);
            }
            foreach (List<MuseumItemLevelRefObj> levelList in _m_museumItemLevelRefDict.Values)
            {
                levelList.Sort((_a, _b) => _a.level.CompareTo(_b.level));
            }
            
            foreach (MuseumItemUpgradeCostRefObj refObj in museumItemUpgradeCostRefCore.refList)
            {
                if (refObj == null)
                    continue;

                List<MuseumItemUpgradeCostRefObj> costList = _m_museumItemUpgradeCostRefDict.getValueDefinitely(refObj.group_id);
                costList.Add(refObj);
            }
            foreach (List<MuseumItemUpgradeCostRefObj> costList in _m_museumItemUpgradeCostRefDict.Values)
            {
                costList.Sort((_a, _b) => _a.level.CompareTo(_b.level));
            }
        }


        public MuseumItemLevelRefObj getMuseumItemLevelRefObj(long _museumId, int _level)
        {
            List<MuseumItemLevelRefObj> levelList = _m_museumItemLevelRefDict.GetValueOrDefault(_museumId);
            return levelList.SafeGet(_level - 1);
        }
        public MuseumItemUpgradeCostRefObj getMuseumItemUpgradeCostRefObj(long _groupId, int _level)
        {
            List<MuseumItemUpgradeCostRefObj> costList = _m_museumItemUpgradeCostRefDict.GetValueOrDefault(_groupId);
            return costList.SafeGet(_level - 1);
        }
    }
}