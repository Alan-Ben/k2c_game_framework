package GS2GC.p032_GuildOp;

import java.nio.ByteBuffer;
public class GS2GC_032_065_OnJoinGuild implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否是创建联盟 */
private boolean isCreate;
private Common.GuildObj.Guild_DetailInfo guildInfo;


public GS2GC_032_065_OnJoinGuild() {
	isCreate = false;
	guildInfo = new Common.GuildObj.Guild_DetailInfo();
}

public GS2GC_032_065_OnJoinGuild(
	 boolean _isCreate
	, Common.GuildObj.Guild_DetailInfo _guildInfo
) {	isCreate = _isCreate;
	guildInfo = _guildInfo;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)65; }

/** 是否是创建联盟 */
public boolean getIsCreate() { return isCreate; }
/** 是否是创建联盟 */
public void setIsCreate(boolean _isCreate) { isCreate = _isCreate; }
public Common.GuildObj.Guild_DetailInfo getGuildInfo() { return guildInfo; }
public void setGuildInfo(Common.GuildObj.Guild_DetailInfo _guildInfo) { guildInfo = _guildInfo; }


public final int GetBufSize() {
	int _size = 1;
	_size += 4 + guildInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;
	_size += 4 + guildInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isCreate = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _guildInfoCustLen = _buf.getInt();
	int _guildInfoCurPos = _buf.position();
	guildInfo.ReadUnzipBuf(_buf, _guildInfoCurPos + _guildInfoCustLen);
	_buf.position(_guildInfoCurPos + _guildInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isCreate?(byte)1:(byte)0);
	_buf.putInt(guildInfo.GetBufSize());
	guildInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)65);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)65);
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

