package GC2GS.p007_CommOp;

import java.nio.ByteBuffer;
/*********
 * 红点-清除红点请求
 **/
public class GC2GS_007_031_ReqClearRedDot implements ALBasicProtocolPack._IALProtocolStructure {
/** 要清除的红点类型列表 */
private java.util.ArrayList<CommonEnum.ERedDotType> redDotTypes;


public GC2GS_007_031_ReqClearRedDot() {
	redDotTypes = new java.util.ArrayList<CommonEnum.ERedDotType>();
}

public GC2GS_007_031_ReqClearRedDot(
	 java.util.ArrayList<CommonEnum.ERedDotType> _redDotTypes
) {	redDotTypes = _redDotTypes;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)31; }

/** 要清除的红点类型列表 */
public java.util.ArrayList<CommonEnum.ERedDotType> getRedDotTypes() { return redDotTypes; }
/** 要清除的红点类型列表 */
public void addRedDotTypes(CommonEnum.ERedDotType _redDotTypes) { redDotTypes.add(_redDotTypes); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (redDotTypes.size() * 4);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (redDotTypes.size() * 4);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _redDotTypesCount = _buf.getShort();
	for(int _i = 0; _i < _redDotTypesCount; _i++) { 
		CommonEnum.ERedDotType _redDotTypes = CommonEnum.ERedDotType.values()[0];
		if(_buf.remaining() > 0) _redDotTypes = CommonEnum.ERedDotType.ERedDotType_FromInt(_buf.getInt());
		redDotTypes.add(_redDotTypes);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)redDotTypes.size());
	for(int _i = 0; _i < redDotTypes.size(); _i++) { 
		_buf.putInt(redDotTypes.get(_i).ordinal());

	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)31);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)31);
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

