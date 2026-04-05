using System;
using ALPackage;
using CommonEnum;

namespace GOE
{
    /// <summary>
    /// 联盟派遣item
    /// </summary>
    public class GGUIWndGuildDispatchItem : _ATALBasicUISubWnd<GGUIMonoGuildDispatchItem>
    {
        private GuildDispatchInfo _m_iDispatchInfo;//派遣信息
        
        private NPGGuiWndTexture _m_wAttrImg;//属性图标
        private GGUIWndGuildDispatchHeroIconContainer _m_wDispatchHeroContainer;//派遣大臣列表
        
        public GGUIWndGuildDispatchItem(GGUIMonoGuildDispatchItem _wnd) : base(_wnd)
        {
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.attrImg != null)
                _m_wAttrImg = new NPGGuiWndTexture(wnd.attrImg);
            
            if(wnd.monoDispatchHeroContainer != null)
                _m_wDispatchHeroContainer = new GGUIWndGuildDispatchHeroIconContainer(wnd.monoDispatchHeroContainer);
        }
        
        protected override void _onDiscard()
        {
            _m_wAttrImg?.discard();
            _m_wAttrImg = null;
            
            _m_wDispatchHeroContainer?.discard();
            _m_wDispatchHeroContainer = null;
        }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_DISPATCH_INFO_CHG, _onDispatchInfoChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_DISPATCH_INFO_CHG, _onDispatchInfoChg);

            _m_wAttrImg?.hideWnd();
            _m_wDispatchHeroContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wAttrImg?.discardTexture();
            _m_wDispatchHeroContainer?.resetWnd();
        }

        public void setData(GuildDispatchInfo _dispatchInfo)
        {
            _m_iDispatchInfo = _dispatchInfo;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || _m_iDispatchInfo == null)
                return;

            // 获取相性配表数据
            BasicAttrRefObj basicAttrRefObj = GRefdataCoreMgr.instance.basicAttrRefCore.getRef((long) _m_iDispatchInfo.specAttrType);
            if (basicAttrRefObj != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtAttrName, TextTranslate.instance.getLanguage(basicAttrRefObj.name));
                if (_m_wAttrImg != null)
                {
                    _m_wAttrImg.showWnd();
                    _m_wAttrImg.setTexture(basicAttrRefObj.icon);
                }
            }

            ALUGUICommon.setLabelTxt(wnd.txtAddPro,
                TextTranslate.instance.getLanguage(TransKeyConst.common_addPropPer_num, (long) Math.Ceiling(_m_iDispatchInfo.totalAddPer / 100d)));
            
            if (_m_wDispatchHeroContainer != null)
            {
                _m_wDispatchHeroContainer.showWnd();
                _m_wDispatchHeroContainer.setData(_m_iDispatchInfo.dispatchHeroInfoList);
            }
        }

        /// <summary>
        /// 派遣信息变更
        /// </summary>
        private void _onDispatchInfoChg(params object[] _objs)
        {
            if(_m_iDispatchInfo == null || _objs == null || _objs.Length < 1 || !(_objs[0] is ESpecAttrType _specAttrType) || _m_iDispatchInfo.specAttrType != _specAttrType)
                return;
            
            _refreshWnd();
        }
    }
}