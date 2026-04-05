package GS2GC.p004_PlayerOp;

import java.nio.ByteBuffer;
public class GS2GC_004_057_OnStationInfoChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 贸易站信息 */
private Common.PlayerObj.Player_StationInfo stationInfo;


public GS2GC_004_057_OnStationInfoChg() {
	stationInfo = new Common.PlayerObj.Player_StationInfo();
}

public GS2GC_004_057_OnStationInfoChg(
	 Common.PlayerObj.Player_StationInfo _stationInfo
) {	stationInfo = _stationInfo;
}

public final byte getMainOrder() { return (byte)4; }

public final byte getSubOrder() { return (byte)57; }

/** 贸易站信息 */
public Common.PlayerObj.Player_StationInfo getStationInfo() { return stationInfo; }
/** 贸易站信息 */
public void setStationInfo(Common.PlayerObj.Player_StationInfo _stationInfo) { stationInfo = _stationInfo; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _stationInfoCustLen = _buf.getInt();
	int _stationInfoCurPos = _buf.position();
	stationInfo.ReadUnzipBuf(_buf, _stationInfoCurPos + _stationInfoCustLen);
	_buf.position(_stationInfoCurPos + _stationInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(stationInfo.GetBufSize());
	stationInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)4);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)4);
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

