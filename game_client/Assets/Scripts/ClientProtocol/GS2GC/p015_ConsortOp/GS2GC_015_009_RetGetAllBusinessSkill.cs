using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 家人-获取所有经营技能数据
/// </summary>
public class GS2GC_015_009_RetGetAllBusinessSkill : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 结果列表
/// </summary>
private List<Common.ConsortObj.Consort_BusinessSkill> dataList;


public GS2GC_015_009_RetGetAllBusinessSkill() {
	dataList = new List<Common.ConsortObj.Consort_BusinessSkill>();
}

public GS2GC_015_009_RetGetAllBusinessSkill(
	List<Common.ConsortObj.Consort_BusinessSkill> _dataList
) {	dataList = _dataList;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)9; }

/// <summary>
/// 结果列表
/// </summary>
public List<Common.ConsortObj.Consort_BusinessSkill> getDataList() { return dataList; }
/// <summary>
/// 结果列表
/// </summary>
public void addDataList(Common.ConsortObj.Consort_BusinessSkill _dataList) { dataList.Add(_dataList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (dataList.Count * 28);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (dataList.Count * 28);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _dataListCount = _buf.getShort();
	for(int _i = 0; _i < _dataListCount; _i++) { 
		Common.ConsortObj.Consort_BusinessSkill _dataList = new Common.ConsortObj.Consort_BusinessSkill();
		int __dataListCustLen = _buf.getInt();
	int __dataListCurPos = _buf.getCurPos();
	_dataList.ReadUnzipBuf(_buf, __dataListCurPos + __dataListCustLen);
	_buf.setPosition(__dataListCurPos + __dataListCustLen);

		dataList.Add(_dataList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)dataList.Count);
	for(int _i = 0; _i < dataList.Count; _i++) { 
		_buf.putInt(dataList[_i].GetBufSize());
	dataList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)9);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)9);
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
	builder.Append("dataList").Append(":").Append(dataList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

