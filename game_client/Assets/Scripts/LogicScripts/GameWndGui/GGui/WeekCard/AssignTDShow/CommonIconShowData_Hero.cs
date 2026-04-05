using NPEnum;

namespace GOE
{
    public class CommonIconShowData_Hero : _IAssignShowData
    {
        // private readonly HeroRefShowData _m_heroData;

        public CommonIconShowData_Hero(long _heroId)
        {
            
            // _m_heroData = new HeroRefShowData(_heroId);
        }
        
        public long id { get => 0; }
        public NPGGoIndex td_show { get => null; }

        public NPGTextureIndex getIcon()
        {
            return null;
        }

        public string getContent()
        {
            return GCommon.getItemName(ENPItemType.HERO, 0);
        }

        public EGameCommonUnlockType getUnlockType()
        {
            return NPPlayer.instance.heroComponent.getHeroInfo(0) == null ? EGameCommonUnlockType.LOCK : EGameCommonUnlockType.UNLOCK;
        }
    }
}