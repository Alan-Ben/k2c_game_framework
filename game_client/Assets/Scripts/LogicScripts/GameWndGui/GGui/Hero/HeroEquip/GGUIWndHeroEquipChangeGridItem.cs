using ALPackage;
using Common.HeroObj;
using CommonEnum;
using NPEnum;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 藏品列表item
    /// </summary>
    public class GGUIWndHeroEquipChangeGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoHeroEquipChangeGridItem >
    {
        //藏品信息
        private EquipInfo _m_equipInfo;
        //伙伴信息
        private HeroInfo _m_heroInfo;
        //藏品item
        private GGUIWndEquipCommonItem _m_wEquipItem;

        public GGUIWndHeroEquipChangeGridItem(GGUIMonoHeroEquipChangeGridItem  _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wEquipItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wEquipItem?.resetWnd();
        }

        protected override void _resetGridItem()
        {
            _m_wEquipItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wEquipItem?.discard();
            _m_wEquipItem = null;

            if (null == wnd)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnWear, _onClickWear);
            ALUGUICommon.uncombineBtnClick(wnd.btnReplace, _onClickReplace);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (wnd.monoEquipItem != null)
                _m_wEquipItem = new GGUIWndEquipCommonItem(wnd.monoEquipItem);

            ALUGUICommon.combineBtnClick(wnd.btnWear, _onClickWear);
            ALUGUICommon.combineBtnClick(wnd.btnReplace, _onClickReplace);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(EquipInfo _equipInfo, HeroInfo _heroInfo)
        {
            _m_equipInfo = _equipInfo;
            _m_heroInfo = _heroInfo;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_equipInfo == null || _m_equipInfo.equipRef == null)
                return;

            //设置图标
            if (_m_wEquipItem != null)
            {
                _m_wEquipItem.showWnd();
                _m_wEquipItem.setInfo(_m_equipInfo);
            }

            //实力加成
            ALUGUICommon.setLabelTxt(wnd.txtPower, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, HeroCommon.calEquipAddPower(_m_equipInfo, _m_heroInfo).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));

            //刷新红点
            _refreshWearRedTip();
        }

        /// <summary>
        /// 刷新穿戴红点提示
        /// </summary>
        private void _refreshWearRedTip()
        {
            if (wnd == null || _m_heroInfo == null || _m_equipInfo == null)
                return;

            //当前伙伴佩戴的藏品数据
            EquipInfo heroWearEquip = NPPlayer.instance.equipComp.getEquipInfoByHeroId(_m_heroInfo.id);
            bool needShow = heroWearEquip == null || GCommon.getItemQuality(ENPItemType.EQUIP, heroWearEquip.equipId) < GCommon.getItemQuality(ENPItemType.EQUIP, _m_equipInfo.equipId);
            ALUGUICommon.setGameObjEnable(wnd.goWearRedTip, needShow);
        }

        //点击穿戴按钮
        private void _onClickWear(GameObject _go)
        {
            if (_m_equipInfo == null || _m_heroInfo == null)
                return;

            NPPlayer.instance.equipComp.reqHeroWearEquip(_m_equipInfo.dbId, _m_heroInfo.id, () =>
            {
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_EQUIP_CHANGE);
            });
        }

        //点击替换按钮
        public void _onClickReplace(GameObject _go)
        {
            if (_m_equipInfo == null || _m_heroInfo == null)
                return;

            //正在佩戴着的伙伴名
            string lastHeroName = GCommon.getItemName(ENPItemType.HERO, _m_equipInfo.wearHeroId);
            //当前伙伴名
            string curHeroName = GCommon.getItemName(ENPItemType.HERO, _m_heroInfo.id);

            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.equip_replaceHeroDesc_str_str_str, lastHeroName, curHeroName),
                TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                null,
                TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                () =>
                {
                    NPPlayer.instance.equipComp.reqHeroWearEquip(_m_equipInfo.dbId, _m_heroInfo.id, () =>
                    {
                        QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_EQUIP_CHANGE);
                    });
                });
        }
    }
}
