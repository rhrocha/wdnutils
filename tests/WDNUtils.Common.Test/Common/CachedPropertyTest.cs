using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WDNUtils.Common.Test
{
    [TestClass]
    public class CachedPropertyTest
    {
        // Counters for the value factories

        private static long _numberToStringCounter = 0;
        private static long _numberValueCounter = 0;

        // Note that _numberValue is initialized after _numberToString during the static initialization, but this does not
        // fail because the referenced property list is retrieved when _numberToString.Value is called for the fist time

        private static CachedProperty<string> _numberToString = new CachedProperty<string>(() =>
            {
                _numberToStringCounter++;
                return NumberUtils.Int32ToStringISO(NumberValue);
            },
            () => _numberValue);

        private static CachedProperty<int> _numberValue = new CachedProperty<int>(() =>
        {
            _numberValueCounter++;
            return RandomUtils.NextInt32();
        });

        // Wrappers for the cached properties

        private static string NumberToString
        {
            get => _numberToString.Value;
            set => _numberToString.Value = value;
        }

        private static int NumberValue
        {
            get => _numberValue.Value;
            set => _numberValue.Value = value;
        }

        [TestMethod]
        public void TestCachedProperty()
        {
            long numberToStringCounter = 0;
            long numberValueCounter = 0;

            Assert.AreEqual(numberToStringCounter, _numberToStringCounter);
            Assert.AreEqual(numberValueCounter, _numberValueCounter);

            // Initialize both cached properties

            var numberToString = NumberToString;

            Assert.AreEqual(++numberToStringCounter, _numberToStringCounter);
            Assert.AreEqual(++numberValueCounter, _numberValueCounter);

            // Retrieve the cached values

            Assert.AreEqual(NumberUtils.Int32ToStringISO(NumberValue), NumberToString);

            Assert.AreEqual(numberToStringCounter, _numberToStringCounter);
            Assert.AreEqual(numberValueCounter, _numberValueCounter);

            // Clear the string value

            _numberToString.Clear();

            Assert.AreEqual(NumberUtils.Int32ToStringISO(NumberValue), NumberToString);

            Assert.AreEqual(++numberToStringCounter, _numberToStringCounter);
            Assert.AreEqual(numberValueCounter, _numberValueCounter);

            // Clear the number value

            _numberValue.Clear();

            Assert.AreEqual(NumberUtils.Int32ToStringISO(NumberValue), NumberToString);

            Assert.AreEqual(++numberToStringCounter, _numberToStringCounter);
            Assert.AreEqual(++numberValueCounter, _numberValueCounter);

            // Set a custom string value

            NumberToString = string.Empty;

            Assert.AreNotEqual(NumberUtils.Int32ToStringISO(NumberValue), NumberToString);

            Assert.AreEqual(numberToStringCounter, _numberToStringCounter);
            Assert.AreEqual(numberValueCounter, _numberValueCounter);

            // Set a custom number value

            NumberValue = 0;

            Assert.AreNotEqual(NumberUtils.Int32ToStringISO(NumberValue), NumberToString);

            Assert.AreEqual(numberToStringCounter, _numberToStringCounter);
            Assert.AreEqual(numberValueCounter, _numberValueCounter);

            // Clear custom string value

            _numberToString.ClearCustomValue();

            Assert.AreEqual(NumberUtils.Int32ToStringISO(NumberValue), NumberToString);

            Assert.AreEqual(++numberToStringCounter, _numberToStringCounter);
            Assert.AreEqual(numberValueCounter, _numberValueCounter);

            // Clear custom number value

            _numberValue.ClearCustomValue();

            Assert.AreEqual(NumberUtils.Int32ToStringISO(NumberValue), NumberToString);

            Assert.AreEqual(++numberToStringCounter, _numberToStringCounter);
            Assert.AreEqual(++numberValueCounter, _numberValueCounter);
        }
    }
}
