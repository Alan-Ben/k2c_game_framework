package GS2GC.p006_BagItemOp;

import java.nio.ByteBuffer;
public class GS2GC_006_050_PushBagItemInfo implements ALBasicProtocolPack._IALProtocolStructure {
private NPCommon.NPCommon_BagItemInfo bagItemInfo;


public GS2GC_006_050_PushBagItemInfo() {
	bagItemInfo = new NPCommon.NPCommon_BagItemInfo();
}

public GS2GC_006_050_PushBagItemInfo(
	 NPCommon.NPCommon_BagItemInfo _bagItemInfo
) {	bagItemInfo = _bagItemInfo;
}

public final byte getMainOrder() { return (byte)6; }

public final byte getSubOrder() { return (byte)50; }

public NPCommon.NPCommon_BagItemInfo getBagItemInfo() { return bagItemInfo; }
public void setBagItemInfo(NPCommon.NPCommon_BagItemInfo _bagItemInfo) { bagItemInfo = _bagItemInfo; }


public final int GetBufSize() {
	int _size = 32;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _bagItemInfoCustLen = _buf.getInt();
	int _bagItemInfoCurPos = _buf.position();
	bagItemInfo.ReadUnzipBuf(_buf, _bagItemInfoCurPos + _bagItemInfoCustLen);
	_buf.position(_bagItemInfoCurPos + _bagItemInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(bagItemInfo.GetBufSize());
	bagItemInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)6);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)6);
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

