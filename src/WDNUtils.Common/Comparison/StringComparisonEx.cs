using System;

namespace WDNUtils.Common
{
    /// <summary>
    /// Specifies the culture, case, and sort rules to be used by StringComparerEx.
    /// </summary>
    public enum StringComparisonEx : int
    {
        /// <summary>
        /// Compare strings using culture-sensitive sort rules and the current culture.
        /// </summary>
        CurrentCulture = 0,

        /// <summary>
        /// Compare strings using culture-sensitive sort rules, the current culture, and ignoring the case of the strings being compared.
        /// </summary>
        CurrentCultureIgnoreCase = 1,

        /// <summary>
        /// Compare strings using culture-sensitive sort rules and the invariant culture.
        /// </summary>
        [Obsolete(message: @"InvariantCulture should not be used in most cases; use Ordinal for application data, and Natural or CurrentCulture for user displayed data.", error: false)]
        InvariantCulture = 2,

        /// <summary>
        /// Compare strings using culture-sensitive sort rules, the invariant culture, and ignoring the case of the strings being compared.
        /// </summary>
        [Obsolete(message: @"InvariantCultureIgnoreCase should not be used in most cases; use OrdinalIgnoreCase for application data, and NaturalIgnoreCase or CurrentCultureIgnoreCase for user displayed data.", error: false)]
        InvariantCultureIgnoreCase = 3,

        /// <summary>
        /// Compare strings using ordinal (binary) sort rules.
        /// </summary>
        Ordinal = 4,

        /// <summary>
        /// Compare strings using ordinal (binary) sort rules and ignoring the case of the strings being compared.
        /// </summary>
        OrdinalIgnoreCase = 5,

        /// <summary>
        /// Compare strings using natural order (natural numbers in latin digits are parsed and compared), culture-sensitive sort rules, and the current culture.
        /// </summary>
        Natural = 6,

        /// <summary>
        /// Compare strings using natural order (natural numbers in latin digits are parsed and compared), culture-sensitive sort rules, the current culture, and ignoring the case of the strings being compared.
        /// </summary>
        NaturalIgnoreCase = 7
    }
}
