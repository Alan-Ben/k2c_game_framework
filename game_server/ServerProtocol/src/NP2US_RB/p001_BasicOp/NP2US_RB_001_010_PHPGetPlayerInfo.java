package NP2US_RB.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2US_RB_001_010_PHPGetPlayerInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 平台需要的玩家信息数据 */
private Common.ServerObj.ServerObj_PHPPlayer player;


public NP2US_RB_001_010_PHPGetPlayerInfo() {
	player = new Common.ServerObj.ServerObj_PHPPlayer();
}

public NP2US_RB_001_010_PHPGetPlayerInfo(
	 Common.ServerObj.ServerObj_PHPPlayer _player
) {	player = _player;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)10; }

/** 平台需要的玩家信息数据 */
public Common.ServerObj.ServerObj_PHPPlayer getPlayer() { return player; }
/** 平台需要的玩家信息数据 */
public void setPlayer(Common.ServerObj.ServerObj_PHPPlayer _player) { player = _player; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + player.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + player.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _playerCustLen = _buf.getInt();
	int _playerCurPos = _buf.position();
	player.ReadUnzipBuf(_buf, _playerCurPos + _playerCustLen);
	_buf.position(_playerCurPos + _playerCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(player.GetBufSize());
	player.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

