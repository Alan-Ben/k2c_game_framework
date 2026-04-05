using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 已解锁妃子详情窗口Tab
    /// </summary>
    public class GGUIWndUnLockConsortDetailTab : _ATNPGGUIWndCommonTab<EUnLockConsortDetailWndTabType, GGUIWndUnLockConsortDetailTab>
    {
        [NotNull] private GGUIMonoUnLockConsortDetailWndTabSetting _m_tabSetting;
        [NotNull] public GGUIMonoUnLockConsortDetailWndTabSetting tabSetting { get { return _m_tabSetting; } }
        
        public GGUIWndUnLockConsortDetailTab([NotNull] GGUIMonoUnLockConsortDetailWndTabSetting _tabSetting) : base(_tabSetting.tabMono, _tabSetting.tabType)
        {
            _m_tabSetting = _tabSetting;
        }

        public override void setSelected(bool _isSelect)
        {
            base.setSelected(_isSelect);

            if (_m_tabSetting != null && _m_tabSetting.selectAnimation != null)
            {
                if (_isSelect)
                {
                    if(!string.IsNullOrEmpty(_m_tabSetting.selectAniName))
                        _m_tabSetting.selectAnimation.ForcePlay(_m_tabSetting.selectAniName);
                }
                else
                {
                    if (!string.IsNullOrEmpty(_m_tabSetting.disSelectAniName))
                        _m_tabSetting.selectAnimation.ForcePlay(_m_tabSetting.disSelectAniName);
                }
            }
        }
        
        public void setState(bool _isSelect)
        {
            base.setSelected(_isSelect);

            if (_m_tabSetting != null && _m_tabSetting.selectAnimation != null)
            {
                if (_isSelect)
                {
                    if(!string.IsNullOrEmpty(_m_tabSetting.selectAniName))
                        _m_tabSetting.selectAnimation.Sample(_m_tabSetting.selectAniName, 1);
                }
                else
                {
                    if (!string.IsNullOrEmpty(_m_tabSetting.selectAniName))
                        _m_tabSetting.selectAnimation.Sample(_m_tabSetting.selectAniName, 1);
                }
            }
        }
    }
}