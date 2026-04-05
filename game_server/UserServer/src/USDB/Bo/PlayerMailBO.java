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
public class PlayerMailBO extends BaseBO {
    
    @DataBaseField(type = "bigint(20)", fieldname = "id", comment = "数据库表唯一键值")
    private long id;

    public static final int FIELD_cid =0;
    @DataBaseField(type = "bigint(20)", fieldname = "cid", comment = "玩家CID")
    private long cid;

    public static final int FIELD_mail_ref_id =1;
    @DataBaseField(type = "bigint(20)", fieldname = "mail_ref_id", comment = "邮件配表ID")
    private long mail_ref_id;

    public static final int FIELD_php_mail_id =2;
    @DataBaseField(type = "bigint(20)", fieldname = "php_mail_id", comment = "后台邮件ID")
    private long php_mail_id;

    public static final int FIELD_sender_id =3;
    @DataBaseField(type = "int(11)", fieldname = "sender_id", comment = "发送者id，0-客户端读取系统配置")
    private int sender_id;

    public static final int FIELD_is_locked =4;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_locked", comment = "是否已锁定")
    private boolean is_locked;

    public static final int FIELD_title =5;
    @DataBaseField(type = "varchar(100)", fieldname = "title", comment = "邮件标题（非配置邮件）")
    private String title;

    public static final int FIELD_content =6;
    @DataBaseField(type = "text", fieldname = "content", comment = "邮件内容（非配置邮件）")
    private String content;

    public static final int FIELD_contentReplace =7;
    @DataBaseField(type = "text", fieldname = "contentReplace", comment = "邮件内容（替换部分）")
    private String contentReplace;

    public static final int FIELD_attachList =8;
    @DataBaseField(type = "varbinary(10240)", fieldname = "attachList", comment = "邮件附件")
    private byte[] attachList;

    public static final int FIELD_createdTs =9;
    @DataBaseField(type = "int(11)", fieldname = "createdTs", comment = "创建时间（秒）")
    private int createdTs;

    public static final int FIELD_readedTs =10;
    @DataBaseField(type = "int(11)", fieldname = "readedTs", comment = "读取时间（秒）")
    private int readedTs;

    public static final int FIELD_takedTs =11;
    @DataBaseField(type = "int(11)", fieldname = "takedTs", comment = "领取礼包（秒）")
    private int takedTs;

    public static final int FIELD_expiredTs =12;
    @DataBaseField(type = "int(11)", fieldname = "expiredTs", comment = "过期时间（秒）")
    private int expiredTs;

    public static final int FIELD_effectSecs =13;
    @DataBaseField(type = "int(11)", fieldname = "effectSecs", comment = "有效时长（秒）")
    private int effectSecs;

    public static final int FIELD_is_must_read =14;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_must_read", comment = "是否必读")
    private boolean is_must_read;

    public static final int FIELD_language =15;
    @DataBaseField(type = "int(11)", fieldname = "language", comment = "当前邮件语言")
    private int language;

    public static final int FIELD_exDataType =16;
    @DataBaseField(type = "int(11)", fieldname = "exDataType", comment = "额外数据类型")
    private int exDataType;

    public static final int FIELD_exData =17;
    @DataBaseField(type = "blob", fieldname = "exData", comment = "额外数据")
    private byte[] exData;

    public static final int FIELD_is_read_over =18;
    @DataBaseField(type = "tinyint(1)", fieldname = "is_read_over", comment = "是否已读完")
    private boolean is_read_over;

    public static final int FIELD_exTitleData =19;
    @DataBaseField(type = "blob", fieldname = "exTitleData", comment = "额外标题数据")
    private byte[] exTitleData;

