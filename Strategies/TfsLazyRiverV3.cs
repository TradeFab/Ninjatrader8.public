// www.TradeFab.com
// ___  __        __   __  __       __
//  |  |__)  /\  |  \ |__ |__  /\  |__)
//  |  |  \ /~~\ |__/ |__ |   /~~\ |__)
//
// TradeFab's Lazy River Strategy.
// The strategy uses EMA(20) and EMA(50) and looks for a constant trend
// and trades if price is bouncing back from the EMAs.
//
//VERSION = "3.2"
// 3.2 FB 2017-01-23 second exit if leaving trend zone
// 3.1 FB 2017-01-23 added riskFactor; optionally could use fix offset, e.g. 200!
// 3.0 FB 2017-01-22 based on V1.3, added SMA(200), histPeriod=14
//

#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.Indicators;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Strategies in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Strategies.TradeFab
{
	public class TfsLazyRiverV3 : Strategy
	{
		private EMA maFast;
		private EMA maSlow;
		private SMA maLong;
		private DonchianChannel dc;

		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Lazy River Strategy based on TradingView strategy.";
				Name										= "TfsLazyRiverV3";
				Calculate									= Calculate.OnBarClose;
				EntriesPerDirection							= 1;
				EntryHandling								= EntryHandling.AllEntries;
				IsExitOnSessionCloseStrategy				= true;
				ExitOnSessionCloseSeconds					= 30;
				IsFillLimitOnTouch							= false;
				MaximumBarsLookBack							= MaximumBarsLookBack.TwoHundredFiftySix;
				OrderFillResolution							= OrderFillResolution.Standard;
				Slippage									= 0;
				StartBehavior								= StartBehavior.WaitUntilFlat;
				TimeInForce									= TimeInForce.Gtc;
				TraceOrders									= false;
				RealtimeErrorHandling						= RealtimeErrorHandling.StopCancelClose;
				StopTargetHandling							= StopTargetHandling.PerEntryExecution;
				BarsRequiredToTrade							= 20;
				// Disable this property for performance gains in Strategy Analyzer optimizations
				// See the Help Guide for additional information
				IsInstantiatedOnEachOptimizationIteration	= true;
				MaFastPeriod					= 20;
				MaSlowPeriod					= 50;
				MaLongPeriod					= 200;
				DcPeriod					= 14;
				RiskFactor					= 1;
				TradeLong					= true;
				TradeShort					= false;
			}
			else if (State == State.Configure)
			{
			}
			else if (State == State.DataLoaded)
			{
				maFast = EMA(MaFastPeriod);
				maSlow = EMA(MaSlowPeriod);
				maLong = SMA(MaLongPeriod);
				dc     = DonchianChannel(DcPeriod);

				maFast.Plots[0].Brush = Brushes.SkyBlue;
				maSlow.Plots[0].Brush = Brushes.DodgerBlue;
				maLong.Plots[0].Brush = Brushes.Blue;
				dc.Plots[0].Brush = Brushes.Beige;

				AddChartIndicator(maFast);
				AddChartIndicator(maSlow);
				AddChartIndicator(maLong);
				AddChartIndicator(dc);
			}
		}

		protected override void OnBarUpdate()
		{
            // check conditions for trade
			if( CurrentBar < BarsRequiredToTrade )
				return;
			
			if (isUpTrend())
			{
				BackBrush = Brushes.Lime;
				Brush newBrush = BackBrush.Clone();
				newBrush.Opacity = 0.25;
				newBrush.Freeze();
				BackBrush = newBrush;
			}
			else
			if (isDownTrend())
			{
				BackBrush = Brushes.HotPink;
				Brush newBrush = BackBrush.Clone();
				newBrush.Opacity = 0.25;
				newBrush.Freeze();
				BackBrush = newBrush;
			}
			
            if ((Position.MarketPosition == MarketPosition.Flat) &&
				isUpTrend() && TradeLong)
            {
				var stopLoss = (Close[0]-dc.Lower[0]) * RiskFactor;
				var stopPrice = Close[0] - stopLoss; 
				var limitPrice = Close[0] + stopLoss;
				EnterLong(10000, "Long");
				ExitLongLimit(5000, limitPrice, "L-EX1", "Long");
				ExitLongStopMarket(stopPrice, "L-SL", "Long");
			}	
            if ((Position.MarketPosition == MarketPosition.Long) &&
				isDownTrend())
            {
				ExitLong("Long");
			}
		}

		bool isUpTrend()
		{
			if ((maFast[0] > maSlow[0]) &&
   				(maSlow[0] > maLong[0]) &&
   				IsRising(maFast) &&
   				IsRising(maSlow))
				return true;
			return false;
		}

		bool isDownTrend()
		{
			if ((maFast[0] < maSlow[0]) &&
   				(maSlow[0] < maLong[0]) &&
   				IsFalling(maFast) &&
   				IsFalling(maSlow))
				return true;
			return false;
		}
		
		#region Properties
		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="MaFastPeriod", Order=1, GroupName="Parameters")]
		public int MaFastPeriod
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="MaSlowPeriod", Order=2, GroupName="Parameters")]
		public int MaSlowPeriod
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="MaLongPeriod", Order=3, GroupName="Parameters")]
		public int MaLongPeriod
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="DcPeriod", Order=4, GroupName="Parameters")]
		public int DcPeriod
		{ get; set; }

		[NinjaScriptProperty]
		[Range(1, int.MaxValue)]
		[Display(Name="RiskFactor", Order=5, GroupName="Parameters")]
		public int RiskFactor
		{ get; set; }

		[NinjaScriptProperty]
		[Display(Name="TradeLong", Order=6, GroupName="Parameters")]
		public bool TradeLong
		{ get; set; }

		[NinjaScriptProperty]
		[Display(Name="TradeShort", Order=7, GroupName="Parameters")]
		public bool TradeShort
		{ get; set; }

		[Browsable(false)]
		[XmlIgnore]
		public Series<double> MaFast
		{
			get { return Values[0]; }
		}

		[Browsable(false)]
		[XmlIgnore]
		public Series<double> MaSlow
		{
			get { return Values[1]; }
		}
		#endregion

	}
}
