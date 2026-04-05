package GC2GS.p015_ConsortOp;

import java.nio.ByteBuffer;
/*********
 * 家人-领悟经营技能等级
 **/
public class GC2GS_015_002_ReqUnderstandBusinessSkill implements ALBasicProtocolPack._IALProtocolStructure {
/** 家人ID */
private long consortId;
/** 经营技能ID */
private long skillId;
/** 是否高级领悟 */
private boolean isAdvanced;


public GC2GS_015_002_ReqUnderstandBusinessSkill() {
	consortId = (long)0;
	skillId = (long)0;
	isAdvanced = false;
}

public GC2GS_015_002_ReqUnderstandBusinessSkill(
	 long _consortId
	, long _skillId
	, boolean _isAdvanced
) {	consortId = _consortId;
	skillId = _skillId;
	isAdvanced = _isAdvanced;
}

public final byte getMainOrder() { return (byte)15; }

public final byte getSubOrder() { return (byte)2; }

/** 家人ID */
public long getConsortId() { return consortId; }
/** 家人ID */
public void setConsortId(long _consortId) { consortId = _consortId; }
/** 经营技能ID */
public long getSkillId() { return skillId; }
/** 经营技能ID */
public void setSkillId(long _skillId) { skillId = _skillId; }
/** 是否高级领悟 */
public boolean getIsAdvanced() { return isAdvanced; }
/** 是否高级领悟 */
public void setIsAdvanced(boolean _isAdvanced) { isAdvanced = _isAdvanced; }


public final int GetBufSize() {
	int _size = 17;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) skillId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isAdvanced = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(consortId);
	_buf.putLong(skillId);
	_buf.put(isAdvanced?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)2);
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

