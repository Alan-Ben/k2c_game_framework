package Common.TowerObj;

import java.nio.ByteBuffer;
/*********
 * 爬塔_对手信息
 **/
public class Tower_OpponentInfo implements ALBasicProtocolPack._IALProtocolStructure {
private Common.TowerObj.Tower_PosInfo posInfo;
private long playerId;


public Tower_OpponentInfo() {
	posInfo = new Common.TowerObj.Tower_PosInfo();
	playerId = (long)0;
}

public Tower_OpponentInfo(
	 Common.TowerObj.Tower_PosInfo _posInfo
	, long _playerId
) {	posInfo = _posInfo;
	playerId = _playerId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public Common.TowerObj.Tower_PosInfo getPosInfo() { return posInfo; }
public void setPosInfo(Common.TowerObj.Tower_PosInfo _posInfo) { posInfo = _posInfo; }
public long getPlayerId() { return playerId; }
public void setPlayerId(long _playerId) { playerId = _playerId; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _posInfoCustLen = _buf.getInt();
	int _posInfoCurPos = _buf.position();
	posInfo.ReadUnzipBuf(_buf, _posInfoCurPos + _posInfoCustLen);
	_buf.position(_posInfoCurPos + _posInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) playerId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(posInfo.GetBufSize());
	posInfo.PutUnzipBuf(_buf);
	_buf.putLong(playerId);
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

