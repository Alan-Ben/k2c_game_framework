using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 伙伴家人可为空头像列表item
    /// </summary>
    public class GGUIWndHeroBlessNullableContainerItem : _ATALBasicUISubWnd<GGUIMonoHeroBlessNullableContainerItem>
    {
        //家人id
        private long _m_lConsortId;
        //头像
        private NPGGuiWndTexture _m_wIconWnd;
        //头像背景
        private GGuiWndSprite _m_wIconBg;
        //是否选中
        private bool _m_bIsSelect;
        //选中item事件
        private Action<GGUIWndHeroBlessNullableContainerItem> _m_bOnSelectItem;

        /// <summary>
        /// 选中item事件
        /// </summary>
        public Action<GGUIWndHeroBlessNullableContainerItem> onSelectItem { get { return _m_bOnSelectItem; } set { _m_bOnSelectItem = value; } }
        /// <summary>
        /// 家人id
        /// </summary>
        public long consortId { get { return _m_lConsortId; } }

        public GGUIWndHeroBlessNullableContainerItem(GGUIMonoHeroBlessNullableContainerItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIconWnd?.hideWnd();
            _m_wIconBg?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIconWnd?.discardTexture();
            _m_wIconBg?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_bOnSelectItem = null;

            _m_wIconWnd?.discard();
            _m_wIconWnd = null;

            _m_wIconBg?.discard();
            _m_wIconBg = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIconWnd = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.imgIconBg != null)
                _m_wIconBg = new GGuiWndSprite(wnd.imgIconBg);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_id"></param>
        public void setInfo(long _consortId)
        {
            if (wnd == null)
                return;

            _m_lConsortId = _consortId;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            if (_m_lConsortId <= 0)
            {
                ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, true);
                ALUGUICommon.setGameObjEnable(wnd.goEmptyHideList, false);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, false);
                ALUGUICommon.setGameObjEnable(wnd.goEmptyHideList, true);

                GGottenConsortInfo consortInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_lConsortId);
                GConsortRefObj consortRef = GRefdataCoreMgr.instance.consortRefCore.getRef(_m_lConsortId);
                long skinId = consortRef != null ? consortRef.default_skin_id : 0;
                if (consortInfo != null && consortInfo.consortSkinShowInfo != null)
                    skinId = consortInfo.consortSkinShowInfo.skinId;
                bool isUnlock = consortInfo != null;

                //设置头像
                _m_wIconWnd?.showWnd();
                _m_wIconWnd?.setTexture(GCommon.getItemTexIcon(ENPItemType.CONSORT_SKIN, skinId));

                //设置品质背景
                _m_wIconBg?.showWnd();
                _m_wIconBg?.setTexture(GCommon.getQualityExtRefObj(ENPItemType.CONSORT, _m_lConsortId)?.consort_head_bg);

                //设置未解锁显隐
                ALUGUICommon.setGameObjEnable(wnd.goLockShowList, !isUnlock);
                ALUGUICommon.setGameObjEnable(wnd.goLockHideList, isUnlock);
            }

            setSelect(false);
        }

        //设置选中状态
        public void setSelect(bool _isSelect)
        {
            if (wnd == null)
                return;

            _m_bIsSelect = _isSelect;
            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, _m_bIsSelect);
            ALUGUICommon.setGameObjEnable(wnd.goSelectHideList, !_m_bIsSelect);
        }

        //点击详情按钮
        private void _onClickItem(GameObject _go)
        {
            if (_m_lConsortId <= 0)
                return;

            _m_bOnSelectItem?.Invoke(this);
        }
    }
}