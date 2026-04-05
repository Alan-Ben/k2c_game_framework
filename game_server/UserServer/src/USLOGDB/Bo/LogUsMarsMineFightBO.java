package USLOGDB.Bo;
import java.nio.ByteBuffer;
import java.sql.ResultSet;
import java.util.ArrayList;
import java.util.List;

import NPCommon.DB.BM.BM;
import NPCommon.NPLogDB.BaseLogBo;
import NPCommon.DB.Annotation.DataBaseField;
import NPCommon.Enum.NPCommonEnum.EDBTag;

public class LogUsMarsMineFightBO extends BaseLogBo {

    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_event_id =0;
    @DataBaseField(type = "int(11)", fieldname = "event_id", comment = "事件类型")
    private int event_id;

    public static final int FIELD_guid =1;
    @DataBaseField(type = "bigint(20)", fieldname = "guid", comment = "事件唯一id")
    private long guid;

    public static final int FIELD_date_time =2;
    @DataBaseField(type = "int(11)", fieldname = "date_time", comment = "日期")
    private int date_time;

    public static final int FIELD_timestamp =3;
    @DataBaseField(type = "int(11)", fieldname = "timestamp", comment = "时间戳")
    private int timestamp;

    public static final int FIELD_mineInstanceId =4;
    @DataBaseField(type = "bigint(20)", fieldname = "mineInstanceId", comment = "火星矿实例ID")
    private long mineInstanceId;

    public static final int FIELD_isWin =5;
    @DataBaseField(type = "tinyint(1)", fieldname = "isWin", comment = "true-攻击方胜利")
    private boolean isWin;

    public static final int FIELD_attackLoseValue =6;
    @DataBaseField(type = "bigint(20)", fieldname = "attackLoseValue", comment = "进攻方损失精兵")
    private long attackLoseValue;

    public static final int FIELD_attackTroopNum =7;
    @DataBaseField(type = "bigint(20)", fieldname = "attackTroopNum", comment = "防守方当前精兵")
    private long attackTroopNum;

    public static final int FIELD_attackPlayerCid =8;
    @DataBaseField(type = "bigint(20)", fieldname = "attackPlayerCid", comment = "攻击方玩家CID")
    private long attackPlayerCid;

    public static final int FIELD_attackCostValue =9;
    @DataBaseField(type = "bigint(20)", fieldname = "attackCostValue", comment = "攻击方损耗数量")
    private long attackCostValue;

    public static final int FIELD_attackTeamId =10;
    @DataBaseField(type = "bigint(20)", fieldname = "attackTeamId", comment = "攻击方队伍ID")
    private long attackTeamId;

    public static final int FIELD_attackTeamPower =11;
    @DataBaseField(type = "bigint(20)", fieldname = "attackTeamPower", comment = "攻击方队伍实力")
    private long attackTeamPower;

    public static final int FIELD_attackTeamSoldierPower =12;
    @DataBaseField(type = "bigint(20)", fieldname = "attackTeamSoldierPower", comment = "攻击方队伍单兵实力")
    private long attackTeamSoldierPower;

    public static final int FIELD_attackTeamTroopNum =13;
    @DataBaseField(type = "bigint(20)", fieldname = "attackTeamTroopNum", comment = "攻击方队伍带兵量")
    private long attackTeamTroopNum;

    public static final int FIELD_attackTeamLossValue =14;
    @DataBaseField(type = "bigint(20)", fieldname = "attackTeamLossValue", comment = "攻击方队伍耗数量")
    private long attackTeamLossValue;

    public static final int FIELD_defencePlayerCid =15;
    @DataBaseField(type = "bigint(20)", fieldname = "defencePlayerCid", comment = "防守方玩家CID")
    private long defencePlayerCid;

    public static final int FIELD_defenceCostValue =16;
    @DataBaseField(type = "bigint(20)", fieldname = "defenceCostValue", comment = "防守方损耗数量")
    private long defenceCostValue;

    public static final int FIELD_defenceTeamId =17;
    @DataBaseField(type = "bigint(20)", fieldname = "defenceTeamId", comment = "防守方队伍CID")
    private long defenceTeamId;

