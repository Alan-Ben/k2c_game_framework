using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// 单个大区item对象
    /// </summary>
    public class NPGGUIWndServerAreaItem : _ANPGGUIBasicSubWnd<NPGGUIMonoServerAreaItem>
    {
        private Action<NPGGUIWndServerAreaItem> _m_dOnClick;//点击处理函数
        private GGUIWndCommonSetColorTab _m_clickToggle;//点击的按钮tab
        private string _m_areaTag;//大区标识

        public NPGGUIWndServerAreaItem(NPGGUIMonoServerAreaItem _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        /******************
         * 显示窗口的事件函数
         **/
        protected override void _onShowWnd()
        {

        }

        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {
        }

        /******************
         * 重置窗口数据的事件函数
         **/
        protected override void _onReset()
        {

        }

        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            _m_clickToggle.discard();
            _m_clickToggle = null;
        }

        /*************
        * 窗口初始化完成调用的函数
        * */
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.clickGo)
            {
                _m_clickToggle = new GGUIWndCommonSetColorTab(wnd.clickGo);

                _m_clickToggle.showWnd();
                _m_clickToggle.clickDelegate = _onClickTab;
                setSelected(false);
            }

        }

        /// <summary>
        /// 大区id
        /// </summary>
        /// <returns></returns>
        public string getAreaTag()
        {
            return _m_areaTag;
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_areaId"></param>
        /// <param name="_areaObj"></param>
        /// <param name="_clickCallback"></param>
        public void setInfo(string _areaTag, NPLoginAreaRefObj _areaObj, Action<NPGGUIWndServerAreaItem> _clickCallback)
        {
            _m_areaTag = _areaTag;
            _m_dOnClick = _clickCallback;

            if (null == wnd || null == _areaObj)
                return;

            ALUGUICommon.setLabelTxt(wnd.areaName, TextTranslate.instance.getLanguage(_areaObj.name));
        }

        /// <summary>
        /// 点击处理
        /// </summary>
        /// <param name="_obj"></param>
        private void _onClickTab(bool _obj)
        {
            if (null != _m_dOnClick)
                _m_dOnClick(this);
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_isSelected"></param>
        public void setSelected(bool _isSelected)
        {
            if (null != _m_clickToggle)
            {
                _m_clickToggle.setSelected(_isSelected);
            }
        }
    }
}
