package GS2GC.p014_ChildOp;

import java.nio.ByteBuffer;
/*********
 * 子嗣（未成年）新增推送
 **/
public class GS2GC_014_050_OnChildAdd implements ALBasicProtocolPack._IALProtocolStructure {
/** 空 */
private Common.ChildObj.Child_Info child;


public GS2GC_014_050_OnChildAdd() {
	child = new Common.ChildObj.Child_Info();
}

public GS2GC_014_050_OnChildAdd(
	 Common.ChildObj.Child_Info _child
) {	child = _child;
}

public final byte getMainOrder() { return (byte)14; }

public final byte getSubOrder() { return (byte)50; }

/** 空 */
public Common.ChildObj.Child_Info getChild() { return child; }
/** 空 */
public void setChild(Common.ChildObj.Child_Info _child) { child = _child; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + child.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + child.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _childCustLen = _buf.getInt();
	int _childCurPos = _buf.position();
	child.ReadUnzipBuf(_buf, _childCurPos + _childCustLen);
	_buf.position(_childCurPos + _childCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(child.GetBufSize());
	child.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)50);
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

