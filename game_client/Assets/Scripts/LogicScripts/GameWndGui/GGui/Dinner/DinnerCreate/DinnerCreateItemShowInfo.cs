using ClientEnum;

namespace GOE
{
    public enum EDinnerCreateItemType
    {
        Item = 0, // item
        Bar_Celebration = 1, // 庆典
        Bar_Party = 2, // 派对
    }
    public class DinnerCreateItemShowInfo
    {
        private DinnerPermit _m_permit; // 开宴凭证id
        private GDinnerTypeRefObj _m_dinnerTypeRef; // 宴会类型配置
        private EDinnerCreateItemType _m_itemType = EDinnerCreateItemType.Item; // item类型

        public DinnerCreateItemShowInfo(DinnerPermit _permit, GDinnerTypeRefObj _refObj, EDinnerCreateItemType _itemType = EDinnerCreateItemType.Item)
        {
            _m_permit = _permit;
            _m_dinnerTypeRef = _refObj;
            _m_itemType = _itemType;
        }
        public EDinnerShowType getDinnerCreateItemType()
        {
            return _m_dinnerTypeRef.dinner_show_type;
        }

        public GDinnerTypeRefObj dinnerTypeRef => _m_dinnerTypeRef;
        public DinnerPermit permit => _m_permit;
        public EDinnerCreateItemType itemType => _m_itemType;
    }
}