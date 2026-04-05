package GS2GC.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 子嗣（未成年）等级变动推送
 **/
public class GS2GC_014_052_OnChildLvlChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 子嗣实例ID */
private long id;
/** 子嗣等级 */
private int lvl;


public GS2GC_014_052_OnChildLvlChg() {
	id = (long)0;
	lvl = 0;
}

public GS2GC_014_052_OnChildLvlChg(
	 long _id
	, int _lvl
) {	id = _id;
	lvl = _lvl;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)52; }

/** 子嗣实例ID */
public long getId() { return id; }
/** 子嗣实例ID */
public void setId(long _id) { id = _id; }
/** 子嗣等级 */
public int getLvl() { return lvl; }
/** 子嗣等级 */
public void setLvl(int _lvl) { lvl = _lvl; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lvl = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putInt(lvl);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)52);
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

