using System;
using System.Collections.Generic;
using System.Dynamic;

namespace FIAT.API.Models.ReportDto
{
    public class TourBaseScoreDto: DynamicObject
    {
        /// <summary>
        /// 数据字段
        /// </summary>
        private Dictionary<string, object> ViewData = new Dictionary<string, object>();

        /// <summary>
        /// 调用 varo(); 时执行
        /// dynamic varo = new VarObject();
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            return base.TryInvoke(binder, args, out result);
        }

        /// <summary>
        /// 调用 varo.Method(); 时执行
        /// dynamic varo = new VarObject();
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            return base.TryInvokeMember(binder, args, out result);
        }

        /// <summary>
        /// 调用 varo + varo; 时执行
        /// dynamic varo = new VarObject();
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="arg"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            return base.TryBinaryOperation(binder, arg, out result);
        }

        /// <summary>
        /// 调用 varo++; 时执行
        /// dynamic varo = new VarObject();
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
        {
            return base.TryUnaryOperation(binder, out result);
        }

        /// <summary>
        /// 调用 varo["key"]; 时执行
        /// dynamic varo = new VarObject();
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="indexes"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if (indexes == null || indexes.Length != 1)
            {
                throw new ArgumentException("indexes");
            }

            result = null;
            string key = indexes[0] as string;
            if (key != null)
            {
                result = ViewData[key];
            }
            else
            {
                throw new ArgumentException("indexes");
            }
            return true;
        }

        /// <summary>
        /// 调用 varo["key"] = "value"; 时执行
        /// dynamic varo = new VarObject();
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="indexes"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (indexes == null || indexes.Length != 1)
            {
                throw new ArgumentException("indexes");
            }

            string key = indexes[0] as string;
            if (key != null)
            {
                ViewData[key] = value;
                return true;
            }
            else
            {
                throw new ArgumentException("indexes");
            }
        }

        /// <summary>
        /// 获取所有key
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return ViewData.Keys;
        }

        /// <summary>
        /// 调用 varo.key; 时执行
        /// dynamic varo = new VarObject();
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = ViewData[binder.Name];
            return true;
        }

        /// <summary>
        /// 调用 varo.key = "value"; 时执行
        /// dynamic varo = new VarObject();
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            ViewData[binder.Name] = value;
            return true;
        }
    }
    public class ColDto
    {
        public string ColName { get; set; }
        public string ColValue { get; set; }
        public string TITitle { get; set; }
    }
    public class RDto
    {
        public string Title { get; set; }
        public RDto()
        {
            colList ="";
            wList = "";
            touList ="";
            bList = "";
            hList = "";
            rList = "";
        }
        public string colList { get; set; }
        public string wList { get; set; }
        public string touList { get; set; }
        public string bList { get; set; }
        public string hList { get; set; }
        public string rList { get; set; }

    }
}
