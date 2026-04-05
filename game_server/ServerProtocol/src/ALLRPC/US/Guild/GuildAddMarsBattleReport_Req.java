package ALLRPC.US.Guild;

import java.nio.ByteBuffer;
public class GuildAddMarsBattleReport_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private long guildId;
private Common.ServerObj.ServerObj_MarsExplorePVPLog logObj;


public GuildAddMarsBattleReport_Req() {
	cid = (long)0;
	guildId = (long)0;
	logObj = new Common.ServerObj.ServerObj_MarsExplorePVPLog();
}

public GuildAddMarsBattleReport_Req(
	 long _cid
	, long _guildId
	, Common.ServerObj.ServerObj_MarsExplorePVPLog _logObj
) {	cid = _cid;
	guildId = _guildId;
	logObj = _logObj;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getGuildId() { return guildId; }
public void setGuildId(long _guildId) { guildId = _guildId; }
public Common.ServerObj.ServerObj_MarsExplorePVPLog getLogObj() { return logObj; }
public void setLogObj(Common.ServerObj.ServerObj_MarsExplorePVPLog _logObj) { logObj = _logObj; }


public final int GetBufSize() {
	int _size = 16;
	_size += 4 + logObj.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 4 + logObj.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _logObjCustLen = _buf.getInt();
	int _logObjCurPos = _buf.position();
	logObj.ReadUnzipBuf(_buf, _logObjCurPos + _logObjCustLen);
	_buf.position(_logObjCurPos + _logObjCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(guildId);
	_buf.putInt(logObj.GetBufSize());
	logObj.PutUnzipBuf(_buf);
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

