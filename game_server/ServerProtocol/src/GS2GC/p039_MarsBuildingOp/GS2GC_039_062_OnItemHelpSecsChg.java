package GS2GC.p039_MarsBuildingOp;

import java.nio.ByteBuffer;
/*********
 * 火星加速道具加速时长数值变化
 **/
public class GS2GC_039_062_OnItemHelpSecsChg implements ALBasicProtocolPack._IALProtocolStructure {
private Common.MarsEnum.EMarsBagItemUseTimeType objType;
/** 对象ID */
private long objId;
/** 加速时长（秒） */
private int secs;


public GS2GC_039_062_OnItemHelpSecsChg() {
	objType = Common.MarsEnum.EMarsBagItemUseTimeType.values()[0];
	objId = (long)0;
	secs = 0;
}

public GS2GC_039_062_OnItemHelpSecsChg(
	 Common.MarsEnum.EMarsBagItemUseTimeType _objType
	, long _objId
	, int _secs
) {	objType = _objType;
	objId = _objId;
	secs = _secs;
}

public final byte getMainOrder() { return (byte)39; }

public final byte getSubOrder() { return (byte)62; }

public Common.MarsEnum.EMarsBagItemUseTimeType getObjType() { return objType; }
public void setObjType(Common.MarsEnum.EMarsBagItemUseTimeType _objType) { objType = _objType; }
/** 对象ID */
public long getObjId() { return objId; }
/** 对象ID */
public void setObjId(long _objId) { objId = _objId; }
/** 加速时长（秒） */
public int getSecs() { return secs; }
/** 加速时长（秒） */
public void setSecs(int _secs) { secs = _secs; }


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
	if(_buf.remaining() > 0) objType = Common.MarsEnum.EMarsBagItemUseTimeType.EMarsBagItemUseTimeType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) objId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) secs = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(objType.ordinal());

	_buf.putLong(objId);
	_buf.putInt(secs);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)39);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
	_recBuf.put((byte)62);
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

