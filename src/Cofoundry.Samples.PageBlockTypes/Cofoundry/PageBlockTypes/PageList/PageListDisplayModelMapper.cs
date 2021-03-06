﻿using Cofoundry.Core;
using Cofoundry.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cofoundry.Samples.PageBlockTypes
{
    public class PageListDisplayModelMapper : IPageBlockTypeDisplayModelMapper<PageListDataModel>
    {
        private readonly IPageRepository _pageRepository;

        public PageListDisplayModelMapper(
            IPageRepository pageRepository
            )
        {
            _pageRepository = pageRepository;
        }

        public async Task<IEnumerable<PageBlockTypeDisplayModelMapperOutput>> MapAsync(
            IReadOnlyCollection<PageBlockTypeDisplayModelMapperInput<PageListDataModel>> inputCollection,
            PublishStatusQuery publishStatusQuery
            )
        {
            var allPageIds = inputCollection.SelectManyDistinctModelValues(m => m.PageIds);

            // Page routes are cached and so are the quickest way to get simple page information.
            // If we needed more data we could use a different but slower query to get it.
            var allPageRoutes = await _pageRepository.GetPageRoutesByIdRangeAsync(allPageIds);

            var results = new List<PageBlockTypeDisplayModelMapperOutput>(inputCollection.Count);

            foreach (var input in inputCollection)
            {
                var output = new PageListDisplayModel();

                // Here will get the relevant pages and order them correctly. 
                // Additionally if we are viewing the published version of the page
                // then we make sure we only show published pages in the list.

                output.Pages = allPageRoutes
                    .FilterAndOrderByKeys(input.DataModel.PageIds)
                    .Where(p => publishStatusQuery != PublishStatusQuery.Published || p.IsPublished())
                    .ToList();

                // The CreateOutput() method wraps the mapped display 
                // model with it's identifier so we can identify later on
                results.Add(input.CreateOutput(output));
            }

            return results;
        }
    }
}