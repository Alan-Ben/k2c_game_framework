using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 星辉效果弹窗
    /// </summary>
    public class GGUIMonoConsortHaloEffect : _AALBasicUIWndMono
    {
        [ALHeader("没有效果时显示")]
        public List<GameObject> noEffectShow;
        
        [ALHeader("有星辉技能时显示")]
        public List<GameObject> hasHaloSkillShow;
        [ALHeader("星辉技能itemContainer")]
        public GGUIMonoConsortHaloSkillLvlItemContainer haloSkillLvlItemContainer;

        [ALHeader("有妃子属性加成时显示")]
        public List<GameObject> hasConsortPropertyAddShow;
     
        [ALHeader("亲密度加成文本")]
        public TextEx txtIntimacyAdd;
        [ALHeader("亲密度加成文本key(需要一个参数, 加成值)")]
        public string txtIntimacyAddKey;
        
        [ALHeader("加护力加成文本")]
        public TextEx txtCharmAdd;
        [ALHeader("加护力加成文本key(需要一个参数, 加成值)")]
        public string txtCharmAddKey;
        
        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1414); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1414);} }
    }
}