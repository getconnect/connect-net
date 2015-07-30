using System;
using System.Collections.Generic;
using System.Linq;
using ConnectSdk.Querying;
using static ConnectSdk.Querying.Aggregations;
using static ConnectSdk.Querying.Filters;
using static ConnectSdk.Querying.TimeType;
using Xunit;

namespace ConnectSdk.Tests.Querying
{
    public class QueryTests
    {
        private static IQuery<dynamic> baseQuery = new Query<dynamic>("Dummy", null);  

        public class WhenAggregating
        {
            [Fact]
            public void It_should_append_aggregates()
            {
                var query = baseQuery
                    .Select(new
                    {
                        MyProp = Sum("prop")
                    })
                    .Select(new
                    {
                        OtherProp = Sum("prop")
                    }).ToString();

                Assert.Equal("{\"select\":{\"MyProp\":{\"sum\":\"prop\"},\"OtherProp\":{\"sum\":\"prop\"}}}", query);
            }

            [Fact]
            public void It_should_add_sum_property()
            {
                var query = baseQuery
                    .Select(new
                    {
                        MyProp = Sum("prop")
                    }).ToString();

                Assert.Equal("{\"select\":{\"MyProp\":{\"sum\":\"prop\"}}}", query);
            }

            [Fact]
            public void It_should_add_min_property()
            {
                var query = baseQuery
                    .Select(new
                    {
                        MyProp = Min("prop")
                    }).ToString();

                Assert.Equal("{\"select\":{\"MyProp\":{\"min\":\"prop\"}}}", query);
            }

            [Fact]
            public void It_should_add_max_property()
            {
                var query = baseQuery
                    .Select(new
                    {
                        MyProp = Max("prop")
                    }).ToString();

                Assert.Equal("{\"select\":{\"MyProp\":{\"max\":\"prop\"}}}", query);
            }

            [Fact]
            public void It_should_add_avg_property()
            {
                var query = baseQuery
                    .Select(new
                    {
                        MyProp = Avg("prop")
                    }).ToString();

                Assert.Equal("{\"select\":{\"MyProp\":{\"avg\":\"prop\"}}}", query);
            }

            [Fact]
            public void It_should_add_count()
            {
                var query = baseQuery
                    .Select(new
                    {
                        MyProp = Count()
                    }).ToString();

                Assert.Equal("{\"select\":{\"MyProp\":\"count\"}}", query);
            }
        }

        public class WhenFiltering
        {
            [Fact]
            public void It_should_add_the_filter_from_object_literal()
            {
                var query = baseQuery
                    .Where(new
                    {
                        MyProp = Gt(5)
                    }).ToString();

                Assert.Equal("{\"filter\":{\"MyProp\":{\"gt\":5}}}", query);
            }

            [Fact]
            public void It_should_add_a_value_as_eq_filter()
            {
                var query = baseQuery
                    .Where("My.Prop", 5).ToString();

                Assert.Equal("{\"filter\":{\"My.Prop\":{\"eq\":5}}}", query);
            }

            [Fact]
            public void It_should_add_a_array_of_values_as_in_filter()
            {
                var query = baseQuery
                    .Where("My.Prop", new []{ 5, 6 }).ToString();

                Assert.Equal("{\"filter\":{\"My.Prop\":{\"in\":[5,6]}}}", query);
            }

            [Fact]
            public void It_should_add_a_single_filter()
            {
                var query = baseQuery
                    .Where("My.Prop", Gt(5)).ToString();

                Assert.Equal("{\"filter\":{\"My.Prop\":{\"gt\":5}}}", query);
            }

            [Fact]
            public void It_should_add_dictionary_of_filters()
            {
                var query = baseQuery
                    .Where(new Dictionary<string, Filter>
                    {
                        ["My.Prop"] = Gt(5)
                    }).ToString();

                Assert.Equal("{\"filter\":{\"My.Prop\":{\"gt\":5}}}", query);
            }

