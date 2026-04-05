using ALBasicProtocolPack;

namespace GOE
{
    public class GSProtocolDispather : ALBasicProtocolDispather
    {
        private static GSProtocolDispather _g_instance = new GSProtocolDispather();

        public static GSProtocolDispather instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GSProtocolDispather();

                return _g_instance;
            }
        }

        public GSProtocolDispather()
        {
			RegProtocol(new GSMainDealer_002_InitOp());
            RegProtocol(new GSMainDealer_004_PlayerOp());
            RegProtocol(new NPGSMainDealer_006_BagItemtOp());
            RegProtocol(new GSMainDealer_007_CommOp());
			RegProtocol(new GSMainDealer_008_TravelOp());
            RegProtocol(new GSMainDealer_009_MailOp());
			RegProtocol(new GSMainDealer_010_BuildingOp());
			RegProtocol(new GSMainDealer_012_ActivityTeamOp());
            RegProtocol(new GSMainDealer_015_ConsortOp());

            RegProtocol(new GSMainDealer_013_HeroOp());
            RegProtocol(new GSMainDealer_014_ChildOp());
            RegProtocol(new GSMainDealer_016_ChapterOp());
            RegProtocol(new GSMainDealer_017_ActivityOp());
			RegProtocol(new GSMainDealer_018_PlayerSkinOp());
			RegProtocol(new GSMainDealer_019_DinnerOp());

            RegProtocol(new GSMainDealer_021_PlayerInfo());
            RegProtocol(new NPGSMainDealer_022_ChatOp());
			RegProtocol(new GSMainDealer_023_ArenaOp());
			RegProtocol(new GSMainDealer_024_DungeonOp());
            RegProtocol(new GSMainDealer_028_QuestOp());
            RegProtocol(new GSMainDealer_030_ShopOp());
			RegProtocol(new GSMainDealer_032_GuildOp());
			RegProtocol(new GSMainDealer_033_SimpleActivityOp());
			RegProtocol(new GSMainDealer_034_InnOp());
			RegProtocol(new GSMainDealer_035_MuseumOp());
			RegProtocol(new GSMainDealer_036_TreasureHuntOp());
			RegProtocol(new GSMainDealer_037_GuildDungeonOp());
			RegProtocol(new GSMainDealer_038_MarsOp());
			RegProtocol(new GSMainDealer_039_MarsBuildingOp());			
            RegProtocol(new GSMainDealer_040_MarsPeopleOp());
			RegProtocol(new GSMainDealer_041_MarsExploreOp());
			RegProtocol(new GSMainDealer_042_GuildRelatedOp());
        }
    }
}