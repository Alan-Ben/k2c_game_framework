using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗选择初始增益item
    /// </summary>
    public class GGUIWndArenaBattleInitBuffItem : _ATALBasicUISubWnd<GGUIMonoArenaBattleInitBuffItem>
    {
        //竞技场增益道具配置
        private ArenaBuffRefObj _m_buffRef;
        //道具图标
        private NPGGuiWndTexture _m_wItemIcon;
        //消耗道具
        private NPGGUIWndCommonItem _m_wCostItem;
        //点击购买回调
        private Action<GGUIWndArenaBattleInitBuffItem> _m_aOnClickItem;

        /// <summary>
        /// 增益道具配置
        /// </summary>
        public ArenaBuffRefObj buffRef => _m_buffRef;
        /// <summary>
        /// 点击购买回调
        /// </summary>
        public Action<GGUIWndArenaBattleInitBuffItem> onClickItem
        {
            get => _m_aOnClickItem;
            set => _m_aOnClickItem = value;
        }


        public GGUIWndArenaBattleInitBuffItem(GGUIMonoArenaBattleInitBuffItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wItemIcon?.hideWnd();
            _m_wCostItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wItemIcon?.discardShowTexture();
            _m_wCostItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wItemIcon?.discard();
            _m_wItemIcon = null;
            _m_wCostItem?.discard();
            _m_wCostItem = null;
            _m_aOnClickItem = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.imgItemIcon != null)
                _m_wItemIcon = new NPGGuiWndTexture(wnd.imgItemIcon);

            if (wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(long _arenaBuffId)
        {
            _m_buffRef = GRefdataCoreMgr.instance.arenaBuffRefCore.getRef(_arenaBuffId);
            _refreshWnd();
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_isSelect"></param>
        public void setSelect(bool _isSelect)
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, _isSelect);
            ALUGUICommon.setGameObjEnable(wnd.goSelectHideList, !_isSelect);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || buffRef == null)
                return;

            //设置道具名称和描述
            ALUGUICommon.setLabelTxt(wnd.txtItemName, TextTranslate.instance.getLanguage(buffRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtItemDesc, TextTranslate.instance.getLanguage(buffRef.desc, buffRef.desc_args));

            //设置道具图标
            if (_m_wItemIcon != null)
            {
                _m_wItemIcon.showWnd();
                _m_wItemIcon.setTexture(buffRef.icon);
            }

            //设置消耗道具
            if (_m_wCostItem != null)
            {
                _m_wCostItem.showWnd();
                _m_wCostItem.setItem(buffRef.cost);
            }

            setSelect(false);
        }

        //点击item
        private void _onClickItem(GameObject _go)
        {
            _m_aOnClickItem?.Invoke(this);
        }
    }
}