            [Fact]
            public void It_should_add_dictionary_of_array_of_filters()
            {
                var query = baseQuery
                    .Where(new Dictionary<string, Filter[]>
                    {
                        ["My.Prop"] = new[] { Gt(5), Lt(5) }
                    }).ToString();

                Assert.Equal("{\"filter\":{\"My.Prop\":{\"gt\":5,\"lt\":5}}}", query);
            }

            [Fact]
            public void It_should_add_dictionary_of_enumerable_of_filters()
            {
                var query = baseQuery
                    .Where(new Dictionary<string, IEnumerable<Filter>>
                    {
                        ["My.Prop"] = new[] { Gt(5), Lt(5) }.AsEnumerable()
                    }).ToString();

                Assert.Equal("{\"filter\":{\"My.Prop\":{\"gt\":5,\"lt\":5}}}", query);
            }

            [Fact]
            public void It_should_merge_filters()
            {
                var query = baseQuery
                    .Where(new
                    {
                        MyProp = Gt(5)
                    })
                    .Where(new
                    {
                        MyProp = Lt(5),
                        OtherProp = Gt(5)
                    }).ToString();

                Assert.Equal("{\"filter\":{\"MyProp\":{\"lt\":5,\"gt\":5},\"OtherProp\":{\"gt\":5}}}", query);
            }

            [Fact]
            public void It_should_multiple_filters_for_property()
            {
                var query = baseQuery
                    .Where(new
                    {
                        MyProp = new[] { Gt(5), Lt(5) }
                    }).ToString();

                Assert.Equal("{\"filter\":{\"MyProp\":{\"gt\":5,\"lt\":5}}}", query);
            }

            [Fact]
            public void It_should_take_latest_filter()
            {
                var query = baseQuery
                    .Where(new
                    {
                        MyProp = Gt(5)
                    })
                    .Where(new
                    {
                        MyProp = Gt(1)
                    }).ToString();

                Assert.Equal("{\"filter\":{\"MyProp\":{\"gt\":1}}}", query);
            }

            [Fact]
            public void It_should_add_a_gte_filter()
            {
                var query = baseQuery
                    .Where("My.Prop", Gte(5)).ToString();

                Assert.Equal("{\"filter\":{\"My.Prop\":{\"gte\":5}}}", query);
            }

            [Fact]
            public void It_should_add_a_lt_filter()
            {
                var query = baseQuery
                    .Where("My.Prop", Lt(5)).ToString();

                Assert.Equal("{\"filter\":{\"My.Prop\":{\"lt\":5}}}", query);
            }

            [Fact]
            public void It_should_add_a_lte_filter()
            {
                var query = baseQuery
                    .Where("My.Prop", Lte(5)).ToString();

                Assert.Equal("{\"filter\":{\"My.Prop\":{\"lte\":5}}}", query);
            }

            [Fact]
            public void It_should_add_a_eq_filter()
            {
                var query = baseQuery
                    .Where("My.Prop", Eq(5)).ToString();

                Assert.Equal("{\"filter\":{\"My.Prop\":{\"eq\":5}}}", query);
            }

            [Fact]
            public void It_should_add_a_neq_filter()
            {
                var query = baseQuery
                    .Where("My.Prop", Neq(5)).ToString();

                Assert.Equal("{\"filter\":{\"My.Prop\":{\"neq\":5}}}", query);
            }

            [Fact]
            public void It_should_add_a_contains_filter()
            {
                var query = baseQuery
                    .Where("My.Prop", Contains("5")).ToString();

                Assert.Equal("{\"filter\":{\"My.Prop\":{\"contains\":\"5\"}}}", query);
            }

            [Fact]
            public void It_should_add_a_starts_with_filter()
            {
                var query = baseQuery
                    .Where("My.Prop", StartsWith("5")).ToString();

                Assert.Equal("{\"filter\":{\"My.Prop\":{\"startsWith\":\"5\"}}}", query);
            }

            [Fact]
            public void It_should_add_a_ends_with_filter()
            {
                var query = baseQuery
                    .Where("My.Prop", EndsWith("5")).ToString();

                Assert.Equal("{\"filter\":{\"My.Prop\":{\"endsWith\":\"5\"}}}", query);
            }

