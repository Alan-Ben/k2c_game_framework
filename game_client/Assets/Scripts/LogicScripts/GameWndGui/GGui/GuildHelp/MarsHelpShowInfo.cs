using System;
using Common.GuildEnum;
using Common.GuildObj;

namespace GOE
{
    public class GuildMarsHelpShowInfo
    {
        public bool isMyHelp;
        public long senderCid;// 发起玩家CID
        public Common.GuildEnum.EGuildMarsHelpObjType objType;// 对象类型
        public long objId;// 对象实例ID
        public int objLvl;// 对象等级
        private long buildingId;
        public int dealLimit;// 求助允许处理的次数上限
        public int dealedCount;// 被帮助的次数
        public int dealSecs;// 求助扣除的时长（秒）
        public GuildMarsHelpShowInfo(Guild_MarsHelpShowInfo _guildMarsHelpShowInfo)
        {
            isMyHelp = false;
            if (_guildMarsHelpShowInfo != null)
            {
                senderCid = _guildMarsHelpShowInfo.getSenderCid();
                objType = _guildMarsHelpShowInfo.getObjType();
                objId = _guildMarsHelpShowInfo.getObjId();
                dealLimit = _guildMarsHelpShowInfo.getDealLimit();
                dealedCount = _guildMarsHelpShowInfo.getDealedCount();
                switch (objType)
                {
                    case EGuildMarsHelpObjType.BUILDING_QUEUE:
                        Guild_MarsHelp_BuildingQueue building = new Guild_MarsHelp_BuildingQueue();
                        building.readPackage(_guildMarsHelpShowInfo.getExt());
                        objLvl = building.getLvl();
                        buildingId = building.getBuildingId();
                        break;
                    case EGuildMarsHelpObjType.TECH_UP:
                        Guild_MarsHelp_TechUp techUp = new Guild_MarsHelp_TechUp();
                        techUp.readPackage(_guildMarsHelpShowInfo.getExt());
                        objLvl = techUp.getLvl();
                        break;
                    case EGuildMarsHelpObjType.TEAM_REPAIR:
                        break;
               
                }
            }
        }

        public GuildMarsHelpShowInfo(Guild_MarsHelpInfo _guildMarsHelpInfo)
        {
            isMyHelp = true;
            if (_guildMarsHelpInfo != null)
            {
                objType = _guildMarsHelpInfo.getObjType();
                objId = _guildMarsHelpInfo.getObjId();
                dealLimit = _guildMarsHelpInfo.getDealLimit();
                dealedCount = _guildMarsHelpInfo.getDealedCount();
                dealSecs = _guildMarsHelpInfo.getDealSecs();
                switch (objType)
                {
                    case EGuildMarsHelpObjType.BUILDING_QUEUE:
                        Guild_MarsHelp_BuildingQueue building = new Guild_MarsHelp_BuildingQueue();
                        building.readPackage(_guildMarsHelpInfo.getExt());
                        objLvl = building.getLvl();
                        buildingId = building.getBuildingId();
                        break;
                    case EGuildMarsHelpObjType.TECH_UP:
                        Guild_MarsHelp_TechUp techUp = new Guild_MarsHelp_TechUp();
                        techUp.readPackage(_guildMarsHelpInfo.getExt());
                        objLvl = techUp.getLvl();
                        break;
                    case EGuildMarsHelpObjType.TEAM_REPAIR:
                        break;
               
                }
            }
        }

        public string getDetailStr()
        {
            switch (objType)
            {
                case EGuildMarsHelpObjType.NONE:
                    break;
                case EGuildMarsHelpObjType.BUILDING_QUEUE:
                    MarsBuildingInfo buildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoByQueueId(buildingId);
                    string buildName = buildingInfo?.nameTranslated;
                    return TextTranslate.instance.getLanguage(TransKeyConst.guild_mars_help_build_desc, objLvl, buildName);
                    break;
                case EGuildMarsHelpObjType.TECH_UP:
                    MarsTechnologyRefObj technologyRef = GRefdataCoreMgr.instance.marsTechnologyRefCore.getRef(objId);
                    string techName = technologyRef?.transName;
                    return TextTranslate.instance.getLanguage(TransKeyConst.guild_mars_help_tech_desc, objLvl, techName);

                    break;
                case EGuildMarsHelpObjType.TEAM_REPAIR:
                    return TextTranslate.instance.getLanguage(TransKeyConst.guild_mars_help_team_repair_desc);
                    break;
               
            }
            return "";
        }

        public string getReduceTimeStr()
        {
            if (isMyHelp)
                return TextTranslate.instance.getLanguage(TransKeyConst.guild_mars_help_reduce_time,TimeUtil.millisecondsToTime_hms(dealedCount * dealSecs * 1000));
            return "";
        }
    }
}