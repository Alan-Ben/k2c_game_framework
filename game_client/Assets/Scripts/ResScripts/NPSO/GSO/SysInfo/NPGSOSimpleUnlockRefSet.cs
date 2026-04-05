using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    /****************
     * 快捷的将条件和系统信息进行对应的处理，这里直接对等级和关卡进行映射，不做其他条件处理
     **/
    [System.Serializable]
    public class NPSimpleUnlockRef : _IALBasicRefObj
    {
        public long _refId { get { return id; } }

        public long id;                 //ID
        public _NPPlayerConditionSerializeInfo condition_info;        //条件集合
        public string unlock_tip;       //解锁描述
        public List<string> unlock_tip_args;        //解锁描述参数

#if NP_GAME
        /// <summary>
        /// 是否满足条件
        /// </summary>
        /// <param name="_varVariableInfo"></param>
        /// <returns></returns>
        public bool isConditionEnable(NPVarInfo _varVariableInfo)
        {
            if (NPPlayer.instance.playerInfo == null || condition_info == null)
                return false;
            else
                return condition_info.IsEnable(_varVariableInfo);
        }  
#endif
        
        /// <summary>
        /// 获取解锁提示文本
        /// </summary>
        /// <returns></returns>
        public string getUnlockTip()
        {
            if (unlock_tip_args == null || unlock_tip_args.Count == 0)
            {
                return TextTranslate.instance.getLanguage(unlock_tip);
            }
            else
            {
                List<string> paramList = new List<string>();
                foreach (string str in unlock_tip_args)
                {
                    if (str.Length >= 2 && str[0].Equals('#'))
                    {
                        paramList.Add(TextTranslate.instance.getLanguage(str));
                    }
                    else
                    {
                        paramList.Add(str);
                    }
                }

                return TextTranslate.instance.getLanguage(unlock_tip, paramList);
            }
        }
    }

    /**************
     * 物品表
     **/
    public class NPGSOSimpleUnlockRefSet : _TALSOBasicRefSet<NPSimpleUnlockRef>
    {
        /************
         * 资源加载路径
         **/
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "simple_unlock"; } }
    }
}

