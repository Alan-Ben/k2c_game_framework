using System;
using ALPackage;
using NPEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱
    /// </summary>
    public class GGUIWndGuildBoxScoreContainerItem : _ATALBasicUISubWnd<GGUIMonoGuildBoxScoreContainerItem>
    {
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        private GuildBoxRefObj _m_boxRef;
        private int _m_iIndex;
        
        public GGUIWndGuildBoxScoreContainerItem(GGUIMonoGuildBoxScoreContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }
    
        protected override void _onHideWnd()
        {
        }
    
        protected override void _onReset()
        {
        }
    
        protected override void _onDiscard()
        {
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        }

        public void setInfo(GuildBoxRefObj _data, int _index)
        {
            _m_boxRef = _data;
            _m_iIndex = _index;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;
            if (_m_boxRef == null) return;
            
            ALUGUICommon.setLabelTxt(wnd.txtName, GCommon.getItemName(ENPItemType.GUILD_BOX, _m_boxRef.id));
            ALUGUICommon.setLabelTxt(wnd.txtScore,
                TextTranslate.instance.getLanguage(TransKeyConst.common_add_num,
                    _m_boxRef.gain_guild_active_point));

            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goOddNumberItemShowList, _m_iIndex % 2 == 0);
            ALUGUICommon.setGameObjEnable(wnd.goOddNumberItemHideList, _m_iIndex % 2 == 1);
        }
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
