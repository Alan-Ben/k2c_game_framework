using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_027_RetPrivilegeCardInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 权益卡数据列表
/// </summary>
private List<Common.PrivilegeCardObj.PrivilegeCardObj_Info> cardList;


public GS2GC_002_027_RetPrivilegeCardInit() {
	cardList = new List<Common.PrivilegeCardObj.PrivilegeCardObj_Info>();
}

public GS2GC_002_027_RetPrivilegeCardInit(
	List<Common.PrivilegeCardObj.PrivilegeCardObj_Info> _cardList
) {	cardList = _cardList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)27; }

/// <summary>
/// 权益卡数据列表
/// </summary>
public List<Common.PrivilegeCardObj.PrivilegeCardObj_Info> getCardList() { return cardList; }
/// <summary>
/// 权益卡数据列表
/// </summary>
public void addCardList(Common.PrivilegeCardObj.PrivilegeCardObj_Info _cardList) { cardList.Add(_cardList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (cardList.Count * 32);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (cardList.Count * 32);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _cardListCount = _buf.getShort();
	for(int _i = 0; _i < _cardListCount; _i++) { 
		Common.PrivilegeCardObj.PrivilegeCardObj_Info _cardList = new Common.PrivilegeCardObj.PrivilegeCardObj_Info();
		int __cardListCustLen = _buf.getInt();
	int __cardListCurPos = _buf.getCurPos();
	_cardList.ReadUnzipBuf(_buf, __cardListCurPos + __cardListCustLen);
	_buf.setPosition(__cardListCurPos + __cardListCustLen);

		cardList.Add(_cardList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)cardList.Count);
	for(int _i = 0; _i < cardList.Count; _i++) { 
		_buf.putInt(cardList[_i].GetBufSize());
	cardList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)27);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)27);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("cardList").Append(":").Append(cardList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

