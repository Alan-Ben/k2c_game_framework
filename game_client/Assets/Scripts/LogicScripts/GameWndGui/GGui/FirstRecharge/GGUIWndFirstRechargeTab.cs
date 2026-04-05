namespace GOE
{
    /// <summary>
    /// 首充礼包界面页签
    /// </summary>
    public class GGUIWndFirstRechargeTab : _ATNPGGUIWndCommonTab<long, GGUIWndFirstRechargeTab>
    {
        // 首充天数
        private long _m_lDay;

        /// <summary>
        /// 首充天数
        /// </summary>
        public long day { get { return _m_lDay; } }

        public GGUIWndFirstRechargeTab(NPGGUIMonoCommonTab _wnd, long _day) : base(_wnd, _day)
        {
            _m_lDay = _day;
        }

        /// <summary>
        /// 设置点击tab
        /// </summary>
        public void setClickTab()
        {
            _onClickSelectButton(null);
        }
    }
}
