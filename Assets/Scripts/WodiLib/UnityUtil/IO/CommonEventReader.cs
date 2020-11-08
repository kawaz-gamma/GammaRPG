using System;
using System.Collections.Generic;
using WodiLib.Common;
using WodiLib.Database;
using WodiLib.Event;

namespace WodiLib.UnityUtil.IO
{
    class CommonEventReader
    {
        public List<CommonEvent> Read(BinaryReadStatus readStatus,int length)
        {
            var list = new List<CommonEvent>();
            for (var i = 0; i < length; i++)
            {
                ReadOneCommonEvent(readStatus, list);
            }

            return list;
        }

        /// <summary>
        /// コモンイベント一つ
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="result">結果格納インスタンス</param>
        /// <exception cref="InvalidOperationException">バイナリデータがファイル仕様と異なる場合</exception>
        private void ReadOneCommonEvent(BinaryReadStatus readStatus, ICollection<CommonEvent> result)
        {
            var commonEvent = new CommonEvent();

            // コモンイベントヘッダ
            ReadHeader(readStatus);

            // コモンイベントID
            ReadCommonEventId(readStatus, commonEvent);

            // 起動条件
            ReadBootCondition(readStatus, commonEvent);

            // 数値引数の数
            ReadNumberArgLength(readStatus, commonEvent);

            // 文字列引数の数
            ReadStringArgLength(readStatus, commonEvent);

            // コモンイベント名
            ReadCommonEventName(readStatus, commonEvent);

            // イベントコマンド
            ReadEventCommand(readStatus, commonEvent);

            // メモ前の文字列
            ReadBeforeMemoString(readStatus, commonEvent);

            // メモ
            ReadMemo(readStatus, commonEvent);

            // 引数特殊指定
            ReadSpecialArgDesc(readStatus, commonEvent);

            // 引数初期値後のチェックディジット
            ReadAfterInitValueBytes(readStatus);

            // ラベル色
            ReadLabelColor(readStatus, commonEvent);

            // 変数名
            ReadSelfVarName(readStatus, commonEvent);

            // セルフ変数名の後のチェックディジット
            ReadAfterMemoBytesSelfVariableNamesBytes(readStatus);

            // フッタ文字列
            ReadFooterString(readStatus, commonEvent);

            // コモンイベント末尾A
            var hasNext = ReadFooterA(readStatus);
            if (hasNext == HasNext.No)
            {
                result.Add(commonEvent);
                return;
            }

            // 返戻値
            ReadReturnValue(readStatus, commonEvent);

            // コモンイベント末尾B
            ReadFooterB(readStatus);

            result.Add(commonEvent);
        }

        /// <summary>
        /// コモンイベントヘッダ
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">ヘッダが仕様と異なる場合</exception>
        private void ReadHeader(BinaryReadStatus readStatus)
        {
            foreach (var b in CommonEvent.HeaderBytes)
            {
                if (readStatus.ReadByte() != b)
                    throw new InvalidOperationException(
                        $"ファイルヘッダが仕様と異なります。（offset:{readStatus.Offset}）");
                readStatus.IncreaseByteOffset();
            }
        }

        /// <summary>
        /// コモンイベントID
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadCommonEventId(BinaryReadStatus readStatus, CommonEvent commonEvent)
        {
            commonEvent.Id = readStatus.ReadInt();
            readStatus.IncreaseIntOffset();
        }

        /// <summary>
        /// 起動条件
        /// </summary>
        /// <param name="readStatus"></param>
        /// <param name="commonEvent"></param>
        private void ReadBootCondition(BinaryReadStatus readStatus, CommonEvent commonEvent)
        {
            // 起動条件比較演算子 & 起動条件
            ReadBootConditionOperationAndType(readStatus, commonEvent.BootCondition);

            // 起動条件左辺
            ReadBootConditionLeftSide(readStatus, commonEvent.BootCondition);

            // 起動条件右辺
            ReadBootConditionRightSide(readStatus, commonEvent.BootCondition);
        }

        /// <summary>
        /// 起動条件比較演算子 &amp; 起動条件
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="condition">結果格納インスタンス</param>
        private void ReadBootConditionOperationAndType(BinaryReadStatus readStatus,
            CommonEventBootCondition condition)
        {
            var b = readStatus.ReadByte();
            readStatus.IncreaseByteOffset();
            condition.Operation = CriteriaOperator.FromByte((byte)(b & 0xF0));
            condition.CommonEventBootType = CommonEventBootType.FromByte((byte)(b & 0x0F));
        }

