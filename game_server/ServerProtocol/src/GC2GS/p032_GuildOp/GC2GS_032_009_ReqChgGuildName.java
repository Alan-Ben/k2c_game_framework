package GC2GS.p032_GuildOp;

import java.nio.ByteBuffer;
/*********
 * 请求变更联盟名称
 **/
public class GC2GS_032_009_ReqChgGuildName implements ALBasicProtocolPack._IALProtocolStructure {
private String name;
private String simpleName;


public GC2GS_032_009_ReqChgGuildName() {
	name = "";
	simpleName = "";
}

public GC2GS_032_009_ReqChgGuildName(
	 String _name
	, String _simpleName
) {	name = _name;
	simpleName = _simpleName;
}

public final byte getMainOrder() { return (byte)32; }

public final byte getSubOrder() { return (byte)9; }

public String getName() { return name; }
public void setName(String _name) { name = _name; }
public String getSimpleName() { return simpleName; }
public void setSimpleName(String _simpleName) { simpleName = _simpleName; }


public final int GetBufSize() {
	int _size = 0;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(simpleName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) simpleName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, simpleName);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)9);
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

