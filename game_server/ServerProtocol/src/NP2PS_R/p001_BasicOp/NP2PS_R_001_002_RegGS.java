package NP2PS_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2PS_R_001_002_RegGS implements ALBasicProtocolPack._IALProtocolStructure {
private String areaTag;
private int userMaxCount;
private int userHandleWeight;
private String connectIp;
private int connectPort;


public NP2PS_R_001_002_RegGS() {
	areaTag = "";
	userMaxCount = 0;
	userHandleWeight = 0;
	connectIp = "";
	connectPort = 0;
}

public NP2PS_R_001_002_RegGS(
	 String _areaTag
	, int _userMaxCount
	, int _userHandleWeight
	, String _connectIp
	, int _connectPort
) {	areaTag = _areaTag;
	userMaxCount = _userMaxCount;
	userHandleWeight = _userHandleWeight;
	connectIp = _connectIp;
	connectPort = _connectPort;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)2; }

public String getAreaTag() { return areaTag; }
public void setAreaTag(String _areaTag) { areaTag = _areaTag; }
public int getUserMaxCount() { return userMaxCount; }
public void setUserMaxCount(int _userMaxCount) { userMaxCount = _userMaxCount; }
public int getUserHandleWeight() { return userHandleWeight; }
public void setUserHandleWeight(int _userHandleWeight) { userHandleWeight = _userHandleWeight; }
public String getConnectIp() { return connectIp; }
public void setConnectIp(String _connectIp) { connectIp = _connectIp; }
public int getConnectPort() { return connectPort; }
public void setConnectPort(int _connectPort) { connectPort = _connectPort; }


public final int GetBufSize() {
	int _size = 12;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(areaTag);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(connectIp);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(areaTag);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(connectIp);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) areaTag = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) userMaxCount = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) userHandleWeight = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) connectIp = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) connectPort = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, areaTag);
	_buf.putInt(userMaxCount);
	_buf.putInt(userHandleWeight);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, connectIp);
	_buf.putInt(connectPort);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)2);
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

