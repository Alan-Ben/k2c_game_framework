using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class NPGGUIWndPlayerLvUpPropItem : _ATALBasicUISubWnd<NPGGUIMonoPlayerLvUpPropItem>
    {
        
        public NPGGUIWndPlayerLvUpPropItem(NPGGUIMonoPlayerLvUpPropItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
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
            
        }
        
        /// <summary>
        /// 设置显示的属性
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(NPPlayerLvUpShowInfo _info)
        {
            if (null == wnd)
            {
                return;
            }
            ALUGUICommon.setLabelTxt(wnd.propOldValueTxt, GCommon.getPlayerPropertyValueStr(_info.type,_info.oldValue));
            ALUGUICommon.setLabelTxt(wnd.propNewValueTxt, GCommon.getPlayerPropertyValueStr(_info.type,_info.newValue));
            ALUGUICommon.setLabelTxt(wnd.propNameTxt, GCommon.getPlayerPropertyName(_info.type));
        }
    }
}