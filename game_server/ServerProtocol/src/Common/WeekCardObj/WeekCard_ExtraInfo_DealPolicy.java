package Common.WeekCardObj;

import java.nio.ByteBuffer;
/*********
 * 周卡-额外设置-懒汉策略
 **/
public class WeekCard_ExtraInfo_DealPolicy implements ALBasicProtocolPack._IALProtocolStructure {
/** 是否懒汉 即只处理超过上限的 */
private boolean isLazy;


public WeekCard_ExtraInfo_DealPolicy() {
	isLazy = false;
}

public WeekCard_ExtraInfo_DealPolicy(
	 boolean _isLazy
) {	isLazy = _isLazy;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 是否懒汉 即只处理超过上限的 */
public boolean getIsLazy() { return isLazy; }
/** 是否懒汉 即只处理超过上限的 */
public void setIsLazy(boolean _isLazy) { isLazy = _isLazy; }


public final int GetBufSize() {
	int _size = 1;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 3;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isLazy = (_buf.get() != 0);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isLazy?(byte)1:(byte)0);
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

