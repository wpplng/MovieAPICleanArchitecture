using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieServiceContracts
{
    public interface IServiceManager
    {
        IMovieService Movies { get; }
        IActorService Actors { get; }
        IReviewService Reviews { get; }
    }
}
