using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴当前佩戴藏品展示界面
    /// </summary>
    public class GGUIWndHeroEquipInfo : _ANPGGUIBasicWnd<GGUIMonoHeroEquipInfo>
    {
        private static GGUIWndHeroEquipInfo _g_instance;
        public static GGUIWndHeroEquipInfo instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndHeroEquipInfo();
                return _g_instance;
            }
        }

        //藏品信息
        private EquipInfo _m_equipInfo;
        //伙伴信息
        private HeroInfo _m_heroInfo;
        //加载出来的品质GO序号
        private NPGGoIndex _m_qualityGoIndex;
        //加载出来的品质GO
        private GameObject _m_qualityGo;
        //藏品图标
        private NPGGuiWndTexture _m_wEquipIcon;
        //额外等级图标
        private NPGGuiWndTexture _m_wAddLevelIcon;

        public GGUIWndHeroEquipInfo() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroEquipInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroEquipInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_HERO_EQUIP_INFO_STRENGTHEN_BTN, _onSimulateClickStrengthenBtn);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_HERO_EQUIP_INFO_STRENGTHEN_BTN, _onSimulateClickStrengthenBtn);
            _pushBackQualityGo();
            _m_wEquipIcon?.hideWnd();
            _m_wAddLevelIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wEquipIcon?.discardTexture();
            _m_wAddLevelIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wEquipIcon?.discard();
            _m_wEquipIcon = null;
            _m_wAddLevelIcon?.discard();
            _m_wAddLevelIcon = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnChange, _onClickChange);
            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onClickDetail);
            ALUGUICommon.uncombineBtnClick(wnd.btnRemove, _onClickRemove);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgEquipIcon != null)
                _m_wEquipIcon = new NPGGuiWndTexture(wnd.imgEquipIcon);

            if (wnd.imgAdditionLevelIcon != null)
                _m_wAddLevelIcon = new NPGGuiWndTexture(wnd.imgAdditionLevelIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnChange, _onClickChange);
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onClickDetail);
            ALUGUICommon.combineBtnClick(wnd.btnRemove, _onClickRemove);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(HeroInfo _heroInfo)
        {
            if (_heroInfo == null)
                return;

            _m_equipInfo = NPPlayer.instance.equipComp.getEquipInfoByHeroId(_heroInfo.id);
            _m_heroInfo = _heroInfo;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_equipInfo == null || _m_equipInfo.equipRef == null)
                return;

            ESpecAttrType heroSpecType = _m_heroInfo != null ? _m_heroInfo.specAttrType : ESpecAttrType.NONE;

            //品质图标GO
            if (wnd.goQualityIconParent != null)
            {
                _pushBackQualityGo();
                _popQualityGo();
            }

            //设置图标
            if (_m_wEquipIcon != null)
            {
                _m_wEquipIcon.showWnd();
                _m_wEquipIcon.setTexture(GCommon.getItemTexIcon(ENPItemType.EQUIP, _m_equipInfo.equipId));
            }

            EQuality equipQuality = GCommon.getItemQuality(ENPItemType.EQUIP, _m_equipInfo.equipId);
            //设置品质等级图标
            if (_m_wAddLevelIcon != null)
            {
                if (equipQuality == EQuality.RED)
                {
                    _m_wAddLevelIcon.showWnd();
                    _m_wAddLevelIcon.setTexture(_m_equipInfo.equipRef.quality_lvl_icon);
                }
                else
                {
                    _m_wAddLevelIcon.hideWnd();
                }
            }

            //设置品质颜色
            NPQualityRefObj qualityRef = GRefdataCoreMgr.instance.getQuality(ENPQualityClass.EQUIP, equipQuality);
            if (qualityRef != null)
                ALUGUICommon.setUIObjColor(wnd.setColorByQualityImage, qualityRef.color);

            //实力
            ALUGUICommon.setLabelTxt(wnd.txtPower, HeroCommon.calEquipAddPower(_m_equipInfo, _m_heroInfo));

            //等级名称
            ALUGUICommon.setLabelTxt(wnd.txtLevel, TextTranslate.instance.getLanguage(TransKeyConst.common_level_num, _m_equipInfo.level));
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.EQUIP, _m_equipInfo.equipId));

            //实力百分比
            ALUGUICommon.setLabelTxt(wnd.txtEquipAddPer, TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, _m_equipInfo.skillAddValue /100f));

            //资质值
            ALUGUICommon.setLabelTxt(wnd.txtEquipTalent, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, _m_equipInfo.talentValue));

            //显示红点
            ALUGUICommon.setGameObjEnable(wnd.goRedTip, NPPlayer.instance.equipComp.haveHigherQualityNoWear(GCommon.getItemQuality(ENPItemType.EQUIP, _m_equipInfo.equipId)));
        }

        //加载品质GO
        private void _popQualityGo()
        {
            if (wnd == null || wnd.goQualityIconParent == null || _m_equipInfo == null)
                return;

            NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.EQUIP, _m_equipInfo.equipId);
            if (qualityExtRef != null && qualityExtRef.quality_go_index != null)
            {
                _m_qualityGoIndex = qualityExtRef.quality_go_index;
                GGoIndexCacheMgr.instance.popItem(_m_qualityGoIndex, _go =>
                {
                    if (_go == null || wnd == null || wnd.goQualityIconParent == null)
                        return;

                    _go.transform.SetParent(wnd.goQualityIconParent);
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

        //模拟点击伙伴佩戴藏品信息界面强化按钮
        private void _onSimulateClickStrengthenBtn()
        {
            _onClickDetail(null);
        }

        #region 点击事件

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_EQUIP_INFO);
        }

        //点击替换按钮
        private void _onClickChange(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndHeroEquipChange.instance, () =>
            {
                GGUIWndHeroEquipChange.instance.showWnd();
                GGUIWndHeroEquipChange.instance.setInfo(_m_equipInfo, _m_heroInfo);
            }, UINodeTagConst.C_HERO_EQUIP_CHANGE);
        }

        //点击查看藏品详情
        private void _onClickDetail(GameObject _go)
        {
            if (_m_equipInfo == null)
                return;

            List<EquipInfo> equipInfoList = new List<EquipInfo>();
            equipInfoList.Add(_m_equipInfo);

            QueueMgr.instance.AddNode(new GMainQueueEquipDetailNode(equipInfoList, _m_equipInfo));
        }

        //点击卸下藏品
        private void _onClickRemove(GameObject _go)
        {
            if (_m_equipInfo == null)
                return;

            NPPlayer.instance.equipComp.reqHeroUnWearEquip(_m_equipInfo.dbId);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_EQUIP_INFO);
        }

        #endregion
    }
}