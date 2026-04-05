package GS2GC.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 获取对玩家的指定联姻请求
 **/
public class GS2GC_014_008_RetGetToMeApply implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private Common.ChildObj.Adult_ToMeApplyInfo apply;


public GS2GC_014_008_RetGetToMeApply() {
	apply = new Common.ChildObj.Adult_ToMeApplyInfo();
}

public GS2GC_014_008_RetGetToMeApply(
	 Common.ChildObj.Adult_ToMeApplyInfo _apply
) {	apply = _apply;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)8; }

/** 空 */
public Common.ChildObj.Adult_ToMeApplyInfo getApply() { return apply; }
/** 空 */
public void setApply(Common.ChildObj.Adult_ToMeApplyInfo _apply) { apply = _apply; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + apply.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + apply.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _applyCustLen = _buf.getInt();
	int _applyCurPos = _buf.position();
	apply.ReadUnzipBuf(_buf, _applyCurPos + _applyCustLen);
	_buf.position(_applyCurPos + _applyCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(apply.GetBufSize());
	apply.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)8);
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

