package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_053_RetLoginCountInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 已经领取的奖励天数 */
private java.util.ArrayList<Integer> hadDrawRewardDays;


public GS2GC_002_053_RetLoginCountInit() {
	hadDrawRewardDays = new java.util.ArrayList<Integer>();
}

public GS2GC_002_053_RetLoginCountInit(
	 java.util.ArrayList<Integer> _hadDrawRewardDays
) {	hadDrawRewardDays = _hadDrawRewardDays;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)53; }

/** 已经领取的奖励天数 */
public java.util.ArrayList<Integer> getHadDrawRewardDays() { return hadDrawRewardDays; }
/** 已经领取的奖励天数 */
public void addHadDrawRewardDays(int _hadDrawRewardDays) { hadDrawRewardDays.add(_hadDrawRewardDays); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (hadDrawRewardDays.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (hadDrawRewardDays.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadDrawRewardDaysCount = _buf.getShort();
	for(int _i = 0; _i < _hadDrawRewardDaysCount; _i++) { 
		int _hadDrawRewardDays = 0;
		if(_buf.remaining() > 0) _hadDrawRewardDays = _buf.getInt();
		hadDrawRewardDays.add(_hadDrawRewardDays);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)hadDrawRewardDays.size());
	for(int _i = 0; _i < hadDrawRewardDays.size(); _i++) { 
		_buf.putInt(hadDrawRewardDays.get(_i));
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)53);
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

