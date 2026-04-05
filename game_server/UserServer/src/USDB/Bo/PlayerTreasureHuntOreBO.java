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
public class PlayerTreasureHuntOreBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_ore_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "ore_id", comment = "矿石ID")
    private long ore_id;

    public static final int FIELD_first_gain_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "first_gain_time_ms", comment = "首次获得时间 ms")
    private long first_gain_time_ms;

    public static final int FIELD_max_record =3;
    @DataBaseField(type = "int(11)", fieldname = "max_record", comment = "最大记录")
    private int max_record;

    public static final int FIELD_reach_max_record_time_ms =4;
    @DataBaseField(type = "bigint(20)", fieldname = "reach_max_record_time_ms", comment = "达到最大记录时间 ms")
    private long reach_max_record_time_ms;

    public static final int FIELD_had_reach_advanced =5;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_reach_advanced", comment = "是否已达到高级矿石")
    private boolean had_reach_advanced;

    public static final int FIELD_had_draw_record_reward_list =6;
    @DataBaseField(type = "varchar(256)", fieldname = "had_draw_record_reward_list", comment = "已领取记录档位奖励列表")
    private String had_draw_record_reward_list;

    public static final int FIELD_total_gain_num =7;
    @DataBaseField(type = "int(11)", fieldname = "total_gain_num", comment = "总获得数量")
    private int total_gain_num;

    public static final int FIELD_normal_pending_num =8;
    @DataBaseField(type = "int(11)", fieldname = "normal_pending_num", comment = "普通矿石待处理数量")
    private int normal_pending_num;

    public static final int FIELD_advanced_pending_num =9;
    @DataBaseField(type = "int(11)", fieldname = "advanced_pending_num", comment = "高级矿石待处理数量")
    private int advanced_pending_num;

    public static final int FIELD_normal_skill_level =10;
    @DataBaseField(type = "int(11)", fieldname = "normal_skill_level", comment = "普通技能等级")
    private int normal_skill_level;

    public static final int FIELD_normal_skill_point =11;
    @DataBaseField(type = "int(11)", fieldname = "normal_skill_point", comment = "普通技能点数")
    private int normal_skill_point;

    public static final int FIELD_advanced_skill_level =12;
    @DataBaseField(type = "int(11)", fieldname = "advanced_skill_level", comment = "高级技能等级")
    private int advanced_skill_level;

    public static final int FIELD_advanced_skill_point =13;
    @DataBaseField(type = "int(11)", fieldname = "advanced_skill_point", comment = "高级技能点数")
    private int advanced_skill_point;

    public PlayerTreasureHuntOreBO() {
        id = 0;
        cid = 0L;
        ore_id = 0L;
        first_gain_time_ms = 0L;
        max_record = 0;
        reach_max_record_time_ms = 0L;
        had_reach_advanced = false;
        had_draw_record_reward_list = "";
        total_gain_num = 0;
        normal_pending_num = 0;
        advanced_pending_num = 0;
        normal_skill_level = 0;
        normal_skill_point = 0;
        advanced_skill_level = 0;
        advanced_skill_point = 0;
    }

    public PlayerTreasureHuntOreBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        ore_id = rs.getLong(3);
        first_gain_time_ms = rs.getLong(4);
        max_record = rs.getInt(5);
        reach_max_record_time_ms = rs.getLong(6);
        had_reach_advanced = rs.getBoolean(7);
        had_draw_record_reward_list = rs.getString(8);
        total_gain_num = rs.getInt(9);
        normal_pending_num = rs.getInt(10);
        advanced_pending_num = rs.getInt(11);
        normal_skill_level = rs.getInt(12);
        normal_skill_point = rs.getInt(13);
        advanced_skill_level = rs.getInt(14);
        advanced_skill_point = rs.getInt(15);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerTreasureHuntOreBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `ore_id`, `first_gain_time_ms`, `max_record`, `reach_max_record_time_ms`, `had_reach_advanced`, `had_draw_record_reward_list`, `total_gain_num`, `normal_pending_num`, `advanced_pending_num`, `normal_skill_level`, `normal_skill_point`, `advanced_skill_level`, `advanced_skill_point`";
    }

    @Override
    public String getTableName() {
        return "`player_treasure_hunt_ore`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(ore_id).append("', ");
        strBuf.append("'").append(first_gain_time_ms).append("', ");
        strBuf.append("'").append(max_record).append("', ");
        strBuf.append("'").append(reach_max_record_time_ms).append("', ");
        strBuf.append("'").append(had_reach_advanced ? 1 : 0).append("', ");
        strBuf.append("'").append(had_draw_record_reward_list == null ? null : had_draw_record_reward_list.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(total_gain_num).append("', ");
        strBuf.append("'").append(normal_pending_num).append("', ");
        strBuf.append("'").append(advanced_pending_num).append("', ");
        strBuf.append("'").append(normal_skill_level).append("', ");
        strBuf.append("'").append(normal_skill_point).append("', ");
        strBuf.append("'").append(advanced_skill_level).append("', ");
        strBuf.append("'").append(advanced_skill_point).append("', ");
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

    // 矿石ID
    public long getOreId() { return this.ore_id; }
    public void setOreId(BM _bm, long ore_id) {
        if(ore_id==this.ore_id) 
            return;
        this.ore_id = ore_id; 
        markField(_bm, FIELD_ore_id); 
    }
    public void saveOreId(BM _bm, long ore_id) {
        if(ore_id==this.ore_id) 
            return;
        this.ore_id = ore_id;
        saveField(_bm, "ore_id", ore_id);
    }

    // 首次获得时间 ms
    public long getFirstGainTimeMs() { return this.first_gain_time_ms; }
    public void setFirstGainTimeMs(BM _bm, long first_gain_time_ms) {
        if(first_gain_time_ms==this.first_gain_time_ms) 
            return;
        this.first_gain_time_ms = first_gain_time_ms; 
        markField(_bm, FIELD_first_gain_time_ms); 
    }
    public void saveFirstGainTimeMs(BM _bm, long first_gain_time_ms) {
        if(first_gain_time_ms==this.first_gain_time_ms) 
            return;
        this.first_gain_time_ms = first_gain_time_ms;
        saveField(_bm, "first_gain_time_ms", first_gain_time_ms);
    }

    // 最大记录
    public int getMaxRecord() { return this.max_record; }
    public void setMaxRecord(BM _bm, int max_record) {
        if(max_record==this.max_record) 
            return;
        this.max_record = max_record; 
        markField(_bm, FIELD_max_record); 
    }
    public void saveMaxRecord(BM _bm, int max_record) {
        if(max_record==this.max_record) 
            return;
        this.max_record = max_record;
        saveField(_bm, "max_record", max_record);
    }

    // 达到最大记录时间 ms
    public long getReachMaxRecordTimeMs() { return this.reach_max_record_time_ms; }
    public void setReachMaxRecordTimeMs(BM _bm, long reach_max_record_time_ms) {
        if(reach_max_record_time_ms==this.reach_max_record_time_ms) 
            return;
        this.reach_max_record_time_ms = reach_max_record_time_ms; 
        markField(_bm, FIELD_reach_max_record_time_ms); 
    }
    public void saveReachMaxRecordTimeMs(BM _bm, long reach_max_record_time_ms) {
        if(reach_max_record_time_ms==this.reach_max_record_time_ms) 
            return;
        this.reach_max_record_time_ms = reach_max_record_time_ms;
        saveField(_bm, "reach_max_record_time_ms", reach_max_record_time_ms);
    }

    // 是否已达到高级矿石
    public boolean getHadReachAdvanced() { return this.had_reach_advanced; }
    public void setHadReachAdvanced(BM _bm, boolean had_reach_advanced) {
        if(had_reach_advanced==this.had_reach_advanced) 
            return;
        this.had_reach_advanced = had_reach_advanced; 
        markField(_bm, FIELD_had_reach_advanced); 
    }
    public void saveHadReachAdvanced(BM _bm, boolean had_reach_advanced) {
        if(had_reach_advanced==this.had_reach_advanced) 
            return;
        this.had_reach_advanced = had_reach_advanced;
        saveField(_bm, "had_reach_advanced", had_reach_advanced ? 1 : 0);
    }

    // 已领取记录档位奖励列表
    public String getHadDrawRecordRewardList() { return this.had_draw_record_reward_list; }
    public void setHadDrawRecordRewardList(BM _bm, String had_draw_record_reward_list) {
        if(had_draw_record_reward_list.equals(this.had_draw_record_reward_list)) 
            return;
        this.had_draw_record_reward_list = had_draw_record_reward_list; 
        markField(_bm, FIELD_had_draw_record_reward_list); 
    }
    public void saveHadDrawRecordRewardList(BM _bm, String had_draw_record_reward_list) {
        if(had_draw_record_reward_list.equals(this.had_draw_record_reward_list)) 
            return;
        this.had_draw_record_reward_list = had_draw_record_reward_list;
        saveField(_bm, "had_draw_record_reward_list", had_draw_record_reward_list);
    }

    // 总获得数量
    public int getTotalGainNum() { return this.total_gain_num; }
    public void setTotalGainNum(BM _bm, int total_gain_num) {
        if(total_gain_num==this.total_gain_num) 
            return;
        this.total_gain_num = total_gain_num; 
        markField(_bm, FIELD_total_gain_num); 
    }
    public void saveTotalGainNum(BM _bm, int total_gain_num) {
        if(total_gain_num==this.total_gain_num) 
            return;
        this.total_gain_num = total_gain_num;
        saveField(_bm, "total_gain_num", total_gain_num);
    }

    // 普通矿石待处理数量
    public int getNormalPendingNum() { return this.normal_pending_num; }
    public void setNormalPendingNum(BM _bm, int normal_pending_num) {
        if(normal_pending_num==this.normal_pending_num) 
            return;
        this.normal_pending_num = normal_pending_num; 
        markField(_bm, FIELD_normal_pending_num); 
    }
    public void saveNormalPendingNum(BM _bm, int normal_pending_num) {
        if(normal_pending_num==this.normal_pending_num) 
            return;
        this.normal_pending_num = normal_pending_num;
        saveField(_bm, "normal_pending_num", normal_pending_num);
    }

    // 高级矿石待处理数量
    public int getAdvancedPendingNum() { return this.advanced_pending_num; }
    public void setAdvancedPendingNum(BM _bm, int advanced_pending_num) {
        if(advanced_pending_num==this.advanced_pending_num) 
            return;
        this.advanced_pending_num = advanced_pending_num; 
        markField(_bm, FIELD_advanced_pending_num); 
    }
    public void saveAdvancedPendingNum(BM _bm, int advanced_pending_num) {
        if(advanced_pending_num==this.advanced_pending_num) 
            return;
        this.advanced_pending_num = advanced_pending_num;
        saveField(_bm, "advanced_pending_num", advanced_pending_num);
    }

    // 普通技能等级
    public int getNormalSkillLevel() { return this.normal_skill_level; }
    public void setNormalSkillLevel(BM _bm, int normal_skill_level) {
        if(normal_skill_level==this.normal_skill_level) 
            return;
        this.normal_skill_level = normal_skill_level; 
        markField(_bm, FIELD_normal_skill_level); 
    }
    public void saveNormalSkillLevel(BM _bm, int normal_skill_level) {
        if(normal_skill_level==this.normal_skill_level) 
            return;
        this.normal_skill_level = normal_skill_level;
        saveField(_bm, "normal_skill_level", normal_skill_level);
    }

    // 普通技能点数
    public int getNormalSkillPoint() { return this.normal_skill_point; }
    public void setNormalSkillPoint(BM _bm, int normal_skill_point) {
        if(normal_skill_point==this.normal_skill_point) 
            return;
        this.normal_skill_point = normal_skill_point; 
        markField(_bm, FIELD_normal_skill_point); 
    }
    public void saveNormalSkillPoint(BM _bm, int normal_skill_point) {
        if(normal_skill_point==this.normal_skill_point) 
            return;
        this.normal_skill_point = normal_skill_point;
        saveField(_bm, "normal_skill_point", normal_skill_point);
    }

    // 高级技能等级
    public int getAdvancedSkillLevel() { return this.advanced_skill_level; }
    public void setAdvancedSkillLevel(BM _bm, int advanced_skill_level) {
        if(advanced_skill_level==this.advanced_skill_level) 
            return;
        this.advanced_skill_level = advanced_skill_level; 
        markField(_bm, FIELD_advanced_skill_level); 
    }
    public void saveAdvancedSkillLevel(BM _bm, int advanced_skill_level) {
        if(advanced_skill_level==this.advanced_skill_level) 
            return;
        this.advanced_skill_level = advanced_skill_level;
        saveField(_bm, "advanced_skill_level", advanced_skill_level);
    }

    // 高级技能点数
    public int getAdvancedSkillPoint() { return this.advanced_skill_point; }
    public void setAdvancedSkillPoint(BM _bm, int advanced_skill_point) {
        if(advanced_skill_point==this.advanced_skill_point) 
            return;
        this.advanced_skill_point = advanced_skill_point; 
        markField(_bm, FIELD_advanced_skill_point); 
    }
    public void saveAdvancedSkillPoint(BM _bm, int advanced_skill_point) {
        if(advanced_skill_point==this.advanced_skill_point) 
            return;
        this.advanced_skill_point = advanced_skill_point;
        saveField(_bm, "advanced_skill_point", advanced_skill_point);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `ore_id` = '").append(ore_id).append("',");
        sBuilder.append(" `first_gain_time_ms` = '").append(first_gain_time_ms).append("',");
        sBuilder.append(" `max_record` = '").append(max_record).append("',");
        sBuilder.append(" `reach_max_record_time_ms` = '").append(reach_max_record_time_ms).append("',");
        sBuilder.append(" `had_reach_advanced` = '").append(had_reach_advanced ? 1 : 0).append("',");
        sBuilder.append(" `had_draw_record_reward_list` = '").append(had_draw_record_reward_list == null ? null : had_draw_record_reward_list.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `total_gain_num` = '").append(total_gain_num).append("',");
        sBuilder.append(" `normal_pending_num` = '").append(normal_pending_num).append("',");
        sBuilder.append(" `advanced_pending_num` = '").append(advanced_pending_num).append("',");
        sBuilder.append(" `normal_skill_level` = '").append(normal_skill_level).append("',");
        sBuilder.append(" `normal_skill_point` = '").append(normal_skill_point).append("',");
        sBuilder.append(" `advanced_skill_level` = '").append(advanced_skill_level).append("',");
        sBuilder.append(" `advanced_skill_point` = '").append(advanced_skill_point).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_ore_id)) sBuilder.append(" `ore_id` = '").append(ore_id).append("',");
        if(isFieldMarked(FIELD_first_gain_time_ms)) sBuilder.append(" `first_gain_time_ms` = '").append(first_gain_time_ms).append("',");
        if(isFieldMarked(FIELD_max_record)) sBuilder.append(" `max_record` = '").append(max_record).append("',");
        if(isFieldMarked(FIELD_reach_max_record_time_ms)) sBuilder.append(" `reach_max_record_time_ms` = '").append(reach_max_record_time_ms).append("',");
        if(isFieldMarked(FIELD_had_reach_advanced)) sBuilder.append(" `had_reach_advanced` = '").append(had_reach_advanced ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_had_draw_record_reward_list)) sBuilder.append(" `had_draw_record_reward_list` = '").append(had_draw_record_reward_list == null ? null : had_draw_record_reward_list.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_total_gain_num)) sBuilder.append(" `total_gain_num` = '").append(total_gain_num).append("',");
        if(isFieldMarked(FIELD_normal_pending_num)) sBuilder.append(" `normal_pending_num` = '").append(normal_pending_num).append("',");
        if(isFieldMarked(FIELD_advanced_pending_num)) sBuilder.append(" `advanced_pending_num` = '").append(advanced_pending_num).append("',");
        if(isFieldMarked(FIELD_normal_skill_level)) sBuilder.append(" `normal_skill_level` = '").append(normal_skill_level).append("',");
        if(isFieldMarked(FIELD_normal_skill_point)) sBuilder.append(" `normal_skill_point` = '").append(normal_skill_point).append("',");
        if(isFieldMarked(FIELD_advanced_skill_level)) sBuilder.append(" `advanced_skill_level` = '").append(advanced_skill_level).append("',");
        if(isFieldMarked(FIELD_advanced_skill_point)) sBuilder.append(" `advanced_skill_point` = '").append(advanced_skill_point).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_treasure_hunt_ore` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`ore_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '矿石ID',"
                + "`first_gain_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '首次获得时间 ms',"
                + "`max_record` int(11) NOT NULL DEFAULT '0' COMMENT '最大记录',"
                + "`reach_max_record_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '达到最大记录时间 ms',"
                + "`had_reach_advanced` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已达到高级矿石',"
                + "`had_draw_record_reward_list` varchar(256) NOT NULL DEFAULT '' COMMENT '已领取记录档位奖励列表',"
                + "`total_gain_num` int(11) NOT NULL DEFAULT '0' COMMENT '总获得数量',"
                + "`normal_pending_num` int(11) NOT NULL DEFAULT '0' COMMENT '普通矿石待处理数量',"
                + "`advanced_pending_num` int(11) NOT NULL DEFAULT '0' COMMENT '高级矿石待处理数量',"
                + "`normal_skill_level` int(11) NOT NULL DEFAULT '0' COMMENT '普通技能等级',"
                + "`normal_skill_point` int(11) NOT NULL DEFAULT '0' COMMENT '普通技能点数',"
                + "`advanced_skill_level` int(11) NOT NULL DEFAULT '0' COMMENT '高级技能等级',"
                + "`advanced_skill_point` int(11) NOT NULL DEFAULT '0' COMMENT '高级技能点数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家太空寻宝矿石数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//cid
        _size+=8;//ore_id
        _size+=8;//first_gain_time_ms
        _size+=4;//max_record
        _size+=8;//reach_max_record_time_ms
        _size+=1;//had_reach_advanced
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(had_draw_record_reward_list);//had_draw_record_reward_list
        _size+=4;//total_gain_num
        _size+=4;//normal_pending_num
        _size+=4;//advanced_pending_num
        _size+=4;//normal_skill_level
        _size+=4;//normal_skill_point
        _size+=4;//advanced_skill_level
        _size+=4;//advanced_skill_point
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(ore_id);
        buff.putLong(first_gain_time_ms);
        buff.putInt(max_record);
        buff.putLong(reach_max_record_time_ms);
        buff.put((byte)(had_reach_advanced?1:0));
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, had_draw_record_reward_list);
        buff.putInt(total_gain_num);
        buff.putInt(normal_pending_num);
        buff.putInt(advanced_pending_num);
        buff.putInt(normal_skill_level);
        buff.putInt(normal_skill_point);
        buff.putInt(advanced_skill_level);
        buff.putInt(advanced_skill_point);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        ore_id=buff.getLong();
        first_gain_time_ms=buff.getLong();
        max_record=buff.getInt();
        reach_max_record_time_ms=buff.getLong();
        had_reach_advanced=(buff.get()==1);
        had_draw_record_reward_list=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        total_gain_num=buff.getInt();
        normal_pending_num=buff.getInt();
        advanced_pending_num=buff.getInt();
        normal_skill_level=buff.getInt();
        normal_skill_point=buff.getInt();
        advanced_skill_level=buff.getInt();
        advanced_skill_point=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
