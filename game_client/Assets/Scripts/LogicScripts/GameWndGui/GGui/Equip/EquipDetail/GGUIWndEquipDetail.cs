using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 藏品详情界面
    /// </summary>
    public class GGUIWndEquipDetail : _ANPGGUIBasicResBarWnd<GGUIMonoEquipDetail>
    {
        private static GGUIWndEquipDetail _g_instance;
        public static GGUIWndEquipDetail instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndEquipDetail();
                return _g_instance;
            }
        }

        //藏品信息
        private EquipInfo _m_equipInfo;
        //配置数据
        private EquipRefObj _m_equipRef;
        //点击上一个
        private Action _m_aOnClickPre;
        //点击下一个
        private Action _m_aOnClickNext;
        //资源id
        private long _m_lUIResId;
        //节点标签
        private string _m_sNodeTag;
        //藏品图标
        private NPGGuiWndTexture _m_wEquipIcon;
        //等级附加图标
        private NPGGuiWndTexture _m_wAdditionQualityIcon;
        //加载出来的品质GO序号
        private NPGGoIndex _m_qualityGoIndex;
        //加载出来的品质GO
        private GameObject _m_qualityGo;
        //特效列表
        private List<CommonUISfxObj> _m_lSfxObjList;

        /// <summary>
        /// 资源id
        /// </summary>
        public long uiResId { get { return _m_lUIResId; } set { _m_lUIResId = value; } }
        /// <summary>
        /// 节点标签
        /// </summary>
        public string nodeTag { get { return _m_sNodeTag; } set { _m_sNodeTag = value; } }

        public GGUIWndEquipDetail() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_EQUIP_BASE_CHG, _onEquipBaseInfoChg);
            WinMsg.RegisterMsg(WinMsgType.ON_EQUIP_LEVEL_CHG, _onEquipLevelChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_EQUIP_BASE_CHG, _onEquipBaseInfoChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_EQUIP_LEVEL_CHG, _onEquipLevelChg);
            _m_wEquipIcon?.hideWnd();
            _m_wAdditionQualityIcon?.hideWnd();

            _pushBackQualityGo();
            _discardSfx();
        }

        protected override void _onReset()
        {
            _m_wEquipIcon?.discardTexture();
            _m_wAdditionQualityIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wEquipIcon?.discard();
            _m_wEquipIcon = null;

            _m_wAdditionQualityIcon?.discard();
            _m_wAdditionQualityIcon = null;

            _discardSfx();

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnRecycle, _onClickRecycle);
            ALUGUICommon.uncombineBtnClick(wnd.btnLock, _onClickLock);
            ALUGUICommon.uncombineBtnClick(wnd.btnDescDetail, _onClickDescDetail);
            ALUGUICommon.uncombineBtnClick(wnd.btnPre, _onClickPre);
            ALUGUICommon.uncombineBtnClick(wnd.btnNext, _onClickNext);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgEquip != null)
                _m_wEquipIcon = new NPGGuiWndTexture(wnd.imgEquip);

            if (wnd.imgAdditionQualityIcon != null)
                _m_wAdditionQualityIcon = new NPGGuiWndTexture(wnd.imgAdditionQualityIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnRecycle, _onClickRecycle);
            ALUGUICommon.combineBtnClick(wnd.btnLock, _onClickLock);
            ALUGUICommon.combineBtnClick(wnd.btnDescDetail, _onClickDescDetail);
            ALUGUICommon.combineBtnClick(wnd.btnPre, _onClickPre);
            ALUGUICommon.combineBtnClick(wnd.btnNext, _onClickNext);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(EquipInfo _info, EquipRefObj _refObj, bool _isFirst, bool _isLast, Action _onClickPre, Action _onClickNext)
        {
            if (wnd == null || _refObj == null)
                return;

            _m_equipInfo = _info;
            _m_equipRef = _refObj;
            _m_aOnClickPre = _onClickPre;
            _m_aOnClickNext = _onClickNext;

            //设置切换按钮显隐
            ALUGUICommon.setGameObjEnable(wnd.btnPre, !_isFirst);
            ALUGUICommon.setGameObjEnable(wnd.btnNext, !_isLast);

            //刷新窗口
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshBaseInfo();
            _refreshLockState();
        }

        //刷新基础信息
        private void _refreshBaseInfo()
        {
            if (wnd == null || _m_equipRef == null)
                return;

            //品质
            EQuality curQuality = GCommon.getItemQuality(ENPItemType.EQUIP, _m_equipRef.id);

            //设置等级附加图标
            if (_m_wAdditionQualityIcon != null)
            {
                if (curQuality == EQuality.RED)
                {
                    _m_wAdditionQualityIcon.showWnd();
                    _m_wAdditionQualityIcon.setTexture(_m_equipRef.quality_lvl_icon);
                }
                else
                    _m_wAdditionQualityIcon.hideWnd();
            }

            //设置名称描述
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.EQUIP, _m_equipRef.id));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, GCommon.getItemDesc(ENPItemType.EQUIP, _m_equipRef.id));
            ALUGUICommon.setLabelTxt(wnd.txtDesc2, GCommon.getItemDesc(ENPItemType.EQUIP, _m_equipRef.id));

            //设置获取途径
            string sourceStr = GCommon.getItemSource(ENPItemType.EQUIP, _m_equipRef.id);
            ALUGUICommon.setLabelTxt(wnd.txtSource, sourceStr);
            ALUGUICommon.setGameObjEnable(wnd.goHaveSourceShowList, !string.IsNullOrEmpty(sourceStr));
            ALUGUICommon.setGameObjEnable(wnd.goHaveSourceHideList, string.IsNullOrEmpty(sourceStr));

            //设置初始资质
            ALUGUICommon.setLabelTxt(wnd.txtInitTalent, TextTranslate.instance.getLanguage(TransKeyConst.equip_initTalentValue_num, _m_equipRef.initial_talent));
            ALUGUICommon.setLabelTxt(wnd.txtInitSkillNum, TextTranslate.instance.getLanguage(TransKeyConst.equip_initSkillCount_num,_m_equipRef.initial_skill_num));

            //设置藏品图标
            if (_m_wEquipIcon != null)
            {
                _m_wEquipIcon.showWnd();
                _m_wEquipIcon.setTexture(GCommon.getItemTexIcon(ENPItemType.EQUIP, _m_equipRef.id));
            }

            //品质图标GO
            if (wnd.goQualityParent != null)
            {
                _pushBackQualityGo();
                _popQualityGo();
            }
        }

        //刷新锁定状态
        private void _refreshLockState()
        {
            if (wnd == null)
                return;

            bool isLock = _m_equipInfo != null && _m_equipInfo.isLock;
            ALUGUICommon.setGameObjEnable(wnd.goLockShowList, isLock);
            ALUGUICommon.setGameObjEnable(wnd.goLockHideList, !isLock);
        }

        //加载品质GO
        private void _popQualityGo()
        {
            if (wnd == null || wnd.goQualityParent == null || _m_equipRef == null)
                return;

            NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.EQUIP, _m_equipRef.id);
            if (qualityExtRef != null && qualityExtRef.quality_go_index != null)
            {
                _m_qualityGoIndex = qualityExtRef.quality_go_index;
                GGoIndexCacheMgr.instance.popItem(_m_qualityGoIndex, _go =>
                {
                    if (_go == null || wnd == null || wnd.goQualityParent == null)
                        return;

                    _go.transform.SetParent(wnd.goQualityParent);
                    _go.transform.localPosition = Vector3.zero;
                    _go.transform.localScale = Vector3.one;
                    _m_qualityGo = _go;
                });
            }
        }

        //回收品质GO
        private void _pushBackQualityGo()
        {
            if (_m_qualityGoIndex != null && _m_qualityGo != null)
                GGoIndexCacheMgr.instance.pushbackItem(_m_qualityGoIndex, _m_qualityGo);
            _m_qualityGoIndex = null;
            _m_qualityGo = null;
        }

        //销毁特效
        private void _discardSfx()
        {
            if (_m_lSfxObjList != null)
            {
                for (int i = 0; i < _m_lSfxObjList.Count; i++)
                {
                    _m_lSfxObjList[i]?.forceDiscard();
                }
                _m_lSfxObjList.Clear();
                _m_lSfxObjList = null;
            }
        }

        //藏品基础信息变更
        private void _onEquipBaseInfoChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0)
                return;

            long dbId = (long) _objects[0];
            if(_m_equipInfo != null && _m_equipInfo.dbId == dbId)
                _refreshLockState();
        }

        //等级变更
        private void _onEquipLevelChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 3 || _m_equipInfo == null)
                return;

            EquipInfo equipInfo = _objects[0] as EquipInfo;
            long oriLevel = (long)_objects[1];
            long newLevel = (long)_objects[2];
            if (equipInfo != null && equipInfo.dbId == _m_equipInfo.dbId)
            {
                //播放特效
                if (_m_lSfxObjList == null)
                    _m_lSfxObjList = new List<CommonUISfxObj>();

                if (wnd != null && wnd.upgradeSfxParent != null)
                {
                    CommonUISfxObj sfxObj = null;
                    if (newLevel - oriLevel == 1 && wnd.singleUpgradeSfxId > 0)//单次升级成功特效
                        sfxObj = PlaySfxMgr.instance.playUISfx(wnd.singleUpgradeSfxId, wnd.upgradeSfxParent);
                    else if (newLevel - oriLevel > 1 && wnd.tenUpgradeSfxId > 0)//十连升级成功特效
                        sfxObj = PlaySfxMgr.instance.playUISfx(wnd.tenUpgradeSfxId, wnd.upgradeSfxParent);

                    if (sfxObj != null)
                        _m_lSfxObjList.Add(sfxObj);
                }
            }
        }

        #region 点击事件

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(_m_sNodeTag);
        }

        //点击分解
        private void _onClickRecycle(GameObject _go)
        {
            if (_m_equipRef == null || _m_equipInfo == null)
                return;

            //是否锁定
            if (_m_equipInfo.isLock)
            {
                //藏品锁定中无法分解
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.equip_lockCanNotRecycle_none);
                return;
            }

            //是否被佩戴
            if (_m_equipInfo.wearHeroId > 0)
            {
                //佩戴中无法分解
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.equip_wearCanNotRecycle_none);
                return;
            }

            List<NPCommonCostItem> itemList = _m_equipRef.disassemble_get_item_list;
            List<long> dbIdList = new List<long>();
            dbIdList.Add(_m_equipInfo.dbId);
            if (dbIdList.Count <= 0)
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndEquipRecycleConfirm.instance, () =>
            {
                GGUIWndEquipRecycleConfirm.instance.showWnd();
                GGUIWndEquipRecycleConfirm.instance.setInfo(dbIdList, itemList, () =>
                {
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_EQUIP_DETAIL);
                });
            }, UINodeTagConst.C_EQUIP_RECYCLE_CONFIRM);
        }

        //点击锁定
        private void _onClickLock(GameObject _go)
        {
            if (_m_equipInfo == null)
                return;

            NPPlayer.instance.equipComp.reqEquipLockStateChg(_m_equipInfo.dbId, !_m_equipInfo.isLock);
        }

        //点击描述详情
        private void _onClickDescDetail(GameObject _go)
        {
            if (wnd == null || _m_equipRef == null)
                return;

            QueueMgr.instance.AddNode(new NPGNodeCommonToolTip_Text(
                UIResPathAssistant.getAssetPath(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                UIResPathAssistant.getObjName(UIResPathConst.WIN_TOOL_TIP_TEXT_FOLLOW),
                GCommon.getItemDesc(ENPItemType.EQUIP, _m_equipRef.id),
                (RectTransform)_go.transform, wnd.descTipIntervalX, wnd.descTipIntervalY));
        }

        //点击上一个
        private void _onClickPre(GameObject _go)
        {
            _discardSfx();
            _m_aOnClickPre?.Invoke();
        }

        //点击下一个
        private void _onClickNext(GameObject _go)
        {
            _discardSfx();
            _m_aOnClickNext?.Invoke();
        }

        #endregion
    }
}