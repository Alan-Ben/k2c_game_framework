using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 居民派遣item
    /// </summary>
    public class GGUIMonoMarsResidentDispatchItem : _TALUGUIMonoGridItem
    {
        [ALHeader("建筑图标")]
        public RawImage imgBuild;
        
        [ALHeader("建筑名称")]
        public TextEx txtBuildName;

        [ALHeader("建筑人数文本(已派遣人数/建筑当前可派遣人数限制)")]
        public TextEx txtBuildPeopleNum; // {0}/{1}

        [ALHeader("可派遣的人数文本")]
        // 取以下三者的最小值: 1.休息中居民数 2.该建筑剩余的派遣空位 3.预设值，GRefdataCoreMgr.instance.npGeneral.mars_resident_dispatch_default_num > 0 时
        public TextEx txtDispatchPeopleNum;

        [ALHeader("建筑可派遣(可派遣人数>0)时显示对象列表")]
        public List<GameObject> canDispatchShowObjList;
        [ALHeader("建筑不可派遣(可派遣人数<=0)时显示对象列表")]
        public List<GameObject> cannotDispatchShowObjList;
        
        [ALHeader("派遣按钮")]
        public GameObject btnDispatch;
    }
}