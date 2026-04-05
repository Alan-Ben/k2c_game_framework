using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 公会协作属性据点item
    /// </summary>
    public class GGUIWndGuildCooperateAttrPosItem : _ATALBasicUISubWnd<GGUIMonoGuildCooperateAttrPosItem>
    {
        // 图标
        private NPGGuiWndTexture _m_wIcon;
        // 图标2
        private NPGGuiWndTexture _m_wIcon2;
        // 进度条
        private NPGGUIWndProgress _m_wProgress;
        // 属性据点信息
        private GuildCooperatePropertyPointInfo _m_pointInfo;
        // 奖励据点信息
        private GuildCooperateAreaPosRefObj _m_areaPosRef;
        // 是否解锁
        private bool _m_bIsUnlock;

        /// <summary>
        /// 属性据点信息
        /// </summary>
        public GuildCooperatePropertyPointInfo pointInfo { get { return _m_pointInfo; } }

        public GGUIWndGuildCooperateAttrPosItem(GGUIMonoGuildCooperateAttrPosItem _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIcon?.hideWnd();
            _m_wIcon2?.hideWnd();
            _m_wProgress?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
            _m_wIcon2?.discardTexture();
            _m_wProgress?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;
            _m_wIcon2?.discard();
            _m_wIcon2 = null;
            _m_wProgress?.discard();
            _m_wProgress = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if(wnd.imgIcon2 != null)
                _m_wIcon2 = new NPGGuiWndTexture(wnd.imgIcon2);

            if(wnd.monoProgress != null)
                _m_wProgress = new NPGGUIWndProgress(wnd.monoProgress);

            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_isUnlock"></param>
        public void setInfo(GuildCooperatePropertyPointInfo _info, GuildCooperateAreaPosRefObj _areaPosRef, bool _isUnlock)
        {
            if (wnd == null || _info == null)
                return;

            _m_pointInfo = _info;
            _m_areaPosRef = _areaPosRef;
            _m_bIsUnlock = _isUnlock;

            //图标
            BasicAttrRefObj baseAttrRefObj = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((int)_m_pointInfo.attr);
            _m_wIcon?.showWnd();
            _m_wIcon?.setTexture(baseAttrRefObj?.icon);
            _m_wIcon2?.showWnd();
            _m_wIcon2?.setTexture(baseAttrRefObj?.icon);

            //进度条
            float curProgress = _m_pointInfo.hadAttackHp * 1.0f / _m_pointInfo.totalHp;
            _m_wProgress?.showWnd();
            _m_wProgress?.setProgress(curProgress);
        }

        /// <summary>
        /// 模拟点击属性据点
        /// </summary>
        public void simulateClickItem()
        {
            _onClickItem(null);
        }

        /// <summary>
        /// 点击属性据点
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickItem(GameObject _go)
        {
            if (!_m_bIsUnlock)
                return;

            //打开属性据点界面
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndGuildCooperateAttrPosDetail.instance,
                () =>
                {
                    GGUIWndGuildCooperateAttrPosDetail.instance.showWnd();
                    GGUIWndGuildCooperateAttrPosDetail.instance.setInfo(_m_pointInfo, _m_areaPosRef);
                }, UINodeTagConst.C_GUIlD_COOPERATE_ATTR_POINT_DETAIL);
        }
    }
}
