using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.NumMergeObj
{

/// <summary>
/// 数字合并-棋盘数据
/// </summary>
public class NumMerge_BoardData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 当前是第几步
/// </summary>
private int currentStep;
/// <summary>
/// 16个格子数据
/// </summary>
private List<Hotfix.Common.NumMergeObj.NumMerge_BlockBase> blocks;
/// <summary>
/// 当前得分
/// </summary>
private long currentScore;
/// <summary>
/// 单局积分最大值
/// </summary>
private long maxScore;
/// <summary>
/// 累计得分
/// </summary>
private long totalScore;


public NumMerge_BoardData() {
	currentStep = 0;
	blocks = new List<Hotfix.Common.NumMergeObj.NumMerge_BlockBase>();
	currentScore = (long)0;
	maxScore = (long)0;
	totalScore = (long)0;
}

public NumMerge_BoardData(
	int _currentStep
	, List<Hotfix.Common.NumMergeObj.NumMerge_BlockBase> _blocks
	, long _currentScore
	, long _maxScore
	, long _totalScore
) {	currentStep = _currentStep;
	blocks = _blocks;
	currentScore = _currentScore;
	maxScore = _maxScore;
	totalScore = _totalScore;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 当前是第几步
/// </summary>
public int getCurrentStep() { return currentStep; }
/// <summary>
/// 当前是第几步
/// </summary>
public void setCurrentStep(int _currentStep) { currentStep = _currentStep; }
/// <summary>
/// 16个格子数据
/// </summary>
public List<Hotfix.Common.NumMergeObj.NumMerge_BlockBase> getBlocks() { return blocks; }
/// <summary>
/// 16个格子数据
/// </summary>
public void addBlocks(Hotfix.Common.NumMergeObj.NumMerge_BlockBase _blocks) { blocks.Add(_blocks); }
/// <summary>
/// 当前得分
/// </summary>
public long getCurrentScore() { return currentScore; }
/// <summary>
/// 当前得分
/// </summary>
public void setCurrentScore(long _currentScore) { currentScore = _currentScore; }
/// <summary>
/// 单局积分最大值
/// </summary>
public long getMaxScore() { return maxScore; }
/// <summary>
/// 单局积分最大值
/// </summary>
public void setMaxScore(long _maxScore) { maxScore = _maxScore; }
/// <summary>
/// 累计得分
/// </summary>
public long getTotalScore() { return totalScore; }
/// <summary>
/// 累计得分
/// </summary>
public void setTotalScore(long _totalScore) { totalScore = _totalScore; }


public int GetBufSize() {
	int _size = 28;
	_size += 2 + (blocks.Count * 12);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;
	_size += 2 + (blocks.Count * 12);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	currentStep = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _blocksCount = _buf.getShort();
	for(int _i = 0; _i < _blocksCount; _i++) { 
		Hotfix.Common.NumMergeObj.NumMerge_BlockBase _blocks = new Hotfix.Common.NumMergeObj.NumMerge_BlockBase();
		int __blocksCustLen = _buf.getInt();
	int __blocksCurPos = _buf.getCurPos();
	_blocks.ReadUnzipBuf(_buf, __blocksCurPos + __blocksCustLen);
	_buf.setPosition(__blocksCurPos + __blocksCustLen);

		blocks.Add(_blocks);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	currentScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	maxScore = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalScore = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(currentStep);
	_buf.putShort((short)blocks.Count);
	for(int _i = 0; _i < blocks.Count; _i++) { 
		_buf.putInt(blocks[_i].GetBufSize());
	blocks[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(currentScore);
	_buf.putLong(maxScore);
	_buf.putLong(totalScore);
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
	builder.Append("currentStep").Append(":").Append(currentStep.ToString()).Append(", ");
	builder.Append("blocks").Append(":").Append(blocks.ToString()).Append(", ");
	builder.Append("currentScore").Append(":").Append(currentScore.ToString()).Append(", ");
	builder.Append("maxScore").Append(":").Append(maxScore.ToString()).Append(", ");
	builder.Append("totalScore").Append(":").Append(totalScore.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

