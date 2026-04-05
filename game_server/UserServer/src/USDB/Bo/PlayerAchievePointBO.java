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
public class PlayerAchievePointBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_type =1;
    @DataBaseField(type = "int(11)", fieldname = "type", comment = "成就点ID")
    private int type;

    public static final int FIELD_count =2;
    @DataBaseField(type = "bigint(20)", fieldname = "count", comment = "计数")
    private long count;

    public static final int FIELD_had_draw_max_step =3;
    @DataBaseField(type = "int(11)", fieldname = "had_draw_max_step", comment = "已领取最大步骤")
    private int had_draw_max_step;

    public PlayerAchievePointBO() {
        id = 0;
        cid = 0L;
        type = 0;
        count = 0L;
        had_draw_max_step = 0;
    }

    public PlayerAchievePointBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        type = rs.getInt(3);
        count = rs.getLong(4);
        had_draw_max_step = rs.getInt(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerAchievePointBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `type`, `count`, `had_draw_max_step`";
    }

    @Override
    public String getTableName() {
        return "`player_achieve_point`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(type).append("', ");
        strBuf.append("'").append(count).append("', ");
        strBuf.append("'").append(had_draw_max_step).append("', ");
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

    // 成就点ID
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

    // 计数
    public long getCount() { return this.count; }
    public void setCount(BM _bm, long count) {
        if(count==this.count) 
            return;
        this.count = count; 
        markField(_bm, FIELD_count); 
    }
    public void saveCount(BM _bm, long count) {
        if(count==this.count) 
            return;
        this.count = count;
        saveField(_bm, "count", count);
    }

    // 已领取最大步骤
    public int getHadDrawMaxStep() { return this.had_draw_max_step; }
    public void setHadDrawMaxStep(BM _bm, int had_draw_max_step) {
        if(had_draw_max_step==this.had_draw_max_step) 
            return;
        this.had_draw_max_step = had_draw_max_step; 
        markField(_bm, FIELD_had_draw_max_step); 
    }
    public void saveHadDrawMaxStep(BM _bm, int had_draw_max_step) {
        if(had_draw_max_step==this.had_draw_max_step) 
            return;
        this.had_draw_max_step = had_draw_max_step;
        saveField(_bm, "had_draw_max_step", had_draw_max_step);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `type` = '").append(type).append("',");
        sBuilder.append(" `count` = '").append(count).append("',");
        sBuilder.append(" `had_draw_max_step` = '").append(had_draw_max_step).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_type)) sBuilder.append(" `type` = '").append(type).append("',");
        if(isFieldMarked(FIELD_count)) sBuilder.append(" `count` = '").append(count).append("',");
        if(isFieldMarked(FIELD_had_draw_max_step)) sBuilder.append(" `had_draw_max_step` = '").append(had_draw_max_step).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_achieve_point` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`type` int(11) NOT NULL DEFAULT '0' COMMENT '成就点ID',"
                + "`count` bigint(20) NOT NULL DEFAULT '0' COMMENT '计数',"
                + "`had_draw_max_step` int(11) NOT NULL DEFAULT '0' COMMENT '已领取最大步骤',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家成就点数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//type
        _size+=8;//count
        _size+=4;//had_draw_max_step
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(type);
        buff.putLong(count);
        buff.putInt(had_draw_max_step);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        type=buff.getInt();
        count=buff.getLong();
        had_draw_max_step=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
