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
public class PlayerLoverCollectBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_target_lover_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "target_lover_id", comment = "选中的情人配置ID，0=未选择")
    private long target_lover_id;

    public static final int FIELD_is_claimed =2;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_claimed", comment = "是否已领取当前目标情人")
    private boolean is_claimed;

    public PlayerLoverCollectBO() {
        id = 0;
        cid = 0L;
        target_lover_id = 0L;
        is_claimed = false;
    }

    public PlayerLoverCollectBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        target_lover_id = rs.getLong(3);
        is_claimed = rs.getBoolean(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerLoverCollectBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `target_lover_id`, `is_claimed`";
    }

    @Override
    public String getTableName() {
        return "`player_lover_collect`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(target_lover_id).append("', ");
        strBuf.append("'").append(is_claimed ? 1 : 0).append("', ");
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

    // 选中的情人配置ID，0=未选择
    public long getTargetLoverId() { return this.target_lover_id; }
    public void setTargetLoverId(BM _bm, long target_lover_id) {
        if(target_lover_id==this.target_lover_id) 
            return;
        this.target_lover_id = target_lover_id; 
        markField(_bm, FIELD_target_lover_id); 
    }
    public void saveTargetLoverId(BM _bm, long target_lover_id) {
        if(target_lover_id==this.target_lover_id) 
            return;
        this.target_lover_id = target_lover_id;
        saveField(_bm, "target_lover_id", target_lover_id);
    }

    // 是否已领取当前目标情人
    public boolean getIsClaimed() { return this.is_claimed; }
    public void setIsClaimed(BM _bm, boolean is_claimed) {
        if(is_claimed==this.is_claimed) 
            return;
        this.is_claimed = is_claimed; 
        markField(_bm, FIELD_is_claimed); 
    }
    public void saveIsClaimed(BM _bm, boolean is_claimed) {
        if(is_claimed==this.is_claimed) 
            return;
        this.is_claimed = is_claimed;
        saveField(_bm, "is_claimed", is_claimed ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `target_lover_id` = '").append(target_lover_id).append("',");
        sBuilder.append(" `is_claimed` = '").append(is_claimed ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_target_lover_id)) sBuilder.append(" `target_lover_id` = '").append(target_lover_id).append("',");
        if(isFieldMarked(FIELD_is_claimed)) sBuilder.append(" `is_claimed` = '").append(is_claimed ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_lover_collect` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`target_lover_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '选中的情人配置ID，0=未选择',"
                + "`is_claimed` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已领取当前目标情人',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家情人收集数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//target_lover_id
        _size+=1;//is_claimed
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(target_lover_id);
        buff.put((byte)(is_claimed?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        target_lover_id=buff.getLong();
        is_claimed=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
