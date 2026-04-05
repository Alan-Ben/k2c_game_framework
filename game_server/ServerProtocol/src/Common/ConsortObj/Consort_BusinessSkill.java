package Common.ConsortObj;

import java.nio.ByteBuffer;
/*********
 * 家人经营技能
 **/
public class Consort_BusinessSkill implements ALBasicProtocolPack._IALProtocolStructure {
/** 技能ID */
private long skillId;
/** 概率加成 */
private long proAdd;
/** 普通领悟次数 */
private int normalOpCount;
/** 高级领悟次数 */
private int advanceOpCount;


public Consort_BusinessSkill() {
	skillId = (long)0;
	proAdd = (long)0;
	normalOpCount = 0;
	advanceOpCount = 0;
}

public Consort_BusinessSkill(
	 long _skillId
	, long _proAdd
	, int _normalOpCount
	, int _advanceOpCount
) {	skillId = _skillId;
	proAdd = _proAdd;
	normalOpCount = _normalOpCount;
	advanceOpCount = _advanceOpCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 技能ID */
public long getSkillId() { return skillId; }
/** 技能ID */
public void setSkillId(long _skillId) { skillId = _skillId; }
/** 概率加成 */
public long getProAdd() { return proAdd; }
/** 概率加成 */
public void setProAdd(long _proAdd) { proAdd = _proAdd; }
/** 普通领悟次数 */
public int getNormalOpCount() { return normalOpCount; }
/** 普通领悟次数 */
public void setNormalOpCount(int _normalOpCount) { normalOpCount = _normalOpCount; }
/** 高级领悟次数 */
public int getAdvanceOpCount() { return advanceOpCount; }
/** 高级领悟次数 */
public void setAdvanceOpCount(int _advanceOpCount) { advanceOpCount = _advanceOpCount; }


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
	if(_buf.remaining() > 0) skillId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) proAdd = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) normalOpCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) advanceOpCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(skillId);
	_buf.putLong(proAdd);
	_buf.putInt(normalOpCount);
	_buf.putInt(advanceOpCount);
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

