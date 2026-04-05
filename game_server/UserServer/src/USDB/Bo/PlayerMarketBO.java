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
public class PlayerMarketBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_market_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "market_id", comment = "集市ID")
    private long market_id;

    public static final int FIELD_market_lvl =2;
    @DataBaseField(type = "bigint(20)", fieldname = "market_lvl", comment = "集市等级")
    private long market_lvl;

    public static final int FIELD_count =3;
    @DataBaseField(type = "int(11)", fieldname = "count", comment = "当前操作次数")
    private int count;

    public static final int FIELD_last_operate_ms =4;
    @DataBaseField(type = "bigint(20)", fieldname = "last_operate_ms", comment = " 上次经营时间（毫秒）")
    private long last_operate_ms;

    public PlayerMarketBO() {
        id = 0;
        cid = 0L;
        market_id = 0L;
        market_lvl = 0L;
        count = 0;
        last_operate_ms = 0L;
    }

    public PlayerMarketBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        market_id = rs.getLong(3);
        market_lvl = rs.getLong(4);
        count = rs.getInt(5);
        last_operate_ms = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarketBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `market_id`, `market_lvl`, `count`, `last_operate_ms`";
    }

    @Override
    public String getTableName() {
        return "`player_market`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(market_id).append("', ");
        strBuf.append("'").append(market_lvl).append("', ");
        strBuf.append("'").append(count).append("', ");
        strBuf.append("'").append(last_operate_ms).append("', ");
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

    // 集市ID
    public long getMarketId() { return this.market_id; }
    public void setMarketId(BM _bm, long market_id) {
        if(market_id==this.market_id) 
            return;
        this.market_id = market_id; 
        markField(_bm, FIELD_market_id); 
    }
    public void saveMarketId(BM _bm, long market_id) {
        if(market_id==this.market_id) 
            return;
        this.market_id = market_id;
        saveField(_bm, "market_id", market_id);
    }

    // 集市等级
    public long getMarketLvl() { return this.market_lvl; }
    public void setMarketLvl(BM _bm, long market_lvl) {
        if(market_lvl==this.market_lvl) 
            return;
        this.market_lvl = market_lvl; 
        markField(_bm, FIELD_market_lvl); 
    }
    public void saveMarketLvl(BM _bm, long market_lvl) {
        if(market_lvl==this.market_lvl) 
            return;
        this.market_lvl = market_lvl;
        saveField(_bm, "market_lvl", market_lvl);
    }

    // 当前操作次数
    public int getCount() { return this.count; }
    public void setCount(BM _bm, int count) {
        if(count==this.count) 
            return;
        this.count = count; 
        markField(_bm, FIELD_count); 
    }
    public void saveCount(BM _bm, int count) {
        if(count==this.count) 
            return;
        this.count = count;
        saveField(_bm, "count", count);
    }

    //  上次经营时间（毫秒）
    public long getLastOperateMs() { return this.last_operate_ms; }
    public void setLastOperateMs(BM _bm, long last_operate_ms) {
        if(last_operate_ms==this.last_operate_ms) 
            return;
        this.last_operate_ms = last_operate_ms; 
        markField(_bm, FIELD_last_operate_ms); 
    }
    public void saveLastOperateMs(BM _bm, long last_operate_ms) {
        if(last_operate_ms==this.last_operate_ms) 
            return;
        this.last_operate_ms = last_operate_ms;
        saveField(_bm, "last_operate_ms", last_operate_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `market_id` = '").append(market_id).append("',");
        sBuilder.append(" `market_lvl` = '").append(market_lvl).append("',");
        sBuilder.append(" `count` = '").append(count).append("',");
        sBuilder.append(" `last_operate_ms` = '").append(last_operate_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_market_id)) sBuilder.append(" `market_id` = '").append(market_id).append("',");
        if(isFieldMarked(FIELD_market_lvl)) sBuilder.append(" `market_lvl` = '").append(market_lvl).append("',");
        if(isFieldMarked(FIELD_count)) sBuilder.append(" `count` = '").append(count).append("',");
        if(isFieldMarked(FIELD_last_operate_ms)) sBuilder.append(" `last_operate_ms` = '").append(last_operate_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_market` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`market_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '集市ID',"
                + "`market_lvl` bigint(20) NOT NULL DEFAULT '0' COMMENT '集市等级',"
                + "`count` int(11) NOT NULL DEFAULT '0' COMMENT '当前操作次数',"
                + "`last_operate_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT ' 上次经营时间（毫秒）',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='Player Market 玩家集市数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//market_id
        _size+=8;//market_lvl
        _size+=4;//count
        _size+=8;//last_operate_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(market_id);
        buff.putLong(market_lvl);
        buff.putInt(count);
        buff.putLong(last_operate_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        market_id=buff.getLong();
        market_lvl=buff.getLong();
        count=buff.getInt();
        last_operate_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
