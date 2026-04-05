package NP2LCS_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2LCS_R_001_099_ReqUpdateAcc implements ALBasicProtocolPack._IALProtocolStructure {
private String userName;
private String pass;


public NP2LCS_R_001_099_ReqUpdateAcc() {
	userName = "";
	pass = "";
}

public NP2LCS_R_001_099_ReqUpdateAcc(
	 String _userName
	, String _pass
) {	userName = _userName;
	pass = _pass;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)99; }

public String getUserName() { return userName; }
public void setUserName(String _userName) { userName = _userName; }
public String getPass() { return pass; }
public void setPass(String _pass) { pass = _pass; }


public final int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(userName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(pass);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(userName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(pass);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) userName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pass = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, userName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, pass);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)99);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)99);
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

