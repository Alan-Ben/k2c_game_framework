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
public class PlayerCounterBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_counter =1;
    @DataBaseField(type = "bigint(20)", size = 10, fieldname = "counter", comment = "计数数值")
    private List<Long> counter;

    public PlayerCounterBO() {
        id = 0;
        cid = 0L;
        counter = new ArrayList<>(10);
        for (int i = 0; i < 10; ++i) {
            counter.add(0L);
        }
    }

    public PlayerCounterBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        counter = new ArrayList<>(10);
        for (int i = 0; i < 10; ++i) {
            counter.add(rs.getLong(i + 3));
        }
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerCounterBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `counter_0`, `counter_1`, `counter_2`, `counter_3`, `counter_4`, `counter_5`, `counter_6`, `counter_7`, `counter_8`, `counter_9`";
    }

    @Override
    public String getTableName() {
        return "`player_counter`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        for (int i = 0; i < counter.size(); ++i) {
            strBuf.append("'").append(counter.get(i)).append("', ");
        }
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

    // 计数数值
    public int getCounterSize() { return this.counter.size(); }
    public List<Long> getCounterAll() { return new ArrayList<>(counter); }
    public void setCounterAll(BM _bm, long value) { for (int i = 0; i < this.counter.size(); ++i) this.counter.set(i, value);  markField(_bm, FIELD_counter); }
    public void saveCounterAll(BM _bm, long value) { setCounterAll(_bm, value); saveAll(_bm); }
    public long getCounter(int index) { return this.counter.get(index); }
    public void setCounter(BM _bm, int index, long value) {
        if(value==this.counter.get(index))
            return;
        this.counter.set(index, value);
        markField(_bm, FIELD_counter); 
    }
    public void saveCounter(BM _bm, int index, long value) {
        if(value==this.counter.get(index))
            return;
        this.counter.set(index, value);
        saveField(_bm, "counter_" + index, this.counter.get(index));
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        for (int i = 0; i < counter.size(); ++i) {
            sBuilder.append(" `counter_").append(i).append("` = '").append(counter.get(i)).append("',");
        }
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_counter))  for (int i = 0; i < counter.size(); ++i) { sBuilder.append(" `counter_").append(i).append("` = '").append(counter.get(i)).append("',");}
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_counter` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`counter_0` bigint(20) NOT NULL DEFAULT '0' COMMENT '计数数值',"
                + "`counter_1` bigint(20) NOT NULL DEFAULT '0' COMMENT '计数数值',"
                + "`counter_2` bigint(20) NOT NULL DEFAULT '0' COMMENT '计数数值',"
                + "`counter_3` bigint(20) NOT NULL DEFAULT '0' COMMENT '计数数值',"
                + "`counter_4` bigint(20) NOT NULL DEFAULT '0' COMMENT '计数数值',"
                + "`counter_5` bigint(20) NOT NULL DEFAULT '0' COMMENT '计数数值',"
                + "`counter_6` bigint(20) NOT NULL DEFAULT '0' COMMENT '计数数值',"
                + "`counter_7` bigint(20) NOT NULL DEFAULT '0' COMMENT '计数数值',"
                + "`counter_8` bigint(20) NOT NULL DEFAULT '0' COMMENT '计数数值',"
                + "`counter_9` bigint(20) NOT NULL DEFAULT '0' COMMENT '计数数值',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家计数数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=2;//counter
        for (int i = 0; i < counter.size(); ++i){
            _size+=8;
        }
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putShort((short)(counter.size()));
        for (int i = 0; i < counter.size(); ++i){
             buff.putLong(counter.get(i));
        }        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        counter.clear();
        int counter_count=buff.getShort();
        for(int i=0;i<counter_count;i++){
             counter.add(buff.getLong());
        } 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
