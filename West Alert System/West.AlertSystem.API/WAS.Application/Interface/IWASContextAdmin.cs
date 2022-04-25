using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Threading;
using System.Threading.Tasks;
using WAS.Domain.Entities;

namespace WAS.Application.Interface
{
    public interface IWasContextAdmin : IWasContext
    {
        
    }
}
