package GC2GS.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 设置子嗣（未成年）名称
 **/
public class GC2GS_014_001_ReqSetChildName implements ALBasicProtocolPack._IALProtocolStructure {
/** 子嗣（未成年）实例ID */
private long id;
/** 子嗣名称 */
private String name;


public GC2GS_014_001_ReqSetChildName() {
	id = (long)0;
	name = "";
}

public GC2GS_014_001_ReqSetChildName(
	 long _id
	, String _name
) {	id = _id;
	name = _name;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)1; }

/** 子嗣（未成年）实例ID */
public long getId() { return id; }
/** 子嗣（未成年）实例ID */
public void setId(long _id) { id = _id; }
/** 子嗣名称 */
public String getName() { return name; }
/** 子嗣名称 */
public void setName(String _name) { name = _name; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(name);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) name = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, name);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
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

