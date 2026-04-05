package GS2GC.p040_MarsPeopleOp;

import java.nio.ByteBuffer;
/*********
 * 满意度变化
 **/
public class GS2GC_040_052_OnSatisfactionChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 满意度万分比 */
private int satisfaction;


public GS2GC_040_052_OnSatisfactionChg() {
	satisfaction = 0;
}

public GS2GC_040_052_OnSatisfactionChg(
	 int _satisfaction
) {	satisfaction = _satisfaction;
}

public final byte getMainOrder() { return (byte)40; }

public final byte getSubOrder() { return (byte)52; }

/** 满意度万分比 */
public int getSatisfaction() { return satisfaction; }
/** 满意度万分比 */
public void setSatisfaction(int _satisfaction) { satisfaction = _satisfaction; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) satisfaction = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(satisfaction);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)40);
	_buf.put((byte)52);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)40);
	_recBuf.put((byte)52);
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

