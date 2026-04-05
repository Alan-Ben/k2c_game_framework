package GS2GC.p040_MarsPeopleOp;

import java.nio.ByteBuffer;
/*********
 * 决策数据变化
 **/
public class GS2GC_040_051_OnIntelligentChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 决策数据 */
private Common.MarsObj.Mars_Intelligent intelligent;


public GS2GC_040_051_OnIntelligentChg() {
	intelligent = new Common.MarsObj.Mars_Intelligent();
}

public GS2GC_040_051_OnIntelligentChg(
	 Common.MarsObj.Mars_Intelligent _intelligent
) {	intelligent = _intelligent;
}

public final byte getMainOrder() { return (byte)40; }

public final byte getSubOrder() { return (byte)51; }

/** 决策数据 */
public Common.MarsObj.Mars_Intelligent getIntelligent() { return intelligent; }
/** 决策数据 */
public void setIntelligent(Common.MarsObj.Mars_Intelligent _intelligent) { intelligent = _intelligent; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _intelligentCustLen = _buf.getInt();
	int _intelligentCurPos = _buf.position();
	intelligent.ReadUnzipBuf(_buf, _intelligentCurPos + _intelligentCustLen);
	_buf.position(_intelligentCurPos + _intelligentCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(intelligent.GetBufSize());
	intelligent.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)40);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)40);
	_recBuf.put((byte)51);
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

