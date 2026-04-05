using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class WCGGS2GC_UniformItemChg : ALBasicProtocolPack._IALProtocolStructure {
private long uniformId;
private long count;
private long srcCount;
private List<Common.WCGGS2GC_UniformItemFix> fixList;


public WCGGS2GC_UniformItemChg() {
	uniformId = (long)0;
	count = (long)0;
	srcCount = (long)0;
	fixList = new List<Common.WCGGS2GC_UniformItemFix>();
}

public WCGGS2GC_UniformItemChg(
	long _uniformId
	, long _count
	, long _srcCount
	, List<Common.WCGGS2GC_UniformItemFix> _fixList
) {	uniformId = _uniformId;
	count = _count;
	srcCount = _srcCount;
	fixList = _fixList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getUniformId() { return uniformId; }
public void setUniformId(long _uniformId) { uniformId = _uniformId; }
public long getCount() { return count; }
public void setCount(long _count) { count = _count; }
public long getSrcCount() { return srcCount; }
public void setSrcCount(long _srcCount) { srcCount = _srcCount; }
public List<Common.WCGGS2GC_UniformItemFix> getFixList() { return fixList; }
public void addFixList(Common.WCGGS2GC_UniformItemFix _fixList) { fixList.Add(_fixList); }


public int GetBufSize() {
	int _size = 24;
	_size += 2 + (fixList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;
	_size += 2 + (fixList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	uniformId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	count = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	srcCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _fixListCount = _buf.getShort();
	for(int _i = 0; _i < _fixListCount; _i++) { 
		Common.WCGGS2GC_UniformItemFix _fixList = new Common.WCGGS2GC_UniformItemFix();
		int __fixListCustLen = _buf.getInt();
	int __fixListCurPos = _buf.getCurPos();
	_fixList.ReadUnzipBuf(_buf, __fixListCurPos + __fixListCustLen);
	_buf.setPosition(__fixListCurPos + __fixListCustLen);

		fixList.Add(_fixList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(uniformId);
	_buf.putLong(count);
	_buf.putLong(srcCount);
	_buf.putShort((short)fixList.Count);
	for(int _i = 0; _i < fixList.Count; _i++) { 
		_buf.putInt(fixList[_i].GetBufSize());
	fixList[_i].PutUnzipBuf(_buf);
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
	builder.Append("uniformId").Append(":").Append(uniformId.ToString()).Append(", ");
	builder.Append("count").Append(":").Append(count.ToString()).Append(", ");
	builder.Append("srcCount").Append(":").Append(srcCount.ToString()).Append(", ");
	builder.Append("fixList").Append(":").Append(fixList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

