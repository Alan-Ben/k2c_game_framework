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
public class PlayerWeekCardSettingBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_settle_type =1;
    @DataBaseField(type = "int(11)", fieldname = "settle_type", comment = "处理类型")
    private int settle_type;

    public static final int FIELD_extra_info =2;
    @DataBaseField(type = "blob", fieldname = "extra_info", comment = "额外信息")
    private byte[] extra_info;

    public PlayerWeekCardSettingBO() {
        id = 0;
        cid = 0L;
        settle_type = 0;
        extra_info = null;
    }

    public PlayerWeekCardSettingBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        settle_type = rs.getInt(3);
        extra_info = rs.getBytes(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerWeekCardSettingBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `settle_type`, `extra_info`";
    }

    @Override
    public String getTableName() {
        return "`player_week_card_setting`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(settle_type).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(extra_info);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_extra_info)) ret.add(extra_info);         return ret;
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

    // 处理类型
    public int getSettleType() { return this.settle_type; }
    public void setSettleType(BM _bm, int settle_type) {
        if(settle_type==this.settle_type) 
            return;
        this.settle_type = settle_type; 
        markField(_bm, FIELD_settle_type); 
    }
    public void saveSettleType(BM _bm, int settle_type) {
        if(settle_type==this.settle_type) 
            return;
        this.settle_type = settle_type;
        saveField(_bm, "settle_type", settle_type);
    }

    // 额外信息
    public byte[] getExtraInfo() { return this.extra_info; }
    public void setExtraInfo(BM _bm, byte[] extra_info) {
        if(extra_info==this.extra_info) 
            return;
        this.extra_info = extra_info; 
        markField(_bm, FIELD_extra_info); 
    }
    public void saveExtraInfo(BM _bm, byte[] extra_info) {
        if(extra_info==this.extra_info) 
            return;
        this.extra_info = extra_info;
        saveFieldBytes(_bm, "extra_info", extra_info);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `settle_type` = '").append(settle_type).append("',");
        sBuilder.append(" `extra_info` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_settle_type)) sBuilder.append(" `settle_type` = '").append(settle_type).append("',");
        if(isFieldMarked(FIELD_extra_info)) sBuilder.append(" `extra_info` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_week_card_setting` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`settle_type` int(11) NOT NULL DEFAULT '0' COMMENT '处理类型',"
                + "`extra_info` blob NULL COMMENT '额外信息',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家周卡设置数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//settle_type
        _size+=2;_size+=extra_info.length;//extra_info
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(settle_type);
        buff.putShort((short)(extra_info == null ? 0 : extra_info.length));if(null != extra_info){buff.put(extra_info);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        settle_type=buff.getInt();
        int extra_info_count = buff.getShort();if(extra_info_count>0){extra_info = new byte[extra_info_count];buff.get(extra_info);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
