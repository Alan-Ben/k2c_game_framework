package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
public class GS2GC_002_027_RetPrivilegeCardInit implements ALBasicProtocolPack._IALProtocolStructure {
/** 权益卡数据列表 */
private java.util.ArrayList<Common.PrivilegeCardObj.PrivilegeCardObj_Info> cardList;


public GS2GC_002_027_RetPrivilegeCardInit() {
	cardList = new java.util.ArrayList<Common.PrivilegeCardObj.PrivilegeCardObj_Info>();
}

public GS2GC_002_027_RetPrivilegeCardInit(
	 java.util.ArrayList<Common.PrivilegeCardObj.PrivilegeCardObj_Info> _cardList
) {	cardList = _cardList;
}

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)27; }

/** 权益卡数据列表 */
public java.util.ArrayList<Common.PrivilegeCardObj.PrivilegeCardObj_Info> getCardList() { return cardList; }
/** 权益卡数据列表 */
public void addCardList(Common.PrivilegeCardObj.PrivilegeCardObj_Info _cardList) { cardList.add(_cardList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (cardList.size() * 32);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (cardList.size() * 32);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _cardListCount = _buf.getShort();
	for(int _i = 0; _i < _cardListCount; _i++) { 
		Common.PrivilegeCardObj.PrivilegeCardObj_Info _cardList = new Common.PrivilegeCardObj.PrivilegeCardObj_Info();
		if(_buf.remaining() <= 0) return;
	int __cardListCustLen = _buf.getInt();
	int __cardListCurPos = _buf.position();
	_cardList.ReadUnzipBuf(_buf, __cardListCurPos + __cardListCustLen);
	_buf.position(__cardListCurPos + __cardListCustLen);

		cardList.add(_cardList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)cardList.size());
	for(int _i = 0; _i < cardList.size(); _i++) { 
		_buf.putInt(cardList.get(_i).GetBufSize());
	cardList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)27);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)27);
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

