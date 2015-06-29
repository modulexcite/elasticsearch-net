﻿using System;
using System.Collections.Generic;
using System.Linq;
using Nest.Resolvers.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Linq.Expressions;

namespace Nest
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	[JsonConverter(typeof(ReadAsTypeConverter<MatchQueryDescriptor<object>>))]
	public interface IMatchQuery : IFieldNameQuery 
	{
		[JsonProperty(PropertyName = "type")]
		string Type { get; }

		[JsonProperty(PropertyName = "query")]
		string Query { get; set; }

		[JsonProperty(PropertyName = "analyzer")]
		string Analyzer { get; set; }

		[JsonProperty(PropertyName = "rewrite")]
		[JsonConverter(typeof (StringEnumConverter))]
		RewriteMultiTerm? Rewrite { get; set; }

		[JsonProperty(PropertyName = "fuzziness")]
		IFuzziness Fuzziness { get; set; }

		[JsonProperty(PropertyName = "fuzzy_transpositions")]
		bool? FuzzyTranspositions { get; set; }

		[JsonProperty(PropertyName = "cutoff_frequency")]
		double? CutoffFrequency { get; set; }

		[JsonProperty(PropertyName = "prefix_length")]
		int? PrefixLength { get; set; }

		[JsonProperty(PropertyName = "max_expansions")]
		int? MaxExpansions { get; set; }

		[JsonProperty(PropertyName = "slop")]
		int? Slop { get; set; }

		[JsonProperty(PropertyName = "boost")]
		double? Boost { get; set; }

		[JsonProperty(PropertyName = "lenient")]
		bool? Lenient { get; set; }
		
		[JsonProperty("minimum_should_match")]
		string MinimumShouldMatch { get; set; }

		[JsonProperty(PropertyName = "operator")]
		[JsonConverter(typeof (StringEnumConverter))]
		Operator? Operator { get; set; }
	}
	
	public class MatchQuery : FieldNameQuery, IMatchQuery
	{
		bool IQuery.Conditionless => IsConditionless(this);
		public string Type { get; set; }
		public string Query { get; set; }
		public string Analyzer { get; set; }
		public RewriteMultiTerm? Rewrite { get; set; }
		public IFuzziness Fuzziness { get; set; }
		public bool? FuzzyTranspositions { get; set; }
		public double? CutoffFrequency { get; set; }
		public int? PrefixLength { get; set; }
		public int? MaxExpansions { get; set; }
		public int? Slop { get; set; }
		public double? Boost { get; set; }
		public bool? Lenient { get; set; }
		public string MinimumShouldMatch { get; set; }
		public Operator? Operator { get; set; }

		protected override void WrapInContainer(IQueryContainer c) => c.Match = this;

		internal static bool IsConditionless(IMatchQuery q) => q.Field.IsConditionless() || q.Query.IsNullOrEmpty();
	}

	[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
	public class MatchQueryDescriptor<T> : IMatchQuery where T : class
	{
		protected virtual string MatchQueryType { get { return null; } }
		private IMatchQuery Self => this;
		string IQuery.Name { get; set; }
		bool IQuery.Conditionless => MatchQuery.IsConditionless(this);
		string IMatchQuery.Type { get { return MatchQueryType; } }
		string IMatchQuery.Query { get; set; }
		string IMatchQuery.Analyzer { get; set; }
		string IMatchQuery.MinimumShouldMatch { get; set; }
		RewriteMultiTerm? IMatchQuery.Rewrite { get; set; }
		IFuzziness IMatchQuery.Fuzziness { get; set; }
		bool? IMatchQuery.FuzzyTranspositions { get; set; }
		double? IMatchQuery.CutoffFrequency { get; set; }
		int? IMatchQuery.PrefixLength { get; set; }
		int? IMatchQuery.MaxExpansions { get; set; }
		int? IMatchQuery.Slop { get; set; }
		double? IMatchQuery.Boost { get; set; }
		bool? IMatchQuery.Lenient { get; set; }
		Operator? IMatchQuery.Operator { get; set; }
		PropertyPathMarker IFieldNameQuery.Field { get; set; }

		public MatchQueryDescriptor<T> Name(string name)
		{
			Self.Name = name;
			return this;
		}

		public MatchQueryDescriptor<T> OnField(string field)
		{
			Self.Field = field;
			return this;
		}

		public MatchQueryDescriptor<T> OnField(Expression<Func<T, object>> objectPath)
		{
			Self.Field = objectPath;
			return this;
		}

		public MatchQueryDescriptor<T> Query(string query)
		{
			Self.Query = query;
			return this;
		}
	
		public MatchQueryDescriptor<T> Lenient(bool lenient = true)
		{
			Self.Lenient = lenient;
			return this;
		}
		
		public MatchQueryDescriptor<T> Analyzer(string analyzer)
		{
			Self.Analyzer = analyzer;
			return this;
		}

		public MatchQueryDescriptor<T> Fuzziness(double ratio)
		{
			Self.Fuzziness = Nest.Fuzziness.Ratio(ratio);
			return this;
		}

		public MatchQueryDescriptor<T> Fuzziness()
		{
			Self.Fuzziness = Nest.Fuzziness.Auto;
			return this;
		}

		public MatchQueryDescriptor<T> Fuzziness(int editDistance)
		{
			Self.Fuzziness = Nest.Fuzziness.EditDistance(editDistance);
			return this;
		}

		public MatchQueryDescriptor<T> FuzzyTranspositions(bool fuzzyTranspositions = true)
		{
			Self.FuzzyTranspositions = fuzzyTranspositions;
			return this;
		}

		public MatchQueryDescriptor<T> CutoffFrequency(double cutoffFrequency)
		{
			Self.CutoffFrequency = cutoffFrequency;
			return this;
		}

		public MatchQueryDescriptor<T> Rewrite(RewriteMultiTerm rewrite)
		{
			Self.Rewrite = rewrite;
			return this;
		}

		public MatchQueryDescriptor<T> Boost(double boost)
		{
			Self.Boost = boost;
			return this;
		}
		
		public MatchQueryDescriptor<T> PrefixLength(int prefixLength)
		{
			Self.PrefixLength = prefixLength;
			return this;
		}
		
		public MatchQueryDescriptor<T> MaxExpansions(int maxExpansions)
		{
			Self.MaxExpansions = maxExpansions;
			return this;
		}
		
		public MatchQueryDescriptor<T> Slop(int slop)
		{
			Self.Slop = slop;
			return this;
		}
		
		public MatchQueryDescriptor<T> MinimumShouldMatch(string minimumShouldMatch)
		{
			Self.MinimumShouldMatch = minimumShouldMatch;
			return this;
		}
	
		public MatchQueryDescriptor<T> Operator(Operator op)
		{
			Self.Operator = op;
			return this;
		}
	}
}