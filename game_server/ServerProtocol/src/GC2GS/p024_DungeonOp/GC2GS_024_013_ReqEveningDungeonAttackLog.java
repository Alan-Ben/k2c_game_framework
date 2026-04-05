package GC2GS.p024_DungeonOp;

import java.nio.ByteBuffer;
/*********
 * 晚间副本攻击日志
 **/
public class GC2GS_024_013_ReqEveningDungeonAttackLog implements ALBasicProtocolPack._IALProtocolStructure {
private long serial;
/** 需要数量 */
private int needNum;


public GC2GS_024_013_ReqEveningDungeonAttackLog() {
	serial = (long)0;
	needNum = 0;
}

public GC2GS_024_013_ReqEveningDungeonAttackLog(
	 long _serial
	, int _needNum
) {	serial = _serial;
	needNum = _needNum;
}

public final byte getMainOrder() { return (byte)24; }

public final byte getSubOrder() { return (byte)13; }

public long getSerial() { return serial; }
public void setSerial(long _serial) { serial = _serial; }
/** 需要数量 */
public int getNeedNum() { return needNum; }
/** 需要数量 */
public void setNeedNum(int _needNum) { needNum = _needNum; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serial = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) needNum = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(serial);
	_buf.putInt(needNum);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)13);
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

