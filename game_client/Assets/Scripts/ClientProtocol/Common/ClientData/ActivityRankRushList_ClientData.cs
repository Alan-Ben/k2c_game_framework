using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ClientData
{

/// <summary>
/// 活动冲榜-客户端数据
/// </summary>
public class ActivityRankRushList_ClientData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 数据列表
/// </summary>
private List<Common.ClientData.ActivityRankRush_ClientData> dataList;


public ActivityRankRushList_ClientData() {
	dataList = new List<Common.ClientData.ActivityRankRush_ClientData>();
}

public ActivityRankRushList_ClientData(
	List<Common.ClientData.ActivityRankRush_ClientData> _dataList
) {	dataList = _dataList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 数据列表
/// </summary>
public List<Common.ClientData.ActivityRankRush_ClientData> getDataList() { return dataList; }
/// <summary>
/// 数据列表
/// </summary>
public void addDataList(Common.ClientData.ActivityRankRush_ClientData _dataList) { dataList.Add(_dataList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (dataList.Count * 36);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (dataList.Count * 36);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _dataListCount = _buf.getShort();
	for(int _i = 0; _i < _dataListCount; _i++) { 
		Common.ClientData.ActivityRankRush_ClientData _dataList = new Common.ClientData.ActivityRankRush_ClientData();
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
	builder.Append("dataList").Append(":").Append(dataList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

