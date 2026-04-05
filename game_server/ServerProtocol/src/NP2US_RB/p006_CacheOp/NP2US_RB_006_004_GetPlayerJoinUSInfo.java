package NP2US_RB.p006_CacheOp;

import java.nio.ByteBuffer;
public class NP2US_RB_006_004_GetPlayerJoinUSInfo implements ALBasicProtocolPack._IALProtocolStructure {
private Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo joinInfo;


public NP2US_RB_006_004_GetPlayerJoinUSInfo() {
	joinInfo = new Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo();
}

public NP2US_RB_006_004_GetPlayerJoinUSInfo(
	 Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo _joinInfo
) {	joinInfo = _joinInfo;
}

public final byte getMainOrder() { return (byte)6; }

public final byte getSubOrder() { return (byte)4; }

public Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo getJoinInfo() { return joinInfo; }
public void setJoinInfo(Common.NpPlayerInfoObj.NP_SYS_PlayerJoinedUSInfo _joinInfo) { joinInfo = _joinInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + joinInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + joinInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _joinInfoCustLen = _buf.getInt();
	int _joinInfoCurPos = _buf.position();
	joinInfo.ReadUnzipBuf(_buf, _joinInfoCurPos + _joinInfoCustLen);
	_buf.position(_joinInfoCurPos + _joinInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(joinInfo.GetBufSize());
	joinInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)6);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
	_recBuf.put((byte)4);
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

