using Sdk.Application.Common.Interfaces;
using System;

namespace Sdk.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
