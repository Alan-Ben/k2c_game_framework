package GC2GS.p013_HeroOp;

import java.nio.ByteBuffer;
/*********
 * 藏品技能重塑
 **/
public class GC2GS_013_022_ReqEquipSkillRebuild implements ALBasicProtocolPack._IALProtocolStructure {
/** 数据id */
private long dbId;
/** 技能索引 */
private int index;
/** 是否是高级 */
private boolean isAdvance;


public GC2GS_013_022_ReqEquipSkillRebuild() {
	dbId = (long)0;
	index = 0;
	isAdvance = false;
}

public GC2GS_013_022_ReqEquipSkillRebuild(
	 long _dbId
	, int _index
	, boolean _isAdvance
) {	dbId = _dbId;
	index = _index;
	isAdvance = _isAdvance;
}

public final byte getMainOrder() { return (byte)13; }

public final byte getSubOrder() { return (byte)22; }

/** 数据id */
public long getDbId() { return dbId; }
/** 数据id */
public void setDbId(long _dbId) { dbId = _dbId; }
/** 技能索引 */
public int getIndex() { return index; }
/** 技能索引 */
public void setIndex(int _index) { index = _index; }
/** 是否是高级 */
public boolean getIsAdvance() { return isAdvance; }
/** 是否是高级 */
public void setIsAdvance(boolean _isAdvance) { isAdvance = _isAdvance; }


public final int GetBufSize() {
	int _size = 13;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 15;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) index = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isAdvance = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dbId);
	_buf.putInt(index);
	_buf.put(isAdvance?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)22);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)22);
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

