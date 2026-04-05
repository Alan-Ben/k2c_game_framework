package GC2GS.p021_PlayerInfo;

import java.nio.ByteBuffer;
/*********
 * 领取权益卡每日奖励
 **/
public class GC2GS_021_004_ReqGainPrivilegeCardDailyReward implements ALBasicProtocolPack._IALProtocolStructure {
/** 权益卡类型 */
private Common.PrivilegeCardEnum.EPrivilegeCardType cardType;


public GC2GS_021_004_ReqGainPrivilegeCardDailyReward() {
	cardType = Common.PrivilegeCardEnum.EPrivilegeCardType.values()[0];
}

public GC2GS_021_004_ReqGainPrivilegeCardDailyReward(
	 Common.PrivilegeCardEnum.EPrivilegeCardType _cardType
) {	cardType = _cardType;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)4; }

/** 权益卡类型 */
public Common.PrivilegeCardEnum.EPrivilegeCardType getCardType() { return cardType; }
/** 权益卡类型 */
public void setCardType(Common.PrivilegeCardEnum.EPrivilegeCardType _cardType) { cardType = _cardType; }


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
	if(_buf.remaining() > 0) cardType = Common.PrivilegeCardEnum.EPrivilegeCardType.EPrivilegeCardType_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(cardType.ordinal());

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)4);
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

