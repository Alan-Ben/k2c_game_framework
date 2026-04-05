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
public class PlayerMarsExploreBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_lvl =1;
    @DataBaseField(type = "int(11)", fieldname = "lvl", comment = "探索等级")
    private int lvl;

    public static final int FIELD_refreshEvents =2;
    @DataBaseField(type = "tinyint(1)", fieldname = "refreshEvents", comment = "已刷新探索事件")
    private boolean refreshEvents;

    public static final int FIELD_exploreSum =3;
    @DataBaseField(type = "int(11)", fieldname = "exploreSum", comment = "探索次数")
    private int exploreSum;

    public static final int FIELD_pvpLogLastCreated =4;
    @DataBaseField(type = "bigint(20)", fieldname = "pvpLogLastCreated", comment = "PVP日志最新创建时间")
    private long pvpLogLastCreated;

    public PlayerMarsExploreBO() {
        id = 0;
        cid = 0L;
        lvl = 0;
        refreshEvents = false;
        exploreSum = 0;
        pvpLogLastCreated = 0L;
    }

    public PlayerMarsExploreBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        lvl = rs.getInt(3);
        refreshEvents = rs.getBoolean(4);
        exploreSum = rs.getInt(5);
        pvpLogLastCreated = rs.getLong(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMarsExploreBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `lvl`, `refreshEvents`, `exploreSum`, `pvpLogLastCreated`";
    }

    @Override
    public String getTableName() {
        return "`player_mars_explore`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(lvl).append("', ");
        strBuf.append("'").append(refreshEvents ? 1 : 0).append("', ");
        strBuf.append("'").append(exploreSum).append("', ");
        strBuf.append("'").append(pvpLogLastCreated).append("', ");
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

    // 探索等级
    public int getLvl() { return this.lvl; }
    public void setLvl(BM _bm, int lvl) {
        if(lvl==this.lvl) 
            return;
        this.lvl = lvl; 
        markField(_bm, FIELD_lvl); 
    }
    public void saveLvl(BM _bm, int lvl) {
        if(lvl==this.lvl) 
            return;
        this.lvl = lvl;
        saveField(_bm, "lvl", lvl);
    }

    // 已刷新探索事件
    public boolean getRefreshEvents() { return this.refreshEvents; }
    public void setRefreshEvents(BM _bm, boolean refreshEvents) {
        if(refreshEvents==this.refreshEvents) 
            return;
        this.refreshEvents = refreshEvents; 
        markField(_bm, FIELD_refreshEvents); 
    }
    public void saveRefreshEvents(BM _bm, boolean refreshEvents) {
        if(refreshEvents==this.refreshEvents) 
            return;
        this.refreshEvents = refreshEvents;
        saveField(_bm, "refreshEvents", refreshEvents ? 1 : 0);
    }

    // 探索次数
    public int getExploreSum() { return this.exploreSum; }
    public void setExploreSum(BM _bm, int exploreSum) {
        if(exploreSum==this.exploreSum) 
            return;
        this.exploreSum = exploreSum; 
        markField(_bm, FIELD_exploreSum); 
    }
    public void saveExploreSum(BM _bm, int exploreSum) {
        if(exploreSum==this.exploreSum) 
            return;
        this.exploreSum = exploreSum;
        saveField(_bm, "exploreSum", exploreSum);
    }

    // PVP日志最新创建时间
    public long getPvpLogLastCreated() { return this.pvpLogLastCreated; }
    public void setPvpLogLastCreated(BM _bm, long pvpLogLastCreated) {
        if(pvpLogLastCreated==this.pvpLogLastCreated) 
            return;
        this.pvpLogLastCreated = pvpLogLastCreated; 
        markField(_bm, FIELD_pvpLogLastCreated); 
    }
    public void savePvpLogLastCreated(BM _bm, long pvpLogLastCreated) {
        if(pvpLogLastCreated==this.pvpLogLastCreated) 
            return;
        this.pvpLogLastCreated = pvpLogLastCreated;
        saveField(_bm, "pvpLogLastCreated", pvpLogLastCreated);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `lvl` = '").append(lvl).append("',");
        sBuilder.append(" `refreshEvents` = '").append(refreshEvents ? 1 : 0).append("',");
        sBuilder.append(" `exploreSum` = '").append(exploreSum).append("',");
        sBuilder.append(" `pvpLogLastCreated` = '").append(pvpLogLastCreated).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_lvl)) sBuilder.append(" `lvl` = '").append(lvl).append("',");
        if(isFieldMarked(FIELD_refreshEvents)) sBuilder.append(" `refreshEvents` = '").append(refreshEvents ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_exploreSum)) sBuilder.append(" `exploreSum` = '").append(exploreSum).append("',");
        if(isFieldMarked(FIELD_pvpLogLastCreated)) sBuilder.append(" `pvpLogLastCreated` = '").append(pvpLogLastCreated).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mars_explore` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`lvl` int(11) NOT NULL DEFAULT '0' COMMENT '探索等级',"
                + "`refreshEvents` tinyint(1) NOT NULL DEFAULT '0' COMMENT '已刷新探索事件',"
                + "`exploreSum` int(11) NOT NULL DEFAULT '0' COMMENT '探索次数',"
                + "`pvpLogLastCreated` bigint(20) NOT NULL DEFAULT '0' COMMENT 'PVP日志最新创建时间',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家火星探险数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=4;//lvl
        _size+=1;//refreshEvents
        _size+=4;//exploreSum
        _size+=8;//pvpLogLastCreated
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putInt(lvl);
        buff.put((byte)(refreshEvents?1:0));
        buff.putInt(exploreSum);
        buff.putLong(pvpLogLastCreated);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        lvl=buff.getInt();
        refreshEvents=(buff.get()==1);
        exploreSum=buff.getInt();
        pvpLogLastCreated=buff.getLong(); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
