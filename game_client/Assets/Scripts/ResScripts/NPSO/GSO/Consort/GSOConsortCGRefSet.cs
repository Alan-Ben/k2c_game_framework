using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public enum EConsortCGType
    {
        NONE,
        [InspectorName("邀约")]
        INVITE,
        [InspectorName("私房")]
        PRIVACY,
    }
    
    /// <summary>
    /// 妃子cg表
    /// </summary>
    [Serializable]
    public class ConsortCGRefObj : _IALBasicRefObj
    {
        public long _refId { get { return cg_id; } }

        public long cg_id;//cg_id
        public long consort_id;//妃子id
        public EConsortCGType cg_type;//cg类型
        public string name;//cg名称
        public NPGTextureIndex cg_icon;//cg图标
        public NPGGoIndex cg_go;//cg对应的go
        public long add;//加护点加成万分比
        public GVideoClipIndex cg_video_index;//cg对应的视频资源index
    }
    
    /// <summary>
    /// 妃子cg表
    /// </summary>
    public class GSOConsortCGRefSet : _TALSOBasicRefSet<ConsortCGRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/consort.unity3d"; } }
        public static string objName { get { return "consort_cg"; } }
    }
}