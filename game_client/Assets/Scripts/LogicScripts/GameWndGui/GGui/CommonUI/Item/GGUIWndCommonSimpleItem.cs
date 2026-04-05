using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 简单的item
    /// </summary>
    public class GGUIWndCommonSimpleItem : _ATALBasicUISubWnd<GGUIMonoCommonSimpleItem>
    {
        private NPGGuiWndTexture _m_wTexIconWnd;//物品图片
        private GGuiWndSprite _m_wQualityWnd;//品质图片
        private long _m_itemCount;

        public GGUIWndCommonSimpleItem(GGUIMonoCommonSimpleItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        public long itemCount { get { return _m_itemCount; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wTexIconWnd?.hideWnd();
            _m_wQualityWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wTexIconWnd?.discardTexture();
            _m_wQualityWnd?.discardTexture();
        }

        protected override void _onDiscard()
        {
            _m_wTexIconWnd?.discard();
            _m_wTexIconWnd = null;

            _m_wQualityWnd?.discard();
            _m_wQualityWnd = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wTexIconWnd = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.imgBg != null)
                _m_wQualityWnd = new GGuiWndSprite(wnd.imgBg);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_itemData"></param>
        /// <param name="_isShowCount"></param>
        public void setItem(NPCommonCostItem _itemData, bool _isShowCount = true, EValueFormatType _countValueType = EValueFormatType.NORMAL_NOT_LARGE_STR)
        {
            if (_itemData == null)
                return;

            setItem(_itemData.getIcon(), _itemData.getQualityIcon(), _itemData.count, _isShowCount, _countValueType);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_itemData"></param>
        /// <param name="_isShowCount"></param>
        public void setItem(_IItem _itemData, bool _isShowCount = true, EValueFormatType _countValueType = EValueFormatType.NORMAL_NOT_LARGE_STR)
        {
            if (_itemData == null)
                return;

            setItem(_itemData.getIcon(), _itemData.getQualityIcon(), _itemData.getCount(), _isShowCount, _countValueType);
        }

        public void setItem(ENPItemType _itemType, long _subId, bool _isShowCount = true, EValueFormatType _countValueType = EValueFormatType.NORMAL_NOT_LARGE_STR)
        {
            setItem(_itemType, _subId, GCommon.getItemCount(_itemType, _subId), _isShowCount, _countValueType);
        }
        
        public void setItem(NPCommonItem _itemData, bool _isShowCount = true, EValueFormatType _countValueType = EValueFormatType.NORMAL_NOT_LARGE_STR)
        {
            if (_itemData == null)
                return;

            setItem(GCommon.getItemTexIcon(_itemData.itemType, _itemData.itemId), GCommon.getItemQualityIcon(_itemData.itemType, _itemData.itemId)
                , GCommon.getItemCount(_itemData), _isShowCount, _countValueType);
        }
        
        public void setItem(NPCommonItem _itemData, long _itemCount, bool _isShowCount = true, EValueFormatType _countValueType = EValueFormatType.NORMAL_NOT_LARGE_STR)
        {
            if (_itemData == null)
                return;

            setItem(GCommon.getItemTexIcon(_itemData.itemType, _itemData.itemId), GCommon.getItemQualityIcon(_itemData.itemType, _itemData.itemId)
                , _itemCount, _isShowCount, _countValueType);
        }
        
        public void setItem(ENPItemType _itemType, long _subId, long _itemCount, bool _isShowCount = true, EValueFormatType _countValueType = EValueFormatType.NORMAL_NOT_LARGE_STR)
        {
            setItem(GCommon.getItemTexIcon(_itemType, _subId), GCommon.getItemQualityIcon(_itemType, _subId), _itemCount, _isShowCount, _countValueType);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_iconIndex"></param>
        /// <param name="_bgIndex"></param>
        /// <param name="_count"></param>
        /// <param name="_isShowCount"></param>
        public void setItem(NPGTextureIndex _iconIndex, NPGSpriteIndex _bgIndex, long _count, bool _isShowCount = true, EValueFormatType _countValueType = EValueFormatType.NORMAL_NOT_LARGE_STR)
        {
            if (wnd == null)
                return;

            _setIcon(_iconIndex);
            _setBg(_bgIndex);
            _m_itemCount = _count;

            if (string.IsNullOrEmpty(wnd.txtNumKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtNum, GCommon.getValueFormatStr(_countValueType, _count));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtNum, TextTranslate.instance.getLanguage(wnd.txtNumKey, GCommon.getValueFormatStr(_countValueType, _count)));
            }
            ALUGUICommon.setGameObjEnable(wnd.goTextHideList, _isShowCount);
        }

        //设置图标
        private void _setIcon(NPGTextureIndex _iconIndex)
        {
            if (_m_wTexIconWnd != null)
            {
                if (_iconIndex != null)
                {
                    _m_wTexIconWnd.showWnd();
                    _m_wTexIconWnd.setTexture(_iconIndex);
                }
                else
                    _m_wTexIconWnd.hideWnd();
            }
        }

        //设置背景
        private void _setBg(NPGSpriteIndex _bgIndex)
        {
            if (_m_wQualityWnd != null)
            {
                if (_bgIndex != null)
                {
                    _m_wQualityWnd.showWnd();
                    _m_wQualityWnd.setTexture(_bgIndex);
                }
                else
                    _m_wQualityWnd.hideWnd();
            }
        }
    }
}
