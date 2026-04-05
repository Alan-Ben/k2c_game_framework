package HSDB.Bo;
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
public class HsWhiteAccBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_acc =0;
    @DataBaseField(type = "varchar(50)", fieldname = "acc", comment = "玩家account")
    private String acc;

    public static final int FIELD_create_at =1;
    @DataBaseField(type = "bigint(20)", fieldname = "create_at", comment = "创建时间ms")
    private long create_at;

    public HsWhiteAccBO() {
        id = 0;
        acc = "";
        create_at = 0L;
    }

    public HsWhiteAccBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        acc = rs.getString(2);
        create_at = rs.getLong(3);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new HsWhiteAccBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `acc`, `create_at`";
    }

    @Override
    public String getTableName() {
        return "`hs_white_acc`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(acc == null ? null : acc.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(create_at).append("', ");
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

    // 玩家account
    public String getAcc() { return this.acc; }
    public void setAcc(BM _bm, String acc) {
        if(acc.equals(this.acc)) 
            return;
        this.acc = acc; 
        markField(_bm, FIELD_acc); 
    }
    public void saveAcc(BM _bm, String acc) {
        if(acc.equals(this.acc)) 
            return;
        this.acc = acc;
        saveField(_bm, "acc", acc);
    }

    // 创建时间ms
    public long getCreateAt() { return this.create_at; }
    public void setCreateAt(BM _bm, long create_at) {
        if(create_at==this.create_at) 
            return;
        this.create_at = create_at; 
        markField(_bm, FIELD_create_at); 
    }
    public void saveCreateAt(BM _bm, long create_at) {
        if(create_at==this.create_at) 
            return;
        this.create_at = create_at;
        saveField(_bm, "create_at", create_at);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `acc` = '").append(acc == null ? null : acc.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `create_at` = '").append(create_at).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_acc)) sBuilder.append(" `acc` = '").append(acc == null ? null : acc.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_create_at)) sBuilder.append(" `create_at` = '").append(create_at).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `hs_white_acc` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`acc` varchar(50) NOT NULL DEFAULT '' COMMENT '玩家account',"
                + "`create_at` bigint(20) NOT NULL DEFAULT '0' COMMENT '创建时间ms',"
                + "KEY `acc` (`acc`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='http 白名单账号列表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.hs_db;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(acc);//acc
        _size+=8;//create_at
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, acc);
        buff.putLong(create_at);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        acc=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        create_at=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
