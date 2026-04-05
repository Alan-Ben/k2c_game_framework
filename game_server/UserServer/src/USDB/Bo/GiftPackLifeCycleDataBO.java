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
public class GiftPackLifeCycleDataBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_gift_pack_id =0;
    @DataBaseField(type = "bigint(20)", fieldname = "gift_pack_id", comment = "礼包ID")
    private long gift_pack_id;

    public GiftPackLifeCycleDataBO() {
        id = 0;
        gift_pack_id = 0L;
    }

    public GiftPackLifeCycleDataBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        gift_pack_id = rs.getLong(2);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GiftPackLifeCycleDataBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `gift_pack_id`";
    }

    @Override
    public String getTableName() {
        return "`gift_pack_life_cycle_data`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(gift_pack_id).append("', ");
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

    // 礼包ID
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



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `gift_pack_id` = '").append(gift_pack_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_gift_pack_id)) sBuilder.append(" `gift_pack_id` = '").append(gift_pack_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `gift_pack_life_cycle_data` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`gift_pack_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '礼包ID',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='礼包生命周期数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//gift_pack_id
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(gift_pack_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        gift_pack_id=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
