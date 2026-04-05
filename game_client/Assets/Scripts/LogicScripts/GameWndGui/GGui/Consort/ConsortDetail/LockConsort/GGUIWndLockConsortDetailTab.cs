using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 未解锁妃子页面tab
    /// </summary>
    public class GGUIWndLockConsortDetailTab : _ATNPGGUIWndTabItem<NPGGUIMonoCommonTab>
    {
        private GGUIMonoLockConsortDetailWndTabSetting _m_tabSetting;
        
        public GGUIWndLockConsortDetailTab([NotNull] GGUIMonoLockConsortDetailWndTabSetting _tabSetting) : base(_tabSetting.tabMono)
        {
            _m_tabSetting = _tabSetting;
        }
        
        [NotNull] public GGUIMonoLockConsortDetailWndTabSetting tabSetting { get { return _m_tabSetting; } }

        public event Action<GGUIWndLockConsortDetailTab> onTabClick;

        protected override void _onDiscard()
        {
            base._onDiscard();

            onTabClick = null;
        }

        protected override void _onClickSelectButton(GameObject _go)
        {
            onTabClick?.Invoke(this);
        }
    }
}