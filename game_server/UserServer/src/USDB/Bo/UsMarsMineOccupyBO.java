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

@RefBo(isIdAuto = true)
public class UsMarsMineOccupyBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_mineInstaceId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "mineInstaceId", comment = "矿实例ID")
    private long mineInstaceId;

    public static final int FIELD_cid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_startCollectMs =2;
    @DataBaseField(type = "bigint(20)", fieldname = "startCollectMs", comment = "玩家开始采集时间（毫秒）")
    private long startCollectMs;

    public static final int FIELD_collectSpeed =3;
    @DataBaseField(type = "bigint(20)", fieldname = "collectSpeed", comment = "玩家采集速度")
    private long collectSpeed;

    public static final int FIELD_occupyPlayer =4;
    @DataBaseField(type = "varbinary(1024)", fieldname = "occupyPlayer", comment = "占领玩家")
    private byte[] occupyPlayer;

    public UsMarsMineOccupyBO() {
        id = 0;
        mineInstaceId = 0L;
        cid = 0L;
        startCollectMs = 0L;
        collectSpeed = 0L;
        occupyPlayer = null;
    }

    public UsMarsMineOccupyBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        mineInstaceId = rs.getLong(2);
        cid = rs.getLong(3);
        startCollectMs = rs.getLong(4);
        collectSpeed = rs.getLong(5);
        occupyPlayer = rs.getBytes(6);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new UsMarsMineOccupyBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `mineInstaceId`, `cid`, `startCollectMs`, `collectSpeed`, `occupyPlayer`";
    }

    @Override
    public String getTableName() {
        return "`us_mars_mine_occupy`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(mineInstaceId).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(startCollectMs).append("', ");
        strBuf.append("'").append(collectSpeed).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(occupyPlayer);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_occupyPlayer)) ret.add(occupyPlayer);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 矿实例ID
    public long getMineInstaceId() { return this.mineInstaceId; }
    public void setMineInstaceId(BM _bm, long mineInstaceId) {
        if(mineInstaceId==this.mineInstaceId) 
            return;
        this.mineInstaceId = mineInstaceId; 
        markField(_bm, FIELD_mineInstaceId); 
    }
    public void saveMineInstaceId(BM _bm, long mineInstaceId) {
        if(mineInstaceId==this.mineInstaceId) 
            return;
        this.mineInstaceId = mineInstaceId;
        saveField(_bm, "mineInstaceId", mineInstaceId);
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

    // 玩家开始采集时间（毫秒）
    public long getStartCollectMs() { return this.startCollectMs; }
    public void setStartCollectMs(BM _bm, long startCollectMs) {
        if(startCollectMs==this.startCollectMs) 
            return;
        this.startCollectMs = startCollectMs; 
        markField(_bm, FIELD_startCollectMs); 
    }
    public void saveStartCollectMs(BM _bm, long startCollectMs) {
        if(startCollectMs==this.startCollectMs) 
            return;
        this.startCollectMs = startCollectMs;
        saveField(_bm, "startCollectMs", startCollectMs);
    }

    // 玩家采集速度
    public long getCollectSpeed() { return this.collectSpeed; }
    public void setCollectSpeed(BM _bm, long collectSpeed) {
        if(collectSpeed==this.collectSpeed) 
            return;
        this.collectSpeed = collectSpeed; 
        markField(_bm, FIELD_collectSpeed); 
    }
    public void saveCollectSpeed(BM _bm, long collectSpeed) {
        if(collectSpeed==this.collectSpeed) 
            return;
        this.collectSpeed = collectSpeed;
        saveField(_bm, "collectSpeed", collectSpeed);
    }

    // 占领玩家
    public byte[] getOccupyPlayer() { return this.occupyPlayer; }
    public void setOccupyPlayer(BM _bm, byte[] occupyPlayer) {
        if(occupyPlayer==this.occupyPlayer) 
            return;
        this.occupyPlayer = occupyPlayer; 
        markField(_bm, FIELD_occupyPlayer); 
    }
    public void saveOccupyPlayer(BM _bm, byte[] occupyPlayer) {
        if(occupyPlayer==this.occupyPlayer) 
            return;
        this.occupyPlayer = occupyPlayer;
        saveFieldBytes(_bm, "occupyPlayer", occupyPlayer);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `mineInstaceId` = '").append(mineInstaceId).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `startCollectMs` = '").append(startCollectMs).append("',");
        sBuilder.append(" `collectSpeed` = '").append(collectSpeed).append("',");
        sBuilder.append(" `occupyPlayer` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_mineInstaceId)) sBuilder.append(" `mineInstaceId` = '").append(mineInstaceId).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_startCollectMs)) sBuilder.append(" `startCollectMs` = '").append(startCollectMs).append("',");
        if(isFieldMarked(FIELD_collectSpeed)) sBuilder.append(" `collectSpeed` = '").append(collectSpeed).append("',");
        if(isFieldMarked(FIELD_occupyPlayer)) sBuilder.append(" `occupyPlayer` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `us_mars_mine_occupy` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`mineInstaceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '矿实例ID',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`startCollectMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家开始采集时间（毫秒）',"
                + "`collectSpeed` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家采集速度',"
                + "`occupyPlayer` varbinary(1024) NOT NULL DEFAULT '' COMMENT '占领玩家',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='本服火星矿产占领玩家数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//mineInstaceId
        _size+=8;//cid
        _size+=8;//startCollectMs
        _size+=8;//collectSpeed
        _size+=2;_size+=occupyPlayer.length;//occupyPlayer
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(mineInstaceId);
        buff.putLong(cid);
        buff.putLong(startCollectMs);
        buff.putLong(collectSpeed);
        buff.putShort((short)(occupyPlayer == null ? 0 : occupyPlayer.length));if(null != occupyPlayer){buff.put(occupyPlayer);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        mineInstaceId=buff.getLong();
        cid=buff.getLong();
        startCollectMs=buff.getLong();
        collectSpeed=buff.getLong();
        int occupyPlayer_count = buff.getShort();if(occupyPlayer_count>0){occupyPlayer = new byte[occupyPlayer_count];buff.get(occupyPlayer);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
