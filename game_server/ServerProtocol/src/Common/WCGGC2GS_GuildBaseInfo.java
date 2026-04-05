package Common;

import java.nio.ByteBuffer;
public class WCGGC2GS_GuildBaseInfo implements ALBasicProtocolPack._IALProtocolStructure {
private String guildName;
private String manifesto;
private long guildIcon;
private long national;
private long joinGrades;
private int joinLimit;


public WCGGC2GS_GuildBaseInfo() {
	guildName = "";
	manifesto = "";
	guildIcon = (long)0;
	national = (long)0;
	joinGrades = (long)0;
	joinLimit = 0;
}

public WCGGC2GS_GuildBaseInfo(
	 String _guildName
	, String _manifesto
	, long _guildIcon
	, long _national
	, long _joinGrades
	, int _joinLimit
) {	guildName = _guildName;
	manifesto = _manifesto;
	guildIcon = _guildIcon;
	national = _national;
	joinGrades = _joinGrades;
	joinLimit = _joinLimit;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public String getGuildName() { return guildName; }
public void setGuildName(String _guildName) { guildName = _guildName; }
public String getManifesto() { return manifesto; }
public void setManifesto(String _manifesto) { manifesto = _manifesto; }
public long getGuildIcon() { return guildIcon; }
public void setGuildIcon(long _guildIcon) { guildIcon = _guildIcon; }
public long getNational() { return national; }
public void setNational(long _national) { national = _national; }
public long getJoinGrades() { return joinGrades; }
public void setJoinGrades(long _joinGrades) { joinGrades = _joinGrades; }
public int getJoinLimit() { return joinLimit; }
public void setJoinLimit(int _joinLimit) { joinLimit = _joinLimit; }


public final int GetBufSize() {
	int _size = 28;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(manifesto);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(guildName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(manifesto);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) manifesto = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guildIcon = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) national = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinGrades = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) joinLimit = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, guildName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, manifesto);
	_buf.putLong(guildIcon);
	_buf.putLong(national);
	_buf.putLong(joinGrades);
	_buf.putInt(joinLimit);
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

