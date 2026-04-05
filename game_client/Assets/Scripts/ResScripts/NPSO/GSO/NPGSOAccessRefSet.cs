
using ALPackage;
using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 获取途径配置
    /// </summary>
    [Serializable]
    public class NPAccessRefObj : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;//唯一id
        public NPGTextureIndex tex_icon;//图标
        public string name;//名字
        public string desc;//描述
        public List<string> desc_args;//描述参数
        public _NPPlayerConditionSerializeInfo unlock_condition;//获取途径解锁条件
        public string lock_desc;//获取途径解锁描述
        public List<string> lock_desc_args;//获取途径解锁描述参数
        public _NPPlayerEffectSerializeInfo go_to;//跳转效果
        
    }

    public class NPGSOAccessRefSet : _TALSOBasicRefSet<NPAccessRefObj>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "access"; } }
    }
}