            [Fact]
            public void It_should_add_a_exists_filter()
            {
                var query = baseQuery
                    .Where("My.Prop", Exists(true)).ToString();

                Assert.Equal("{\"filter\":{\"My.Prop\":{\"exists\":true}}}", query);
            }
        }

        public class WhenSpecifyingAbsoluteTimeframe
        {
            [Fact]
            public void It_should_add_start_and_end_date()
            {
                var july = new DateTime(2015, 07, 01, 10, 0, 0, DateTimeKind.Local);

                var query = baseQuery
                    .Between(july, july)
                    .ToString();

                Assert.Equal("{\"timeframe\":{\"start\":\"2015-07-01T00:00:00Z\",\"end\":\"2015-07-01T00:00:00Z\"}}", query);
            }

            [Fact]
            public void It_should_add_start_date_only()
            {
                var july = new DateTime(2015, 07, 01, 10, 0, 0, DateTimeKind.Local);

                var query = baseQuery
                    .StartingAt(july)
                    .ToString();

                Assert.Equal("{\"timeframe\":{\"start\":\"2015-07-01T00:00:00Z\"}}", query);
            }

            [Fact]
            public void It_should_add_end_date_only()
            {
                var july = new DateTime(2015, 07, 01, 10, 0, 0, DateTimeKind.Local);

                var query = baseQuery
                    .EndingAt(july)
                    .ToString();

                Assert.Equal("{\"timeframe\":{\"end\":\"2015-07-01T00:00:00Z\"}}", query);
            }
        }

        public class WhenSpecifyingPeriodRelativeTimeframe
        {
            [Fact]
            public void It_should_add_previous_timeframe()
            {
                var query = baseQuery
                    .Previous(5, Days)
                    .ToString();

                Assert.Equal("{\"timeframe\":{\"previous\":{\"days\":5}}}", query);
            }

            [Fact]
            public void It_should_add_current_timeframe()
            {
                var query = baseQuery
                    .Current(5, Days)
                    .ToString();

                Assert.Equal("{\"timeframe\":{\"current\":{\"days\":5}}}", query);
            }
        }

        public class WhenSpecifyingIntreval
        {
            [Fact]
            public void It_should_add_minutely_interval()
            {
                var query = baseQuery
                    .Minutely()
                    .ToString();

                Assert.Equal("{\"interval\":\"minutely\"}", query);
            }

            [Fact]
            public void It_should_add_hourly_interval()
            {
                var query = baseQuery
                    .Hourly()
                    .ToString();

                Assert.Equal("{\"interval\":\"hourly\"}", query);
            }

            [Fact]
            public void It_should_add_weekly_interval()
            {
                var query = baseQuery
                    .Weekly()
                    .ToString();

                Assert.Equal("{\"interval\":\"weekly\"}", query);
            }

            [Fact]
            public void It_should_add_monthly_interval()
            {
                var query = baseQuery
                    .Monthly()
                    .ToString();

                Assert.Equal("{\"interval\":\"monthly\"}", query);
            }

            [Fact]
            public void It_should_add_quarterly_interval()
            {
                var query = baseQuery
                    .Quarterly()
                    .ToString();

                Assert.Equal("{\"interval\":\"quarterly\"}", query);
            }

            [Fact]
            public void It_should_add_yearly_interval()
            {
                var query = baseQuery
                    .Yearly()
                    .ToString();

                Assert.Equal("{\"interval\":\"yearly\"}", query);
            }
        }

        public class WhenSpecifyingRelativeTimeframe
        {
            [Fact]
            public void It_should_add_this_minute()
            {
                var query = baseQuery
                    .ThisMinute()
                    .ToString();

                Assert.Equal("{\"timeframe\":\"this_minute\"}", query);
            }

            [Fact]
            public void It_should_add_this_last_minute()
            {
                var query = baseQuery
                    .LastMinute()
                    .ToString();

                Assert.Equal("{\"timeframe\":\"last_minute\"}", query);
            }

            [Fact]
            public void It_should_add_this_hour()
            {
                var query = baseQuery
                    .ThisHour()
                    .ToString();

                Assert.Equal("{\"timeframe\":\"this_hour\"}", query);
            }

