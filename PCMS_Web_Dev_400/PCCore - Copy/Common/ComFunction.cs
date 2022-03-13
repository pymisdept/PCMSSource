using System;
using System.Collections.Generic;
using System.Text;
using PCCore;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Data;
using System.Data.Common;
using SimpleControls.SimpleDatabase;
using System.Collections;
using System.Web;
using System.Threading;
using Microsoft;

namespace PCCore
{
    public class ComFunction2
    {

        /// <summary>
        /// 判断当前是否进行的是UnitTest 
        /// 使用的判断对象是System.AppDomain
        /// </summary>
        public static bool IsUnitTest
        {
            get
            {
                //Assembly a = Assembly.GetCallingAssembly();
                //return a.GetName().Name == "PCTest";

                //IDE 进行调试的时候没有ApplicationName 
                return AppDomain.CurrentDomain.SetupInformation.ApplicationName == null;
            }
        }

        /// <summary>
        /// SessionInfo.Database + "SY_SYSTEMSETTING";
        /// </summary>
        public static string SysSystemSetting
        {
            get
            {
                return SessionInfo.Database + "SY_SYSTEMSETTING";
            }
        }

        /// <summary>
        /// SessionInfo.Database + "SY_SYSTEMSE";
        /// </summary>
        public static string SysSystemSet
        {
            get
            {
                return SessionInfo.Database + "SY_SYSTEMSE";
            }
        }



        /// <summary>
        /// 根据资源取资源值,add by Jawance 2008-06-18
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="pStringID"></param>
        /// <param name="IsNoFoundUseKeyOut">Is Not found ,out put use key</param>
        /// <returns></returns>
        public static string GetGlobalResourceObject_ByStringID(string pType, string pStringID, bool IsNoFoundUseKeyOut)
        {
            return GetGlobalResourceObject_ByStringID(pType, pStringID, null, IsNoFoundUseKeyOut);
        }


        /// <summary>
        /// 根据资源取资源值,add by Jawance 2006-08-09
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="pStringID"></param>
        /// <returns></returns>
        public static string GetGlobalResourceObject_ByStringID(string pType, string pStringID)
        {
            return GetGlobalResourceObject_ByStringID(pType, pStringID, null, false);
        }

