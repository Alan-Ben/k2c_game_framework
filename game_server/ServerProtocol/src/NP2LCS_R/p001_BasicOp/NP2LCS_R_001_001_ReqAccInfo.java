package NP2LCS_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2LCS_R_001_001_ReqAccInfo implements ALBasicProtocolPack._IALProtocolStructure {
private String accountName;
private String accountPass;


public NP2LCS_R_001_001_ReqAccInfo() {
	accountName = "";
	accountPass = "";
}

public NP2LCS_R_001_001_ReqAccInfo(
	 String _accountName
	, String _accountPass
) {	accountName = _accountName;
	accountPass = _accountPass;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)1; }

public String getAccountName() { return accountName; }
public void setAccountName(String _accountName) { accountName = _accountName; }
public String getAccountPass() { return accountPass; }
public void setAccountPass(String _accountPass) { accountPass = _accountPass; }


public final int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(accountName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(accountPass);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(accountName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(accountPass);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) accountName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) accountPass = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, accountName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, accountPass);
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

