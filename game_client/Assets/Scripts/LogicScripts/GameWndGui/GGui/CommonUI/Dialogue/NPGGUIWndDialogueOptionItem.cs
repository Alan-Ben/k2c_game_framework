using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 对话回应选项
    /// </summary>
    public class NPGGUIWndDialogueOptionItem : _ANPGGUIBasicSubWnd<NPGGUIMonoDialogueOptionItem>
    {
        private int _m_iOptionIndex;//该选项的索引
        private Action<NPGGUIWndDialogueOptionItem> _m_aOnSelect;//选中回调
        private NPDialogueResponseOptionRefObj _m_refObj;
        private NPGGUIWndDialogueOptionItemPrefab _wndPrefab;

        public NPGGUIWndDialogueOptionItem(NPGGUIMonoDialogueOptionItem _mono) : base(_mono)
        {
            initWnd();
        }

        /// <summary> 该选项的索引 </summary>
        public int optionIndex { get { return _m_iOptionIndex; } }
        /// <summary>
        /// 选项的配置
        /// </summary>
        public NPDialogueResponseOptionRefObj optionRef { get { return _m_refObj; } }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            _m_aOnSelect = null;
            _m_iOptionIndex = -1;
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_aOnSelect = null;
            _m_iOptionIndex = -1;
            _wndPrefab?.discard();
            _wndPrefab = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
        }

        /// <summary>
        /// 点击选择按钮
        /// </summary>
        /// <param name="_obj"></param>
        private void _onSelectItem(NPGGUIWndDialogueOptionItemPrefab _obj)
        {
            optionRef?.client_effect?.dealEffect(null);
            _m_aOnSelect?.Invoke(this);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_index"></param>
        /// <param name="_onSelect"></param>
        /// <param name="_refObj"></param>
        public void setInfo(int _index, Action<NPGGUIWndDialogueOptionItem> _onSelect, NPDialogueResponseOptionRefObj _refObj)
        {
            if (wnd == null)
                return;
            _m_refObj = _refObj;
            _m_iOptionIndex = _index;
            _m_aOnSelect = _onSelect;
            if(null != _wndPrefab)
            {
                _wndPrefab.discard();
                _wndPrefab = null;
            }

            _wndPrefab = new NPGGUIWndDialogueOptionItemPrefab(_m_refObj, wnd.loadPrefabParent);
            _wndPrefab.setInfo(_onSelectItem);
        }
    }
}
