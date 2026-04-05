using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;


namespace GOE
{
    [Serializable]
    public partial class NPCurrencyResObj : _IALBasicRefObj
    {
        public long _refId
        {
            get
            {
                return (long)type;
            }
        }
        public CommonEnum.ECurrency type;//资源类型
        public int multiple;//倍数，用来做精度，消灭小数
    }

    public class NPSOCurrencyResRefSet : _TALSOBasicRefSet<NPCurrencyResObj>
    {

        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "currency"; } }
    }
}

