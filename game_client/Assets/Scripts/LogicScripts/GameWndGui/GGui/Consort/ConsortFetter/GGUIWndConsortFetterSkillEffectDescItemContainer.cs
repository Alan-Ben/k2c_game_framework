using System.Collections.Generic;

namespace GOE
{
    public class GGUIWndConsortFetterSkillEffectDescItemContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoConsortFetterSkillEffectDescItem, GGUIMonoConsortFetterSkillEffectDescItemContainer, GGUIWndConsortFetterSkillEffectDescItem>
    {
        private ConsortFettersLvlRefObj _m_iFettersLvlRefObj;//当前所处羁绊等级配表数据
        private ConsortFettersSkillRefObj _m_rSkillRefObj;//羁绊技能配表

        private List<GGUIWndConsortFetterSkillEffectDescItem> _m_lSubItemList;
        
        public GGUIWndConsortFetterSkillEffectDescItemContainer(GGUIMonoConsortFetterSkillEffectDescItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override GGUIWndConsortFetterSkillEffectDescItem _createItemWnd(GGUIMonoConsortFetterSkillEffectDescItem _itemMono)
        {
            return new GGUIWndConsortFetterSkillEffectDescItem(_itemMono);
        }
        
        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            _m_lSubItemList?.Clear();
            _m_lSubItemList = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _hideAllItemWnd();
        }

        protected override void _onReset()
        {
            if (_m_lSubItemList != null)
            {
                foreach (var itemWnd in _m_lSubItemList)
                {
                    itemWnd?.resetWnd();
                }       
            }
        }

        public void setData(ConsortFettersLvlRefObj _fettersLvlRefObj, ConsortFettersSkillRefObj _skillRefObj)
        {
            _m_iFettersLvlRefObj = _fettersLvlRefObj;
            _m_rSkillRefObj = _skillRefObj;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(_m_rSkillRefObj == null)
                return;

            if (GRefdataCoreMgr.instance.consortFettersLvlRefCore.refList == null ||
                GRefdataCoreMgr.instance.consortFettersLvlRefCore.refList.Count <= 0)
            {
                _hideAllItemWnd();
                return;
            }

            if(_m_lSubItemList == null)
                _m_lSubItemList = new List<GGUIWndConsortFetterSkillEffectDescItem>();

            GGUIWndConsortFetterSkillEffectDescItem itemWnd = null;
            int wndCount = 0;
            ConsortFettersLvlRefObj nowFetterSkillLvlCorrespondingFettersLvl = null;//当前羁绊技能等级对应的羁绊等级
            foreach (var fettersSkillLvlRefObj in _m_rSkillRefObj.skillLvlRefList)
            {
                if(fettersSkillLvlRefObj == null)
                    continue;

                //对应的羁绊技能等级 小于 玩家当前羁绊等级对应羁绊技能等级, 不需要展示
                if(fettersSkillLvlRefObj.lvl < (_m_iFettersLvlRefObj?.consort_fetters_skill_lvl ?? 0))
                    continue;
                
                // 查找当前羁绊技能等级对应的羁绊等级
                nowFetterSkillLvlCorrespondingFettersLvl =
                    GRefdataCoreMgr.instance.consortFettersLvlRefCore.refList.Find((_refObj) =>
                        _refObj != null && _refObj.consort_fetters_skill_lvl == fettersSkillLvlRefObj.lvl);
                if(nowFetterSkillLvlCorrespondingFettersLvl == null)
                    continue;
                
                if (wndCount >= _m_lSubItemList.Count)
                {
                    itemWnd = addItemWnd();
                    _m_lSubItemList.Add(itemWnd);
                }
                else
                {
                    itemWnd = _m_lSubItemList[wndCount];
                    if (itemWnd == null)
                    {
                        itemWnd = addItemWnd();
                        _m_lSubItemList[wndCount] = itemWnd;
                    }
                }
                
                if(itemWnd == null)
                    return;

                wndCount++;
                
                itemWnd.showWnd();
                itemWnd.setData(nowFetterSkillLvlCorrespondingFettersLvl, _m_rSkillRefObj, 
                    (_m_iFettersLvlRefObj == null || _m_iFettersLvlRefObj.lvl < nowFetterSkillLvlCorrespondingFettersLvl.lvl) ? EGameCommonUnlockType.LOCK : EGameCommonUnlockType.UNLOCK);
            }

            for (int i = wndCount; i < _m_lSubItemList.Count; i++)
            {
                itemWnd = _m_lSubItemList[i];
                if (itemWnd != null)
                {
                    itemWnd.hideWnd();
                }
            }
        }

        private void _hideAllItemWnd()
        {
            if (_m_lSubItemList != null)
            {
                foreach (var itemWnd in _m_lSubItemList)
                {
                    itemWnd?.hideWnd();
                }       
            }
        }
    }
}