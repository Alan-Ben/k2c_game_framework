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
public class PlayerFuncUnlockBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_func_type =1;
    @DataBaseField(type = "int(11)", fieldname = "func_type", comment = "已领取功能类型")
    private int func_type;

    public static final int FIELD_is_client_notified =2;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_client_notified", comment = "客户端是否已通知")
    private boolean is_client_notified;

    public static final int FIELD_not_draw_reward =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "not_draw_reward", comment = "未领取奖励")
    private boolean not_draw_reward;

    public PlayerFuncUnlockBO() {
        id = 0;
        cid = 0L;
        func_type = 0;
        is_client_notified = false;
        not_draw_reward = false;
    }

    public PlayerFuncUnlockBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        func_type = rs.getInt(3);
        is_client_notified = rs.getBoolean(4);
        not_draw_reward = rs.getBoolean(5);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerFuncUnlockBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `func_type`, `is_client_notified`, `not_draw_reward`";
    }

    @Override
    public String getTableName() {
        return "`player_func_unlock`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(func_type).append("', ");
        strBuf.append("'").append(is_client_notified ? 1 : 0).append("', ");
        strBuf.append("'").append(not_draw_reward ? 1 : 0).append("', ");
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

    // 已领取功能类型
    public int getFuncType() { return this.func_type; }
    public void setFuncType(BM _bm, int func_type) {
        if(func_type==this.func_type) 
            return;
        this.func_type = func_type; 
        markField(_bm, FIELD_func_type); 
    }
    public void saveFuncType(BM _bm, int func_type) {
        if(func_type==this.func_type) 
            return;
        this.func_type = func_type;
        saveField(_bm, "func_type", func_type);
    }

    // 客户端是否已通知
    public boolean getIsClientNotified() { return this.is_client_notified; }
    public void setIsClientNotified(BM _bm, boolean is_client_notified) {
        if(is_client_notified==this.is_client_notified) 
            return;
        this.is_client_notified = is_client_notified; 
        markField(_bm, FIELD_is_client_notified); 
    }
    public void saveIsClientNotified(BM _bm, boolean is_client_notified) {
        if(is_client_notified==this.is_client_notified) 
            return;
        this.is_client_notified = is_client_notified;
        saveField(_bm, "is_client_notified", is_client_notified ? 1 : 0);
    }

    // 未领取奖励
    public boolean getNotDrawReward() { return this.not_draw_reward; }
    public void setNotDrawReward(BM _bm, boolean not_draw_reward) {
        if(not_draw_reward==this.not_draw_reward) 
            return;
        this.not_draw_reward = not_draw_reward; 
        markField(_bm, FIELD_not_draw_reward); 
    }
    public void saveNotDrawReward(BM _bm, boolean not_draw_reward) {
        if(not_draw_reward==this.not_draw_reward) 
            return;
        this.not_draw_reward = not_draw_reward;
        saveField(_bm, "not_draw_reward", not_draw_reward ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `func_type` = '").append(func_type).append("',");
        sBuilder.append(" `is_client_notified` = '").append(is_client_notified ? 1 : 0).append("',");
        sBuilder.append(" `not_draw_reward` = '").append(not_draw_reward ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_func_type)) sBuilder.append(" `func_type` = '").append(func_type).append("',");
        if(isFieldMarked(FIELD_is_client_notified)) sBuilder.append(" `is_client_notified` = '").append(is_client_notified ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_not_draw_reward)) sBuilder.append(" `not_draw_reward` = '").append(not_draw_reward ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_func_unlock` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`func_type` int(11) NOT NULL DEFAULT '0' COMMENT '已领取功能类型',"
                + "`is_client_notified` tinyint(1) NOT NULL DEFAULT '0' COMMENT '客户端是否已通知',"
                + "`not_draw_reward` tinyint(1) NOT NULL DEFAULT '0' COMMENT '未领取奖励',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家功能解锁数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//func_type
        _size+=1;//is_client_notified
        _size+=1;//not_draw_reward
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(func_type);
        buff.put((byte)(is_client_notified?1:0));
        buff.put((byte)(not_draw_reward?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        func_type=buff.getInt();
        is_client_notified=(buff.get()==1);
        not_draw_reward=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
