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
public class PlayerDinnerStartLogBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_instance_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "instance_id", comment = "宴会实例ID")
    private long instance_id;

    public static final int FIELD_log_idx =2;
    @DataBaseField(type = "blob", fieldname = "log_idx", comment = "开宴日志索引数据")
    private byte[] log_idx;

    public static final int FIELD_log_info =3;
    @DataBaseField(type = "blob", fieldname = "log_info", comment = "开宴日志详细数据")
    private byte[] log_info;

    public static final int FIELD_start_ts =4;
    @DataBaseField(type = "int(11)", fieldname = "start_ts", comment = "开宴时间（秒）")
    private int start_ts;

    public PlayerDinnerStartLogBO() {
        id = 0;
        cid = 0L;
        instance_id = 0L;
        log_idx = null;
        log_info = null;
        start_ts = 0;
    }

    public PlayerDinnerStartLogBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        instance_id = rs.getLong(3);
        log_idx = rs.getBytes(4);
        log_info = rs.getBytes(5);
        start_ts = rs.getInt(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerDinnerStartLogBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `instance_id`, `log_idx`, `log_info`, `start_ts`";
    }

    @Override
    public String getTableName() {
        return "`player_dinner_start_log`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(instance_id).append("', ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.append("'").append(start_ts).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(log_idx); 
        ret.add(log_info);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_log_idx)) ret.add(log_idx); 
        if(isFieldMarked(FIELD_log_info)) ret.add(log_info);         return ret;
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

    // 宴会实例ID
    public long getInstanceId() { return this.instance_id; }
    public void setInstanceId(BM _bm, long instance_id) {
        if(instance_id==this.instance_id) 
            return;
        this.instance_id = instance_id; 
        markField(_bm, FIELD_instance_id); 
    }
    public void saveInstanceId(BM _bm, long instance_id) {
        if(instance_id==this.instance_id) 
            return;
        this.instance_id = instance_id;
        saveField(_bm, "instance_id", instance_id);
    }

    // 开宴日志索引数据
    public byte[] getLogIdx() { return this.log_idx; }
    public void setLogIdx(BM _bm, byte[] log_idx) {
        if(log_idx==this.log_idx) 
            return;
        this.log_idx = log_idx; 
        markField(_bm, FIELD_log_idx); 
    }
    public void saveLogIdx(BM _bm, byte[] log_idx) {
        if(log_idx==this.log_idx) 
            return;
        this.log_idx = log_idx;
        saveFieldBytes(_bm, "log_idx", log_idx);
    }

    // 开宴日志详细数据
    public byte[] getLogInfo() { return this.log_info; }
    public void setLogInfo(BM _bm, byte[] log_info) {
        if(log_info==this.log_info) 
            return;
        this.log_info = log_info; 
        markField(_bm, FIELD_log_info); 
    }
    public void saveLogInfo(BM _bm, byte[] log_info) {
        if(log_info==this.log_info) 
            return;
        this.log_info = log_info;
        saveFieldBytes(_bm, "log_info", log_info);
    }

    // 开宴时间（秒）
    public int getStartTs() { return this.start_ts; }
    public void setStartTs(BM _bm, int start_ts) {
        if(start_ts==this.start_ts) 
            return;
        this.start_ts = start_ts; 
        markField(_bm, FIELD_start_ts); 
    }
    public void saveStartTs(BM _bm, int start_ts) {
        if(start_ts==this.start_ts) 
            return;
        this.start_ts = start_ts;
        saveField(_bm, "start_ts", start_ts);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        sBuilder.append(" `log_idx` = ?,");
        sBuilder.append(" `log_info` = ?,");
        sBuilder.append(" `start_ts` = '").append(start_ts).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_instance_id)) sBuilder.append(" `instance_id` = '").append(instance_id).append("',");
        if(isFieldMarked(FIELD_log_idx)) sBuilder.append(" `log_idx` = ?,");
        if(isFieldMarked(FIELD_log_info)) sBuilder.append(" `log_info` = ?,");
        if(isFieldMarked(FIELD_start_ts)) sBuilder.append(" `start_ts` = '").append(start_ts).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_dinner_start_log` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`instance_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '宴会实例ID',"
                + "`log_idx` blob NULL COMMENT '开宴日志索引数据',"
                + "`log_info` blob NULL COMMENT '开宴日志详细数据',"
                + "`start_ts` int(11) NOT NULL DEFAULT '0' COMMENT '开宴时间（秒）',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家开宴日志' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//instance_id
        _size+=2;_size+=log_idx.length;//log_idx
        _size+=2;_size+=log_info.length;//log_info
        _size+=4;//start_ts
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(instance_id);
        buff.putShort((short)(log_idx == null ? 0 : log_idx.length));if(null != log_idx){buff.put(log_idx);}
        buff.putShort((short)(log_info == null ? 0 : log_info.length));if(null != log_info){buff.put(log_info);}
        buff.putInt(start_ts);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        instance_id=buff.getLong();
        int log_idx_count = buff.getShort();if(log_idx_count>0){log_idx = new byte[log_idx_count];buff.get(log_idx);}
        int log_info_count = buff.getShort();if(log_info_count>0){log_info = new byte[log_info_count];buff.get(log_info);}
        start_ts=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
