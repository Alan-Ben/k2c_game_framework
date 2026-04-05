using ALPackage;
using NPEnum;

namespace GOE
{
    public class GGUISubWndCommonItemDetail : _ANPGGUIBasicSubWnd<GGUISubMonoCommonItemDetail>
    {
        private ENPItemType _m_eItemType;//物品类型
        private long _m_lItemId;//物品id
        private long _m_lItemCount;//物品数量
        private string _m_sItemCountStr;//物品数量字符串
        private NPGGuiWndTexture _m_wTexIconWnd;//物品图片
        private GGuiWndSprite _m_wQualityWnd;//品质图片

        public GGUISubWndCommonItemDetail(GGUISubMonoCommonItemDetail _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {
            _m_wTexIconWnd?.discardTexture();
            _m_wQualityWnd?.discardTexture();
        }

        protected override void _onReset()
        {
            if (_m_wTexIconWnd != null)
                _m_wTexIconWnd.discardTexture();

            if (_m_wQualityWnd != null)
                _m_wQualityWnd.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_wTexIconWnd != null)
                _m_wTexIconWnd.discard();
            _m_wTexIconWnd = null;

            if (_m_wQualityWnd != null)
                _m_wQualityWnd.discard();
            _m_wQualityWnd = null;
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgItemIcon != null)
            {
                _m_wTexIconWnd = new NPGGuiWndTexture(wnd.imgItemIcon);
            }

            if (wnd.imgQualityBg != null)
            {
                _m_wQualityWnd = new GGuiWndSprite(wnd.imgQualityBg);
            }
        }


        #region 窗体事件

        /// <summary>
        /// 刷新窗体显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            //物品名称
            ALUGUICommon.setLabelTxt(wnd.txtItemName, GCommon.getItemName(_m_eItemType, _m_lItemId));
            //持有数量
            if (string.IsNullOrEmpty(_m_sItemCountStr))
            {
                if(wnd.txtNumKey != null)
                    ALUGUICommon.setLabelTxt(wnd.txtNum, TextTranslate.instance.getLanguage(wnd.txtNumKey, _m_lItemCount));
                else
                    ALUGUICommon.setLabelTxt(wnd.txtNum, _m_lItemCount);
            }
            else
                ALUGUICommon.setLabelTxt(wnd.txtNum, _m_sItemCountStr);
            //获取途径
            ALUGUICommon.setLabelTxt(wnd.txtAccess, TextTranslate.instance.getLanguage(TransKeyConst.common_resource_production_tip, GCommon.getItemSource(_m_eItemType, _m_lItemId)));
            //物品描述
            ALUGUICommon.setLabelTxt(wnd.txtItemDesc, GCommon.getItemDesc(_m_eItemType, _m_lItemId));
            
            //物品图片
            if (_m_wTexIconWnd != null)
            {
                _m_wTexIconWnd.setTexture(GCommon.getItemTexIcon(_m_eItemType, _m_lItemId));
                _m_wTexIconWnd.showWnd();
            }

            //品质底图
            if (_m_wQualityWnd != null)
            {
                _m_wQualityWnd.setTexture(GCommon.getItemQualityIcon(_m_eItemType, _m_lItemId));
                _m_wQualityWnd.showWnd();
            }
        }

        #endregion


        #region 外部调用

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_itemType">物品类型</param>
        /// <param name="_itemId">物品id</param>
        public void setShowData(ENPItemType _itemType, long _itemId, string _itemCountStr)
        {
            //设置信息
            _m_eItemType = _itemType;
            _m_lItemId = _itemId;
            _m_sItemCountStr = _itemCountStr;

            //刷新窗体
            _refreshWnd();
        }

        public void setShowData(ENPItemType _itemType, long _itemId, long _itemCount)
        {
            //设置信息
            _m_eItemType = _itemType;
            _m_lItemId = _itemId;
            _m_lItemCount = _itemCount;
            _m_sItemCountStr = string.Empty;

            //刷新窗体
            _refreshWnd();
        }
        
        public void setShowData(NPCommonItem _commonItem)
        {
            if(_commonItem == null)
                return;
            
            //设置显示数据
            setShowData(_commonItem.itemType, _commonItem.itemId, GCommon.getItemCount(_commonItem));
        }
        
        public void setShowData(NPCommonItem _commonItem, long _customCount)
        {
            if(_commonItem == null)
                return;
            
            //设置显示数据
            setShowData(_commonItem.itemType, _commonItem.itemId, _customCount);
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_item">物品</param>
        public void setShowData(_IItem _item)
        {
            if (_item == null)
                return;

            //设置显示数据
            setShowData(_item.getItemType(), _item.subId, _item.getCount());
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_item">物品</param>
        public void setShowData(_IItem _item, long _customCount)
        {
            if (_item == null)
                return;

            //设置显示数据
            setShowData(_item.getItemType(), _item.subId, _customCount);
        }
        
        public void setShowData(NPCommon.NPCommon_ItemInfo _itemInfo)
        {
            if(_itemInfo == null)
                return;
            
            setShowData((ENPItemType)_itemInfo.getItemType(), _itemInfo.getSubId(), _itemInfo.getCount());
        }

        public void setShowData(NPCommon.NPCommon_ItemInfo _itemInfo, long _customCount)
        {
            if(_itemInfo == null)
                return;
            
            setShowData((ENPItemType)_itemInfo.getItemType(), _itemInfo.getSubId(), _customCount);
        }
        
        #endregion

    }
}