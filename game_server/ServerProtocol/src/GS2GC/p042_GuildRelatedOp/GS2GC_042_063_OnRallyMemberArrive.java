package GS2GC.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
/*********
 * 成员到达集结推送
 **/
public class GS2GC_042_063_OnRallyMemberArrive implements ALBasicProtocolPack._IALProtocolStructure {
/** 集结ID */
private long rallyId;
/** 成员CID */
private long memberCid;


public GS2GC_042_063_OnRallyMemberArrive() {
	rallyId = (long)0;
	memberCid = (long)0;
}

public GS2GC_042_063_OnRallyMemberArrive(
	 long _rallyId
	, long _memberCid
) {	rallyId = _rallyId;
	memberCid = _memberCid;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)63; }

/** 集结ID */
public long getRallyId() { return rallyId; }
/** 集结ID */
public void setRallyId(long _rallyId) { rallyId = _rallyId; }
/** 成员CID */
public long getMemberCid() { return memberCid; }
/** 成员CID */
public void setMemberCid(long _memberCid) { memberCid = _memberCid; }


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
	if(_buf.remaining() > 0) rallyId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) memberCid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(rallyId);
	_buf.putLong(memberCid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)63);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)63);
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

