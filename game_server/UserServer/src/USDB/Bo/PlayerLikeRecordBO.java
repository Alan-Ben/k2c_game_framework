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
public class PlayerLikeRecordBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家cid")
    private long cid;

    public static final int FIELD_last_refresh_date =1;
    @DataBaseField(type = "int(11)", fieldname = "last_refresh_date", comment = "上次刷新时间")
    private int last_refresh_date;

    public static final int FIELD_like_cid_list =2;
    @DataBaseField(type = "blob", fieldname = "like_cid_list", comment = "今天点赞的cid列表")
    private byte[] like_cid_list;

    public PlayerLikeRecordBO() {
        id = 0;
        cid = 0L;
        last_refresh_date = 0;
        like_cid_list = null;
    }

    public PlayerLikeRecordBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        last_refresh_date = rs.getInt(3);
        like_cid_list = rs.getBytes(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerLikeRecordBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `last_refresh_date`, `like_cid_list`";
    }

    @Override
    public String getTableName() {
        return "`player_like_record`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(last_refresh_date).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(like_cid_list);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_like_cid_list)) ret.add(like_cid_list);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 玩家cid
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

    // 上次刷新时间
    public int getLastRefreshDate() { return this.last_refresh_date; }
    public void setLastRefreshDate(BM _bm, int last_refresh_date) {
        if(last_refresh_date==this.last_refresh_date) 
            return;
        this.last_refresh_date = last_refresh_date; 
        markField(_bm, FIELD_last_refresh_date); 
    }
    public void saveLastRefreshDate(BM _bm, int last_refresh_date) {
        if(last_refresh_date==this.last_refresh_date) 
            return;
        this.last_refresh_date = last_refresh_date;
        saveField(_bm, "last_refresh_date", last_refresh_date);
    }

    // 今天点赞的cid列表
    public byte[] getLikeCidList() { return this.like_cid_list; }
    public void setLikeCidList(BM _bm, byte[] like_cid_list) {
        if(like_cid_list==this.like_cid_list) 
            return;
        this.like_cid_list = like_cid_list; 
        markField(_bm, FIELD_like_cid_list); 
    }
    public void saveLikeCidList(BM _bm, byte[] like_cid_list) {
        if(like_cid_list==this.like_cid_list) 
            return;
        this.like_cid_list = like_cid_list;
        saveFieldBytes(_bm, "like_cid_list", like_cid_list);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `last_refresh_date` = '").append(last_refresh_date).append("',");
        sBuilder.append(" `like_cid_list` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_last_refresh_date)) sBuilder.append(" `last_refresh_date` = '").append(last_refresh_date).append("',");
        if(isFieldMarked(FIELD_like_cid_list)) sBuilder.append(" `like_cid_list` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_like_record` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家cid',"
                + "`last_refresh_date` int(11) NOT NULL DEFAULT '0' COMMENT '上次刷新时间',"
                + "`like_cid_list` blob NULL COMMENT '今天点赞的cid列表',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家点赞记录' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//last_refresh_date
        _size+=2;_size+=like_cid_list.length;//like_cid_list
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(last_refresh_date);
        buff.putShort((short)(like_cid_list == null ? 0 : like_cid_list.length));if(null != like_cid_list){buff.put(like_cid_list);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        last_refresh_date=buff.getInt();
        int like_cid_list_count = buff.getShort();if(like_cid_list_count>0){like_cid_list = new byte[like_cid_list_count];buff.get(like_cid_list);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
