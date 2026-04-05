using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 物品点击查看详情弹窗
    /// </summary>
    public class NPGGUIWndCommonItemDetail_NoCount : _ANPGGUIBasicWnd<NPGGUIMonoCommonItemDetail_NoCount>
    {
        private static NPGGUIWndCommonItemDetail_NoCount _g_instance;
        public static NPGGUIWndCommonItemDetail_NoCount instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new NPGGUIWndCommonItemDetail_NoCount();
                return _g_instance;
            }
        }

        private ENPItemType _m_eItemType;//物品类型
        private long _m_lItemId;//物品id
        private NPGGuiWndTexture _m_wTexIconWnd;//物品图片
        private GGuiWndSprite _m_wQualityWnd;//品质图片

        private NPGGUIWndCommonItemDetail_NoCount() : base(EALUIWndLayer.ADDITION)
        {

        }

        protected override string _monoAssetPath { get { return NPGGUIMonoCommonItemDetail_NoCount.assetPath; } }
        protected override string _monoObjName { get { return NPGGUIMonoCommonItemDetail_NoCount.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

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
        public void setShowData(ENPItemType _itemType, long _itemId)
        {
            //设置信息
            _m_eItemType = _itemType;
            _m_lItemId = _itemId;

            //刷新窗体
            _refreshWnd();
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
            setShowData(_item.getItemType(), _item.subId);
        }

        #endregion
    }
}
