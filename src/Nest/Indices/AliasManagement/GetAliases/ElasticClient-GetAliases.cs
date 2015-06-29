﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;

namespace Nest
{
	using GetAliasesConverter = Func<IElasticsearchResponse, Stream, GetAliasesResponse>;
	using CrazyAliasesResponse = Dictionary<string, Dictionary<string, Dictionary<string, AliasDefinition>>>;

	public partial class ElasticClient
	{
	
		/// <inheritdoc />
		public IGetAliasesResponse GetAliases(Func<GetAliasesDescriptor, GetAliasesDescriptor> getAliasesDescriptor)
		{
			return this.Dispatcher.Dispatch<GetAliasesDescriptor, GetAliasesRequestParameters, GetAliasesResponse>(
				getAliasesDescriptor,
				(p, d) => this.RawDispatch.IndicesGetAliasesDispatch<GetAliasesResponse>(
					p.DeserializationState(new GetAliasesConverter(DeserializeGetAliasesResponse))
				)
			);
		}

		/// <inheritdoc />
		public IGetAliasesResponse GetAliases(IGetAliasesRequest getAliasesRequest)
		{
			return this.Dispatcher.Dispatch<IGetAliasesRequest, GetAliasesRequestParameters, GetAliasesResponse>(
				getAliasesRequest,
				(p, d) => this.RawDispatch.IndicesGetAliasesDispatch<GetAliasesResponse>(
					p.DeserializationState(new GetAliasesConverter(DeserializeGetAliasesResponse))
				)
			);
		}

		/// <inheritdoc />
		public Task<IGetAliasesResponse> GetAliasesAsync(Func<GetAliasesDescriptor, GetAliasesDescriptor> getAliasesDescriptor)
		{
			return this.Dispatcher.DispatchAsync<GetAliasesDescriptor, GetAliasesRequestParameters, GetAliasesResponse, IGetAliasesResponse>(
				getAliasesDescriptor,
				(p, d) => this.RawDispatch.IndicesGetAliasesDispatchAsync<GetAliasesResponse>(
					p.DeserializationState(new GetAliasesConverter(DeserializeGetAliasesResponse))
				)
			);
		}

		/// <inheritdoc />
		public Task<IGetAliasesResponse> GetAliasesAsync(IGetAliasesRequest getAliasesRequest)
		{
			return this.Dispatcher.DispatchAsync<IGetAliasesRequest, GetAliasesRequestParameters, GetAliasesResponse, IGetAliasesResponse>(
				getAliasesRequest,
				(p, d) => this.RawDispatch.IndicesGetAliasesDispatchAsync<GetAliasesResponse>(
					p.DeserializationState(new GetAliasesConverter(DeserializeGetAliasesResponse))
				)
			);
		}

		/// <inheritdoc />
		private GetAliasesResponse DeserializeGetAliasesResponse(IElasticsearchResponse connectionStatus, Stream stream)
		{
			if (!connectionStatus.Success)
				return new GetAliasesResponse() { IsValid = false };

			var dict = this.Serializer.Deserialize<CrazyAliasesResponse>(stream);

			var d = new Dictionary<string, IList<AliasDefinition>>();

			foreach (var kv in dict)
			{
				var indexDict = kv.Key;
				var aliases = new List<AliasDefinition>();
				if (kv.Value != null && kv.Value.ContainsKey("aliases"))
				{
					var aliasDict = kv.Value["aliases"];
					if (aliasDict != null)
						aliases = aliasDict.Select(kva =>
						{
							var alias = kva.Value;
							alias.Name = kva.Key;
							return alias;
						}).ToList();
				}

				d.Add(indexDict, aliases);
			}

			return new GetAliasesResponse()
			{
				IsValid = true,
				Indices = d
			};
		}
	}
}