    public PlayerMailBO() {
        id = 0;
        cid = 0L;
        mail_ref_id = 0L;
        php_mail_id = 0L;
        sender_id = 0;
        is_locked = false;
        title = "";
        content = "";
        contentReplace = "";
        attachList = null;
        createdTs = 0;
        readedTs = 0;
        takedTs = 0;
        expiredTs = 0;
        effectSecs = 0;
        is_must_read = false;
        language = 0;
        exDataType = 0;
        exData = null;
        is_read_over = false;
        exTitleData = null;
    }

    public PlayerMailBO(ResultSet rs) throws Exception {
        id = rs.getLong(1);
        cid = rs.getLong(2);
        mail_ref_id = rs.getLong(3);
        php_mail_id = rs.getLong(4);
        sender_id = rs.getInt(5);
        is_locked = rs.getBoolean(6);
        title = rs.getString(7);
        content = rs.getString(8);
        contentReplace = rs.getString(9);
        attachList = rs.getBytes(10);
        createdTs = rs.getInt(11);
        readedTs = rs.getInt(12);
        takedTs = rs.getInt(13);
        expiredTs = rs.getInt(14);
        effectSecs = rs.getInt(15);
        is_must_read = rs.getBoolean(16);
        language = rs.getInt(17);
        exDataType = rs.getInt(18);
        exData = rs.getBytes(19);
        is_read_over = rs.getBoolean(20);
        exTitleData = rs.getBytes(21);
    }

    @Override
    @SuppressWarnings({ "unchecked", "rawtypes" })
    public void getFromResultSet(ResultSet rs, List list) throws Exception {
        list.add(new PlayerMailBO(rs));
    }

    @Override
    public String getItemsName() {
        return "`id`, `cid`, `mail_ref_id`, `php_mail_id`, `sender_id`, `is_locked`, `title`, `content`, `contentReplace`, `attachList`, `createdTs`, `readedTs`, `takedTs`, `expiredTs`, `effectSecs`, `is_must_read`, `language`, `exDataType`, `exData`, `is_read_over`, `exTitleData`";
    }

    @Override
    public String getTableName() {
        return "`player_mail`";
    }

    @Override
    public String getItemsValue() {
        StringBuilder strBuf = new StringBuilder();
        strBuf.append("'").append(id).append("', ");
        strBuf.append("'").append(cid).append("', ");
        strBuf.append("'").append(mail_ref_id).append("', ");
        strBuf.append("'").append(php_mail_id).append("', ");
        strBuf.append("'").append(sender_id).append("', ");
        strBuf.append("'").append(is_locked ? 1 : 0).append("', ");
        strBuf.append("'").append(title == null ? null : title.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("'").append(contentReplace == null ? null : contentReplace.replace("'","''").replace("\\","\\\\")).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(createdTs).append("', ");
        strBuf.append("'").append(readedTs).append("', ");
        strBuf.append("'").append(takedTs).append("', ");
        strBuf.append("'").append(expiredTs).append("', ");
        strBuf.append("'").append(effectSecs).append("', ");
        strBuf.append("'").append(is_must_read ? 1 : 0).append("', ");
        strBuf.append("'").append(language).append("', ");
        strBuf.append("'").append(exDataType).append("', ");
        strBuf.append("?, ");
        strBuf.append("'").append(is_read_over ? 1 : 0).append("', ");
        strBuf.append("?, ");
        strBuf.deleteCharAt(strBuf.length() - 2);
        return strBuf.toString();
    }

    @Override
    public ArrayList<byte[]> getInsertValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        ret.add(attachList); 
        ret.add(exData); 
        ret.add(exTitleData);         return ret;
    }

