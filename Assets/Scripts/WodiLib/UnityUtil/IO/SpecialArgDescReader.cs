using System;
using System.Collections.Generic;
using System.Linq;
using WodiLib.Common;

namespace WodiLib.UnityUtil.IO
{
    class SpecialArgDescReader
    {
        /// <summary>数値引数名リスト</summary>
        public List<string> NumberArgNameList { get; private set; } = new List<string>();

        /// <summary>文字列引数名リスト</summary>
        public List<string> StringArgNameList { get; private set; } = new List<string>();

        /// <summary>数値引数種別リスト</summary>
        public List<CommonEventArgType> NumberArgTypeList { get; private set; } = new List<CommonEventArgType>();

        /// <summary>文字列引数種別リスト</summary>
        public List<CommonEventArgType> StringArgTypeList { get; private set; } = new List<CommonEventArgType>();

        /// <summary>数値引数特殊指定文字列パラメータリスト</summary>
        public List<List<string>> NumberArgStringParamsList { get; private set; } = new List<List<string>>();

        /// <summary>文字列引数特殊指定文字列パラメータリスト</summary>
        public List<List<string>> StringArgStringParamsList { get; private set; } = new List<List<string>>();

        /// <summary>数値引数特殊指定数値パラメータリスト</summary>
        public List<List<int>> NumberArgNumberParamsList { get; private set; } = new List<List<int>>();

        /// <summary>文字列引数特殊指定数値パラメータリスト</summary>
        public List<List<int>> StringArgNumberParamsList { get; private set; } = new List<List<int>>();

        /// <summary>数値特殊指定数値初期値リスト</summary>
        public List<int> NumberArgInitValueList { get; private set; } = new List<int>();

        /// <summary>
        /// 特殊引数リストを読み込み、返す。
        /// </summary>
        /// <returns>特殊引数リスト</returns>
        /// <exception cref="InvalidOperationException">ファイル仕様が異なる場合</exception>
        public void Read(BinaryReadStatus readStatus)
        {
            try
            {
                // ヘッダ
                foreach (var b in CommonEventSpecialArgDescList.Header)
                {
                    if (readStatus.ReadByte() != b)
                        throw new InvalidOperationException(
                            $"ファイルデータが仕様と異なります。（offset:{readStatus.Offset}）");
                    readStatus.IncreaseByteOffset();
                }

                // 引数名
                var argNameList = ReadArgNames(readStatus);
                var argNameListCount = argNameList.Count;
                NumberArgNameList = argNameList.Take(argNameListCount / 2).ToList();
                StringArgNameList = argNameList.Skip(argNameListCount / 2).ToList();
                // 引数特殊指定
                var argTypeList = ReadSpecialArgType(readStatus);
                var argTypeListCount = argTypeList.Count;
                NumberArgTypeList = argTypeList.Take(argTypeListCount / 2).ToList();
                StringArgTypeList = argTypeList.Skip(argTypeListCount / 2).ToList();
                // 数値特殊指定文字列パラメータ
                var stringArgLists = ReadSpecialStringArgList(readStatus);
                var stringArgListsCount = stringArgLists.Count;
                NumberArgStringParamsList = stringArgLists.Take(stringArgListsCount / 2).ToList();
                StringArgStringParamsList = stringArgLists.Skip(stringArgListsCount / 2).ToList();
                // 数値特殊指定数値パラメータ
                var numberArgLists = ReadSpecialNumberArgList(readStatus);
                var numberArgListsCount = numberArgLists.Count;
                NumberArgNumberParamsList = numberArgLists.Take(numberArgListsCount / 2).ToList();
                StringArgNumberParamsList = numberArgLists.Skip(numberArgListsCount / 2).ToList();
                // 数値特殊指定数値初期値
                NumberArgInitValueList = ReadInitValue(readStatus);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"引数特殊指定データが仕様と異なります。（offset:{readStatus.Offset}）", ex);
            }
        }

        /// <summary>
        /// 引数名
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <returns>引数名リスト</returns>
        private List<string> ReadArgNames(BinaryReadStatus readStatus)
        {
            var result = new List<string>();

            var length = readStatus.ReadInt();
            readStatus.IncreaseIntOffset();

            for (var i = 0; i < length; i++)
            {
                var argName = readStatus.ReadString();
                readStatus.AddOffset(argName.ByteLength);
                result.Add(argName.String);
            }

            return result;
        }

        /// <summary>
        /// 引数特殊指定
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <returns>引数特殊指定リスト</returns>
        private List<CommonEventArgType> ReadSpecialArgType(BinaryReadStatus readStatus)
        {
            var result = new List<CommonEventArgType>();

            var length = readStatus.ReadInt();
            readStatus.IncreaseIntOffset();

            for (var i = 0; i < length; i++)
            {
                var b = readStatus.ReadByte();
                readStatus.IncreaseByteOffset();
                result.Add(CommonEventArgType.FromByte(b));
            }

            return result;
        }

        /// <summary>
        /// 数値特殊指定文字列パラメータ
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <returns>数値特殊指定文字列パラメータリスト</returns>
        private List<List<string>> ReadSpecialStringArgList(BinaryReadStatus readStatus)
        {
            var result = new List<List<string>>();

            var length = readStatus.ReadInt();
            readStatus.IncreaseIntOffset();

            for (var i = 0; i < length; i++)
            {
                var caseLength = readStatus.ReadInt();
                readStatus.IncreaseIntOffset();

                var caseDescriptionList = new List<string>();

                for (var j = 0; j < caseLength; j++)
                {
                    var caseDescription = readStatus.ReadString();
                    readStatus.AddOffset(caseDescription.ByteLength);
                    caseDescriptionList.Add(caseDescription.String);
                }

                result.Add(caseDescriptionList);
            }

            return result;
        }

        /// <summary>
        /// 数値特殊指定数値パラメータ
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <returns>数値特殊指定数値パラメータリスト</returns>
        private List<List<int>> ReadSpecialNumberArgList(BinaryReadStatus readStatus)
        {
            var result = new List<List<int>>();

            var length = readStatus.ReadInt();
            readStatus.IncreaseIntOffset();

            for (var i = 0; i < length; i++)
            {
                var caseLength = readStatus.ReadInt();
                readStatus.IncreaseIntOffset();

                var caseNumberList = new List<int>();

                for (var j = 0; j < caseLength; j++)
                {
                    var caseNumber = readStatus.ReadInt();
                    readStatus.IncreaseIntOffset();
                    caseNumberList.Add(caseNumber);
                }

                result.Add(caseNumberList);
            }

            return result;
        }

        /// <summary>
        /// 数値特殊指定数値初期値
        /// </summary>
        /// <param name="readStatus">読み込み経過状態</param>
        /// <returns>数値特殊指定数値初期値リスト</returns>
        private List<int> ReadInitValue(BinaryReadStatus readStatus)
        {
            var result = new List<int>();

            var length = readStatus.ReadInt();
            readStatus.IncreaseIntOffset();

            for (var i = 0; i < length; i++)
            {
                var value = readStatus.ReadInt();
                readStatus.IncreaseIntOffset();

                result.Add(value);
            }

            return result;
        }
    }
}
