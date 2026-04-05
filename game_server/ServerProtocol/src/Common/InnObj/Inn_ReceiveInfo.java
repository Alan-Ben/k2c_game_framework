package Common.InnObj;

import java.nio.ByteBuffer;
/*********
 * 旅店_接待队列信息
 **/
public class Inn_ReceiveInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 接待开始时间 毫秒 */
private long startTimeMs;
/** 需要接待数量 */
private int needReceiveNum;


public Inn_ReceiveInfo() {
	startTimeMs = (long)0;
	needReceiveNum = 0;
}

public Inn_ReceiveInfo(
	 long _startTimeMs
	, int _needReceiveNum
) {	startTimeMs = _startTimeMs;
	needReceiveNum = _needReceiveNum;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 接待开始时间 毫秒 */
public long getStartTimeMs() { return startTimeMs; }
/** 接待开始时间 毫秒 */
public void setStartTimeMs(long _startTimeMs) { startTimeMs = _startTimeMs; }
/** 需要接待数量 */
public int getNeedReceiveNum() { return needReceiveNum; }
/** 需要接待数量 */
public void setNeedReceiveNum(int _needReceiveNum) { needReceiveNum = _needReceiveNum; }


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
	if(_buf.remaining() > 0) startTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) needReceiveNum = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(startTimeMs);
	_buf.putInt(needReceiveNum);
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

