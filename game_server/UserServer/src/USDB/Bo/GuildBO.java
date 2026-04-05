package USDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.DB.BaseBO;
import NPCommon.DB.Annotation.RefBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

@RefBo(isIdAuto= true)
public class GuildBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guild_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "联盟id")
    private long guild_id;

    public static final int FIELD_flag_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "flag_id", comment = "旗帜id")
    private long flag_id;

    public static final int FIELD_name =2;
    @DataBaseField(type = "varchar(512)", fieldname = "name", comment = "名称")
    private String name;

    public static final int FIELD_simple_name =3;
    @DataBaseField(type = "varchar(128)", fieldname = "simple_name", comment = "简称")
    private String simple_name;

    public static final int FIELD_declaration =4;
    @DataBaseField(type = "varchar(1024)", fieldname = "declaration", comment = "宣言")
    private String declaration;

    public static final int FIELD_announcement =5;
    @DataBaseField(type = "varchar(1024)", fieldname = "announcement", comment = "公告")
    private String announcement;

    public static final int FIELD_level =6;
    @DataBaseField(type = "int(11)", fieldname = "level", comment = "等级")
    private int level;

    public static final int FIELD_exp =7;
    @DataBaseField(type = "bigint(20)", fieldname = "exp", comment = "经验")
    private long exp;

    public static final int FIELD_join_type =8;
    @DataBaseField(type = "int(11)", fieldname = "join_type", comment = "加入类型")
    private int join_type;

    public static final int FIELD_join_limit_info =9;
    @DataBaseField(type = "blob", fieldname = "join_limit_info", comment = "加入限制信息")
    private byte[] join_limit_info;

    public static final int FIELD_is_dissovle =10;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_dissovle", comment = "是否解散")
    private boolean is_dissovle;

    public static final int FIELD_wealth =11;
    @DataBaseField(type = "bigint(20)", fieldname = "wealth", comment = "联盟财富")
    private long wealth;

    public static final int FIELD_construct_list =12;
    @DataBaseField(type = "blob", fieldname = "construct_list", comment = "建造信息")
    private byte[] construct_list;

    public static final int FIELD_last_proactive_trans_leader_time_ms =13;
    @DataBaseField(type = "bigint(20)", fieldname = "last_proactive_trans_leader_time_ms", comment = "上次主动转让盟主时间")
    private long last_proactive_trans_leader_time_ms;

    public static final int FIELD_next_open_recruit_time_ms =14;
    @DataBaseField(type = "bigint(20)", fieldname = "next_open_recruit_time_ms", comment = "下一次公开招募时间")
    private long next_open_recruit_time_ms;

    public static final int FIELD_entrust_ref_id =15;
    @DataBaseField(type = "bigint(20)", fieldname = "entrust_ref_id", comment = "委托配表id")
    private long entrust_ref_id;

    public static final int FIELD_entrust_event_id =16;
    @DataBaseField(type = "bigint(20)", fieldname = "entrust_event_id", comment = "委托事件id")
    private long entrust_event_id;

    public static final int FIELD_entrust_weight_base_list =17;
    @DataBaseField(type = "varchar(512)", fieldname = "entrust_weight_base_list", comment = "委托权重依据列表")
    private String entrust_weight_base_list;

    public static final int FIELD_entrust_point =18;
    @DataBaseField(type = "int(11)", fieldname = "entrust_point", comment = "委托点数")
    private int entrust_point;

    public static final int FIELD_active_point =19;
    @DataBaseField(type = "bigint(20)", fieldname = "active_point", comment = "活跃点")
    private long active_point;

    public static final int FIELD_active_point_target_lvl =20;
    @DataBaseField(type = "int(11)", fieldname = "active_point_target_lvl", comment = "活跃点目标等级")
    private int active_point_target_lvl;

    public static final int FIELD_mars_help_daily_record =21;
    @DataBaseField(type = "blob", fieldname = "mars_help_daily_record", comment = "火星互助每日记录数据")
    private byte[] mars_help_daily_record;

    public GuildBO() {
        id = 0;
        guild_id = 0L;
        flag_id = 0L;
        name = "";
        simple_name = "";
        declaration = "";
        announcement = "";
        level = 0;
        exp = 0L;
        join_type = 0;
        join_limit_info = null;
        is_dissovle = false;
        wealth = 0L;
        construct_list = null;
        last_proactive_trans_leader_time_ms = 0L;
        next_open_recruit_time_ms = 0L;
        entrust_ref_id = 0L;
        entrust_event_id = 0L;
        entrust_weight_base_list = "";
        entrust_point = 0;
        active_point = 0L;
        active_point_target_lvl = 0;
        mars_help_daily_record = null;
    }

    public GuildBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guild_id = rs.getLong(2);
        flag_id = rs.getLong(3);
        name = rs.getString(4);
        simple_name = rs.getString(5);
        declaration = rs.getString(6);
        announcement = rs.getString(7);
        level = rs.getInt(8);
        exp = rs.getLong(9);
        join_type = rs.getInt(10);
        join_limit_info = rs.getBytes(11);
        is_dissovle = rs.getBoolean(12);
        wealth = rs.getLong(13);
        construct_list = rs.getBytes(14);
        last_proactive_trans_leader_time_ms = rs.getLong(15);
        next_open_recruit_time_ms = rs.getLong(16);
        entrust_ref_id = rs.getLong(17);
        entrust_event_id = rs.getLong(18);
        entrust_weight_base_list = rs.getString(19);
        entrust_point = rs.getInt(20);
        active_point = rs.getLong(21);
        active_point_target_lvl = rs.getInt(22);
        mars_help_daily_record = rs.getBytes(23);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guild_id`, `flag_id`, `name`, `simple_name`, `declaration`, `announcement`, `level`, `exp`, `join_type`, `join_limit_info`, `is_dissovle`, `wealth`, `construct_list`, `last_proactive_trans_leader_time_ms`, `next_open_recruit_time_ms`, `entrust_ref_id`, `entrust_event_id`, `entrust_weight_base_list`, `entrust_point`, `active_point`, `active_point_target_lvl`, `mars_help_daily_record`";
    }

    @Override
    public String getTableName() {
        return "`guild`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(flag_id).append("', ");
        strBuf.append("'").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(simple_name == null ? null : simple_name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(declaration == null ? null : declaration.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(announcement == null ? null : announcement.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(level).append("', ");
        strBuf.append("'").append(exp).append("', ");
        strBuf.append("'").append(join_type).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(is_dissovle ? 1 : 0).append("', ");
        strBuf.append("'").append(wealth).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(last_proactive_trans_leader_time_ms).append("', ");
        strBuf.append("'").append(next_open_recruit_time_ms).append("', ");
        strBuf.append("'").append(entrust_ref_id).append("', ");
        strBuf.append("'").append(entrust_event_id).append("', ");
        strBuf.append("'").append(entrust_weight_base_list == null ? null : entrust_weight_base_list.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(entrust_point).append("', ");
        strBuf.append("'").append(active_point).append("', ");
        strBuf.append("'").append(active_point_target_lvl).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(join_limit_info); 
        ret.add(construct_list); 
        ret.add(mars_help_daily_record);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_join_limit_info)) ret.add(join_limit_info); 
        if(isFieldMarked(FIELD_construct_list)) ret.add(construct_list); 
        if(isFieldMarked(FIELD_mars_help_daily_record)) ret.add(mars_help_daily_record);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 联盟id
    public long getGuildId() { return this.guild_id; }
    public void setGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id; 
        markField(_bm, FIELD_guild_id); 
    }
    public void saveGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id;
        saveField(_bm, "guild_id", guild_id);
    }

    // 旗帜id
    public long getFlagId() { return this.flag_id; }
    public void setFlagId(BM _bm, long flag_id) {
        if(flag_id==this.flag_id) 
            return;
        this.flag_id = flag_id; 
        markField(_bm, FIELD_flag_id); 
    }
    public void saveFlagId(BM _bm, long flag_id) {
        if(flag_id==this.flag_id) 
            return;
        this.flag_id = flag_id;
        saveField(_bm, "flag_id", flag_id);
    }

    // 名称
    public String getName() { return this.name; }
    public void setName(BM _bm, String name) {
        if(name.equals(this.name)) 
            return;
        this.name = name; 
        markField(_bm, FIELD_name); 
    }
    public void saveName(BM _bm, String name) {
        if(name.equals(this.name)) 
            return;
        this.name = name;
        saveField(_bm, "name", name);
    }

    // 简称
    public String getSimpleName() { return this.simple_name; }
    public void setSimpleName(BM _bm, String simple_name) {
        if(simple_name.equals(this.simple_name)) 
            return;
        this.simple_name = simple_name; 
        markField(_bm, FIELD_simple_name); 
    }
    public void saveSimpleName(BM _bm, String simple_name) {
        if(simple_name.equals(this.simple_name)) 
            return;
        this.simple_name = simple_name;
        saveField(_bm, "simple_name", simple_name);
    }

    // 宣言
    public String getDeclaration() { return this.declaration; }
    public void setDeclaration(BM _bm, String declaration) {
        if(declaration.equals(this.declaration)) 
            return;
        this.declaration = declaration; 
        markField(_bm, FIELD_declaration); 
    }
    public void saveDeclaration(BM _bm, String declaration) {
        if(declaration.equals(this.declaration)) 
            return;
        this.declaration = declaration;
        saveField(_bm, "declaration", declaration);
    }

    // 公告
    public String getAnnouncement() { return this.announcement; }
    public void setAnnouncement(BM _bm, String announcement) {
        if(announcement.equals(this.announcement)) 
            return;
        this.announcement = announcement; 
        markField(_bm, FIELD_announcement); 
    }
    public void saveAnnouncement(BM _bm, String announcement) {
        if(announcement.equals(this.announcement)) 
            return;
        this.announcement = announcement;
        saveField(_bm, "announcement", announcement);
    }

    // 等级
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

    // 经验
    public long getExp() { return this.exp; }
    public void setExp(BM _bm, long exp) {
        if(exp==this.exp) 
            return;
        this.exp = exp; 
        markField(_bm, FIELD_exp); 
    }
    public void saveExp(BM _bm, long exp) {
        if(exp==this.exp) 
            return;
        this.exp = exp;
        saveField(_bm, "exp", exp);
    }

    // 加入类型
    public int getJoinType() { return this.join_type; }
    public void setJoinType(BM _bm, int join_type) {
        if(join_type==this.join_type) 
            return;
        this.join_type = join_type; 
        markField(_bm, FIELD_join_type); 
    }
    public void saveJoinType(BM _bm, int join_type) {
        if(join_type==this.join_type) 
            return;
        this.join_type = join_type;
        saveField(_bm, "join_type", join_type);
    }

    // 加入限制信息
    public byte[] getJoinLimitInfo() { return this.join_limit_info; }
    public void setJoinLimitInfo(BM _bm, byte[] join_limit_info) {
        if(join_limit_info==this.join_limit_info) 
            return;
        this.join_limit_info = join_limit_info; 
        markField(_bm, FIELD_join_limit_info); 
    }
    public void saveJoinLimitInfo(BM _bm, byte[] join_limit_info) {
        if(join_limit_info==this.join_limit_info) 
            return;
        this.join_limit_info = join_limit_info;
        saveFieldBytes(_bm, "join_limit_info", join_limit_info);
    }

    // 是否解散
    public boolean getIsDissovle() { return this.is_dissovle; }
    public void setIsDissovle(BM _bm, boolean is_dissovle) {
        if(is_dissovle==this.is_dissovle) 
            return;
        this.is_dissovle = is_dissovle; 
        markField(_bm, FIELD_is_dissovle); 
    }
    public void saveIsDissovle(BM _bm, boolean is_dissovle) {
        if(is_dissovle==this.is_dissovle) 
            return;
        this.is_dissovle = is_dissovle;
        saveField(_bm, "is_dissovle", is_dissovle ? 1 : 0);
    }

    // 联盟财富
    public long getWealth() { return this.wealth; }
    public void setWealth(BM _bm, long wealth) {
        if(wealth==this.wealth) 
            return;
        this.wealth = wealth; 
        markField(_bm, FIELD_wealth); 
    }
    public void saveWealth(BM _bm, long wealth) {
        if(wealth==this.wealth) 
            return;
        this.wealth = wealth;
        saveField(_bm, "wealth", wealth);
    }

    // 建造信息
    public byte[] getConstructList() { return this.construct_list; }
    public void setConstructList(BM _bm, byte[] construct_list) {
        if(construct_list==this.construct_list) 
            return;
        this.construct_list = construct_list; 
        markField(_bm, FIELD_construct_list); 
    }
    public void saveConstructList(BM _bm, byte[] construct_list) {
        if(construct_list==this.construct_list) 
            return;
        this.construct_list = construct_list;
        saveFieldBytes(_bm, "construct_list", construct_list);
    }

    // 上次主动转让盟主时间
    public long getLastProactiveTransLeaderTimeMs() { return this.last_proactive_trans_leader_time_ms; }
    public void setLastProactiveTransLeaderTimeMs(BM _bm, long last_proactive_trans_leader_time_ms) {
        if(last_proactive_trans_leader_time_ms==this.last_proactive_trans_leader_time_ms) 
            return;
        this.last_proactive_trans_leader_time_ms = last_proactive_trans_leader_time_ms; 
        markField(_bm, FIELD_last_proactive_trans_leader_time_ms); 
    }
    public void saveLastProactiveTransLeaderTimeMs(BM _bm, long last_proactive_trans_leader_time_ms) {
        if(last_proactive_trans_leader_time_ms==this.last_proactive_trans_leader_time_ms) 
            return;
        this.last_proactive_trans_leader_time_ms = last_proactive_trans_leader_time_ms;
        saveField(_bm, "last_proactive_trans_leader_time_ms", last_proactive_trans_leader_time_ms);
    }

    // 下一次公开招募时间
    public long getNextOpenRecruitTimeMs() { return this.next_open_recruit_time_ms; }
    public void setNextOpenRecruitTimeMs(BM _bm, long next_open_recruit_time_ms) {
        if(next_open_recruit_time_ms==this.next_open_recruit_time_ms) 
            return;
        this.next_open_recruit_time_ms = next_open_recruit_time_ms; 
        markField(_bm, FIELD_next_open_recruit_time_ms); 
    }
    public void saveNextOpenRecruitTimeMs(BM _bm, long next_open_recruit_time_ms) {
        if(next_open_recruit_time_ms==this.next_open_recruit_time_ms) 
            return;
        this.next_open_recruit_time_ms = next_open_recruit_time_ms;
        saveField(_bm, "next_open_recruit_time_ms", next_open_recruit_time_ms);
    }

    // 委托配表id
    public long getEntrustRefId() { return this.entrust_ref_id; }
    public void setEntrustRefId(BM _bm, long entrust_ref_id) {
        if(entrust_ref_id==this.entrust_ref_id) 
            return;
        this.entrust_ref_id = entrust_ref_id; 
        markField(_bm, FIELD_entrust_ref_id); 
    }
    public void saveEntrustRefId(BM _bm, long entrust_ref_id) {
        if(entrust_ref_id==this.entrust_ref_id) 
            return;
        this.entrust_ref_id = entrust_ref_id;
        saveField(_bm, "entrust_ref_id", entrust_ref_id);
    }

    // 委托事件id
    public long getEntrustEventId() { return this.entrust_event_id; }
    public void setEntrustEventId(BM _bm, long entrust_event_id) {
        if(entrust_event_id==this.entrust_event_id) 
            return;
        this.entrust_event_id = entrust_event_id; 
        markField(_bm, FIELD_entrust_event_id); 
    }
    public void saveEntrustEventId(BM _bm, long entrust_event_id) {
        if(entrust_event_id==this.entrust_event_id) 
            return;
        this.entrust_event_id = entrust_event_id;
        saveField(_bm, "entrust_event_id", entrust_event_id);
    }

    // 委托权重依据列表
    public String getEntrustWeightBaseList() { return this.entrust_weight_base_list; }
    public void setEntrustWeightBaseList(BM _bm, String entrust_weight_base_list) {
        if(entrust_weight_base_list.equals(this.entrust_weight_base_list)) 
            return;
        this.entrust_weight_base_list = entrust_weight_base_list; 
        markField(_bm, FIELD_entrust_weight_base_list); 
    }
    public void saveEntrustWeightBaseList(BM _bm, String entrust_weight_base_list) {
        if(entrust_weight_base_list.equals(this.entrust_weight_base_list)) 
            return;
        this.entrust_weight_base_list = entrust_weight_base_list;
        saveField(_bm, "entrust_weight_base_list", entrust_weight_base_list);
    }

    // 委托点数
    public int getEntrustPoint() { return this.entrust_point; }
    public void setEntrustPoint(BM _bm, int entrust_point) {
        if(entrust_point==this.entrust_point) 
            return;
        this.entrust_point = entrust_point; 
        markField(_bm, FIELD_entrust_point); 
    }
    public void saveEntrustPoint(BM _bm, int entrust_point) {
        if(entrust_point==this.entrust_point) 
            return;
        this.entrust_point = entrust_point;
        saveField(_bm, "entrust_point", entrust_point);
    }

    // 活跃点
    public long getActivePoint() { return this.active_point; }
    public void setActivePoint(BM _bm, long active_point) {
        if(active_point==this.active_point) 
            return;
        this.active_point = active_point; 
        markField(_bm, FIELD_active_point); 
    }
    public void saveActivePoint(BM _bm, long active_point) {
        if(active_point==this.active_point) 
            return;
        this.active_point = active_point;
        saveField(_bm, "active_point", active_point);
    }

    // 活跃点目标等级
    public int getActivePointTargetLvl() { return this.active_point_target_lvl; }
    public void setActivePointTargetLvl(BM _bm, int active_point_target_lvl) {
        if(active_point_target_lvl==this.active_point_target_lvl) 
            return;
        this.active_point_target_lvl = active_point_target_lvl; 
        markField(_bm, FIELD_active_point_target_lvl); 
    }
    public void saveActivePointTargetLvl(BM _bm, int active_point_target_lvl) {
        if(active_point_target_lvl==this.active_point_target_lvl) 
            return;
        this.active_point_target_lvl = active_point_target_lvl;
        saveField(_bm, "active_point_target_lvl", active_point_target_lvl);
    }

    // 火星互助每日记录数据
    public byte[] getMarsHelpDailyRecord() { return this.mars_help_daily_record; }
    public void setMarsHelpDailyRecord(BM _bm, byte[] mars_help_daily_record) {
        if(mars_help_daily_record==this.mars_help_daily_record) 
            return;
        this.mars_help_daily_record = mars_help_daily_record; 
        markField(_bm, FIELD_mars_help_daily_record); 
    }
    public void saveMarsHelpDailyRecord(BM _bm, byte[] mars_help_daily_record) {
        if(mars_help_daily_record==this.mars_help_daily_record) 
            return;
        this.mars_help_daily_record = mars_help_daily_record;
        saveFieldBytes(_bm, "mars_help_daily_record", mars_help_daily_record);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `flag_id` = '").append(flag_id).append("',");
        sBuilder.append(" `name` = '").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `simple_name` = '").append(simple_name == null ? null : simple_name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `declaration` = '").append(declaration == null ? null : declaration.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `announcement` = '").append(announcement == null ? null : announcement.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.append(" `exp` = '").append(exp).append("',");
        sBuilder.append(" `join_type` = '").append(join_type).append("',");
        sBuilder.append(" `join_limit_info` = ?,");
        sBuilder.append(" `is_dissovle` = '").append(is_dissovle ? 1 : 0).append("',");
        sBuilder.append(" `wealth` = '").append(wealth).append("',");
        sBuilder.append(" `construct_list` = ?,");
        sBuilder.append(" `last_proactive_trans_leader_time_ms` = '").append(last_proactive_trans_leader_time_ms).append("',");
        sBuilder.append(" `next_open_recruit_time_ms` = '").append(next_open_recruit_time_ms).append("',");
        sBuilder.append(" `entrust_ref_id` = '").append(entrust_ref_id).append("',");
        sBuilder.append(" `entrust_event_id` = '").append(entrust_event_id).append("',");
        sBuilder.append(" `entrust_weight_base_list` = '").append(entrust_weight_base_list == null ? null : entrust_weight_base_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `entrust_point` = '").append(entrust_point).append("',");
        sBuilder.append(" `active_point` = '").append(active_point).append("',");
        sBuilder.append(" `active_point_target_lvl` = '").append(active_point_target_lvl).append("',");
        sBuilder.append(" `mars_help_daily_record` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_flag_id)) sBuilder.append(" `flag_id` = '").append(flag_id).append("',");
        if(isFieldMarked(FIELD_name)) sBuilder.append(" `name` = '").append(name == null ? null : name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_simple_name)) sBuilder.append(" `simple_name` = '").append(simple_name == null ? null : simple_name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_declaration)) sBuilder.append(" `declaration` = '").append(declaration == null ? null : declaration.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_announcement)) sBuilder.append(" `announcement` = '").append(announcement == null ? null : announcement.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_level)) sBuilder.append(" `level` = '").append(level).append("',");
        if(isFieldMarked(FIELD_exp)) sBuilder.append(" `exp` = '").append(exp).append("',");
        if(isFieldMarked(FIELD_join_type)) sBuilder.append(" `join_type` = '").append(join_type).append("',");
        if(isFieldMarked(FIELD_join_limit_info)) sBuilder.append(" `join_limit_info` = ?,");
        if(isFieldMarked(FIELD_is_dissovle)) sBuilder.append(" `is_dissovle` = '").append(is_dissovle ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_wealth)) sBuilder.append(" `wealth` = '").append(wealth).append("',");
        if(isFieldMarked(FIELD_construct_list)) sBuilder.append(" `construct_list` = ?,");
        if(isFieldMarked(FIELD_last_proactive_trans_leader_time_ms)) sBuilder.append(" `last_proactive_trans_leader_time_ms` = '").append(last_proactive_trans_leader_time_ms).append("',");
        if(isFieldMarked(FIELD_next_open_recruit_time_ms)) sBuilder.append(" `next_open_recruit_time_ms` = '").append(next_open_recruit_time_ms).append("',");
        if(isFieldMarked(FIELD_entrust_ref_id)) sBuilder.append(" `entrust_ref_id` = '").append(entrust_ref_id).append("',");
        if(isFieldMarked(FIELD_entrust_event_id)) sBuilder.append(" `entrust_event_id` = '").append(entrust_event_id).append("',");
        if(isFieldMarked(FIELD_entrust_weight_base_list)) sBuilder.append(" `entrust_weight_base_list` = '").append(entrust_weight_base_list == null ? null : entrust_weight_base_list.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_entrust_point)) sBuilder.append(" `entrust_point` = '").append(entrust_point).append("',");
        if(isFieldMarked(FIELD_active_point)) sBuilder.append(" `active_point` = '").append(active_point).append("',");
        if(isFieldMarked(FIELD_active_point_target_lvl)) sBuilder.append(" `active_point_target_lvl` = '").append(active_point_target_lvl).append("',");
        if(isFieldMarked(FIELD_mars_help_daily_record)) sBuilder.append(" `mars_help_daily_record` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟id',"
                + "`flag_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '旗帜id',"
                + "`name` varchar(512) NOT NULL DEFAULT '' COMMENT '名称',"
                + "`simple_name` varchar(128) NOT NULL DEFAULT '' COMMENT '简称',"
                + "`declaration` varchar(1024) NOT NULL DEFAULT '' COMMENT '宣言',"
                + "`announcement` varchar(1024) NOT NULL DEFAULT '' COMMENT '公告',"
                + "`level` int(11) NOT NULL DEFAULT '0' COMMENT '等级',"
                + "`exp` bigint(20) NOT NULL DEFAULT '0' COMMENT '经验',"
                + "`join_type` int(11) NOT NULL DEFAULT '0' COMMENT '加入类型',"
                + "`join_limit_info` blob NULL COMMENT '加入限制信息',"
                + "`is_dissovle` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否解散',"
                + "`wealth` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟财富',"
                + "`construct_list` blob NULL COMMENT '建造信息',"
                + "`last_proactive_trans_leader_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次主动转让盟主时间',"
                + "`next_open_recruit_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '下一次公开招募时间',"
                + "`entrust_ref_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '委托配表id',"
                + "`entrust_event_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '委托事件id',"
                + "`entrust_weight_base_list` varchar(512) NOT NULL DEFAULT '' COMMENT '委托权重依据列表',"
                + "`entrust_point` int(11) NOT NULL DEFAULT '0' COMMENT '委托点数',"
                + "`active_point` bigint(20) NOT NULL DEFAULT '0' COMMENT '活跃点',"
                + "`active_point_target_lvl` int(11) NOT NULL DEFAULT '0' COMMENT '活跃点目标等级',"
                + "`mars_help_daily_record` blob NULL COMMENT '火星互助每日记录数据',"
                + "KEY `guild_id` (`guild_id`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//guild_id
        _size+=8;//flag_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);//name
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simple_name);//simple_name
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(declaration);//declaration
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(announcement);//announcement
        _size+=4;//level
        _size+=8;//exp
        _size+=4;//join_type
        _size+=2;_size+=join_limit_info.length;//join_limit_info
        _size+=1;//is_dissovle
        _size+=8;//wealth
        _size+=2;_size+=construct_list.length;//construct_list
        _size+=8;//last_proactive_trans_leader_time_ms
        _size+=8;//next_open_recruit_time_ms
        _size+=8;//entrust_ref_id
        _size+=8;//entrust_event_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(entrust_weight_base_list);//entrust_weight_base_list
        _size+=4;//entrust_point
        _size+=8;//active_point
        _size+=4;//active_point_target_lvl
        _size+=2;_size+=mars_help_daily_record.length;//mars_help_daily_record
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guild_id);
        buff.putLong(flag_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, name);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, simple_name);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, declaration);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, announcement);
        buff.putInt(level);
        buff.putLong(exp);
        buff.putInt(join_type);
        buff.putShort((short)(join_limit_info == null ? 0 : join_limit_info.length));if(null != join_limit_info){buff.put(join_limit_info);}
        buff.put((byte)(is_dissovle?1:0));
        buff.putLong(wealth);
        buff.putShort((short)(construct_list == null ? 0 : construct_list.length));if(null != construct_list){buff.put(construct_list);}
        buff.putLong(last_proactive_trans_leader_time_ms);
        buff.putLong(next_open_recruit_time_ms);
        buff.putLong(entrust_ref_id);
        buff.putLong(entrust_event_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, entrust_weight_base_list);
        buff.putInt(entrust_point);
        buff.putLong(active_point);
        buff.putInt(active_point_target_lvl);
        buff.putShort((short)(mars_help_daily_record == null ? 0 : mars_help_daily_record.length));if(null != mars_help_daily_record){buff.put(mars_help_daily_record);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guild_id=buff.getLong();
        flag_id=buff.getLong();
        name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        simple_name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        declaration=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        announcement=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        level=buff.getInt();
        exp=buff.getLong();
        join_type=buff.getInt();
        int join_limit_info_count = buff.getShort();if(join_limit_info_count>0){join_limit_info = new byte[join_limit_info_count];buff.get(join_limit_info);}
        is_dissovle=(buff.get()==1);
        wealth=buff.getLong();
        int construct_list_count = buff.getShort();if(construct_list_count>0){construct_list = new byte[construct_list_count];buff.get(construct_list);}
        last_proactive_trans_leader_time_ms=buff.getLong();
        next_open_recruit_time_ms=buff.getLong();
        entrust_ref_id=buff.getLong();
        entrust_event_id=buff.getLong();
        entrust_weight_base_list=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        entrust_point=buff.getInt();
        active_point=buff.getLong();
        active_point_target_lvl=buff.getInt();
        int mars_help_daily_record_count = buff.getShort();if(mars_help_daily_record_count>0){mars_help_daily_record = new byte[mars_help_daily_record_count];buff.get(mars_help_daily_record);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
