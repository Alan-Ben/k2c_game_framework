package NP2CGS_RB.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2CGS_RB_001_004_RetGM implements ALBasicProtocolPack._IALProtocolStructure {
private boolean res;
private String returnMsg;


public NP2CGS_RB_001_004_RetGM() {
	res = false;
	returnMsg = "";
}

public NP2CGS_RB_001_004_RetGM(
	 boolean _res
	, String _returnMsg
) {	res = _res;
	returnMsg = _returnMsg;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)4; }

public boolean getRes() { return res; }
public void setRes(boolean _res) { res = _res; }
public String getReturnMsg() { return returnMsg; }
public void setReturnMsg(String _returnMsg) { returnMsg = _returnMsg; }


public final int GetBufSize() {
	int _size = 1;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(returnMsg);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(returnMsg);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) res = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) returnMsg = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(res?(byte)1:(byte)0);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, returnMsg);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

