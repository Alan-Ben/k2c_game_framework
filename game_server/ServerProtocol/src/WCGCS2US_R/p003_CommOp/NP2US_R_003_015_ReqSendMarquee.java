package WCGCS2US_R.p003_CommOp;

import java.nio.ByteBuffer;
/*********
 * 发送跑马灯
 **/
public class NP2US_R_003_015_ReqSendMarquee implements ALBasicProtocolPack._IALProtocolStructure {
/** 跑马灯数据 */
private Common.ServerObj.ServerObj_PHPMarquee marquee;


public NP2US_R_003_015_ReqSendMarquee() {
	marquee = new Common.ServerObj.ServerObj_PHPMarquee();
}

public NP2US_R_003_015_ReqSendMarquee(
	 Common.ServerObj.ServerObj_PHPMarquee _marquee
) {	marquee = _marquee;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)15; }

/** 跑马灯数据 */
public Common.ServerObj.ServerObj_PHPMarquee getMarquee() { return marquee; }
/** 跑马灯数据 */
public void setMarquee(Common.ServerObj.ServerObj_PHPMarquee _marquee) { marquee = _marquee; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + marquee.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + marquee.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _marqueeCustLen = _buf.getInt();
	int _marqueeCurPos = _buf.position();
	marquee.ReadUnzipBuf(_buf, _marqueeCurPos + _marqueeCustLen);
	_buf.position(_marqueeCurPos + _marqueeCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(marquee.GetBufSize());
	marquee.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)15);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)15);
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

