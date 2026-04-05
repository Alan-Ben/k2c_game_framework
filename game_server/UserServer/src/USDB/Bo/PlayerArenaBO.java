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
public class PlayerArenaBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_last_reset_time_ms =1;
    @DataBaseField(type = "bigint(20)", fieldname = "last_reset_time_ms", comment = "上次重置计数时间")
    private long last_reset_time_ms;

    public static final int FIELD_had_select_attack_num =2;
    @DataBaseField(type = "int(11)", fieldname = "had_select_attack_num", comment = "已指定攻击次数")
    private int had_select_attack_num;

    public static final int FIELD_had_random_attack_num =3;
    @DataBaseField(type = "int(11)", fieldname = "had_random_attack_num", comment = "已随机攻击次数")
    private int had_random_attack_num;

    public static final int FIELD_had_buy_random_attack_num =4;
    @DataBaseField(type = "int(11)", fieldname = "had_buy_random_attack_num", comment = "已购买随机攻击次数")
    private int had_buy_random_attack_num;

    public static final int FIELD_had_select_attack_hero_list =5;
    @DataBaseField(type = "blob", fieldname = "had_select_attack_hero_list", comment = "已指定攻击大臣id列表")
    private byte[] had_select_attack_hero_list;

    public static final int FIELD_had_random_attack_hero_list =6;
    @DataBaseField(type = "blob", fieldname = "had_random_attack_hero_list", comment = "已随机攻击大臣id列表")
    private byte[] had_random_attack_hero_list;

    public static final int FIELD_had_attack_opponent_cid_list =7;
    @DataBaseField(type = "blob", fieldname = "had_attack_opponent_cid_list", comment = "已攻击过的对手CID列表(当天)")
    private byte[] had_attack_opponent_cid_list;

    public static final int FIELD_had_unlock_arena =8;
    @DataBaseField(type = "tinyint(1)", fieldname = "had_unlock_arena", comment = "是否解锁竞技场")
    private boolean had_unlock_arena;

    public PlayerArenaBO() {
        id = 0;
        cid = 0L;
        last_reset_time_ms = 0L;
        had_select_attack_num = 0;
        had_random_attack_num = 0;
        had_buy_random_attack_num = 0;
        had_select_attack_hero_list = null;
        had_random_attack_hero_list = null;
        had_attack_opponent_cid_list = null;
        had_unlock_arena = false;
    }

    public PlayerArenaBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        last_reset_time_ms = rs.getLong(3);
        had_select_attack_num = rs.getInt(4);
        had_random_attack_num = rs.getInt(5);
        had_buy_random_attack_num = rs.getInt(6);
        had_select_attack_hero_list = rs.getBytes(7);
        had_random_attack_hero_list = rs.getBytes(8);
        had_attack_opponent_cid_list = rs.getBytes(9);
        had_unlock_arena = rs.getBoolean(10);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerArenaBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `last_reset_time_ms`, `had_select_attack_num`, `had_random_attack_num`, `had_buy_random_attack_num`, `had_select_attack_hero_list`, `had_random_attack_hero_list`, `had_attack_opponent_cid_list`, `had_unlock_arena`";
    }

    @Override
    public String getTableName() {
        return "`player_arena`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(last_reset_time_ms).append("', ");
        strBuf.append("'").append(had_select_attack_num).append("', ");
        strBuf.append("'").append(had_random_attack_num).append("', ");
        strBuf.append("'").append(had_buy_random_attack_num).append("', ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.append("'").append(had_unlock_arena ? 1 : 0).append("', ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(had_select_attack_hero_list); 
        ret.add(had_random_attack_hero_list); 
        ret.add(had_attack_opponent_cid_list);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_had_select_attack_hero_list)) ret.add(had_select_attack_hero_list); 
        if(isFieldMarked(FIELD_had_random_attack_hero_list)) ret.add(had_random_attack_hero_list); 
        if(isFieldMarked(FIELD_had_attack_opponent_cid_list)) ret.add(had_attack_opponent_cid_list);         return ret;
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

    // 上次重置计数时间
    public long getLastResetTimeMs() { return this.last_reset_time_ms; }
    public void setLastResetTimeMs(BM _bm, long last_reset_time_ms) {
        if(last_reset_time_ms==this.last_reset_time_ms) 
            return;
        this.last_reset_time_ms = last_reset_time_ms; 
        markField(_bm, FIELD_last_reset_time_ms); 
    }
    public void saveLastResetTimeMs(BM _bm, long last_reset_time_ms) {
        if(last_reset_time_ms==this.last_reset_time_ms) 
            return;
        this.last_reset_time_ms = last_reset_time_ms;
        saveField(_bm, "last_reset_time_ms", last_reset_time_ms);
    }

    // 已指定攻击次数
    public int getHadSelectAttackNum() { return this.had_select_attack_num; }
    public void setHadSelectAttackNum(BM _bm, int had_select_attack_num) {
        if(had_select_attack_num==this.had_select_attack_num) 
            return;
        this.had_select_attack_num = had_select_attack_num; 
        markField(_bm, FIELD_had_select_attack_num); 
    }
    public void saveHadSelectAttackNum(BM _bm, int had_select_attack_num) {
        if(had_select_attack_num==this.had_select_attack_num) 
            return;
        this.had_select_attack_num = had_select_attack_num;
        saveField(_bm, "had_select_attack_num", had_select_attack_num);
    }

    // 已随机攻击次数
    public int getHadRandomAttackNum() { return this.had_random_attack_num; }
    public void setHadRandomAttackNum(BM _bm, int had_random_attack_num) {
        if(had_random_attack_num==this.had_random_attack_num) 
            return;
        this.had_random_attack_num = had_random_attack_num; 
        markField(_bm, FIELD_had_random_attack_num); 
    }
    public void saveHadRandomAttackNum(BM _bm, int had_random_attack_num) {
        if(had_random_attack_num==this.had_random_attack_num) 
            return;
        this.had_random_attack_num = had_random_attack_num;
        saveField(_bm, "had_random_attack_num", had_random_attack_num);
    }

    // 已购买随机攻击次数
    public int getHadBuyRandomAttackNum() { return this.had_buy_random_attack_num; }
    public void setHadBuyRandomAttackNum(BM _bm, int had_buy_random_attack_num) {
        if(had_buy_random_attack_num==this.had_buy_random_attack_num) 
            return;
        this.had_buy_random_attack_num = had_buy_random_attack_num; 
        markField(_bm, FIELD_had_buy_random_attack_num); 
    }
    public void saveHadBuyRandomAttackNum(BM _bm, int had_buy_random_attack_num) {
        if(had_buy_random_attack_num==this.had_buy_random_attack_num) 
            return;
        this.had_buy_random_attack_num = had_buy_random_attack_num;
        saveField(_bm, "had_buy_random_attack_num", had_buy_random_attack_num);
    }

    // 已指定攻击大臣id列表
    public byte[] getHadSelectAttackHeroList() { return this.had_select_attack_hero_list; }
    public void setHadSelectAttackHeroList(BM _bm, byte[] had_select_attack_hero_list) {
        if(had_select_attack_hero_list==this.had_select_attack_hero_list) 
            return;
        this.had_select_attack_hero_list = had_select_attack_hero_list; 
        markField(_bm, FIELD_had_select_attack_hero_list); 
    }
    public void saveHadSelectAttackHeroList(BM _bm, byte[] had_select_attack_hero_list) {
        if(had_select_attack_hero_list==this.had_select_attack_hero_list) 
            return;
        this.had_select_attack_hero_list = had_select_attack_hero_list;
        saveFieldBytes(_bm, "had_select_attack_hero_list", had_select_attack_hero_list);
    }

    // 已随机攻击大臣id列表
    public byte[] getHadRandomAttackHeroList() { return this.had_random_attack_hero_list; }
    public void setHadRandomAttackHeroList(BM _bm, byte[] had_random_attack_hero_list) {
        if(had_random_attack_hero_list==this.had_random_attack_hero_list) 
            return;
        this.had_random_attack_hero_list = had_random_attack_hero_list; 
        markField(_bm, FIELD_had_random_attack_hero_list); 
    }
    public void saveHadRandomAttackHeroList(BM _bm, byte[] had_random_attack_hero_list) {
        if(had_random_attack_hero_list==this.had_random_attack_hero_list) 
            return;
        this.had_random_attack_hero_list = had_random_attack_hero_list;
        saveFieldBytes(_bm, "had_random_attack_hero_list", had_random_attack_hero_list);
    }

    // 已攻击过的对手CID列表(当天)
    public byte[] getHadAttackOpponentCidList() { return this.had_attack_opponent_cid_list; }
    public void setHadAttackOpponentCidList(BM _bm, byte[] had_attack_opponent_cid_list) {
        if(had_attack_opponent_cid_list==this.had_attack_opponent_cid_list) 
            return;
        this.had_attack_opponent_cid_list = had_attack_opponent_cid_list; 
        markField(_bm, FIELD_had_attack_opponent_cid_list); 
    }
    public void saveHadAttackOpponentCidList(BM _bm, byte[] had_attack_opponent_cid_list) {
        if(had_attack_opponent_cid_list==this.had_attack_opponent_cid_list) 
            return;
        this.had_attack_opponent_cid_list = had_attack_opponent_cid_list;
        saveFieldBytes(_bm, "had_attack_opponent_cid_list", had_attack_opponent_cid_list);
    }

    // 是否解锁竞技场
    public boolean getHadUnlockArena() { return this.had_unlock_arena; }
    public void setHadUnlockArena(BM _bm, boolean had_unlock_arena) {
        if(had_unlock_arena==this.had_unlock_arena) 
            return;
        this.had_unlock_arena = had_unlock_arena; 
        markField(_bm, FIELD_had_unlock_arena); 
    }
    public void saveHadUnlockArena(BM _bm, boolean had_unlock_arena) {
        if(had_unlock_arena==this.had_unlock_arena) 
            return;
        this.had_unlock_arena = had_unlock_arena;
        saveField(_bm, "had_unlock_arena", had_unlock_arena ? 1 : 0);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `last_reset_time_ms` = '").append(last_reset_time_ms).append("',");
        sBuilder.append(" `had_select_attack_num` = '").append(had_select_attack_num).append("',");
        sBuilder.append(" `had_random_attack_num` = '").append(had_random_attack_num).append("',");
        sBuilder.append(" `had_buy_random_attack_num` = '").append(had_buy_random_attack_num).append("',");
        sBuilder.append(" `had_select_attack_hero_list` = ?,");
        sBuilder.append(" `had_random_attack_hero_list` = ?,");
        sBuilder.append(" `had_attack_opponent_cid_list` = ?,");
        sBuilder.append(" `had_unlock_arena` = '").append(had_unlock_arena ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_last_reset_time_ms)) sBuilder.append(" `last_reset_time_ms` = '").append(last_reset_time_ms).append("',");
        if(isFieldMarked(FIELD_had_select_attack_num)) sBuilder.append(" `had_select_attack_num` = '").append(had_select_attack_num).append("',");
        if(isFieldMarked(FIELD_had_random_attack_num)) sBuilder.append(" `had_random_attack_num` = '").append(had_random_attack_num).append("',");
        if(isFieldMarked(FIELD_had_buy_random_attack_num)) sBuilder.append(" `had_buy_random_attack_num` = '").append(had_buy_random_attack_num).append("',");
        if(isFieldMarked(FIELD_had_select_attack_hero_list)) sBuilder.append(" `had_select_attack_hero_list` = ?,");
        if(isFieldMarked(FIELD_had_random_attack_hero_list)) sBuilder.append(" `had_random_attack_hero_list` = ?,");
        if(isFieldMarked(FIELD_had_attack_opponent_cid_list)) sBuilder.append(" `had_attack_opponent_cid_list` = ?,");
        if(isFieldMarked(FIELD_had_unlock_arena)) sBuilder.append(" `had_unlock_arena` = '").append(had_unlock_arena ? 1 : 0).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_arena` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`last_reset_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '上次重置计数时间',"
                + "`had_select_attack_num` int(11) NOT NULL DEFAULT '0' COMMENT '已指定攻击次数',"
                + "`had_random_attack_num` int(11) NOT NULL DEFAULT '0' COMMENT '已随机攻击次数',"
                + "`had_buy_random_attack_num` int(11) NOT NULL DEFAULT '0' COMMENT '已购买随机攻击次数',"
                + "`had_select_attack_hero_list` blob NULL COMMENT '已指定攻击大臣id列表',"
                + "`had_random_attack_hero_list` blob NULL COMMENT '已随机攻击大臣id列表',"
                + "`had_attack_opponent_cid_list` blob NULL COMMENT '已攻击过的对手CID列表(当天)',"
                + "`had_unlock_arena` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否解锁竞技场',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家竞技场数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//last_reset_time_ms
        _size+=4;//had_select_attack_num
        _size+=4;//had_random_attack_num
        _size+=4;//had_buy_random_attack_num
        _size+=2;_size+=had_select_attack_hero_list.length;//had_select_attack_hero_list
        _size+=2;_size+=had_random_attack_hero_list.length;//had_random_attack_hero_list
        _size+=2;_size+=had_attack_opponent_cid_list.length;//had_attack_opponent_cid_list
        _size+=1;//had_unlock_arena
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(last_reset_time_ms);
        buff.putInt(had_select_attack_num);
        buff.putInt(had_random_attack_num);
        buff.putInt(had_buy_random_attack_num);
        buff.putShort((short)(had_select_attack_hero_list == null ? 0 : had_select_attack_hero_list.length));if(null != had_select_attack_hero_list){buff.put(had_select_attack_hero_list);}
        buff.putShort((short)(had_random_attack_hero_list == null ? 0 : had_random_attack_hero_list.length));if(null != had_random_attack_hero_list){buff.put(had_random_attack_hero_list);}
        buff.putShort((short)(had_attack_opponent_cid_list == null ? 0 : had_attack_opponent_cid_list.length));if(null != had_attack_opponent_cid_list){buff.put(had_attack_opponent_cid_list);}
        buff.put((byte)(had_unlock_arena?1:0));        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        last_reset_time_ms=buff.getLong();
        had_select_attack_num=buff.getInt();
        had_random_attack_num=buff.getInt();
        had_buy_random_attack_num=buff.getInt();
        int had_select_attack_hero_list_count = buff.getShort();if(had_select_attack_hero_list_count>0){had_select_attack_hero_list = new byte[had_select_attack_hero_list_count];buff.get(had_select_attack_hero_list);}
        int had_random_attack_hero_list_count = buff.getShort();if(had_random_attack_hero_list_count>0){had_random_attack_hero_list = new byte[had_random_attack_hero_list_count];buff.get(had_random_attack_hero_list);}
        int had_attack_opponent_cid_list_count = buff.getShort();if(had_attack_opponent_cid_list_count>0){had_attack_opponent_cid_list = new byte[had_attack_opponent_cid_list_count];buff.get(had_attack_opponent_cid_list);}
        had_unlock_arena=(buff.get()==1); 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