        /// <summary>
        /// 起動条件左辺
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="condition">結果格納インスタンス</param>
        private void ReadBootConditionLeftSide(BinaryReadStatus readStatus,
            CommonEventBootCondition condition)
        {
            condition.LeftSide = readStatus.ReadInt();
            readStatus.IncreaseIntOffset();
        }

        /// <summary>
        /// 起動条件右辺
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="condition">結果格納インスタンス</param>
        private void ReadBootConditionRightSide(BinaryReadStatus readStatus,
            CommonEventBootCondition condition)
        {
            condition.RightSide = readStatus.ReadInt();
            readStatus.IncreaseIntOffset();
        }

        /// <summary>
        /// 数値引数の数
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadNumberArgLength(BinaryReadStatus readStatus, CommonEvent commonEvent)
        {
            commonEvent.NumberArgsLength = readStatus.ReadByte();
            readStatus.IncreaseByteOffset();
        }

        /// <summary>
        /// 文字列引数の数
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadStringArgLength(BinaryReadStatus readStatus, CommonEvent commonEvent)
        {
            commonEvent.StrArgsLength = readStatus.ReadByte();
            readStatus.IncreaseByteOffset();
        }

        /// <summary>
        /// コモンイベント名
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadCommonEventName(BinaryReadStatus readStatus, CommonEvent commonEvent)
        {
            var commonEventName = readStatus.ReadString();
            commonEvent.Name = commonEventName.String;
            readStatus.AddOffset(commonEventName.ByteLength);
        }

        /// <summary>
        /// イベントコマンド
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadEventCommand(BinaryReadStatus readStatus, CommonEvent commonEvent)
        {
            var length = readStatus.ReadInt();
            readStatus.IncreaseIntOffset();

            var reader = new EventCommandListReader();
            commonEvent.EventCommands = reader.Read(readStatus, length);
        }

        /// <summary>
        /// メモ前の文字列
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadBeforeMemoString(BinaryReadStatus readStatus, CommonEvent commonEvent)
        {
            var str = readStatus.ReadString();
            commonEvent.Description = str.String;
            readStatus.AddOffset(str.ByteLength);
        }

        /// <summary>
        /// メモ
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadMemo(BinaryReadStatus readStatus, CommonEvent commonEvent)
        {
            var str = readStatus.ReadString();
            commonEvent.Memo = str.String;
            readStatus.AddOffset(str.ByteLength);
        }

        /// <summary>
        /// 引数特殊指定
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        /// <exception cref="InvalidOperationException">データが仕様と異なる場合</exception>
        private void ReadSpecialArgDesc(BinaryReadStatus readStatus, CommonEvent commonEvent)
        {
            try
            {
                var specialArgDescReader = new SpecialArgDescReader();
                specialArgDescReader.Read(readStatus);

                UpdateSpecialNumberArgDesc(
                    specialArgDescReader.NumberArgNameList,
                    specialArgDescReader.NumberArgTypeList,
                    specialArgDescReader.NumberArgStringParamsList,
                    specialArgDescReader.NumberArgNumberParamsList,
                    specialArgDescReader.NumberArgInitValueList,
                    commonEvent
                );

                UpdateSpecialStringArgDesc(
                    specialArgDescReader.StringArgNameList,
                    commonEvent
                );
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"引数特殊指定データが仕様と異なります。（offset:{readStatus.Offset}）", ex);
            }
        }

