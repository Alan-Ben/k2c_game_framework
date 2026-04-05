using ALPackage;
using Common.MailEnum;
using NPCommon;
using NPEnum;

namespace GOE
{
    public class GGUIWndMailDetail_ItemExpired : GGUIWndMailDetail_Base<GGUIMonoMailDetail_ItemExpired>
    {
        private NPGGuiWndTexture _m_texItemIcon;


        public GGUIWndMailDetail_ItemExpired(GMailDataInfo _info)
            : base(_info)
        {
        }

        protected override void _onResetEx()
        {
            base._onResetEx();
            _m_texItemIcon?.discardTexture();
        }

        protected override void _onWndInitDoneEx()
        {
            base._onWndInitDoneEx();

            if (null == wnd)
                return;
            if (null != wnd.texItemIcon)
            {
                _m_texItemIcon = new NPGGuiWndTexture(wnd.texItemIcon);
            }
        }

        protected override void _onDiscardEx()
        {
            base._onDiscardEx();
            
            _m_texItemIcon?.discard();
            _m_texItemIcon = null;
        }

        protected override void _refreshWndEx()
        {
            if (null == wnd)
                return;
            if (null == _m_miMailDataInfo)
                return;
            if (_m_miMailDataInfo.mailDetailInfo == null)
                return;
            if (_m_miMailDataInfo.mailDetailInfo.getExType() == (int) EMailExtType.TEST_ITEM_LIST)
            {
                NPCommon_ItemList itemList = new NPCommon_ItemList();
                itemList.readPackage(_m_miMailDataInfo.mailDetailInfo.getExData());
                if (itemList.getItemList().Count == 0)
                    return;
                NPCommon_ItemInfo item = itemList.getItemList()[0];
                ENPItemType itemType = (ENPItemType) item.getItemType();
                long itemId = item.getSubId();
                _m_texItemIcon?.showWnd();
                _m_texItemIcon?.setTexture(GCommon.getItemTexIcon(itemType,itemId));

                ALUGUICommon.setLabelTxt(wnd.txtItemName,GCommon.getItemName(itemType,itemId));
                ALUGUICommon.setLabelTxt(wnd.txtItemDesc, GCommon.getItemDesc(itemType,itemId));
            }
        }
        
    }
}