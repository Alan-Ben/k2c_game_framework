package Common.AchieveObj;

import java.nio.ByteBuffer;
/*********
 * 成就点数据
 **/
public class Achieve_AchievePointInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 成就点ID EAchieveType */
private int type;
/** 数量 */
private long count;
/** 已领取最大成就步骤 */
private int hadDrawMaxStep;


public Achieve_AchievePointInfo() {
	type = 0;
	count = (long)0;
	hadDrawMaxStep = 0;
}

public Achieve_AchievePointInfo(
	 int _type
	, long _count
	, int _hadDrawMaxStep
) {	type = _type;
	count = _count;
	hadDrawMaxStep = _hadDrawMaxStep;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 成就点ID EAchieveType */
public int getType() { return type; }
/** 成就点ID EAchieveType */
public void setType(int _type) { type = _type; }
/** 数量 */
public long getCount() { return count; }
/** 数量 */
public void setCount(long _count) { count = _count; }
/** 已领取最大成就步骤 */
public int getHadDrawMaxStep() { return hadDrawMaxStep; }
/** 已领取最大成就步骤 */
public void setHadDrawMaxStep(int _hadDrawMaxStep) { hadDrawMaxStep = _hadDrawMaxStep; }


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
	if(_buf.remaining() > 0) type = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) count = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) hadDrawMaxStep = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(type);
	_buf.putLong(count);
	_buf.putInt(hadDrawMaxStep);
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

