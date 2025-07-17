using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieServiceContracts
{
    public interface IActorService
    {
        Task<bool> AddActorToMovieAsync(int movieId, int actorId);
    }
}
