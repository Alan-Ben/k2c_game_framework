using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空区域产出的矿石、奇物详情信息窗口
    /// </summary>
    public class GGUIWndTreasureAreaOreTreasureDetail : _ANPGGUIBasicWnd<GGUIMonoTreasureAreaOreTreasureDetail>
    {
        private static GGUIWndTreasureAreaOreTreasureDetail _g_instance;
        public static GGUIWndTreasureAreaOreTreasureDetail instance { get { return _g_instance ??= new GGUIWndTreasureAreaOreTreasureDetail(); } }

        private NPGGUIWndCommonTab _m_wOreTab;
        private NPGGUIWndCommonTab _m_wTreasureTab;
        private GGUIWndTreasureHuntOreItemContainer _m_wOreContainer;
        private GGUIWndTreasureHuntTreasureItemContainer _m_wTreasureContainer;

        // private Dictionary<EQuality, List<_ITreasureHuntOreInfo>> _m_dQualityOreInfoDic;
        // private Dictionary<EQuality, List<_ITreasureHuntTreasureInfo>> _m_dQualityTreasureInfoDic;
        private List<_ITreasureHuntOreInfo> _m_lOreInfoList;
        private List<_ITreasureHuntTreasureInfo> _m_lTreasureInfoList;
        private ETreasureHuntAreaOreTreasureDetailTabType _m_eSelectedTabType = ETreasureHuntAreaOreTreasureDetailTabType.ORE;

        public GGUIWndTreasureAreaOreTreasureDetail() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureAreaOreTreasureDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureAreaOreTreasureDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoOreTab != null)
            {
                _m_wOreTab = new NPGGUIWndCommonTab(wnd.monoOreTab);
                _m_wOreTab.clickDelegate += _onClickOreTab;
            }

            if (wnd.monoTreasureTab != null)
            {
                _m_wTreasureTab = new NPGGUIWndCommonTab(wnd.monoTreasureTab);
                _m_wTreasureTab.clickDelegate += _onClickTreasureTab;
            }

            if (wnd.monoOreContainer != null)
                _m_wOreContainer = new GGUIWndTreasureHuntOreItemContainer(wnd.monoOreContainer);

            if (wnd.monoTreasureContainer != null)
                _m_wTreasureContainer = new GGUIWndTreasureHuntTreasureItemContainer(wnd.monoTreasureContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            }

            if (_m_wOreTab != null)
            {
                _m_wOreTab.clickDelegate -= _onClickOreTab;
                _m_wOreTab.discard();
                _m_wOreTab = null;
            }

            if (_m_wTreasureTab != null)
            {
                _m_wTreasureTab.clickDelegate -= _onClickTreasureTab;
                _m_wTreasureTab.discard();
                _m_wTreasureTab = null;
            }

            _m_wOreContainer?.discard();
            _m_wOreContainer = null;

            _m_wTreasureContainer?.discard();
            _m_wTreasureContainer = null;

            // _m_dQualityOreInfoDic?.Clear();
            // _m_dQualityOreInfoDic = null;
            // _m_dQualityTreasureInfoDic?.Clear();
            // _m_dQualityTreasureInfoDic = null;
            _m_lOreInfoList = null;//因为_m_lOreInfoList是从外部传入的，所以这里不做Clear操作
            _m_lTreasureInfoList = null;//因为_m_lTreasureInfoList是从外部传入的，所以这里不做Clear操作
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wOreTab?.hideWnd();
            _m_wTreasureTab?.hideWnd();
            _m_wOreContainer?.hideWnd();
            _m_wTreasureContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wOreTab?.resetWnd();
            _m_wTreasureTab?.resetWnd();
            _m_wOreContainer?.resetWnd();
            _m_wTreasureContainer?.resetWnd();
        }

        public void setData(List<_ITreasureHuntOreInfo> _oreList, List<_ITreasureHuntTreasureInfo> _treasureList)
        {
            _m_lOreInfoList = _oreList;
            _m_lTreasureInfoList = _treasureList;
            // // 按品质分组矿石信息
            // if (_m_dQualityOreInfoDic == null)
            //     _m_dQualityOreInfoDic = new Dictionary<EQuality, List<_ITreasureHuntOreInfo>>();
            // _m_dQualityOreInfoDic.Clear();
            //
            // if (_oreList != null)
            // {
            //     List<_ITreasureHuntOreInfo> tmpOreInfoList = null;
            //     foreach (var oreInfo in _oreList)
            //     {
            //         if (oreInfo == null || oreInfo.oreRefObj == null)
            //             continue;
            //
            //         EQuality quality = oreInfo.oreRefObj.quality;
            //         if(!_m_dQualityOreInfoDic.TryGetValue(quality, out tmpOreInfoList) || tmpOreInfoList == null)
            //         {
            //             tmpOreInfoList = new List<_ITreasureHuntOreInfo>();
            //             _m_dQualityOreInfoDic[quality] = tmpOreInfoList;
            //         }
            //         tmpOreInfoList.Add(oreInfo);
            //     }
            // }
            //
            // // 按品质分组奇物信息
            // if (_m_dQualityTreasureInfoDic == null)
            //     _m_dQualityTreasureInfoDic = new Dictionary<EQuality, List<_ITreasureHuntTreasureInfo>>();
            // _m_dQualityTreasureInfoDic.Clear();
            //
            // if (_treasureList != null)
            // {
            //     List<_ITreasureHuntTreasureInfo> tmpTreasureInfoList = null;
            //     foreach (var treasureInfo in _treasureList)
            //     {
            //         if (treasureInfo == null || treasureInfo.treasureRefObj == null)
            //             continue;
            //
            //         EQuality quality = treasureInfo.treasureRefObj.quality;
            //         if(!_m_dQualityTreasureInfoDic.TryGetValue(quality, out tmpTreasureInfoList) || tmpTreasureInfoList == null)
            //         {
            //             tmpTreasureInfoList = new List<_ITreasureHuntTreasureInfo>();
            //             _m_dQualityTreasureInfoDic[quality] = tmpTreasureInfoList;
            //         }
            //         tmpTreasureInfoList.Add(treasureInfo);
            //     }
            // }

            _refreshSelectedTabContent();
        }

        /// <summary>
        /// 设置选中页签
        /// </summary>
        public void setSelectTab(ETreasureHuntAreaOreTreasureDetailTabType _tabType)
        {
            _m_eSelectedTabType = _tabType;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || !isShow)
                return;

            // 刷新Tab状态
            _refreshTabState();

            // 根据选中的Tab显示对应内容
            _refreshSelectedTabContent();
        }

        /// <summary>
        /// 刷新Tab状态
        /// </summary>
        private void _refreshTabState()
        {
            if(!isShow)
                return;
            
            if (_m_wOreTab != null)
            {
                _m_wOreTab.showWnd();
                _m_wOreTab.setSelected(_m_eSelectedTabType == ETreasureHuntAreaOreTreasureDetailTabType.ORE);
            }

            if (_m_wTreasureTab != null)
            {
                _m_wTreasureTab.showWnd();
                _m_wTreasureTab.setSelected(_m_eSelectedTabType == ETreasureHuntAreaOreTreasureDetailTabType.TREASURE);
            }
        }

        /// <summary>
        /// 刷新选中Tab内容
        /// </summary>
        private void _refreshSelectedTabContent()
        {
            if(!isShow)
                return;

            switch (_m_eSelectedTabType)
            {
                case ETreasureHuntAreaOreTreasureDetailTabType.ORE:
                    _showOreContent();
                    break;
                case ETreasureHuntAreaOreTreasureDetailTabType.TREASURE:
                    _showTreasureContent();
                    break;
            }
        }

        /// <summary>
        /// 显示矿石内容
        /// </summary>
        private void _showOreContent()
        {
            if (_m_wOreContainer != null)
            {
                _m_wOreContainer.showWnd();
                _m_wOreContainer.setData(_m_lOreInfoList);
            }

            _m_wTreasureContainer?.hideWnd();
        }

        /// <summary>
        /// 显示奇物内容
        /// </summary>
        private void _showTreasureContent()
        {
            if (_m_wTreasureContainer != null)
            {
                _m_wTreasureContainer.showWnd();
                _m_wTreasureContainer.setData(_m_lTreasureInfoList);
            }

            _m_wOreContainer?.hideWnd();
        }

        private void _onClickOreTab(bool _isOn)
        {
            if (!_isOn || _m_eSelectedTabType == ETreasureHuntAreaOreTreasureDetailTabType.ORE)
                return;

            setSelectTab(ETreasureHuntAreaOreTreasureDetailTabType.ORE);
        }

        private void _onClickTreasureTab(bool _isOn)
        {
            if (!_isOn || _m_eSelectedTabType == ETreasureHuntAreaOreTreasureDetailTabType.TREASURE)
                return;

            setSelectTab(ETreasureHuntAreaOreTreasureDetailTabType.TREASURE);
        }

        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_AREA_ORE_TREASURE_DETAIL);
        }
    }
}