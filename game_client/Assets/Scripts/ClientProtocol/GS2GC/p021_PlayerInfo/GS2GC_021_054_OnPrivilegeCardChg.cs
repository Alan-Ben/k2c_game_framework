using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p021_PlayerInfo
{

public class GS2GC_021_054_OnPrivilegeCardChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 权益卡数据
/// </summary>
private Common.PrivilegeCardObj.PrivilegeCardObj_Info card;


public GS2GC_021_054_OnPrivilegeCardChg() {
	card = new Common.PrivilegeCardObj.PrivilegeCardObj_Info();
}

public GS2GC_021_054_OnPrivilegeCardChg(
	Common.PrivilegeCardObj.PrivilegeCardObj_Info _card
) {	card = _card;
}

public byte getMainOrder() { return (byte)21; }

public byte getSubOrder() { return (byte)54; }

/// <summary>
/// 权益卡数据
/// </summary>
public Common.PrivilegeCardObj.PrivilegeCardObj_Info getCard() { return card; }
/// <summary>
/// 权益卡数据
/// </summary>
public void setCard(Common.PrivilegeCardObj.PrivilegeCardObj_Info _card) { card = _card; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _cardCustLen = _buf.getInt();
	int _cardCurPos = _buf.getCurPos();
	card.ReadUnzipBuf(_buf, _cardCurPos + _cardCustLen);
	_buf.setPosition(_cardCurPos + _cardCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(card.GetBufSize());
	card.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)54);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)54);
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
	builder.Append("card").Append(":").Append(card == null ? "null" : card.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

