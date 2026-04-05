package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_001_RetPlayerInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private String cname;
private java.util.ArrayList<NPCommon.NPCommon_PlayerParam> playerParams;


public GS2GC_002_001_RetPlayerInfo() {
	cid = (long)0;
	cname = "";
	playerParams = new java.util.ArrayList<NPCommon.NPCommon_PlayerParam>();
}

public GS2GC_002_001_RetPlayerInfo(
	 long _cid
	, String _cname
	, java.util.ArrayList<NPCommon.NPCommon_PlayerParam> _playerParams
) {	cid = _cid;
	cname = _cname;
	playerParams = _playerParams;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)1; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public String getCname() { return cname; }
public void setCname(String _cname) { cname = _cname; }
public java.util.ArrayList<NPCommon.NPCommon_PlayerParam> getPlayerParams() { return playerParams; }
public void addPlayerParams(NPCommon.NPCommon_PlayerParam _playerParams) { playerParams.add(_playerParams); }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);
	_size += 2 + (playerParams.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);
	_size += 2 + (playerParams.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cname = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _playerParamsCount = _buf.getShort();
	for(int _i = 0; _i < _playerParamsCount; _i++) { 
		NPCommon.NPCommon_PlayerParam _playerParams = new NPCommon.NPCommon_PlayerParam();
		if(_buf.remaining() <= 0) return;
	int __playerParamsCustLen = _buf.getInt();
	int __playerParamsCurPos = _buf.position();
	_playerParams.ReadUnzipBuf(_buf, __playerParamsCurPos + __playerParamsCustLen);
	_buf.position(__playerParamsCurPos + __playerParamsCustLen);

		playerParams.add(_playerParams);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, cname);
	_buf.putShort((short)playerParams.size());
	for(int _i = 0; _i < playerParams.size(); _i++) { 
		_buf.putInt(playerParams.get(_i).GetBufSize());
	playerParams.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
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

