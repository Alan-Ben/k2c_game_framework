using ChatPackage;

namespace GOE
{
    public class ShareIconShow_Child : _IShareIconSHow
    {
        private readonly ChildInfo _m_info;

        public ShareIconShow_Child(ChildInfo _info)
        {
            _m_info = _info;
        }

        public ChildInfo info { get => _m_info; }

        public NPGTextureIndex getIcon()
        {
            return _m_info?.resRef?.icon;
        }

        public NPGSpriteIndex getIconBg()
        {
            return _m_info?.qualityRef?.head_bg;
        }

        public bool needShowSpecial()
        {
            return false;
        }

        public string getName()
        {
            return _m_info.name;
        }

        public _AMsgDetailInfo creatShareInfo()
        {
            return NPMsgDetailInfoFactory.createShareChildInfo(_m_info);
        }
    }
}