    public static final int FIELD_defenceTeamPower =18;
    @DataBaseField(type = "bigint(20)", fieldname = "defenceTeamPower", comment = "防守方队伍实力")
    private long defenceTeamPower;

    public static final int FIELD_defenceTeamTroopNum =19;
    @DataBaseField(type = "bigint(20)", fieldname = "defenceTeamTroopNum", comment = "防守方队伍带兵量")
    private long defenceTeamTroopNum;

    public static final int FIELD_defenceTeamLossValue =20;
    @DataBaseField(type = "bigint(20)", fieldname = "defenceTeamLossValue", comment = "防守方队伍耗数量")
    private long defenceTeamLossValue;

    public LogUsMarsMineFightBO() {
        id = 0;
        event_id = 0;
        guid = 0L;
        date_time = 0;
        timestamp = 0;
        mineInstanceId = 0L;
        isWin = false;
        attackLoseValue = 0L;
        attackTroopNum = 0L;
        attackPlayerCid = 0L;
        attackCostValue = 0L;
        attackTeamId = 0L;
        attackTeamPower = 0L;
        attackTeamSoldierPower = 0L;
        attackTeamTroopNum = 0L;
        attackTeamLossValue = 0L;
        defencePlayerCid = 0L;
        defenceCostValue = 0L;
        defenceTeamId = 0L;
        defenceTeamPower = 0L;
        defenceTeamTroopNum = 0L;
        defenceTeamLossValue = 0L;
    }

