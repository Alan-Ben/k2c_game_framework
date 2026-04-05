package Common.TravelObj;

import java.nio.ByteBuffer;
/*********
 * 游历事件结果-妃子亲密度额外数据
 **/
public class Travel_EventResultExt_ConsoleIntimacy implements ALBasicProtocolPack._IALProtocolStructure {
private long preIntimacy;


public Travel_EventResultExt_ConsoleIntimacy() {
	preIntimacy = (long)0;
}

public Travel_EventResultExt_ConsoleIntimacy(
	 long _preIntimacy
) {	preIntimacy = _preIntimacy;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getPreIntimacy() { return preIntimacy; }
public void setPreIntimacy(long _preIntimacy) { preIntimacy = _preIntimacy; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) preIntimacy = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(preIntimacy);
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

