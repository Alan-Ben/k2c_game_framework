package NP2LCS_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2LCS_R_001_002_ReqAccInfoByChkKey implements ALBasicProtocolPack._IALProtocolStructure {
private String accountName;
private String chkKey;


public NP2LCS_R_001_002_ReqAccInfoByChkKey() {
	accountName = "";
	chkKey = "";
}

public NP2LCS_R_001_002_ReqAccInfoByChkKey(
	 String _accountName
	, String _chkKey
) {	accountName = _accountName;
	chkKey = _chkKey;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)2; }

public String getAccountName() { return accountName; }
public void setAccountName(String _accountName) { accountName = _accountName; }
public String getChkKey() { return chkKey; }
public void setChkKey(String _chkKey) { chkKey = _chkKey; }


public final int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(accountName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(chkKey);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(accountName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(chkKey);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) accountName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chkKey = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, accountName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, chkKey);
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

