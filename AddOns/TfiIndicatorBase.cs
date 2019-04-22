#region Using declarations
using System;
using System.ComponentModel.DataAnnotations;
using NinjaTrader.NinjaScript.Indicators;
#endregion

// www.TradeFab.com
// ___  __        __   __  __       __
//  |  |__)  /\  |  \ |__ |__  /\  |__)
//  |  |  \ /~~\ |__/ |__ |   /~~\ |__)
//
// DISCLAIMER: Futures, stocks and options trading involves substantial risk of loss 
// and is not suitable for every investor. You are responsible for all the risks and 
// financial resources you use and for the chosen trading system.
// Past performance is not indicative for future results. In making an investment decision,
// traders must rely on their own examination of the entity making the trading decisions!
//
// TF indicator script base. 
//
// 1.0 	FB 2019-01-22 	basic version
//

namespace NinjaTrader.NinjaScript.AddOns.TradeFab
{
    public class TfiIndicatorBase : Indicator
	{
		// === VARIABLES ===
		#region Variables

        public enum VerboseLevelType
        {
            Trace,
            Debug,
            Info,
            Error,
        }
		#endregion

		// === PROPERTIES ===
		#region Properties

		[NinjaScriptProperty]
        [Display(Name = "Verbose Level", Order=0, GroupName = "Debug")]
        public VerboseLevelType VerboseLevel 
		{ get; set; }

		#endregion
		
		// === FUNCTIONS ===
		#region Functions

 		public void Trace(object str) 
		{ 
			if (VerboseLevel <= VerboseLevelType.Trace)
			{
 				Print(GetNow()+"|"+"TRACE|"+Name+"|"+str);
			}
		}
 		public void Debug(object str) 
		{ 
			if (VerboseLevel <= VerboseLevelType.Debug)
			{
 				Print(GetNow()+"|"+"DEBUG|"+Name+"|"+str);
			}
		}
 		public void Info(object str) 
		{ 
			if (VerboseLevel <= VerboseLevelType.Info)
			{
 				Print(GetNow()+"|"+"INFO |"+Name+"|"+str);
			}
		}
 		public void Error(object str) 
		{ 
			if (VerboseLevel <= VerboseLevelType.Error)
			{
 				Print(GetNow()+"|"+"ERROR|"+Name+"|"+str);
			}
		}
 		
        public string GetNow()
        {
            return DateTime.Now.ToString("MM/dd/yy HH:mm:ss.fff");
        }

        public string GetBarTime()
        {
            return Time[0].ToString("MM/dd/yy HH:mm");
        }
		#endregion
	}
}

