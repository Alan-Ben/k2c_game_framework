using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public enum EConsortMainTargetConsortType
    {
        NONE,
        [InspectorName("可升级加护技能的妃子(作用于已解锁妃子)")]
        CAN_UPGRADE_BLESS_SKILL,
        [InspectorName("指定id的妃子(配置格式 SPECIFIED_ID:id)")]
        SPECIFIED_ID,
    }

    /// <summary>
    /// 家人系统主页面
    /// </summary>
    public class GGUIMonoConsortMain : _ANPBasicUIWndResBarMono
    {
        [ALHeader("总亲密度")]
        public TextEx txtTotalIntimacy;
        
        [ALHeader("总魅力值")]
        public TextEx txtTotalCharm;

        [ALHeader("妃子总数量")]
        public TextEx totalConsortNum;

        [ALHeader("妃子列表")]
        public GGUIMonoConsortListGrid consortList;

        [ALHeader("邀约buff子窗口")]
        public GGUISubMonoConsortInviteBuff monoBuff;
        
        [ALHeader("一键邀约解锁状态配置")] 
        public List<NPCommonEnumStatInfo<EGameCommonUnlockType>> statAKeyInvite; 
        [ALHeader("一键邀约toggle")]
        public NPGGUIMonoCommonToggleEx AkeyInviteToggle;
        
        [ALHeader("邀约按钮")] 
        public GameObject btnInvite;
        [ALHeader("一键邀约按钮")] 
        public GameObject btnAKeyInvite;

        [ALHeader("体力LazyCd")]
        public GGUIMonoCommonLazyCDCountResume monoStrengthLazyCd;

        [ALHeader("返回按钮")]
        public GameObject btnReturn;

        [ALHeader("cg按钮")]
        public GameObject btnCg;
        
        [ALHeader("切换入口妃子按钮")]
        public GameObject btnChgEntranceConsort;
        
        /************
         * 资源加载路径
         */
        public static string assetPath { get { return UIResPathAssistant.getAssetPath(1401); } }
        public static string objName { get { return UIResPathAssistant.getObjName(1401);} }
    }
}