    public LogUsMarsMineFightBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        event_id = rs.getInt(2);
        guid = rs.getLong(3);
        date_time = rs.getInt(4);
        timestamp = rs.getInt(5);
        mineInstanceId = rs.getLong(6);
        isWin = rs.getBoolean(7);
        attackLoseValue = rs.getLong(8);
        attackTroopNum = rs.getLong(9);
        attackPlayerCid = rs.getLong(10);
        attackCostValue = rs.getLong(11);
        attackTeamId = rs.getLong(12);
        attackTeamPower = rs.getLong(13);
        attackTeamSoldierPower = rs.getLong(14);
        attackTeamTroopNum = rs.getLong(15);
        attackTeamLossValue = rs.getLong(16);
        defencePlayerCid = rs.getLong(17);
        defenceCostValue = rs.getLong(18);
        defenceTeamId = rs.getLong(19);
        defenceTeamPower = rs.getLong(20);
        defenceTeamTroopNum = rs.getLong(21);
        defenceTeamLossValue = rs.getLong(22);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new LogUsMarsMineFightBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `event_id`, `guid`, `date_time`, `timestamp`, `mineInstanceId`, `isWin`, `attackLoseValue`, `attackTroopNum`, `attackPlayerCid`, `attackCostValue`, `attackTeamId`, `attackTeamPower`, `attackTeamSoldierPower`, `attackTeamTroopNum`, `attackTeamLossValue`, `defencePlayerCid`, `defenceCostValue`, `defenceTeamId`, `defenceTeamPower`, `defenceTeamTroopNum`, `defenceTeamLossValue`";
    }

    @Override
    public String getTableName() {
        return "`log_us_mars_mine_fight`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(event_id).append("', ");
        strBuf.append("'").append(guid).append("', ");
        strBuf.append("'").append(date_time).append("', ");
        strBuf.append("'").append(timestamp).append("', ");
        strBuf.append("'").append(mineInstanceId).append("', ");
        strBuf.append("'").append(isWin ? 1 : 0).append("', ");
        strBuf.append("'").append(attackLoseValue).append("', ");
        strBuf.append("'").append(attackTroopNum).append("', ");
        strBuf.append("'").append(attackPlayerCid).append("', ");
        strBuf.append("'").append(attackCostValue).append("', ");
        strBuf.append("'").append(attackTeamId).append("', ");
        strBuf.append("'").append(attackTeamPower).append("', ");
        strBuf.append("'").append(attackTeamSoldierPower).append("', ");
        strBuf.append("'").append(attackTeamTroopNum).append("', ");
        strBuf.append("'").append(attackTeamLossValue).append("', ");
        strBuf.append("'").append(defencePlayerCid).append("', ");
        strBuf.append("'").append(defenceCostValue).append("', ");
        strBuf.append("'").append(defenceTeamId).append("', ");
        strBuf.append("'").append(defenceTeamPower).append("', ");
        strBuf.append("'").append(defenceTeamTroopNum).append("', ");
        strBuf.append("'").append(defenceTeamLossValue).append("', ");
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

    // 事件类型
    public int getEventId() { return this.event_id; }
    public void setEventId(BM _bm, int event_id) {
        if(event_id==this.event_id) 
            return;
        this.event_id = event_id; 
        markField(_bm, FIELD_event_id); 
    }
    public void saveEventId(BM _bm, int event_id) {
        if(event_id==this.event_id) 
            return;
        this.event_id = event_id;
        saveField(_bm, "event_id", event_id);
    }

    // 事件唯一id
    public long getGuid() { return this.guid; }
    public void setGuid(BM _bm, long guid) {
        if(guid==this.guid) 
            return;
        this.guid = guid; 
        markField(_bm, FIELD_guid); 
    }
    public void saveGuid(BM _bm, long guid) {
        if(guid==this.guid) 
            return;
        this.guid = guid;
        saveField(_bm, "guid", guid);
    }

    // 日期
    public int getDateTime() { return this.date_time; }
    public void setDateTime(BM _bm, int date_time) {
        if(date_time==this.date_time) 
            return;
        this.date_time = date_time; 
        markField(_bm, FIELD_date_time); 
    }
    public void saveDateTime(BM _bm, int date_time) {
        if(date_time==this.date_time) 
            return;
        this.date_time = date_time;
        saveField(_bm, "date_time", date_time);
    }

    // 时间戳
    public int getTimestamp() { return this.timestamp; }
    public void setTimestamp(BM _bm, int timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp; 
        markField(_bm, FIELD_timestamp); 
    }
    public void saveTimestamp(BM _bm, int timestamp) {
        if(timestamp==this.timestamp) 
            return;
        this.timestamp = timestamp;
        saveField(_bm, "timestamp", timestamp);
    }

    // 火星矿实例ID
    public long getMineInstanceId() { return this.mineInstanceId; }
    public void setMineInstanceId(BM _bm, long mineInstanceId) {
        if(mineInstanceId==this.mineInstanceId) 
            return;
        this.mineInstanceId = mineInstanceId; 
        markField(_bm, FIELD_mineInstanceId); 
    }
    public void saveMineInstanceId(BM _bm, long mineInstanceId) {
        if(mineInstanceId==this.mineInstanceId) 
            return;
        this.mineInstanceId = mineInstanceId;
        saveField(_bm, "mineInstanceId", mineInstanceId);
    }

    // true-攻击方胜利
    public boolean getIsWin() { return this.isWin; }
    public void setIsWin(BM _bm, boolean isWin) {
        if(isWin==this.isWin) 
            return;
        this.isWin = isWin; 
        markField(_bm, FIELD_isWin); 
    }
    public void saveIsWin(BM _bm, boolean isWin) {
        if(isWin==this.isWin) 
            return;
        this.isWin = isWin;
        saveField(_bm, "isWin", isWin ? 1 : 0);
    }

    // 进攻方损失精兵
    public long getAttackLoseValue() { return this.attackLoseValue; }
    public void setAttackLoseValue(BM _bm, long attackLoseValue) {
        if(attackLoseValue==this.attackLoseValue) 
            return;
        this.attackLoseValue = attackLoseValue; 
        markField(_bm, FIELD_attackLoseValue); 
    }
    public void saveAttackLoseValue(BM _bm, long attackLoseValue) {
        if(attackLoseValue==this.attackLoseValue) 
            return;
        this.attackLoseValue = attackLoseValue;
        saveField(_bm, "attackLoseValue", attackLoseValue);
    }

    // 防守方当前精兵
    public long getAttackTroopNum() { return this.attackTroopNum; }
    public void setAttackTroopNum(BM _bm, long attackTroopNum) {
        if(attackTroopNum==this.attackTroopNum) 
            return;
        this.attackTroopNum = attackTroopNum; 
        markField(_bm, FIELD_attackTroopNum); 
    }
    public void saveAttackTroopNum(BM _bm, long attackTroopNum) {
        if(attackTroopNum==this.attackTroopNum) 
            return;
        this.attackTroopNum = attackTroopNum;
        saveField(_bm, "attackTroopNum", attackTroopNum);
    }

    // 攻击方玩家CID
    public long getAttackPlayerCid() { return this.attackPlayerCid; }
    public void setAttackPlayerCid(BM _bm, long attackPlayerCid) {
        if(attackPlayerCid==this.attackPlayerCid) 
            return;
        this.attackPlayerCid = attackPlayerCid; 
        markField(_bm, FIELD_attackPlayerCid); 
    }
    public void saveAttackPlayerCid(BM _bm, long attackPlayerCid) {
        if(attackPlayerCid==this.attackPlayerCid) 
            return;
        this.attackPlayerCid = attackPlayerCid;
        saveField(_bm, "attackPlayerCid", attackPlayerCid);
    }

    // 攻击方损耗数量
    public long getAttackCostValue() { return this.attackCostValue; }
    public void setAttackCostValue(BM _bm, long attackCostValue) {
        if(attackCostValue==this.attackCostValue) 
            return;
        this.attackCostValue = attackCostValue; 
        markField(_bm, FIELD_attackCostValue); 
    }
    public void saveAttackCostValue(BM _bm, long attackCostValue) {
        if(attackCostValue==this.attackCostValue) 
            return;
        this.attackCostValue = attackCostValue;
        saveField(_bm, "attackCostValue", attackCostValue);
    }

    // 攻击方队伍ID
    public long getAttackTeamId() { return this.attackTeamId; }
    public void setAttackTeamId(BM _bm, long attackTeamId) {
        if(attackTeamId==this.attackTeamId) 
            return;
        this.attackTeamId = attackTeamId; 
        markField(_bm, FIELD_attackTeamId); 
    }
    public void saveAttackTeamId(BM _bm, long attackTeamId) {
        if(attackTeamId==this.attackTeamId) 
            return;
        this.attackTeamId = attackTeamId;
        saveField(_bm, "attackTeamId", attackTeamId);
    }

    // 攻击方队伍实力
    public long getAttackTeamPower() { return this.attackTeamPower; }
    public void setAttackTeamPower(BM _bm, long attackTeamPower) {
        if(attackTeamPower==this.attackTeamPower) 
            return;
        this.attackTeamPower = attackTeamPower; 
        markField(_bm, FIELD_attackTeamPower); 
    }
    public void saveAttackTeamPower(BM _bm, long attackTeamPower) {
        if(attackTeamPower==this.attackTeamPower) 
            return;
        this.attackTeamPower = attackTeamPower;
        saveField(_bm, "attackTeamPower", attackTeamPower);
    }

    // 攻击方队伍单兵实力
    public long getAttackTeamSoldierPower() { return this.attackTeamSoldierPower; }
    public void setAttackTeamSoldierPower(BM _bm, long attackTeamSoldierPower) {
        if(attackTeamSoldierPower==this.attackTeamSoldierPower) 
            return;
        this.attackTeamSoldierPower = attackTeamSoldierPower; 
        markField(_bm, FIELD_attackTeamSoldierPower); 
    }
    public void saveAttackTeamSoldierPower(BM _bm, long attackTeamSoldierPower) {
        if(attackTeamSoldierPower==this.attackTeamSoldierPower) 
            return;
        this.attackTeamSoldierPower = attackTeamSoldierPower;
        saveField(_bm, "attackTeamSoldierPower", attackTeamSoldierPower);
    }

    // 攻击方队伍带兵量
    public long getAttackTeamTroopNum() { return this.attackTeamTroopNum; }
    public void setAttackTeamTroopNum(BM _bm, long attackTeamTroopNum) {
        if(attackTeamTroopNum==this.attackTeamTroopNum) 
            return;
        this.attackTeamTroopNum = attackTeamTroopNum; 
        markField(_bm, FIELD_attackTeamTroopNum); 
    }
    public void saveAttackTeamTroopNum(BM _bm, long attackTeamTroopNum) {
        if(attackTeamTroopNum==this.attackTeamTroopNum) 
            return;
        this.attackTeamTroopNum = attackTeamTroopNum;
        saveField(_bm, "attackTeamTroopNum", attackTeamTroopNum);
    }

    // 攻击方队伍耗数量
    public long getAttackTeamLossValue() { return this.attackTeamLossValue; }
    public void setAttackTeamLossValue(BM _bm, long attackTeamLossValue) {
        if(attackTeamLossValue==this.attackTeamLossValue) 
            return;
        this.attackTeamLossValue = attackTeamLossValue; 
        markField(_bm, FIELD_attackTeamLossValue); 
    }
    public void saveAttackTeamLossValue(BM _bm, long attackTeamLossValue) {
        if(attackTeamLossValue==this.attackTeamLossValue) 
            return;
        this.attackTeamLossValue = attackTeamLossValue;
        saveField(_bm, "attackTeamLossValue", attackTeamLossValue);
    }

    // 防守方玩家CID
    public long getDefencePlayerCid() { return this.defencePlayerCid; }
    public void setDefencePlayerCid(BM _bm, long defencePlayerCid) {
        if(defencePlayerCid==this.defencePlayerCid) 
            return;
        this.defencePlayerCid = defencePlayerCid; 
        markField(_bm, FIELD_defencePlayerCid); 
    }
    public void saveDefencePlayerCid(BM _bm, long defencePlayerCid) {
        if(defencePlayerCid==this.defencePlayerCid) 
            return;
        this.defencePlayerCid = defencePlayerCid;
        saveField(_bm, "defencePlayerCid", defencePlayerCid);
    }

    // 防守方损耗数量
    public long getDefenceCostValue() { return this.defenceCostValue; }
    public void setDefenceCostValue(BM _bm, long defenceCostValue) {
        if(defenceCostValue==this.defenceCostValue) 
            return;
        this.defenceCostValue = defenceCostValue; 
        markField(_bm, FIELD_defenceCostValue); 
    }
    public void saveDefenceCostValue(BM _bm, long defenceCostValue) {
        if(defenceCostValue==this.defenceCostValue) 
            return;
        this.defenceCostValue = defenceCostValue;
        saveField(_bm, "defenceCostValue", defenceCostValue);
    }

    // 防守方队伍CID
    public long getDefenceTeamId() { return this.defenceTeamId; }
    public void setDefenceTeamId(BM _bm, long defenceTeamId) {
        if(defenceTeamId==this.defenceTeamId) 
            return;
        this.defenceTeamId = defenceTeamId; 
        markField(_bm, FIELD_defenceTeamId); 
    }
    public void saveDefenceTeamId(BM _bm, long defenceTeamId) {
        if(defenceTeamId==this.defenceTeamId) 
            return;
        this.defenceTeamId = defenceTeamId;
        saveField(_bm, "defenceTeamId", defenceTeamId);
    }

    // 防守方队伍实力
    public long getDefenceTeamPower() { return this.defenceTeamPower; }
    public void setDefenceTeamPower(BM _bm, long defenceTeamPower) {
        if(defenceTeamPower==this.defenceTeamPower) 
            return;
        this.defenceTeamPower = defenceTeamPower; 
        markField(_bm, FIELD_defenceTeamPower); 
    }
    public void saveDefenceTeamPower(BM _bm, long defenceTeamPower) {
        if(defenceTeamPower==this.defenceTeamPower) 
            return;
        this.defenceTeamPower = defenceTeamPower;
        saveField(_bm, "defenceTeamPower", defenceTeamPower);
    }

    // 防守方队伍带兵量
    public long getDefenceTeamTroopNum() { return this.defenceTeamTroopNum; }
    public void setDefenceTeamTroopNum(BM _bm, long defenceTeamTroopNum) {
        if(defenceTeamTroopNum==this.defenceTeamTroopNum) 
            return;
        this.defenceTeamTroopNum = defenceTeamTroopNum; 
        markField(_bm, FIELD_defenceTeamTroopNum); 
    }
    public void saveDefenceTeamTroopNum(BM _bm, long defenceTeamTroopNum) {
        if(defenceTeamTroopNum==this.defenceTeamTroopNum) 
            return;
        this.defenceTeamTroopNum = defenceTeamTroopNum;
        saveField(_bm, "defenceTeamTroopNum", defenceTeamTroopNum);
    }

    // 防守方队伍耗数量
    public long getDefenceTeamLossValue() { return this.defenceTeamLossValue; }
    public void setDefenceTeamLossValue(BM _bm, long defenceTeamLossValue) {
        if(defenceTeamLossValue==this.defenceTeamLossValue) 
            return;
        this.defenceTeamLossValue = defenceTeamLossValue; 
        markField(_bm, FIELD_defenceTeamLossValue); 
    }
    public void saveDefenceTeamLossValue(BM _bm, long defenceTeamLossValue) {
        if(defenceTeamLossValue==this.defenceTeamLossValue) 
            return;
        this.defenceTeamLossValue = defenceTeamLossValue;
        saveField(_bm, "defenceTeamLossValue", defenceTeamLossValue);
    }



    @Override
    protected String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `event_id` = '").append(event_id).append("',");
        sBuilder.append(" `guid` = '").append(guid).append("',");
        sBuilder.append(" `date_time` = '").append(date_time).append("',");
        sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        sBuilder.append(" `mineInstanceId` = '").append(mineInstanceId).append("',");
        sBuilder.append(" `isWin` = '").append(isWin ? 1 : 0).append("',");
        sBuilder.append(" `attackLoseValue` = '").append(attackLoseValue).append("',");
        sBuilder.append(" `attackTroopNum` = '").append(attackTroopNum).append("',");
        sBuilder.append(" `attackPlayerCid` = '").append(attackPlayerCid).append("',");
        sBuilder.append(" `attackCostValue` = '").append(attackCostValue).append("',");
        sBuilder.append(" `attackTeamId` = '").append(attackTeamId).append("',");
        sBuilder.append(" `attackTeamPower` = '").append(attackTeamPower).append("',");
        sBuilder.append(" `attackTeamSoldierPower` = '").append(attackTeamSoldierPower).append("',");
        sBuilder.append(" `attackTeamTroopNum` = '").append(attackTeamTroopNum).append("',");
        sBuilder.append(" `attackTeamLossValue` = '").append(attackTeamLossValue).append("',");
        sBuilder.append(" `defencePlayerCid` = '").append(defencePlayerCid).append("',");
        sBuilder.append(" `defenceCostValue` = '").append(defenceCostValue).append("',");
        sBuilder.append(" `defenceTeamId` = '").append(defenceTeamId).append("',");
        sBuilder.append(" `defenceTeamPower` = '").append(defenceTeamPower).append("',");
        sBuilder.append(" `defenceTeamTroopNum` = '").append(defenceTeamTroopNum).append("',");
        sBuilder.append(" `defenceTeamLossValue` = '").append(defenceTeamLossValue).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }

    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_event_id)) sBuilder.append(" `event_id` = '").append(event_id).append("',");
        if(isFieldMarked(FIELD_guid)) sBuilder.append(" `guid` = '").append(guid).append("',");
        if(isFieldMarked(FIELD_date_time)) sBuilder.append(" `date_time` = '").append(date_time).append("',");
        if(isFieldMarked(FIELD_timestamp)) sBuilder.append(" `timestamp` = '").append(timestamp).append("',");
        if(isFieldMarked(FIELD_mineInstanceId)) sBuilder.append(" `mineInstanceId` = '").append(mineInstanceId).append("',");
        if(isFieldMarked(FIELD_isWin)) sBuilder.append(" `isWin` = '").append(isWin ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_attackLoseValue)) sBuilder.append(" `attackLoseValue` = '").append(attackLoseValue).append("',");
        if(isFieldMarked(FIELD_attackTroopNum)) sBuilder.append(" `attackTroopNum` = '").append(attackTroopNum).append("',");
        if(isFieldMarked(FIELD_attackPlayerCid)) sBuilder.append(" `attackPlayerCid` = '").append(attackPlayerCid).append("',");
        if(isFieldMarked(FIELD_attackCostValue)) sBuilder.append(" `attackCostValue` = '").append(attackCostValue).append("',");
        if(isFieldMarked(FIELD_attackTeamId)) sBuilder.append(" `attackTeamId` = '").append(attackTeamId).append("',");
        if(isFieldMarked(FIELD_attackTeamPower)) sBuilder.append(" `attackTeamPower` = '").append(attackTeamPower).append("',");
        if(isFieldMarked(FIELD_attackTeamSoldierPower)) sBuilder.append(" `attackTeamSoldierPower` = '").append(attackTeamSoldierPower).append("',");
        if(isFieldMarked(FIELD_attackTeamTroopNum)) sBuilder.append(" `attackTeamTroopNum` = '").append(attackTeamTroopNum).append("',");
        if(isFieldMarked(FIELD_attackTeamLossValue)) sBuilder.append(" `attackTeamLossValue` = '").append(attackTeamLossValue).append("',");
        if(isFieldMarked(FIELD_defencePlayerCid)) sBuilder.append(" `defencePlayerCid` = '").append(defencePlayerCid).append("',");
        if(isFieldMarked(FIELD_defenceCostValue)) sBuilder.append(" `defenceCostValue` = '").append(defenceCostValue).append("',");
        if(isFieldMarked(FIELD_defenceTeamId)) sBuilder.append(" `defenceTeamId` = '").append(defenceTeamId).append("',");
        if(isFieldMarked(FIELD_defenceTeamPower)) sBuilder.append(" `defenceTeamPower` = '").append(defenceTeamPower).append("',");
        if(isFieldMarked(FIELD_defenceTeamTroopNum)) sBuilder.append(" `defenceTeamTroopNum` = '").append(defenceTeamTroopNum).append("',");
        if(isFieldMarked(FIELD_defenceTeamLossValue)) sBuilder.append(" `defenceTeamLossValue` = '").append(defenceTeamLossValue).append("',");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `log_us_mars_mine_fight` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`event_id` int(11) NOT NULL DEFAULT '0' COMMENT '事件类型',"
                + "`guid` bigint(20) NOT NULL DEFAULT '0' COMMENT '事件唯一id',"
                + "`date_time` int(11) NOT NULL DEFAULT '0' COMMENT '日期',"
                + "`timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '时间戳',"
                + "`mineInstanceId` bigint(20) NOT NULL DEFAULT '0' COMMENT '火星矿实例ID',"
                + "`isWin` tinyint(1) NOT NULL DEFAULT '0' COMMENT 'true-攻击方胜利',"
                + "`attackLoseValue` bigint(20) NOT NULL DEFAULT '0' COMMENT '进攻方损失精兵',"
                + "`attackTroopNum` bigint(20) NOT NULL DEFAULT '0' COMMENT '防守方当前精兵',"
                + "`attackPlayerCid` bigint(20) NOT NULL DEFAULT '0' COMMENT '攻击方玩家CID',"
                + "`attackCostValue` bigint(20) NOT NULL DEFAULT '0' COMMENT '攻击方损耗数量',"
                + "`attackTeamId` bigint(20) NOT NULL DEFAULT '0' COMMENT '攻击方队伍ID',"
                + "`attackTeamPower` bigint(20) NOT NULL DEFAULT '0' COMMENT '攻击方队伍实力',"
                + "`attackTeamSoldierPower` bigint(20) NOT NULL DEFAULT '0' COMMENT '攻击方队伍单兵实力',"
                + "`attackTeamTroopNum` bigint(20) NOT NULL DEFAULT '0' COMMENT '攻击方队伍带兵量',"
                + "`attackTeamLossValue` bigint(20) NOT NULL DEFAULT '0' COMMENT '攻击方队伍耗数量',"
                + "`defencePlayerCid` bigint(20) NOT NULL DEFAULT '0' COMMENT '防守方玩家CID',"
                + "`defenceCostValue` bigint(20) NOT NULL DEFAULT '0' COMMENT '防守方损耗数量',"
                + "`defenceTeamId` bigint(20) NOT NULL DEFAULT '0' COMMENT '防守方队伍CID',"
                + "`defenceTeamPower` bigint(20) NOT NULL DEFAULT '0' COMMENT '防守方队伍实力',"
                + "`defenceTeamTroopNum` bigint(20) NOT NULL DEFAULT '0' COMMENT '防守方队伍带兵量',"
                + "`defenceTeamLossValue` bigint(20) NOT NULL DEFAULT '0' COMMENT '防守方队伍耗数量',"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='US火星矿战斗日志表' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
        return sql;
    }
    
    @Override
    public EDBTag getDBTag() {
        return EDBTag.us_log;
    }
    private int getBufferSize()
    {
        int _size=ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize( this.getClass().getName());
        _size+=8;//id
        _size+=4;//event_id
        _size+=8;//guid
        _size+=4;//date_time
        _size+=4;//timestamp
        _size+=8;//mineInstanceId
        _size+=1;//isWin
        _size+=8;//attackLoseValue
        _size+=8;//attackTroopNum
        _size+=8;//attackPlayerCid
        _size+=8;//attackCostValue
        _size+=8;//attackTeamId
        _size+=8;//attackTeamPower
        _size+=8;//attackTeamSoldierPower
        _size+=8;//attackTeamTroopNum
        _size+=8;//attackTeamLossValue
        _size+=8;//defencePlayerCid
        _size+=8;//defenceCostValue
        _size+=8;//defenceTeamId
        _size+=8;//defenceTeamPower
        _size+=8;//defenceTeamTroopNum
        _size+=8;//defenceTeamLossValue
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putInt(event_id);
        buff.putLong(guid);
        buff.putInt(date_time);
        buff.putInt(timestamp);
        buff.putLong(mineInstanceId);
        buff.put((byte)(isWin?1:0));
        buff.putLong(attackLoseValue);
        buff.putLong(attackTroopNum);
        buff.putLong(attackPlayerCid);
        buff.putLong(attackCostValue);
        buff.putLong(attackTeamId);
        buff.putLong(attackTeamPower);
        buff.putLong(attackTeamSoldierPower);
        buff.putLong(attackTeamTroopNum);
        buff.putLong(attackTeamLossValue);
        buff.putLong(defencePlayerCid);
        buff.putLong(defenceCostValue);
        buff.putLong(defenceTeamId);
        buff.putLong(defenceTeamPower);
        buff.putLong(defenceTeamTroopNum);
        buff.putLong(defenceTeamLossValue);        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        event_id=buff.getInt();
        guid=buff.getLong();
        date_time=buff.getInt();
        timestamp=buff.getInt();
        mineInstanceId=buff.getLong();
        isWin=(buff.get()==1);
        attackLoseValue=buff.getLong();
        attackTroopNum=buff.getLong();
        attackPlayerCid=buff.getLong();
        attackCostValue=buff.getLong();
        attackTeamId=buff.getLong();
        attackTeamPower=buff.getLong();
        attackTeamSoldierPower=buff.getLong();
        attackTeamTroopNum=buff.getLong();
        attackTeamLossValue=buff.getLong();
        defencePlayerCid=buff.getLong();
        defenceCostValue=buff.getLong();
        defenceTeamId=buff.getLong();
        defenceTeamPower=buff.getLong();
        defenceTeamTroopNum=buff.getLong();
        defenceTeamLossValue=buff.getLong(); 
    }
	
	@Override
	public  int getRecordExpiredTimeSec()
    {
    	return 7776000 ;
    }
}
