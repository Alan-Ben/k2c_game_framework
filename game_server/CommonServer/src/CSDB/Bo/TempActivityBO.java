package CSDB.Bo;
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
public class TempActivityBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_uid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "uid", comment = "玩家id")
    private long uid;

    public static final int FIELD_type =1;
    @DataBaseField(type = "int(11)", fieldname = "type", comment = "类型，1v1or2v2")
    private int type;

    public static final int FIELD_count =2;
    @DataBaseField(type = "int(11)", fieldname = "count", comment = "次数")
    private int count;

    public static final int FIELD_tag =3;
    @DataBaseField(type = "bigint(20)", fieldname = "tag", comment = "标记")
    private long tag;

    public TempActivityBO() {
        id = 0;
        uid = 0L;
        type = 0;
        count = 0;
        tag = 0L;
    }

    public TempActivityBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        uid = rs.getLong(2);
        type = rs.getInt(3);
        count = rs.getInt(4);
        tag = rs.getLong(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new TempActivityBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `uid`, `type`, `count`, `tag`";
    }

    @Override
    public String getTableName() {
        return "`temp_activity`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(uid).append("', ");
        strBuf.append("'").append(type).append("', ");
        strBuf.append("'").append(count).append("', ");
        strBuf.append("'").append(tag).append("', ");
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

    // 玩家id
    public long getUid() { return this.uid; }
    public void setUid(BM _bm, long uid) {
        if(uid==this.uid) 
            return;
        this.uid = uid; 
        markField(_bm, FIELD_uid); 
    }
    public void saveUid(BM _bm, long uid) {
        if(uid==this.uid) 
            return;
        this.uid = uid;
        saveField(_bm, "uid", uid);
    }

    // 类型，1v1or2v2
    public int getType() { return this.type; }
    public void setType(BM _bm, int type) {
        if(type==this.type) 
            return;
        this.type = type; 
        markField(_bm, FIELD_type); 
    }
    public void saveType(BM _bm, int type) {
        if(type==this.type) 
            return;
        this.type = type;
        saveField(_bm, "type", type);
    }

    // 次数
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

    // 标记
    public long getTag() { return this.tag; }
    public void setTag(BM _bm, long tag) {
        if(tag==this.tag) 
            return;
        this.tag = tag; 
        markField(_bm, FIELD_tag); 
    }
    public void saveTag(BM _bm, long tag) {
        if(tag==this.tag) 
            return;
        this.tag = tag;
        saveField(_bm, "tag", tag);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `uid` = '").append(uid).append("',");
        sBuilder.append(" `type` = '").append(type).append("',");
        sBuilder.append(" `count` = '").append(count).append("',");
        sBuilder.append(" `tag` = '").append(tag).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_uid)) sBuilder.append(" `uid` = '").append(uid).append("',");
        if(isFieldMarked(FIELD_type)) sBuilder.append(" `type` = '").append(type).append("',");
        if(isFieldMarked(FIELD_count)) sBuilder.append(" `count` = '").append(count).append("',");
        if(isFieldMarked(FIELD_tag)) sBuilder.append(" `tag` = '").append(tag).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `temp_activity` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`uid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家id',"
                + "`type` int(11) NOT NULL DEFAULT '0' COMMENT '类型，1v1or2v2',"
                + "`count` int(11) NOT NULL DEFAULT '0' COMMENT '次数',"
                + "`tag` bigint(20) NOT NULL DEFAULT '0' COMMENT '标记',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='临时活动信息' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.comm_main;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=8;//uid
        _size+=4;//type
        _size+=4;//count
        _size+=8;//tag
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(uid);
        buff.putInt(type);
        buff.putInt(count);
        buff.putLong(tag);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        uid=buff.getLong();
        type=buff.getInt();
        count=buff.getInt();
        tag=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
