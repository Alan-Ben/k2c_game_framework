using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 奇物放入窗口
    /// </summary>
    public class GGUIWndTreasureHuntLabTreasurePutIn : _ANPGGUIBasicWnd<GGUIMonoTreasureHuntLabTreasurePutIn>
    {
        private static GGUIWndTreasureHuntLabTreasurePutIn _g_instance;
        public static GGUIWndTreasureHuntLabTreasurePutIn instance { get { return _g_instance ??= new GGUIWndTreasureHuntLabTreasurePutIn(); } }

        private TreasureHuntLabRefObj _m_labRefObj;//实验室配表数据
        private TreasureHuntGotTreasureInfo _m_treasureInfo;//放入的奇物信息
        
        private GGUIWndTreasureHuntTreasureInfo _m_wTreasureInfo;
        private GGUIWndTreasureHuntSkillInfo<GGUIMonoTreasureHuntSkillInfo> _m_wSkillInfo;

        public GGUIWndTreasureHuntLabTreasurePutIn() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntLabTreasurePutIn.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntLabTreasurePutIn.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoTreasureInfo != null)
                _m_wTreasureInfo = new GGUIWndTreasureHuntTreasureInfo(wnd.monoTreasureInfo);

            if (wnd.monoSkillInfo != null)
                _m_wSkillInfo = new GGUIWndTreasureHuntSkillInfo<GGUIMonoTreasureHuntSkillInfo>(wnd.monoSkillInfo);

            ALUGUICommon.combineBtnClick(wnd.btnSure, _onClickSure);
            ALUGUICommon.combineBtnClick(wnd.btnGoto, _onClickGoto);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onClickSure);
                ALUGUICommon.uncombineBtnClick(wnd.btnGoto, _onClickGoto);
            }

            _m_wTreasureInfo?.discard();
            _m_wTreasureInfo = null;

            _m_wSkillInfo?.discard();
            _m_wSkillInfo = null;

            _m_labRefObj = null;
            _m_treasureInfo = null;
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wTreasureInfo?.hideWnd();
            _m_wSkillInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wTreasureInfo?.resetWnd();
            _m_wSkillInfo?.resetWnd();
        }

        public void setData(TreasureHuntLabRefObj _labRefObj, TreasureHuntGotTreasureInfo _treasureInfo)
        {
            _m_labRefObj = _labRefObj;
            _m_treasureInfo = _treasureInfo;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || !isShow)
                return;

            // 刷新描述文本
            _refreshDescText();

            // 刷新奇物信息
            if (_m_treasureInfo != null && _m_wTreasureInfo != null)
            {
                _m_wTreasureInfo.showWnd();
                _m_wTreasureInfo.setData(_m_treasureInfo);
            }
            else
            {
                _m_wTreasureInfo?.hideWnd();
            }

            // 刷新技能信息
            _refreshSkillInfo();
        }

        /// <summary>
        /// 刷新描述文本
        /// </summary>
        private void _refreshDescText()
        {
            if(wnd == null || _m_labRefObj == null || _m_treasureInfo == null)
                return;

            string descKey = string.IsNullOrEmpty(wnd.txtDescKey) ? TransKeyConst.common_twoParam_str_str : wnd.txtDescKey;
            string labName = TextTranslate.instance.getLanguage(_m_labRefObj.name);
            string treasureName = TextTranslate.instance.getLanguage(_m_treasureInfo.treasureRefObj?.name ?? "");
            
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(descKey, labName, treasureName));
        }

        /// <summary>
        /// 刷新技能信息
        /// </summary>
        private void _refreshSkillInfo()
        {
            if (_m_wSkillInfo != null)
            {
                if (_m_treasureInfo == null || _m_treasureInfo.skillInfo == null)
                {
                    _m_wSkillInfo.hideWnd();
                }
                else
                {
                    _m_wSkillInfo.showWnd();
                    _m_wSkillInfo.setData(_m_treasureInfo.skillInfo);
                }
            }
        }

        private void _onClickSure(GameObject _go)
        {
            // 关闭窗口
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_LAB_TREASURE_PUT_IN);
        }

        private void _onClickGoto(GameObject _go)
        {
            if(_m_treasureInfo == null)
                return;
            
            TreasureHuntGotTreasureInfo treasureInfo = _m_treasureInfo;
            
            // 关闭当前窗口
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_LAB_TREASURE_PUT_IN);
            
            // 打开奇物详情窗口
            GGUIWndTreasureHuntTreasureDetailInfo.instace.setData(null, treasureInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd_OnlyCloseDiscard(GGUIWndTreasureHuntTreasureDetailInfo.instace, () =>
            {
                GGUIWndTreasureHuntTreasureDetailInfo.instace.showWnd();
            }, UINodeTagConst.C_TREASURE_HUNT_TREASURE_DETAIL_INFO);
        }
    }
}