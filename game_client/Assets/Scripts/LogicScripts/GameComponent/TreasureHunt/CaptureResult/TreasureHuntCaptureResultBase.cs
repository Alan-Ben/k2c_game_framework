using System;
using Common.TreasureHuntEnum;
using Common.TreasureHuntObj;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 太空寻宝捕捉结果基类
    /// </summary>
    public abstract class TreasureHuntCaptureResultBase
    {
        /// <summary>
        /// 原始协议数据
        /// </summary>
        [NotNull] protected TreasureHunt_CaptureReward _protocolData;

        /// <summary>
        /// 获得类型
        /// </summary>
        public ETreasureHuntGainType gainType => _protocolData.getGainType();

        /// <summary>
        /// 原始数据
        /// </summary>
        public byte[] byteDataArray => _protocolData.getData();

        protected TreasureHuntCaptureResultBase([NotNull] TreasureHunt_CaptureReward protocolData)
        {
            _protocolData = protocolData;
        }

        /// <summary>
        /// 创建对应类型的捕捉结果实例
        /// </summary>
        /// <param name="captureResult">协议数据</param>
        /// <returns>对应类型的实例</returns>
        public static TreasureHuntCaptureResultBase CreateCaptureResult(TreasureHunt_CaptureReward captureResult)
        {
            if (captureResult == null)
                return null;

            switch (captureResult.getGainType())
            {
                case ETreasureHuntGainType.ORE:
                    return new TreasureHuntCaptureResultOre(captureResult);
                case ETreasureHuntGainType.TREASURE:
                    return new TreasureHuntCaptureResultTreasure(captureResult);
                case ETreasureHuntGainType.REWARD:
                    return new TreasureHuntCaptureResultReward(captureResult);
                default:
                    return null;
            }
        }
        
        /// <summary>
        /// 展示捕捉结果的抽象方法，由子类实现具体逻辑
        /// </summary>
        public abstract void showCaptureResult(Action _showDone);
    }
}
