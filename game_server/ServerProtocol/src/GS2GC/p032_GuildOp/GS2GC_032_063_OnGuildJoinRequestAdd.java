package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_063_OnGuildJoinRequestAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 入盟请求 */
private Common.GuildObj.Guild_JoinRequestInfo joinRequestList;


public GS2GC_032_063_OnGuildJoinRequestAdd() {
	joinRequestList = new Common.GuildObj.Guild_JoinRequestInfo();
}

public GS2GC_032_063_OnGuildJoinRequestAdd(
	 Common.GuildObj.Guild_JoinRequestInfo _joinRequestList
) {	joinRequestList = _joinRequestList;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)63; }

/** 入盟请求 */
public Common.GuildObj.Guild_JoinRequestInfo getJoinRequestList() { return joinRequestList; }
/** 入盟请求 */
public void setJoinRequestList(Common.GuildObj.Guild_JoinRequestInfo _joinRequestList) { joinRequestList = _joinRequestList; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _joinRequestListCustLen = _buf.getInt();
	int _joinRequestListCurPos = _buf.position();
	joinRequestList.ReadUnzipBuf(_buf, _joinRequestListCurPos + _joinRequestListCustLen);
	_buf.position(_joinRequestListCurPos + _joinRequestListCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(joinRequestList.GetBufSize());
	joinRequestList.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)63);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
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

