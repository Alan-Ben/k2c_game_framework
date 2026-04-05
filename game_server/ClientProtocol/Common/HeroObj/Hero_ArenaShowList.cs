using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.HeroObj
{

/// <summary>
/// 大臣竞技场展示信息列表
/// </summary>
public class Hero_ArenaShowList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 大臣列表
/// </summary>
private List<Common.HeroObj.Hero_ArenaShowInfo> heroList;


public Hero_ArenaShowList() {
	heroList = new List<Common.HeroObj.Hero_ArenaShowInfo>();
}

public Hero_ArenaShowList(
	List<Common.HeroObj.Hero_ArenaShowInfo> _heroList
) {	heroList = _heroList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 大臣列表
/// </summary>
public List<Common.HeroObj.Hero_ArenaShowInfo> getHeroList() { return heroList; }
/// <summary>
/// 大臣列表
/// </summary>
public void addHeroList(Common.HeroObj.Hero_ArenaShowInfo _heroList) { heroList.Add(_heroList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (heroList.Count * 32);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (heroList.Count * 32);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _heroListCount = _buf.getShort();
	for(int _i = 0; _i < _heroListCount; _i++) { 
		Common.HeroObj.Hero_ArenaShowInfo _heroList = new Common.HeroObj.Hero_ArenaShowInfo();
		int __heroListCustLen = _buf.getInt();
	int __heroListCurPos = _buf.getCurPos();
	_heroList.ReadUnzipBuf(_buf, __heroListCurPos + __heroListCustLen);
	_buf.setPosition(__heroListCurPos + __heroListCustLen);

		heroList.Add(_heroList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)heroList.Count);
	for(int _i = 0; _i < heroList.Count; _i++) { 
		_buf.putInt(heroList[_i].GetBufSize());
	heroList[_i].PutUnzipBuf(_buf);
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
	builder.Append("heroList").Append(":").Append(heroList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

