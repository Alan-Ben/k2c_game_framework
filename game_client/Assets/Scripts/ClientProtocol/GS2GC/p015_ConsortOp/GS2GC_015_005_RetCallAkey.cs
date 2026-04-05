using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 家人-一键邀约
/// </summary>
public class GS2GC_015_005_RetCallAkey : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 邀约结果列表
/// </summary>
private List<Common.ConsortObj.Consort_CallRes> resList;


public GS2GC_015_005_RetCallAkey() {
	resList = new List<Common.ConsortObj.Consort_CallRes>();
}

public GS2GC_015_005_RetCallAkey(
	List<Common.ConsortObj.Consort_CallRes> _resList
) {	resList = _resList;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)5; }

/// <summary>
/// 邀约结果列表
/// </summary>
public List<Common.ConsortObj.Consort_CallRes> getResList() { return resList; }
/// <summary>
/// 邀约结果列表
/// </summary>
public void addResList(Common.ConsortObj.Consort_CallRes _resList) { resList.Add(_resList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (resList.Count * 37);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (resList.Count * 37);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _resListCount = _buf.getShort();
	for(int _i = 0; _i < _resListCount; _i++) { 
		Common.ConsortObj.Consort_CallRes _resList = new Common.ConsortObj.Consort_CallRes();
		int __resListCustLen = _buf.getInt();
	int __resListCurPos = _buf.getCurPos();
	_resList.ReadUnzipBuf(_buf, __resListCurPos + __resListCustLen);
	_buf.setPosition(__resListCurPos + __resListCustLen);

		resList.Add(_resList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)resList.Count);
	for(int _i = 0; _i < resList.Count; _i++) { 
		_buf.putInt(resList[_i].GetBufSize());
	resList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)5);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)5);
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
	builder.Append("resList").Append(":").Append(resList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

