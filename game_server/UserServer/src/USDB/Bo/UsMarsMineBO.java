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

@RefBo(isIdAuto = false)
public class UsMarsMineBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_refId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "refId", comment = "配置ID")
    private long refId;

    public static final int FIELD_endShowMs =1;
    @DataBaseField(type = "bigint(20)", fieldname = "endShowMs", comment = "结束展示时间（毫秒）")
    private long endShowMs;

    public static final int FIELD_remainNum =2;
    @DataBaseField(type = "bigint(20)", fieldname = "remainNum", comment = "剩余资源数量")
    private long remainNum;

    public static final int FIELD_cid =3;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_teamId =4;
    @DataBaseField(type = "bigint(20)", fieldname = "teamId", comment = "玩家队伍ID")
    private long teamId;

    public static final int FIELD_teamSoldierPower =5;
    @DataBaseField(type = "bigint(20)", fieldname = "teamSoldierPower", comment = "玩家队伍单兵实力")
    private long teamSoldierPower;

    public static final int FIELD_teamTroopNum =6;
    @DataBaseField(type = "bigint(20)", fieldname = "teamTroopNum", comment = "玩家队伍带兵量")
    private long teamTroopNum;

    public static final int FIELD_teamLossValue =7;
    @DataBaseField(type = "bigint(20)", fieldname = "teamLossValue", comment = "玩家队伍损耗数量")
    private long teamLossValue;

    public static final int FIELD_startCollectMs =8;
    @DataBaseField(type = "bigint(20)", fieldname = "startCollectMs", comment = "玩家开始采集时间（毫秒）")
    private long startCollectMs;

    public static final int FIELD_collectSpeed =9;
    @DataBaseField(type = "bigint(20)", fieldname = "collectSpeed", comment = "玩家采集速度（秒）")
    private long collectSpeed;

    public static final int FIELD_guild_id =10;
    @DataBaseField(type = "bigint(20)", fieldname = "guild_id", comment = "玩家公会ID")
    private long guild_id;

    public static final int FIELD_cname =11;
    @DataBaseField(type = "varchar(64)", fieldname = "cname", comment = "玩家名称")
    private String cname;

    public static final int FIELD_guild_simple_name =12;
    @DataBaseField(type = "varchar(128)", fieldname = "guild_simple_name", comment = "公会简称")
    private String guild_simple_name;

    public static final int FIELD_attacked_guild_ids =13;
    @DataBaseField(type = "blob", fieldname = "attacked_guild_ids", comment = "进攻过的联盟ID列表")
    private byte[] attacked_guild_ids;

    public UsMarsMineBO() {
        id = 0;
        refId = 0L;
        endShowMs = 0L;
        remainNum = 0L;
        cid = 0L;
        teamId = 0L;
        teamSoldierPower = 0L;
        teamTroopNum = 0L;
        teamLossValue = 0L;
        startCollectMs = 0L;
        collectSpeed = 0L;
        guild_id = 0L;
        cname = "";
        guild_simple_name = "";
        attacked_guild_ids = null;
    }

    public UsMarsMineBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        refId = rs.getLong(2);
        endShowMs = rs.getLong(3);
        remainNum = rs.getLong(4);
        cid = rs.getLong(5);
        teamId = rs.getLong(6);
        teamSoldierPower = rs.getLong(7);
        teamTroopNum = rs.getLong(8);
        teamLossValue = rs.getLong(9);
        startCollectMs = rs.getLong(10);
        collectSpeed = rs.getLong(11);
        guild_id = rs.getLong(12);
        cname = rs.getString(13);
        guild_simple_name = rs.getString(14);
        attacked_guild_ids = rs.getBytes(15);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new UsMarsMineBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `refId`, `endShowMs`, `remainNum`, `cid`, `teamId`, `teamSoldierPower`, `teamTroopNum`, `teamLossValue`, `startCollectMs`, `collectSpeed`, `guild_id`, `cname`, `guild_simple_name`, `attacked_guild_ids`";
    }

    @Override
    public String getTableName() {
        return "`us_mars_mine`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(refId).append("', ");
        strBuf.append("'").append(endShowMs).append("', ");
        strBuf.append("'").append(remainNum).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(teamId).append("', ");
        strBuf.append("'").append(teamSoldierPower).append("', ");
        strBuf.append("'").append(teamTroopNum).append("', ");
        strBuf.append("'").append(teamLossValue).append("', ");
        strBuf.append("'").append(startCollectMs).append("', ");
        strBuf.append("'").append(collectSpeed).append("', ");
        strBuf.append("'").append(guild_id).append("', ");
        strBuf.append("'").append(cname == null ? null : cname.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(guild_simple_name == null ? null : guild_simple_name.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(attacked_guild_ids);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_attacked_guild_ids)) ret.add(attacked_guild_ids);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 配置ID
    public long getRefId() { return this.refId; }
    public void setRefId(BM _bm, long refId) {
        if(refId==this.refId) 
            return;
        this.refId = refId; 
        markField(_bm, FIELD_refId); 
    }
    public void saveRefId(BM _bm, long refId) {
        if(refId==this.refId) 
            return;
        this.refId = refId;
        saveField(_bm, "refId", refId);
    }

    // 结束展示时间（毫秒）
    public long getEndShowMs() { return this.endShowMs; }
    public void setEndShowMs(BM _bm, long endShowMs) {
        if(endShowMs==this.endShowMs) 
            return;
        this.endShowMs = endShowMs; 
        markField(_bm, FIELD_endShowMs); 
    }
    public void saveEndShowMs(BM _bm, long endShowMs) {
        if(endShowMs==this.endShowMs) 
            return;
        this.endShowMs = endShowMs;
        saveField(_bm, "endShowMs", endShowMs);
    }

    // 剩余资源数量
    public long getRemainNum() { return this.remainNum; }
    public void setRemainNum(BM _bm, long remainNum) {
        if(remainNum==this.remainNum) 
            return;
        this.remainNum = remainNum; 
        markField(_bm, FIELD_remainNum); 
    }
    public void saveRemainNum(BM _bm, long remainNum) {
        if(remainNum==this.remainNum) 
            return;
        this.remainNum = remainNum;
        saveField(_bm, "remainNum", remainNum);
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

    // 玩家队伍ID
    public long getTeamId() { return this.teamId; }
    public void setTeamId(BM _bm, long teamId) {
        if(teamId==this.teamId) 
            return;
        this.teamId = teamId; 
        markField(_bm, FIELD_teamId); 
    }
    public void saveTeamId(BM _bm, long teamId) {
        if(teamId==this.teamId) 
            return;
        this.teamId = teamId;
        saveField(_bm, "teamId", teamId);
    }

    // 玩家队伍单兵实力
    public long getTeamSoldierPower() { return this.teamSoldierPower; }
    public void setTeamSoldierPower(BM _bm, long teamSoldierPower) {
        if(teamSoldierPower==this.teamSoldierPower) 
            return;
        this.teamSoldierPower = teamSoldierPower; 
        markField(_bm, FIELD_teamSoldierPower); 
    }
    public void saveTeamSoldierPower(BM _bm, long teamSoldierPower) {
        if(teamSoldierPower==this.teamSoldierPower) 
            return;
        this.teamSoldierPower = teamSoldierPower;
        saveField(_bm, "teamSoldierPower", teamSoldierPower);
    }

    // 玩家队伍带兵量
    public long getTeamTroopNum() { return this.teamTroopNum; }
    public void setTeamTroopNum(BM _bm, long teamTroopNum) {
        if(teamTroopNum==this.teamTroopNum) 
            return;
        this.teamTroopNum = teamTroopNum; 
        markField(_bm, FIELD_teamTroopNum); 
    }
    public void saveTeamTroopNum(BM _bm, long teamTroopNum) {
        if(teamTroopNum==this.teamTroopNum) 
            return;
        this.teamTroopNum = teamTroopNum;
        saveField(_bm, "teamTroopNum", teamTroopNum);
    }

    // 玩家队伍损耗数量
    public long getTeamLossValue() { return this.teamLossValue; }
    public void setTeamLossValue(BM _bm, long teamLossValue) {
        if(teamLossValue==this.teamLossValue) 
            return;
        this.teamLossValue = teamLossValue; 
        markField(_bm, FIELD_teamLossValue); 
    }
    public void saveTeamLossValue(BM _bm, long teamLossValue) {
        if(teamLossValue==this.teamLossValue) 
            return;
        this.teamLossValue = teamLossValue;
        saveField(_bm, "teamLossValue", teamLossValue);
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

    // 玩家采集速度（秒）
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

    // 玩家公会ID
    public long getGuildId() { return this.guild_id; }
    public void setGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id; 
        markField(_bm, FIELD_guild_id); 
    }
    public void saveGuildId(BM _bm, long guild_id) {
        if(guild_id==this.guild_id) 
            return;
        this.guild_id = guild_id;
        saveField(_bm, "guild_id", guild_id);
    }

    // 玩家名称
    public String getCname() { return this.cname; }
    public void setCname(BM _bm, String cname) {
        if(cname.equals(this.cname)) 
            return;
        this.cname = cname; 
        markField(_bm, FIELD_cname); 
    }
    public void saveCname(BM _bm, String cname) {
        if(cname.equals(this.cname)) 
            return;
        this.cname = cname;
        saveField(_bm, "cname", cname);
    }

    // 公会简称
    public String getGuildSimpleName() { return this.guild_simple_name; }
    public void setGuildSimpleName(BM _bm, String guild_simple_name) {
        if(guild_simple_name.equals(this.guild_simple_name)) 
            return;
        this.guild_simple_name = guild_simple_name; 
        markField(_bm, FIELD_guild_simple_name); 
    }
    public void saveGuildSimpleName(BM _bm, String guild_simple_name) {
        if(guild_simple_name.equals(this.guild_simple_name)) 
            return;
        this.guild_simple_name = guild_simple_name;
        saveField(_bm, "guild_simple_name", guild_simple_name);
    }

    // 进攻过的联盟ID列表
    public byte[] getAttackedGuildIds() { return this.attacked_guild_ids; }
    public void setAttackedGuildIds(BM _bm, byte[] attacked_guild_ids) {
        if(attacked_guild_ids==this.attacked_guild_ids) 
            return;
        this.attacked_guild_ids = attacked_guild_ids; 
        markField(_bm, FIELD_attacked_guild_ids); 
    }
    public void saveAttackedGuildIds(BM _bm, byte[] attacked_guild_ids) {
        if(attacked_guild_ids==this.attacked_guild_ids) 
            return;
        this.attacked_guild_ids = attacked_guild_ids;
        saveFieldBytes(_bm, "attacked_guild_ids", attacked_guild_ids);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `refId` = '").append(refId).append("',");
        sBuilder.append(" `endShowMs` = '").append(endShowMs).append("',");
        sBuilder.append(" `remainNum` = '").append(remainNum).append("',");
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `teamId` = '").append(teamId).append("',");
        sBuilder.append(" `teamSoldierPower` = '").append(teamSoldierPower).append("',");
        sBuilder.append(" `teamTroopNum` = '").append(teamTroopNum).append("',");
        sBuilder.append(" `teamLossValue` = '").append(teamLossValue).append("',");
        sBuilder.append(" `startCollectMs` = '").append(startCollectMs).append("',");
        sBuilder.append(" `collectSpeed` = '").append(collectSpeed).append("',");
        sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        sBuilder.append(" `cname` = '").append(cname == null ? null : cname.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `guild_simple_name` = '").append(guild_simple_name == null ? null : guild_simple_name.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `attacked_guild_ids` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_refId)) sBuilder.append(" `refId` = '").append(refId).append("',");
        if(isFieldMarked(FIELD_endShowMs)) sBuilder.append(" `endShowMs` = '").append(endShowMs).append("',");
        if(isFieldMarked(FIELD_remainNum)) sBuilder.append(" `remainNum` = '").append(remainNum).append("',");
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_teamId)) sBuilder.append(" `teamId` = '").append(teamId).append("',");
        if(isFieldMarked(FIELD_teamSoldierPower)) sBuilder.append(" `teamSoldierPower` = '").append(teamSoldierPower).append("',");
        if(isFieldMarked(FIELD_teamTroopNum)) sBuilder.append(" `teamTroopNum` = '").append(teamTroopNum).append("',");
        if(isFieldMarked(FIELD_teamLossValue)) sBuilder.append(" `teamLossValue` = '").append(teamLossValue).append("',");
        if(isFieldMarked(FIELD_startCollectMs)) sBuilder.append(" `startCollectMs` = '").append(startCollectMs).append("',");
        if(isFieldMarked(FIELD_collectSpeed)) sBuilder.append(" `collectSpeed` = '").append(collectSpeed).append("',");
        if(isFieldMarked(FIELD_guild_id)) sBuilder.append(" `guild_id` = '").append(guild_id).append("',");
        if(isFieldMarked(FIELD_cname)) sBuilder.append(" `cname` = '").append(cname == null ? null : cname.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_guild_simple_name)) sBuilder.append(" `guild_simple_name` = '").append(guild_simple_name == null ? null : guild_simple_name.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_attacked_guild_ids)) sBuilder.append(" `attacked_guild_ids` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `us_mars_mine` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`refId` bigint(20) NOT NULL DEFAULT '0' COMMENT '配置ID',"
                + "`endShowMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '结束展示时间（毫秒）',"
                + "`remainNum` bigint(20) NOT NULL DEFAULT '0' COMMENT '剩余资源数量',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`teamId` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家队伍ID',"
                + "`teamSoldierPower` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家队伍单兵实力',"
                + "`teamTroopNum` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家队伍带兵量',"
                + "`teamLossValue` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家队伍损耗数量',"
                + "`startCollectMs` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家开始采集时间（毫秒）',"
                + "`collectSpeed` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家采集速度（秒）',"
                + "`guild_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家公会ID',"
                + "`cname` varchar(64) NOT NULL DEFAULT '' COMMENT '玩家名称',"
                + "`guild_simple_name` varchar(128) NOT NULL DEFAULT '' COMMENT '公会简称',"
                + "`attacked_guild_ids` blob NULL COMMENT '进攻过的联盟ID列表',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='本服火星矿产数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//refId
        _size+=8;//endShowMs
        _size+=8;//remainNum
        _size+=8;//cid
        _size+=8;//teamId
        _size+=8;//teamSoldierPower
        _size+=8;//teamTroopNum
        _size+=8;//teamLossValue
        _size+=8;//startCollectMs
        _size+=8;//collectSpeed
        _size+=8;//guild_id
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);//cname
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guild_simple_name);//guild_simple_name
        _size+=2;_size+=attacked_guild_ids.length;//attacked_guild_ids
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(refId);
        buff.putLong(endShowMs);
        buff.putLong(remainNum);
        buff.putLong(cid);
        buff.putLong(teamId);
        buff.putLong(teamSoldierPower);
        buff.putLong(teamTroopNum);
        buff.putLong(teamLossValue);
        buff.putLong(startCollectMs);
        buff.putLong(collectSpeed);
        buff.putLong(guild_id);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, cname);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, guild_simple_name);
        buff.putShort((short)(attacked_guild_ids == null ? 0 : attacked_guild_ids.length));if(null != attacked_guild_ids){buff.put(attacked_guild_ids);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        refId=buff.getLong();
        endShowMs=buff.getLong();
        remainNum=buff.getLong();
        cid=buff.getLong();
        teamId=buff.getLong();
        teamSoldierPower=buff.getLong();
        teamTroopNum=buff.getLong();
        teamLossValue=buff.getLong();
        startCollectMs=buff.getLong();
        collectSpeed=buff.getLong();
        guild_id=buff.getLong();
        cname=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        guild_simple_name=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        int attacked_guild_ids_count = buff.getShort();if(attacked_guild_ids_count>0){attacked_guild_ids = new byte[attacked_guild_ids_count];buff.get(attacked_guild_ids);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
