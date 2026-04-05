package GS2GC.p008_TravelOp;

import java.nio.ByteBuffer;
/*********
 * 处理增加妃子好感度游历事件
 **/
public class GS2GC_008_008_RetDealConsortLikeTravel implements ALBasicProtocolPack._IALProtocolStructure {
/** 游历事件结果 */
private Common.TravelObj.Travel_EventResult result;


public GS2GC_008_008_RetDealConsortLikeTravel() {
	result = new Common.TravelObj.Travel_EventResult();
}

public GS2GC_008_008_RetDealConsortLikeTravel(
	 Common.TravelObj.Travel_EventResult _result
) {	result = _result;
}

public final byte getMainOrder() { return (byte)8; }

public final byte getSubOrder() { return (byte)8; }

/** 游历事件结果 */
public Common.TravelObj.Travel_EventResult getResult() { return result; }
/** 游历事件结果 */
public void setResult(Common.TravelObj.Travel_EventResult _result) { result = _result; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + result.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + result.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _resultCustLen = _buf.getInt();
	int _resultCurPos = _buf.position();
	result.ReadUnzipBuf(_buf, _resultCurPos + _resultCustLen);
	_buf.position(_resultCurPos + _resultCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(result.GetBufSize());
	result.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
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

