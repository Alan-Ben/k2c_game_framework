package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_010_RetSomeOnePlayerInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家展示信息 */
private Common.NpPlayerInfoObj.PlayerInfo_CommonShow someOneShowInfo;


public GS2GC_004_010_RetSomeOnePlayerInfo() {
	someOneShowInfo = new Common.NpPlayerInfoObj.PlayerInfo_CommonShow();
}

public GS2GC_004_010_RetSomeOnePlayerInfo(
	 Common.NpPlayerInfoObj.PlayerInfo_CommonShow _someOneShowInfo
) {	someOneShowInfo = _someOneShowInfo;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)10; }

/** 玩家展示信息 */
public Common.NpPlayerInfoObj.PlayerInfo_CommonShow getSomeOneShowInfo() { return someOneShowInfo; }
/** 玩家展示信息 */
public void setSomeOneShowInfo(Common.NpPlayerInfoObj.PlayerInfo_CommonShow _someOneShowInfo) { someOneShowInfo = _someOneShowInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + someOneShowInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + someOneShowInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _someOneShowInfoCustLen = _buf.getInt();
	int _someOneShowInfoCurPos = _buf.position();
	someOneShowInfo.ReadUnzipBuf(_buf, _someOneShowInfoCurPos + _someOneShowInfoCustLen);
	_buf.position(_someOneShowInfoCurPos + _someOneShowInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(someOneShowInfo.GetBufSize());
	someOneShowInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)10);
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