            [Fact]
            public void It_should_add_last_hour()
            {
                var query = baseQuery
                    .LastHour()
                    .ToString();

                Assert.Equal("{\"timeframe\":\"last_hour\"}", query);
            }

            [Fact]
            public void It_should_add_today()
            {
                var query = baseQuery
                    .Today()
                    .ToString();

                Assert.Equal("{\"timeframe\":\"today\"}", query);
            }

            [Fact]
            public void It_should_add_yesterday()
            {
                var query = baseQuery
                    .Yesterday()
                    .ToString();

                Assert.Equal("{\"timeframe\":\"yesterday\"}", query);
            }

            [Fact]
            public void It_should_add_this_week()
            {
                var query = baseQuery
                    .ThisWeek()
                    .ToString();

                Assert.Equal("{\"timeframe\":\"this_week\"}", query);
            }

            [Fact]
            public void It_should_add_last_week()
            {
                var query = baseQuery
                    .LastWeek()
                    .ToString();

                Assert.Equal("{\"timeframe\":\"last_week\"}", query);
            }

            [Fact]
            public void It_should_add_this_month()
            {
                var query = baseQuery
                    .ThisMonth()
                    .ToString();

                Assert.Equal("{\"timeframe\":\"this_month\"}", query);
            }

            [Fact]
            public void It_should_add_last_month()
            {
                var query = baseQuery
                    .LastMonth()
                    .ToString();

                Assert.Equal("{\"timeframe\":\"last_month\"}", query);
            }

            [Fact]
            public void It_should_add_this_quarter()
            {
                var query = baseQuery
                    .ThisQuarter()
                    .ToString();

                Assert.Equal("{\"timeframe\":\"this_quarter\"}", query);
            }

            [Fact]
            public void It_should_add_last_quarter()
            {
                var query = baseQuery
                    .LastQuarter()
                    .ToString();

                Assert.Equal("{\"timeframe\":\"last_quarter\"}", query);
            }

            [Fact]
            public void It_should_add_this_year()
            {
                var query = baseQuery
                    .ThisYear()
                    .ToString();

                Assert.Equal("{\"timeframe\":\"this_year\"}", query);
            }

            [Fact]
            public void It_should_add_last_year()
            {
                var query = baseQuery
                    .LastYear()
                    .ToString();

                Assert.Equal("{\"timeframe\":\"last_year\"}", query);
            }
        }

        public class WhenSpecifyingTimezoneOffset
        {
            [Fact]
            public void It_should_add_as_timezone()
            {
                var query = baseQuery
                    .Timezone(10.5m)
                    .ToString();

                Assert.Equal("{\"timezone\":10.5}", query);
            }
        }

        public class WhenSpecifyingTimezone
        {
            [Fact]
            public void It_should_add_as_timezone()
            {
                var query = baseQuery
                    .Timezone("Japan/Tokyo")
                    .ToString();

                Assert.Equal("{\"timezone\":\"Japan/Tokyo\"}", query);
            }
        }

        public class WhenAddingGroupBy
        {
            [Fact]
            public void It_should_add_single_group_by()
            {
                var query = baseQuery
                    .GroupBy("G1")
                    .ToString();

                Assert.Equal("{\"groupBy\":[\"G1\"]}", query);
            }

            [Fact]
            public void It_should_add_multiple_group_bys()
            {
                var query = baseQuery
                    .GroupBy("G1", "G2")
                    .ToString();

                Assert.Equal("{\"groupBy\":[\"G1\",\"G2\"]}", query);
            }

            [Fact]
            public void It_should_add_group_by_array()
            {
                var groupArray = new[] {"G1", "G2"};
                var query = baseQuery
                    .GroupBy(groupArray)
                    .ToString();

                Assert.Equal("{\"groupBy\":[\"G1\",\"G2\"]}", query);
            }

            [Fact]
            public void It_should_filter_out_duplicates()
            {
                var query = baseQuery
                    .GroupBy("G1", "G2")
                    .GroupBy("G1", "G3")
                    .ToString();

                Assert.Equal("{\"groupBy\":[\"G1\",\"G2\",\"G3\"]}", query);
            }
        }
    }
}