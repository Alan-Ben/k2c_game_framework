using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p007_CommOp
{

public class GS2GC_007_021_RetTakeOfflineRewardList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 成功处理的列表
/// </summary>
private List<long> idList;


public GS2GC_007_021_RetTakeOfflineRewardList() {
	idList = new List<long>();
}

public GS2GC_007_021_RetTakeOfflineRewardList(
	List<long> _idList
) {	idList = _idList;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)21; }

/// <summary>
/// 成功处理的列表
/// </summary>
public List<long> getIdList() { return idList; }
/// <summary>
/// 成功处理的列表
/// </summary>
public void addIdList(long _idList) { idList.Add(_idList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (idList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (idList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _idListCount = _buf.getShort();
	for(int _i = 0; _i < _idListCount; _i++) { 
		long _idList = (long)0;
		_idList = _buf.getLong();
		idList.Add(_idList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)idList.Count);
	for(int _i = 0; _i < idList.Count; _i++) { 
		_buf.putLong(idList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)21);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)21);
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
	builder.Append("idList").Append(":").Append(idList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

