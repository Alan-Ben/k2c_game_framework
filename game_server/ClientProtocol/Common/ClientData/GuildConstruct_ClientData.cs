using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ClientData
{

/// <summary>
/// 联盟建设-客户端数据
/// </summary>
public class GuildConstruct_ClientData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 日期
/// </summary>
private int date;
/// <summary>
/// 建设id列表
/// </summary>
private List<long> constructRefIdList;


public GuildConstruct_ClientData() {
	date = 0;
	constructRefIdList = new List<long>();
}

public GuildConstruct_ClientData(
	int _date
	, List<long> _constructRefIdList
) {	date = _date;
	constructRefIdList = _constructRefIdList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 日期
/// </summary>
public int getDate() { return date; }
/// <summary>
/// 日期
/// </summary>
public void setDate(int _date) { date = _date; }
/// <summary>
/// 建设id列表
/// </summary>
public List<long> getConstructRefIdList() { return constructRefIdList; }
/// <summary>
/// 建设id列表
/// </summary>
public void addConstructRefIdList(long _constructRefIdList) { constructRefIdList.Add(_constructRefIdList); }


public int GetBufSize() {
	int _size = 4;
	_size += 2 + (constructRefIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (constructRefIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	date = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _constructRefIdListCount = _buf.getShort();
	for(int _i = 0; _i < _constructRefIdListCount; _i++) { 
		long _constructRefIdList = (long)0;
		_constructRefIdList = _buf.getLong();
		constructRefIdList.Add(_constructRefIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(date);
	_buf.putShort((short)constructRefIdList.Count);
	for(int _i = 0; _i < constructRefIdList.Count; _i++) { 
		_buf.putLong(constructRefIdList[_i]);
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
	builder.Append("date").Append(":").Append(date.ToString()).Append(", ");
	builder.Append("constructRefIdList").Append(":").Append(constructRefIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

