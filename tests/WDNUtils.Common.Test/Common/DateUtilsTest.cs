using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.Threading;

namespace WDNUtils.Common.Test
{
    [TestClass]
    public class DateUtilsTest
    {
        #region Date/time to string conversion

        [TestMethod]
        public void TestDateUtilsToString()
        {
            Assert.IsNull(DateUtils.ToStringEx(value: null, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.IsNull(DateUtils.ToStringEx(value: null, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: null, minimumDateIsNull: true, useInvariantCulture: null));
            Assert.IsNotNull(DateUtils.ToStringEx(value: DateTime.MinValue, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.IsNull(DateUtils.ToStringEx(value: DateTime.MinValue, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: null, minimumDateIsNull: true, useInvariantCulture: null));

            Assert.AreEqual(@"NULL", DateUtils.ToStringEx(value: null, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: @"NULL", minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual(@"NULL", DateUtils.ToStringEx(value: null, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: @"NULL", minimumDateIsNull: true, useInvariantCulture: null));
            Assert.AreNotEqual(@"NULL", DateUtils.ToStringEx(value: DateTime.MinValue, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: @"NULL", minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual(@"NULL", DateUtils.ToStringEx(value: DateTime.MinValue, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: @"NULL", minimumDateIsNull: true, useInvariantCulture: null));

            var oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(@"de-DE");

            var value = new DateTimeOffset(1992, 2, 29, 12, 34, 56, 789, TimeSpan.Zero).UtcDateTime;

            var hour = (value + TimeZoneInfo.Local.GetUtcOffset(value)).Hour.ToString(@"00");
            var minute = (value + TimeZoneInfo.Local.GetUtcOffset(value)).Minute.ToString(@"00");

            var offset = (TimeZoneInfo.Local.GetUtcOffset(value) == TimeSpan.Zero) ? @"Z" :
                string.Concat(TimeZoneInfo.Local.GetUtcOffset(value).Hours.ToString(@"00"), ":", TimeZoneInfo.Local.GetUtcOffset(value).Minutes.ToString(@"00"));

            Assert.AreEqual(@"29.02.1992", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDate, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual(@"29.02.1992", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDate, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual(@"02/29/1992", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDate, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));
            Assert.AreEqual(@"12:34:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureTime, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual(@"12:34", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureTimeWithoutSeconds, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual(@"29.02.1992 12:34:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTime, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual(@"29.02.1992 12:34:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTime, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual(@"02/29/1992 12:34:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTime, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));
            Assert.AreEqual(@"29.02.1992 12:34", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTimeWithoutSeconds, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual(@"29.02.1992 12:34", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTimeWithoutSeconds, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual(@"02/29/1992 12:34", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTimeWithoutSeconds, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));
            Assert.AreEqual(@"1992-02-29T12:34:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.ISOShort, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual(@"1992-02-29T12:34:56.7890000Z", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.ISOLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual(@"1992-02-29T12:34:56.7890000Z", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.ISOLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual(@"1992-02-29T12:34:56.7890000Z", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.ISOLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));
            Assert.AreEqual(@"1992-02-29 12:34:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomShort, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual(@"1992-02-29 12:34:56.7890000", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual(@"1992-02-29 12:34:56.7890000", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual(@"1992-02-29 12:34:56.7890000", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));
            Assert.AreEqual(@"1992-02-29 12:34:56.7890000Z", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual(@"1992-02-29 12:34:56.7890000Z", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual(@"1992-02-29 12:34:56.7890000Z", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));

            value = TimeZoneInfo.ConvertTimeFromUtc(value, TimeZoneInfo.Local);

            Assert.AreEqual(@"29.02.1992", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDate, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual(@"29.02.1992", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDate, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual(@"02/29/1992", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDate, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));
            Assert.AreEqual($@"{hour}:{minute}:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureTime, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"{hour}:{minute}", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureTimeWithoutSeconds, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"29.02.1992 {hour}:{minute}:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTime, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"29.02.1992 {hour}:{minute}:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTime, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual($@"02/29/1992 {hour}:{minute}:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTime, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));
            Assert.AreEqual($@"29.02.1992 {hour}:{minute}", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTimeWithoutSeconds, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"29.02.1992 {hour}:{minute}", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTimeWithoutSeconds, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual($@"02/29/1992 {hour}:{minute}", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTimeWithoutSeconds, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));
            Assert.AreEqual($@"1992-02-29T{hour}:{minute}:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.ISOShort, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"1992-02-29T{hour}:{minute}:56.7890000{offset}", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.ISOLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"1992-02-29T{hour}:{minute}:56.7890000{offset}", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.ISOLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual($@"1992-02-29T{hour}:{minute}:56.7890000{offset}", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.ISOLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));
            Assert.AreEqual($@"1992-02-29 {hour}:{minute}:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomShort, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"1992-02-29 {hour}:{minute}:56.7890000", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"1992-02-29 {hour}:{minute}:56.7890000", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual($@"1992-02-29 {hour}:{minute}:56.7890000", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));
            Assert.AreEqual($@"1992-02-29 {hour}:{minute}:56.7890000{offset}", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"1992-02-29 {hour}:{minute}:56.7890000{offset}", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual($@"1992-02-29 {hour}:{minute}:56.7890000{offset}", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));

            value = DateTime.SpecifyKind(value, DateTimeKind.Unspecified);

            Assert.AreEqual(@"29.02.1992", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDate, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual(@"29.02.1992", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDate, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual(@"02/29/1992", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDate, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));
            Assert.AreEqual($@"{hour}:{minute}:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureTime, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"{hour}:{minute}", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureTimeWithoutSeconds, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"29.02.1992 {hour}:{minute}:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTime, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"29.02.1992 {hour}:{minute}:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTime, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual($@"02/29/1992 {hour}:{minute}:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTime, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));
            Assert.AreEqual($@"29.02.1992 {hour}:{minute}", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTimeWithoutSeconds, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"29.02.1992 {hour}:{minute}", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTimeWithoutSeconds, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual($@"02/29/1992 {hour}:{minute}", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CultureDateTimeWithoutSeconds, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));
            Assert.AreEqual($@"1992-02-29T{hour}:{minute}:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.ISOShort, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"1992-02-29T{hour}:{minute}:56.7890000", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.ISOLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"1992-02-29T{hour}:{minute}:56.7890000", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.ISOLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual($@"1992-02-29T{hour}:{minute}:56.7890000", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.ISOLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));
            Assert.AreEqual($@"1992-02-29 {hour}:{minute}:56", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomShort, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"1992-02-29 {hour}:{minute}:56.7890000", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"1992-02-29 {hour}:{minute}:56.7890000", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual($@"1992-02-29 {hour}:{minute}:56.7890000", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLocal, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));
            Assert.AreEqual($@"1992-02-29 {hour}:{minute}:56.7890000", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: null));
            Assert.AreEqual($@"1992-02-29 {hour}:{minute}:56.7890000", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: false));
            Assert.AreEqual($@"1992-02-29 {hour}:{minute}:56.7890000", DateUtils.ToStringEx(value: value, format: DateUtils.DateTimeFormat.CustomLong, nullValue: null, minimumDateIsNull: false, useInvariantCulture: true));

            Thread.CurrentThread.CurrentCulture = oldCulture;
        }

        [TestMethod]
        public void TestDateUtilsTimeSpanToString()
        {
            var oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(@"de-DE");

            Assert.IsNull(DateUtils.ToStringEx(value: (TimeSpan?)null, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.IsNull(DateUtils.ToStringEx(value: (TimeSpan?)null, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.IsNull(DateUtils.ToStringEx(value: (TimeSpan?)null, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.IsNull(DateUtils.ToStringEx(value: (TimeSpan?)null, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.IsNull(DateUtils.ToStringEx(value: (TimeSpan?)null, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.IsNull(DateUtils.ToStringEx(value: (TimeSpan?)null, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.IsNull(DateUtils.ToStringEx(value: (TimeSpan?)null, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));

            Assert.AreEqual(@"NULL", DateUtils.ToStringEx(value: (TimeSpan?)null, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: @"NULL"));
            Assert.AreEqual(@"NULL", DateUtils.ToStringEx(value: (TimeSpan?)null, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: @"NULL"));
            Assert.AreEqual(@"NULL", DateUtils.ToStringEx(value: (TimeSpan?)null, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: @"NULL"));
            Assert.AreEqual(@"NULL", DateUtils.ToStringEx(value: (TimeSpan?)null, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: @"NULL"));
            Assert.AreEqual(@"NULL", DateUtils.ToStringEx(value: (TimeSpan?)null, format: DateUtils.TimeSpanFormat.Short, nullValue: @"NULL", useInvariantCulture: null));
            Assert.AreEqual(@"NULL", DateUtils.ToStringEx(value: (TimeSpan?)null, format: DateUtils.TimeSpanFormat.Long, nullValue: @"NULL", useInvariantCulture: null));
            Assert.AreEqual(@"NULL", DateUtils.ToStringEx(value: (TimeSpan?)null, format: DateUtils.TimeSpanFormat.Invariant, nullValue: @"NULL", useInvariantCulture: null));

            var value = TimeSpan.Zero;

            Assert.AreEqual(@"0:00:00", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"0:00:00", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"0:00:00", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"0:00:00:00,0000000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"0:00:00:00,0000000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"0:00:00:00.0000000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:00:00", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"00:00:00", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"00:00:00", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00:00", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00:00", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));

            value = TimeSpan.FromMilliseconds(1);

            Assert.AreEqual(@"0:00:00,001", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"0:00:00,001", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"0:00:00.001", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"0:00:00:00,0010000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"0:00:00:00,0010000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"0:00:00:00.0010000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:00:00.0010000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"00:00:00.0010000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"00:00:00.0010000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00:00", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00:00", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            value = value.Negate();
            Assert.AreEqual(@"-0:00:00,001", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-0:00:00,001", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-0:00:00.001", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-0:00:00:00,0010000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-0:00:00:00,0010000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-0:00:00:00.0010000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-00:00:00.0010000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-00:00:00.0010000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-00:00:00.0010000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00:00", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00:00", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));

            value = TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(@"0:00:00,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"0:00:00,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"0:00:00.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"0:00:00:00,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"0:00:00:00,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"0:00:00:00.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:00:00.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"00:00:00.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"00:00:00.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00:00", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00:00", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            value = value.Negate();
            Assert.AreEqual(@"-0:00:00,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-0:00:00,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-0:00:00.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-0:00:00:00,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-0:00:00:00,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-0:00:00:00.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-00:00:00.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-00:00:00.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-00:00:00.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00:00", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00:00", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));

            value = TimeSpan.FromSeconds(1) + TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(@"0:00:01,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"0:00:01,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"0:00:01.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"0:00:00:01,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"0:00:00:01,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"0:00:00:01.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:00:01.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"00:00:01.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"00:00:01.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00:01", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00:01", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            value = value.Negate();
            Assert.AreEqual(@"-0:00:01,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-0:00:01,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-0:00:01.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-0:00:00:01,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-0:00:00:01,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-0:00:00:01.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-00:00:01.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-00:00:01.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-00:00:01.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-00:00:01", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-00:00:01", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));

            value = TimeSpan.FromSeconds(59) + TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(@"0:00:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"0:00:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"0:00:59.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"0:00:00:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"0:00:00:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"0:00:00:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:00:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"00:00:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"00:00:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            value = value.Negate();
            Assert.AreEqual(@"-0:00:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-0:00:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-0:00:59.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-0:00:00:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-0:00:00:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-0:00:00:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-00:00:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-00:00:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-00:00:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-00:00:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:00", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-00:00:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));

            value = TimeSpan.FromMinutes(1) + TimeSpan.FromSeconds(59) + TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(@"0:01:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"0:01:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"0:01:59.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"0:00:01:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"0:00:01:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"0:00:01:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:01:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"00:01:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"00:01:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:01", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:01:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:01", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:01:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            value = value.Negate();
            Assert.AreEqual(@"-0:01:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-0:01:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-0:01:59.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-0:00:01:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-0:00:01:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-0:00:01:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-00:01:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-00:01:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-00:01:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-00:01", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-00:01:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-00:01", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-00:01:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));

            value = TimeSpan.FromMinutes(59) + TimeSpan.FromSeconds(59) + TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(@"0:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"0:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"0:59:59.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"0:00:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"0:00:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"0:00:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"00:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"00:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"00:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:59:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"00:59:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            value = value.Negate();
            Assert.AreEqual(@"-0:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-0:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-0:59:59.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-0:00:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-0:00:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-0:00:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-00:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-00:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-00:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-00:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-00:59:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-00:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-00:59:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));

            value = TimeSpan.FromHours(1) + TimeSpan.FromMinutes(59) + TimeSpan.FromSeconds(59) + TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(@"1:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"1:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"1:59:59.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"0:01:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"0:01:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"0:01:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"01:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"01:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"01:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"01:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"01:59:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"01:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"01:59:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            value = value.Negate();
            Assert.AreEqual(@"-1:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-1:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-1:59:59.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-0:01:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-0:01:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-0:01:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-01:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-01:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-01:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-01:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-01:59:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-01:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-01:59:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));

            value = TimeSpan.FromHours(23) + TimeSpan.FromMinutes(59) + TimeSpan.FromSeconds(59) + TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(@"23:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"23:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"23:59:59.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"0:23:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"0:23:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"0:23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"23:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"23:59:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"23:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"23:59:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            value = value.Negate();
            Assert.AreEqual(@"-23:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-23:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-23:59:59.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-0:23:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-0:23:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-0:23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-23:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-23:59:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-23:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-23:59:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));

            value = TimeSpan.FromDays(1) + TimeSpan.FromHours(23) + TimeSpan.FromMinutes(59) + TimeSpan.FromSeconds(59) + TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(@"1:23:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"1:23:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"1:23:59:59.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"1:23:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"1:23:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"1:23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"1.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"1.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"1.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual($@"{1 * 24 + 23}:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual($@"{1 * 24 + 23}:59:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"1d23:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"1d23:59:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            value = value.Negate();
            Assert.AreEqual(@"-1:23:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-1:23:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-1:23:59:59.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-1:23:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-1:23:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-1:23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-1.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-1.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-1.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual($@"-{1 * 24 + 23}:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual($@"-{1 * 24 + 23}:59:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-1d23:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-1d23:59:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));

            value = TimeSpan.FromDays(99) + TimeSpan.FromHours(23) + TimeSpan.FromMinutes(59) + TimeSpan.FromSeconds(59) + TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(@"99:23:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"99:23:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"99:23:59:59.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"99:23:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"99:23:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"99:23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"99.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"99.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"99.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual($@"{99 * 24 + 23}:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual($@"{99 * 24 + 23}:59:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"99d23:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"99d23:59:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            value = value.Negate();
            Assert.AreEqual(@"-99:23:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-99:23:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-99:23:59:59.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-99:23:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-99:23:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-99:23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-99.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-99.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-99.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual($@"-{99 * 24 + 23}:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual($@"-{99 * 24 + 23}:59:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-99d23:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-99d23:59:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));

            value = TimeSpan.FromDays(99999) + TimeSpan.FromHours(23) + TimeSpan.FromMinutes(59) + TimeSpan.FromSeconds(59) + TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(@"99999:23:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"99999:23:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"99999:23:59:59.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"99999:23:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"99999:23:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"99999:23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"99999.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"99999.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"99999.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual($@"{99999 * 24 + 23}:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual($@"{99999 * 24 + 23}:59:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"99999d23:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"99999d23:59:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            value = value.Negate();
            Assert.AreEqual(@"-99999:23:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-99999:23:59:59,999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-99999:23:59:59.999", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Short, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-99999:23:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-99999:23:59:59,9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-99999:23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Long, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual(@"-99999.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: null));
            Assert.AreEqual(@"-99999.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: false));
            Assert.AreEqual(@"-99999.23:59:59.9990000", DateUtils.ToStringEx(value: value, format: DateUtils.TimeSpanFormat.Invariant, nullValue: null, useInvariantCulture: true));
            Assert.AreEqual($@"-{99999 * 24 + 23}:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual($@"-{99999 * 24 + 23}:59:59", DateUtils.ToStringEx(value, displayDays: false, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-99999d23:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: false, dayTimeSeparator: "d", nullValue: null));
            Assert.AreEqual(@"-99999d23:59:59", DateUtils.ToStringEx(value, displayDays: true, displaySeconds: true, dayTimeSeparator: "d", nullValue: null));

            Thread.CurrentThread.CurrentCulture = oldCulture;
        }

        #endregion

        #region String to date/time conversion

        [TestMethod]
        public void TestDateUtilsStringToDateTime()
        {
            var oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(@"de-DE");

            var cultureInfo = new CultureInfo(@"pt-BR");

            Assert.IsNull(DateUtils.ToDateTime(value: null, useInvariantCulture: false, nullValue: null));
            Assert.IsNull(DateUtils.ToDateTime(value: null, useInvariantCulture: true, nullValue: null));
            Assert.IsNull(DateUtils.ToDateTime(value: null, cultureInfo: cultureInfo, nullValue: null));
            Assert.IsNull(DateUtils.ToDateTime(value: @"NULL", useInvariantCulture: false, nullValue: @"NULL"));
            Assert.IsNull(DateUtils.ToDateTime(value: @"NULL", useInvariantCulture: true, nullValue: @"NULL"));
            Assert.IsNull(DateUtils.ToDateTime(value: @"NULL", cultureInfo: cultureInfo, nullValue: @"NULL"));

            try
            {
                DateUtils.ToDateTime(value: @"NULL", useInvariantCulture: false, nullValue: null);
                Assert.Fail();
            }
            catch (FormatException)
            {
                // OK
            }

            try
            {
                DateUtils.ToDateTime(value: @"NULL", useInvariantCulture: true, nullValue: null);
                Assert.Fail();
            }
            catch (FormatException)
            {
                // OK
            }

            try
            {
                DateUtils.ToDateTime(value: @"NULL", cultureInfo: cultureInfo, nullValue: null);
                Assert.Fail();
            }
            catch (FormatException)
            {
                // OK
            }

            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"1990-1-2 03:04:05.6780000", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"1990-1-2 03:04:05.6780000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"1990-1-2 03:04:05.6780000", cultureInfo: cultureInfo, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"1990-1-2 03:04:05.678", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"1990-1-2 03:04:05.678", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"1990-1-2 03:04:05.678", cultureInfo: cultureInfo, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 0), DateUtils.ToDateTime(value: @"1990-1-2 03:04:05", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 0), DateUtils.ToDateTime(value: @"1990-1-2 03:04:05", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 0), DateUtils.ToDateTime(value: @"1990-1-2 03:04:05", cultureInfo: cultureInfo, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 0, 0), DateUtils.ToDateTime(value: @"1990-1-2 03:04", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 0, 0), DateUtils.ToDateTime(value: @"1990-1-2 03:04", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 0, 0), DateUtils.ToDateTime(value: @"1990-1-2 03:04", cultureInfo: cultureInfo, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 0, 0, 0, 0), DateUtils.ToDateTime(value: @"1990-1-2", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 0, 0, 0, 0), DateUtils.ToDateTime(value: @"1990-1-2", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 0, 0, 0, 0), DateUtils.ToDateTime(value: @"1990-1-2", cultureInfo: cultureInfo, nullValue: null));

            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"01/02/1990 03:04:05.6780000", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"02.01.1990 03:04:05.6780000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"02/01/1990 03:04:05.6780000", cultureInfo: cultureInfo, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"01/02/1990 03:04:05.678", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"02.01.1990 03:04:05.678", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"02/01/1990 03:04:05.678", cultureInfo: cultureInfo, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 0), DateUtils.ToDateTime(value: @"01/02/1990 03:04:05", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 0), DateUtils.ToDateTime(value: @"02.01.1990 03:04:05", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 0), DateUtils.ToDateTime(value: @"02/01/1990 03:04:05", cultureInfo: cultureInfo, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 0, 0), DateUtils.ToDateTime(value: @"01/02/1990 03:04", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 0, 0), DateUtils.ToDateTime(value: @"02.01.1990 03:04", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 0, 0), DateUtils.ToDateTime(value: @"02/01/1990 03:04", cultureInfo: cultureInfo, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 0, 0, 0, 0), DateUtils.ToDateTime(value: @"01/02/1990", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 0, 0, 0, 0), DateUtils.ToDateTime(value: @"02.01.1990", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 0, 0, 0, 0), DateUtils.ToDateTime(value: @"02/01/1990", cultureInfo: cultureInfo, nullValue: null));

            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"1/2/90 03:04:05.6780000", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"2.1.90 03:04:05.6780000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"2/1/90 03:04:05.6780000", cultureInfo: cultureInfo, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"1/2/90 03:04:05.678", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"2.1.90 03:04:05.678", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 678), DateUtils.ToDateTime(value: @"2/1/90 03:04:05.678", cultureInfo: cultureInfo, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 0), DateUtils.ToDateTime(value: @"1/2/90 03:04:05", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 0), DateUtils.ToDateTime(value: @"2.1.90 03:04:05", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 5, 0), DateUtils.ToDateTime(value: @"2/1/90 03:04:05", cultureInfo: cultureInfo, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 0, 0), DateUtils.ToDateTime(value: @"1/2/90 03:04", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 0, 0), DateUtils.ToDateTime(value: @"2.1.90 03:04", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 3, 4, 0, 0), DateUtils.ToDateTime(value: @"2/1/90 03:04", cultureInfo: cultureInfo, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 0, 0, 0, 0), DateUtils.ToDateTime(value: @"1/2/90", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 0, 0, 0, 0), DateUtils.ToDateTime(value: @"2.1.90", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(new DateTime(1990, 1, 2, 0, 0, 0, 0), DateUtils.ToDateTime(value: @"2/1/90", cultureInfo: cultureInfo, nullValue: null));

            Thread.CurrentThread.CurrentCulture = oldCulture;
        }

        [TestMethod]
        public void TestDateUtilsStringToTimeSpan()
        {
            var oldCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo(@"de-DE");

            var cultureInfoDot = new CultureInfo(@"en-US");
            var cultureInfoComma = new CultureInfo(@"pt-BR");

            Assert.IsNull(DateUtils.ToDateTime(value: null, useInvariantCulture: false, nullValue: null));
            Assert.IsNull(DateUtils.ToDateTime(value: null, useInvariantCulture: true, nullValue: null));
            Assert.IsNull(DateUtils.ToDateTime(value: null, cultureInfo: cultureInfoDot, nullValue: null));
            Assert.IsNull(DateUtils.ToDateTime(value: null, cultureInfo: cultureInfoComma, nullValue: null));
            Assert.IsNull(DateUtils.ToDateTime(value: @"NULL", useInvariantCulture: false, nullValue: @"NULL"));
            Assert.IsNull(DateUtils.ToDateTime(value: @"NULL", useInvariantCulture: true, nullValue: @"NULL"));
            Assert.IsNull(DateUtils.ToDateTime(value: @"NULL", cultureInfo: cultureInfoDot, nullValue: @"NULL"));
            Assert.IsNull(DateUtils.ToDateTime(value: @"NULL", cultureInfo: cultureInfoComma, nullValue: @"NULL"));

            try
            {
                DateUtils.ToTimeSpan(value: @"NULL", useInvariantCulture: false, nullValue: null);
                Assert.Fail();
            }
            catch (FormatException)
            {
                // OK
            }

            try
            {
                DateUtils.ToTimeSpan(value: @"NULL", useInvariantCulture: true, nullValue: null);
                Assert.Fail();
            }
            catch (FormatException)
            {
                // OK
            }

            try
            {
                DateUtils.ToTimeSpan(value: @"NULL", cultureInfo: cultureInfoComma, nullValue: null);
                Assert.Fail();
            }
            catch (FormatException)
            {
                // OK
            }

            var value = TimeSpan.Zero;

            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:00.0000000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:00,0000000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:00,0000000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:00.0000000", useInvariantCulture: true, nullValue: null));

            value = TimeSpan.FromMilliseconds(1);

            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00.001", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00,001", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00,001", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00.001", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:00.0010000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:00,0010000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:00,0010000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:00.0010000", useInvariantCulture: true, nullValue: null));
            value = value.Negate();
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00.001", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00,001", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00,001", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00.001", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00:00.0010000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00:00,0010000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00:00,0010000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00:00.0010000", useInvariantCulture: true, nullValue: null));

            value = TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00.999", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00,999", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00,999", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00.999", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:00.9990000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:00,9990000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:00,9990000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:00.9990000", useInvariantCulture: true, nullValue: null));
            value = value.Negate();
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00.999", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00,999", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00,999", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00.999", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00:00.9990000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00:00,9990000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00:00,9990000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00:00.9990000", useInvariantCulture: true, nullValue: null));

            value = TimeSpan.FromSeconds(1) + TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:01.999", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:01,999", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:01,999", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:01.999", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:01.9990000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:01,9990000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:01,9990000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:01.9990000", useInvariantCulture: true, nullValue: null));
            value = value.Negate();
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:01.999", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:01,999", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:01,999", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:01.999", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00:01.9990000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00:01,9990000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00:01,9990000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00:01.9990000", useInvariantCulture: true, nullValue: null));

            value = TimeSpan.FromSeconds(59) + TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:59.999", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:59,999", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:59,999", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:59.999", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:59.9990000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:59,9990000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:59,9990000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:00:59.9990000", useInvariantCulture: true, nullValue: null));
            value = value.Negate();
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:59.999", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:59,999", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:59,999", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:59.999", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00:59.9990000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00:59,9990000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00:59,9990000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:00:59.9990000", useInvariantCulture: true, nullValue: null));

            value = TimeSpan.FromMinutes(1) + TimeSpan.FromSeconds(59) + TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:01:59.999", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:01:59,999", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:01:59,999", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:01:59.999", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:01:59.9990000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:01:59,9990000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:01:59,9990000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:01:59.9990000", useInvariantCulture: true, nullValue: null));
            value = value.Negate();
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:01:59.999", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:01:59,999", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:01:59,999", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:01:59.999", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:01:59.9990000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:01:59,9990000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:01:59,9990000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:01:59.9990000", useInvariantCulture: true, nullValue: null));

            value = TimeSpan.FromMinutes(59) + TimeSpan.FromSeconds(59) + TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:59:59.999", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:59:59,999", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:59:59,999", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:59:59.999", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:59:59.9990000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:59:59,9990000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:59:59,9990000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:00:59:59.9990000", useInvariantCulture: true, nullValue: null));
            value = value.Negate();
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:59:59.999", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:59:59,999", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:59:59,999", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:59:59.999", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:59:59.9990000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:59:59,9990000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:59:59,9990000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:00:59:59.9990000", useInvariantCulture: true, nullValue: null));

            value = TimeSpan.FromHours(1) + TimeSpan.FromMinutes(59) + TimeSpan.FromSeconds(59) + TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"1:59:59.999", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"1:59:59,999", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"1:59:59,999", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"1:59:59.999", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:01:59:59.9990000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:01:59:59,9990000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:01:59:59,9990000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:01:59:59.9990000", useInvariantCulture: true, nullValue: null));
            value = value.Negate();
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-1:59:59.999", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-1:59:59,999", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-1:59:59,999", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-1:59:59.999", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:01:59:59.9990000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:01:59:59,9990000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:01:59:59,9990000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:01:59:59.9990000", useInvariantCulture: true, nullValue: null));

            value = TimeSpan.FromHours(23) + TimeSpan.FromMinutes(59) + TimeSpan.FromSeconds(59) + TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"23:59:59.999", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"23:59:59,999", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"23:59:59,999", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"23:59:59.999", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:23:59:59.9990000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:23:59:59,9990000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:23:59:59,9990000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"0:23:59:59.9990000", useInvariantCulture: true, nullValue: null));
            value = value.Negate();
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-23:59:59.999", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-23:59:59,999", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-23:59:59,999", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-23:59:59.999", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:23:59:59.9990000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:23:59:59,9990000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:23:59:59,9990000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-0:23:59:59.9990000", useInvariantCulture: true, nullValue: null));

            value = TimeSpan.FromDays(1) + TimeSpan.FromHours(23) + TimeSpan.FromMinutes(59) + TimeSpan.FromSeconds(59) + TimeSpan.FromMilliseconds(999);

            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"1:23:59:59.999", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"1:23:59:59,999", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"1:23:59:59,999", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"1:23:59:59.999", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"1:23:59:59.9990000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"1:23:59:59,9990000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"1:23:59:59,9990000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"1:23:59:59.9990000", useInvariantCulture: true, nullValue: null));
            value = value.Negate();
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-1:23:59:59.999", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-1:23:59:59,999", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-1:23:59:59,999", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-1:23:59:59.999", useInvariantCulture: true, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-1:23:59:59.9990000", cultureInfo: cultureInfoDot, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-1:23:59:59,9990000", cultureInfo: cultureInfoComma, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-1:23:59:59,9990000", useInvariantCulture: false, nullValue: null));
            Assert.AreEqual(value, DateUtils.ToTimeSpan(value: @"-1:23:59:59.9990000", useInvariantCulture: true, nullValue: null));

            Thread.CurrentThread.CurrentCulture = oldCulture;
        }

        #endregion

        #region Rounding and age calculation

        [TestMethod]
        public void TestDateUtilsRoundDateTime()
        {
            TestValues(TimeSpan.Zero);
            TestValues(TimeSpan.FromMilliseconds(-33));
            TestValues(TimeSpan.FromMilliseconds(33));
            TestValues(TimeSpan.FromMinutes(-33));
            TestValues(TimeSpan.FromMinutes(33));
            TestValues(TimeSpan.FromHours(-33));
            TestValues(TimeSpan.FromHours(33));

            void TestValues(TimeSpan offset)
            {
                var value1 = new DateTime(2012, 5, 14, 11, 29, 29, 444).Add(offset);
                var value2 = new DateTime(2012, 6, 16, 13, 31, 31, 556).Add(offset);

                Assert.AreEqual(value1, DateUtils.Floor(value1, TimeSpan.FromTicks(1), offset));
                Assert.AreEqual(value1, DateUtils.Round(value1, TimeSpan.FromTicks(1), offset));
                Assert.AreEqual(value1, DateUtils.Ceiling(value1, TimeSpan.FromTicks(1), offset));
                Assert.AreEqual(value2, DateUtils.Floor(value2, TimeSpan.FromTicks(1), offset));
                Assert.AreEqual(value2, DateUtils.Round(value2, TimeSpan.FromTicks(1), offset));
                Assert.AreEqual(value2, DateUtils.Ceiling(value2, TimeSpan.FromTicks(1), offset));

                Assert.AreEqual(value1, DateUtils.Floor(value1, TimeSpan.FromMilliseconds(1), offset));
                Assert.AreEqual(value1, DateUtils.Round(value1, TimeSpan.FromMilliseconds(1), offset));
                Assert.AreEqual(value1, DateUtils.Ceiling(value1, TimeSpan.FromMilliseconds(1), offset));
                Assert.AreEqual(value2, DateUtils.Floor(value2, TimeSpan.FromMilliseconds(1), offset));
                Assert.AreEqual(value2, DateUtils.Round(value2, TimeSpan.FromMilliseconds(1), offset));
                Assert.AreEqual(value2, DateUtils.Ceiling(value2, TimeSpan.FromMilliseconds(1), offset));

                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 29, 29, 440).Add(offset), DateUtils.Floor(value1, TimeSpan.FromMilliseconds(10), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 29, 29, 440).Add(offset), DateUtils.Round(value1, TimeSpan.FromMilliseconds(10), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 29, 29, 450).Add(offset), DateUtils.Ceiling(value1, TimeSpan.FromMilliseconds(10), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 31, 31, 550).Add(offset), DateUtils.Floor(value2, TimeSpan.FromMilliseconds(10), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 31, 31, 560).Add(offset), DateUtils.Round(value2, TimeSpan.FromMilliseconds(10), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 31, 31, 560).Add(offset), DateUtils.Ceiling(value2, TimeSpan.FromMilliseconds(10), offset));

                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 29, 29, 400).Add(offset), DateUtils.Floor(value1, TimeSpan.FromMilliseconds(100), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 29, 29, 400).Add(offset), DateUtils.Round(value1, TimeSpan.FromMilliseconds(100), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 29, 29, 500).Add(offset), DateUtils.Ceiling(value1, TimeSpan.FromMilliseconds(100), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 31, 31, 500).Add(offset), DateUtils.Floor(value2, TimeSpan.FromMilliseconds(100), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 31, 31, 600).Add(offset), DateUtils.Round(value2, TimeSpan.FromMilliseconds(100), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 31, 31, 600).Add(offset), DateUtils.Ceiling(value2, TimeSpan.FromMilliseconds(100), offset));

                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 29, 29, 0).Add(offset), DateUtils.Floor(value1, TimeSpan.FromSeconds(1), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 29, 29, 0).Add(offset), DateUtils.Round(value1, TimeSpan.FromSeconds(1), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 29, 30, 0).Add(offset), DateUtils.Ceiling(value1, TimeSpan.FromSeconds(1), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 31, 31, 0).Add(offset), DateUtils.Floor(value2, TimeSpan.FromSeconds(1), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 31, 32, 0).Add(offset), DateUtils.Round(value2, TimeSpan.FromSeconds(1), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 31, 32, 0).Add(offset), DateUtils.Ceiling(value2, TimeSpan.FromSeconds(1), offset));

                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 29, 15, 0).Add(offset), DateUtils.Floor(value1, TimeSpan.FromSeconds(15), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 29, 30, 0).Add(offset), DateUtils.Round(value1, TimeSpan.FromSeconds(15), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 29, 30, 0).Add(offset), DateUtils.Ceiling(value1, TimeSpan.FromSeconds(15), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 31, 30, 0).Add(offset), DateUtils.Floor(value2, TimeSpan.FromSeconds(15), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 31, 30, 0).Add(offset), DateUtils.Round(value2, TimeSpan.FromSeconds(15), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 31, 45, 0).Add(offset), DateUtils.Ceiling(value2, TimeSpan.FromSeconds(15), offset));

                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 29, 0, 0).Add(offset), DateUtils.Floor(value1, TimeSpan.FromMinutes(1), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 29, 0, 0).Add(offset), DateUtils.Round(value1, TimeSpan.FromMinutes(1), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 30, 0, 0).Add(offset), DateUtils.Ceiling(value1, TimeSpan.FromMinutes(1), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 31, 0, 0).Add(offset), DateUtils.Floor(value2, TimeSpan.FromMinutes(1), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 32, 0, 0).Add(offset), DateUtils.Round(value2, TimeSpan.FromMinutes(1), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 32, 0, 0).Add(offset), DateUtils.Ceiling(value2, TimeSpan.FromMinutes(1), offset));

                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 15, 0, 0).Add(offset), DateUtils.Floor(value1, TimeSpan.FromMinutes(15), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 30, 0, 0).Add(offset), DateUtils.Round(value1, TimeSpan.FromMinutes(15), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 30, 0, 0).Add(offset), DateUtils.Ceiling(value1, TimeSpan.FromMinutes(15), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 30, 0, 0).Add(offset), DateUtils.Floor(value2, TimeSpan.FromMinutes(15), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 30, 0, 0).Add(offset), DateUtils.Round(value2, TimeSpan.FromMinutes(15), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 45, 0, 0).Add(offset), DateUtils.Ceiling(value2, TimeSpan.FromMinutes(15), offset));

                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 20, 0, 0).Add(offset), DateUtils.Floor(value1, TimeSpan.FromMinutes(20), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 20, 0, 0).Add(offset), DateUtils.Round(value1, TimeSpan.FromMinutes(20), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 40, 0, 0).Add(offset), DateUtils.Ceiling(value1, TimeSpan.FromMinutes(20), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 20, 0, 0).Add(offset), DateUtils.Floor(value2, TimeSpan.FromMinutes(20), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 40, 0, 0).Add(offset), DateUtils.Round(value2, TimeSpan.FromMinutes(20), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 40, 0, 0).Add(offset), DateUtils.Ceiling(value2, TimeSpan.FromMinutes(20), offset));

                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 0, 0, 0).Add(offset), DateUtils.Floor(value1, TimeSpan.FromHours(1), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 11, 0, 0, 0).Add(offset), DateUtils.Round(value1, TimeSpan.FromHours(1), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 12, 0, 0, 0).Add(offset), DateUtils.Ceiling(value1, TimeSpan.FromHours(1), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 13, 0, 0, 0).Add(offset), DateUtils.Floor(value2, TimeSpan.FromHours(1), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 14, 0, 0, 0).Add(offset), DateUtils.Round(value2, TimeSpan.FromHours(1), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 14, 0, 0, 0).Add(offset), DateUtils.Ceiling(value2, TimeSpan.FromHours(1), offset));

                Assert.AreEqual(new DateTime(2012, 5, 14, 0, 0, 0, 0).Add(offset), DateUtils.Floor(value1, TimeSpan.FromDays(1), offset));
                Assert.AreEqual(new DateTime(2012, 5, 14, 0, 0, 0, 0).Add(offset), DateUtils.Round(value1, TimeSpan.FromDays(1), offset));
                Assert.AreEqual(new DateTime(2012, 5, 15, 0, 0, 0, 0).Add(offset), DateUtils.Ceiling(value1, TimeSpan.FromDays(1), offset));
                Assert.AreEqual(new DateTime(2012, 6, 16, 0, 0, 0, 0).Add(offset), DateUtils.Floor(value2, TimeSpan.FromDays(1), offset));
                Assert.AreEqual(new DateTime(2012, 6, 17, 0, 0, 0, 0).Add(offset), DateUtils.Round(value2, TimeSpan.FromDays(1), offset));
                Assert.AreEqual(new DateTime(2012, 6, 17, 0, 0, 0, 0).Add(offset), DateUtils.Ceiling(value2, TimeSpan.FromDays(1), offset));
            }
        }

        [TestMethod]
        public void TestDateUtilsRoundTimeSpan()
        {
            TestValues(TimeSpan.Zero);
            TestValues(TimeSpan.FromMilliseconds(-33));
            TestValues(TimeSpan.FromMilliseconds(33));
            TestValues(TimeSpan.FromMinutes(-33));
            TestValues(TimeSpan.FromMinutes(33));
            TestValues(TimeSpan.FromHours(-33));
            TestValues(TimeSpan.FromHours(33));

            void TestValues(TimeSpan offset)
            {
                var value1 = CreateTimeSpan(14, 11, 29, 29, 444);
                var value2 = CreateTimeSpan(16, 13, 31, 31, 556);

                Assert.AreEqual(value1, DateUtils.Floor(value1, TimeSpan.FromTicks(1), offset));
                Assert.AreEqual(value1, DateUtils.Round(value1, TimeSpan.FromTicks(1), offset));
                Assert.AreEqual(value1, DateUtils.Ceiling(value1, TimeSpan.FromTicks(1), offset));
                Assert.AreEqual(value2, DateUtils.Floor(value2, TimeSpan.FromTicks(1), offset));
                Assert.AreEqual(value2, DateUtils.Round(value2, TimeSpan.FromTicks(1), offset));
                Assert.AreEqual(value2, DateUtils.Ceiling(value2, TimeSpan.FromTicks(1), offset));

                Assert.AreEqual(value1, DateUtils.Floor(value1, TimeSpan.FromMilliseconds(1), offset));
                Assert.AreEqual(value1, DateUtils.Round(value1, TimeSpan.FromMilliseconds(1), offset));
                Assert.AreEqual(value1, DateUtils.Ceiling(value1, TimeSpan.FromMilliseconds(1), offset));
                Assert.AreEqual(value2, DateUtils.Floor(value2, TimeSpan.FromMilliseconds(1), offset));
                Assert.AreEqual(value2, DateUtils.Round(value2, TimeSpan.FromMilliseconds(1), offset));
                Assert.AreEqual(value2, DateUtils.Ceiling(value2, TimeSpan.FromMilliseconds(1), offset));

                Assert.AreEqual(CreateTimeSpan(14, 11, 29, 29, 440), DateUtils.Floor(value1, TimeSpan.FromMilliseconds(10), offset));
                Assert.AreEqual(CreateTimeSpan(14, 11, 29, 29, 440), DateUtils.Round(value1, TimeSpan.FromMilliseconds(10), offset));
                Assert.AreEqual(CreateTimeSpan(14, 11, 29, 29, 450), DateUtils.Ceiling(value1, TimeSpan.FromMilliseconds(10), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 31, 31, 550), DateUtils.Floor(value2, TimeSpan.FromMilliseconds(10), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 31, 31, 560), DateUtils.Round(value2, TimeSpan.FromMilliseconds(10), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 31, 31, 560), DateUtils.Ceiling(value2, TimeSpan.FromMilliseconds(10), offset));

                Assert.AreEqual(CreateTimeSpan(14, 11, 29, 29, 400), DateUtils.Floor(value1, TimeSpan.FromMilliseconds(100), offset));
                Assert.AreEqual(CreateTimeSpan(14, 11, 29, 29, 400), DateUtils.Round(value1, TimeSpan.FromMilliseconds(100), offset));
                Assert.AreEqual(CreateTimeSpan(14, 11, 29, 29, 500), DateUtils.Ceiling(value1, TimeSpan.FromMilliseconds(100), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 31, 31, 500), DateUtils.Floor(value2, TimeSpan.FromMilliseconds(100), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 31, 31, 600), DateUtils.Round(value2, TimeSpan.FromMilliseconds(100), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 31, 31, 600), DateUtils.Ceiling(value2, TimeSpan.FromMilliseconds(100), offset));

                Assert.AreEqual(CreateTimeSpan(14, 11, 29, 29, 0), DateUtils.Floor(value1, TimeSpan.FromSeconds(1), offset));
                Assert.AreEqual(CreateTimeSpan(14, 11, 29, 29, 0), DateUtils.Round(value1, TimeSpan.FromSeconds(1), offset));
                Assert.AreEqual(CreateTimeSpan(14, 11, 29, 30, 0), DateUtils.Ceiling(value1, TimeSpan.FromSeconds(1), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 31, 31, 0), DateUtils.Floor(value2, TimeSpan.FromSeconds(1), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 31, 32, 0), DateUtils.Round(value2, TimeSpan.FromSeconds(1), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 31, 32, 0), DateUtils.Ceiling(value2, TimeSpan.FromSeconds(1), offset));

                Assert.AreEqual(CreateTimeSpan(14, 11, 29, 15, 0), DateUtils.Floor(value1, TimeSpan.FromSeconds(15), offset));
                Assert.AreEqual(CreateTimeSpan(14, 11, 29, 30, 0), DateUtils.Round(value1, TimeSpan.FromSeconds(15), offset));
                Assert.AreEqual(CreateTimeSpan(14, 11, 29, 30, 0), DateUtils.Ceiling(value1, TimeSpan.FromSeconds(15), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 31, 30, 0), DateUtils.Floor(value2, TimeSpan.FromSeconds(15), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 31, 30, 0), DateUtils.Round(value2, TimeSpan.FromSeconds(15), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 31, 45, 0), DateUtils.Ceiling(value2, TimeSpan.FromSeconds(15), offset));

                Assert.AreEqual(CreateTimeSpan(14, 11, 29, 0, 0), DateUtils.Floor(value1, TimeSpan.FromMinutes(1), offset));
                Assert.AreEqual(CreateTimeSpan(14, 11, 29, 0, 0), DateUtils.Round(value1, TimeSpan.FromMinutes(1), offset));
                Assert.AreEqual(CreateTimeSpan(14, 11, 30, 0, 0), DateUtils.Ceiling(value1, TimeSpan.FromMinutes(1), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 31, 0, 0), DateUtils.Floor(value2, TimeSpan.FromMinutes(1), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 32, 0, 0), DateUtils.Round(value2, TimeSpan.FromMinutes(1), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 32, 0, 0), DateUtils.Ceiling(value2, TimeSpan.FromMinutes(1), offset));

                Assert.AreEqual(CreateTimeSpan(14, 11, 15, 0, 0), DateUtils.Floor(value1, TimeSpan.FromMinutes(15), offset));
                Assert.AreEqual(CreateTimeSpan(14, 11, 30, 0, 0), DateUtils.Round(value1, TimeSpan.FromMinutes(15), offset));
                Assert.AreEqual(CreateTimeSpan(14, 11, 30, 0, 0), DateUtils.Ceiling(value1, TimeSpan.FromMinutes(15), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 30, 0, 0), DateUtils.Floor(value2, TimeSpan.FromMinutes(15), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 30, 0, 0), DateUtils.Round(value2, TimeSpan.FromMinutes(15), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 45, 0, 0), DateUtils.Ceiling(value2, TimeSpan.FromMinutes(15), offset));

                Assert.AreEqual(CreateTimeSpan(14, 11, 20, 0, 0), DateUtils.Floor(value1, TimeSpan.FromMinutes(20), offset));
                Assert.AreEqual(CreateTimeSpan(14, 11, 20, 0, 0), DateUtils.Round(value1, TimeSpan.FromMinutes(20), offset));
                Assert.AreEqual(CreateTimeSpan(14, 11, 40, 0, 0), DateUtils.Ceiling(value1, TimeSpan.FromMinutes(20), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 20, 0, 0), DateUtils.Floor(value2, TimeSpan.FromMinutes(20), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 40, 0, 0), DateUtils.Round(value2, TimeSpan.FromMinutes(20), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 40, 0, 0), DateUtils.Ceiling(value2, TimeSpan.FromMinutes(20), offset));

                Assert.AreEqual(CreateTimeSpan(14, 11, 0, 0, 0), DateUtils.Floor(value1, TimeSpan.FromHours(1), offset));
                Assert.AreEqual(CreateTimeSpan(14, 11, 0, 0, 0), DateUtils.Round(value1, TimeSpan.FromHours(1), offset));
                Assert.AreEqual(CreateTimeSpan(14, 12, 0, 0, 0), DateUtils.Ceiling(value1, TimeSpan.FromHours(1), offset));
                Assert.AreEqual(CreateTimeSpan(16, 13, 0, 0, 0), DateUtils.Floor(value2, TimeSpan.FromHours(1), offset));
                Assert.AreEqual(CreateTimeSpan(16, 14, 0, 0, 0), DateUtils.Round(value2, TimeSpan.FromHours(1), offset));
                Assert.AreEqual(CreateTimeSpan(16, 14, 0, 0, 0), DateUtils.Ceiling(value2, TimeSpan.FromHours(1), offset));

                Assert.AreEqual(CreateTimeSpan(14, 0, 0, 0, 0), DateUtils.Floor(value1, TimeSpan.FromDays(1), offset));
                Assert.AreEqual(CreateTimeSpan(14, 0, 0, 0, 0), DateUtils.Round(value1, TimeSpan.FromDays(1), offset));
                Assert.AreEqual(CreateTimeSpan(15, 0, 0, 0, 0), DateUtils.Ceiling(value1, TimeSpan.FromDays(1), offset));
                Assert.AreEqual(CreateTimeSpan(16, 0, 0, 0, 0), DateUtils.Floor(value2, TimeSpan.FromDays(1), offset));
                Assert.AreEqual(CreateTimeSpan(17, 0, 0, 0, 0), DateUtils.Round(value2, TimeSpan.FromDays(1), offset));
                Assert.AreEqual(CreateTimeSpan(17, 0, 0, 0, 0), DateUtils.Ceiling(value2, TimeSpan.FromDays(1), offset));

                TimeSpan CreateTimeSpan(int days, int hours, int minutes, int seconds, int millisecond)
                {
                    return (new DateTime(2012, 6, 17, hours, minutes, seconds, millisecond).AddDays(days) - new DateTime(2000, 7, 1, 0, 0, 0, 0)).Add(offset);
                }
            }
        }

        [TestMethod]
        public void TestDateUtilsCalculateAge()
        {
            Assert.AreEqual(7, DateUtils.CalculateAge(endDate: new DateTime(2004, 2, 28), startDate: new DateTime(1996, 2, 29)));
            Assert.AreEqual(8, DateUtils.CalculateAge(endDate: new DateTime(2004, 2, 29), startDate: new DateTime(1996, 2, 28)));
            Assert.AreEqual(8, DateUtils.CalculateAge(endDate: new DateTime(2004, 2, 29), startDate: new DateTime(1996, 2, 29)));

            Assert.AreEqual(0, DateUtils.CalculateAge(endDate: new DateTime(2010, 6, 15), startDate: new DateTime(2010, 6, 10)));
            Assert.AreEqual(0, DateUtils.CalculateAge(endDate: new DateTime(2010, 6, 15), startDate: new DateTime(2010, 6, 14)));
            Assert.AreEqual(0, DateUtils.CalculateAge(endDate: new DateTime(2010, 6, 15), startDate: new DateTime(2010, 6, 15)));
            Assert.AreEqual(0, DateUtils.CalculateAge(endDate: new DateTime(2010, 6, 15, 12, 0, 0), startDate: new DateTime(2010, 6, 15, 9, 0, 0)));
            Assert.AreEqual(0, DateUtils.CalculateAge(endDate: new DateTime(2010, 6, 15, 12, 0, 0), startDate: new DateTime(2010, 6, 15, 12, 0, 0)));
            Assert.AreEqual(0, DateUtils.CalculateAge(endDate: new DateTime(2010, 6, 15, 12, 0, 0), startDate: new DateTime(2010, 6, 15, 15, 0, 0)));

            try
            {
                DateUtils.CalculateAge(endDate: new DateTime(2010, 6, 15), startDate: new DateTime(2010, 6, 16));
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.AreEqual(@"startDate", ex.ParamName);
            }

            try
            {
                DateUtils.CalculateAge(endDate: new DateTime(2010, 6, 15), startDate: new DateTime(2010, 6, 20));
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Assert.AreEqual(@"startDate", ex.ParamName);
            }

            Assert.AreEqual(1, DateUtils.CalculateAge(endDate: new DateTime(2011, 6, 15), startDate: new DateTime(2010, 6, 10)));
            Assert.AreEqual(1, DateUtils.CalculateAge(endDate: new DateTime(2011, 6, 15), startDate: new DateTime(2010, 6, 14)));
            Assert.AreEqual(1, DateUtils.CalculateAge(endDate: new DateTime(2011, 6, 15), startDate: new DateTime(2010, 6, 15)));
            Assert.AreEqual(1, DateUtils.CalculateAge(endDate: new DateTime(2011, 6, 15, 12, 0, 0), startDate: new DateTime(2010, 6, 15, 9, 0, 0)));
            Assert.AreEqual(1, DateUtils.CalculateAge(endDate: new DateTime(2011, 6, 15, 12, 0, 0), startDate: new DateTime(2010, 6, 15, 12, 0, 0)));
            Assert.AreEqual(1, DateUtils.CalculateAge(endDate: new DateTime(2011, 6, 15, 12, 0, 0), startDate: new DateTime(2010, 6, 15, 15, 0, 0)));
            Assert.AreEqual(0, DateUtils.CalculateAge(endDate: new DateTime(2011, 6, 15), startDate: new DateTime(2010, 6, 16)));
            Assert.AreEqual(0, DateUtils.CalculateAge(endDate: new DateTime(2011, 6, 15), startDate: new DateTime(2010, 6, 20)));

            Assert.AreEqual(2, DateUtils.CalculateAge(endDate: new DateTime(2012, 6, 15), startDate: new DateTime(2010, 6, 10)));
            Assert.AreEqual(2, DateUtils.CalculateAge(endDate: new DateTime(2012, 6, 15), startDate: new DateTime(2010, 6, 14)));
            Assert.AreEqual(2, DateUtils.CalculateAge(endDate: new DateTime(2012, 6, 15), startDate: new DateTime(2010, 6, 15)));
            Assert.AreEqual(2, DateUtils.CalculateAge(endDate: new DateTime(2012, 6, 15, 12, 0, 0), startDate: new DateTime(2010, 6, 15, 9, 0, 0)));
            Assert.AreEqual(2, DateUtils.CalculateAge(endDate: new DateTime(2012, 6, 15, 12, 0, 0), startDate: new DateTime(2010, 6, 15, 12, 0, 0)));
            Assert.AreEqual(2, DateUtils.CalculateAge(endDate: new DateTime(2012, 6, 15, 12, 0, 0), startDate: new DateTime(2010, 6, 15, 15, 0, 0)));
            Assert.AreEqual(1, DateUtils.CalculateAge(endDate: new DateTime(2012, 6, 15), startDate: new DateTime(2010, 6, 16)));
            Assert.AreEqual(1, DateUtils.CalculateAge(endDate: new DateTime(2012, 6, 15), startDate: new DateTime(2010, 6, 20)));
        }

        #endregion
    }
}
