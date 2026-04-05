package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_011_RetSomeOnePlayerBriefInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家简要信息 */
private NPCommon.PlayerInfo_IconShow playerBrief;


public GS2GC_004_011_RetSomeOnePlayerBriefInfo() {
	playerBrief = new NPCommon.PlayerInfo_IconShow();
}

public GS2GC_004_011_RetSomeOnePlayerBriefInfo(
	 NPCommon.PlayerInfo_IconShow _playerBrief
) {	playerBrief = _playerBrief;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)11; }

/** 玩家简要信息 */
public NPCommon.PlayerInfo_IconShow getPlayerBrief() { return playerBrief; }
/** 玩家简要信息 */
public void setPlayerBrief(NPCommon.PlayerInfo_IconShow _playerBrief) { playerBrief = _playerBrief; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + playerBrief.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + playerBrief.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _playerBriefCustLen = _buf.getInt();
	int _playerBriefCurPos = _buf.position();
	playerBrief.ReadUnzipBuf(_buf, _playerBriefCurPos + _playerBriefCustLen);
	_buf.position(_playerBriefCurPos + _playerBriefCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(playerBrief.GetBufSize());
	playerBrief.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
	_recBuf.put((byte)11);
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

