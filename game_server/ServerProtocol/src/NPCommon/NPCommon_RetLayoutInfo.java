package NPCommon;

import java.nio.ByteBuffer;
/*********
 * 服务端返回给客户端阵型数据
 **/
public class NPCommon_RetLayoutInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 宠物实例id */
private long petInstanceId;
/** 阵型位置id */
private long layoutIndex;
/** 血量万分比 */
private short hp;
/** 宠物能量值 */
private short sp;


public NPCommon_RetLayoutInfo() {
	petInstanceId = (long)0;
	layoutIndex = (long)0;
	hp = (short)0;
	sp = (short)0;
}

public NPCommon_RetLayoutInfo(
	 long _petInstanceId
	, long _layoutIndex
	, short _hp
	, short _sp
) {	petInstanceId = _petInstanceId;
	layoutIndex = _layoutIndex;
	hp = _hp;
	sp = _sp;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 宠物实例id */
public long getPetInstanceId() { return petInstanceId; }
/** 宠物实例id */
public void setPetInstanceId(long _petInstanceId) { petInstanceId = _petInstanceId; }
/** 阵型位置id */
public long getLayoutIndex() { return layoutIndex; }
/** 阵型位置id */
public void setLayoutIndex(long _layoutIndex) { layoutIndex = _layoutIndex; }
/** 血量万分比 */
public short getHp() { return hp; }
/** 血量万分比 */
public void setHp(short _hp) { hp = _hp; }
/** 宠物能量值 */
public short getSp() { return sp; }
/** 宠物能量值 */
public void setSp(short _sp) { sp = _sp; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) petInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) layoutIndex = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hp = _buf.getShort();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sp = _buf.getShort();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(petInstanceId);
	_buf.putLong(layoutIndex);
	_buf.putShort(hp);
	_buf.putShort(sp);
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

