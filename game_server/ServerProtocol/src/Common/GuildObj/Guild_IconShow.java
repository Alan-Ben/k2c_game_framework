package Common.GuildObj;

import java.nio.ByteBuffer;
/*********
 * 联盟最简展示信息
 **/
public class Guild_IconShow implements ALBasicProtocolPack._IALProtocolStructure {
private long guidId;
private long flagId;
private String name;


public Guild_IconShow() {
	guidId = (long)0;
	flagId = (long)0;
	name = "";
}

public Guild_IconShow(
	 long _guidId
	, long _flagId
	, String _name
) {	guidId = _guidId;
	flagId = _flagId;
	name = _name;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getGuidId() { return guidId; }
public void setGuidId(long _guidId) { guidId = _guidId; }
public long getFlagId() { return flagId; }
public void setFlagId(long _flagId) { flagId = _flagId; }
public String getName() { return name; }
public void setName(String _name) { name = _name; }


public final int GetBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) guidId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) flagId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(guidId);
	_buf.putLong(flagId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
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

