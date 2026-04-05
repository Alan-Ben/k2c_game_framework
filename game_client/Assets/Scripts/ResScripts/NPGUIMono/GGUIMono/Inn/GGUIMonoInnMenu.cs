using System;
using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    [Serializable]
    public class GGUIMonoInnMenuPropertyAdd
    {
        public ESpecAttrType attrType;
        public Text txtValue;
    }
    public class GGUIMonoInnMenu : _AALBasicUIWndMono
    {
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        [ALHeader("菜品加成相关内容"),
         ALInfo("选择要展示的加成属性类型\n" +
                "和取出属性加成值之后应用的语言 key \n" +
                "勾选是否显示为百分比\n" +
                "设置属性值列表并选中加成的相性")]
        public EBonusPropertyType propertyType;
        public string propertyValueKey;
        public bool propertyIsPercentage;
        public List<GGUIMonoInnMenuPropertyAdd> listValueList;
        [ALHeader("列表对象")]
        public GGUIMonoInnMenuGrid monoItemGrid;
        [ALHeader("菜品解锁情况")]
        public Text txtUnlockProgress;
        
        
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(6409); } }
        public static string objName { get { return UIResPathAssistant.getObjName(6409); } }
    }
}