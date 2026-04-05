using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 藏品列表item
    /// </summary>
    public class GGUIWndEquipRecycleSelectGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoEquipRecycleSelectGridItem>
    {
        //展示信息
        private EquipInfo _m_showInfo;
        //点击回调
        private Action<GGUIWndEquipRecycleSelectGridItem> _m_dClickDelegate;
        //藏品item
        private GGUIWndEquipCommonItem _m_wEquipItem;
        //是否选中
        private bool _m_bIsSelect;

        /// <summary>
        /// 展示信息
        /// </summary>
        public EquipInfo showInfo { get { return _m_showInfo; }}
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool isSelect { get { return _m_bIsSelect; } }
        /// <summary>
        /// 点击回调
        /// </summary>
        public Action<GGUIWndEquipRecycleSelectGridItem> clickDelegate { get { return _m_dClickDelegate; } set { _m_dClickDelegate = value; } }

        public GGUIWndEquipRecycleSelectGridItem(GGUIMonoEquipRecycleSelectGridItem _wnd)
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
            _m_dClickDelegate = null;

            _m_wEquipItem?.discard();
            _m_wEquipItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if(wnd.monoEquipItem != null)
                _m_wEquipItem = new GGUIWndEquipCommonItem(wnd.monoEquipItem);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(EquipInfo _info)
        {
            _m_showInfo = _info;
            _m_bIsSelect = false;
            _refreshWnd();
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            _m_bIsSelect = _isSelect;
            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, _m_bIsSelect);
            ALUGUICommon.setGameObjEnable(wnd.goSelectHideList, !_m_bIsSelect);
        }

        /// <summary>
        /// 设置点击item
        /// </summary>
        public void setClickItem()
        {
            _onClickItem(null);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_showInfo == null || _m_showInfo.equipRef == null)
                return;

            EQuality quality = GCommon.getItemQuality(ENPItemType.EQUIP, _m_showInfo.equipRef.id);
            NPQualityExtRefObj qualityExtRef = GCommon.getQualityExtRefObj(ENPItemType.EQUIP, _m_showInfo.equipRef.id);

            //设置藏品item
            if (_m_wEquipItem != null)
            {
                _m_wEquipItem.showWnd();
                _m_wEquipItem.setInfo(_m_showInfo);
            }
        }

        //点击item
        private void _onClickItem(GameObject _go)
        {
            setSelect(!_m_bIsSelect);
            _m_dClickDelegate?.Invoke(this);
        }
    }
}
