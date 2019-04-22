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
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

// www.TradeFab.com
// ___  __        __   __  __       __
//  |  |__)  /\  |  \ |__ |__  /\  |__)
//  |  |  \ /~~\ |__/ |__ |   /~~\ |__)
//
// Previous Day High/Low.
//
// 1.1 FB 2018-04-29 added DayOfYear
// 1.0 FB 2018-04-28 basic version
//

namespace NinjaTrader.NinjaScript.Indicators.TradeFab
{
	public class DayHL : Indicator
	{
		// === VARIABLES ===
		#region Variables

		private	Series<int>	mDayOfYear;

		#endregion

		// === PROPERTIES ===
		#region Properties

        [Description("Previous day high")]
		[Browsable(false)]
		[XmlIgnore]
		public Series<double> DayHigh
		{
			get { return Values[0]; }
		}

        [Description("Previous day low")]
		[Browsable(false)]
		[XmlIgnore]
		public Series<double> DayLow
		{
			get { return Values[1]; }
		}

		[Description("Previous day of year")]
		[Browsable(false)]
		[XmlIgnore]
		public Series<int> DayOfYear
		{
			get { return mDayOfYear; }
		}

        [Description("Number of bars in current day")]
		[Browsable(false)]
		[XmlIgnore]
		public int BarsInDay
		{
			get { return Bars.BarsSinceNewTradingDay; }
		}

		#endregion

		// === FUNCTIONS ===
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Draw the previous day High/Low.";
				Name										= "DayHL";
				Calculate									= Calculate.OnBarClose;
				IsOverlay									= true;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= true;
				DrawVerticalGridLines						= true;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;

				AddPlot(new Stroke(Brushes.SeaGreen,DashStyleHelper.Dash, 2), PlotStyle.Square, "DayHigh");
				AddPlot(new Stroke(Brushes.Red,		DashStyleHelper.Dash, 2), PlotStyle.Square, "DayLow");
			}
			else if (State == State.Configure)
			{
				// add 1day data seriess
				AddDataSeries(Data.BarsPeriodType.Day, 1);
			}
			else if (State == State.DataLoaded)
			{
				mDayOfYear = new Series<int>(this);
			}
			else if (State == State.Historical)
			{
				if (!Bars.BarsType.IsIntraday)
				{
					Draw.TextFixed(this, "NinjaScriptInfo", Name+" only works on intraday intervals", TextPosition.BottomRight);
				}
			}
		}

		protected override void OnBarUpdate()
		{
			if (!Bars.BarsType.IsIntraday) 
				return;

			if (CurrentBars[1] < 1)
				return;

			DayHigh[0] 	= Highs[1][0];
			DayLow[0] 	= Lows[1][0];
			DayOfYear[0]= Times[1][0].Date.DayOfYear;
		}
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private TradeFab.DayHL[] cacheDayHL;
		public TradeFab.DayHL DayHL()
		{
			return DayHL(Input);
		}

		public TradeFab.DayHL DayHL(ISeries<double> input)
		{
			if (cacheDayHL != null)
				for (int idx = 0; idx < cacheDayHL.Length; idx++)
					if (cacheDayHL[idx] != null &&  cacheDayHL[idx].EqualsInput(input))
						return cacheDayHL[idx];
			return CacheIndicator<TradeFab.DayHL>(new TradeFab.DayHL(), input, ref cacheDayHL);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.TradeFab.DayHL DayHL()
		{
			return indicator.DayHL(Input);
		}

		public Indicators.TradeFab.DayHL DayHL(ISeries<double> input )
		{
			return indicator.DayHL(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.TradeFab.DayHL DayHL()
		{
			return indicator.DayHL(Input);
		}

		public Indicators.TradeFab.DayHL DayHL(ISeries<double> input )
		{
			return indicator.DayHL(input);
		}
	}
}

#endregion
