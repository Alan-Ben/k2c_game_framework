using System.Collections.Generic;
using NPEnum;

namespace GOE
{
    public partial class GRefdataCoreMgr
    {
        public NPQualityRefObj getQuality(ENPQualityClass _qualityClass, EQuality _quality)
        {
            if (null == qualityRefCore.refList || _quality == EQuality.NONE)
                return null;
            foreach (NPQualityRefObj item in qualityRefCore.refList)
            {
                if (item.quality_class == _qualityClass && item.quality == _quality)
                {
                    return item;
                }
            }
#if UNITY_EDITOR
            UnityEngine.Debug.LogError("Error! 取不到品质数据 , EQualityClass:  " + _qualityClass + "\t\tEQualityType:  " + _quality);
#endif
            return null;
        }
        
        public NPQualityRefObj getQuality(ENPItemType _itemType, EQuality _quality)
        {
            if (null == qualityRefCore.refList || _quality == EQuality.NONE)
                return null;
            foreach (NPQualityRefObj item in qualityRefCore.refList)
            {
                if (item.quality_class == _itemType.toQualityClass() && item.quality == _quality)
                {
                    return item;
                }
            }
#if UNITY_EDITOR
            UnityEngine.Debug.LogError("Error! 取不到品质数据 , EQualityClass:  " + _itemType.toQualityClass() + "\t\tEQualityType:  " + _quality);
#endif
            return null;
        }

        public List<NPQualityRefObj> getQualityListByClass(ENPQualityClass _qualityClass)
        {
            List<NPQualityRefObj> result = new List<NPQualityRefObj>();
            foreach (NPQualityRefObj npQualityRefObj in qualityRefCore.refList)
            {
                if(null == npQualityRefObj)
                    continue;

                if (npQualityRefObj.quality_class == _qualityClass)
                {
                    result.Add(npQualityRefObj);
                }
            }

            return result;
        }
    }
}