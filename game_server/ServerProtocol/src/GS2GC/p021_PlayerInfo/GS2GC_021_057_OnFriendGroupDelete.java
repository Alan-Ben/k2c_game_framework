package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 分组删除
 **/
public class GS2GC_021_057_OnFriendGroupDelete implements ALBasicProtocolPack._IALProtocolStructure {
/** 分组数据id */
private long groupDbId;


public GS2GC_021_057_OnFriendGroupDelete() {
	groupDbId = (long)0;
}

public GS2GC_021_057_OnFriendGroupDelete(
	 long _groupDbId
) {	groupDbId = _groupDbId;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)57; }

/** 分组数据id */
public long getGroupDbId() { return groupDbId; }
/** 分组数据id */
public void setGroupDbId(long _groupDbId) { groupDbId = _groupDbId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) groupDbId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(groupDbId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)57);
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

