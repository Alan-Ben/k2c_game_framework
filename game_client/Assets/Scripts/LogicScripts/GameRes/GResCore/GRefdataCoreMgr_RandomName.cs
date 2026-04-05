using CommonEnum;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace GOE
{
    partial class GRefdataCoreMgr
    {
        /// <summary>
        /// 获取子嗣随机昵称
        /// </summary>
        /// <returns></returns>
        public string getChildRandomName(EChildSexType _type)
        {
            int refCount = childNameRefCore != null && childNameRefCore.refList != null ? childNameRefCore.refList.Count : 0;
            if (refCount > 0)
            {
                ChildNameRefObj nameRef = childNameRefCore.refList[Random.Range(0, childNameRefCore.refList.Count)];
                switch (_type)
                {
                    case EChildSexType.BOY:
                        {
                            return nameRef.boy_name;
                        }
                    default:
                        return nameRef.girl_name;
                }
            }
            else
            {
                Debug.LogError("找不到子嗣随机名字");
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取玩家默认随机昵称
        /// </summary>
        /// <returns></returns>
        public string getRandomInitName()
        {
            string randomName = string.Empty;
            switch (GameSetting.instance.getCurrentLanguage())
            {
                case ENPLanguage.ZH_CN:
                case ENPLanguage.ZH_TW:
                    randomName = _getRandomInitNameInZH_CN();
                    break;
                default:
                    randomName = _getRandomInitNameNormal();
                    break;
            }
            return randomName;
        }

        // /// <summary>
        // /// 俄语取名特殊处理
        // /// </summary>
        // /// <returns></returns>
        // private string _getRandomInitNameInRu()
        // {
        //     int refCount = playerNameRefCore != null && playerNameRefCore.refList != null ? playerNameRefCore.refList.Count : 0;
        //     if (refCount > 0)
        //     {
        //         string randomName = playerNameRefCore.refList[Random.Range(0, refCount)].suffix_name + "_" + Random.Range(100000, 1000000);
        //         return randomName;
        //     }
        //     else
        //     {
        //         Debug.LogError("找不到随机名字");
        //         return string.Empty;
        //     }
        // }

        /// <summary>
        /// 获取玩家默认随机昵称
        /// </summary>
        /// <returns></returns>
        private string _getRandomInitNameNormal()
        {
            int prefixNameCount = 0;
            int suffixNameCount = 0;

            int refCount = playerNameRefCore != null && playerNameRefCore.refList != null ? playerNameRefCore.refList.Count : 0;
            if (refCount > 0)
            {
                if (prefixNameCount <= 0 && suffixNameCount <= 0)
                {
                    var enusRefObjs = playerNameRefCore.refList;
                    prefixNameCount = 0;
                    suffixNameCount = 0;
                    for (int i = 0; i < enusRefObjs.Count; i++)
                    {
                        if (enusRefObjs[i] == null)
                            continue;
                        if (enusRefObjs[i].suffix_name != "")
                            suffixNameCount++;
                        if (enusRefObjs[i].prefix_name != "")
                            prefixNameCount++;
                    }
                }

                string suffixName = playerNameRefCore.refList[Random.Range(0, suffixNameCount)].suffix_name;
                string prefixName = playerNameRefCore.refList[Random.Range(0, prefixNameCount)].prefix_name;
                string randomName = suffixName;
                if (!string.IsNullOrEmpty(prefixName))
                    randomName = suffixName + '\u00A0' + prefixName;
                return randomName;
            }
            else
            {
                Debug.LogError("找不到随机名字");
                return string.Empty;
            }
        }
        
        /// <summary>
        /// 中文玩家默认随机昵称
        /// </summary>
        /// <returns></returns>
        private string _getRandomInitNameInZH_CN()
        {
            int prefixNameCount = 0;
            int suffixNameCount = 0;

            int refCount = playerNameRefCore != null && playerNameRefCore.refList != null ? playerNameRefCore.refList.Count : 0;
            if (refCount > 0)
            {
                if (prefixNameCount <= 0 && suffixNameCount <= 0)
                {
                    var enusRefObjs = playerNameRefCore.refList;
                    prefixNameCount = 0;
                    suffixNameCount = 0;
                    for (int i = 0; i < enusRefObjs.Count; i++)
                    {
                        if (enusRefObjs[i] == null)
                            continue;
                        if (enusRefObjs[i].suffix_name != "")
                            suffixNameCount++;
                        if (enusRefObjs[i].prefix_name != "")
                            prefixNameCount++;
                    }
                }
                string randomName = playerNameRefCore.refList[Random.Range(0, suffixNameCount)].suffix_name + playerNameRefCore.refList[Random.Range(0, prefixNameCount)].prefix_name;
                return randomName;
            }
            else
            {
                Debug.LogError("找不到随机名字");
                return string.Empty;
            }
        }
    }
}
