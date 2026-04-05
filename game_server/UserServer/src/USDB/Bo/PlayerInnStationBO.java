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
public class PlayerInnStationBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_station_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "station_id", comment = "设施ID")
    private long station_id;

    public static final int FIELD_level =2;
    @DataBaseField(type = "int(11)", fieldname = "level", comment = "等级")
    private int level;

    public PlayerInnStationBO() {
        id = 0;
        cid = 0L;
        station_id = 0L;
        level = 0;
    }

    public PlayerInnStationBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        station_id = rs.getLong(3);
        level = rs.getInt(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerInnStationBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `station_id`, `level`";
    }

    @Override
    public String getTableName() {
        return "`player_inn_station`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(station_id).append("', ");
        strBuf.append("'").append(level).append("', ");
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

    // 设施ID
    public long getStationId() { return this.station_id; }
    public void setStationId(BM _bm, long station_id) {
        if(station_id==this.station_id) 
            return;
        this.station_id = station_id; 
        markField(_bm, FIELD_station_id); 
    }
    public void saveStationId(BM _bm, long station_id) {
        if(station_id==this.station_id) 
            return;
        this.station_id = station_id;
        saveField(_bm, "station_id", station_id);
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



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `station_id` = '").append(station_id).append("',");
        sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_station_id)) sBuilder.append(" `station_id` = '").append(station_id).append("',");
        if(isFieldMarked(FIELD_level)) sBuilder.append(" `level` = '").append(level).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_inn_station` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`station_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '设施ID',"
                + "`level` int(11) NOT NULL DEFAULT '0' COMMENT '等级',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家旅店 设施数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//station_id
        _size+=4;//level
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(station_id);
        buff.putInt(level);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        station_id=buff.getLong();
        level=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
