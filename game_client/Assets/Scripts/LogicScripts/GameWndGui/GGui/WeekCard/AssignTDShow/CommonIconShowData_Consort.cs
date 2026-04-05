using NPEnum;

namespace GOE
{
    public class CommonIconShowData_Consort : _IAssignShowData
    {
        private readonly ConsortInfo _m_consortData;

        public CommonIconShowData_Consort(long _consortId)
        {
            _m_consortData = new ConsortInfo(_consortId);
        }
        
        public long id { get => _m_consortData.id; }
        public NPGGoIndex td_show { get => _m_consortData?.consortSkinShowInfo?.tdShow; }

        public NPGTextureIndex getIcon()
        {
            return _m_consortData?.consortSkinShowInfo?.consortHeadIcon;
        }

        public string getContent()
        {
            return GCommon.getItemName(ENPItemType.CONSORT, _m_consortData.id);
        }

        public EGameCommonUnlockType getUnlockType()
        {
            return NPPlayer.instance.consortComp.getConsortUnlockType(_m_consortData.id);
        }
    }
}