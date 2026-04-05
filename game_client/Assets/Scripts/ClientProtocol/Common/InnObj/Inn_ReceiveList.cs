using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.InnObj
{

/// <summary>
/// 旅店_接待队列列表
/// </summary>
public class Inn_ReceiveList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已接待数量
/// </summary>
private long hadBeenReceiveNum;
/// <summary>
/// 接待队列列表
/// </summary>
private List<Common.InnObj.Inn_ReceiveInfo> receiveList;


public Inn_ReceiveList() {
	hadBeenReceiveNum = (long)0;
	receiveList = new List<Common.InnObj.Inn_ReceiveInfo>();
}

public Inn_ReceiveList(
	long _hadBeenReceiveNum
	, List<Common.InnObj.Inn_ReceiveInfo> _receiveList
) {	hadBeenReceiveNum = _hadBeenReceiveNum;
	receiveList = _receiveList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 已接待数量
/// </summary>
public long getHadBeenReceiveNum() { return hadBeenReceiveNum; }
/// <summary>
/// 已接待数量
/// </summary>
public void setHadBeenReceiveNum(long _hadBeenReceiveNum) { hadBeenReceiveNum = _hadBeenReceiveNum; }
/// <summary>
/// 接待队列列表
/// </summary>
public List<Common.InnObj.Inn_ReceiveInfo> getReceiveList() { return receiveList; }
/// <summary>
/// 接待队列列表
/// </summary>
public void addReceiveList(Common.InnObj.Inn_ReceiveInfo _receiveList) { receiveList.Add(_receiveList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (receiveList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (receiveList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hadBeenReceiveNum = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _receiveListCount = _buf.getShort();
	for(int _i = 0; _i < _receiveListCount; _i++) { 
		Common.InnObj.Inn_ReceiveInfo _receiveList = new Common.InnObj.Inn_ReceiveInfo();
		int __receiveListCustLen = _buf.getInt();
	int __receiveListCurPos = _buf.getCurPos();
	_receiveList.ReadUnzipBuf(_buf, __receiveListCurPos + __receiveListCustLen);
	_buf.setPosition(__receiveListCurPos + __receiveListCustLen);

		receiveList.Add(_receiveList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(hadBeenReceiveNum);
	_buf.putShort((short)receiveList.Count);
	for(int _i = 0; _i < receiveList.Count; _i++) { 
		_buf.putInt(receiveList[_i].GetBufSize());
	receiveList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
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
	builder.Append("hadBeenReceiveNum").Append(":").Append(hadBeenReceiveNum.ToString()).Append(", ");
	builder.Append("receiveList").Append(":").Append(receiveList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

