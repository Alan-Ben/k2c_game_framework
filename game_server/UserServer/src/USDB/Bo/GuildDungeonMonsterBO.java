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
public class GuildDungeonMonsterBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_guildId =0;
    @DataBaseField(type = "bigint(20)", fieldname = "guildId", comment = "联盟ID")
    private long guildId;

    public static final int FIELD_instaceId =1;
    @DataBaseField(type = "bigint(20)", fieldname = "instaceId", comment = "联盟副本实例ID")
    private long instaceId;

    public static final int FIELD_monsterId =2;
    @DataBaseField(type = "bigint(20)", fieldname = "monsterId", comment = "怪物ID")
    private long monsterId;

    public static final int FIELD_isReward =3;
    @DataBaseField(type = "tinyint(1)", fieldname = "isReward", comment = "是否怪物奖励")
    private boolean isReward;

    public static final int FIELD_hp =4;
    @DataBaseField(type = "bigint(20)", fieldname = "hp", comment = "怪物当前血量")
    private long hp;

    public static final int FIELD_gainedCidList =5;
    @DataBaseField(type = "blob", fieldname = "gainedCidList", comment = "已领取怪物奖励的玩家列表")
    private byte[] gainedCidList;

    public static final int FIELD_isTag =6;
    @DataBaseField(type = "tinyint(1)", fieldname = "isTag", comment = "是否标记")
    private boolean isTag;

    public GuildDungeonMonsterBO() {
        id = 0;
        guildId = 0L;
        instaceId = 0L;
        monsterId = 0L;
        isReward = false;
        hp = 0L;
        gainedCidList = null;
        isTag = false;
    }

    public GuildDungeonMonsterBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        guildId = rs.getLong(2);
        instaceId = rs.getLong(3);
        monsterId = rs.getLong(4);
        isReward = rs.getBoolean(5);
        hp = rs.getLong(6);
        gainedCidList = rs.getBytes(7);
        isTag = rs.getBoolean(8);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new GuildDungeonMonsterBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `guildId`, `instaceId`, `monsterId`, `isReward`, `hp`, `gainedCidList`, `isTag`";
    }

    @Override
    public String getTableName() {
        return "`guild_dungeon_monster`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(guildId).append("', ");
        strBuf.append("'").append(instaceId).append("', ");
        strBuf.append("'").append(monsterId).append("', ");
        strBuf.append("'").append(isReward ? 1 : 0).append("', ");
        strBuf.append("'").append(hp).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(isTag ? 1 : 0).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(gainedCidList);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_gainedCidList)) ret.add(gainedCidList);         return ret;
    }
    
    @Override
    public void setId(long iID) {
        id = iID;
    }

    @Override
    public long getId() {
        return id;
    }

    // 联盟ID
    public long getGuildId() { return this.guildId; }
    public void setGuildId(BM _bm, long guildId) {
        if(guildId==this.guildId) 
            return;
        this.guildId = guildId; 
        markField(_bm, FIELD_guildId); 
    }
    public void saveGuildId(BM _bm, long guildId) {
        if(guildId==this.guildId) 
            return;
        this.guildId = guildId;
        saveField(_bm, "guildId", guildId);
    }

    // 联盟副本实例ID
    public long getInstaceId() { return this.instaceId; }
    public void setInstaceId(BM _bm, long instaceId) {
        if(instaceId==this.instaceId) 
            return;
        this.instaceId = instaceId; 
        markField(_bm, FIELD_instaceId); 
    }
    public void saveInstaceId(BM _bm, long instaceId) {
        if(instaceId==this.instaceId) 
            return;
        this.instaceId = instaceId;
        saveField(_bm, "instaceId", instaceId);
    }

    // 怪物ID
    public long getMonsterId() { return this.monsterId; }
    public void setMonsterId(BM _bm, long monsterId) {
        if(monsterId==this.monsterId) 
            return;
        this.monsterId = monsterId; 
        markField(_bm, FIELD_monsterId); 
    }
    public void saveMonsterId(BM _bm, long monsterId) {
        if(monsterId==this.monsterId) 
            return;
        this.monsterId = monsterId;
        saveField(_bm, "monsterId", monsterId);
    }

    // 是否怪物奖励
    public boolean getIsReward() { return this.isReward; }
    public void setIsReward(BM _bm, boolean isReward) {
        if(isReward==this.isReward) 
            return;
        this.isReward = isReward; 
        markField(_bm, FIELD_isReward); 
    }
    public void saveIsReward(BM _bm, boolean isReward) {
        if(isReward==this.isReward) 
            return;
        this.isReward = isReward;
        saveField(_bm, "isReward", isReward ? 1 : 0);
    }

    // 怪物当前血量
    public long getHp() { return this.hp; }
    public void setHp(BM _bm, long hp) {
        if(hp==this.hp) 
            return;
        this.hp = hp; 
        markField(_bm, FIELD_hp); 
    }
    public void saveHp(BM _bm, long hp) {
        if(hp==this.hp) 
            return;
        this.hp = hp;
        saveField(_bm, "hp", hp);
    }

    // 已领取怪物奖励的玩家列表
    public byte[] getGainedCidList() { return this.gainedCidList; }
    public void setGainedCidList(BM _bm, byte[] gainedCidList) {
        if(gainedCidList==this.gainedCidList) 
            return;
        this.gainedCidList = gainedCidList; 
        markField(_bm, FIELD_gainedCidList); 
    }
    public void saveGainedCidList(BM _bm, byte[] gainedCidList) {
        if(gainedCidList==this.gainedCidList) 
            return;
        this.gainedCidList = gainedCidList;
        saveFieldBytes(_bm, "gainedCidList", gainedCidList);
    }

    // 是否标记
    public boolean getIsTag() { return this.isTag; }
    public void setIsTag(BM _bm, boolean isTag) {
        if(isTag==this.isTag) 
            return;
        this.isTag = isTag; 
        markField(_bm, FIELD_isTag); 
    }
    public void saveIsTag(BM _bm, boolean isTag) {
        if(isTag==this.isTag) 
            return;
        this.isTag = isTag;
        saveField(_bm, "isTag", isTag ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `guildId` = '").append(guildId).append("',");
        sBuilder.append(" `instaceId` = '").append(instaceId).append("',");
        sBuilder.append(" `monsterId` = '").append(monsterId).append("',");
        sBuilder.append(" `isReward` = '").append(isReward ? 1 : 0).append("',");
        sBuilder.append(" `hp` = '").append(hp).append("',");
        sBuilder.append(" `gainedCidList` = ?,");
        sBuilder.append(" `isTag` = '").append(isTag ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_guildId)) sBuilder.append(" `guildId` = '").append(guildId).append("',");
        if(isFieldMarked(FIELD_instaceId)) sBuilder.append(" `instaceId` = '").append(instaceId).append("',");
        if(isFieldMarked(FIELD_monsterId)) sBuilder.append(" `monsterId` = '").append(monsterId).append("',");
        if(isFieldMarked(FIELD_isReward)) sBuilder.append(" `isReward` = '").append(isReward ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_hp)) sBuilder.append(" `hp` = '").append(hp).append("',");
        if(isFieldMarked(FIELD_gainedCidList)) sBuilder.append(" `gainedCidList` = ?,");
        if(isFieldMarked(FIELD_isTag)) sBuilder.append(" `isTag` = '").append(isTag ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `guild_dungeon_monster` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`guildId` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟ID',"
                + "`instaceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '联盟副本实例ID',"
                + "`monsterId` bigint(20) NOT NULL DEFAULT '0' COMMENT '怪物ID',"
                + "`isReward` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否怪物奖励',"
                + "`hp` bigint(20) NOT NULL DEFAULT '0' COMMENT '怪物当前血量',"
                + "`gainedCidList` blob NULL COMMENT '已领取怪物奖励的玩家列表',"
                + "`isTag` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否标记',"
                + "KEY `guildId` (`guildId`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='联盟副本数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//guildId
        _size+=8;//instaceId
        _size+=8;//monsterId
        _size+=1;//isReward
        _size+=8;//hp
        _size+=2;_size+=gainedCidList.length;//gainedCidList
        _size+=1;//isTag
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(guildId);
        buff.putLong(instaceId);
        buff.putLong(monsterId);
        buff.put((byte)(isReward?1:0));
        buff.putLong(hp);
        buff.putShort((short)(gainedCidList == null ? 0 : gainedCidList.length));if(null != gainedCidList){buff.put(gainedCidList);}
        buff.put((byte)(isTag?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        guildId=buff.getLong();
        instaceId=buff.getLong();
        monsterId=buff.getLong();
        isReward=(buff.get()==1);
        hp=buff.getLong();
        int gainedCidList_count = buff.getShort();if(gainedCidList_count>0){gainedCidList = new byte[gainedCidList_count];buff.get(gainedCidList);}
        isTag=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
