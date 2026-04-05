package NP2LCS_RB.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2LCS_RB_001_001_RetAccInfo implements ALBasicProtocolPack._IALProtocolStructure {
private boolean res;
private String uid;
private String chkKey;


public NP2LCS_RB_001_001_RetAccInfo() {
	res = false;
	uid = "";
	chkKey = "";
}

public NP2LCS_RB_001_001_RetAccInfo(
	 boolean _res
	, String _uid
	, String _chkKey
) {	res = _res;
	uid = _uid;
	chkKey = _chkKey;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)1; }

public boolean getRes() { return res; }
public void setRes(boolean _res) { res = _res; }
public String getUid() { return uid; }
public void setUid(String _uid) { uid = _uid; }
public String getChkKey() { return chkKey; }
public void setChkKey(String _chkKey) { chkKey = _chkKey; }


public final int GetBufSize() {
	int _size = 1;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(chkKey);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(uid);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(chkKey);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) res = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chkKey = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(res?(byte)1:(byte)0);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, uid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, chkKey);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
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

