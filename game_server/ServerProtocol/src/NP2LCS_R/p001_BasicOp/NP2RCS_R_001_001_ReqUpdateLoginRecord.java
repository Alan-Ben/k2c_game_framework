package NP2LCS_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2RCS_R_001_001_ReqUpdateLoginRecord implements ALBasicProtocolPack._IALProtocolStructure {
/** 账号id */
private String accountId;
/** 角色id */
private long cid;
/** US服务器LogicId */
private int serverLogicId;
/** US服务器typeId */
private int serverTypeId;


public NP2RCS_R_001_001_ReqUpdateLoginRecord() {
	accountId = "";
	cid = (long)0;
	serverLogicId = 0;
	serverTypeId = 0;
}

public NP2RCS_R_001_001_ReqUpdateLoginRecord(
	 String _accountId
	, long _cid
	, int _serverLogicId
	, int _serverTypeId
) {	accountId = _accountId;
	cid = _cid;
	serverLogicId = _serverLogicId;
	serverTypeId = _serverTypeId;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)1; }

/** 账号id */
public String getAccountId() { return accountId; }
/** 账号id */
public void setAccountId(String _accountId) { accountId = _accountId; }
/** 角色id */
public long getCid() { return cid; }
/** 角色id */
public void setCid(long _cid) { cid = _cid; }
/** US服务器LogicId */
public int getServerLogicId() { return serverLogicId; }
/** US服务器LogicId */
public void setServerLogicId(int _serverLogicId) { serverLogicId = _serverLogicId; }
/** US服务器typeId */
public int getServerTypeId() { return serverTypeId; }
/** US服务器typeId */
public void setServerTypeId(int _serverTypeId) { serverTypeId = _serverTypeId; }


public final int GetBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(accountId);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(accountId);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) accountId = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverLogicId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) serverTypeId = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, accountId);
	_buf.putLong(cid);
	_buf.putInt(serverLogicId);
	_buf.putInt(serverTypeId);
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

