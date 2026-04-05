package GS2GC.p034_InnOp;

import java.nio.ByteBuffer;
/*********
 * 旅店奖牌等级变更
 **/
public class GS2GC_034_059_OnInnMedalLevelChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 新奖牌等级 */
private int medalLevel;


public GS2GC_034_059_OnInnMedalLevelChg() {
	medalLevel = 0;
}

public GS2GC_034_059_OnInnMedalLevelChg(
	 int _medalLevel
) {	medalLevel = _medalLevel;
}

public final byte getMainOrder() { return (byte)34; }

public final byte getSubOrder() { return (byte)59; }

/** 新奖牌等级 */
public int getMedalLevel() { return medalLevel; }
/** 新奖牌等级 */
public void setMedalLevel(int _medalLevel) { medalLevel = _medalLevel; }


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
	if(_buf.remaining() > 0) medalLevel = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(medalLevel);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)59);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
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

