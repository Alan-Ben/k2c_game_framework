using ALPackage;

namespace GOE
{
    /// <summary>
    /// 火星科技加成属性展示数据结构
    /// </summary>
    public struct MasrTechnologyAddPropertyShow
    {
        public _IPropertyShow propertyShow;

        public long value;
    }
    
    /// <summary>
    /// 火星科技加成属性项
    /// </summary>
    public class GGUIWndMasrTechnologyAddOverviewPropertyItem : _ANPGGUIBasicGridItemWnd<GGUIMonoMasrTechnologyAddOverviewPropertyItem>
    {
        private GGUIWndCommonPropertyShow _m_propertyShowWnd;
        private int _m_iInListIndex;

        private MasrTechnologyAddPropertyShow _m_showData;
        
        public GGUIWndMasrTechnologyAddOverviewPropertyItem(GGUIMonoMasrTechnologyAddOverviewPropertyItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.propertyShow != null)
                _m_propertyShowWnd = new GGUIWndCommonPropertyShow(wnd.propertyShow);
        }
        
        protected override void _onDiscard()
        {
            _m_propertyShowWnd?.discard();
            _m_propertyShowWnd = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_propertyShowWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_propertyShowWnd?.resetWnd();
        }
        
        protected override void _resetGridItem()
        {
        }

        public void setData(MasrTechnologyAddPropertyShow _data, int _inListIndex)
        {
            _m_showData = _data;
            _m_iInListIndex = _inListIndex;
            
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            string valueStr = "";
            if (_m_showData.propertyShow != null && _m_showData.propertyShow.isAddPer)
            {
                float value = _m_showData.value / 100f;
                valueStr = TextTranslate.instance.getLanguage(TransKeyConst.common_percentage_num, value.ToString("F2"));
            }
            else
            {
                valueStr = _m_showData.value.ToString();
            }
            // 若是正数则添加“+”号
            if (_m_showData.value >= 0)
                valueStr = TextTranslate.instance.getLanguage(TransKeyConst.common_add_num, valueStr);

            if (_m_propertyShowWnd != null)
            {
                _m_propertyShowWnd.showWnd();
                _m_propertyShowWnd.refreshWnd(_m_showData.propertyShow, valueStr);
            }
            
            if (wnd.inListCyclicIndexShowGoList != null && wnd.inListCyclicIndexShowGoList.Count > 0)
            {
                int index = _m_iInListIndex % wnd.inListCyclicIndexShowGoList.Count;
                for (int i = 0; i < wnd.inListCyclicIndexShowGoList.Count; i++)
                {
                    ALUGUICommon.setGameObjEnable(wnd.inListCyclicIndexShowGoList[i], i == index);
                }
            }
        }
    }
}