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
public class DeplomacyUsBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_us_id =0;
    @DataBaseField(type = "int(11)", fieldname = "us_id", comment = "usId")
    private int us_id;

    public static final int FIELD_build_time_ms =1;
    @DataBaseField(type = "bigint(20)", fieldname = "build_time_ms", comment = "建交时间")
    private long build_time_ms;

    public DeplomacyUsBO() {
        id = 0;
        us_id = 0;
        build_time_ms = 0L;
    }

    public DeplomacyUsBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        us_id = rs.getInt(2);
        build_time_ms = rs.getLong(3);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new DeplomacyUsBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `us_id`, `build_time_ms`";
    }

    @Override
    public String getTableName() {
        return "`deplomacy_us`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(us_id).append("', ");
        strBuf.append("'").append(build_time_ms).append("', ");
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

    // usId
    public int getUsId() { return this.us_id; }
    public void setUsId(BM _bm, int us_id) {
        if(us_id==this.us_id) 
            return;
        this.us_id = us_id; 
        markField(_bm, FIELD_us_id); 
    }
    public void saveUsId(BM _bm, int us_id) {
        if(us_id==this.us_id) 
            return;
        this.us_id = us_id;
        saveField(_bm, "us_id", us_id);
    }

    // 建交时间
    public long getBuildTimeMs() { return this.build_time_ms; }
    public void setBuildTimeMs(BM _bm, long build_time_ms) {
        if(build_time_ms==this.build_time_ms) 
            return;
        this.build_time_ms = build_time_ms; 
        markField(_bm, FIELD_build_time_ms); 
    }
    public void saveBuildTimeMs(BM _bm, long build_time_ms) {
        if(build_time_ms==this.build_time_ms) 
            return;
        this.build_time_ms = build_time_ms;
        saveField(_bm, "build_time_ms", build_time_ms);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `us_id` = '").append(us_id).append("',");
        sBuilder.append(" `build_time_ms` = '").append(build_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_us_id)) sBuilder.append(" `us_id` = '").append(us_id).append("',");
        if(isFieldMarked(FIELD_build_time_ms)) sBuilder.append(" `build_time_ms` = '").append(build_time_ms).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `deplomacy_us` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`us_id` int(11) NOT NULL DEFAULT '0' COMMENT 'usId',"
                + "`build_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '建交时间',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='建交USd信息' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//us_id
        _size+=8;//build_time_ms
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putInt(us_id);
        buff.putLong(build_time_ms);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        us_id=buff.getInt();
        build_time_ms=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
