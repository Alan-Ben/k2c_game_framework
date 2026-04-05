package ALLRPC.MarryMatchServer.MarryMatch;

import java.nio.ByteBuffer;
public class MmsRemoveMatchItem_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 分组ID */
private long groupId;
/** 请求子嗣ID */
private long applyAdultId;


public MmsRemoveMatchItem_Req() {
	groupId = (long)0;
	applyAdultId = (long)0;
}

public MmsRemoveMatchItem_Req(
	 long _groupId
	, long _applyAdultId
) {	groupId = _groupId;
	applyAdultId = _applyAdultId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 分组ID */
public long getGroupId() { return groupId; }
/** 分组ID */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** 请求子嗣ID */
public long getApplyAdultId() { return applyAdultId; }
/** 请求子嗣ID */
public void setApplyAdultId(long _applyAdultId) { applyAdultId = _applyAdultId; }


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
	if(_buf.remaining() > 0) groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyAdultId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putLong(applyAdultId);
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

