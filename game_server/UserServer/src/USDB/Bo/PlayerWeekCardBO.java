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
public class PlayerWeekCardBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_active_time_ms =1;
    @DataBaseField(type = "bigint(20)", fieldname = "active_time_ms", comment = "生效时间戳")
    private long active_time_ms;

    public static final int FIELD_expire_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "expire_time_ms", comment = "有效截止时间戳")
    private long expire_time_ms;

    public static final int FIELD_had_use_free_trial =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_use_free_trial", comment = "是否使用过免费试用")
    private boolean had_use_free_trial;

    public static final int FIELD_npc_type =4;
    @DataBaseField(type = "int(11)", fieldname = "npc_type", comment = "npc类型")
    private int npc_type;

    public static final int FIELD_npc_id =5;
    @DataBaseField(type = "bigint(20)", fieldname = "npc_id", comment = "npcID")
    private long npc_id;

    public PlayerWeekCardBO() {
        id = 0;
        cid = 0L;
        active_time_ms = 0L;
        expire_time_ms = 0L;
        had_use_free_trial = false;
        npc_type = 0;
        npc_id = 0L;
    }

    public PlayerWeekCardBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        active_time_ms = rs.getLong(3);
        expire_time_ms = rs.getLong(4);
        had_use_free_trial = rs.getBoolean(5);
        npc_type = rs.getInt(6);
        npc_id = rs.getLong(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerWeekCardBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `active_time_ms`, `expire_time_ms`, `had_use_free_trial`, `npc_type`, `npc_id`";
    }

    @Override
    public String getTableName() {
        return "`player_week_card`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(active_time_ms).append("', ");
        strBuf.append("'").append(expire_time_ms).append("', ");
        strBuf.append("'").append(had_use_free_trial ? 1 : 0).append("', ");
        strBuf.append("'").append(npc_type).append("', ");
        strBuf.append("'").append(npc_id).append("', ");
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

    // 生效时间戳
    public long getActiveTimeMs() { return this.active_time_ms; }
    public void setActiveTimeMs(BM _bm, long active_time_ms) {
        if(active_time_ms==this.active_time_ms) 
            return;
        this.active_time_ms = active_time_ms; 
        markField(_bm, FIELD_active_time_ms); 
    }
    public void saveActiveTimeMs(BM _bm, long active_time_ms) {
        if(active_time_ms==this.active_time_ms) 
            return;
        this.active_time_ms = active_time_ms;
        saveField(_bm, "active_time_ms", active_time_ms);
    }

    // 有效截止时间戳
    public long getExpireTimeMs() { return this.expire_time_ms; }
    public void setExpireTimeMs(BM _bm, long expire_time_ms) {
        if(expire_time_ms==this.expire_time_ms) 
            return;
        this.expire_time_ms = expire_time_ms; 
        markField(_bm, FIELD_expire_time_ms); 
    }
    public void saveExpireTimeMs(BM _bm, long expire_time_ms) {
        if(expire_time_ms==this.expire_time_ms) 
            return;
        this.expire_time_ms = expire_time_ms;
        saveField(_bm, "expire_time_ms", expire_time_ms);
    }

    // 是否使用过免费试用
    public boolean getHadUseFreeTrial() { return this.had_use_free_trial; }
    public void setHadUseFreeTrial(BM _bm, boolean had_use_free_trial) {
        if(had_use_free_trial==this.had_use_free_trial) 
            return;
        this.had_use_free_trial = had_use_free_trial; 
        markField(_bm, FIELD_had_use_free_trial); 
    }
    public void saveHadUseFreeTrial(BM _bm, boolean had_use_free_trial) {
        if(had_use_free_trial==this.had_use_free_trial) 
            return;
        this.had_use_free_trial = had_use_free_trial;
        saveField(_bm, "had_use_free_trial", had_use_free_trial ? 1 : 0);
    }

    // npc类型
    public int getNpcType() { return this.npc_type; }
    public void setNpcType(BM _bm, int npc_type) {
        if(npc_type==this.npc_type) 
            return;
        this.npc_type = npc_type; 
        markField(_bm, FIELD_npc_type); 
    }
    public void saveNpcType(BM _bm, int npc_type) {
        if(npc_type==this.npc_type) 
            return;
        this.npc_type = npc_type;
        saveField(_bm, "npc_type", npc_type);
    }

    // npcID
    public long getNpcId() { return this.npc_id; }
    public void setNpcId(BM _bm, long npc_id) {
        if(npc_id==this.npc_id) 
            return;
        this.npc_id = npc_id; 
        markField(_bm, FIELD_npc_id); 
    }
    public void saveNpcId(BM _bm, long npc_id) {
        if(npc_id==this.npc_id) 
            return;
        this.npc_id = npc_id;
        saveField(_bm, "npc_id", npc_id);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `active_time_ms` = '").append(active_time_ms).append("',");
        sBuilder.append(" `expire_time_ms` = '").append(expire_time_ms).append("',");
        sBuilder.append(" `had_use_free_trial` = '").append(had_use_free_trial ? 1 : 0).append("',");
        sBuilder.append(" `npc_type` = '").append(npc_type).append("',");
        sBuilder.append(" `npc_id` = '").append(npc_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_active_time_ms)) sBuilder.append(" `active_time_ms` = '").append(active_time_ms).append("',");
        if(isFieldMarked(FIELD_expire_time_ms)) sBuilder.append(" `expire_time_ms` = '").append(expire_time_ms).append("',");
        if(isFieldMarked(FIELD_had_use_free_trial)) sBuilder.append(" `had_use_free_trial` = '").append(had_use_free_trial ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_npc_type)) sBuilder.append(" `npc_type` = '").append(npc_type).append("',");
        if(isFieldMarked(FIELD_npc_id)) sBuilder.append(" `npc_id` = '").append(npc_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_week_card` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`active_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '生效时间戳',"
                + "`expire_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '有效截止时间戳',"
                + "`had_use_free_trial` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否使用过免费试用',"
                + "`npc_type` int(11) NOT NULL DEFAULT '0' COMMENT 'npc类型',"
                + "`npc_id` bigint(20) NOT NULL DEFAULT '0' COMMENT 'npcID',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家周卡数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//active_time_ms
        _size+=8;//expire_time_ms
        _size+=1;//had_use_free_trial
        _size+=4;//npc_type
        _size+=8;//npc_id
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(active_time_ms);
        buff.putLong(expire_time_ms);
        buff.put((byte)(had_use_free_trial?1:0));
        buff.putInt(npc_type);
        buff.putLong(npc_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        active_time_ms=buff.getLong();
        expire_time_ms=buff.getLong();
        had_use_free_trial=(buff.get()==1);
        npc_type=buff.getInt();
        npc_id=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
