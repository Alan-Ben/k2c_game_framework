using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 其他联盟信息联盟成员列表item
    /// </summary>
    public class GGUIWndGuildOtherInfoMemberGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoGuildOtherInfoMemberGridItem>
    {
        private GuildMemberInfo _m_guildMemberInfo;
        private long _m_lShowSerializeId;
        private RectTransform _m_wContainerContentRect;
        
        public GGUIWndGuildOtherInfoMemberGridItem(GGUIMonoGuildOtherInfoMemberGridItem _wnd, RectTransform _containerContentRect) : base(_wnd)
        {
            _m_wContainerContentRect = _containerContentRect;
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_lShowSerializeId = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerializeId = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        protected override void _resetGridItem()
        {
        }

        protected override void _onDiscard()
        {
            if(wnd != null)
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _clickItem);
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _clickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_id"></param>
        public void setInfo(GuildMemberInfo _guildMemberInfo)
        {
            _m_guildMemberInfo = _guildMemberInfo;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(_m_guildMemberInfo == null || wnd == null)
                return;

            //先重置显示
            ALUGUICommon.setLabelTxt(wnd.txtName, "");
            ALUGUICommon.setLabelTxt(wnd.txtPosition, "");

            //获取玩家信息
            long serializeId = _m_lShowSerializeId;
            _m_guildMemberInfo.getPlayerDetailInfo((_detailInfo) =>
            {
                if(_m_lShowSerializeId != serializeId || _detailInfo == null || wnd == null || _m_guildMemberInfo == null || _m_guildMemberInfo.cid != _detailInfo.cid)
                    return;

                ALUGUICommon.setLabelTxt(wnd.txtName, _detailInfo.name);
                
                GuildPositionRefObj positionRefObj = GRefdataCoreMgr.instance.guildPositionRefCore.getRef(_m_guildMemberInfo.positionId);
                if (positionRefObj != null)
                    ALUGUICommon.setLabelTxt(wnd.txtPosition, TextTranslate.instance.getLanguage(positionRefObj.name));
            });
        }
        
        private void _clickItem(GameObject _go)
        {
            if(_m_guildMemberInfo == null)
                return;
            
            long serializeId = _m_lShowSerializeId;
            _m_guildMemberInfo.getPlayerDetailInfo((_info) =>
            {
                if (serializeId != _m_lShowSerializeId || wnd == null || !isShow)
                    return;

                GCommon.showPlayerInfoWndTip(_info, (RectTransform)_go.transform, 0, _m_wContainerContentRect);
            }, false);
        }
    }
}
