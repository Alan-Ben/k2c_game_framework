package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogSectionPlayerV2BO extends BaseLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_sectionType =0;
    @DataBaseField(type = "int(11)", fieldname = "sectionType", comment = "截面日志类型，枚举ELogSectionType")
    private int sectionType;

    public static final int FIELD_event_id =1;
    @DataBaseField(type = "int(11)", fieldname = "event_id", comment = "事件类型")
    private int event_id;

    public static final int FIELD_guid =2;
    @DataBaseField(type = "bigint(20)", fieldname = "guid", comment = "事件唯一id")
    private long guid;

    public static final int FIELD_date_time =3;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_timestamp =4;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public static final int FIELD_cid =5;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_villageEarning =6;
    @DataBaseField(type = "bigint(20)", fieldname = "villageEarning", comment = "村庄总收益")
    private long villageEarning;

    public static final int FIELD_childEarning =7;
    @DataBaseField(type = "bigint(20)", fieldname = "childEarning", comment = "子嗣总收益")
    private long childEarning;

    public static final int FIELD_chapterStageId =8;
    @DataBaseField(type = "bigint(20)", fieldname = "chapterStageId", comment = "最新停留关卡")
    private long chapterStageId;

    public static final int FIELD_consortNum =9;
    @DataBaseField(type = "int(11)", fieldname = "consortNum", comment = "知己数量")
    private int consortNum;

    public static final int FIELD_heroNum =10;
    @DataBaseField(type = "int(11)", fieldname = "heroNum", comment = "骑士数量")
    private int heroNum;

    public static final int FIELD_level =11;
    @DataBaseField(type = "int(11)", fieldname = "level", comment = "玩家等级")
    private int level;

    public static final int FIELD_totalTalent =12;
    @DataBaseField(type = "bigint(20)", fieldname = "totalTalent", comment = "总资质（大臣）")
    private long totalTalent;

    public static final int FIELD_totalFightPower =13;
    @DataBaseField(type = "bigint(20)", fieldname = "totalFightPower", comment = "总战力")
    private long totalFightPower;

    public static final int FIELD_totalDiamondGain =14;
    @DataBaseField(type = "bigint(20)", fieldname = "totalDiamondGain", comment = "玩家累计获得的钻石")
    private long totalDiamondGain;

    public static final int FIELD_totalGoldGain =15;
    @DataBaseField(type = "bigint(20)", fieldname = "totalGoldGain", comment = "玩家累计获得的金币")
    private long totalGoldGain;

    public static final int FIELD_totalDiamondCost =16;
    @DataBaseField(type = "bigint(20)", fieldname = "totalDiamondCost", comment = "玩家累计消耗的钻石")
    private long totalDiamondCost;

    public static final int FIELD_totalGoldCost =17;
    @DataBaseField(type = "bigint(20)", fieldname = "totalGoldCost", comment = "玩家累计消耗的金币")
    private long totalGoldCost;

    public static final int FIELD_farmGoldGain =18;
    @DataBaseField(type = "bigint(20)", fieldname = "farmGoldGain", comment = "农田累计征收获得的金币")
    private long farmGoldGain;

    public static final int FIELD_totalPlayerExp =19;
    @DataBaseField(type = "bigint(20)", fieldname = "totalPlayerExp", comment = "声望值（累计）")
    private long totalPlayerExp;

    public static final int FIELD_questId =20;
    @DataBaseField(type = "bigint(20)", fieldname = "questId", comment = "当前正在进行的主线任务ID")
    private long questId;

    public static final int FIELD_loginDayCount =21;
    @DataBaseField(type = "int(11)", fieldname = "loginDayCount", comment = "登录天数")
    private int loginDayCount;

    public static final int FIELD_highestEarning =22;
    @DataBaseField(type = "bigint(20)", fieldname = "highestEarning", comment = "历史最高赚速")
    private long highestEarning;

    public static final int FIELD_farmCollect =23;
    @DataBaseField(type = "int(11)", fieldname = "farmCollect", comment = "农田收取金币次数")
    private int farmCollect;

    public static final int FIELD_farmLvl =24;
    @DataBaseField(type = "int(11)", fieldname = "farmLvl", comment = "农田等级")
    private int farmLvl;

    public static final int FIELD_totalHeroLvl =25;
    @DataBaseField(type = "int(11)", fieldname = "totalHeroLvl", comment = "伙伴总等级")
    private int totalHeroLvl;

    public static final int FIELD_totalBuildingLvl =26;
    @DataBaseField(type = "int(11)", fieldname = "totalBuildingLvl", comment = "建筑总等级")
    private int totalBuildingLvl;

    public static final int FIELD_gainEquipTimes =27;
    @DataBaseField(type = "int(11)", fieldname = "gainEquipTimes", comment = "累计获得藏品数量")
    private int gainEquipTimes;

    public static final int FIELD_consortRandCallCount =28;
    @DataBaseField(type = "int(11)", fieldname = "consortRandCallCount", comment = "问候知己次数")
    private int consortRandCallCount;

    public static final int FIELD_consortCallCount =29;
    @DataBaseField(type = "int(11)", fieldname = "consortCallCount", comment = "指定问候知己次数")
    private int consortCallCount;

    public static final int FIELD_childNum =30;
    @DataBaseField(type = "int(11)", fieldname = "childNum", comment = "子嗣总数量")
    private int childNum;

    public static final int FIELD_travelCount =31;
    @DataBaseField(type = "int(11)", fieldname = "travelCount", comment = "游历次数")
    private int travelCount;

    public static final int FIELD_starDinnerCount =32;
    @DataBaseField(type = "int(11)", fieldname = "starDinnerCount", comment = "举办宴会次数")
    private int starDinnerCount;

    public static final int FIELD_joinDinnerCount =33;
    @DataBaseField(type = "int(11)", fieldname = "joinDinnerCount", comment = "参与宴会次数")
    private int joinDinnerCount;

    public static final int FIELD_arenaAttackTimes =34;
    @DataBaseField(type = "int(11)", fieldname = "arenaAttackTimes", comment = "谈判次数")
    private int arenaAttackTimes;

    public static final int FIELD_ArenaStationCollectTimes =35;
    @DataBaseField(type = "int(11)", fieldname = "ArenaStationCollectTimes", comment = "竞技场领取收益次数")
    private int ArenaStationCollectTimes;

    public static final int FIELD_towerPassedChapter =36;
    @DataBaseField(type = "bigint(20)", fieldname = "towerPassedChapter", comment = "迷宫层数")
    private long towerPassedChapter;

    public static final int FIELD_rankLikeCount =37;
    @DataBaseField(type = "int(11)", fieldname = "rankLikeCount", comment = "排行榜点赞次数")
    private int rankLikeCount;

    public static final int FIELD_youngChildNum =38;
    @DataBaseField(type = "int(11)", fieldname = "youngChildNum", comment = "当前未成年子嗣数量")
    private int youngChildNum;

    public static final int FIELD_adultChildNum =39;
    @DataBaseField(type = "int(11)", fieldname = "adultChildNum", comment = "成年子嗣数量")
    private int adultChildNum;

    public static final int FIELD_childMarryCount =40;
    @DataBaseField(type = "int(11)", fieldname = "childMarryCount", comment = "子嗣联姻次数")
    private int childMarryCount;

    public static final int FIELD_marryChildEarning =41;
    @DataBaseField(type = "bigint(20)", fieldname = "marryChildEarning", comment = "已婚子嗣收益(己方子嗣+对方子嗣)")
    private long marryChildEarning;

    public static final int FIELD_unmarriedChildEarning =42;
    @DataBaseField(type = "bigint(20)", fieldname = "unmarriedChildEarning", comment = "未婚子嗣收益")
    private long unmarriedChildEarning;

    public static final int FIELD_childCultureCount =43;
    @DataBaseField(type = "int(11)", fieldname = "childCultureCount", comment = "子嗣培养次数")
    private int childCultureCount;

    public LogSectionPlayerV2BO() {
        id = 0;
        sectionType = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        cid = 0L;
        villageEarning = 0L;
        childEarning = 0L;
        chapterStageId = 0L;
        consortNum = 0;
        heroNum = 0;
        level = 0;
        totalTalent = 0L;
        totalFightPower = 0L;
        totalDiamondGain = 0L;
        totalGoldGain = 0L;
        totalDiamondCost = 0L;
        totalGoldCost = 0L;
        farmGoldGain = 0L;
        totalPlayerExp = 0L;
        questId = 0L;
        loginDayCount = 0;
        highestEarning = 0L;
        farmCollect = 0;
        farmLvl = 0;
        totalHeroLvl = 0;
        totalBuildingLvl = 0;
        gainEquipTimes = 0;
        consortRandCallCount = 0;
        consortCallCount = 0;
        childNum = 0;
        travelCount = 0;
        starDinnerCount = 0;
        joinDinnerCount = 0;
        arenaAttackTimes = 0;
        ArenaStationCollectTimes = 0;
        towerPassedChapter = 0L;
        rankLikeCount = 0;
        youngChildNum = 0;
        adultChildNum = 0;
        childMarryCount = 0;
        marryChildEarning = 0L;
        unmarriedChildEarning = 0L;
        childCultureCount = 0;
    }

    public LogSectionPlayerV2BO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        sectionType = rs.getInt(2);
        event_id = rs.getInt(3);
        guid = rs.getLong(4);
        date_time = rs.getInt(5);
        timestamp = rs.getInt(6);
        cid = rs.getLong(7);
        villageEarning = rs.getLong(8);
        childEarning = rs.getLong(9);
        chapterStageId = rs.getLong(10);
        consortNum = rs.getInt(11);
        heroNum = rs.getInt(12);
        level = rs.getInt(13);
        totalTalent = rs.getLong(14);
        totalFightPower = rs.getLong(15);
        totalDiamondGain = rs.getLong(16);
        totalGoldGain = rs.getLong(17);
        totalDiamondCost = rs.getLong(18);
        totalGoldCost = rs.getLong(19);
        farmGoldGain = rs.getLong(20);
        totalPlayerExp = rs.getLong(21);
        questId = rs.getLong(22);
        loginDayCount = rs.getInt(23);
        highestEarning = rs.getLong(24);
        farmCollect = rs.getInt(25);
        farmLvl = rs.getInt(26);
        totalHeroLvl = rs.getInt(27);
        totalBuildingLvl = rs.getInt(28);
        gainEquipTimes = rs.getInt(29);
        consortRandCallCount = rs.getInt(30);
        consortCallCount = rs.getInt(31);
        childNum = rs.getInt(32);
        travelCount = rs.getInt(33);
        starDinnerCount = rs.getInt(34);
        joinDinnerCount = rs.getInt(35);
        arenaAttackTimes = rs.getInt(36);
        ArenaStationCollectTimes = rs.getInt(37);
        towerPassedChapter = rs.getLong(38);
        rankLikeCount = rs.getInt(39);
        youngChildNum = rs.getInt(40);
        adultChildNum = rs.getInt(41);
        childMarryCount = rs.getInt(42);
        marryChildEarning = rs.getLong(43);
        unmarriedChildEarning = rs.getLong(44);
        childCultureCount = rs.getInt(45);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogSectionPlayerV2BO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `sectionType`, `event_id`, `guid`, `date_time`, `timestamp`, `cid`, `villageEarning`, `childEarning`, `chapterStageId`, `consortNum`, `heroNum`, `level`, `totalTalent`, `totalFightPower`, `totalDiamondGain`, `totalGoldGain`, `totalDiamondCost`, `totalGoldCost`, `farmGoldGain`, `totalPlayerExp`, `questId`, `loginDayCount`, `highestEarning`, `farmCollect`, `farmLvl`, `totalHeroLvl`, `totalBuildingLvl`, `gainEquipTimes`, `consortRandCallCount`, `consortCallCount`, `childNum`, `travelCount`, `starDinnerCount`, `joinDinnerCount`, `arenaAttackTimes`, `ArenaStationCollectTimes`, `towerPassedChapter`, `rankLikeCount`, `youngChildNum`, `adultChildNum`, `childMarryCount`, `marryChildEarning`, `unmarriedChildEarning`, `childCultureCount`";
    }

    @Override
    public String getTableName() {
        return "`log_section_player_v2`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(sectionType).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(villageEarning).append("', ");
        strBuf.append("'").append(childEarning).append("', ");
        strBuf.append("'").append(chapterStageId).append("', ");
        strBuf.append("'").append(consortNum).append("', ");
        strBuf.append("'").append(heroNum).append("', ");
        strBuf.append("'").append(level).append("', ");
        strBuf.append("'").append(totalTalent).append("', ");
        strBuf.append("'").append(totalFightPower).append("', ");
        strBuf.append("'").append(totalDiamondGain).append("', ");
        strBuf.append("'").append(totalGoldGain).append("', ");
        strBuf.append("'").append(totalDiamondCost).append("', ");
        strBuf.append("'").append(totalGoldCost).append("', ");
        strBuf.append("'").append(farmGoldGain).append("', ");
        strBuf.append("'").append(totalPlayerExp).append("', ");
        strBuf.append("'").append(questId).append("', ");
        strBuf.append("'").append(loginDayCount).append("', ");
        strBuf.append("'").append(highestEarning).append("', ");
        strBuf.append("'").append(farmCollect).append("', ");
        strBuf.append("'").append(farmLvl).append("', ");
        strBuf.append("'").append(totalHeroLvl).append("', ");
        strBuf.append("'").append(totalBuildingLvl).append("', ");
        strBuf.append("'").append(gainEquipTimes).append("', ");
        strBuf.append("'").append(consortRandCallCount).append("', ");
        strBuf.append("'").append(consortCallCount).append("', ");
        strBuf.append("'").append(childNum).append("', ");
        strBuf.append("'").append(travelCount).append("', ");
        strBuf.append("'").append(starDinnerCount).append("', ");
        strBuf.append("'").append(joinDinnerCount).append("', ");
        strBuf.append("'").append(arenaAttackTimes).append("', ");
        strBuf.append("'").append(ArenaStationCollectTimes).append("', ");
        strBuf.append("'").append(towerPassedChapter).append("', ");
        strBuf.append("'").append(rankLikeCount).append("', ");
        strBuf.append("'").append(youngChildNum).append("', ");
        strBuf.append("'").append(adultChildNum).append("', ");
        strBuf.append("'").append(childMarryCount).append("', ");
        strBuf.append("'").append(marryChildEarning).append("', ");
        strBuf.append("'").append(unmarriedChildEarning).append("', ");
        strBuf.append("'").append(childCultureCount).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        return ret;
    }

   @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        return ret;
    }
	
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 截面日志类型，枚举ELogSectionType
    public int getSectionType() { return this.sectionType; }
    public void setSectionType(BM _bm, int sectionType) {
        if(sectionType==this.sectionType) 
            return;
        this.sectionType = sectionType; 
        markField(_bm, FIELD_sectionType); 
    }
    public void saveSectionType(BM _bm, int sectionType) {
        if(sectionType==this.sectionType) 
            return;
        this.sectionType = sectionType;
        saveField(_bm, "sectionType", sectionType);
    }

    // 事件类型
    public int getEventId() { return this.event_id; }
    public void setEventId(BM _bm, int event_id) {
        if(event_id==this.event_id) 
            return;
        this.event_id = event_id; 
        markField(_bm, FIELD_event_id); 
    }
    public void saveEventId(BM _bm, int event_id) {
        if(event_id==this.event_id) 
            return;
        this.event_id = event_id;
        saveField(_bm, "event_id", event_id);
    }

    // 事件唯一id
    public long getGuid() { return this.guid; }
    public void setGuid(BM _bm, long guid) {
        if(guid==this.guid) 
            return;
        this.guid = guid; 
        markField(_bm, FIELD_guid); 
    }
    public void saveGuid(BM _bm, long guid) {
        if(guid==this.guid) 
            return;
        this.guid = guid;
        saveField(_bm, "guid", guid);
    }

    // 日期
    public int getDateTime() { return this.date_time; }
    public void setDateTime(BM _bm, int date_time) {
        if(date_time==this.date_time) 
            return;
        this.date_time = date_time; 
        markField(_bm, FIELD_date_time); 
    }
    public void saveDateTime(BM _bm, int date_time) {
        if(date_time==this.date_time) 
            return;
        this.date_time = date_time;
        saveField(_bm, "date_time", date_time);
    }

    // 时间戳
    public int getTimestamp() { return this.timestamp; }
    public void setTimestamp(BM _bm, int timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp; 
        markField(_bm, FIELD_timestamp); 
    }
    public void saveTimestamp(BM _bm, int timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp;
        saveField(_bm, "timestamp", timestamp);
    }

    // 玩家CID
    public long getCid() { return this.cid; }
    public void setCid(BM _bm, long cid) {
        if(cid==this.cid) 
            return;
        this.cid = cid; 
        markField(_bm, FIELD_cid); 
    }
    public void saveCid(BM _bm, long cid) {
        if(cid==this.cid) 
            return;
        this.cid = cid;
        saveField(_bm, "cid", cid);
    }

    // 村庄总收益
    public long getVillageEarning() { return this.villageEarning; }
    public void setVillageEarning(BM _bm, long villageEarning) {
        if(villageEarning==this.villageEarning) 
            return;
        this.villageEarning = villageEarning; 
        markField(_bm, FIELD_villageEarning); 
    }
    public void saveVillageEarning(BM _bm, long villageEarning) {
        if(villageEarning==this.villageEarning) 
            return;
        this.villageEarning = villageEarning;
        saveField(_bm, "villageEarning", villageEarning);
    }

    // 子嗣总收益
    public long getChildEarning() { return this.childEarning; }
    public void setChildEarning(BM _bm, long childEarning) {
        if(childEarning==this.childEarning) 
            return;
        this.childEarning = childEarning; 
        markField(_bm, FIELD_childEarning); 
    }
    public void saveChildEarning(BM _bm, long childEarning) {
        if(childEarning==this.childEarning) 
            return;
        this.childEarning = childEarning;
        saveField(_bm, "childEarning", childEarning);
    }

    // 最新停留关卡
    public long getChapterStageId() { return this.chapterStageId; }
    public void setChapterStageId(BM _bm, long chapterStageId) {
        if(chapterStageId==this.chapterStageId) 
            return;
        this.chapterStageId = chapterStageId; 
        markField(_bm, FIELD_chapterStageId); 
    }
    public void saveChapterStageId(BM _bm, long chapterStageId) {
        if(chapterStageId==this.chapterStageId) 
            return;
        this.chapterStageId = chapterStageId;
        saveField(_bm, "chapterStageId", chapterStageId);
    }

    // 知己数量
    public int getConsortNum() { return this.consortNum; }
    public void setConsortNum(BM _bm, int consortNum) {
        if(consortNum==this.consortNum) 
            return;
        this.consortNum = consortNum; 
        markField(_bm, FIELD_consortNum); 
    }
    public void saveConsortNum(BM _bm, int consortNum) {
        if(consortNum==this.consortNum) 
            return;
        this.consortNum = consortNum;
        saveField(_bm, "consortNum", consortNum);
    }

    // 骑士数量
    public int getHeroNum() { return this.heroNum; }
    public void setHeroNum(BM _bm, int heroNum) {
        if(heroNum==this.heroNum) 
            return;
        this.heroNum = heroNum; 
        markField(_bm, FIELD_heroNum); 
    }
    public void saveHeroNum(BM _bm, int heroNum) {
        if(heroNum==this.heroNum) 
            return;
        this.heroNum = heroNum;
        saveField(_bm, "heroNum", heroNum);
    }

    // 玩家等级
    public int getLevel() { return this.level; }
    public void setLevel(BM _bm, int level) {
        if(level==this.level) 
            return;
        this.level = level; 
        markField(_bm, FIELD_level); 
    }
    public void saveLevel(BM _bm, int level) {
        if(level==this.level) 
            return;
        this.level = level;
        saveField(_bm, "level", level);
    }

    // 总资质（大臣）
    public long getTotalTalent() { return this.totalTalent; }
    public void setTotalTalent(BM _bm, long totalTalent) {
        if(totalTalent==this.totalTalent) 
            return;
        this.totalTalent = totalTalent; 
        markField(_bm, FIELD_totalTalent); 
    }
    public void saveTotalTalent(BM _bm, long totalTalent) {
        if(totalTalent==this.totalTalent) 
            return;
        this.totalTalent = totalTalent;
        saveField(_bm, "totalTalent", totalTalent);
    }

    // 总战力
    public long getTotalFightPower() { return this.totalFightPower; }
    public void setTotalFightPower(BM _bm, long totalFightPower) {
        if(totalFightPower==this.totalFightPower) 
            return;
        this.totalFightPower = totalFightPower; 
        markField(_bm, FIELD_totalFightPower); 
    }
    public void saveTotalFightPower(BM _bm, long totalFightPower) {
        if(totalFightPower==this.totalFightPower) 
            return;
        this.totalFightPower = totalFightPower;
        saveField(_bm, "totalFightPower", totalFightPower);
    }

    // 玩家累计获得的钻石
    public long getTotalDiamondGain() { return this.totalDiamondGain; }
    public void setTotalDiamondGain(BM _bm, long totalDiamondGain) {
        if(totalDiamondGain==this.totalDiamondGain) 
            return;
        this.totalDiamondGain = totalDiamondGain; 
        markField(_bm, FIELD_totalDiamondGain); 
    }
    public void saveTotalDiamondGain(BM _bm, long totalDiamondGain) {
        if(totalDiamondGain==this.totalDiamondGain) 
            return;
        this.totalDiamondGain = totalDiamondGain;
        saveField(_bm, "totalDiamondGain", totalDiamondGain);
    }

    // 玩家累计获得的金币
    public long getTotalGoldGain() { return this.totalGoldGain; }
    public void setTotalGoldGain(BM _bm, long totalGoldGain) {
        if(totalGoldGain==this.totalGoldGain) 
            return;
        this.totalGoldGain = totalGoldGain; 
        markField(_bm, FIELD_totalGoldGain); 
    }
    public void saveTotalGoldGain(BM _bm, long totalGoldGain) {
        if(totalGoldGain==this.totalGoldGain) 
            return;
        this.totalGoldGain = totalGoldGain;
        saveField(_bm, "totalGoldGain", totalGoldGain);
    }

    // 玩家累计消耗的钻石
    public long getTotalDiamondCost() { return this.totalDiamondCost; }
    public void setTotalDiamondCost(BM _bm, long totalDiamondCost) {
        if(totalDiamondCost==this.totalDiamondCost) 
            return;
        this.totalDiamondCost = totalDiamondCost; 
        markField(_bm, FIELD_totalDiamondCost); 
    }
    public void saveTotalDiamondCost(BM _bm, long totalDiamondCost) {
        if(totalDiamondCost==this.totalDiamondCost) 
            return;
        this.totalDiamondCost = totalDiamondCost;
        saveField(_bm, "totalDiamondCost", totalDiamondCost);
    }

    // 玩家累计消耗的金币
    public long getTotalGoldCost() { return this.totalGoldCost; }
    public void setTotalGoldCost(BM _bm, long totalGoldCost) {
        if(totalGoldCost==this.totalGoldCost) 
            return;
        this.totalGoldCost = totalGoldCost; 
        markField(_bm, FIELD_totalGoldCost); 
    }
    public void saveTotalGoldCost(BM _bm, long totalGoldCost) {
        if(totalGoldCost==this.totalGoldCost) 
            return;
        this.totalGoldCost = totalGoldCost;
        saveField(_bm, "totalGoldCost", totalGoldCost);
    }

    // 农田累计征收获得的金币
    public long getFarmGoldGain() { return this.farmGoldGain; }
    public void setFarmGoldGain(BM _bm, long farmGoldGain) {
        if(farmGoldGain==this.farmGoldGain) 
            return;
        this.farmGoldGain = farmGoldGain; 
        markField(_bm, FIELD_farmGoldGain); 
    }
    public void saveFarmGoldGain(BM _bm, long farmGoldGain) {
        if(farmGoldGain==this.farmGoldGain) 
            return;
        this.farmGoldGain = farmGoldGain;
        saveField(_bm, "farmGoldGain", farmGoldGain);
    }

    // 声望值（累计）
    public long getTotalPlayerExp() { return this.totalPlayerExp; }
    public void setTotalPlayerExp(BM _bm, long totalPlayerExp) {
        if(totalPlayerExp==this.totalPlayerExp) 
            return;
        this.totalPlayerExp = totalPlayerExp; 
        markField(_bm, FIELD_totalPlayerExp); 
    }
    public void saveTotalPlayerExp(BM _bm, long totalPlayerExp) {
        if(totalPlayerExp==this.totalPlayerExp) 
            return;
        this.totalPlayerExp = totalPlayerExp;
        saveField(_bm, "totalPlayerExp", totalPlayerExp);
    }

    // 当前正在进行的主线任务ID
    public long getQuestId() { return this.questId; }
    public void setQuestId(BM _bm, long questId) {
        if(questId==this.questId) 
            return;
        this.questId = questId; 
        markField(_bm, FIELD_questId); 
    }
    public void saveQuestId(BM _bm, long questId) {
        if(questId==this.questId) 
            return;
        this.questId = questId;
        saveField(_bm, "questId", questId);
    }

    // 登录天数
    public int getLoginDayCount() { return this.loginDayCount; }
    public void setLoginDayCount(BM _bm, int loginDayCount) {
        if(loginDayCount==this.loginDayCount) 
            return;
        this.loginDayCount = loginDayCount; 
        markField(_bm, FIELD_loginDayCount); 
    }
    public void saveLoginDayCount(BM _bm, int loginDayCount) {
        if(loginDayCount==this.loginDayCount) 
            return;
        this.loginDayCount = loginDayCount;
        saveField(_bm, "loginDayCount", loginDayCount);
    }

    // 历史最高赚速
    public long getHighestEarning() { return this.highestEarning; }
    public void setHighestEarning(BM _bm, long highestEarning) {
        if(highestEarning==this.highestEarning) 
            return;
        this.highestEarning = highestEarning; 
        markField(_bm, FIELD_highestEarning); 
    }
    public void saveHighestEarning(BM _bm, long highestEarning) {
        if(highestEarning==this.highestEarning) 
            return;
        this.highestEarning = highestEarning;
        saveField(_bm, "highestEarning", highestEarning);
    }

    // 农田收取金币次数
    public int getFarmCollect() { return this.farmCollect; }
    public void setFarmCollect(BM _bm, int farmCollect) {
        if(farmCollect==this.farmCollect) 
            return;
        this.farmCollect = farmCollect; 
        markField(_bm, FIELD_farmCollect); 
    }
    public void saveFarmCollect(BM _bm, int farmCollect) {
        if(farmCollect==this.farmCollect) 
            return;
        this.farmCollect = farmCollect;
        saveField(_bm, "farmCollect", farmCollect);
    }

    // 农田等级
    public int getFarmLvl() { return this.farmLvl; }
    public void setFarmLvl(BM _bm, int farmLvl) {
        if(farmLvl==this.farmLvl) 
            return;
        this.farmLvl = farmLvl; 
        markField(_bm, FIELD_farmLvl); 
    }
    public void saveFarmLvl(BM _bm, int farmLvl) {
        if(farmLvl==this.farmLvl) 
            return;
        this.farmLvl = farmLvl;
        saveField(_bm, "farmLvl", farmLvl);
    }

    // 伙伴总等级
    public int getTotalHeroLvl() { return this.totalHeroLvl; }
    public void setTotalHeroLvl(BM _bm, int totalHeroLvl) {
        if(totalHeroLvl==this.totalHeroLvl) 
            return;
        this.totalHeroLvl = totalHeroLvl; 
        markField(_bm, FIELD_totalHeroLvl); 
    }
    public void saveTotalHeroLvl(BM _bm, int totalHeroLvl) {
        if(totalHeroLvl==this.totalHeroLvl) 
            return;
        this.totalHeroLvl = totalHeroLvl;
        saveField(_bm, "totalHeroLvl", totalHeroLvl);
    }

    // 建筑总等级
    public int getTotalBuildingLvl() { return this.totalBuildingLvl; }
    public void setTotalBuildingLvl(BM _bm, int totalBuildingLvl) {
        if(totalBuildingLvl==this.totalBuildingLvl) 
            return;
        this.totalBuildingLvl = totalBuildingLvl; 
        markField(_bm, FIELD_totalBuildingLvl); 
    }
    public void saveTotalBuildingLvl(BM _bm, int totalBuildingLvl) {
        if(totalBuildingLvl==this.totalBuildingLvl) 
            return;
        this.totalBuildingLvl = totalBuildingLvl;
        saveField(_bm, "totalBuildingLvl", totalBuildingLvl);
    }

    // 累计获得藏品数量
    public int getGainEquipTimes() { return this.gainEquipTimes; }
    public void setGainEquipTimes(BM _bm, int gainEquipTimes) {
        if(gainEquipTimes==this.gainEquipTimes) 
            return;
        this.gainEquipTimes = gainEquipTimes; 
        markField(_bm, FIELD_gainEquipTimes); 
    }
    public void saveGainEquipTimes(BM _bm, int gainEquipTimes) {
        if(gainEquipTimes==this.gainEquipTimes) 
            return;
        this.gainEquipTimes = gainEquipTimes;
        saveField(_bm, "gainEquipTimes", gainEquipTimes);
    }

    // 问候知己次数
    public int getConsortRandCallCount() { return this.consortRandCallCount; }
    public void setConsortRandCallCount(BM _bm, int consortRandCallCount) {
        if(consortRandCallCount==this.consortRandCallCount) 
            return;
        this.consortRandCallCount = consortRandCallCount; 
        markField(_bm, FIELD_consortRandCallCount); 
    }
    public void saveConsortRandCallCount(BM _bm, int consortRandCallCount) {
        if(consortRandCallCount==this.consortRandCallCount) 
            return;
        this.consortRandCallCount = consortRandCallCount;
        saveField(_bm, "consortRandCallCount", consortRandCallCount);
    }

    // 指定问候知己次数
    public int getConsortCallCount() { return this.consortCallCount; }
    public void setConsortCallCount(BM _bm, int consortCallCount) {
        if(consortCallCount==this.consortCallCount) 
            return;
        this.consortCallCount = consortCallCount; 
        markField(_bm, FIELD_consortCallCount); 
    }
    public void saveConsortCallCount(BM _bm, int consortCallCount) {
        if(consortCallCount==this.consortCallCount) 
            return;
        this.consortCallCount = consortCallCount;
        saveField(_bm, "consortCallCount", consortCallCount);
    }

    // 子嗣总数量
    public int getChildNum() { return this.childNum; }
    public void setChildNum(BM _bm, int childNum) {
        if(childNum==this.childNum) 
            return;
        this.childNum = childNum; 
        markField(_bm, FIELD_childNum); 
    }
    public void saveChildNum(BM _bm, int childNum) {
        if(childNum==this.childNum) 
            return;
        this.childNum = childNum;
        saveField(_bm, "childNum", childNum);
    }

    // 游历次数
    public int getTravelCount() { return this.travelCount; }
    public void setTravelCount(BM _bm, int travelCount) {
        if(travelCount==this.travelCount) 
            return;
        this.travelCount = travelCount; 
        markField(_bm, FIELD_travelCount); 
    }
    public void saveTravelCount(BM _bm, int travelCount) {
        if(travelCount==this.travelCount) 
            return;
        this.travelCount = travelCount;
        saveField(_bm, "travelCount", travelCount);
    }

    // 举办宴会次数
    public int getStarDinnerCount() { return this.starDinnerCount; }
    public void setStarDinnerCount(BM _bm, int starDinnerCount) {
        if(starDinnerCount==this.starDinnerCount) 
            return;
        this.starDinnerCount = starDinnerCount; 
        markField(_bm, FIELD_starDinnerCount); 
    }
    public void saveStarDinnerCount(BM _bm, int starDinnerCount) {
        if(starDinnerCount==this.starDinnerCount) 
            return;
        this.starDinnerCount = starDinnerCount;
        saveField(_bm, "starDinnerCount", starDinnerCount);
    }

    // 参与宴会次数
    public int getJoinDinnerCount() { return this.joinDinnerCount; }
    public void setJoinDinnerCount(BM _bm, int joinDinnerCount) {
        if(joinDinnerCount==this.joinDinnerCount) 
            return;
        this.joinDinnerCount = joinDinnerCount; 
        markField(_bm, FIELD_joinDinnerCount); 
    }
    public void saveJoinDinnerCount(BM _bm, int joinDinnerCount) {
        if(joinDinnerCount==this.joinDinnerCount) 
            return;
        this.joinDinnerCount = joinDinnerCount;
        saveField(_bm, "joinDinnerCount", joinDinnerCount);
    }

    // 谈判次数
    public int getArenaAttackTimes() { return this.arenaAttackTimes; }
    public void setArenaAttackTimes(BM _bm, int arenaAttackTimes) {
        if(arenaAttackTimes==this.arenaAttackTimes) 
            return;
        this.arenaAttackTimes = arenaAttackTimes; 
        markField(_bm, FIELD_arenaAttackTimes); 
    }
    public void saveArenaAttackTimes(BM _bm, int arenaAttackTimes) {
        if(arenaAttackTimes==this.arenaAttackTimes) 
            return;
        this.arenaAttackTimes = arenaAttackTimes;
        saveField(_bm, "arenaAttackTimes", arenaAttackTimes);
    }

    // 竞技场领取收益次数
    public int getArenaStationCollectTimes() { return this.ArenaStationCollectTimes; }
    public void setArenaStationCollectTimes(BM _bm, int ArenaStationCollectTimes) {
        if(ArenaStationCollectTimes==this.ArenaStationCollectTimes) 
            return;
        this.ArenaStationCollectTimes = ArenaStationCollectTimes; 
        markField(_bm, FIELD_ArenaStationCollectTimes); 
    }
    public void saveArenaStationCollectTimes(BM _bm, int ArenaStationCollectTimes) {
        if(ArenaStationCollectTimes==this.ArenaStationCollectTimes) 
            return;
        this.ArenaStationCollectTimes = ArenaStationCollectTimes;
        saveField(_bm, "ArenaStationCollectTimes", ArenaStationCollectTimes);
    }

    // 迷宫层数
    public long getTowerPassedChapter() { return this.towerPassedChapter; }
    public void setTowerPassedChapter(BM _bm, long towerPassedChapter) {
        if(towerPassedChapter==this.towerPassedChapter) 
            return;
        this.towerPassedChapter = towerPassedChapter; 
        markField(_bm, FIELD_towerPassedChapter); 
    }
    public void saveTowerPassedChapter(BM _bm, long towerPassedChapter) {
        if(towerPassedChapter==this.towerPassedChapter) 
            return;
        this.towerPassedChapter = towerPassedChapter;
        saveField(_bm, "towerPassedChapter", towerPassedChapter);
    }

    // 排行榜点赞次数
    public int getRankLikeCount() { return this.rankLikeCount; }
    public void setRankLikeCount(BM _bm, int rankLikeCount) {
        if(rankLikeCount==this.rankLikeCount) 
            return;
        this.rankLikeCount = rankLikeCount; 
        markField(_bm, FIELD_rankLikeCount); 
    }
    public void saveRankLikeCount(BM _bm, int rankLikeCount) {
        if(rankLikeCount==this.rankLikeCount) 
            return;
        this.rankLikeCount = rankLikeCount;
        saveField(_bm, "rankLikeCount", rankLikeCount);
    }

    // 当前未成年子嗣数量
    public int getYoungChildNum() { return this.youngChildNum; }
    public void setYoungChildNum(BM _bm, int youngChildNum) {
        if(youngChildNum==this.youngChildNum) 
            return;
        this.youngChildNum = youngChildNum; 
        markField(_bm, FIELD_youngChildNum); 
    }
    public void saveYoungChildNum(BM _bm, int youngChildNum) {
        if(youngChildNum==this.youngChildNum) 
            return;
        this.youngChildNum = youngChildNum;
        saveField(_bm, "youngChildNum", youngChildNum);
    }

    // 成年子嗣数量
    public int getAdultChildNum() { return this.adultChildNum; }
    public void setAdultChildNum(BM _bm, int adultChildNum) {
        if(adultChildNum==this.adultChildNum) 
            return;
        this.adultChildNum = adultChildNum; 
        markField(_bm, FIELD_adultChildNum); 
    }
    public void saveAdultChildNum(BM _bm, int adultChildNum) {
        if(adultChildNum==this.adultChildNum) 
            return;
        this.adultChildNum = adultChildNum;
        saveField(_bm, "adultChildNum", adultChildNum);
    }

    // 子嗣联姻次数
    public int getChildMarryCount() { return this.childMarryCount; }
    public void setChildMarryCount(BM _bm, int childMarryCount) {
        if(childMarryCount==this.childMarryCount) 
            return;
        this.childMarryCount = childMarryCount; 
        markField(_bm, FIELD_childMarryCount); 
    }
    public void saveChildMarryCount(BM _bm, int childMarryCount) {
        if(childMarryCount==this.childMarryCount) 
            return;
        this.childMarryCount = childMarryCount;
        saveField(_bm, "childMarryCount", childMarryCount);
    }

    // 已婚子嗣收益(己方子嗣+对方子嗣)
    public long getMarryChildEarning() { return this.marryChildEarning; }
    public void setMarryChildEarning(BM _bm, long marryChildEarning) {
        if(marryChildEarning==this.marryChildEarning) 
            return;
        this.marryChildEarning = marryChildEarning; 
        markField(_bm, FIELD_marryChildEarning); 
    }
    public void saveMarryChildEarning(BM _bm, long marryChildEarning) {
        if(marryChildEarning==this.marryChildEarning) 
            return;
        this.marryChildEarning = marryChildEarning;
        saveField(_bm, "marryChildEarning", marryChildEarning);
    }

    // 未婚子嗣收益
    public long getUnmarriedChildEarning() { return this.unmarriedChildEarning; }
    public void setUnmarriedChildEarning(BM _bm, long unmarriedChildEarning) {
        if(unmarriedChildEarning==this.unmarriedChildEarning) 
            return;
        this.unmarriedChildEarning = unmarriedChildEarning; 
        markField(_bm, FIELD_unmarriedChildEarning); 
    }
    public void saveUnmarriedChildEarning(BM _bm, long unmarriedChildEarning) {
        if(unmarriedChildEarning==this.unmarriedChildEarning) 
            return;
        this.unmarriedChildEarning = unmarriedChildEarning;
        saveField(_bm, "unmarriedChildEarning", unmarriedChildEarning);
    }

    // 子嗣培养次数
    public int getChildCultureCount() { return this.childCultureCount; }
    public void setChildCultureCount(BM _bm, int childCultureCount) {
        if(childCultureCount==this.childCultureCount) 
            return;
        this.childCultureCount = childCultureCount; 
        markField(_bm, FIELD_childCultureCount); 
    }
    public void saveChildCultureCount(BM _bm, int childCultureCount) {
        if(childCultureCount==this.childCultureCount) 
            return;
        this.childCultureCount = childCultureCount;
        saveField(_bm, "childCultureCount", childCultureCount);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `sectionType` = '").append(sectionType).append("',");
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `villageEarning` = '").append(villageEarning).append("',");
        sBuilder.append(" `childEarning` = '").append(childEarning).append("',");
        sBuilder.append(" `chapterStageId` = '").append(chapterStageId).append("',");
        sBuilder.append(" `consortNum` = '").append(consortNum).append("',");
        sBuilder.append(" `heroNum` = '").append(heroNum).append("',");
        sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.append(" `totalTalent` = '").append(totalTalent).append("',");
        sBuilder.append(" `totalFightPower` = '").append(totalFightPower).append("',");
        sBuilder.append(" `totalDiamondGain` = '").append(totalDiamondGain).append("',");
        sBuilder.append(" `totalGoldGain` = '").append(totalGoldGain).append("',");
        sBuilder.append(" `totalDiamondCost` = '").append(totalDiamondCost).append("',");
        sBuilder.append(" `totalGoldCost` = '").append(totalGoldCost).append("',");
        sBuilder.append(" `farmGoldGain` = '").append(farmGoldGain).append("',");
        sBuilder.append(" `totalPlayerExp` = '").append(totalPlayerExp).append("',");
        sBuilder.append(" `questId` = '").append(questId).append("',");
        sBuilder.append(" `loginDayCount` = '").append(loginDayCount).append("',");
        sBuilder.append(" `highestEarning` = '").append(highestEarning).append("',");
        sBuilder.append(" `farmCollect` = '").append(farmCollect).append("',");
        sBuilder.append(" `farmLvl` = '").append(farmLvl).append("',");
        sBuilder.append(" `totalHeroLvl` = '").append(totalHeroLvl).append("',");
        sBuilder.append(" `totalBuildingLvl` = '").append(totalBuildingLvl).append("',");
        sBuilder.append(" `gainEquipTimes` = '").append(gainEquipTimes).append("',");
        sBuilder.append(" `consortRandCallCount` = '").append(consortRandCallCount).append("',");
        sBuilder.append(" `consortCallCount` = '").append(consortCallCount).append("',");
        sBuilder.append(" `childNum` = '").append(childNum).append("',");
        sBuilder.append(" `travelCount` = '").append(travelCount).append("',");
        sBuilder.append(" `starDinnerCount` = '").append(starDinnerCount).append("',");
        sBuilder.append(" `joinDinnerCount` = '").append(joinDinnerCount).append("',");
        sBuilder.append(" `arenaAttackTimes` = '").append(arenaAttackTimes).append("',");
        sBuilder.append(" `ArenaStationCollectTimes` = '").append(ArenaStationCollectTimes).append("',");
        sBuilder.append(" `towerPassedChapter` = '").append(towerPassedChapter).append("',");
        sBuilder.append(" `rankLikeCount` = '").append(rankLikeCount).append("',");
        sBuilder.append(" `youngChildNum` = '").append(youngChildNum).append("',");
        sBuilder.append(" `adultChildNum` = '").append(adultChildNum).append("',");
        sBuilder.append(" `childMarryCount` = '").append(childMarryCount).append("',");
        sBuilder.append(" `marryChildEarning` = '").append(marryChildEarning).append("',");
        sBuilder.append(" `unmarriedChildEarning` = '").append(unmarriedChildEarning).append("',");
        sBuilder.append(" `childCultureCount` = '").append(childCultureCount).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_sectionType)) sBuilder.append(" `sectionType` = '").append(sectionType).append("',");
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_guid)) sBuilder.append(" `guid` = '").append(guid).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_villageEarning)) sBuilder.append(" `villageEarning` = '").append(villageEarning).append("',");
        if(isFieldMarked(FIELD_childEarning)) sBuilder.append(" `childEarning` = '").append(childEarning).append("',");
        if(isFieldMarked(FIELD_chapterStageId)) sBuilder.append(" `chapterStageId` = '").append(chapterStageId).append("',");
        if(isFieldMarked(FIELD_consortNum)) sBuilder.append(" `consortNum` = '").append(consortNum).append("',");
        if(isFieldMarked(FIELD_heroNum)) sBuilder.append(" `heroNum` = '").append(heroNum).append("',");
        if(isFieldMarked(FIELD_level)) sBuilder.append(" `level` = '").append(level).append("',");
        if(isFieldMarked(FIELD_totalTalent)) sBuilder.append(" `totalTalent` = '").append(totalTalent).append("',");
        if(isFieldMarked(FIELD_totalFightPower)) sBuilder.append(" `totalFightPower` = '").append(totalFightPower).append("',");
        if(isFieldMarked(FIELD_totalDiamondGain)) sBuilder.append(" `totalDiamondGain` = '").append(totalDiamondGain).append("',");
        if(isFieldMarked(FIELD_totalGoldGain)) sBuilder.append(" `totalGoldGain` = '").append(totalGoldGain).append("',");
        if(isFieldMarked(FIELD_totalDiamondCost)) sBuilder.append(" `totalDiamondCost` = '").append(totalDiamondCost).append("',");
        if(isFieldMarked(FIELD_totalGoldCost)) sBuilder.append(" `totalGoldCost` = '").append(totalGoldCost).append("',");
        if(isFieldMarked(FIELD_farmGoldGain)) sBuilder.append(" `farmGoldGain` = '").append(farmGoldGain).append("',");
        if(isFieldMarked(FIELD_totalPlayerExp)) sBuilder.append(" `totalPlayerExp` = '").append(totalPlayerExp).append("',");
        if(isFieldMarked(FIELD_questId)) sBuilder.append(" `questId` = '").append(questId).append("',");
        if(isFieldMarked(FIELD_loginDayCount)) sBuilder.append(" `loginDayCount` = '").append(loginDayCount).append("',");
        if(isFieldMarked(FIELD_highestEarning)) sBuilder.append(" `highestEarning` = '").append(highestEarning).append("',");
        if(isFieldMarked(FIELD_farmCollect)) sBuilder.append(" `farmCollect` = '").append(farmCollect).append("',");
        if(isFieldMarked(FIELD_farmLvl)) sBuilder.append(" `farmLvl` = '").append(farmLvl).append("',");
        if(isFieldMarked(FIELD_totalHeroLvl)) sBuilder.append(" `totalHeroLvl` = '").append(totalHeroLvl).append("',");
        if(isFieldMarked(FIELD_totalBuildingLvl)) sBuilder.append(" `totalBuildingLvl` = '").append(totalBuildingLvl).append("',");
        if(isFieldMarked(FIELD_gainEquipTimes)) sBuilder.append(" `gainEquipTimes` = '").append(gainEquipTimes).append("',");
        if(isFieldMarked(FIELD_consortRandCallCount)) sBuilder.append(" `consortRandCallCount` = '").append(consortRandCallCount).append("',");
        if(isFieldMarked(FIELD_consortCallCount)) sBuilder.append(" `consortCallCount` = '").append(consortCallCount).append("',");
        if(isFieldMarked(FIELD_childNum)) sBuilder.append(" `childNum` = '").append(childNum).append("',");
        if(isFieldMarked(FIELD_travelCount)) sBuilder.append(" `travelCount` = '").append(travelCount).append("',");
        if(isFieldMarked(FIELD_starDinnerCount)) sBuilder.append(" `starDinnerCount` = '").append(starDinnerCount).append("',");
        if(isFieldMarked(FIELD_joinDinnerCount)) sBuilder.append(" `joinDinnerCount` = '").append(joinDinnerCount).append("',");
        if(isFieldMarked(FIELD_arenaAttackTimes)) sBuilder.append(" `arenaAttackTimes` = '").append(arenaAttackTimes).append("',");
        if(isFieldMarked(FIELD_ArenaStationCollectTimes)) sBuilder.append(" `ArenaStationCollectTimes` = '").append(ArenaStationCollectTimes).append("',");
        if(isFieldMarked(FIELD_towerPassedChapter)) sBuilder.append(" `towerPassedChapter` = '").append(towerPassedChapter).append("',");
        if(isFieldMarked(FIELD_rankLikeCount)) sBuilder.append(" `rankLikeCount` = '").append(rankLikeCount).append("',");
        if(isFieldMarked(FIELD_youngChildNum)) sBuilder.append(" `youngChildNum` = '").append(youngChildNum).append("',");
        if(isFieldMarked(FIELD_adultChildNum)) sBuilder.append(" `adultChildNum` = '").append(adultChildNum).append("',");
        if(isFieldMarked(FIELD_childMarryCount)) sBuilder.append(" `childMarryCount` = '").append(childMarryCount).append("',");
        if(isFieldMarked(FIELD_marryChildEarning)) sBuilder.append(" `marryChildEarning` = '").append(marryChildEarning).append("',");
        if(isFieldMarked(FIELD_unmarriedChildEarning)) sBuilder.append(" `unmarriedChildEarning` = '").append(unmarriedChildEarning).append("',");
        if(isFieldMarked(FIELD_childCultureCount)) sBuilder.append(" `childCultureCount` = '").append(childCultureCount).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_section_player_v2` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`sectionType` int(11) NOT NULL DEFAULT '0' COMMENT '截面日志类型，枚举ELogSectionType',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`villageEarning` bigint(20) NOT NULL DEFAULT '0' COMMENT '村庄总收益',"
                + "`childEarning` bigint(20) NOT NULL DEFAULT '0' COMMENT '子嗣总收益',"
                + "`chapterStageId` bigint(20) NOT NULL DEFAULT '0' COMMENT '最新停留关卡',"
                + "`consortNum` int(11) NOT NULL DEFAULT '0' COMMENT '知己数量',"
                + "`heroNum` int(11) NOT NULL DEFAULT '0' COMMENT '骑士数量',"
                + "`level` int(11) NOT NULL DEFAULT '0' COMMENT '玩家等级',"
                + "`totalTalent` bigint(20) NOT NULL DEFAULT '0' COMMENT '总资质（大臣）',"
                + "`totalFightPower` bigint(20) NOT NULL DEFAULT '0' COMMENT '总战力',"
                + "`totalDiamondGain` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家累计获得的钻石',"
                + "`totalGoldGain` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家累计获得的金币',"
                + "`totalDiamondCost` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家累计消耗的钻石',"
                + "`totalGoldCost` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家累计消耗的金币',"
                + "`farmGoldGain` bigint(20) NOT NULL DEFAULT '0' COMMENT '农田累计征收获得的金币',"
                + "`totalPlayerExp` bigint(20) NOT NULL DEFAULT '0' COMMENT '声望值（累计）',"
                + "`questId` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前正在进行的主线任务ID',"
                + "`loginDayCount` int(11) NOT NULL DEFAULT '0' COMMENT '登录天数',"
                + "`highestEarning` bigint(20) NOT NULL DEFAULT '0' COMMENT '历史最高赚速',"
                + "`farmCollect` int(11) NOT NULL DEFAULT '0' COMMENT '农田收取金币次数',"
                + "`farmLvl` int(11) NOT NULL DEFAULT '0' COMMENT '农田等级',"
                + "`totalHeroLvl` int(11) NOT NULL DEFAULT '0' COMMENT '伙伴总等级',"
                + "`totalBuildingLvl` int(11) NOT NULL DEFAULT '0' COMMENT '建筑总等级',"
                + "`gainEquipTimes` int(11) NOT NULL DEFAULT '0' COMMENT '累计获得藏品数量',"
                + "`consortRandCallCount` int(11) NOT NULL DEFAULT '0' COMMENT '问候知己次数',"
                + "`consortCallCount` int(11) NOT NULL DEFAULT '0' COMMENT '指定问候知己次数',"
                + "`childNum` int(11) NOT NULL DEFAULT '0' COMMENT '子嗣总数量',"
                + "`travelCount` int(11) NOT NULL DEFAULT '0' COMMENT '游历次数',"
                + "`starDinnerCount` int(11) NOT NULL DEFAULT '0' COMMENT '举办宴会次数',"
                + "`joinDinnerCount` int(11) NOT NULL DEFAULT '0' COMMENT '参与宴会次数',"
                + "`arenaAttackTimes` int(11) NOT NULL DEFAULT '0' COMMENT '谈判次数',"
                + "`ArenaStationCollectTimes` int(11) NOT NULL DEFAULT '0' COMMENT '竞技场领取收益次数',"
                + "`towerPassedChapter` bigint(20) NOT NULL DEFAULT '0' COMMENT '迷宫层数',"
                + "`rankLikeCount` int(11) NOT NULL DEFAULT '0' COMMENT '排行榜点赞次数',"
                + "`youngChildNum` int(11) NOT NULL DEFAULT '0' COMMENT '当前未成年子嗣数量',"
                + "`adultChildNum` int(11) NOT NULL DEFAULT '0' COMMENT '成年子嗣数量',"
                + "`childMarryCount` int(11) NOT NULL DEFAULT '0' COMMENT '子嗣联姻次数',"
                + "`marryChildEarning` bigint(20) NOT NULL DEFAULT '0' COMMENT '已婚子嗣收益(己方子嗣+对方子嗣)',"
                + "`unmarriedChildEarning` bigint(20) NOT NULL DEFAULT '0' COMMENT '未婚子嗣收益',"
                + "`childCultureCount` int(11) NOT NULL DEFAULT '0' COMMENT '子嗣培养次数',"
                + "KEY `sectionType` (`sectionType`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家截面数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.us_log;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=4;//sectionType
        _size+=4;//event_id
        _size+=8;//guid
        _size+=4;//date_time
        _size+=4;//timestamp
        _size+=8;//cid
        _size+=8;//villageEarning
        _size+=8;//childEarning
        _size+=8;//chapterStageId
        _size+=4;//consortNum
        _size+=4;//heroNum
        _size+=4;//level
        _size+=8;//totalTalent
        _size+=8;//totalFightPower
        _size+=8;//totalDiamondGain
        _size+=8;//totalGoldGain
        _size+=8;//totalDiamondCost
        _size+=8;//totalGoldCost
        _size+=8;//farmGoldGain
        _size+=8;//totalPlayerExp
        _size+=8;//questId
        _size+=4;//loginDayCount
        _size+=8;//highestEarning
        _size+=4;//farmCollect
        _size+=4;//farmLvl
        _size+=4;//totalHeroLvl
        _size+=4;//totalBuildingLvl
        _size+=4;//gainEquipTimes
        _size+=4;//consortRandCallCount
        _size+=4;//consortCallCount
        _size+=4;//childNum
        _size+=4;//travelCount
        _size+=4;//starDinnerCount
        _size+=4;//joinDinnerCount
        _size+=4;//arenaAttackTimes
        _size+=4;//ArenaStationCollectTimes
        _size+=8;//towerPassedChapter
        _size+=4;//rankLikeCount
        _size+=4;//youngChildNum
        _size+=4;//adultChildNum
        _size+=4;//childMarryCount
        _size+=8;//marryChildEarning
        _size+=8;//unmarriedChildEarning
        _size+=4;//childCultureCount
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putInt(sectionType);
        buff.putInt(event_id);
        buff.putLong(guid);
        buff.putInt(date_time);
        buff.putInt(timestamp);
        buff.putLong(cid);
        buff.putLong(villageEarning);
        buff.putLong(childEarning);
        buff.putLong(chapterStageId);
        buff.putInt(consortNum);
        buff.putInt(heroNum);
        buff.putInt(level);
        buff.putLong(totalTalent);
        buff.putLong(totalFightPower);
        buff.putLong(totalDiamondGain);
        buff.putLong(totalGoldGain);
        buff.putLong(totalDiamondCost);
        buff.putLong(totalGoldCost);
        buff.putLong(farmGoldGain);
        buff.putLong(totalPlayerExp);
        buff.putLong(questId);
        buff.putInt(loginDayCount);
        buff.putLong(highestEarning);
        buff.putInt(farmCollect);
        buff.putInt(farmLvl);
        buff.putInt(totalHeroLvl);
        buff.putInt(totalBuildingLvl);
        buff.putInt(gainEquipTimes);
        buff.putInt(consortRandCallCount);
        buff.putInt(consortCallCount);
        buff.putInt(childNum);
        buff.putInt(travelCount);
        buff.putInt(starDinnerCount);
        buff.putInt(joinDinnerCount);
        buff.putInt(arenaAttackTimes);
        buff.putInt(ArenaStationCollectTimes);
        buff.putLong(towerPassedChapter);
        buff.putInt(rankLikeCount);
        buff.putInt(youngChildNum);
        buff.putInt(adultChildNum);
        buff.putInt(childMarryCount);
        buff.putLong(marryChildEarning);
        buff.putLong(unmarriedChildEarning);
        buff.putInt(childCultureCount);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        sectionType=buff.getInt();
        event_id=buff.getInt();
        guid=buff.getLong();
        date_time=buff.getInt();
        timestamp=buff.getInt();
        cid=buff.getLong();
        villageEarning=buff.getLong();
        childEarning=buff.getLong();
        chapterStageId=buff.getLong();
        consortNum=buff.getInt();
        heroNum=buff.getInt();
        level=buff.getInt();
        totalTalent=buff.getLong();
        totalFightPower=buff.getLong();
        totalDiamondGain=buff.getLong();
        totalGoldGain=buff.getLong();
        totalDiamondCost=buff.getLong();
        totalGoldCost=buff.getLong();
        farmGoldGain=buff.getLong();
        totalPlayerExp=buff.getLong();
        questId=buff.getLong();
        loginDayCount=buff.getInt();
        highestEarning=buff.getLong();
        farmCollect=buff.getInt();
        farmLvl=buff.getInt();
        totalHeroLvl=buff.getInt();
        totalBuildingLvl=buff.getInt();
        gainEquipTimes=buff.getInt();
        consortRandCallCount=buff.getInt();
        consortCallCount=buff.getInt();
        childNum=buff.getInt();
        travelCount=buff.getInt();
        starDinnerCount=buff.getInt();
        joinDinnerCount=buff.getInt();
        arenaAttackTimes=buff.getInt();
        ArenaStationCollectTimes=buff.getInt();
        towerPassedChapter=buff.getLong();
        rankLikeCount=buff.getInt();
        youngChildNum=buff.getInt();
        adultChildNum=buff.getInt();
        childMarryCount=buff.getInt();
        marryChildEarning=buff.getLong();
        unmarriedChildEarning=buff.getLong();
        childCultureCount=buff.getInt(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
