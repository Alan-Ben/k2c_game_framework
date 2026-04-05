package GC2GS.p004_PlayerOp;

import java.nio.ByteBuffer;
/*********
 * 领取七日登录奖励
 **/
public class GC2GS_004_036_ReqDrawLoginCountReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 第几天 */
private int day;


public GC2GS_004_036_ReqDrawLoginCountReward() {
	day = 0;
}

public GC2GS_004_036_ReqDrawLoginCountReward(
	 int _day
) {	day = _day;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)36; }

/** 第几天 */
public int getDay() { return day; }
/** 第几天 */
public void setDay(int _day) { day = _day; }


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
	if(_buf.remaining() > 0) day = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(day);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)36);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)36);
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

