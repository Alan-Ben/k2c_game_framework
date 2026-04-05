using ALPackage;
using System;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 解锁玩家皮肤界面
    /// </summary>
    public class GGUIWndPlayerSkinUnlock : _ANPGGUIBasicWnd<GGUIMonoPlayerSkinUnlock>
    {
        private static GGUIWndPlayerSkinUnlock _g_instance;
        public static GGUIWndPlayerSkinUnlock instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndPlayerSkinUnlock();
                return _g_instance;
            }
        }

        //形象
        private NPGGUIWndCommonShowCase _m_wShowCase;
        //皮肤数据
        private NPCommon_ItemInfo _m_item;
        //关闭回调
        private Action _m_doneAction;

        private GGUIWndPlayerSkinUnlock() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoPlayerSkinUnlock.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoPlayerSkinUnlock.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _refresh();
        }

        protected override void _onHideWnd()
        {
            _m_wShowCase?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wShowCase?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wShowCase?.discard();
            _m_wShowCase = null;
            
            if (null != _m_doneAction)
                _m_doneAction();
            _m_doneAction = null;
            
            if (null == wnd)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onClickGoTo);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoShowCase != null)
                _m_wShowCase = new NPGGUIWndCommonShowCase(wnd.monoShowCase);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onClickGoTo);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_item"></param>
        /// <param name="_doneAction"></param>
        public void setItem(NPCommon_ItemInfo _item, Action _doneAction)
        {
            if (null == _item)
                return;

            _m_item = _item;
            _m_doneAction = _doneAction;
            _refresh();
        }

        //刷新窗口
        private void _refresh()
        {
            if (null == _m_item || null == wnd)
                return;

            PlayerSkinRefObj skinRef = GRefdataCoreMgr.instance.playerSkinRefCore.getRef(_m_item.getSubId());
            PlayerSkinLevelRefObj skinLevelRef = GRefdataCoreMgr.instance.getPlayerSkinLevelRef(_m_item.getSubId(), 1);
            if (skinRef == null || skinLevelRef == null)
                return;

            if (_m_wShowCase != null)
            {
                _AShowCaseUnitInfoObj[] showCaseUnitInfoObjList = new _AShowCaseUnitInfoObj[1];
                showCaseUnitInfoObjList.SetValue(new ShowCaseCommonResUnitInfoObj(skinRef.td_show), 0);
                _m_wShowCase.showWnd(showCaseUnitInfoObjList);
            }

            //设置皮肤名称
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.PLAYER_SKIN, _m_item.getSubId()));
            //是否拥有属性
            bool isHaveIntimacy = skinLevelRef.player_property != null && skinLevelRef.player_property.getPropertyValue(ENPPlayerPropertyType.INTIMACY) > 0;
            bool isHaveCharm = skinLevelRef.player_property != null && skinLevelRef.player_property.getPropertyValue(ENPPlayerPropertyType.CHARM) > 0;
            ALUGUICommon.setLabelTxt(wnd.txtIntimacy, isHaveIntimacy
                    ? skinLevelRef.player_property.getPropertyValue(ENPPlayerPropertyType.INTIMACY)
                    : 0);
            ALUGUICommon.setLabelTxt(wnd.txtCharm, isHaveCharm
                    ? skinLevelRef.player_property.getPropertyValue(ENPPlayerPropertyType.CHARM)
                    : 0);

            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goHaveIntimacyHideList, !isHaveIntimacy);
            ALUGUICommon.setGameObjEnable(wnd.goHaveIntimacyShowList, isHaveIntimacy);
            ALUGUICommon.setGameObjEnable(wnd.goHaveCharmHideList, !isHaveCharm);
            ALUGUICommon.setGameObjEnable(wnd.goHaveCharmShowList, isHaveCharm);
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
           QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_PLAYER_SKIN_GET);
        }

        //点击前往按钮
        private void _onClickGoTo(GameObject _go)
        {
            //已经打开的情况下，不处理
            if (GGUIWndPlayerSkin.instance.isShow)
                return;

            QueueMgr.instance.AddNode(new GNodeBuilding());
            QueueMgr.instance.AddNode(new GNodePlayerInfo());
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndPlayerSkin.instance,
                UINodeTagConst_PlayerInfo.C_MAIN_PLAYERINFO_SKIN_NODE,
                ()=>GGUIWndPlayerSkin.instance.setSelectSkin(_m_item.getSubId()), null, 0);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_PLAYER_SKIN_GET);
        }
    }
}
