package NP2HS_RB.p002_HsClientOp;

import java.nio.ByteBuffer;
public class ExeGmSuperResult implements ALBasicProtocolPack._IALProtocolStructure {
private int serverType;
private int serverTypeId;
private boolean isSucc;
private String result;


public ExeGmSuperResult() {
	serverType = 0;
	serverTypeId = 0;
	isSucc = false;
	result = "";
}

public ExeGmSuperResult(
	 int _serverType
	, int _serverTypeId
	, boolean _isSucc
	, String _result
) {	serverType = _serverType;
	serverTypeId = _serverTypeId;
	isSucc = _isSucc;
	result = _result;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getServerType() { return serverType; }
public void setServerType(int _serverType) { serverType = _serverType; }
public int getServerTypeId() { return serverTypeId; }
public void setServerTypeId(int _serverTypeId) { serverTypeId = _serverTypeId; }
public boolean getIsSucc() { return isSucc; }
public void setIsSucc(boolean _isSucc) { isSucc = _isSucc; }
public String getResult() { return result; }
public void setResult(String _result) { result = _result; }


public final int GetBufSize() {
	int _size = 9;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(result);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(result);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverTypeId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isSucc = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) result = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(serverType);
	_buf.putInt(serverTypeId);
	_buf.put(isSucc?(byte)1:(byte)0);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, result);
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

