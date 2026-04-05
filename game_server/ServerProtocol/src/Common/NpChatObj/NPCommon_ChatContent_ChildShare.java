package Common.NpChatObj;

import java.nio.ByteBuffer;
/*********
 * 子嗣分享
 **/
public class NPCommon_ChatContent_ChildShare implements ALBasicProtocolPack._IALProtocolStructure {
/** 子嗣名称 */
private String childName;
/** 监护人id */
private long consortId;
/** 子嗣等级 */
private int childLevel;
/** 子嗣升学阶段 */
private int childStep;
/** 子嗣资源id */
private long childResId;
/** 子嗣职业id */
private long childCareerId;
/** 子嗣品质id */
private long childQualityId;
/** 相性 */
private CommonEnum.ESpecAttrType attrType;
/** 收益 */
private long earnings;
/** 单次教学奖励 */
private long educationExpValue;
/** 基础奖励 */
private long educatingBaseAwards;
/** 天资加成 */
private long childQualityBonus;
/** 情人羁绊加成 */
private long consortBonus;
/** 顾问技能加成 */
private long heroBonus;
/** 是否卷王 */
private boolean isSuper;
private long cid;
/** 是否毕业，未毕业的adultId无效 */
private boolean isGraduated;
private long adultId;


public NPCommon_ChatContent_ChildShare() {
	childName = "";
	consortId = (long)0;
	childLevel = 0;
	childStep = 0;
	childResId = (long)0;
	childCareerId = (long)0;
	childQualityId = (long)0;
	attrType = CommonEnum.ESpecAttrType.values()[0];
	earnings = (long)0;
	educationExpValue = (long)0;
	educatingBaseAwards = (long)0;
	childQualityBonus = (long)0;
	consortBonus = (long)0;
	heroBonus = (long)0;
	isSuper = false;
	cid = (long)0;
	isGraduated = false;
	adultId = (long)0;
}

public NPCommon_ChatContent_ChildShare(
	 String _childName
	, long _consortId
	, int _childLevel
	, int _childStep
	, long _childResId
	, long _childCareerId
	, long _childQualityId
	, CommonEnum.ESpecAttrType _attrType
	, long _earnings
	, long _educationExpValue
	, long _educatingBaseAwards
	, long _childQualityBonus
	, long _consortBonus
	, long _heroBonus
	, boolean _isSuper
	, long _cid
	, boolean _isGraduated
	, long _adultId
) {	childName = _childName;
	consortId = _consortId;
	childLevel = _childLevel;
	childStep = _childStep;
	childResId = _childResId;
	childCareerId = _childCareerId;
	childQualityId = _childQualityId;
	attrType = _attrType;
	earnings = _earnings;
	educationExpValue = _educationExpValue;
	educatingBaseAwards = _educatingBaseAwards;
	childQualityBonus = _childQualityBonus;
	consortBonus = _consortBonus;
	heroBonus = _heroBonus;
	isSuper = _isSuper;
	cid = _cid;
	isGraduated = _isGraduated;
	adultId = _adultId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 子嗣名称 */
public String getChildName() { return childName; }
/** 子嗣名称 */
public void setChildName(String _childName) { childName = _childName; }
/** 监护人id */
public long getConsortId() { return consortId; }
/** 监护人id */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 子嗣等级 */
public int getChildLevel() { return childLevel; }
/** 子嗣等级 */
public void setChildLevel(int _childLevel) { childLevel = _childLevel; }
/** 子嗣升学阶段 */
public int getChildStep() { return childStep; }
/** 子嗣升学阶段 */
public void setChildStep(int _childStep) { childStep = _childStep; }
/** 子嗣资源id */
public long getChildResId() { return childResId; }
/** 子嗣资源id */
public void setChildResId(long _childResId) { childResId = _childResId; }
/** 子嗣职业id */
public long getChildCareerId() { return childCareerId; }
/** 子嗣职业id */
public void setChildCareerId(long _childCareerId) { childCareerId = _childCareerId; }
/** 子嗣品质id */
public long getChildQualityId() { return childQualityId; }
/** 子嗣品质id */
public void setChildQualityId(long _childQualityId) { childQualityId = _childQualityId; }
/** 相性 */
public CommonEnum.ESpecAttrType getAttrType() { return attrType; }
/** 相性 */
public void setAttrType(CommonEnum.ESpecAttrType _attrType) { attrType = _attrType; }
/** 收益 */
public long getEarnings() { return earnings; }
/** 收益 */
public void setEarnings(long _earnings) { earnings = _earnings; }
/** 单次教学奖励 */
public long getEducationExpValue() { return educationExpValue; }
/** 单次教学奖励 */
public void setEducationExpValue(long _educationExpValue) { educationExpValue = _educationExpValue; }
/** 基础奖励 */
public long getEducatingBaseAwards() { return educatingBaseAwards; }
/** 基础奖励 */
public void setEducatingBaseAwards(long _educatingBaseAwards) { educatingBaseAwards = _educatingBaseAwards; }
/** 天资加成 */
public long getChildQualityBonus() { return childQualityBonus; }
/** 天资加成 */
public void setChildQualityBonus(long _childQualityBonus) { childQualityBonus = _childQualityBonus; }
/** 情人羁绊加成 */
public long getConsortBonus() { return consortBonus; }
/** 情人羁绊加成 */
public void setConsortBonus(long _consortBonus) { consortBonus = _consortBonus; }
/** 顾问技能加成 */
public long getHeroBonus() { return heroBonus; }
/** 顾问技能加成 */
public void setHeroBonus(long _heroBonus) { heroBonus = _heroBonus; }
/** 是否卷王 */
public boolean getIsSuper() { return isSuper; }
/** 是否卷王 */
public void setIsSuper(boolean _isSuper) { isSuper = _isSuper; }
public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
/** 是否毕业，未毕业的adultId无效 */
public boolean getIsGraduated() { return isGraduated; }
/** 是否毕业，未毕业的adultId无效 */
public void setIsGraduated(boolean _isGraduated) { isGraduated = _isGraduated; }
public long getAdultId() { return adultId; }
public void setAdultId(long _adultId) { adultId = _adultId; }


public final int GetBufSize() {
	int _size = 110;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(childName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 112;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(childName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) childName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) childLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) childStep = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) childResId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) childCareerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) childQualityId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) attrType = CommonEnum.ESpecAttrType.ESpecAttrType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) earnings = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) educationExpValue = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) educatingBaseAwards = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) childQualityBonus = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortBonus = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroBonus = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isSuper = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isGraduated = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) adultId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, childName);
	_buf.putLong(consortId);
	_buf.putInt(childLevel);
	_buf.putInt(childStep);
	_buf.putLong(childResId);
	_buf.putLong(childCareerId);
	_buf.putLong(childQualityId);
	_buf.putInt(attrType.ordinal());

	_buf.putLong(earnings);
	_buf.putLong(educationExpValue);
	_buf.putLong(educatingBaseAwards);
	_buf.putLong(childQualityBonus);
	_buf.putLong(consortBonus);
	_buf.putLong(heroBonus);
	_buf.put(isSuper?(byte)1:(byte)0);
	_buf.putLong(cid);
	_buf.put(isGraduated?(byte)1:(byte)0);
	_buf.putLong(adultId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

