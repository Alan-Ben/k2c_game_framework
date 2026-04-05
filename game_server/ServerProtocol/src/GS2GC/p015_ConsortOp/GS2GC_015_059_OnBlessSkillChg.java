package GS2GC.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人加护技能数据变更
 **/
public class GS2GC_015_059_OnBlessSkillChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private long consortId;
/** 家人加护技能数据 */
private Common.ConsortObj.Consort_BlessSkill skill;


public GS2GC_015_059_OnBlessSkillChg() {
	consortId = (long)0;
	skill = new Common.ConsortObj.Consort_BlessSkill();
}

public GS2GC_015_059_OnBlessSkillChg(
	 long _consortId
	, Common.ConsortObj.Consort_BlessSkill _skill
) {	consortId = _consortId;
	skill = _skill;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)59; }

/** 空 */
public long getConsortId() { return consortId; }
/** 空 */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 家人加护技能数据 */
public Common.ConsortObj.Consort_BlessSkill getSkill() { return skill; }
/** 家人加护技能数据 */
public void setSkill(Common.ConsortObj.Consort_BlessSkill _skill) { skill = _skill; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _skillCustLen = _buf.getInt();
	int _skillCurPos = _buf.position();
	skill.ReadUnzipBuf(_buf, _skillCurPos + _skillCustLen);
	_buf.position(_skillCurPos + _skillCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putInt(skill.GetBufSize());
	skill.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)59);
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

