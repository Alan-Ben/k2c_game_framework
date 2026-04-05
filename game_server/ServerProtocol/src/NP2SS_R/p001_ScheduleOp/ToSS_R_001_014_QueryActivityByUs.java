package NP2SS_R.p001_ScheduleOp;

import java.nio.ByteBuffer;
/*********
 * 查询指定US的待激活排期
 **/
public class ToSS_R_001_014_QueryActivityByUs implements ALBasicProtocolPack._IALProtocolStructure {
/** 用户服务器ID */
private int usId;


public ToSS_R_001_014_QueryActivityByUs() {
	usId = 0;
}

public ToSS_R_001_014_QueryActivityByUs(
	 int _usId
) {	usId = _usId;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)14; }

/** 用户服务器ID */
public int getUsId() { return usId; }
/** 用户服务器ID */
public void setUsId(int _usId) { usId = _usId; }


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
	if(_buf.remaining() > 0) usId = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(usId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)14);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)14);
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

