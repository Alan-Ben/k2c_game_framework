using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// VIP等级升级界面
    /// </summary>
    public class GGUIWndVIPLevelUpgrade : _ANPGGUIBasicWnd<GGUIMonoVIPLevelUpgrade>
    {
        private static GGUIWndVIPLevelUpgrade _g_instance = new GGUIWndVIPLevelUpgrade();

        public static GGUIWndVIPLevelUpgrade instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndVIPLevelUpgrade();

                return _g_instance;
            }
        }

        //vip等级
        private long _m_lVIPLevel;
        //特权列表
        private GGUIWndVIPLevelPrivilegeContainer _m_wPrivilegeContainer;

        public GGUIWndVIPLevelUpgrade() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoVIPLevelUpgrade.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoVIPLevelUpgrade.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wPrivilegeContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wPrivilegeContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wPrivilegeContainer?.discard();
            _m_wPrivilegeContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onClickGoTo);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoPrivilegeContainer != null)
                _m_wPrivilegeContainer = new GGUIWndVIPLevelPrivilegeContainer(wnd.monoPrivilegeContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onClickGoTo);
        }

        public void setInfo(long _vipLevel)
        {
            _m_lVIPLevel = _vipLevel;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtVIPLevel, TextTranslate.instance.getLanguage(TransKeyConst.playerinfo_vip_num, _m_lVIPLevel));
            ALUGUICommon.setLabelTxt(wnd.txtVIPPrivilegeTitle, TextTranslate.instance.getLanguage(TransKeyConst.vip_privilegeDescTitle_num, _m_lVIPLevel));
            _refreshPrivilegeList();
        }

        /// <summary>
        /// 刷新特权列表
        /// </summary>
        private void _refreshPrivilegeList()
        {
            if (wnd == null)
                return;

            List<VIPUpgradePrivilegeShowData> showDataList = new List<VIPUpgradePrivilegeShowData>();
            VipRefObj curVIPRef = GRefdataCoreMgr.instance.vipRefCore.getRef(_m_lVIPLevel);
            VipRefObj lastVIPRef = GRefdataCoreMgr.instance.vipRefCore.getRef(_m_lVIPLevel - 1);

            if (curVIPRef != null && curVIPRef.player_property != null)
            {
                NPPlayerPropertyModifier playerPropertyModifier = curVIPRef.player_property.duplicate();
                if (lastVIPRef == null || lastVIPRef.vip_lvl == 0)
                {
                    //第一级的情况
                    for (int i = 0; i < playerPropertyModifier.propertyObjList.Count; i++)
                    {
                        NPPlayerPropertyInfoObj tempPropertyObj = playerPropertyModifier.propertyObjList[i];
                        if (tempPropertyObj == null)
                            continue;

                        NPPlayerPropertyRefObj playerPropertyRefObj = GRefdataCoreMgr.instance.playerPropertyCore.getRef((long)tempPropertyObj.type);
                        if (playerPropertyRefObj != null && playerPropertyRefObj.is_show)
                        {
                            VIPUpgradePrivilegeShowData tempDate = new VIPUpgradePrivilegeShowData();
                            tempDate.privilegeDesc = GCommon.getPlayerPropertyName(tempPropertyObj.type);
                            tempDate.lastValue = 0;
                            tempDate.curValue = tempPropertyObj.value;
                            showDataList.Add(tempDate);
                        }
                    }
                }
                else
                {
                    //非第一级的情况
                    for (int i = 0; i < playerPropertyModifier.propertyObjList.Count; i++)
                    {
                        NPPlayerPropertyInfoObj tempPropertyObj = playerPropertyModifier.propertyObjList[i];
                        if (tempPropertyObj == null)
                            continue;

                        long curValue = tempPropertyObj.value;
                        long lastValue = 0;
                        if (lastVIPRef.player_property != null && lastVIPRef.player_property.propertyObjList != null)
                        {
                            for (int j = 0; j < lastVIPRef.player_property.propertyObjList.Count; j++)
                            {
                                if (tempPropertyObj.type == lastVIPRef.player_property.propertyObjList[j].type)
                                {
                                    lastValue = lastVIPRef.player_property.propertyObjList[j].value;
                                    break;
                                }
                            }
                        }

                        //只显示有变化的特权
                        if (curValue != lastValue)
                        {
                            VIPUpgradePrivilegeShowData tempDate = new VIPUpgradePrivilegeShowData();
                            tempDate.privilegeDesc = GCommon.getPlayerPropertyName(tempPropertyObj.type);
                            tempDate.lastValue = lastValue;
                            tempDate.curValue = curValue;
                            showDataList.Add(tempDate);
                        }
                    }
                }
            }

            _m_wPrivilegeContainer?.showWnd();
            _m_wPrivilegeContainer?.showItemList(showDataList);
        }

        /// <summary>
        /// 点击关闭
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickClose(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_VIP_LEVEL_UPGRADE);
        }

        /// <summary>
        /// 点击前往
        /// </summary>
        /// <param name="obj"></param>
        private void _onClickGoTo(GameObject obj)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_VIP_LEVEL_UPGRADE);
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndVIPMain.instance, UINodeTagConst.C_VIP_MAIN, null, null, 0);
        }
    }
}
