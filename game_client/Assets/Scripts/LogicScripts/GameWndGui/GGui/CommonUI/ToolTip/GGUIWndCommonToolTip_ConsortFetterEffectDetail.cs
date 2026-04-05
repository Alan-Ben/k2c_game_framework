using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndCommonToolTip_ConsortFetterEffectDetail : _ATNPGGUIWndCommonItemToolTip<GGUIMonoCommonToolTip_ConsortFetterEffectDetail>
    {
        private int _m_iFettersLvl;//当前所处羁绊等级
        
        private GGUIWndConsortFetterEffectDescItemContainer _m_wEffectDescItemContainer;
        [NotNull] private List<ConsortFettersLvlRefObj> _m_lTmpConsortFettersLvlRefObjList = new List<ConsortFettersLvlRefObj>();//显示的羁绊等级列表
        
        public GGUIWndCommonToolTip_ConsortFetterEffectDetail(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            
            if(wnd == null)
                return;
            
            if(wnd.monoFettersLvlEffectDescContainer != null)
            {
                _m_wEffectDescItemContainer = new GGUIWndConsortFetterEffectDescItemContainer(wnd.monoFettersLvlEffectDescContainer);
            }
        }
        
        protected override void _onDiscard()
        {
            base._onDiscard();
            
            _m_wEffectDescItemContainer?.discard();
            _m_wEffectDescItemContainer = null;
        }
        
        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            
            _m_wEffectDescItemContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
            
            _m_wEffectDescItemContainer?.resetWnd();
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(GNodeCommonToolTip_ConsortFetterEffect));
        }

        public void setData(int _fettersLvl, RectTransform _targetTransRoot, float _intervalX,float _intervalY)
        {
            _m_iFettersLvl = _fettersLvl;
            
            _refreshWnd();
            
            setPos(_targetTransRoot, _intervalX, _intervalY);
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            _m_lTmpConsortFettersLvlRefObjList.Clear();
            _m_lTmpConsortFettersLvlRefObjList.AddRange(GRefdataCoreMgr.instance.consortFettersLvlRefCore.refList);
            _m_lTmpConsortFettersLvlRefObjList.Reverse();
            
            if (_m_wEffectDescItemContainer != null)
            {
                _m_wEffectDescItemContainer.showWnd();
                _m_wEffectDescItemContainer.setData(_m_lTmpConsortFettersLvlRefObjList, _m_iFettersLvl);
            }
        }
    }
}