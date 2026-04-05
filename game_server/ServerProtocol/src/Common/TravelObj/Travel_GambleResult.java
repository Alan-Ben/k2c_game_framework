package Common.TravelObj;

import java.nio.ByteBuffer;
/*********
 * 博彩事件额外结果
 **/
public class Travel_GambleResult implements ALBasicProtocolPack._IALProtocolStructure {
/** 结果类型 */
private Common.TravelEnum.ETravelGambleResult resultType;
/** 钻石变化量（正获得负损失） */
private long diamondChange;
/** 下注金额 */
private long betAmount;


public Travel_GambleResult() {
	resultType = Common.TravelEnum.ETravelGambleResult.values()[0];
	diamondChange = (long)0;
	betAmount = (long)0;
}

public Travel_GambleResult(
	 Common.TravelEnum.ETravelGambleResult _resultType
	, long _diamondChange
	, long _betAmount
) {	resultType = _resultType;
	diamondChange = _diamondChange;
	betAmount = _betAmount;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 结果类型 */
public Common.TravelEnum.ETravelGambleResult getResultType() { return resultType; }
/** 结果类型 */
public void setResultType(Common.TravelEnum.ETravelGambleResult _resultType) { resultType = _resultType; }
/** 钻石变化量（正获得负损失） */
public long getDiamondChange() { return diamondChange; }
/** 钻石变化量（正获得负损失） */
public void setDiamondChange(long _diamondChange) { diamondChange = _diamondChange; }
/** 下注金额 */
public long getBetAmount() { return betAmount; }
/** 下注金额 */
public void setBetAmount(long _betAmount) { betAmount = _betAmount; }


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
	if(_buf.remaining() > 0) resultType = Common.TravelEnum.ETravelGambleResult.ETravelGambleResult_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) diamondChange = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) betAmount = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(resultType.ordinal());

	_buf.putLong(diamondChange);
	_buf.putLong(betAmount);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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

