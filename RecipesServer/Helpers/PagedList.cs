﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesServer.Helpers
{
	public class PagedList<T> : List<T>
	{
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int PageSize { get; set; }
		public int TotalCount { get; set; }

		public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
		{
			CurrentPage = pageNumber;
			//podeli koliko ima stvarima sa page sizom koji je 10 npr
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);
			PageSize = pageSize;
			TotalCount = count;
			AddRange(items);
		}

		public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber,
			int pageSize)
		{
			var count = await source.CountAsync();
			//gleda na mojoj smo stranici 2 oduzima prvu stranu *5  sto znaci preskace prvih 5 i uzima sled 5
			var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
			return new PagedList<T>(items, count, pageNumber, pageSize);
		}
	}
}
