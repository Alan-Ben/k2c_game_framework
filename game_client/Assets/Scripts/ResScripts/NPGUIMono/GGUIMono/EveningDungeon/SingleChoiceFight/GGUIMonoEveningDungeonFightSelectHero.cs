using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 选择大臣页面
    /// </summary>
    public class GGUIMonoEveningDungeonFightSelectHero : _AALBasicUIWndMono
    {
        [ALHeader("选中大臣的大臣头像")]
        public GGUIMonoHeroIconNullableItem selectHeroIcon;
        
        [ALHeader("选中大臣的战斗攻击力")]
        public TextEx txtSelectHeroFightATK;
        [ALHeader("选中大臣的战斗攻击力描述key(一个参数, 大臣攻击力)")]
        public string txtSelectHeroFightATKKey;
        
        [ALHeader("选择大臣Grid")]
        public GGUIMonoEveningDungeonFightSelectHeroItemGrid monoSelectHeroItemGrid;
        
        [ALHeader("确认按钮")]
        public GameObject btnSure;

        [ALHeader("关闭按钮")]
        public GameObject btnClose;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(5503); } }
        public static string objName { get { return UIResPathAssistant.getObjName(5503);} }
    }
}