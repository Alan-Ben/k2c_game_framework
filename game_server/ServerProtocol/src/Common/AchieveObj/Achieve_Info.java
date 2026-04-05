package Common.AchieveObj;

import java.nio.ByteBuffer;
/*********
 * 成就数据
 **/
public class Achieve_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 成就ID */
private long achieveId;
/** 成就步骤计数 */
private long counter;
/** 已领取成就步骤列表 */
private java.util.ArrayList<Integer> hadDrawStepList;


public Achieve_Info() {
	achieveId = (long)0;
	counter = (long)0;
	hadDrawStepList = new java.util.ArrayList<Integer>();
}

public Achieve_Info(
	 long _achieveId
	, long _counter
	, java.util.ArrayList<Integer> _hadDrawStepList
) {	achieveId = _achieveId;
	counter = _counter;
	hadDrawStepList = _hadDrawStepList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 成就ID */
public long getAchieveId() { return achieveId; }
/** 成就ID */
public void setAchieveId(long _achieveId) { achieveId = _achieveId; }
/** 成就步骤计数 */
public long getCounter() { return counter; }
/** 成就步骤计数 */
public void setCounter(long _counter) { counter = _counter; }
/** 已领取成就步骤列表 */
public java.util.ArrayList<Integer> getHadDrawStepList() { return hadDrawStepList; }
/** 已领取成就步骤列表 */
public void addHadDrawStepList(int _hadDrawStepList) { hadDrawStepList.add(_hadDrawStepList); }


public final int GetBufSize() {
	int _size = 16;
	_size += 2 + (hadDrawStepList.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2 + (hadDrawStepList.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) achieveId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) counter = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadDrawStepListCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawStepListCount; _i++) { 
		int _hadDrawStepList = 0;
		if(_buf.remaining() > 0) _hadDrawStepList = _buf.getInt();
		hadDrawStepList.add(_hadDrawStepList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(achieveId);
	_buf.putLong(counter);
	_buf.putShort((short)hadDrawStepList.size());
	for(int _i = 0; _i < hadDrawStepList.size(); _i++) { 
		_buf.putInt(hadDrawStepList.get(_i));
	}
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

