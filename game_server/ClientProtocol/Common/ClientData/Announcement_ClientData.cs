using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ClientData
{

/// <summary>
/// 运营公告-客户端数据
/// </summary>
public class Announcement_ClientData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 最大的运营公告id
/// </summary>
private long maxId;
/// <summary>
/// 已读列表
/// </summary>
private List<long> readIdList;


public Announcement_ClientData() {
	maxId = (long)0;
	readIdList = new List<long>();
}

public Announcement_ClientData(
	long _maxId
	, List<long> _readIdList
) {	maxId = _maxId;
	readIdList = _readIdList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 最大的运营公告id
/// </summary>
public long getMaxId() { return maxId; }
/// <summary>
/// 最大的运营公告id
/// </summary>
public void setMaxId(long _maxId) { maxId = _maxId; }
/// <summary>
/// 已读列表
/// </summary>
public List<long> getReadIdList() { return readIdList; }
/// <summary>
/// 已读列表
/// </summary>
public void addReadIdList(long _readIdList) { readIdList.Add(_readIdList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (readIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (readIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	maxId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _readIdListCount = _buf.getShort();
	for(int _i = 0; _i < _readIdListCount; _i++) { 
		long _readIdList = (long)0;
		_readIdList = _buf.getLong();
		readIdList.Add(_readIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(maxId);
	_buf.putShort((short)readIdList.Count);
	for(int _i = 0; _i < readIdList.Count; _i++) { 
		_buf.putLong(readIdList[_i]);
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
	builder.Append("maxId").Append(":").Append(maxId.ToString()).Append(", ");
	builder.Append("readIdList").Append(":").Append(readIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

