using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p034_InnOp
{

/// <summary>
/// 旅店接待信息变更
/// </summary>
public class GS2GC_034_050_OnInnReceiveChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 接待信息
/// </summary>
private Common.InnObj.Inn_ReceiveList receiveList;


public GS2GC_034_050_OnInnReceiveChg() {
	receiveList = new Common.InnObj.Inn_ReceiveList();
}

public GS2GC_034_050_OnInnReceiveChg(
	Common.InnObj.Inn_ReceiveList _receiveList
) {	receiveList = _receiveList;
}

public byte getMainOrder() { return (byte)34; }

public byte getSubOrder() { return (byte)50; }

/// <summary>
/// 接待信息
/// </summary>
public Common.InnObj.Inn_ReceiveList getReceiveList() { return receiveList; }
/// <summary>
/// 接待信息
/// </summary>
public void setReceiveList(Common.InnObj.Inn_ReceiveList _receiveList) { receiveList = _receiveList; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + receiveList.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + receiveList.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _receiveListCustLen = _buf.getInt();
	int _receiveListCurPos = _buf.getCurPos();
	receiveList.ReadUnzipBuf(_buf, _receiveListCurPos + _receiveListCustLen);
	_buf.setPosition(_receiveListCurPos + _receiveListCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(receiveList.GetBufSize());
	receiveList.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)50);
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
	builder.Append("receiveList").Append(":").Append(receiveList == null ? "null" : receiveList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

