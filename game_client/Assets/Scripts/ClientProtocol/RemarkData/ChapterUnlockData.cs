using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace RemarkData
{

/// <summary>
/// 章节解锁数据
/// </summary>
public class ChapterUnlockData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 章节解锁id列表
/// </summary>
private List<long> unlockIdList;


public ChapterUnlockData() {
	unlockIdList = new List<long>();
}

public ChapterUnlockData(
	List<long> _unlockIdList
) {	unlockIdList = _unlockIdList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 章节解锁id列表
/// </summary>
public List<long> getUnlockIdList() { return unlockIdList; }
/// <summary>
/// 章节解锁id列表
/// </summary>
public void addUnlockIdList(long _unlockIdList) { unlockIdList.Add(_unlockIdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (unlockIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (unlockIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _unlockIdListCount = _buf.getShort();
	for(int _i = 0; _i < _unlockIdListCount; _i++) { 
		long _unlockIdList = (long)0;
		_unlockIdList = _buf.getLong();
		unlockIdList.Add(_unlockIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)unlockIdList.Count);
	for(int _i = 0; _i < unlockIdList.Count; _i++) { 
		_buf.putLong(unlockIdList[_i]);
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
	builder.Append("unlockIdList").Append(":").Append(unlockIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

