package NP2PS_RB.p003_LSOp;

import java.nio.ByteBuffer;
public class NP2PS_RB_003_001_RetGateKey implements ALBasicProtocolPack._IALProtocolStructure {
private boolean res;
private String uid;
private String gateServerIp;
private int gateServerPort;
private String checkCode;


public NP2PS_RB_003_001_RetGateKey() {
	res = false;
	uid = "";
	gateServerIp = "";
	gateServerPort = 0;
	checkCode = "";
}

public NP2PS_RB_003_001_RetGateKey(
	 boolean _res
	, String _uid
	, String _gateServerIp
	, int _gateServerPort
	, String _checkCode
) {	res = _res;
	uid = _uid;
	gateServerIp = _gateServerIp;
	gateServerPort = _gateServerPort;
	checkCode = _checkCode;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)1; }

public boolean getRes() { return res; }
public void setRes(boolean _res) { res = _res; }
public String getUid() { return uid; }
public void setUid(String _uid) { uid = _uid; }
public String getGateServerIp() { return gateServerIp; }
public void setGateServerIp(String _gateServerIp) { gateServerIp = _gateServerIp; }
public int getGateServerPort() { return gateServerPort; }
public void setGateServerPort(int _gateServerPort) { gateServerPort = _gateServerPort; }
public String getCheckCode() { return checkCode; }
public void setCheckCode(String _checkCode) { checkCode = _checkCode; }


public final int GetBufSize() {
	int _size = 5;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(gateServerIp);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(checkCode);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 7;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(gateServerIp);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(checkCode);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) res = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gateServerIp = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gateServerPort = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) checkCode = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(res?(byte)1:(byte)0);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, uid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, gateServerIp);
	_buf.putInt(gateServerPort);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, checkCode);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)1);
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

