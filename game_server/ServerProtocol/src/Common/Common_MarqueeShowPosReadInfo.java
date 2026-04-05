package Common;

import java.nio.ByteBuffer;
public class Common_MarqueeShowPosReadInfo implements ALBasicProtocolPack._IALProtocolStructure {
private int showPosId;
/** 已经读取的跑马灯唯一id */
private long hadReadDbId;


public Common_MarqueeShowPosReadInfo() {
	showPosId = 0;
	hadReadDbId = (long)0;
}

public Common_MarqueeShowPosReadInfo(
	 int _showPosId
	, long _hadReadDbId
) {	showPosId = _showPosId;
	hadReadDbId = _hadReadDbId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getShowPosId() { return showPosId; }
public void setShowPosId(int _showPosId) { showPosId = _showPosId; }
/** 已经读取的跑马灯唯一id */
public long getHadReadDbId() { return hadReadDbId; }
/** 已经读取的跑马灯唯一id */
public void setHadReadDbId(long _hadReadDbId) { hadReadDbId = _hadReadDbId; }


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
	if(_buf.remaining() > 0) showPosId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadReadDbId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(showPosId);
	_buf.putLong(hadReadDbId);
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

