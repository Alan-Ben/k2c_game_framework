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
public class PlayerGachaRecordBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家账号ID")
    private long cid;

    public static final int FIELD_pool_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "pool_id", comment = "奖池id")
    private long pool_id;

    public static final int FIELD_item_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "item_id", comment = "奖品id")
    private long item_id;

    public static final int FIELD_roll_time_sec =3;
    @DataBaseField(type = "int(11)", fieldname = "roll_time_sec", comment = "抽奖时间 秒")
    private int roll_time_sec;

    public PlayerGachaRecordBO() {
        id = 0;
        cid = 0L;
        pool_id = 0L;
        item_id = 0L;
        roll_time_sec = 0;
    }

    public PlayerGachaRecordBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        pool_id = rs.getLong(3);
        item_id = rs.getLong(4);
        roll_time_sec = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerGachaRecordBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `pool_id`, `item_id`, `roll_time_sec`";
    }

    @Override
    public String getTableName() {
        return "`player_gacha_record`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(pool_id).append("', ");
        strBuf.append("'").append(item_id).append("', ");
        strBuf.append("'").append(roll_time_sec).append("', ");
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

    // 玩家账号ID
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

    // 奖池id
    public long getPoolId() { return this.pool_id; }
    public void setPoolId(BM _bm, long pool_id) {
        if(pool_id==this.pool_id) 
            return;
        this.pool_id = pool_id; 
        markField(_bm, FIELD_pool_id); 
    }
    public void savePoolId(BM _bm, long pool_id) {
        if(pool_id==this.pool_id) 
            return;
        this.pool_id = pool_id;
        saveField(_bm, "pool_id", pool_id);
    }

    // 奖品id
    public long getItemId() { return this.item_id; }
    public void setItemId(BM _bm, long item_id) {
        if(item_id==this.item_id) 
            return;
        this.item_id = item_id; 
        markField(_bm, FIELD_item_id); 
    }
    public void saveItemId(BM _bm, long item_id) {
        if(item_id==this.item_id) 
            return;
        this.item_id = item_id;
        saveField(_bm, "item_id", item_id);
    }

    // 抽奖时间 秒
    public int getRollTimeSec() { return this.roll_time_sec; }
    public void setRollTimeSec(BM _bm, int roll_time_sec) {
        if(roll_time_sec==this.roll_time_sec) 
            return;
        this.roll_time_sec = roll_time_sec; 
        markField(_bm, FIELD_roll_time_sec); 
    }
    public void saveRollTimeSec(BM _bm, int roll_time_sec) {
        if(roll_time_sec==this.roll_time_sec) 
            return;
        this.roll_time_sec = roll_time_sec;
        saveField(_bm, "roll_time_sec", roll_time_sec);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `pool_id` = '").append(pool_id).append("',");
        sBuilder.append(" `item_id` = '").append(item_id).append("',");
        sBuilder.append(" `roll_time_sec` = '").append(roll_time_sec).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_pool_id)) sBuilder.append(" `pool_id` = '").append(pool_id).append("',");
        if(isFieldMarked(FIELD_item_id)) sBuilder.append(" `item_id` = '").append(item_id).append("',");
        if(isFieldMarked(FIELD_roll_time_sec)) sBuilder.append(" `roll_time_sec` = '").append(roll_time_sec).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_gacha_record` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家账号ID',"
                + "`pool_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '奖池id',"
                + "`item_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '奖品id',"
                + "`roll_time_sec` int(11) NOT NULL DEFAULT '0' COMMENT '抽奖时间 秒',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家抽卡记录信息' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//pool_id
        _size+=8;//item_id
        _size+=4;//roll_time_sec
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(pool_id);
        buff.putLong(item_id);
        buff.putInt(roll_time_sec);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        pool_id=buff.getLong();
        item_id=buff.getLong();
        roll_time_sec=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
