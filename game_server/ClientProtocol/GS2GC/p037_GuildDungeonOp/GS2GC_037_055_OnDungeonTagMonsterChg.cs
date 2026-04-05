using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p037_GuildDungeonOp
{

/// <summary>
/// 公会副本-标签怪物变更
/// </summary>
public class GS2GC_037_055_OnDungeonTagMonsterChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 公会实例ID
/// </summary>
private long id;
private List<long> tagMonsterIdList;


public GS2GC_037_055_OnDungeonTagMonsterChg() {
	id = (long)0;
	tagMonsterIdList = new List<long>();
}

public GS2GC_037_055_OnDungeonTagMonsterChg(
	long _id
	, List<long> _tagMonsterIdList
) {	id = _id;
	tagMonsterIdList = _tagMonsterIdList;
}

public byte getMainOrder() { return (byte)37; }

public byte getSubOrder() { return (byte)55; }

/// <summary>
/// 公会实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 公会实例ID
/// </summary>
public void setId(long _id) { id = _id; }
public List<long> getTagMonsterIdList() { return tagMonsterIdList; }
public void addTagMonsterIdList(long _tagMonsterIdList) { tagMonsterIdList.Add(_tagMonsterIdList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (tagMonsterIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (tagMonsterIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _tagMonsterIdListCount = _buf.getShort();
	for(int _i = 0; _i < _tagMonsterIdListCount; _i++) { 
		long _tagMonsterIdList = (long)0;
		_tagMonsterIdList = _buf.getLong();
		tagMonsterIdList.Add(_tagMonsterIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putShort((short)tagMonsterIdList.Count);
	for(int _i = 0; _i < tagMonsterIdList.Count; _i++) { 
		_buf.putLong(tagMonsterIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)55);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)55);
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
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("tagMonsterIdList").Append(":").Append(tagMonsterIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

