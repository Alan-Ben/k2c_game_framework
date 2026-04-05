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
public class PlayerHeroSkinBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_skin_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "skin_id", comment = "皮肤id")
    private long skin_id;

    public static final int FIELD_skin_level =2;
    @DataBaseField(type = "int(11)", fieldname = "skin_level", comment = "皮肤等级")
    private int skin_level;

    public PlayerHeroSkinBO() {
        id = 0;
        cid = 0L;
        skin_id = 0L;
        skin_level = 0;
    }

    public PlayerHeroSkinBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        skin_id = rs.getLong(3);
        skin_level = rs.getInt(4);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerHeroSkinBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `skin_id`, `skin_level`";
    }

    @Override
    public String getTableName() {
        return "`player_hero_skin`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(skin_id).append("', ");
        strBuf.append("'").append(skin_level).append("', ");
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

    // 皮肤id
    public long getSkinId() { return this.skin_id; }
    public void setSkinId(BM _bm, long skin_id) {
        if(skin_id==this.skin_id) 
            return;
        this.skin_id = skin_id; 
        markField(_bm, FIELD_skin_id); 
    }
    public void saveSkinId(BM _bm, long skin_id) {
        if(skin_id==this.skin_id) 
            return;
        this.skin_id = skin_id;
        saveField(_bm, "skin_id", skin_id);
    }

    // 皮肤等级
    public int getSkinLevel() { return this.skin_level; }
    public void setSkinLevel(BM _bm, int skin_level) {
        if(skin_level==this.skin_level) 
            return;
        this.skin_level = skin_level; 
        markField(_bm, FIELD_skin_level); 
    }
    public void saveSkinLevel(BM _bm, int skin_level) {
        if(skin_level==this.skin_level) 
            return;
        this.skin_level = skin_level;
        saveField(_bm, "skin_level", skin_level);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `skin_id` = '").append(skin_id).append("',");
        sBuilder.append(" `skin_level` = '").append(skin_level).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_skin_id)) sBuilder.append(" `skin_id` = '").append(skin_id).append("',");
        if(isFieldMarked(FIELD_skin_level)) sBuilder.append(" `skin_level` = '").append(skin_level).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_hero_skin` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`skin_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '皮肤id',"
                + "`skin_level` int(11) NOT NULL DEFAULT '0' COMMENT '皮肤等级',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家大臣皮肤数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//skin_id
        _size+=4;//skin_level
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(skin_id);
        buff.putInt(skin_level);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        skin_id=buff.getLong();
        skin_level=buff.getInt(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
