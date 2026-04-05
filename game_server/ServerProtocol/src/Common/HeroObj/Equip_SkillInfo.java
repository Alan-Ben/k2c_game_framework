package Common.HeroObj;

import java.nio.ByteBuffer;
/*********
 * 藏品技能信息
 **/
public class Equip_SkillInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 技能索引 */
private int index;
/** 当前加成值 */
private int value;
/** 上次重塑的加成值 */
private int pendingValue;
/** 普通重塑次数 */
private int normalRebuildNum;


public Equip_SkillInfo() {
	index = 0;
	value = 0;
	pendingValue = 0;
	normalRebuildNum = 0;
}

public Equip_SkillInfo(
	 int _index
	, int _value
	, int _pendingValue
	, int _normalRebuildNum
) {	index = _index;
	value = _value;
	pendingValue = _pendingValue;
	normalRebuildNum = _normalRebuildNum;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 技能索引 */
public int getIndex() { return index; }
/** 技能索引 */
public void setIndex(int _index) { index = _index; }
/** 当前加成值 */
public int getValue() { return value; }
/** 当前加成值 */
public void setValue(int _value) { value = _value; }
/** 上次重塑的加成值 */
public int getPendingValue() { return pendingValue; }
/** 上次重塑的加成值 */
public void setPendingValue(int _pendingValue) { pendingValue = _pendingValue; }
/** 普通重塑次数 */
public int getNormalRebuildNum() { return normalRebuildNum; }
/** 普通重塑次数 */
public void setNormalRebuildNum(int _normalRebuildNum) { normalRebuildNum = _normalRebuildNum; }


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
	if(_buf.remaining() > 0) index = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) value = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pendingValue = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) normalRebuildNum = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(index);
	_buf.putInt(value);
	_buf.putInt(pendingValue);
	_buf.putInt(normalRebuildNum);
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

