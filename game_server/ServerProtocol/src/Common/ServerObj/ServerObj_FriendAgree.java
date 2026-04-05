package Common.ServerObj;

import java.nio.ByteBuffer;
/*********
 * 同意好友申请数据
 **/
public class ServerObj_FriendAgree implements ALBasicProtocolPack._IALProtocolStructure {
/** 同意申请玩家CID */
private long agreeCid;


public ServerObj_FriendAgree() {
	agreeCid = (long)0;
}

public ServerObj_FriendAgree(
	 long _agreeCid
) {	agreeCid = _agreeCid;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 同意申请玩家CID */
public long getAgreeCid() { return agreeCid; }
/** 同意申请玩家CID */
public void setAgreeCid(long _agreeCid) { agreeCid = _agreeCid; }


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
	if(_buf.remaining() > 0) agreeCid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(agreeCid);
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

