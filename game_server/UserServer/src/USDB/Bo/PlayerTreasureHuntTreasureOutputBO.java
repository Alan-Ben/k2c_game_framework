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
public class PlayerTreasureHuntTreasureOutputBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_treasure_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "treasure_id", comment = "奇物ID")
    private long treasure_id;

    public static final int FIELD_next_can_draw_time_ms =2;
    @DataBaseField(type = "bigint(20)", fieldname = "next_can_draw_time_ms", comment = "下次可领取时间")
    private long next_can_draw_time_ms;

    public static final int FIELD_next_can_draw_num =3;
    @DataBaseField(type = "int(11)", fieldname = "next_can_draw_num", comment = "下次可领取数量")
    private int next_can_draw_num;

    public PlayerTreasureHuntTreasureOutputBO() {
        id = 0;
        cid = 0L;
        treasure_id = 0L;
        next_can_draw_time_ms = 0L;
        next_can_draw_num = 0;
    }

    public PlayerTreasureHuntTreasureOutputBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        treasure_id = rs.getLong(3);
        next_can_draw_time_ms = rs.getLong(4);
        next_can_draw_num = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerTreasureHuntTreasureOutputBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `treasure_id`, `next_can_draw_time_ms`, `next_can_draw_num`";
    }

    @Override
    public String getTableName() {
        return "`player_treasure_hunt_treasure_output`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(treasure_id).append("', ");
        strBuf.append("'").append(next_can_draw_time_ms).append("', ");
        strBuf.append("'").append(next_can_draw_num).append("', ");
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

    // 奇物ID
    public long getTreasureId() { return this.treasure_id; }
    public void setTreasureId(BM _bm, long treasure_id) {
        if(treasure_id==this.treasure_id) 
            return;
        this.treasure_id = treasure_id; 
        markField(_bm, FIELD_treasure_id); 
    }
    public void saveTreasureId(BM _bm, long treasure_id) {
        if(treasure_id==this.treasure_id) 
            return;
        this.treasure_id = treasure_id;
        saveField(_bm, "treasure_id", treasure_id);
    }

    // 下次可领取时间
    public long getNextCanDrawTimeMs() { return this.next_can_draw_time_ms; }
    public void setNextCanDrawTimeMs(BM _bm, long next_can_draw_time_ms) {
        if(next_can_draw_time_ms==this.next_can_draw_time_ms) 
            return;
        this.next_can_draw_time_ms = next_can_draw_time_ms; 
        markField(_bm, FIELD_next_can_draw_time_ms); 
    }
    public void saveNextCanDrawTimeMs(BM _bm, long next_can_draw_time_ms) {
        if(next_can_draw_time_ms==this.next_can_draw_time_ms) 
            return;
        this.next_can_draw_time_ms = next_can_draw_time_ms;
        saveField(_bm, "next_can_draw_time_ms", next_can_draw_time_ms);
    }

    // 下次可领取数量
    public int getNextCanDrawNum() { return this.next_can_draw_num; }
    public void setNextCanDrawNum(BM _bm, int next_can_draw_num) {
        if(next_can_draw_num==this.next_can_draw_num) 
            return;
        this.next_can_draw_num = next_can_draw_num; 
        markField(_bm, FIELD_next_can_draw_num); 
    }
    public void saveNextCanDrawNum(BM _bm, int next_can_draw_num) {
        if(next_can_draw_num==this.next_can_draw_num) 
            return;
        this.next_can_draw_num = next_can_draw_num;
        saveField(_bm, "next_can_draw_num", next_can_draw_num);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `treasure_id` = '").append(treasure_id).append("',");
        sBuilder.append(" `next_can_draw_time_ms` = '").append(next_can_draw_time_ms).append("',");
        sBuilder.append(" `next_can_draw_num` = '").append(next_can_draw_num).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_treasure_id)) sBuilder.append(" `treasure_id` = '").append(treasure_id).append("',");
        if(isFieldMarked(FIELD_next_can_draw_time_ms)) sBuilder.append(" `next_can_draw_time_ms` = '").append(next_can_draw_time_ms).append("',");
        if(isFieldMarked(FIELD_next_can_draw_num)) sBuilder.append(" `next_can_draw_num` = '").append(next_can_draw_num).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_treasure_hunt_treasure_output` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`treasure_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '奇物ID',"
                + "`next_can_draw_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '下次可领取时间',"
                + "`next_can_draw_num` int(11) NOT NULL DEFAULT '0' COMMENT '下次可领取数量',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家太空寻宝奇物产出数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//treasure_id
        _size+=8;//next_can_draw_time_ms
        _size+=4;//next_can_draw_num
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(treasure_id);
        buff.putLong(next_can_draw_time_ms);
        buff.putInt(next_can_draw_num);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        treasure_id=buff.getLong();
        next_can_draw_time_ms=buff.getLong();
        next_can_draw_num=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
