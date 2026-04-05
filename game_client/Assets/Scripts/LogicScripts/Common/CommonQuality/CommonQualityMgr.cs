using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 通用质量相关数据管理类
    /// </summary>
    public partial class CommonQualityMgr
    {
        private static CommonQualityMgr _m_instance;

        [NotNull]
        public static CommonQualityMgr instance
        {
            get
            {
                if(_m_instance == null)
                {
                    _m_instance = new CommonQualityMgr();
                }
                return _m_instance;
            }
        }
    }
}