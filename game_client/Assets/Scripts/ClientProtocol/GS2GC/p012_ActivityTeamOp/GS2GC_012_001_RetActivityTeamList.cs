using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p012_ActivityTeamOp
{

public class GS2GC_012_001_RetActivityTeamList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动队伍基础数据列表
/// </summary>
private List<Common.CrossTeamObj.CrossTeam_BaseInfo> teamBaseList;
/// <summary>
/// 队伍总数量，用于客户端分页计算
/// </summary>
private int totalCount;


public GS2GC_012_001_RetActivityTeamList() {
	teamBaseList = new List<Common.CrossTeamObj.CrossTeam_BaseInfo>();
	totalCount = 0;
}

public GS2GC_012_001_RetActivityTeamList(
	List<Common.CrossTeamObj.CrossTeam_BaseInfo> _teamBaseList
	, int _totalCount
) {	teamBaseList = _teamBaseList;
	totalCount = _totalCount;
}

public byte getMainOrder() { return (byte)12; }

public byte getSubOrder() { return (byte)1; }

/// <summary>
/// 活动队伍基础数据列表
/// </summary>
public List<Common.CrossTeamObj.CrossTeam_BaseInfo> getTeamBaseList() { return teamBaseList; }
/// <summary>
/// 活动队伍基础数据列表
/// </summary>
public void addTeamBaseList(Common.CrossTeamObj.CrossTeam_BaseInfo _teamBaseList) { teamBaseList.Add(_teamBaseList); }
/// <summary>
/// 队伍总数量，用于客户端分页计算
/// </summary>
public int getTotalCount() { return totalCount; }
/// <summary>
/// 队伍总数量，用于客户端分页计算
/// </summary>
public void setTotalCount(int _totalCount) { totalCount = _totalCount; }


public int GetBufSize() {
	int _size = 4;
	_size += 2;
for(int _i = 0; _i < teamBaseList.Count; _i++) {
	_size += 4 + teamBaseList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 2;
for(int _i = 0; _i < teamBaseList.Count; _i++) {
	_size += 4 + teamBaseList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _teamBaseListCount = _buf.getShort();
	for(int _i = 0; _i < _teamBaseListCount; _i++) { 
		Common.CrossTeamObj.CrossTeam_BaseInfo _teamBaseList = new Common.CrossTeamObj.CrossTeam_BaseInfo();
		int __teamBaseListCustLen = _buf.getInt();
	int __teamBaseListCurPos = _buf.getCurPos();
	_teamBaseList.ReadUnzipBuf(_buf, __teamBaseListCurPos + __teamBaseListCustLen);
	_buf.setPosition(__teamBaseListCurPos + __teamBaseListCustLen);

		teamBaseList.Add(_teamBaseList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalCount = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)teamBaseList.Count);
	for(int _i = 0; _i < teamBaseList.Count; _i++) { 
		_buf.putInt(teamBaseList[_i].GetBufSize());
	teamBaseList[_i].PutUnzipBuf(_buf);
	}
	_buf.putInt(totalCount);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)12);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)12);
	_recBuf.put((byte)1);
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
	builder.Append("teamBaseList").Append(":").Append(teamBaseList.ToString()).Append(", ");
	builder.Append("totalCount").Append(":").Append(totalCount.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

