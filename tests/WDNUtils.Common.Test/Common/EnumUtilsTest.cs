using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace WDNUtils.Common.Test
{
    [TestClass]
    public class EnumUtilsTest
    {
        [TestMethod]
        public void TestEnumUtils()
        {
            Assert.IsTrue(EnumUtils.GetValues<EnumSByte>().Select(item => (decimal)item).SequenceEqual(new decimal[] { 0, 1, 2, 4 }));

            Assert.IsTrue((EnumSByte.V0 | EnumSByte.V1 | EnumSByte.V2).HasAnyFlag(EnumSByte.V0 | EnumSByte.V2));
            Assert.IsFalse((EnumSByte.V0 | EnumSByte.V1 | EnumSByte.V2).HasAnyFlag(EnumSByte.V0 | EnumSByte.V4));
            Assert.IsTrue(EnumSByte.V1.HasAnyBit(EnumSByte.V0 | EnumSByte.V1 | EnumSByte.V2));
            Assert.IsFalse(EnumSByte.V4.HasAnyBit(EnumSByte.V0 | EnumSByte.V1 | EnumSByte.V2));
            Assert.AreEqual(EnumSByte.V0, EnumUtils.ConvertEnum((sbyte?)null, EnumSByte.V0));
            Assert.AreEqual(EnumSByte.V0, EnumUtils.ConvertEnum<EnumSByte>(0));
            Assert.AreEqual(EnumSByte.V2, EnumUtils.ConvertEnum((sbyte?)2, EnumSByte.V0));
            Assert.AreEqual(EnumSByte.V2, EnumUtils.ConvertEnum<EnumSByte>(2));

            Assert.IsTrue((EnumByte.V0 | EnumByte.V1 | EnumByte.V2).HasAnyFlag(EnumByte.V0 | EnumByte.V2));
            Assert.IsFalse((EnumByte.V0 | EnumByte.V1 | EnumByte.V2).HasAnyFlag(EnumByte.V0 | EnumByte.V4));
            Assert.IsTrue(EnumByte.V1.HasAnyBit(EnumByte.V0 | EnumByte.V1 | EnumByte.V2));
            Assert.IsFalse(EnumByte.V4.HasAnyBit(EnumByte.V0 | EnumByte.V1 | EnumByte.V2));
            Assert.AreEqual(EnumByte.V0, EnumUtils.ConvertEnum((byte?)null, EnumByte.V0));
            Assert.AreEqual(EnumByte.V0, EnumUtils.ConvertEnum<EnumByte>(0));
            Assert.AreEqual(EnumByte.V2, EnumUtils.ConvertEnum((byte?)2, EnumByte.V0));
            Assert.AreEqual(EnumByte.V2, EnumUtils.ConvertEnum<EnumByte>(2));

            Assert.IsTrue((EnumUInt16.V0 | EnumUInt16.V1 | EnumUInt16.V2).HasAnyFlag(EnumUInt16.V0 | EnumUInt16.V2));
            Assert.IsFalse((EnumUInt16.V0 | EnumUInt16.V1 | EnumUInt16.V2).HasAnyFlag(EnumUInt16.V0 | EnumUInt16.V4));
            Assert.IsTrue(EnumUInt16.V1.HasAnyBit(EnumUInt16.V0 | EnumUInt16.V1 | EnumUInt16.V2));
            Assert.IsFalse(EnumUInt16.V4.HasAnyBit(EnumUInt16.V0 | EnumUInt16.V1 | EnumUInt16.V2));
            Assert.AreEqual(EnumUInt16.V0, EnumUtils.ConvertEnum((ushort?)null, EnumUInt16.V0));
            Assert.AreEqual(EnumUInt16.V0, EnumUtils.ConvertEnum<EnumUInt16>(0));
            Assert.AreEqual(EnumUInt16.V2, EnumUtils.ConvertEnum((ushort?)2, EnumUInt16.V0));
            Assert.AreEqual(EnumUInt16.V2, EnumUtils.ConvertEnum<EnumUInt16>(2));

            Assert.IsTrue((EnumInt16.V0 | EnumInt16.V1 | EnumInt16.V2).HasAnyFlag(EnumInt16.V0 | EnumInt16.V2));
            Assert.IsFalse((EnumInt16.V0 | EnumInt16.V1 | EnumInt16.V2).HasAnyFlag(EnumInt16.V0 | EnumInt16.V4));
            Assert.IsTrue(EnumInt16.V1.HasAnyBit(EnumInt16.V0 | EnumInt16.V1 | EnumInt16.V2));
            Assert.IsFalse(EnumInt16.V4.HasAnyBit(EnumInt16.V0 | EnumInt16.V1 | EnumInt16.V2));
            Assert.AreEqual(EnumInt16.V0, EnumUtils.ConvertEnum((short?)null, EnumInt16.V0));
            Assert.AreEqual(EnumInt16.V0, EnumUtils.ConvertEnum<EnumInt16>(0));
            Assert.AreEqual(EnumInt16.V2, EnumUtils.ConvertEnum((short?)2, EnumInt16.V0));
            Assert.AreEqual(EnumInt16.V2, EnumUtils.ConvertEnum<EnumInt16>(2));

            Assert.IsTrue((EnumUInt32.V0 | EnumUInt32.V1 | EnumUInt32.V2).HasAnyFlag(EnumUInt32.V0 | EnumUInt32.V2));
            Assert.IsFalse((EnumUInt32.V0 | EnumUInt32.V1 | EnumUInt32.V2).HasAnyFlag(EnumUInt32.V0 | EnumUInt32.V4));
            Assert.IsTrue(EnumUInt32.V1.HasAnyBit(EnumUInt32.V0 | EnumUInt32.V1 | EnumUInt32.V2));
            Assert.IsFalse(EnumUInt32.V4.HasAnyBit(EnumUInt32.V0 | EnumUInt32.V1 | EnumUInt32.V2));
            Assert.AreEqual(EnumUInt32.V0, EnumUtils.ConvertEnum((uint?)null, EnumUInt32.V0));
            Assert.AreEqual(EnumUInt32.V0, EnumUtils.ConvertEnum<EnumUInt32>(0));
            Assert.AreEqual(EnumUInt32.V2, EnumUtils.ConvertEnum((uint?)2, EnumUInt32.V0));
            Assert.AreEqual(EnumUInt32.V2, EnumUtils.ConvertEnum<EnumUInt32>(2));

            Assert.IsTrue((EnumInt32.V0 | EnumInt32.V1 | EnumInt32.V2).HasAnyFlag(EnumInt32.V0 | EnumInt32.V2));
            Assert.IsFalse((EnumInt32.V0 | EnumInt32.V1 | EnumInt32.V2).HasAnyFlag(EnumInt32.V0 | EnumInt32.V4));
            Assert.IsTrue(EnumInt32.V1.HasAnyBit(EnumInt32.V0 | EnumInt32.V1 | EnumInt32.V2));
            Assert.IsFalse(EnumInt32.V4.HasAnyBit(EnumInt32.V0 | EnumInt32.V1 | EnumInt32.V2));
            Assert.AreEqual(EnumInt32.V0, EnumUtils.ConvertEnum((int?)null, EnumInt32.V0));
            Assert.AreEqual(EnumInt32.V0, EnumUtils.ConvertEnum<EnumInt32>(0));
            Assert.AreEqual(EnumInt32.V2, EnumUtils.ConvertEnum((int?)2, EnumInt32.V0));
            Assert.AreEqual(EnumInt32.V2, EnumUtils.ConvertEnum<EnumInt32>(2));

            Assert.IsTrue((EnumUInt64.V0 | EnumUInt64.V1 | EnumUInt64.V2).HasAnyFlag(EnumUInt64.V0 | EnumUInt64.V2));
            Assert.IsFalse((EnumUInt64.V0 | EnumUInt64.V1 | EnumUInt64.V2).HasAnyFlag(EnumUInt64.V0 | EnumUInt64.V4));
            Assert.IsTrue(EnumUInt64.V1.HasAnyBit(EnumUInt64.V0 | EnumUInt64.V1 | EnumUInt64.V2));
            Assert.IsFalse(EnumUInt64.V4.HasAnyBit(EnumUInt64.V0 | EnumUInt64.V1 | EnumUInt64.V2));
            Assert.AreEqual(EnumUInt64.V0, EnumUtils.ConvertEnum((ulong?)null, EnumUInt64.V0));
            Assert.AreEqual(EnumUInt64.V0, EnumUtils.ConvertEnum<EnumUInt64>(0));
            Assert.AreEqual(EnumUInt64.V2, EnumUtils.ConvertEnum((ulong?)2, EnumUInt64.V0));
            Assert.AreEqual(EnumUInt64.V2, EnumUtils.ConvertEnum<EnumUInt64>(2));

            Assert.IsTrue((EnumInt64.V0 | EnumInt64.V1 | EnumInt64.V2).HasAnyFlag(EnumInt64.V0 | EnumInt64.V2));
            Assert.IsFalse((EnumInt64.V0 | EnumInt64.V1 | EnumInt64.V2).HasAnyFlag(EnumInt64.V0 | EnumInt64.V4));
            Assert.IsTrue(EnumInt64.V1.HasAnyBit(EnumInt64.V0 | EnumInt64.V1 | EnumInt64.V2));
            Assert.IsFalse(EnumInt64.V4.HasAnyBit(EnumInt64.V0 | EnumInt64.V1 | EnumInt64.V2));
            Assert.AreEqual(EnumInt64.V0, EnumUtils.ConvertEnum((long?)null, EnumInt64.V0));
            Assert.AreEqual(EnumInt64.V0, EnumUtils.ConvertEnum<EnumInt64>(0));
            Assert.AreEqual(EnumInt64.V2, EnumUtils.ConvertEnum((long?)2, EnumInt64.V0));
            Assert.AreEqual(EnumInt64.V2, EnumUtils.ConvertEnum<EnumInt64>(2));
        }

        [Flags]
        private enum EnumSByte : sbyte
        {
            V0 = 0,
            V1 = 1,
            V2 = 2,
            V4 = 4
        }

        [Flags]
        private enum EnumByte : byte
        {
            V0 = 0,
            V1 = 1,
            V2 = 2,
            V4 = 4
        }

        [Flags]
        private enum EnumInt16 : short
        {
            V0 = 0,
            V1 = 1,
            V2 = 2,
            V4 = 4
        }

        [Flags]
        private enum EnumUInt16 : ushort
        {
            V0 = 0,
            V1 = 1,
            V2 = 2,
            V4 = 4
        }

        [Flags]
        private enum EnumInt32 : int
        {
            V0 = 0,
            V1 = 1,
            V2 = 2,
            V4 = 4
        }

        [Flags]
        private enum EnumUInt32 : uint
        {
            V0 = 0,
            V1 = 1,
            V2 = 2,
            V4 = 4
        }

        [Flags]
        private enum EnumInt64 : long
        {
            V0 = 0,
            V1 = 1,
            V2 = 2,
            V4 = 4
        }

        [Flags]
        private enum EnumUInt64 : ulong
        {
            V0 = 0,
            V1 = 1,
            V2 = 2,
            V4 = 4
        }
    }
}
