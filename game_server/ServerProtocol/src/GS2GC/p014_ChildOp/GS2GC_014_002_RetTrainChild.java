package GS2GC.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 训练子嗣（未成年）
 **/
public class GS2GC_014_002_RetTrainChild implements ALBasicProtocolPack._IALProtocolStructure {
/** 子嗣上课金币消耗 */
private long costValue;
/** 子嗣上课获得伙伴经验 */
private long gainValue;
/** 子嗣收益加成增加 */
private long addBonus;


public GS2GC_014_002_RetTrainChild() {
	costValue = (long)0;
	gainValue = (long)0;
	addBonus = (long)0;
}

public GS2GC_014_002_RetTrainChild(
	 long _costValue
	, long _gainValue
	, long _addBonus
) {	costValue = _costValue;
	gainValue = _gainValue;
	addBonus = _addBonus;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)2; }

/** 子嗣上课金币消耗 */
public long getCostValue() { return costValue; }
/** 子嗣上课金币消耗 */
public void setCostValue(long _costValue) { costValue = _costValue; }
/** 子嗣上课获得伙伴经验 */
public long getGainValue() { return gainValue; }
/** 子嗣上课获得伙伴经验 */
public void setGainValue(long _gainValue) { gainValue = _gainValue; }
/** 子嗣收益加成增加 */
public long getAddBonus() { return addBonus; }
/** 子嗣收益加成增加 */
public void setAddBonus(long _addBonus) { addBonus = _addBonus; }


public final int GetBufSize() {
	int _size = 24;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) costValue = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainValue = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) addBonus = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(costValue);
	_buf.putLong(gainValue);
	_buf.putLong(addBonus);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)2);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)2);
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

