package ALLRPC.US.Dinner;

import java.nio.ByteBuffer;
public class UsJoinDinner_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long instanceId;
private Common.ServerObj.ServerObj_DinnerJoiner player;


public UsJoinDinner_Req() {
	instanceId = (long)0;
	player = new Common.ServerObj.ServerObj_DinnerJoiner();
}

public UsJoinDinner_Req(
	 long _instanceId
	, Common.ServerObj.ServerObj_DinnerJoiner _player
) {	instanceId = _instanceId;
	player = _player;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getInstanceId() { return instanceId; }
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
public Common.ServerObj.ServerObj_DinnerJoiner getPlayer() { return player; }
public void setPlayer(Common.ServerObj.ServerObj_DinnerJoiner _player) { player = _player; }


public final int GetBufSize() {
	int _size = 8;
	_size += 4 + player.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 4 + player.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _playerCustLen = _buf.getInt();
	int _playerCurPos = _buf.position();
	player.ReadUnzipBuf(_buf, _playerCurPos + _playerCustLen);
	_buf.position(_playerCurPos + _playerCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(instanceId);
	_buf.putInt(player.GetBufSize());
	player.PutUnzipBuf(_buf);
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

