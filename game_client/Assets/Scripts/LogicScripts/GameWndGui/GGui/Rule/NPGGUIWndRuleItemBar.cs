using ALPackage;
using NPEnum;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIWndRuleItemBar : _ANPGGUIBasicLoadPrefabSubWnd<NPGGUIMonoRuleItemBar>
    {
        private NPRuleRefObj _m_ruleRefObj;
        private NPGGUIWndRuleListSubItemContainer _m_itemContainer;

        public NPGGUIWndRuleItemBar(Transform _parent) : base(_parent)
        {

        }

        protected override string _monoAssetPath { get { return NPGGUIMonoRuleItemBar.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoRuleItemBar.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {

            _m_ruleRefObj = null;
        }

        protected override void _onDiscard()
        {
            if (null != _m_itemContainer)
            {
                _m_itemContainer.discard();
                _m_itemContainer = null;
            }
            _m_ruleRefObj = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (null != wnd.itemContainer)
            {
                _m_itemContainer = new NPGGUIWndRuleListSubItemContainer(wnd.itemContainer);
            }
        }

        #region 窗体事件

        /// <summary>
        /// 刷新
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_ruleRefObj == null)
                return;
            _m_itemContainer?.showItemList(_m_ruleRefObj.ruleSubList);
            _m_itemContainer?.showWnd();
        }

        #endregion


        #region 外部调用

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_partyInfo"></param>
        public void setInfo(NPRuleRefObj _ruleRefObj)
        {
            _m_ruleRefObj = _ruleRefObj;
            _refreshWnd();
            
            
            //设置完数据需要刷新一下
            foreach (RectTransform rectTrans in wnd.refreshRectTrans)
            {
                if (null == rectTrans)
                    continue;

                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTrans);
            }
        }

        #endregion
    }
}
