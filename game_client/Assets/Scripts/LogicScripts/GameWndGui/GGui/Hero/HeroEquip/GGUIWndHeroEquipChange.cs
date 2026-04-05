using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴藏品替换界面
    /// </summary>
    public class GGUIWndHeroEquipChange : _ANPGGUIBasicWnd<GGUIMonoHeroEquipChange>
    {
        private static GGUIWndHeroEquipChange _g_instance;
        public static GGUIWndHeroEquipChange instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndHeroEquipChange();
                return _g_instance;
            }
        }

        //藏品信息
        private EquipInfo _m_equipInfo;
        //伙伴信息
        private HeroInfo _m_heroInfo;
        //藏品item
        private GGUIWndEquipCommonItem _m_wEquipItem;
        //藏品列表
        private GGUIWndHeroEquipChangeGrid _m_wEquipGrid;

        public GGUIWndHeroEquipChange() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoHeroEquipChange.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoHeroEquipChange.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_HERO_WEAR_EQUIP_BY_INDEX, _onSimulateClickHeroWearEquipByIndex);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_HERO_WEAR_EQUIP_BY_INDEX, _onSimulateClickHeroWearEquipByIndex);
            _m_wEquipItem?.hideWnd();
            _m_wEquipGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wEquipItem?.resetWnd();
            _m_wEquipGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wEquipItem?.discard();
            _m_wEquipItem = null;
            _m_wEquipGrid?.discard();
            _m_wEquipGrid = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoEquipItem != null)
                _m_wEquipItem = new GGUIWndEquipCommonItem(wnd.monoEquipItem);

            if(wnd.monoEquipGrid != null)
                _m_wEquipGrid = new GGUIWndHeroEquipChangeGrid(wnd.monoEquipGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(EquipInfo _info, HeroInfo _heroInfo)
        {
            _m_equipInfo = _info;
            _m_heroInfo = _heroInfo;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            //有佩戴信息
            if (_m_equipInfo != null && _m_equipInfo.equipRef != null)
            {
                //藏品信息
                if (_m_wEquipItem != null)
                {
                    _m_wEquipItem.showWnd();
                    _m_wEquipItem.setInfo(_m_equipInfo);
                }

                //实力
                ALUGUICommon.setLabelTxt(wnd.txtPower, TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, HeroCommon.calEquipAddPower(_m_equipInfo, _m_heroInfo).ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT)));

                ALUGUICommon.setGameObjEnable(wnd.goHaveEquipHideList, false);
                ALUGUICommon.setGameObjEnable(wnd.goHaveEquipShowList, true);
            }
            else //没有佩戴信息
            {
                ALUGUICommon.setGameObjEnable(wnd.goHaveEquipHideList, true);
                ALUGUICommon.setGameObjEnable(wnd.goHaveEquipShowList, false);
            }

            List<EquipInfo> infoList = new List<EquipInfo>();
            NPPlayer.instance.equipComp.getEquipInfoList(infoList);
            infoList.Sort(_sortEquipList);

            //如果有佩戴，需要移除正在佩戴的数据
            if (_m_equipInfo != null)
            {
                for (int i = 0; i < infoList.Count; i++)
                {
                    if (infoList[i].dbId == _m_equipInfo.dbId)
                    {
                        infoList.RemoveAt(i);
                        break;
                    }
                }
            }

            //设置藏品列表
            if (_m_wEquipGrid != null)
            {
                _m_wEquipGrid.showWnd();
                _m_wEquipGrid.setInfo(infoList, _m_heroInfo);
            }
        }

        //藏品列表排序：无伙伴装备藏品 > 有伙伴装备藏品 > 实力加成从高到低排序 > 藏品ID
        private int _sortEquipList(EquipInfo _a, EquipInfo _b)
        {
            if (_a == null || _b == null)
                return 0;

            //无伙伴装备藏品 > 有伙伴装备藏品
            bool haveHeroA = _a.wearHeroId > 0;
            bool haveHeroB = _b.wearHeroId > 0;
            int haveHeroCompare = haveHeroA.CompareTo(haveHeroB);
            if (haveHeroCompare != 0)
                return haveHeroCompare;

            //实力加成从高到低排序
            long powerA = HeroCommon.calEquipAddPower(_a, _m_heroInfo);
            long powerB = HeroCommon.calEquipAddPower(_b, _m_heroInfo);
            int powerCompare = powerA.CompareTo(powerB);
            if (powerCompare != 0)
                return -powerCompare;

            return _a.dbId.CompareTo(_b.dbId);
        }

        //模拟点击伙伴选择佩戴藏品，item下标从0开始
        private void _onSimulateClickHeroWearEquipByIndex(params object[] _obj)
        {
            if (_obj == null || _obj.Length == 0)
                return;

            long index = (long)_obj[0];
            if (index < 0)
                return;

            _m_wEquipGrid?.setWearByIndex((int)index);
        }

        #region 点击事件

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_HERO_EQUIP_CHANGE);
        }

        #endregion
    }
}