﻿using System;
using System.Globalization;

namespace YahooFinanceApi
{
    static class RowExtension
    {
        internal static bool IgnoreEmptyRows;

        public static Candle ToCandle(this string[] row)
        {
            var candle = new Candle
            {
                DateTime      = row[0].ToDateTime(),
                Open          = row[1].ToDecimal(),
                High          = row[2].ToDecimal(),
                Low           = row[3].ToDecimal(),
                Close         = row[4].ToDecimal(),
                AdjustedClose = row[5].ToDecimal(),
                Volume        = row[6].ToInt64()
            };

            if (IgnoreEmptyRows &&
                candle.Open == 0 && candle.High == 0 && candle.Low == 0 && candle.Close == 0 &&
                candle.AdjustedClose == 0 &&  candle.Volume == 0)
                return null;

            return candle;
        }

        public static DividendTick ToDividendTick(this string[] row)
        {
            var tick = new DividendTick
            {
                DateTime = row[0].ToDateTime(),
                Dividend = row[1].ToDecimal()
            };

            if (IgnoreEmptyRows && tick.Dividend == 0)
                return null;

            return tick;
        }

        public static SplitTick ToSplitTick(this string[] row)
        {
            var tick = new SplitTick { DateTime = row[0].ToDateTime() };

            var split = row[1].Split('/');
            if (split.Length == 2)
            {
                tick.AfterSplit  = split[0].ToDecimal();
                tick.BeforeSplit = split[1].ToDecimal();
            }

            if (IgnoreEmptyRows && tick.AfterSplit == 0 && tick.BeforeSplit == 0)
                return null;

            return tick;
        }

        private static DateTime ToDateTime(this string str)
            => DateTime.Parse(str, CultureInfo.InvariantCulture);

        private static Decimal ToDecimal(this string str)
        {
            Decimal.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out Decimal result);
            return result;
        }

        private static Int64 ToInt64(this string str)
        {
            Int64.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out Int64 result);
            return result;
        }
    }
}