        /// <summary>
        /// 数値引数特殊指定
        /// </summary>
        /// <param name="argNameList">引数名リスト</param>
        /// <param name="argTypeList">引数特殊指定リスト</param>
        /// <param name="stringArgLists">数値特殊指定文字列パラメータリスト</param>
        /// <param name="numberArgLists">数値特殊指定数値パラメータリスト</param>
        /// <param name="numberArgInitValueList">数値特殊指定数値初期値リスト</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void UpdateSpecialNumberArgDesc(IReadOnlyList<string> argNameList,
            IReadOnlyList<CommonEventArgType> argTypeList, IReadOnlyList<List<string>> stringArgLists,
            IReadOnlyList<List<int>> numberArgLists, IReadOnlyList<int> numberArgInitValueList,
            CommonEvent commonEvent)
        {
            var argNameListCount = argNameList.Count;
            if (argNameListCount != argTypeList.Count || argNameListCount != stringArgLists.Count ||
                argNameListCount != numberArgLists.Count || argNameListCount != numberArgInitValueList.Count)
                throw new ArgumentException("引数リストの長さが異なります。");

            for (var i = 0; i < argNameListCount; i++)
            {
                var stringArgList = stringArgLists[i];
                var numberArgList = numberArgLists[i];

                var desc = MakeSpecialNumberArgDesc(argTypeList[i], argNameList[i],
                    numberArgInitValueList[i], numberArgList, stringArgList);

                commonEvent.NumberArgDescList[i] = desc;
            }
        }

        private CommonEventSpecialNumberArgDesc MakeSpecialNumberArgDesc(CommonEventArgType type,
            string argName, int initValue, List<int> numberArgList, List<string> stringArgList)
        {
            return type == CommonEventArgType.ReferDatabase
                ? UpdateSpecialNumberArgDesc_MakeDescForReferDatabase(type, argName,
                    initValue, numberArgList, stringArgList)
                : UpdateSpecialNumberArgDesc_MakeDescForElse(type, argName,
                    initValue, numberArgList, stringArgList);
        }

        private CommonEventSpecialNumberArgDesc UpdateSpecialNumberArgDesc_MakeDescForReferDatabase(
            CommonEventArgType type,
            string argName, int initValue, List<int> numberArgList, List<string> stringArgList)
        {
            var caseList = new List<CommonEventSpecialArgCase>();
            for (var j = 0; j < stringArgList.Count; j++)
            {
                var argCase = new CommonEventSpecialArgCase(-1 * (j + 1), stringArgList[j]);
                caseList.Add(argCase);
            }

            var desc = new CommonEventSpecialNumberArgDesc
            {
                ArgName = argName,
                InitValue = initValue
            };

            desc.ChangeArgType(type, caseList);
            desc.SetDatabaseRefer(DBKind.FromSpecialArgCode(numberArgList[0]), numberArgList[1]);
            desc.SetDatabaseUseAdditionalItemsFlag(numberArgList[2] == 1);

            return desc;
        }

        private CommonEventSpecialNumberArgDesc UpdateSpecialNumberArgDesc_MakeDescForElse(
            CommonEventArgType type,
            string argName, int initValue, List<int> numberArgList, List<string> stringArgList)
        {
            var stringArgListCount = stringArgList.Count;
            var numberArgListCount = numberArgList.Count;

            // 旧バージョンで作られたデータ限定？で文字列と数値の数が一致しないことがある。
            //   基本システムVer2のコモンイベント14などで確認。
            if (stringArgListCount != numberArgListCount)
            {
                // 何らかの処理を書くことになるか？
            }

            var loopTimes = stringArgListCount <= numberArgListCount
                ? stringArgListCount
                : numberArgListCount;

            var caseList = new List<CommonEventSpecialArgCase>();
            for (var j = 0; j < loopTimes; j++)
            {
                var argCase = new CommonEventSpecialArgCase(numberArgList[j], stringArgList[j]);
                caseList.Add(argCase);
            }

            var desc = new CommonEventSpecialNumberArgDesc
            {
                ArgName = argName,
                InitValue = initValue
            };

            desc.ChangeArgType(type, caseList);

            return desc;
        }

        /// <summary>
        /// 文字列引数特殊指定
        /// </summary>
        /// <param name="argNameList">引数名リスト</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void UpdateSpecialStringArgDesc(IReadOnlyList<string> argNameList,
            CommonEvent commonEvent)
        {
            var argNameListCount = argNameList.Count;

            for (var i = 0; i < argNameListCount; i++)
            {
                var desc = new CommonEventSpecialStringArgDesc
                {
                    ArgName = argNameList[i]
                };

                commonEvent.StringArgDescList[i] = desc;
            }
        }

        /// <summary>
        /// 引数初期値後のチェックディジット
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">データが仕様と異なる場合</exception>
        private void ReadAfterInitValueBytes(BinaryReadStatus readStatus)
        {
            foreach (var b in CommonEvent.AfterInitValueBytes)
            {
                if (readStatus.ReadByte() != b)
                    throw new InvalidOperationException(
                        $"ファイルデータが仕様と異なります。（offset:{readStatus.Offset}）");
                readStatus.IncreaseByteOffset();
            }
        }

        /// <summary>
        /// ラベル色
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadLabelColor(BinaryReadStatus readStatus, CommonEvent commonEvent)
        {
            var colorNumber = readStatus.ReadInt();
            readStatus.IncreaseIntOffset();

            commonEvent.LabelColor = CommonEventLabelColor.FromInt(colorNumber);
        }

        /// <summary>
        /// 変数名
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadSelfVarName(BinaryReadStatus readStatus, CommonEvent commonEvent)
        {
            const int varNameLength = 100;

            var varNameList = new List<CommonEventSelfVariableName>();

            for (var i = 0; i < varNameLength; i++)
            {
                var varName = readStatus.ReadString();
                readStatus.AddOffset(varName.ByteLength);

                varNameList.Add(varName.String);
            }

            commonEvent.SelfVariableNameList = new CommonEventSelfVariableNameList(varNameList);
        }

        /// <summary>
        /// セルフ変数名の後のチェックディジット
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">データが仕様と異なる場合</exception>
        private void ReadAfterMemoBytesSelfVariableNamesBytes(BinaryReadStatus readStatus)
        {
            foreach (var b in CommonEvent.AfterMemoBytesSelfVariableNamesBytes)
            {
                if (readStatus.ReadByte() != b)
                    throw new InvalidOperationException(
                        $"ファイルデータが仕様と異なります。（offset:{readStatus.Offset}）");
                readStatus.IncreaseByteOffset();
            }
        }

        /// <summary>
        /// フッタ文字列
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadFooterString(BinaryReadStatus readStatus, CommonEvent commonEvent)
        {
            var footerString = readStatus.ReadString();
            readStatus.AddOffset(footerString.ByteLength);

            commonEvent.FooterString = footerString.String;
        }

        /// <summary>
        /// コモンイベント末尾A
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <returns>あとに返戻値が続く場合true</returns>
        /// <exception cref="InvalidOperationException">データが仕様と異なる場合</exception>
        private HasNext ReadFooterA(BinaryReadStatus readStatus)
        {
            var b1 = readStatus.ReadByte();

            if (b1 == CommonEvent.BeforeReturnValueSummaryBytesBefore[0])
            {
                foreach (var b in CommonEvent.BeforeReturnValueSummaryBytesBefore)
                {
                    if (readStatus.ReadByte() != b)
                        throw new InvalidOperationException(
                            $"ファイルデータが仕様と異なります。（offset:{readStatus.Offset}）");
                    readStatus.IncreaseByteOffset();
                }

                return HasNext.Yes;
            }

            if (b1 == CommonEvent.FooterBytesBeforeVer2_00[0])
            {
                readStatus.IncreaseByteOffset();
                return HasNext.No;
            }

            throw new InvalidOperationException(
                $"ファイルデータが仕様と異なります。（offset:{readStatus.Offset}）");
        }

        /// <summary>
        /// 返戻値
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadReturnValue(BinaryReadStatus readStatus, CommonEvent commonEvent)
        {
            // 返戻値の意味
            ReadReturnValueDescription(readStatus, commonEvent);

            // 返戻セルフ変数インデックス
            ReadReturnVariableIndex(readStatus, commonEvent);
        }

        /// <summary>
        /// 返戻値の意味
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadReturnValueDescription(BinaryReadStatus readStatus, CommonEvent commonEvent)
        {
            var description = readStatus.ReadString();
            readStatus.AddOffset(description.ByteLength);

            commonEvent.ReturnValueDescription = description.String;
        }

        /// <summary>
        /// 返戻セルフ変数インデックス
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <param name="commonEvent">結果格納インスタンス</param>
        private void ReadReturnVariableIndex(BinaryReadStatus readStatus, CommonEvent commonEvent)
        {
            var index = readStatus.ReadInt();
            readStatus.IncreaseIntOffset();

            commonEvent.SetReturnVariableIndex(index);
        }

        /// <summary>
        /// コモンイベント末尾B
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <exception cref="InvalidOperationException">データが仕様と異なる場合</exception>
        private void ReadFooterB(BinaryReadStatus readStatus)
        {
            foreach (var b in CommonEvent.FooterBytesAfterVer2_00)
            {
                if (readStatus.ReadByte() != b)
                    throw new InvalidOperationException(
                        $"ファイルデータが仕様と異なります。（offset:{readStatus.Offset}）");
                readStatus.IncreaseByteOffset();
            }
        }

        /// <summary>
        /// 次へ続くフラグ
        /// </summary>
        private enum HasNext
        {
            Yes,
            No
        }
    }
}
