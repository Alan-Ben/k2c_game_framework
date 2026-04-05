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
public class PlayerTreasureHuntGuaranteeBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_area_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "area_id", comment = "区域ID")
    private long area_id;

    public static final int FIELD_treasure_guarantee_count =2;
    @DataBaseField(type = "int(11)", fieldname = "treasure_guarantee_count", comment = "奇物保底计数")
    private int treasure_guarantee_count;

    public static final int FIELD_consecutive_high_quality_count =3;
    @DataBaseField(type = "int(11)", fieldname = "consecutive_high_quality_count", comment = "连续获得高品质矿石次数")
    private int consecutive_high_quality_count;

    public static final int FIELD_consecutive_no_high_quality_count =4;
    @DataBaseField(type = "int(11)", fieldname = "consecutive_no_high_quality_count", comment = "连续未获得高品质矿石次数")
    private int consecutive_no_high_quality_count;

    public PlayerTreasureHuntGuaranteeBO() {
        id = 0;
        cid = 0L;
        area_id = 0L;
        treasure_guarantee_count = 0;
        consecutive_high_quality_count = 0;
        consecutive_no_high_quality_count = 0;
    }

    public PlayerTreasureHuntGuaranteeBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        area_id = rs.getLong(3);
        treasure_guarantee_count = rs.getInt(4);
        consecutive_high_quality_count = rs.getInt(5);
        consecutive_no_high_quality_count = rs.getInt(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerTreasureHuntGuaranteeBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `area_id`, `treasure_guarantee_count`, `consecutive_high_quality_count`, `consecutive_no_high_quality_count`";
    }

    @Override
    public String getTableName() {
        return "`player_treasure_hunt_guarantee`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(area_id).append("', ");
        strBuf.append("'").append(treasure_guarantee_count).append("', ");
        strBuf.append("'").append(consecutive_high_quality_count).append("', ");
        strBuf.append("'").append(consecutive_no_high_quality_count).append("', ");
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

    // 区域ID
    public long getAreaId() { return this.area_id; }
    public void setAreaId(BM _bm, long area_id) {
        if(area_id==this.area_id) 
            return;
        this.area_id = area_id; 
        markField(_bm, FIELD_area_id); 
    }
    public void saveAreaId(BM _bm, long area_id) {
        if(area_id==this.area_id) 
            return;
        this.area_id = area_id;
        saveField(_bm, "area_id", area_id);
    }

    // 奇物保底计数
    public int getTreasureGuaranteeCount() { return this.treasure_guarantee_count; }
    public void setTreasureGuaranteeCount(BM _bm, int treasure_guarantee_count) {
        if(treasure_guarantee_count==this.treasure_guarantee_count) 
            return;
        this.treasure_guarantee_count = treasure_guarantee_count; 
        markField(_bm, FIELD_treasure_guarantee_count); 
    }
    public void saveTreasureGuaranteeCount(BM _bm, int treasure_guarantee_count) {
        if(treasure_guarantee_count==this.treasure_guarantee_count) 
            return;
        this.treasure_guarantee_count = treasure_guarantee_count;
        saveField(_bm, "treasure_guarantee_count", treasure_guarantee_count);
    }

    // 连续获得高品质矿石次数
    public int getConsecutiveHighQualityCount() { return this.consecutive_high_quality_count; }
    public void setConsecutiveHighQualityCount(BM _bm, int consecutive_high_quality_count) {
        if(consecutive_high_quality_count==this.consecutive_high_quality_count) 
            return;
        this.consecutive_high_quality_count = consecutive_high_quality_count; 
        markField(_bm, FIELD_consecutive_high_quality_count); 
    }
    public void saveConsecutiveHighQualityCount(BM _bm, int consecutive_high_quality_count) {
        if(consecutive_high_quality_count==this.consecutive_high_quality_count) 
            return;
        this.consecutive_high_quality_count = consecutive_high_quality_count;
        saveField(_bm, "consecutive_high_quality_count", consecutive_high_quality_count);
    }

    // 连续未获得高品质矿石次数
    public int getConsecutiveNoHighQualityCount() { return this.consecutive_no_high_quality_count; }
    public void setConsecutiveNoHighQualityCount(BM _bm, int consecutive_no_high_quality_count) {
        if(consecutive_no_high_quality_count==this.consecutive_no_high_quality_count) 
            return;
        this.consecutive_no_high_quality_count = consecutive_no_high_quality_count; 
        markField(_bm, FIELD_consecutive_no_high_quality_count); 
    }
    public void saveConsecutiveNoHighQualityCount(BM _bm, int consecutive_no_high_quality_count) {
        if(consecutive_no_high_quality_count==this.consecutive_no_high_quality_count) 
            return;
        this.consecutive_no_high_quality_count = consecutive_no_high_quality_count;
        saveField(_bm, "consecutive_no_high_quality_count", consecutive_no_high_quality_count);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `area_id` = '").append(area_id).append("',");
        sBuilder.append(" `treasure_guarantee_count` = '").append(treasure_guarantee_count).append("',");
        sBuilder.append(" `consecutive_high_quality_count` = '").append(consecutive_high_quality_count).append("',");
        sBuilder.append(" `consecutive_no_high_quality_count` = '").append(consecutive_no_high_quality_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_area_id)) sBuilder.append(" `area_id` = '").append(area_id).append("',");
        if(isFieldMarked(FIELD_treasure_guarantee_count)) sBuilder.append(" `treasure_guarantee_count` = '").append(treasure_guarantee_count).append("',");
        if(isFieldMarked(FIELD_consecutive_high_quality_count)) sBuilder.append(" `consecutive_high_quality_count` = '").append(consecutive_high_quality_count).append("',");
        if(isFieldMarked(FIELD_consecutive_no_high_quality_count)) sBuilder.append(" `consecutive_no_high_quality_count` = '").append(consecutive_no_high_quality_count).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_treasure_hunt_guarantee` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`area_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '区域ID',"
                + "`treasure_guarantee_count` int(11) NOT NULL DEFAULT '0' COMMENT '奇物保底计数',"
                + "`consecutive_high_quality_count` int(11) NOT NULL DEFAULT '0' COMMENT '连续获得高品质矿石次数',"
                + "`consecutive_no_high_quality_count` int(11) NOT NULL DEFAULT '0' COMMENT '连续未获得高品质矿石次数',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家太空寻宝保底数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//area_id
        _size+=4;//treasure_guarantee_count
        _size+=4;//consecutive_high_quality_count
        _size+=4;//consecutive_no_high_quality_count
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(area_id);
        buff.putInt(treasure_guarantee_count);
        buff.putInt(consecutive_high_quality_count);
        buff.putInt(consecutive_no_high_quality_count);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        area_id=buff.getLong();
        treasure_guarantee_count=buff.getInt();
        consecutive_high_quality_count=buff.getInt();
        consecutive_no_high_quality_count=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
