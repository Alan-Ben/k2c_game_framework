using System;
using System.Collections.Generic;


namespace ALPackage
{

    /***************
     * xml导出路径选择对象
     **/
    public class ALExportAllItem : _AALComfirmItem
    {
        /** 导出所有的操作函数 */
        private Action _m_dExportAllAction = default(Action);

        /****************
         * 显示的说明
         **/
        protected override string _text { get { return "全 部 导 出"; } }
        /**************
         * 具体的处理函数
         */
        protected override void _dealComfirm()
        {
            //调用所有的处理
            if (_m_dExportAllAction != null) {
                _m_dExportAllAction();
            }
                
        }

        /** 注册导出函数 */
        public void regExportFunc(Action _action)
        {
            if (null == _action)
                return;

            _m_dExportAllAction += _action;
        }
    }

    public class ALSelectItem : _AALToggleItem
    {
        private string _m_sText;
        /** 选择所有的操作函数 */
        private Action<bool> _m_dSelected = default(Action<bool>);

        public ALSelectItem (string _text) {
             _m_sText = _text;
        }

        public ALSelectItem (string _text, Action<bool> _action) {
            _m_sText = _text;
            _m_dSelected = _action;
        }

        /****************
         * 显示的说明
         **/
        protected override string _text { get { return _m_sText; } }

         /**************
         * 具体的处理函数
         */
        protected override void _dealTgeChg (bool _isOn) {
           //调用处理
           if(_m_dSelected != null)
               _m_dSelected(_isOn);
        }

        /** 注册导出函数 */
        public void regSelectFunc(Action<bool> _action)
        {
            if (null == _action)
                return;

            _m_dSelected += _action;
        }
    }
}

