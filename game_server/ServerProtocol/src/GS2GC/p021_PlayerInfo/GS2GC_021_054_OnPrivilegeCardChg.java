package GS2GC.p021_PlayerInfo;

import java.nio.ByteBuffer;
public class GS2GC_021_054_OnPrivilegeCardChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 权益卡数据 */
private Common.PrivilegeCardObj.PrivilegeCardObj_Info card;


public GS2GC_021_054_OnPrivilegeCardChg() {
	card = new Common.PrivilegeCardObj.PrivilegeCardObj_Info();
}

public GS2GC_021_054_OnPrivilegeCardChg(
	 Common.PrivilegeCardObj.PrivilegeCardObj_Info _card
) {	card = _card;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)54; }

/** 权益卡数据 */
public Common.PrivilegeCardObj.PrivilegeCardObj_Info getCard() { return card; }
/** 权益卡数据 */
public void setCard(Common.PrivilegeCardObj.PrivilegeCardObj_Info _card) { card = _card; }


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
	int _cardCustLen = _buf.getInt();
	int _cardCurPos = _buf.position();
	card.ReadUnzipBuf(_buf, _cardCurPos + _cardCustLen);
	_buf.position(_cardCurPos + _cardCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(card.GetBufSize());
	card.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)54);
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

