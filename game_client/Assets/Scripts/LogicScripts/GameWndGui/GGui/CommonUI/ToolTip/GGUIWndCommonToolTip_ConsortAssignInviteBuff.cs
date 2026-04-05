using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子指定邀约buff通用ToolTip
    /// </summary>
    public class GGUIWndCommonToolTip_ConsortAssignInviteBuff : _ATNPGGUIWndCommonItemToolTip<GGUIMonoCommonToolTip_ConsortAssignInviteBuff>
    {
        private GGUIWndConsortIconItemContainer _m_wAssignConsortContainer;
        
        public GGUIWndCommonToolTip_ConsortAssignInviteBuff(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            
            if(wnd == null)
                return;

            if (wnd.monoAssignConsortContainer != null)
                _m_wAssignConsortContainer = new GGUIWndConsortIconItemContainer(wnd.monoAssignConsortContainer);
        }
        
        protected override void _onDiscard()
        {
            base._onDiscard();
         
            _m_wAssignConsortContainer?.discard();
            _m_wAssignConsortContainer = null;
        }
        
        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            
            _m_wAssignConsortContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
            
            _m_wAssignConsortContainer?.resetWnd();
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(GNodeCommonToolTip_ConsortAssignInviteBuff));
        }

        public void setData(RectTransform _targetTransRoot, float _intervalX,float _intervalY)
        {
            _refreshWnd();
            
            setPos(_targetTransRoot, _intervalX, _intervalY);
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            List<_IConsortShowInfo> consortShowInfoList = new List<_IConsortShowInfo>();
            if (NPPlayer.instance.consortComp.randCallConsortIdList != null)
            {
                foreach (long consortId in NPPlayer.instance.consortComp.randCallConsortIdList)
                {
                    consortShowInfoList.Add(new ConsortRefShowInfo(consortId));
                }
            }
            
            if (_m_wAssignConsortContainer != null)
            {
                _m_wAssignConsortContainer.showWnd();
                _m_wAssignConsortContainer.showItemList(consortShowInfoList);
            }

            string buffCountKey = string.IsNullOrEmpty(wnd.txtBuffCountKey) ? TransKeyConst.common_value : wnd.txtBuffCountKey;
            ALUGUICommon.setLabelTxt(wnd.txtBuffCount, TextTranslate.instance.getLanguage(buffCountKey, consortShowInfoList.Count));
            
            ALUGUICommon.setGameObjEnable(wnd.hasBuffCountShow, consortShowInfoList.Count > 0);
            ALUGUICommon.setGameObjEnable(wnd.noBuffCountShow, consortShowInfoList.Count <= 0);
        }
    }
}