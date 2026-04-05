using ChatPackage;

namespace GOE
{
    public class ShareIconShow_Consort : _IShareIconSHow
    {
        private readonly GGottenConsortInfo _m_info;

        public ShareIconShow_Consort(GGottenConsortInfo _info)
        {
            _m_info = _info;
        }

        public GConsortRefObj consortRef { get => _m_info.consortRefObj; }
        public GGottenConsortInfo consortInfo { get { return _m_info; } }

        public NPGTextureIndex getIcon()
        {
            return _m_info?.consortSkinShowInfo?.consortHeadIcon;
        }

        public NPGSpriteIndex getIconBg()
        {
            return GCommon.getQualityExtRefObj(NPEnum.ENPItemType.CONSORT, _m_info?.consortId ?? 0)?.consort_head_bg;
        }

        public bool needShowSpecial()
        {
            return false;
        }

        public string getName()
        {
            return _m_info.consortRefObj.transName;
        }

        public _AMsgDetailInfo creatShareInfo()
        {
            return NPMsgDetailInfoFactory.createShareConsortInfo(_m_info);
        }
    }
}