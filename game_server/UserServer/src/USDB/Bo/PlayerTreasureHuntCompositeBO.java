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
public class PlayerTreasureHuntCompositeBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_composite_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "composite_id", comment = "组合id")
    private long composite_id;

    public static final int FIELD_collect_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "collect_time_ms", comment = "收集时间 ms")
    private long collect_time_ms;

    public static final int FIELD_is_normal_skill_active =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_normal_skill_active", comment = "是否激活普通技能")
    private boolean is_normal_skill_active;

    public static final int FIELD_is_advanced_skill_active =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_advanced_skill_active", comment = "是否激活高级技能")
    private boolean is_advanced_skill_active;

    public PlayerTreasureHuntCompositeBO() {
        id = 0;
        cid = 0L;
        composite_id = 0L;
        collect_time_ms = 0L;
        is_normal_skill_active = false;
        is_advanced_skill_active = false;
    }

    public PlayerTreasureHuntCompositeBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        composite_id = rs.getLong(3);
        collect_time_ms = rs.getLong(4);
        is_normal_skill_active = rs.getBoolean(5);
        is_advanced_skill_active = rs.getBoolean(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerTreasureHuntCompositeBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `composite_id`, `collect_time_ms`, `is_normal_skill_active`, `is_advanced_skill_active`";
    }

    @Override
    public String getTableName() {
        return "`player_treasure_hunt_composite`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(composite_id).append("', ");
        strBuf.append("'").append(collect_time_ms).append("', ");
        strBuf.append("'").append(is_normal_skill_active ? 1 : 0).append("', ");
        strBuf.append("'").append(is_advanced_skill_active ? 1 : 0).append("', ");
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

    // 组合id
    public long getCompositeId() { return this.composite_id; }
    public void setCompositeId(BM _bm, long composite_id) {
        if(composite_id==this.composite_id) 
            return;
        this.composite_id = composite_id; 
        markField(_bm, FIELD_composite_id); 
    }
    public void saveCompositeId(BM _bm, long composite_id) {
        if(composite_id==this.composite_id) 
            return;
        this.composite_id = composite_id;
        saveField(_bm, "composite_id", composite_id);
    }

    // 收集时间 ms
    public long getCollectTimeMs() { return this.collect_time_ms; }
    public void setCollectTimeMs(BM _bm, long collect_time_ms) {
        if(collect_time_ms==this.collect_time_ms) 
            return;
        this.collect_time_ms = collect_time_ms; 
        markField(_bm, FIELD_collect_time_ms); 
    }
    public void saveCollectTimeMs(BM _bm, long collect_time_ms) {
        if(collect_time_ms==this.collect_time_ms) 
            return;
        this.collect_time_ms = collect_time_ms;
        saveField(_bm, "collect_time_ms", collect_time_ms);
    }

    // 是否激活普通技能
    public boolean getIsNormalSkillActive() { return this.is_normal_skill_active; }
    public void setIsNormalSkillActive(BM _bm, boolean is_normal_skill_active) {
        if(is_normal_skill_active==this.is_normal_skill_active) 
            return;
        this.is_normal_skill_active = is_normal_skill_active; 
        markField(_bm, FIELD_is_normal_skill_active); 
    }
    public void saveIsNormalSkillActive(BM _bm, boolean is_normal_skill_active) {
        if(is_normal_skill_active==this.is_normal_skill_active) 
            return;
        this.is_normal_skill_active = is_normal_skill_active;
        saveField(_bm, "is_normal_skill_active", is_normal_skill_active ? 1 : 0);
    }

    // 是否激活高级技能
    public boolean getIsAdvancedSkillActive() { return this.is_advanced_skill_active; }
    public void setIsAdvancedSkillActive(BM _bm, boolean is_advanced_skill_active) {
        if(is_advanced_skill_active==this.is_advanced_skill_active) 
            return;
        this.is_advanced_skill_active = is_advanced_skill_active; 
        markField(_bm, FIELD_is_advanced_skill_active); 
    }
    public void saveIsAdvancedSkillActive(BM _bm, boolean is_advanced_skill_active) {
        if(is_advanced_skill_active==this.is_advanced_skill_active) 
            return;
        this.is_advanced_skill_active = is_advanced_skill_active;
        saveField(_bm, "is_advanced_skill_active", is_advanced_skill_active ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `composite_id` = '").append(composite_id).append("',");
        sBuilder.append(" `collect_time_ms` = '").append(collect_time_ms).append("',");
        sBuilder.append(" `is_normal_skill_active` = '").append(is_normal_skill_active ? 1 : 0).append("',");
        sBuilder.append(" `is_advanced_skill_active` = '").append(is_advanced_skill_active ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_composite_id)) sBuilder.append(" `composite_id` = '").append(composite_id).append("',");
        if(isFieldMarked(FIELD_collect_time_ms)) sBuilder.append(" `collect_time_ms` = '").append(collect_time_ms).append("',");
        if(isFieldMarked(FIELD_is_normal_skill_active)) sBuilder.append(" `is_normal_skill_active` = '").append(is_normal_skill_active ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_is_advanced_skill_active)) sBuilder.append(" `is_advanced_skill_active` = '").append(is_advanced_skill_active ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_treasure_hunt_composite` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`composite_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '组合id',"
                + "`collect_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '收集时间 ms',"
                + "`is_normal_skill_active` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否激活普通技能',"
                + "`is_advanced_skill_active` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否激活高级技能',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家太空寻宝组合数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//composite_id
        _size+=8;//collect_time_ms
        _size+=1;//is_normal_skill_active
        _size+=1;//is_advanced_skill_active
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(composite_id);
        buff.putLong(collect_time_ms);
        buff.put((byte)(is_normal_skill_active?1:0));
        buff.put((byte)(is_advanced_skill_active?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        composite_id=buff.getLong();
        collect_time_ms=buff.getLong();
        is_normal_skill_active=(buff.get()==1);
        is_advanced_skill_active=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