        /// <summary>
        /// 根据使用的资源类型和资源关键字获取资源值
        /// 如果没有找到，使用资源关键字进行填充。
        /// 2008-06-18
        /// </summary>
        /// <param name="pType"></param>
        /// <param name="pStringID"></param>
        /// <param name="pCultureInfo"></param>
        /// <param name="IsNotfoundUseKeyOut"></param>
        /// <returns></returns>
        public static string GetGlobalResourceObject_ByStringID(string pType, string pStringID, System.Globalization.CultureInfo pCultureInfo, bool IsNotfoundUseKeyOut)
        {
            string _ReturnResourcesString = string.Empty;
            object ResourceText = string.Empty;
            object EmptyObj = pStringID;
            switch (pType)
            {
                case Consts.ResourcesMessages:
                    if (pCultureInfo != null)
                    {
                        ResourceText = SimpleControls.SimpleUtils.IfNull(HttpContext.GetGlobalResourceObject(Consts.ResourcesMessages,
                            pStringID, pCultureInfo), EmptyObj);
                    }
                    else
                    {
                        ResourceText = SimpleControls.SimpleUtils.IfNull(HttpContext.GetGlobalResourceObject(Consts.ResourcesMessages, pStringID)
                            , EmptyObj);
                    }
                    _ReturnResourcesString = ResourceText.ToString();
                    break;
                case Consts.ResourcesLabels:
                    if (pCultureInfo != null)
                    {
                        ResourceText = SimpleControls.SimpleUtils.IfNull(HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels,
                            pStringID, pCultureInfo), EmptyObj);
                    }
                    else
                    {
                        ResourceText = SimpleControls.SimpleUtils.IfNull(HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, pStringID)
                            , EmptyObj);
                    }
                    _ReturnResourcesString = ResourceText.ToString();
                    break;
                case Consts.ResourcesCommon:
                    if (pCultureInfo != null)
                    {
                        ResourceText = SimpleControls.SimpleUtils.IfNull(HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon,
                            pStringID, pCultureInfo), EmptyObj);
                    }
                    else
                    {
                        ResourceText = SimpleControls.SimpleUtils.IfNull(HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, pStringID)
                            , EmptyObj);
                    }
                    _ReturnResourcesString = ResourceText.ToString();
                    break;
                case Consts.ResourcesReportLabels:

                    if (pCultureInfo != null)
                    {
                        ResourceText = SimpleControls.SimpleUtils.IfNull(HttpContext.GetGlobalResourceObject(Consts.ResourcesReportLabels,
                            pStringID, pCultureInfo), string.Empty);//报表的暂时没有录入的按照设计时的值显示
                    }
                    else
                    {
                        ResourceText = SimpleControls.SimpleUtils.IfNull(HttpContext.GetGlobalResourceObject(Consts.ResourcesReportLabels, pStringID)
                            , string.Empty);//报表的暂时没有录入的按照设计时的值显示
                    }
                    _ReturnResourcesString = ResourceText.ToString();

                    break;
            }
            //如果没有找到的话并且参数使用KeyOut，就使用KeyOutPut
            if (IsNotfoundUseKeyOut && string.IsNullOrEmpty(_ReturnResourcesString))
            {
                _ReturnResourcesString = pStringID;
            }
            return _ReturnResourcesString;
        }       

        /// <summary>
        /// eval string to decimal
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>

        public static decimal Eval(string strValue)
        {
            double tmpvalue = 0;
            try
            {
                //tmpvalue = Convert.ToDouble(Microsoft.JScript.Eval.JScriptEvaluate(strValue, _engine).ToString());
                tmpvalue = Convert.ToDouble(SimpleControls.SimpleEvaluator.Eval(strValue));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return Convert.ToDecimal(tmpvalue);
        }

        #region Get TimsSpan Between to DateTime
        public static decimal GetTimeSpan(string type, DateTime start, DateTime end)
        {
            decimal tmp = 0;

            TimeSpan ts = end - start;

            switch (type)
            {
                case Consts.Day:
                    tmp = Convert.ToDecimal(Math.Round(ts.TotalDays + 1, 2));
                    break;
                case Consts.Hour:
                    tmp = Convert.ToDecimal(Math.Round(ts.TotalHours, 2));
                    break;
                case Consts.Minute:
                    tmp = Convert.ToDecimal(Math.Round(ts.TotalMinutes, 2));
                    break;
                case Consts.Year:
                    tmp = Convert.ToDecimal(end.Year - start.Year + (end.Month - start.Month) / 12);
                    break;
                case Consts.Month:
                    tmp = Convert.ToDecimal((end.Year - start.Year) * 12 + (end.Month - start.Month));
                    break;

                default:
                    tmp = Convert.ToDecimal(Math.Round(ts.TotalHours, 2));
                    break;
            }

            return tmp;
        }

        /* 可耻的函数
        public static decimal GetTotalTime(string type, DateTime start, DateTime end)
        {
            decimal tmp = 0;

            TimeSpan ts = end - start;

            switch (type)
            {
                case Consts.Day:
                    tmp = Convert.ToDecimal(Math.Round(ts.TotalDays, 2));
                    break;

                default:
                    tmp = Convert.ToDecimal(Math.Round(ts.TotalDays, 2));
                    break;
            }

            return tmp + 1;
        }
         */

        public static decimal GetTimeSpan(DateTime start, DateTime end)
        {
            decimal tmp = 0;
            TimeSpan ts = end - start;
            tmp = Convert.ToDecimal(Math.Round(ts.TotalHours, 2));
            return tmp;
        }

        public static decimal MathRound(decimal d,int decimals)
        {
            return Math.Round(d, decimals, MidpointRounding.AwayFromZero);
        }

        public static double MathRound(double value, int digits)
        {
            return Math.Round(value, digits, MidpointRounding.AwayFromZero);
        }
        #endregion




        public static void InitCulture(Thread thread)
        {
            string culture = SessionInfo.CurrentLanguage;

            CultureInfo ci;
            try
            {
                ci = new CultureInfo(culture);
            }
            catch
            {
                culture = "en-US";
                ci = new CultureInfo(culture);
            }

            ci.DateTimeFormat.LongDatePattern = Consts.DateLongFormat;
            ci.DateTimeFormat.ShortDatePattern = Consts.DateFormat;
            ci.DateTimeFormat.LongTimePattern = Consts.TimeFormat;
            ci.DateTimeFormat.ShortTimePattern = Consts.ShortTimeFormat;

            if (thread == null) thread = Thread.CurrentThread;

            thread.CurrentCulture = ci;
            thread.CurrentUICulture = ci;
        }

        #region HandleError
        public static void HandleError(Exception ex, SimpleControls.Web.SimpleNote pNote)
        {
            pNote.ShowException(ex);
            pNote.Visible = true;
        }
        #endregion

        #region format
        public static string LeftString(string s, int length, char c)
        {
            if (s.Length < length)
            {
                for (int i = 0; i < length - s.Length; i++)
                {
                    s = c + s;
                }
            }
            return s;
        }
        #endregion

        public static string DateString(string s)
        {
            DateTime _d = Convert.ToDateTime(s);
            return _d.ToString("yyyy-MM-dd");
            
        }

    }//end of class


}