    @Override
    public ArrayList<byte[]> getMarkedValueBytes() {
        ArrayList<byte[]> ret = new ArrayList<>();
        if(isFieldMarked(FIELD_attachList)) ret.add(attachList); 
        if(isFieldMarked(FIELD_exData)) ret.add(exData); 
        if(isFieldMarked(FIELD_exTitleData)) ret.add(exTitleData);         return ret;
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

    // 邮件配表ID
    public long getMailRefId() { return this.mail_ref_id; }
    public void setMailRefId(BM _bm, long mail_ref_id) {
        if(mail_ref_id==this.mail_ref_id) 
            return;
        this.mail_ref_id = mail_ref_id; 
        markField(_bm, FIELD_mail_ref_id); 
    }
    public void saveMailRefId(BM _bm, long mail_ref_id) {
        if(mail_ref_id==this.mail_ref_id) 
            return;
        this.mail_ref_id = mail_ref_id;
        saveField(_bm, "mail_ref_id", mail_ref_id);
    }

    // 后台邮件ID
    public long getPhpMailId() { return this.php_mail_id; }
    public void setPhpMailId(BM _bm, long php_mail_id) {
        if(php_mail_id==this.php_mail_id) 
            return;
        this.php_mail_id = php_mail_id; 
        markField(_bm, FIELD_php_mail_id); 
    }
    public void savePhpMailId(BM _bm, long php_mail_id) {
        if(php_mail_id==this.php_mail_id) 
            return;
        this.php_mail_id = php_mail_id;
        saveField(_bm, "php_mail_id", php_mail_id);
    }

    // 发送者id，0-客户端读取系统配置
    public int getSenderId() { return this.sender_id; }
    public void setSenderId(BM _bm, int sender_id) {
        if(sender_id==this.sender_id) 
            return;
        this.sender_id = sender_id; 
        markField(_bm, FIELD_sender_id); 
    }
    public void saveSenderId(BM _bm, int sender_id) {
        if(sender_id==this.sender_id) 
            return;
        this.sender_id = sender_id;
        saveField(_bm, "sender_id", sender_id);
    }

    // 是否已锁定
    public boolean getIsLocked() { return this.is_locked; }
    public void setIsLocked(BM _bm, boolean is_locked) {
        if(is_locked==this.is_locked) 
            return;
        this.is_locked = is_locked; 
        markField(_bm, FIELD_is_locked); 
    }
    public void saveIsLocked(BM _bm, boolean is_locked) {
        if(is_locked==this.is_locked) 
            return;
        this.is_locked = is_locked;
        saveField(_bm, "is_locked", is_locked ? 1 : 0);
    }

    // 邮件标题（非配置邮件）
    public String getTitle() { return this.title; }
    public void setTitle(BM _bm, String title) {
        if(title.equals(this.title)) 
            return;
        this.title = title; 
        markField(_bm, FIELD_title); 
    }
    public void saveTitle(BM _bm, String title) {
        if(title.equals(this.title)) 
            return;
        this.title = title;
        saveField(_bm, "title", title);
    }

    // 邮件内容（非配置邮件）
    public String getContent() { return this.content; }
    public void setContent(BM _bm, String content) {
        if(content.equals(this.content)) 
            return;
        this.content = content; 
        markField(_bm, FIELD_content); 
    }
    public void saveContent(BM _bm, String content) {
        if(content.equals(this.content)) 
            return;
        this.content = content;
        saveField(_bm, "content", content);
    }

    // 邮件内容（替换部分）
    public String getContentReplace() { return this.contentReplace; }
    public void setContentReplace(BM _bm, String contentReplace) {
        if(contentReplace.equals(this.contentReplace)) 
            return;
        this.contentReplace = contentReplace; 
        markField(_bm, FIELD_contentReplace); 
    }
    public void saveContentReplace(BM _bm, String contentReplace) {
        if(contentReplace.equals(this.contentReplace)) 
            return;
        this.contentReplace = contentReplace;
        saveField(_bm, "contentReplace", contentReplace);
    }

    // 邮件附件
    public byte[] getAttachList() { return this.attachList; }
    public void setAttachList(BM _bm, byte[] attachList) {
        if(attachList==this.attachList) 
            return;
        this.attachList = attachList; 
        markField(_bm, FIELD_attachList); 
    }
    public void saveAttachList(BM _bm, byte[] attachList) {
        if(attachList==this.attachList) 
            return;
        this.attachList = attachList;
        saveFieldBytes(_bm, "attachList", attachList);
    }

    // 创建时间（秒）
    public int getCreatedTs() { return this.createdTs; }
    public void setCreatedTs(BM _bm, int createdTs) {
        if(createdTs==this.createdTs) 
            return;
        this.createdTs = createdTs; 
        markField(_bm, FIELD_createdTs); 
    }
    public void saveCreatedTs(BM _bm, int createdTs) {
        if(createdTs==this.createdTs) 
            return;
        this.createdTs = createdTs;
        saveField(_bm, "createdTs", createdTs);
    }

    // 读取时间（秒）
    public int getReadedTs() { return this.readedTs; }
    public void setReadedTs(BM _bm, int readedTs) {
        if(readedTs==this.readedTs) 
            return;
        this.readedTs = readedTs; 
        markField(_bm, FIELD_readedTs); 
    }
    public void saveReadedTs(BM _bm, int readedTs) {
        if(readedTs==this.readedTs) 
            return;
        this.readedTs = readedTs;
        saveField(_bm, "readedTs", readedTs);
    }

    // 领取礼包（秒）
    public int getTakedTs() { return this.takedTs; }
    public void setTakedTs(BM _bm, int takedTs) {
        if(takedTs==this.takedTs) 
            return;
        this.takedTs = takedTs; 
        markField(_bm, FIELD_takedTs); 
    }
    public void saveTakedTs(BM _bm, int takedTs) {
        if(takedTs==this.takedTs) 
            return;
        this.takedTs = takedTs;
        saveField(_bm, "takedTs", takedTs);
    }

    // 过期时间（秒）
    public int getExpiredTs() { return this.expiredTs; }
    public void setExpiredTs(BM _bm, int expiredTs) {
        if(expiredTs==this.expiredTs) 
            return;
        this.expiredTs = expiredTs; 
        markField(_bm, FIELD_expiredTs); 
    }
    public void saveExpiredTs(BM _bm, int expiredTs) {
        if(expiredTs==this.expiredTs) 
            return;
        this.expiredTs = expiredTs;
        saveField(_bm, "expiredTs", expiredTs);
    }

    // 有效时长（秒）
    public int getEffectSecs() { return this.effectSecs; }
    public void setEffectSecs(BM _bm, int effectSecs) {
        if(effectSecs==this.effectSecs) 
            return;
        this.effectSecs = effectSecs; 
        markField(_bm, FIELD_effectSecs); 
    }
    public void saveEffectSecs(BM _bm, int effectSecs) {
        if(effectSecs==this.effectSecs) 
            return;
        this.effectSecs = effectSecs;
        saveField(_bm, "effectSecs", effectSecs);
    }

    // 是否必读
    public boolean getIsMustRead() { return this.is_must_read; }
    public void setIsMustRead(BM _bm, boolean is_must_read) {
        if(is_must_read==this.is_must_read) 
            return;
        this.is_must_read = is_must_read; 
        markField(_bm, FIELD_is_must_read); 
    }
    public void saveIsMustRead(BM _bm, boolean is_must_read) {
        if(is_must_read==this.is_must_read) 
            return;
        this.is_must_read = is_must_read;
        saveField(_bm, "is_must_read", is_must_read ? 1 : 0);
    }

    // 当前邮件语言
    public int getLanguage() { return this.language; }
    public void setLanguage(BM _bm, int language) {
        if(language==this.language) 
            return;
        this.language = language; 
        markField(_bm, FIELD_language); 
    }
    public void saveLanguage(BM _bm, int language) {
        if(language==this.language) 
            return;
        this.language = language;
        saveField(_bm, "language", language);
    }

    // 额外数据类型
    public int getExDataType() { return this.exDataType; }
    public void setExDataType(BM _bm, int exDataType) {
        if(exDataType==this.exDataType) 
            return;
        this.exDataType = exDataType; 
        markField(_bm, FIELD_exDataType); 
    }
    public void saveExDataType(BM _bm, int exDataType) {
        if(exDataType==this.exDataType) 
            return;
        this.exDataType = exDataType;
        saveField(_bm, "exDataType", exDataType);
    }

    // 额外数据
    public byte[] getExData() { return this.exData; }
    public void setExData(BM _bm, byte[] exData) {
        if(exData==this.exData) 
            return;
        this.exData = exData; 
        markField(_bm, FIELD_exData); 
    }
    public void saveExData(BM _bm, byte[] exData) {
        if(exData==this.exData) 
            return;
        this.exData = exData;
        saveFieldBytes(_bm, "exData", exData);
    }

    // 是否已读完
    public boolean getIsReadOver() { return this.is_read_over; }
    public void setIsReadOver(BM _bm, boolean is_read_over) {
        if(is_read_over==this.is_read_over) 
            return;
        this.is_read_over = is_read_over; 
        markField(_bm, FIELD_is_read_over); 
    }
    public void saveIsReadOver(BM _bm, boolean is_read_over) {
        if(is_read_over==this.is_read_over) 
            return;
        this.is_read_over = is_read_over;
        saveField(_bm, "is_read_over", is_read_over ? 1 : 0);
    }

    // 额外标题数据
    public byte[] getExTitleData() { return this.exTitleData; }
    public void setExTitleData(BM _bm, byte[] exTitleData) {
        if(exTitleData==this.exTitleData) 
            return;
        this.exTitleData = exTitleData; 
        markField(_bm, FIELD_exTitleData); 
    }
    public void saveExTitleData(BM _bm, byte[] exTitleData) {
        if(exTitleData==this.exTitleData) 
            return;
        this.exTitleData = exTitleData;
        saveFieldBytes(_bm, "exTitleData", exTitleData);
    }



    @Override
    public String getUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        sBuilder.append(" `cid` = '").append(cid).append("',");
        sBuilder.append(" `mail_ref_id` = '").append(mail_ref_id).append("',");
        sBuilder.append(" `php_mail_id` = '").append(php_mail_id).append("',");
        sBuilder.append(" `sender_id` = '").append(sender_id).append("',");
        sBuilder.append(" `is_locked` = '").append(is_locked ? 1 : 0).append("',");
        sBuilder.append(" `title` = '").append(title == null ? null : title.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `content` = '").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `contentReplace` = '").append(contentReplace == null ? null : contentReplace.replace("'","''").replace("\\","\\\\")).append("',");
        sBuilder.append(" `attachList` = ?,");
        sBuilder.append(" `createdTs` = '").append(createdTs).append("',");
        sBuilder.append(" `readedTs` = '").append(readedTs).append("',");
        sBuilder.append(" `takedTs` = '").append(takedTs).append("',");
        sBuilder.append(" `expiredTs` = '").append(expiredTs).append("',");
        sBuilder.append(" `effectSecs` = '").append(effectSecs).append("',");
        sBuilder.append(" `is_must_read` = '").append(is_must_read ? 1 : 0).append("',");
        sBuilder.append(" `language` = '").append(language).append("',");
        sBuilder.append(" `exDataType` = '").append(exDataType).append("',");
        sBuilder.append(" `exData` = ?,");
        sBuilder.append(" `is_read_over` = '").append(is_read_over ? 1 : 0).append("',");
        sBuilder.append(" `exTitleData` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
    @Override
    public String getMarkedUpdateKeyValue() {
        StringBuilder sBuilder = new StringBuilder();
        if(isFieldMarked(FIELD_cid)) sBuilder.append(" `cid` = '").append(cid).append("',");
        if(isFieldMarked(FIELD_mail_ref_id)) sBuilder.append(" `mail_ref_id` = '").append(mail_ref_id).append("',");
        if(isFieldMarked(FIELD_php_mail_id)) sBuilder.append(" `php_mail_id` = '").append(php_mail_id).append("',");
        if(isFieldMarked(FIELD_sender_id)) sBuilder.append(" `sender_id` = '").append(sender_id).append("',");
        if(isFieldMarked(FIELD_is_locked)) sBuilder.append(" `is_locked` = '").append(is_locked ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_title)) sBuilder.append(" `title` = '").append(title == null ? null : title.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_content)) sBuilder.append(" `content` = '").append(content == null ? null : content.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_contentReplace)) sBuilder.append(" `contentReplace` = '").append(contentReplace == null ? null : contentReplace.replace("'","''").replace("\\","\\\\")).append("',");
        if(isFieldMarked(FIELD_attachList)) sBuilder.append(" `attachList` = ?,");
        if(isFieldMarked(FIELD_createdTs)) sBuilder.append(" `createdTs` = '").append(createdTs).append("',");
        if(isFieldMarked(FIELD_readedTs)) sBuilder.append(" `readedTs` = '").append(readedTs).append("',");
        if(isFieldMarked(FIELD_takedTs)) sBuilder.append(" `takedTs` = '").append(takedTs).append("',");
        if(isFieldMarked(FIELD_expiredTs)) sBuilder.append(" `expiredTs` = '").append(expiredTs).append("',");
        if(isFieldMarked(FIELD_effectSecs)) sBuilder.append(" `effectSecs` = '").append(effectSecs).append("',");
        if(isFieldMarked(FIELD_is_must_read)) sBuilder.append(" `is_must_read` = '").append(is_must_read ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_language)) sBuilder.append(" `language` = '").append(language).append("',");
        if(isFieldMarked(FIELD_exDataType)) sBuilder.append(" `exDataType` = '").append(exDataType).append("',");
        if(isFieldMarked(FIELD_exData)) sBuilder.append(" `exData` = ?,");
        if(isFieldMarked(FIELD_is_read_over)) sBuilder.append(" `is_read_over` = '").append(is_read_over ? 1 : 0).append("',");
        if(isFieldMarked(FIELD_exTitleData)) sBuilder.append(" `exTitleData` = ?,");
        sBuilder.deleteCharAt(sBuilder.length() - 1);
        return sBuilder.toString();
    }
	
    public static String getSql_TableCreate() {
        String sql = "CREATE TABLE IF NOT EXISTS `player_mail` ("
                + "`id` bigint(20) NOT NULL AUTO_INCREMENT COMMENT '自增主键',"
                + "`cid` bigint(20) NOT NULL DEFAULT '0' COMMENT '玩家CID',"
                + "`mail_ref_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '邮件配表ID',"
                + "`php_mail_id` bigint(20) NOT NULL DEFAULT '0' COMMENT '后台邮件ID',"
                + "`sender_id` int(11) NOT NULL DEFAULT '0' COMMENT '发送者id，0-客户端读取系统配置',"
                + "`is_locked` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已锁定',"
                + "`title` varchar(100) NOT NULL DEFAULT '' COMMENT '邮件标题（非配置邮件）',"
                + "`content` text NULL COMMENT '邮件内容（非配置邮件）',"
                + "`contentReplace` text NULL COMMENT '邮件内容（替换部分）',"
                + "`attachList` varbinary(10240) NOT NULL DEFAULT '' COMMENT '邮件附件',"
                + "`createdTs` int(11) NOT NULL DEFAULT '0' COMMENT '创建时间（秒）',"
                + "`readedTs` int(11) NOT NULL DEFAULT '0' COMMENT '读取时间（秒）',"
                + "`takedTs` int(11) NOT NULL DEFAULT '0' COMMENT '领取礼包（秒）',"
                + "`expiredTs` int(11) NOT NULL DEFAULT '0' COMMENT '过期时间（秒）',"
                + "`effectSecs` int(11) NOT NULL DEFAULT '0' COMMENT '有效时长（秒）',"
                + "`is_must_read` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否必读',"
                + "`language` int(11) NOT NULL DEFAULT '0' COMMENT '当前邮件语言',"
                + "`exDataType` int(11) NOT NULL DEFAULT '0' COMMENT '额外数据类型',"
                + "`exData` blob NULL COMMENT '额外数据',"
                + "`is_read_over` tinyint(1) NOT NULL DEFAULT '0' COMMENT '是否已读完',"
                + "`exTitleData` blob NULL COMMENT '额外标题数据',"
                + "KEY `cid` (`cid`),"
                + "PRIMARY KEY (`id`)"
                + ") COMMENT='Mail 玩家Mail数据' engine=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE utf8mb4_general_ci AUTO_INCREMENT=1";
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
        _size+=8;//mail_ref_id
        _size+=8;//php_mail_id
        _size+=4;//sender_id
        _size+=1;//is_locked
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);//title
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);//content
        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(contentReplace);//contentReplace
        _size+=2;_size+=attachList.length;//attachList
        _size+=4;//createdTs
        _size+=4;//readedTs
        _size+=4;//takedTs
        _size+=4;//expiredTs
        _size+=4;//effectSecs
        _size+=1;//is_must_read
        _size+=4;//language
        _size+=4;//exDataType
        _size+=2;_size+=exData.length;//exData
        _size+=1;//is_read_over
        _size+=2;_size+=exTitleData.length;//exTitleData
         return _size;
    }
   

    @Override
    public ByteBuffer toByteBuffer()
    {
        ByteBuffer buff= ByteBuffer.allocate(getBufferSize());
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, this.getClass().getName());
        buff.putLong(id);
        buff.putLong(cid);
        buff.putLong(mail_ref_id);
        buff.putLong(php_mail_id);
        buff.putInt(sender_id);
        buff.put((byte)(is_locked?1:0));
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, title);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, content);
        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, contentReplace);
        buff.putShort((short)(attachList == null ? 0 : attachList.length));if(null != attachList){buff.put(attachList);}
        buff.putInt(createdTs);
        buff.putInt(readedTs);
        buff.putInt(takedTs);
        buff.putInt(expiredTs);
        buff.putInt(effectSecs);
        buff.put((byte)(is_must_read?1:0));
        buff.putInt(language);
        buff.putInt(exDataType);
        buff.putShort((short)(exData == null ? 0 : exData.length));if(null != exData){buff.put(exData);}
        buff.put((byte)(is_read_over?1:0));
        buff.putShort((short)(exTitleData == null ? 0 : exTitleData.length));if(null != exTitleData){buff.put(exTitleData);}        
        return buff;
    }

    @Override
    protected void readFromByteBuffer(ByteBuffer buff)
    {
        id=buff.getLong();
        cid=buff.getLong();
        mail_ref_id=buff.getLong();
        php_mail_id=buff.getLong();
        sender_id=buff.getInt();
        is_locked=(buff.get()==1);
        title=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        content=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        contentReplace=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);
        int attachList_count = buff.getShort();if(attachList_count>0){attachList = new byte[attachList_count];buff.get(attachList);}
        createdTs=buff.getInt();
        readedTs=buff.getInt();
        takedTs=buff.getInt();
        expiredTs=buff.getInt();
        effectSecs=buff.getInt();
        is_must_read=(buff.get()==1);
        language=buff.getInt();
        exDataType=buff.getInt();
        int exData_count = buff.getShort();if(exData_count>0){exData = new byte[exData_count];buff.get(exData);}
        is_read_over=(buff.get()==1);
        int exTitleData_count = buff.getShort();if(exTitleData_count>0){exTitleData = new byte[exTitleData_count];buff.get(exTitleData);} 
    }
	@Override
	public int getRecordExpiredTimeSec()
    {
    	return 0 ;
    }
}
