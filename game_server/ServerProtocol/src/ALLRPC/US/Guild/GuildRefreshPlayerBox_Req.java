package ALLRPC.US.Guild;

import java.nio.ByteBuffer;
public class GuildRefreshPlayerBox_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private long guildId;
private Common.GuildEnum.EGuildBoxType boxType;
private int needCount;


public GuildRefreshPlayerBox_Req() {
	cid = (long)0;
	guildId = (long)0;
	boxType = Common.GuildEnum.EGuildBoxType.values()[0];
	needCount = 0;
}

public GuildRefreshPlayerBox_Req(
	 long _cid
	, long _guildId
	, Common.GuildEnum.EGuildBoxType _boxType
	, int _needCount
) {	cid = _cid;
	guildId = _guildId;
	boxType = _boxType;
	needCount = _needCount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getGuildId() { return guildId; }
public void setGuildId(long _guildId) { guildId = _guildId; }
public Common.GuildEnum.EGuildBoxType getBoxType() { return boxType; }
public void setBoxType(Common.GuildEnum.EGuildBoxType _boxType) { boxType = _boxType; }
public int getNeedCount() { return needCount; }
public void setNeedCount(int _needCount) { needCount = _needCount; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) boxType = Common.GuildEnum.EGuildBoxType.EGuildBoxType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) needCount = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(guildId);
	_buf.putInt(boxType.ordinal());

	_buf.putInt(needCount);
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

