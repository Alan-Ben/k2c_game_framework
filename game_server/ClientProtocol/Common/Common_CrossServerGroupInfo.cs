using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_CrossServerGroupInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 跨服分组id
/// </summary>
private long groupId;
/// <summary>
/// 参与跨服的us列表
/// </summary>
private List<int> usIdList;


public Common_CrossServerGroupInfo() {
	groupId = (long)0;
	usIdList = new List<int>();
}

public Common_CrossServerGroupInfo(
	long _groupId
	, List<int> _usIdList
) {	groupId = _groupId;
	usIdList = _usIdList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 跨服分组id
/// </summary>
public long getGroupId() { return groupId; }
/// <summary>
/// 跨服分组id
/// </summary>
public void setGroupId(long _groupId) { groupId = _groupId; }
/// <summary>
/// 参与跨服的us列表
/// </summary>
public List<int> getUsIdList() { return usIdList; }
/// <summary>
/// 参与跨服的us列表
/// </summary>
public void addUsIdList(int _usIdList) { usIdList.Add(_usIdList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (usIdList.Count * 4);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (usIdList.Count * 4);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	groupId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _usIdListCount = _buf.getShort();
	for(int _i = 0; _i < _usIdListCount; _i++) { 
		int _usIdList = 0;
		_usIdList = _buf.getInt();
		usIdList.Add(_usIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(groupId);
	_buf.putShort((short)usIdList.Count);
	for(int _i = 0; _i < usIdList.Count; _i++) { 
		_buf.putInt(usIdList[_i]);
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
	builder.Append("groupId").Append(":").Append(groupId.ToString()).Append(", ");
	builder.Append("usIdList").Append(":").Append(usIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

