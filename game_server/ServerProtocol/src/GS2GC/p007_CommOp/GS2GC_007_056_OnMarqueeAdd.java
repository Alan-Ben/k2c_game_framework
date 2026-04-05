package GS2GC.p007_CommOp;

import java.nio.ByteBuffer;
public class GS2GC_007_056_OnMarqueeAdd implements ALBasicProtocolPack._IALProtocolStructure {
private int showPosId;
private Common.Common_MarqueeInfo marqueeInfo;


public GS2GC_007_056_OnMarqueeAdd() {
	showPosId = 0;
	marqueeInfo = new Common.Common_MarqueeInfo();
}

public GS2GC_007_056_OnMarqueeAdd(
	 int _showPosId
	, Common.Common_MarqueeInfo _marqueeInfo
) {	showPosId = _showPosId;
	marqueeInfo = _marqueeInfo;
}

public final byte getMainOrder() { return (byte)7; }

public final byte getSubOrder() { return (byte)56; }

public int getShowPosId() { return showPosId; }
public void setShowPosId(int _showPosId) { showPosId = _showPosId; }
public Common.Common_MarqueeInfo getMarqueeInfo() { return marqueeInfo; }
public void setMarqueeInfo(Common.Common_MarqueeInfo _marqueeInfo) { marqueeInfo = _marqueeInfo; }


public final int GetBufSize() {
	int _size = 4;
	_size += 4 + marqueeInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + marqueeInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) showPosId = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _marqueeInfoCustLen = _buf.getInt();
	int _marqueeInfoCurPos = _buf.position();
	marqueeInfo.ReadUnzipBuf(_buf, _marqueeInfoCurPos + _marqueeInfoCustLen);
	_buf.position(_marqueeInfoCurPos + _marqueeInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(showPosId);
	_buf.putInt(marqueeInfo.GetBufSize());
	marqueeInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
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

