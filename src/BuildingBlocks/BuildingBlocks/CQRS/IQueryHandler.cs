﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace BuildingBlocks.CQRS
{
    public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
        where TResponse : notnull
    {
        Task<TResponse> Handle(TQuery query);
    }
}
