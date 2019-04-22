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
// Bollinger Band with filled area btw UBB and LBB.
//
// 1.0 FB 2018-11-18 basic version: filling area btw UBB and LBB, UBB/LBB are transparent
//
namespace NinjaTrader.NinjaScript.Indicators.TradeFab
{
	/// <summary>
	/// Bollinger Bands are plotted at standard deviation levels above and below a moving average.
	/// Since standard deviation is a measure of volatility, the bands are self-adjusting:
	/// widening during volatile markets and contracting during calmer periods.
	/// </summary>
	public class BollingerFilled : Indicator
	{
		private SMA		sma;
		private StdDev	stdDev;

		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description					= NinjaTrader.Custom.Resource.NinjaScriptIndicatorDescriptionBollinger;
				Name						= "BollingerFilled";
				IsOverlay					= true;
				IsSuspendedWhileInactive	= true;
				NumStdDev					= 2;
				Period						= 14;

				AddPlot(Brushes.Transparent, NinjaTrader.Custom.Resource.BollingerUpperBand);
				AddPlot(Brushes.Azure,       NinjaTrader.Custom.Resource.BollingerMiddleBand);
				AddPlot(Brushes.Transparent, NinjaTrader.Custom.Resource.BollingerLowerBand);
			}
			else if (State == State.DataLoaded)
			{
				sma		= SMA(Period);
				stdDev	= StdDev(Period);
            }
		}

		protected override void OnBarUpdate()
		{
			double sma0		= sma[0];
			double stdDev0	= stdDev[0];

			Upper[0]		= sma0 + NumStdDev * stdDev0;
			Middle[0]		= sma0;
			Lower[0]		= sma0 - NumStdDev * stdDev0;

            // color Bollinger
			Draw.Region(this, "Bollinger", CurrentBar, 0, Upper, Lower, null, Brushes.Blue, 10);
		}

		#region Properties
		[Browsable(false)]
		[XmlIgnore()]
		public Series<double> Upper
		{
			get { return Values[0]; }
		}

		[Browsable(false)]
		[XmlIgnore()]
		public Series<double> Middle
		{
			get { return Values[1]; }
		}

        [Browsable(false)]
		[XmlIgnore()]
		public Series<double> Lower
		{
			get { return Values[2]; }
		}

		[Range(0, int.MaxValue), NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "NumStdDev", GroupName = "NinjaScriptParameters", Order = 0)]
		public double NumStdDev
		{ get; set; }

		[Range(1, int.MaxValue), NinjaScriptProperty]
		[Display(ResourceType = typeof(Custom.Resource), Name = "Period", GroupName = "NinjaScriptParameters", Order = 1)]
		public int Period
		{ get; set; }
		#endregion
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private TradeFab.BollingerFilled[] cacheBollingerFilled;
		public TradeFab.BollingerFilled BollingerFilled(double numStdDev, int period)
		{
			return BollingerFilled(Input, numStdDev, period);
		}

		public TradeFab.BollingerFilled BollingerFilled(ISeries<double> input, double numStdDev, int period)
		{
			if (cacheBollingerFilled != null)
				for (int idx = 0; idx < cacheBollingerFilled.Length; idx++)
					if (cacheBollingerFilled[idx] != null && cacheBollingerFilled[idx].NumStdDev == numStdDev && cacheBollingerFilled[idx].Period == period && cacheBollingerFilled[idx].EqualsInput(input))
						return cacheBollingerFilled[idx];
			return CacheIndicator<TradeFab.BollingerFilled>(new TradeFab.BollingerFilled(){ NumStdDev = numStdDev, Period = period }, input, ref cacheBollingerFilled);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.TradeFab.BollingerFilled BollingerFilled(double numStdDev, int period)
		{
			return indicator.BollingerFilled(Input, numStdDev, period);
		}

		public Indicators.TradeFab.BollingerFilled BollingerFilled(ISeries<double> input , double numStdDev, int period)
		{
			return indicator.BollingerFilled(input, numStdDev, period);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.TradeFab.BollingerFilled BollingerFilled(double numStdDev, int period)
		{
			return indicator.BollingerFilled(Input, numStdDev, period);
		}

		public Indicators.TradeFab.BollingerFilled BollingerFilled(ISeries<double> input , double numStdDev, int period)
		{
			return indicator.BollingerFilled(input, numStdDev, period);
		}
	}
}

#endregion
