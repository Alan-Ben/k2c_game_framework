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
public class PlayerTargetRewardBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_ref_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "ref_id", comment = "目标ID")
    private long ref_id;

    public static final int FIELD_extra_count =2;
    @DataBaseField(type = "bigint(20)", fieldname = "extra_count", comment = "额外计数")
    private long extra_count;

    public static final int FIELD_activity_instance_id =3;
    @DataBaseField(type = "bigint(20)", fieldname = "activity_instance_id", comment = "活动实例id")
    private long activity_instance_id;

    public static final int FIELD_had_draw =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_draw", comment = "是否已领取")
    private boolean had_draw;

    public PlayerTargetRewardBO() {
        id = 0;
        cid = 0L;
        ref_id = 0L;
        extra_count = 0L;
        activity_instance_id = 0L;
        had_draw = false;
    }

    public PlayerTargetRewardBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        ref_id = rs.getLong(3);
        extra_count = rs.getLong(4);
        activity_instance_id = rs.getLong(5);
        had_draw = rs.getBoolean(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerTargetRewardBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `ref_id`, `extra_count`, `activity_instance_id`, `had_draw`";
    }

    @Override
    public String getTableName() {
        return "`player_target_reward`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(ref_id).append("', ");
        strBuf.append("'").append(extra_count).append("', ");
        strBuf.append("'").append(activity_instance_id).append("', ");
        strBuf.append("'").append(had_draw ? 1 : 0).append("', ");
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

    // 目标ID
    public long getRefId() { return this.ref_id; }
    public void setRefId(BM _bm, long ref_id) {
        if(ref_id==this.ref_id) 
            return;
        this.ref_id = ref_id; 
        markField(_bm, FIELD_ref_id); 
    }
    public void saveRefId(BM _bm, long ref_id) {
        if(ref_id==this.ref_id) 
            return;
        this.ref_id = ref_id;
        saveField(_bm, "ref_id", ref_id);
    }

    // 额外计数
    public long getExtraCount() { return this.extra_count; }
    public void setExtraCount(BM _bm, long extra_count) {
        if(extra_count==this.extra_count) 
            return;
        this.extra_count = extra_count; 
        markField(_bm, FIELD_extra_count); 
    }
    public void saveExtraCount(BM _bm, long extra_count) {
        if(extra_count==this.extra_count) 
            return;
        this.extra_count = extra_count;
        saveField(_bm, "extra_count", extra_count);
    }

    // 活动实例id
    public long getActivityInstanceId() { return this.activity_instance_id; }
    public void setActivityInstanceId(BM _bm, long activity_instance_id) {
        if(activity_instance_id==this.activity_instance_id) 
            return;
        this.activity_instance_id = activity_instance_id; 
        markField(_bm, FIELD_activity_instance_id); 
    }
    public void saveActivityInstanceId(BM _bm, long activity_instance_id) {
        if(activity_instance_id==this.activity_instance_id) 
            return;
        this.activity_instance_id = activity_instance_id;
        saveField(_bm, "activity_instance_id", activity_instance_id);
    }

    // 是否已领取
    public boolean getHadDraw() { return this.had_draw; }
    public void setHadDraw(BM _bm, boolean had_draw) {
        if(had_draw==this.had_draw) 
            return;
        this.had_draw = had_draw; 
        markField(_bm, FIELD_had_draw); 
    }
    public void saveHadDraw(BM _bm, boolean had_draw) {
        if(had_draw==this.had_draw) 
            return;
        this.had_draw = had_draw;
        saveField(_bm, "had_draw", had_draw ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `ref_id` = '").append(ref_id).append("',");
        sBuilder.append(" `extra_count` = '").append(extra_count).append("',");
        sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        sBuilder.append(" `had_draw` = '").append(had_draw ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_ref_id)) sBuilder.append(" `ref_id` = '").append(ref_id).append("',");
        if(isFieldMarked(FIELD_extra_count)) sBuilder.append(" `extra_count` = '").append(extra_count).append("',");
        if(isFieldMarked(FIELD_activity_instance_id)) sBuilder.append(" `activity_instance_id` = '").append(activity_instance_id).append("',");
        if(isFieldMarked(FIELD_had_draw)) sBuilder.append(" `had_draw` = '").append(had_draw ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_target_reward` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`ref_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '目标ID',"
                + "`extra_count` bigint(20) NOT NULL DEFAULT '0' COMMENT '额外计数',"
                + "`activity_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例id',"
                + "`had_draw` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已领取',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家目标奖励数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//ref_id
        _size+=8;//extra_count
        _size+=8;//activity_instance_id
        _size+=1;//had_draw
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(ref_id);
        buff.putLong(extra_count);
        buff.putLong(activity_instance_id);
        buff.put((byte)(had_draw?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        ref_id=buff.getLong();
        extra_count=buff.getLong();
        activity_instance_id=buff.getLong();
        had_draw=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
