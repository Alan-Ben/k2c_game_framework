using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 家人初始化
/// </summary>
public class GS2GC_002_015_RetConsortList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已获得家人列表
/// </summary>
private List<Common.ConsortObj.Consort_Info> consortList;
/// <summary>
/// 已解锁的CG列表
/// </summary>
private List<Common.ConsortObj.Consort_CGInfo> unlockedCGList;
/// <summary>
/// 随机邀约的指定妃子ID列表
/// </summary>
private List<long> randCallConsortIdList;


public GS2GC_002_015_RetConsortList() {
	consortList = new List<Common.ConsortObj.Consort_Info>();
	unlockedCGList = new List<Common.ConsortObj.Consort_CGInfo>();
	randCallConsortIdList = new List<long>();
}

public GS2GC_002_015_RetConsortList(
	List<Common.ConsortObj.Consort_Info> _consortList
	, List<Common.ConsortObj.Consort_CGInfo> _unlockedCGList
	, List<long> _randCallConsortIdList
) {	consortList = _consortList;
	unlockedCGList = _unlockedCGList;
	randCallConsortIdList = _randCallConsortIdList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)15; }

/// <summary>
/// 已获得家人列表
/// </summary>
public List<Common.ConsortObj.Consort_Info> getConsortList() { return consortList; }
/// <summary>
/// 已获得家人列表
/// </summary>
public void addConsortList(Common.ConsortObj.Consort_Info _consortList) { consortList.Add(_consortList); }
/// <summary>
/// 已解锁的CG列表
/// </summary>
public List<Common.ConsortObj.Consort_CGInfo> getUnlockedCGList() { return unlockedCGList; }
/// <summary>
/// 已解锁的CG列表
/// </summary>
public void addUnlockedCGList(Common.ConsortObj.Consort_CGInfo _unlockedCGList) { unlockedCGList.Add(_unlockedCGList); }
/// <summary>
/// 随机邀约的指定妃子ID列表
/// </summary>
public List<long> getRandCallConsortIdList() { return randCallConsortIdList; }
/// <summary>
/// 随机邀约的指定妃子ID列表
/// </summary>
public void addRandCallConsortIdList(long _randCallConsortIdList) { randCallConsortIdList.Add(_randCallConsortIdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < consortList.Count; _i++) {
	_size += 4 + consortList[_i].GetBufSize();
	}

	_size += 2 + (unlockedCGList.Count * 13);
	_size += 2 + (randCallConsortIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < consortList.Count; _i++) {
	_size += 4 + consortList[_i].GetBufSize();
	}

	_size += 2 + (unlockedCGList.Count * 13);
	_size += 2 + (randCallConsortIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _consortListCount = _buf.getShort();
	for(int _i = 0; _i < _consortListCount; _i++) { 
		Common.ConsortObj.Consort_Info _consortList = new Common.ConsortObj.Consort_Info();
		int __consortListCustLen = _buf.getInt();
	int __consortListCurPos = _buf.getCurPos();
	_consortList.ReadUnzipBuf(_buf, __consortListCurPos + __consortListCustLen);
	_buf.setPosition(__consortListCurPos + __consortListCustLen);

		consortList.Add(_consortList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _unlockedCGListCount = _buf.getShort();
	for(int _i = 0; _i < _unlockedCGListCount; _i++) { 
		Common.ConsortObj.Consort_CGInfo _unlockedCGList = new Common.ConsortObj.Consort_CGInfo();
		int __unlockedCGListCustLen = _buf.getInt();
	int __unlockedCGListCurPos = _buf.getCurPos();
	_unlockedCGList.ReadUnzipBuf(_buf, __unlockedCGListCurPos + __unlockedCGListCustLen);
	_buf.setPosition(__unlockedCGListCurPos + __unlockedCGListCustLen);

		unlockedCGList.Add(_unlockedCGList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _randCallConsortIdListCount = _buf.getShort();
	for(int _i = 0; _i < _randCallConsortIdListCount; _i++) { 
		long _randCallConsortIdList = (long)0;
		_randCallConsortIdList = _buf.getLong();
		randCallConsortIdList.Add(_randCallConsortIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)consortList.Count);
	for(int _i = 0; _i < consortList.Count; _i++) { 
		_buf.putInt(consortList[_i].GetBufSize());
	consortList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)unlockedCGList.Count);
	for(int _i = 0; _i < unlockedCGList.Count; _i++) { 
		_buf.putInt(unlockedCGList[_i].GetBufSize());
	unlockedCGList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)randCallConsortIdList.Count);
	for(int _i = 0; _i < randCallConsortIdList.Count; _i++) { 
		_buf.putLong(randCallConsortIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)15);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)15);
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
	builder.Append("consortList").Append(":").Append(consortList.ToString()).Append(", ");
	builder.Append("unlockedCGList").Append(":").Append(unlockedCGList.ToString()).Append(", ");
	builder.Append("randCallConsortIdList").Append(":").Append(randCallConsortIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

