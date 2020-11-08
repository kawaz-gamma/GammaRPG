using System;
using System.Collections.Generic;
using WodiLib.Common;
using WodiLib.Event;
using WodiLib.Event.EventCommand;

namespace WodiLib.UnityUtil.IO
{
    public class CommonEventDatFileReader : WoditorFileReader<CommonEventData>
    {
        public CommonEventDatFileReader() { }

        protected override CommonEventData Read()
        {
            var commonEventData = new CommonEventData();

            // ヘッダチェック
            ReadHeader();

            // コモンイベント
            ReadCommonEvent(commonEventData);

            // フッタチェック
            ReadFooter();

            return commonEventData;
        }

        private void ReadHeader()
        {
            foreach (var b in CommonEventData.Header)
            {
                if (ReadStatus.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルヘッダがファイル仕様と異なります（offset:{ReadStatus.Offset}）");
                }

                ReadStatus.IncreaseByteOffset();
            }
        }

        /// <summary>
        /// コモンイベントリスト
        /// </summary>
        /// <param name="data">結果格納インスタンス</param>
        /// <exception cref="InvalidOperationException">ファイルが仕様と異なる場合</exception>
        private void ReadCommonEvent( CommonEventData data)
        {
            // コモンイベント数
            var length = ReadCommonEventLength();

            // コモンイベントリスト
            ReadCommonEventList(length, data);
        }


        /// <summary>
        /// コモンイベント数
        /// </summary>
        /// <returns>コモンイベント数</returns>
        private int ReadCommonEventLength()
        {
            var length = ReadStatus.ReadInt();
            ReadStatus.IncreaseIntOffset();

            return length;
        }

        /// <summary>
        /// コモンイベントリスト
        /// </summary>
        /// <param name="length">コモンイベント数</param>
        /// <param name="data">結果格納インスタンス</param>
        /// <exception cref="InvalidOperationException">ファイルが仕様と異なる場合</exception>
        private void ReadCommonEventList(int length, CommonEventData data)
        {
            var reader = new CommonEventReader();

            var commonEventList = reader.Read(ReadStatus, length);
            data.CommonEventList = new CommonEventList(commonEventList);
        }

        /// <summary>
        /// フッタ
        /// </summary>
        /// <exception cref="InvalidOperationException">ファイルヘッダが仕様と異なる場合</exception>
        private void ReadFooter()
        {
            foreach (var b in CommonEventData.Footer)
            {
                if (ReadStatus.ReadByte() != b)
                {
                    throw new InvalidOperationException(
                        $"ファイルフッタがファイル仕様と異なります（offset:{ReadStatus.Offset}）");
                }

                ReadStatus.IncreaseByteOffset();
            }
        }
    }
}
