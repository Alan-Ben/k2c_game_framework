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
public class PlayerMiddayDungeonBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_round_start_time_ms =1;
    @DataBaseField(type = "bigint(20)", fieldname = "round_start_time_ms", comment = "副本开始时间")
    private long round_start_time_ms;

    public static final int FIELD_wave =2;
    @DataBaseField(type = "int(11)", fieldname = "wave", comment = "波次")
    private int wave;

    public static final int FIELD_deducted_hp =3;
    @DataBaseField(type = "bigint(20)", fieldname = "deducted_hp", comment = "当前波次已扣除血量")
    private long deducted_hp;

    public static final int FIELD_had_fight_hero_list =4;
    @DataBaseField(type = "blob", fieldname = "had_fight_hero_list", comment = "已战斗英雄列表")
    private byte[] had_fight_hero_list;

    public static final int FIELD_had_borrow_hero_list =5;
    @DataBaseField(type = "blob", fieldname = "had_borrow_hero_list", comment = "已借用英雄列表")
    private byte[] had_borrow_hero_list;

    public PlayerMiddayDungeonBO() {
        id = 0;
        cid = 0L;
        round_start_time_ms = 0L;
        wave = 0;
        deducted_hp = 0L;
        had_fight_hero_list = null;
        had_borrow_hero_list = null;
    }

    public PlayerMiddayDungeonBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        round_start_time_ms = rs.getLong(3);
        wave = rs.getInt(4);
        deducted_hp = rs.getLong(5);
        had_fight_hero_list = rs.getBytes(6);
        had_borrow_hero_list = rs.getBytes(7);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMiddayDungeonBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `round_start_time_ms`, `wave`, `deducted_hp`, `had_fight_hero_list`, `had_borrow_hero_list`";
    }

    @Override
    public String getTableName() {
        return "`player_midday_dungeon`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(round_start_time_ms).append("', ");
        strBuf.append("'").append(wave).append("', ");
        strBuf.append("'").append(deducted_hp).append("', ");
        strBuf.append("?, ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(had_fight_hero_list); 
        ret.add(had_borrow_hero_list);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_had_fight_hero_list)) ret.add(had_fight_hero_list); 
        if(isFieldMarked(FIELD_had_borrow_hero_list)) ret.add(had_borrow_hero_list);         return ret;
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

    // 副本开始时间
    public long getRoundStartTimeMs() { return this.round_start_time_ms; }
    public void setRoundStartTimeMs(BM _bm, long round_start_time_ms) {
        if(round_start_time_ms==this.round_start_time_ms) 
            return;
        this.round_start_time_ms = round_start_time_ms; 
        markField(_bm, FIELD_round_start_time_ms); 
    }
    public void saveRoundStartTimeMs(BM _bm, long round_start_time_ms) {
        if(round_start_time_ms==this.round_start_time_ms) 
            return;
        this.round_start_time_ms = round_start_time_ms;
        saveField(_bm, "round_start_time_ms", round_start_time_ms);
    }

    // 波次
    public int getWave() { return this.wave; }
    public void setWave(BM _bm, int wave) {
        if(wave==this.wave) 
            return;
        this.wave = wave; 
        markField(_bm, FIELD_wave); 
    }
    public void saveWave(BM _bm, int wave) {
        if(wave==this.wave) 
            return;
        this.wave = wave;
        saveField(_bm, "wave", wave);
    }

    // 当前波次已扣除血量
    public long getDeductedHp() { return this.deducted_hp; }
    public void setDeductedHp(BM _bm, long deducted_hp) {
        if(deducted_hp==this.deducted_hp) 
            return;
        this.deducted_hp = deducted_hp; 
        markField(_bm, FIELD_deducted_hp); 
    }
    public void saveDeductedHp(BM _bm, long deducted_hp) {
        if(deducted_hp==this.deducted_hp) 
            return;
        this.deducted_hp = deducted_hp;
        saveField(_bm, "deducted_hp", deducted_hp);
    }

    // 已战斗英雄列表
    public byte[] getHadFightHeroList() { return this.had_fight_hero_list; }
    public void setHadFightHeroList(BM _bm, byte[] had_fight_hero_list) {
        if(had_fight_hero_list==this.had_fight_hero_list) 
            return;
        this.had_fight_hero_list = had_fight_hero_list; 
        markField(_bm, FIELD_had_fight_hero_list); 
    }
    public void saveHadFightHeroList(BM _bm, byte[] had_fight_hero_list) {
        if(had_fight_hero_list==this.had_fight_hero_list) 
            return;
        this.had_fight_hero_list = had_fight_hero_list;
        saveFieldBytes(_bm, "had_fight_hero_list", had_fight_hero_list);
    }

    // 已借用英雄列表
    public byte[] getHadBorrowHeroList() { return this.had_borrow_hero_list; }
    public void setHadBorrowHeroList(BM _bm, byte[] had_borrow_hero_list) {
        if(had_borrow_hero_list==this.had_borrow_hero_list) 
            return;
        this.had_borrow_hero_list = had_borrow_hero_list; 
        markField(_bm, FIELD_had_borrow_hero_list); 
    }
    public void saveHadBorrowHeroList(BM _bm, byte[] had_borrow_hero_list) {
        if(had_borrow_hero_list==this.had_borrow_hero_list) 
            return;
        this.had_borrow_hero_list = had_borrow_hero_list;
        saveFieldBytes(_bm, "had_borrow_hero_list", had_borrow_hero_list);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `round_start_time_ms` = '").append(round_start_time_ms).append("',");
        sBuilder.append(" `wave` = '").append(wave).append("',");
        sBuilder.append(" `deducted_hp` = '").append(deducted_hp).append("',");
        sBuilder.append(" `had_fight_hero_list` = ?,");
        sBuilder.append(" `had_borrow_hero_list` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_round_start_time_ms)) sBuilder.append(" `round_start_time_ms` = '").append(round_start_time_ms).append("',");
        if(isFieldMarked(FIELD_wave)) sBuilder.append(" `wave` = '").append(wave).append("',");
        if(isFieldMarked(FIELD_deducted_hp)) sBuilder.append(" `deducted_hp` = '").append(deducted_hp).append("',");
        if(isFieldMarked(FIELD_had_fight_hero_list)) sBuilder.append(" `had_fight_hero_list` = ?,");
        if(isFieldMarked(FIELD_had_borrow_hero_list)) sBuilder.append(" `had_borrow_hero_list` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_midday_dungeon` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`round_start_time_ms` bigint(20) NOT NULL DEFAULT '0' COMMENT '副本开始时间',"
                + "`wave` int(11) NOT NULL DEFAULT '0' COMMENT '波次',"
                + "`deducted_hp` bigint(20) NOT NULL DEFAULT '0' COMMENT '当前波次已扣除血量',"
                + "`had_fight_hero_list` blob NULL COMMENT '已战斗英雄列表',"
                + "`had_borrow_hero_list` blob NULL COMMENT '已借用英雄列表',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='玩家午间副本信息' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//round_start_time_ms
        _size+=4;//wave
        _size+=8;//deducted_hp
        _size+=2;_size+=had_fight_hero_list.length;//had_fight_hero_list
        _size+=2;_size+=had_borrow_hero_list.length;//had_borrow_hero_list
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(round_start_time_ms);
        buff.putInt(wave);
        buff.putLong(deducted_hp);
        buff.putShort((short)(had_fight_hero_list == null ? 0 : had_fight_hero_list.length));if(null != had_fight_hero_list){buff.put(had_fight_hero_list);}
        buff.putShort((short)(had_borrow_hero_list == null ? 0 : had_borrow_hero_list.length));if(null != had_borrow_hero_list){buff.put(had_borrow_hero_list);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        round_start_time_ms=buff.getLong();
        wave=buff.getInt();
        deducted_hp=buff.getLong();
        int had_fight_hero_list_count = buff.getShort();if(had_fight_hero_list_count>0){had_fight_hero_list = new byte[had_fight_hero_list_count];buff.get(had_fight_hero_list);}
        int had_borrow_hero_list_count = buff.getShort();if(had_borrow_hero_list_count>0){had_borrow_hero_list = new byte[had_borrow_hero_list_count];buff.get(had_borrow_hero_list);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
