package ALLRPC.CrossTeamServer.Team;

import java.nio.ByteBuffer;
public class CTSQuitTeamGroup_Req implements ALBasicProtocolPack._IALProtocolStructure {
/** 分组实例ID */
private long groupId;
/** US服务器ID */
private long usId;


public CTSQuitTeamGroup_Req() {
	groupId = (long)0;
	usId = (long)0;
}

public CTSQuitTeamGroup_Req(
	 long _groupId
	, long _usId
) {	groupId = _groupId;
	usId = _usId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 分组实例ID */
public long getGroupId() { return groupId; }
/** 分组实例ID */
public void setGroupId(long _groupId) { groupId = _groupId; }
/** US服务器ID */
public long getUsId() { return usId; }
/** US服务器ID */
public void setUsId(long _usId) { usId = _usId; }


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
	if(_buf.remaining() > 0) usId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupId);
	_buf.putLong(usId);
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

