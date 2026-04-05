using JetBrains.Annotations;

namespace Hotfix
{
    public class HotfixAccountSettingMgr
    {
        private static HotfixAccountSettingMgr _g_instance;
        [NotNull]
        public static HotfixAccountSettingMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new HotfixAccountSettingMgr();
                return _g_instance;
            }
        }
        
        private HotfixAccountSetting _m_hotfixAccountSetting;//账号存档

        
        /// <summary>
        /// 账号存档
        /// </summary>
        public HotfixAccountSetting hotfixAccountSetting { get { return _m_hotfixAccountSetting; } }

        public void init()
        {
            _m_hotfixAccountSetting = new HotfixAccountSetting();
            _m_hotfixAccountSetting.init();
        }

        public void delete()
        {
            if(_m_hotfixAccountSetting == null)
                _m_hotfixAccountSetting = new HotfixAccountSetting();
            _m_hotfixAccountSetting.delete();
        }
        
        public void discard()
        {
            _m_hotfixAccountSetting = null;
        }
    }
}