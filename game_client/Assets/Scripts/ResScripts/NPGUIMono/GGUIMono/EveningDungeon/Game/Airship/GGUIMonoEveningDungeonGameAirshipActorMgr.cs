using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 飞船形象加载路径配置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class EveningDungeonGameAirshipActorAssetPathConfig<T> where T : Enum
    {
        [ALHeader("状态类型")]
        public T type;
        
        [ALHeader("飞船Actor预制体GoIndex")]
        public NPGGoIndex goIndex;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class GGUIMonoEveningDungeonGameAirshipActorMgr<T>
        where T : Enum
    {
        [ALHeader("飞船Actor根节点")]
        public GameObject airshipActorRoot;
        
        public List<EveningDungeonGameAirshipActorAssetPathConfig<T>> airshipActorAssetPathConfigList;
        
        public EveningDungeonGameAirshipActorAssetPathConfig<T> getAirshipActorAssetPathConfig(T type)
        {
            if (airshipActorAssetPathConfigList == null || airshipActorAssetPathConfigList.Count <= 0)
            {
                return null;
            }

            return airshipActorAssetPathConfigList.Find((config) => { return config != null && config.type.Equals(type); });
        }
    }
}