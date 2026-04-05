package GS2GC.p034_InnOp;

import java.nio.ByteBuffer;
/*********
 * 旅店等级变更
 **/
public class GS2GC_034_057_OnInnLevelChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 新等级 */
private int newLevel;


public GS2GC_034_057_OnInnLevelChg() {
	newLevel = 0;
}

public GS2GC_034_057_OnInnLevelChg(
	 int _newLevel
) {	newLevel = _newLevel;
}

public final byte getMainOrder() { return (byte)34; }

public final byte getSubOrder() { return (byte)57; }

/** 新等级 */
public int getNewLevel() { return newLevel; }
/** 新等级 */
public void setNewLevel(int _newLevel) { newLevel = _newLevel; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) newLevel = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(newLevel);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)57);
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

