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
public class PlayerGiftPackRecordBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_gift_pack_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "gift_pack_id", comment = "礼包id")
    private long gift_pack_id;

    public static final int FIELD_client_buy_count =2;
    @DataBaseField(type = "int(11)", fieldname = "client_buy_count", comment = "客户端购买次数")
    private int client_buy_count;

    public static final int FIELD_buy_count =3;
    @DataBaseField(type = "int(11)", fieldname = "buy_count", comment = "购买次数")
    private int buy_count;

    public static final int FIELD_next_refresh_time_ms =4;
    @DataBaseField(type = "bigint(20)", fieldname = "next_refresh_time_ms", comment = "下次刷新时间")
    private long next_refresh_time_ms;

    public static final int FIELD_relative_life_cycle_instance_id =5;
    @DataBaseField(type = "bigint(20)", fieldname = "relative_life_cycle_instance_id", comment = "活动实例id")
    private long relative_life_cycle_instance_id;

    public PlayerGiftPackRecordBO() {
        id = 0;
        cid = 0L;
        gift_pack_id = 0L;
        client_buy_count = 0;
        buy_count = 0;
        next_refresh_time_ms = 0L;
        relative_life_cycle_instance_id = 0L;
    }

    public PlayerGiftPackRecordBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        gift_pack_id = rs.getLong(3);
        client_buy_count = rs.getInt(4);
        buy_count = rs.getInt(5);
        next_refresh_time_ms = rs.getLong(6);
        relative_life_cycle_instance_id = rs.getLong(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerGiftPackRecordBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `gift_pack_id`, `client_buy_count`, `buy_count`, `next_refresh_time_ms`, `relative_life_cycle_instance_id`";
    }

    @Override
    public String getTableName() {
        return "`player_gift_pack_record`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(gift_pack_id).append("', ");
        strBuf.append("'").append(client_buy_count).append("', ");
        strBuf.append("'").append(buy_count).append("', ");
        strBuf.append("'").append(next_refresh_time_ms).append("', ");
        strBuf.append("'").append(relative_life_cycle_instance_id).append("', ");
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

    // 礼包id
    public long getGiftPackId() { return this.gift_pack_id; }
    public void setGiftPackId(BM _bm, long gift_pack_id) {
        if(gift_pack_id==this.gift_pack_id) 
            return;
        this.gift_pack_id = gift_pack_id; 
        markField(_bm, FIELD_gift_pack_id); 
    }
    public void saveGiftPackId(BM _bm, long gift_pack_id) {
        if(gift_pack_id==this.gift_pack_id) 
            return;
        this.gift_pack_id = gift_pack_id;
        saveField(_bm, "gift_pack_id", gift_pack_id);
    }

    // 客户端购买次数
    public int getClientBuyCount() { return this.client_buy_count; }
    public void setClientBuyCount(BM _bm, int client_buy_count) {
        if(client_buy_count==this.client_buy_count) 
            return;
        this.client_buy_count = client_buy_count; 
        markField(_bm, FIELD_client_buy_count); 
    }
    public void saveClientBuyCount(BM _bm, int client_buy_count) {
        if(client_buy_count==this.client_buy_count) 
            return;
        this.client_buy_count = client_buy_count;
        saveField(_bm, "client_buy_count", client_buy_count);
    }

    // 购买次数
    public int getBuyCount() { return this.buy_count; }
    public void setBuyCount(BM _bm, int buy_count) {
        if(buy_count==this.buy_count) 
            return;
        this.buy_count = buy_count; 
        markField(_bm, FIELD_buy_count); 
    }
    public void saveBuyCount(BM _bm, int buy_count) {
        if(buy_count==this.buy_count) 
            return;
        this.buy_count = buy_count;
        saveField(_bm, "buy_count", buy_count);
    }

    // 下次刷新时间
    public long getNextRefreshTimeMs() { return this.next_refresh_time_ms; }
    public void setNextRefreshTimeMs(BM _bm, long next_refresh_time_ms) {
        if(next_refresh_time_ms==this.next_refresh_time_ms) 
            return;
        this.next_refresh_time_ms = next_refresh_time_ms; 
        markField(_bm, FIELD_next_refresh_time_ms); 
    }
    public void saveNextRefreshTimeMs(BM _bm, long next_refresh_time_ms) {
        if(next_refresh_time_ms==this.next_refresh_time_ms) 
            return;
        this.next_refresh_time_ms = next_refresh_time_ms;
        saveField(_bm, "next_refresh_time_ms", next_refresh_time_ms);
    }

    // 活动实例id
    public long getRelativeLifeCycleInstanceId() { return this.relative_life_cycle_instance_id; }
    public void setRelativeLifeCycleInstanceId(BM _bm, long relative_life_cycle_instance_id) {
        if(relative_life_cycle_instance_id==this.relative_life_cycle_instance_id) 
            return;
        this.relative_life_cycle_instance_id = relative_life_cycle_instance_id; 
        markField(_bm, FIELD_relative_life_cycle_instance_id); 
    }
    public void saveRelativeLifeCycleInstanceId(BM _bm, long relative_life_cycle_instance_id) {
        if(relative_life_cycle_instance_id==this.relative_life_cycle_instance_id) 
            return;
        this.relative_life_cycle_instance_id = relative_life_cycle_instance_id;
        saveField(_bm, "relative_life_cycle_instance_id", relative_life_cycle_instance_id);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `gift_pack_id` = '").append(gift_pack_id).append("',");
        sBuilder.append(" `client_buy_count` = '").append(client_buy_count).append("',");
        sBuilder.append(" `buy_count` = '").append(buy_count).append("',");
        sBuilder.append(" `next_refresh_time_ms` = '").append(next_refresh_time_ms).append("',");
        sBuilder.append(" `relative_life_cycle_instance_id` = '").append(relative_life_cycle_instance_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_gift_pack_id)) sBuilder.append(" `gift_pack_id` = '").append(gift_pack_id).append("',");
        if(isFieldMarked(FIELD_client_buy_count)) sBuilder.append(" `client_buy_count` = '").append(client_buy_count).append("',");
        if(isFieldMarked(FIELD_buy_count)) sBuilder.append(" `buy_count` = '").append(buy_count).append("',");
        if(isFieldMarked(FIELD_next_refresh_time_ms)) sBuilder.append(" `next_refresh_time_ms` = '").append(next_refresh_time_ms).append("',");
        if(isFieldMarked(FIELD_relative_life_cycle_instance_id)) sBuilder.append(" `relative_life_cycle_instance_id` = '").append(relative_life_cycle_instance_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_gift_pack_record` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`gift_pack_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '礼包id',"
                + "`client_buy_count` int(11) NOT NULL DEFAULT '0' COMMENT '客户端购买次数',"
                + "`buy_count` int(11) NOT NULL DEFAULT '0' COMMENT '购买次数',"
                + "`next_refresh_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '下次刷新时间',"
                + "`relative_life_cycle_instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '活动实例id',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家礼包购买记录' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//gift_pack_id
        _size+=4;//client_buy_count
        _size+=4;//buy_count
        _size+=8;//next_refresh_time_ms
        _size+=8;//relative_life_cycle_instance_id
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(gift_pack_id);
        buff.putInt(client_buy_count);
        buff.putInt(buy_count);
        buff.putLong(next_refresh_time_ms);
        buff.putLong(relative_life_cycle_instance_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        gift_pack_id=buff.getLong();
        client_buy_count=buff.getInt();
        buy_count=buff.getInt();
        next_refresh_time_ms=buff.getLong();
        relative_life_cycle_instance_id=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
