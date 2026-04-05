using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// SDK->Engine   sys-mjAdfrom: 梦加数据中心 -- 广告渠道标识信息
    /// </summary>
    public class MJSDK_Basic_2Engine_sys_mjAdfrom : MJSDK_2Engine_Base
    {
        //一级渠道.  固定格式：ios/aos
        public string adfrom;
        //二级渠道.   固定格式：adfrom_bundleId（ios_com.xxx.xxx）
        public string adfrom2;
    }
}
