using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 骑士信息跟随窗口
    /// </summary>
    public class GGUIWndCommonToolTip_HeroInfo : _ATNPGGUIWndCommonItemToolTip<GGUIMonoCommonToolTip_HeroInfo>
    {
        private GGUIWndCommonAttrContainer _m_attrContainer;
        // public GGUIWndHeroCommonStarContainer _m_starContainer;
        private long _m_heroId;
        private long _m_star;

        public GGUIWndCommonToolTip_HeroInfo(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            
            _m_attrContainer?.discard();
            _m_attrContainer = null;
            
            // _m_starContainer?.discard();
            // _m_starContainer = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
            if (null == wnd)
                return;
            if (null != wnd.attrContainer)
            {
                _m_attrContainer = new GGUIWndCommonAttrContainer(wnd.attrContainer);
            }
            // if (null != wnd.starContainer)
            // {
            //     _m_starContainer = new GGUIWndHeroCommonStarContainer(wnd.starContainer);
            // }
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(GNodeCommonToolTip_HeroInfo));
        }
        
        public void setInfo(long _heroId, long _star, RectTransform _targetTransRoot, float _intervalX,float _intervalY)
        {
            _m_heroId = _heroId;
            _m_star = _star;
            _refreshWnd();
            setPos(_targetTransRoot,_intervalX, _intervalY);
        }

        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            HeroRefObj heroRef = GRefdataCoreMgr.instance.heroRefCore.getRef(_m_heroId);
            if (null == heroRef)
                return;
            ALUGUICommon.setLabelTxt(wnd.txtName, heroRef.transName);
            //
            // _m_starContainer?.showWnd();
            // _m_starContainer?.showItemList(_m_star);
            //
            // List<CommonAttrItemInfo> attrList = new List<CommonAttrItemInfo>();
            // for (int i = 0; i < heroRef.attr_type_list.Count; i++)
            // {
            //     attrList.Add(new CommonAttrItemInfo(heroRef.attr_type_list[i],0));
            // }
            
            // _m_attrContainer?.showWnd();
            // _m_attrContainer?.showItemList(attrList);
        }
    }
}