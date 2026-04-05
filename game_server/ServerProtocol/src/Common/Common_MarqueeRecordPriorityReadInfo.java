package Common;

import java.nio.ByteBuffer;
public class Common_MarqueeRecordPriorityReadInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long priorityId;
private long hadReadDbId;


public Common_MarqueeRecordPriorityReadInfo() {
	priorityId = (long)0;
	hadReadDbId = (long)0;
}

public Common_MarqueeRecordPriorityReadInfo(
	 long _priorityId
	, long _hadReadDbId
) {	priorityId = _priorityId;
	hadReadDbId = _hadReadDbId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getPriorityId() { return priorityId; }
public void setPriorityId(long _priorityId) { priorityId = _priorityId; }
public long getHadReadDbId() { return hadReadDbId; }
public void setHadReadDbId(long _hadReadDbId) { hadReadDbId = _hadReadDbId; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) priorityId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadReadDbId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(priorityId);
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

