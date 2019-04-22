//
// Copyright (C) 2018, TradeFab LLC <www.tradeFab.com>.
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
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
using NinjaTrader.NinjaScript.Indicators.TradeFab;
#endregion

namespace NinjaTrader.NinjaScript.Indicators.TradeFab
{
    /// <summary>
    /// Detects Pin Bars. 
	/// Bearish pin bars have a long tail on top, while bullish pin bars have a long tail at the bottom.
    /// </summary>
	public class PinBar : Indicator
	{
        ////////////////////////////////////////////////////////////////////////////////
        #region Variables

        private	Series<bool>    mBullishPinBar;
        private	Series<bool>    mBearishPinBar;    

		#endregion

		// === PROPERTIES ===
		#region Properties

        /// <summary>
        /// Is Bullish Pin Bar pattern.
        /// </summary>
        public Series<bool> IsBullishPinBar
        { get { return mBullishPinBar; }}

        /// <summary>
        /// Is Bearish Pin Bar pattern.
        /// </summary>
        public Series<bool> IsBearishPinBar
        { get { return mBearishPinBar; }}

		
        #endregion
        ////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// An event driven method which is called whenever the script enters a new State.
        /// </summary>
        protected override
        void OnStateChange()
		{
            // SetDefaults is always called when displaying objects in a UI list such as
            // the Indicators dialogue window since temporary objects are created for the purpose of UI display.
            // Notes:   - Keep as lean as possible
            //          - Set default values (pushed to UI)
            //          - For indicators and strategies: AddPlot() and AddLine()
            //          - PinBar calculation requires calculation at the END OF THE BAR!!! => Calculate = Calculate.OnBarClose;
            //            Scripts that require Calculate to be set by the developer must set this property in State.Historical in its OnStateChange() 
            //            in order to ensure that if this script is a child (hosted) that the parent.Calculate property which is adopted by the child is overridden again.
			if (State == State.SetDefaults)
			{
                Description     = @"NT8 indicator calculating oin bar pattern.";
                Name            = "PinBar";
				Calculate		= Calculate.OnBarClose;
                IsOverlay       = true; // Indicator plots are drawn on the chart panel on top of price

				AddPlot(new Stroke(Brushes.LimeGreen, 2), PlotStyle.Bar, "PatternFound");
			}
            else

            // Configure is called after a user adds an object to the applied list of objects and
            // presses the OK or Apply button. This state is called only once for the life of the object.
            // Notes:   - Add additional data series via AddDataSeries()
            //          - Declare custom resources
            if( State == State.Configure )
            {
                mBullishPinBar = new Series<bool>(this);
                mBearishPinBar = new Series<bool>(this);
            }
		}

        /// <summary>
        /// Called on each bar update event (incoming tick)
        /// </summary>
        protected override
        void OnBarUpdate()
		{
            // set defaults
            mBullishPinBar[0] = false; 
            mBearishPinBar[0] = false; 

            // basic checks
            if (CurrentBar <= 1)
                return;

            // analyze pattern (don't change order!)
		    {
		        bool bullish   = Open[0] < Close[0];
                double lowShdw = bullish?(Open[0]-Low[0]):(Close[0]-Low[0]);
                double uprShdw = bullish?(High[0]-Close[0]):(High[0]-Open[0]);
                double curBody = Math.Abs(Open[0]-Close[0]);

				mBullishPinBar[0] = lowShdw > curBody*2;
				mBearishPinBar[0] = uprShdw > curBody*2;
                //Print("PinBar: bar "+CurrentBar+", result "+mBullishPinBar[0]+"/"+mBearishPinBar[0]);
            }
        }
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private TradeFab.PinBar[] cachePinBar;
		public TradeFab.PinBar PinBar()
		{
			return PinBar(Input);
		}

		public TradeFab.PinBar PinBar(ISeries<double> input)
		{
			if (cachePinBar != null)
				for (int idx = 0; idx < cachePinBar.Length; idx++)
					if (cachePinBar[idx] != null &&  cachePinBar[idx].EqualsInput(input))
						return cachePinBar[idx];
			return CacheIndicator<TradeFab.PinBar>(new TradeFab.PinBar(), input, ref cachePinBar);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.TradeFab.PinBar PinBar()
		{
			return indicator.PinBar(Input);
		}

		public Indicators.TradeFab.PinBar PinBar(ISeries<double> input )
		{
			return indicator.PinBar(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.TradeFab.PinBar PinBar()
		{
			return indicator.PinBar(Input);
		}

		public Indicators.TradeFab.PinBar PinBar(ISeries<double> input )
		{
			return indicator.PinBar(input);
		}
	}
}

#endregion
