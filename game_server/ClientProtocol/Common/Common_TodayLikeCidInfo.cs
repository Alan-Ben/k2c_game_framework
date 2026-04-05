using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_TodayLikeCidInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 上次刷新日期
/// </summary>
private int lastFreshDate;
/// <summary>
/// 点赞过玩家CID列表
/// </summary>
private List<long> likeCidList;


public Common_TodayLikeCidInfo() {
	lastFreshDate = 0;
	likeCidList = new List<long>();
}

public Common_TodayLikeCidInfo(
	int _lastFreshDate
	, List<long> _likeCidList
) {	lastFreshDate = _lastFreshDate;
	likeCidList = _likeCidList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 上次刷新日期
/// </summary>
public int getLastFreshDate() { return lastFreshDate; }
/// <summary>
/// 上次刷新日期
/// </summary>
public void setLastFreshDate(int _lastFreshDate) { lastFreshDate = _lastFreshDate; }
/// <summary>
/// 点赞过玩家CID列表
/// </summary>
public List<long> getLikeCidList() { return likeCidList; }
/// <summary>
/// 点赞过玩家CID列表
/// </summary>
public void addLikeCidList(long _likeCidList) { likeCidList.Add(_likeCidList); }


public int GetBufSize() {
	int _size = 4;
	_size += 2 + (likeCidList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (likeCidList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lastFreshDate = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _likeCidListCount = _buf.getShort();
	for(int _i = 0; _i < _likeCidListCount; _i++) { 
		long _likeCidList = (long)0;
		_likeCidList = _buf.getLong();
		likeCidList.Add(_likeCidList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(lastFreshDate);
	_buf.putShort((short)likeCidList.Count);
	for(int _i = 0; _i < likeCidList.Count; _i++) { 
		_buf.putLong(likeCidList[_i]);
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
	builder.Append("lastFreshDate").Append(":").Append(lastFreshDate.ToString()).Append(", ");
	builder.Append("likeCidList").Append(":").Append(likeCidList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

