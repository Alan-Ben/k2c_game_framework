using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p019_DinnerOp
{

/// <summary>
/// 宴会交互记录
/// </summary>
public class GS2GC_019_003_RetJoinedPlayerList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 赴宴玩家记录
/// </summary>
private List<Common.DinnerObj.Dinner_JoinerLogList> joinerList;


public GS2GC_019_003_RetJoinedPlayerList() {
	joinerList = new List<Common.DinnerObj.Dinner_JoinerLogList>();
}

public GS2GC_019_003_RetJoinedPlayerList(
	List<Common.DinnerObj.Dinner_JoinerLogList> _joinerList
) {	joinerList = _joinerList;
}

public byte getMainOrder() { return (byte)19; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 赴宴玩家记录
/// </summary>
public List<Common.DinnerObj.Dinner_JoinerLogList> getJoinerList() { return joinerList; }
/// <summary>
/// 赴宴玩家记录
/// </summary>
public void addJoinerList(Common.DinnerObj.Dinner_JoinerLogList _joinerList) { joinerList.Add(_joinerList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (joinerList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (joinerList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _joinerListCount = _buf.getShort();
	for(int _i = 0; _i < _joinerListCount; _i++) { 
		Common.DinnerObj.Dinner_JoinerLogList _joinerList = new Common.DinnerObj.Dinner_JoinerLogList();
		int __joinerListCustLen = _buf.getInt();
	int __joinerListCurPos = _buf.getCurPos();
	_joinerList.ReadUnzipBuf(_buf, __joinerListCurPos + __joinerListCustLen);
	_buf.setPosition(__joinerListCurPos + __joinerListCustLen);

		joinerList.Add(_joinerList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)joinerList.Count);
	for(int _i = 0; _i < joinerList.Count; _i++) { 
		_buf.putInt(joinerList[_i].GetBufSize());
	joinerList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)3);
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
	builder.Append("joinerList").Append(":").Append(joinerList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

