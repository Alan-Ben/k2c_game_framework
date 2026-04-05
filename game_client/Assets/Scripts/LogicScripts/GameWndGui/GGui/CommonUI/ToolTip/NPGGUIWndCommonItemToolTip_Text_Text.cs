using ALPackage;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 通用带两个文本的自定义跟随窗口
    /// </summary>
    public class NPGGUIWndCommonItemToolTip_Text_Text : _ATNPGGUIWndCommonItemToolTip<NPGGUIMonoCommonToolTip_Text_Text>
    {
        private string _m_txtOne;

        private string _m_txtTwo;

        private Action _m_actionOne;
        private Action _m_actionTwo;

        public NPGGUIWndCommonItemToolTip_Text_Text(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
            
        }

        protected override void _onShowWnd()
        {
            ALUGUICommon.combineBtnClick(wnd.goOne, _goOneDidClick);
            ALUGUICommon.combineBtnClick(wnd.goTwo, _goTwoDidClick);
        }

        protected override void _onHideWnd()
        {
            ALUGUICommon.uncombineBtnClick(wnd.goOne, _goOneDidClick);
            ALUGUICommon.uncombineBtnClick(wnd.goTwo, _goTwoDidClick);
        }

        protected override float getSelfHeight()
        {
            if (null == wnd)
                return 0;

            return rectTransform.rect.height;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(string _txtOne, string _txtTwo, RectTransform _targetTransRoot, float _interval)
        {
            if(null == wnd)
                return;

            _m_txtOne = _txtOne;
            _m_txtTwo = _txtTwo;

            ALUGUICommon.setLabelTxt(wnd.txtOne, _txtOne);
            ALUGUICommon.setLabelTxt(wnd.txtTwo, _txtTwo);
            if (wnd.txtOne != null) LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.txtOne.rectTransform);
            if (wnd.txtTwo != null) LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.txtTwo.rectTransform);

            //设置位置
            setPos(_targetTransRoot, _interval);

            //刷新
            LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.gameObject.GetComponent<RectTransform>());
        }
        
        public void setClickDelegate(Action _actionOne,Action _actionTwo)
        {
            _m_actionOne = _actionOne;
            _m_actionTwo = _actionTwo;
        }

        private void _goOneDidClick(GameObject _go)
        {
            if (null != _m_actionOne)
                _m_actionOne();
        }

        private void _goTwoDidClick(GameObject _go)
        {
            if (null != _m_actionTwo)
                _m_actionTwo();
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(NPGNodeCommonToolTip_Text_Text));
        }
    }
}