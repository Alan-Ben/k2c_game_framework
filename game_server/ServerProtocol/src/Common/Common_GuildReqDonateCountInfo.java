package Common;

import java.nio.ByteBuffer;
public class Common_GuildReqDonateCountInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long donateUid;
private short donateCount;


public Common_GuildReqDonateCountInfo() {
	donateUid = (long)0;
	donateCount = (short)0;
}

public Common_GuildReqDonateCountInfo(
	 long _donateUid
	, short _donateCount
) {	donateUid = _donateUid;
	donateCount = _donateCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getDonateUid() { return donateUid; }
public void setDonateUid(long _donateUid) { donateUid = _donateUid; }
public short getDonateCount() { return donateCount; }
public void setDonateCount(short _donateCount) { donateCount = _donateCount; }


public final int GetBufSize() {
	int _size = 10;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 12;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) donateUid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) donateCount = _buf.getShort();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(donateUid);
	_buf.putShort(donateCount);
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

