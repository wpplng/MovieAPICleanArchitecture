using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Data;
using MovieAPI.Models.Entities;

namespace MovieAPI.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly MovieContext context;

        public ReviewsController(MovieContext context)
        {
            this.context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReview()
        {
            return await context.Reviews.ToListAsync();
        }

        //[HttpPost]
        //public async Task<ActionResult<Review>> PostReview(Review review)
        //{
        //    context.Reviews.Add(review);
        //    await context.SaveChangesAsync();

        //    return CreatedAtAction("GetReview", new { id = review.Id }, review);
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteReview(int id)
        //{
        //    var review = await context.Reviews.FindAsync(id);
        //    if (review == null)
        //    {
        //        return NotFound();
        //    }

        //    context.Reviews.Remove(review);
        //    await context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}
