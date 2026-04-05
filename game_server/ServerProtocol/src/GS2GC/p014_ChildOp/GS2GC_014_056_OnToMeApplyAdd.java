package GS2GC.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 对自身指定请求新增推送
 **/
public class GS2GC_014_056_OnToMeApplyAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private Common.ChildObj.Adult_ToMeApplyBaseInfo toMeApply;


public GS2GC_014_056_OnToMeApplyAdd() {
	toMeApply = new Common.ChildObj.Adult_ToMeApplyBaseInfo();
}

public GS2GC_014_056_OnToMeApplyAdd(
	 Common.ChildObj.Adult_ToMeApplyBaseInfo _toMeApply
) {	toMeApply = _toMeApply;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)56; }

/** 空 */
public Common.ChildObj.Adult_ToMeApplyBaseInfo getToMeApply() { return toMeApply; }
/** 空 */
public void setToMeApply(Common.ChildObj.Adult_ToMeApplyBaseInfo _toMeApply) { toMeApply = _toMeApply; }


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
	if(_buf.remaining() <= 0) return;
	int _toMeApplyCustLen = _buf.getInt();
	int _toMeApplyCurPos = _buf.position();
	toMeApply.ReadUnzipBuf(_buf, _toMeApplyCurPos + _toMeApplyCustLen);
	_buf.position(_toMeApplyCurPos + _toMeApplyCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(toMeApply.GetBufSize());
	toMeApply.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)56);
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

