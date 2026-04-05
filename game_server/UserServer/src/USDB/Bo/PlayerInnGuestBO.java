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
public class PlayerInnGuestBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_guest_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "guest_id", comment = "客人ID")
    private long guest_id;

    public static final int FIELD_had_draw_handbook_reward =2;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_draw_handbook_reward", comment = "是否已领取图鉴奖励")
    private boolean had_draw_handbook_reward;

    public static final int FIELD_start_line_up_id =3;
    @DataBaseField(type = "bigint(20)", fieldname = "start_line_up_id", comment = "开始排队的id")
    private long start_line_up_id;

    public PlayerInnGuestBO() {
        id = 0;
        cid = 0L;
        guest_id = 0L;
        had_draw_handbook_reward = false;
        start_line_up_id = 0L;
    }

    public PlayerInnGuestBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        guest_id = rs.getLong(3);
        had_draw_handbook_reward = rs.getBoolean(4);
        start_line_up_id = rs.getLong(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerInnGuestBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `guest_id`, `had_draw_handbook_reward`, `start_line_up_id`";
    }

    @Override
    public String getTableName() {
        return "`player_inn_guest`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(guest_id).append("', ");
        strBuf.append("'").append(had_draw_handbook_reward ? 1 : 0).append("', ");
        strBuf.append("'").append(start_line_up_id).append("', ");
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

    // 客人ID
    public long getGuestId() { return this.guest_id; }
    public void setGuestId(BM _bm, long guest_id) {
        if(guest_id==this.guest_id) 
            return;
        this.guest_id = guest_id; 
        markField(_bm, FIELD_guest_id); 
    }
    public void saveGuestId(BM _bm, long guest_id) {
        if(guest_id==this.guest_id) 
            return;
        this.guest_id = guest_id;
        saveField(_bm, "guest_id", guest_id);
    }

    // 是否已领取图鉴奖励
    public boolean getHadDrawHandbookReward() { return this.had_draw_handbook_reward; }
    public void setHadDrawHandbookReward(BM _bm, boolean had_draw_handbook_reward) {
        if(had_draw_handbook_reward==this.had_draw_handbook_reward) 
            return;
        this.had_draw_handbook_reward = had_draw_handbook_reward; 
        markField(_bm, FIELD_had_draw_handbook_reward); 
    }
    public void saveHadDrawHandbookReward(BM _bm, boolean had_draw_handbook_reward) {
        if(had_draw_handbook_reward==this.had_draw_handbook_reward) 
            return;
        this.had_draw_handbook_reward = had_draw_handbook_reward;
        saveField(_bm, "had_draw_handbook_reward", had_draw_handbook_reward ? 1 : 0);
    }

    // 开始排队的id
    public long getStartLineUpId() { return this.start_line_up_id; }
    public void setStartLineUpId(BM _bm, long start_line_up_id) {
        if(start_line_up_id==this.start_line_up_id) 
            return;
        this.start_line_up_id = start_line_up_id; 
        markField(_bm, FIELD_start_line_up_id); 
    }
    public void saveStartLineUpId(BM _bm, long start_line_up_id) {
        if(start_line_up_id==this.start_line_up_id) 
            return;
        this.start_line_up_id = start_line_up_id;
        saveField(_bm, "start_line_up_id", start_line_up_id);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `guest_id` = '").append(guest_id).append("',");
        sBuilder.append(" `had_draw_handbook_reward` = '").append(had_draw_handbook_reward ? 1 : 0).append("',");
        sBuilder.append(" `start_line_up_id` = '").append(start_line_up_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_guest_id)) sBuilder.append(" `guest_id` = '").append(guest_id).append("',");
        if(isFieldMarked(FIELD_had_draw_handbook_reward)) sBuilder.append(" `had_draw_handbook_reward` = '").append(had_draw_handbook_reward ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_start_line_up_id)) sBuilder.append(" `start_line_up_id` = '").append(start_line_up_id).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_inn_guest` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`guest_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '客人ID',"
                + "`had_draw_handbook_reward` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已领取图鉴奖励',"
                + "`start_line_up_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '开始排队的id',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家旅店 客人数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//guest_id
        _size+=1;//had_draw_handbook_reward
        _size+=8;//start_line_up_id
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(guest_id);
        buff.put((byte)(had_draw_handbook_reward?1:0));
        buff.putLong(start_line_up_id);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        guest_id=buff.getLong();
        had_draw_handbook_reward=(buff.get()==1);
        start_line_up_id=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
