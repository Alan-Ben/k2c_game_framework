using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 火星基地智能控制容器项窗口
    /// </summary>
    public class GGUIWndMarsIntelligentControlContainerItem : _ANPGGUIBasicSubWnd<GGUIMonoMarsIntelligentControlContainerItem>
    {
        private _IMarsIntelligentControlInfo _m_iInfo;
        private GGUIWndMarsIntelligentControlInfo _m_wIntelligentControlInfo;
        
        public GGUIWndMarsIntelligentControlContainerItem(GGUIMonoMarsIntelligentControlContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化智能控制信息窗口
            if (wnd.intelligentControlInfo != null)
                _m_wIntelligentControlInfo = new GGUIWndMarsIntelligentControlInfo(wnd.intelligentControlInfo);
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
            }
            
            // 销毁智能控制信息窗口
            _m_wIntelligentControlInfo?.discard();
            _m_wIntelligentControlInfo = null;
            
            _m_iInfo = null;
        }

        protected override void _onShowWnd()
        {
            refreshWnd();
        }

        protected override void _onHideWnd()
        {
            // 隐藏子窗口
            _m_wIntelligentControlInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            // 重置子窗口
            _m_wIntelligentControlInfo?.resetWnd();
        }
        
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_info">智能控制信息</param>
        public void setData(_IMarsIntelligentControlInfo _info)
        {
            _m_iInfo = _info;
            refreshWnd();
        }
        
        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !isShow || _m_iInfo == null)
                return;

            // 显示并刷新智能控制信息窗口
            if (_m_wIntelligentControlInfo != null)
            {
                _m_wIntelligentControlInfo.showWnd();
                _m_wIntelligentControlInfo.setInfo(_m_iInfo);
            }
        }
        
        /// <summary>
        /// 点击智能控制项
        /// </summary>
        private void _onClickItem(GameObject _go)
        {
            if(_m_iInfo == null)
                return;

            switch (_m_iInfo.getState())
            {
                case EMarsIntelligentControlState.LOCK:
                    if (_m_iInfo.refObj != null)
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(_m_iInfo.refObj.unlock_cond_unable_tip);
                    break;
                
                default:
                    //打开智能控制详情界面
                    GGUIWndMarsIntelligentControlDetail.addNode(_m_iInfo);
                    // QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndMarsIntelligentControlDetail.instance, UINodeTagConst.C_MARS_INTELLIGENT_CONTROL_DETAIL, null,
                    //     () =>
                    //     {
                    //         GGUIWndMarsIntelligentControlDetail.instance.showWnd();
                    //         GGUIWndMarsIntelligentControlDetail.instance.setInfo(_m_iInfo);
                    //     },
                    //     0);
                    break;
            }
        }
    }
}