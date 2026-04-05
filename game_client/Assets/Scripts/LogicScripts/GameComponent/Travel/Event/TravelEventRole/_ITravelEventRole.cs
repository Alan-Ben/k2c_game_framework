
using UnityEngine;

namespace GOE
{
    public interface _ITravelEventRole
    {
        ETravelEventRoleType roleType { get; }
        
        long id { get; }
        
        string name { get; }
        
        _AShowCaseUnitInfoObj showCaseUnitInfo { get; }

        /// <summary>
        /// 显示形象时的位置偏移
        /// </summary>
        Vector3 showCaseOffset { get; }
        
        /// <summary>
        /// 半身像
        /// </summary>
        NPGTextureIndex midImage { get; }
    }
}