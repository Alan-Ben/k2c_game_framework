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
public class PlayerDinnerPermitBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_permit_type =1;
    @DataBaseField(type = "int(11)", fieldname = "permit_type", comment = "凭证类型")
    private int permit_type;

    public static final int FIELD_type_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "type_id", comment = "凭证额外类型ID")
    private long type_id;

    public static final int FIELD_expired_ts =3;
    @DataBaseField(type = "int(11)", fieldname = "expired_ts", comment = "凭证过期时间（秒）")
    private int expired_ts;

    public PlayerDinnerPermitBO() {
        id = 0;
        cid = 0L;
        permit_type = 0;
        type_id = 0L;
        expired_ts = 0;
    }

    public PlayerDinnerPermitBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        permit_type = rs.getInt(3);
        type_id = rs.getLong(4);
        expired_ts = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerDinnerPermitBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `permit_type`, `type_id`, `expired_ts`";
    }

    @Override
    public String getTableName() {
        return "`player_dinner_permit`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(permit_type).append("', ");
        strBuf.append("'").append(type_id).append("', ");
        strBuf.append("'").append(expired_ts).append("', ");
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

    // 凭证类型
    public int getPermitType() { return this.permit_type; }
    public void setPermitType(BM _bm, int permit_type) {
        if(permit_type==this.permit_type) 
            return;
        this.permit_type = permit_type; 
        markField(_bm, FIELD_permit_type); 
    }
    public void savePermitType(BM _bm, int permit_type) {
        if(permit_type==this.permit_type) 
            return;
        this.permit_type = permit_type;
        saveField(_bm, "permit_type", permit_type);
    }

    // 凭证额外类型ID
    public long getTypeId() { return this.type_id; }
    public void setTypeId(BM _bm, long type_id) {
        if(type_id==this.type_id) 
            return;
        this.type_id = type_id; 
        markField(_bm, FIELD_type_id); 
    }
    public void saveTypeId(BM _bm, long type_id) {
        if(type_id==this.type_id) 
            return;
        this.type_id = type_id;
        saveField(_bm, "type_id", type_id);
    }

    // 凭证过期时间（秒）
    public int getExpiredTs() { return this.expired_ts; }
    public void setExpiredTs(BM _bm, int expired_ts) {
        if(expired_ts==this.expired_ts) 
            return;
        this.expired_ts = expired_ts; 
        markField(_bm, FIELD_expired_ts); 
    }
    public void saveExpiredTs(BM _bm, int expired_ts) {
        if(expired_ts==this.expired_ts) 
            return;
        this.expired_ts = expired_ts;
        saveField(_bm, "expired_ts", expired_ts);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `permit_type` = '").append(permit_type).append("',");
        sBuilder.append(" `type_id` = '").append(type_id).append("',");
        sBuilder.append(" `expired_ts` = '").append(expired_ts).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_permit_type)) sBuilder.append(" `permit_type` = '").append(permit_type).append("',");
        if(isFieldMarked(FIELD_type_id)) sBuilder.append(" `type_id` = '").append(type_id).append("',");
        if(isFieldMarked(FIELD_expired_ts)) sBuilder.append(" `expired_ts` = '").append(expired_ts).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_dinner_permit` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`permit_type` int(11) NOT NULL DEFAULT '0' COMMENT '凭证类型',"
                + "`type_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '凭证额外类型ID',"
                + "`expired_ts` int(11) NOT NULL DEFAULT '0' COMMENT '凭证过期时间（秒）',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家宴会凭证数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//permit_type
        _size+=8;//type_id
        _size+=4;//expired_ts
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(permit_type);
        buff.putLong(type_id);
        buff.putInt(expired_ts);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        permit_type=buff.getInt();
        type_id=buff.getLong();
        expired_ts=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
