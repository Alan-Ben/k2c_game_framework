package GC2GS.p036_TreasureHuntOp;

import java.nio.ByteBuffer;
/*********
 * 太空寻宝-矿石技能激活
 **/
public class GC2GS_036_002_ReqTreasureHuntOreSkillActive implements ALBasicProtocolPack._IALProtocolStructure {
/** 矿石ID */
private long oreId;
/** true普通技能 false高级技能 */
private boolean isNormal;


public GC2GS_036_002_ReqTreasureHuntOreSkillActive() {
	oreId = (long)0;
	isNormal = false;
}

public GC2GS_036_002_ReqTreasureHuntOreSkillActive(
	 long _oreId
	, boolean _isNormal
) {	oreId = _oreId;
	isNormal = _isNormal;
}

public final byte getMainOrder() { return (byte)36; }

public final byte getSubOrder() { return (byte)2; }

/** 矿石ID */
public long getOreId() { return oreId; }
/** 矿石ID */
public void setOreId(long _oreId) { oreId = _oreId; }
/** true普通技能 false高级技能 */
public boolean getIsNormal() { return isNormal; }
/** true普通技能 false高级技能 */
public void setIsNormal(boolean _isNormal) { isNormal = _isNormal; }


public final int GetBufSize() {
	int _size = 9;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) oreId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isNormal = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(oreId);
	_buf.put(isNormal?(byte)1:(byte)0);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)36);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)36);
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

