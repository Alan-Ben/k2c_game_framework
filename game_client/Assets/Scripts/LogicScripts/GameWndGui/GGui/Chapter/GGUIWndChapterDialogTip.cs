using System;
using ALPackage;
using NPEnum;
using System.Collections.Generic;
using System.Linq;
using Common.ChapterEnum;
using CommonEnum;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOE
{
    /// <summary>
    /// 关卡自动挑战设定
    /// </summary>
    public class GGUIWndChapterDialogTip : _ATALBasicUIWnd<GGUIMonoChapterDialogTip>
    {
        private NPGGuiWndTexture _m_wTexIconWnd;//物品图片
        private Action _m_doneAction;//关闭回调
        private long _m_uiResPathId;
        
        public GGUIWndChapterDialogTip(long _uiResPathId) : base(EALUIWndLayer.ADDITION)
        {
            _m_uiResPathId = _uiResPathId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_uiResPathId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_uiResPathId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            if(null == wnd)
                return;
            
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                _onClickClose(null);
            }, wnd.autoCloseTime);
        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            if (_m_wTexIconWnd != null) 
                _m_wTexIconWnd.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (null == wnd)
                return;

            if (_m_wTexIconWnd != null) 
                _m_wTexIconWnd.discard();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.texIcon != null) 
                _m_wTexIconWnd = new NPGGuiWndTexture(wnd.texIcon);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        public void setInfo(NPGTextureIndex _index, string _name, Action _doneAction)
        {
            if (_m_wTexIconWnd != null) 
                _m_wTexIconWnd.setTexture(_index);

            _m_doneAction = _doneAction;

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_name));
        }
        
        
        //点击关闭
        private void _onClickClose(GameObject _obj)
        {
            if (null != _m_doneAction)
                _m_doneAction();
            _m_doneAction = null;
        }
        
    }
}