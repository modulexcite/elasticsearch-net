﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Elasticsearch.Net;

namespace Nest
{
	public partial class ElasticClient
	{
		public ICatResponse<CatSegmentsRecord> CatSegments(Func<CatSegmentsDescriptor, CatSegmentsDescriptor> selector = null)
		{
			return this.DoCat<CatSegmentsDescriptor, CatSegmentsRequestParameters, CatSegmentsRecord>(selector, this.RawDispatch.CatSegmentsDispatch<CatResponse<CatSegmentsRecord>>);
		}

		public ICatResponse<CatSegmentsRecord> CatSegments(ICatSegmentsRequest request)
		{
			return this.DoCat<ICatSegmentsRequest, CatSegmentsRequestParameters, CatSegmentsRecord>(request, this.RawDispatch.CatSegmentsDispatch<CatResponse<CatSegmentsRecord>>);
		}

		public Task<ICatResponse<CatSegmentsRecord>> CatSegmentsAsync(Func<CatSegmentsDescriptor, CatSegmentsDescriptor> selector = null)
		{
			return this.DoCatAsync<CatSegmentsDescriptor, CatSegmentsRequestParameters, CatSegmentsRecord>(selector, this.RawDispatch.CatSegmentsDispatchAsync<CatResponse<CatSegmentsRecord>>);
		}

		public Task<ICatResponse<CatSegmentsRecord>> CatSegmentsAsync(ICatSegmentsRequest request)
		{
			return this.DoCatAsync<ICatSegmentsRequest, CatSegmentsRequestParameters, CatSegmentsRecord>(request, this.RawDispatch.CatSegmentsDispatchAsync<CatResponse<CatSegmentsRecord>>);
		}

	}
}