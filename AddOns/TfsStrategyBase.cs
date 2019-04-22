#region Using declarations
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NinjaTrader.NinjaScript.Strategies;
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
// TF strategy script base. 
//
// 1.0 	FB 2018-08-24 	basic version
//

namespace NinjaTrader.NinjaScript.AddOns.TradeFab
{
    public class TfsStrategyBase : Strategy //, ICustomTypeDescriptor
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
 				Print(GetNow()+"|"+"TRACE|"+Name+"|"+Instrument.FullName+"|"+str);
			}
		}
 		public void Debug(object str) 
		{ 
			if (VerboseLevel <= VerboseLevelType.Debug)
			{
 				Print(GetNow()+"|"+"DEBUG|"+Name+"|"+Instrument.FullName+"|"+str);
			}
		}
 		public void Info(object str) 
		{ 
			if (VerboseLevel <= VerboseLevelType.Info)
			{
 				Print(GetNow()+"|"+"INFO |"+Name+"|"+Instrument.FullName+"|"+str);
			}
		}
 		public void Error(object str) 
		{ 
			if (VerboseLevel <= VerboseLevelType.Error)
			{
 				Print(GetNow()+"|"+"ERROR|"+Name+"|"+Instrument.FullName+"|"+str);
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

        //#region Custom Property Manipulation

        //private void ModifyProperties(PropertyDescriptorCollection col)
        //{
        //    // pls overload!
        //}

        //#endregion

        //#region ICustomTypeDescriptor Members
        
        //public AttributeCollection GetAttributes()
        //{
        //    return TypeDescriptor.GetAttributes(GetType());
        //}

        //public string GetClassName()
        //{
        //    return TypeDescriptor.GetClassName(GetType());
        //}

        //public string GetComponentName()
        //{
        //    return TypeDescriptor.GetComponentName(GetType());
        //}

        //public TypeConverter GetConverter()
        //{
        //    return TypeDescriptor.GetConverter(GetType());
        //}

        //public EventDescriptor GetDefaultEvent()
        //{
        //    return TypeDescriptor.GetDefaultEvent(GetType());
        //}

        //public PropertyDescriptor GetDefaultProperty()
        //{
        //    return TypeDescriptor.GetDefaultProperty(GetType());
        //}

        //public object GetEditor(Type editorBaseType)
        //{
        //    return TypeDescriptor.GetEditor(GetType(), editorBaseType);
        //}

        //public EventDescriptorCollection GetEvents()
        //{
        //    return TypeDescriptor.GetEvents(GetType());
        //}

        //public EventDescriptorCollection GetEvents(Attribute[] attributes)
        //{
        //    return TypeDescriptor.GetEvents(GetType(), attributes);
        //}

        //public PropertyDescriptorCollection GetProperties()
        //{
        //    return TypeDescriptor.GetProperties(GetType());
        //}

        //public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        //{
        //    PropertyDescriptorCollection orig = TypeDescriptor.GetProperties(GetType(), attributes);
        //    PropertyDescriptor[] arr = new PropertyDescriptor[orig.Count];
        //    orig.CopyTo(arr, 0);
        //    PropertyDescriptorCollection col = new PropertyDescriptorCollection(arr);

        //    ModifyProperties(col);
        //    return col;
        //}

        //public object GetPropertyOwner(PropertyDescriptor pd)
        //{
        //    return this;
        //}
        //#endregion
    }
}
