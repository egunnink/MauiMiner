using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiMiner
{
    [Flags]
    public enum Offering
    {
        NONE    = 0,
        FALL    = (1 << 0),
        WINTER  = (1 << 2),
        SPRING  = (1 << 3),
        SUMMER  = (1 << 4),
        ALL     = ~(-1 << 4)
    }

    public static class OfferingExtensions
    {
        public static Offering FromString(this Offering flag, string value)
        {
            if (value.StartsWith("Fall"))
                flag |= Offering.FALL;
            else if (value.StartsWith("Winter"))
                flag |= Offering.WINTER;
            else if (value.StartsWith("Spring"))
                flag |= Offering.SPRING;
            else if (value.StartsWith("Summer"))
                flag |= Offering.SUMMER;
            return flag;
        }

        public static IEnumerable<Offering> ActiveFlags(this Offering flag)
        {
            return Enum.GetValues(typeof(Offering))
                    .Cast<Offering>()
                    .Where(o =>
                        o != Offering.NONE && o != Offering.ALL
                        && flag.HasFlag(o));
        }

        public static Offering FirstSemester(this Offering flag)
        {
            if(flag.HasFlag(Offering.FALL))
                return Offering.FALL;
            if(flag.HasFlag(Offering.WINTER))
                return Offering.WINTER;
            if(flag.HasFlag(Offering.SPRING))
                return Offering.SPRING;
            if(flag.HasFlag(Offering.SUMMER))
                return Offering.SUMMER;
            return Offering.NONE;
        }
    }
}
