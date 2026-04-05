package NPCommon;

import java.nio.ByteBuffer;
/*********
 * 设置阵型时客户端发给服务端的数据
 **/
public class NPCommon_layoutInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 宠物实例id */
private long petInstanceId;
/** 阵型里的坐标 */
private long layoutIndex;


public NPCommon_layoutInfo() {
	petInstanceId = (long)0;
	layoutIndex = (long)0;
}

public NPCommon_layoutInfo(
	 long _petInstanceId
	, long _layoutIndex
) {	petInstanceId = _petInstanceId;
	layoutIndex = _layoutIndex;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 宠物实例id */
public long getPetInstanceId() { return petInstanceId; }
/** 宠物实例id */
public void setPetInstanceId(long _petInstanceId) { petInstanceId = _petInstanceId; }
/** 阵型里的坐标 */
public long getLayoutIndex() { return layoutIndex; }
/** 阵型里的坐标 */
public void setLayoutIndex(long _layoutIndex) { layoutIndex = _layoutIndex; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) petInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) layoutIndex = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(petInstanceId);
	_buf.putLong(layoutIndex);
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

