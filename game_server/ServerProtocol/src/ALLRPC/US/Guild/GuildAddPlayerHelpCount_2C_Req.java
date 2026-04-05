package ALLRPC.US.Guild;

import java.nio.ByteBuffer;
public class GuildAddPlayerHelpCount_2C_Req implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
private long guildId;
private byte[] helpSucData;


public GuildAddPlayerHelpCount_2C_Req() {
	cid = (long)0;
	guildId = (long)0;
	helpSucData = null;
}

public GuildAddPlayerHelpCount_2C_Req(
	 long _cid
	, long _guildId
	, byte[] _helpSucData
) {	cid = _cid;
	guildId = _guildId;
	helpSucData = _helpSucData;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
public long getGuildId() { return guildId; }
public void setGuildId(long _guildId) { guildId = _guildId; }
public byte[] getHelpSucData() { return helpSucData; }
public java.nio.ByteBuffer get_buffer_HelpSucData() { if(null == helpSucData)return null; else return ByteBuffer.wrap(helpSucData); }

public void setHelpSucData(byte[] _helpSucData) { helpSucData = _helpSucData; }
public void setHelpSucData(java.nio.ByteBuffer _helpSucData) 
{
	if(null == _helpSucData){return;}
	int _oldPos = _helpSucData.position();
	int _bufLength = _helpSucData.remaining();
	helpSucData = new byte[_bufLength];
	_helpSucData.get(helpSucData);
	_helpSucData.position(_oldPos);
}



public final int GetBufSize() {
	int _size = 16;
	_size += 4 + (helpSucData == null ? 0 : helpSucData.length);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 4 + (helpSucData == null ? 0 : helpSucData.length);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _helpSucDataCount = _buf.getInt();
	if(0 < _helpSucDataCount){
		helpSucData = new byte[_helpSucDataCount];
		_buf.get(helpSucData);
	}

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(guildId);
	_buf.putInt((helpSucData == null ? 0 : helpSucData.length));
	if(null != helpSucData){_buf.put(helpSucData);}

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

