package ALLRPC.US.Guild;

import java.nio.ByteBuffer;
public class GuildMemberRmv_2C_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private long guildId;
private String guildName;
private long timeMS;
private boolean isKicked;


public GuildMemberRmv_2C_Req() {
	cid = (long)0;
	guildId = (long)0;
	guildName = "";
	timeMS = (long)0;
	isKicked = false;
}

public GuildMemberRmv_2C_Req(
	 long _cid
	, long _guildId
	, String _guildName
	, long _timeMS
	, boolean _isKicked
) {	cid = _cid;
	guildId = _guildId;
	guildName = _guildName;
	timeMS = _timeMS;
	isKicked = _isKicked;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getGuildId() { return guildId; }
public void setGuildId(long _guildId) { guildId = _guildId; }
public String getGuildName() { return guildName; }
public void setGuildName(String _guildName) { guildName = _guildName; }
public long getTimeMS() { return timeMS; }
public void setTimeMS(long _timeMS) { timeMS = _timeMS; }
public boolean getIsKicked() { return isKicked; }
public void setIsKicked(boolean _isKicked) { isKicked = _isKicked; }


public final int GetBufSize() {
	int _size = 25;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 27;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) timeMS = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isKicked = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(guildId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, guildName);
	_buf.putLong(timeMS);
	_buf.put(isKicked?(byte)1:(byte)0);
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

